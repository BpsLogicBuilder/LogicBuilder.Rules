namespace LogicBuilder.Workflow.ComponentModel.Serialization
{
    using System;
    using System.CodeDom;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.ComponentModel.Design.Serialization;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Reflection;
    using LogicBuilder.Workflow.ComponentModel.Design;
    using System.Xml;

    #region Class WorkflowMarkupSerializationManager
    public class WorkflowMarkupSerializationManager : IDesignerSerializationManager
    {
        private Assembly localAssembly = null;
        private int writerDepth = 0;
        private ContextStack workflowMarkupStack = new ContextStack();
        // Stack to keep a list of objects being serialized, to avoid stack overflow
        private Stack serializationStack = new Stack();
        private IDesignerSerializationManager serializationManager;
        private bool designMode = false;
        internal event EventHandler<WorkflowMarkupElementEventArgs> FoundDefTag;

        //These are temporary variables for speedy lookup
        private Dictionary<int, WorkflowMarkupSerializerMapping> clrNamespaceBasedMappings = new Dictionary<int, WorkflowMarkupSerializerMapping>();
        private Dictionary<string, List<WorkflowMarkupSerializerMapping>> xmlNamespaceBasedMappings = new Dictionary<string, List<WorkflowMarkupSerializerMapping>>();
        private Dictionary<string, List<WorkflowMarkupSerializerMapping>> prefixBasedMappings = new Dictionary<string, List<WorkflowMarkupSerializerMapping>>();
        private List<WorkflowMarkupSerializer> extendedPropertiesProviders;
        private Dictionary<XmlQualifiedName, Type> cachedXmlQualifiedNameTypes = new Dictionary<XmlQualifiedName, Type>();

        public WorkflowMarkupSerializationManager(IDesignerSerializationManager manager)
        {
            this.serializationManager = manager ?? throw new ArgumentNullException("manager");
            AddSerializationProvider(new WellKnownTypeSerializationProvider());

            // push standard mappings
            AddMappings(WorkflowMarkupSerializerMapping.WellKnownMappings);


            this.designMode = (manager.GetService(typeof(ITypeResolutionService)) != null);
        }

        public ContextStack Context
        {
            get
            {
                return this.serializationManager.Context;
            }
        }

        internal Stack SerializationStack
        {
            get
            {
                return this.serializationStack;
            }
        }

        public void ReportError(object errorInformation)
        {
            if (errorInformation == null)
                throw new ArgumentNullException("errorInformation");

            this.serializationManager.ReportError(errorInformation);
        }

        protected internal IDesignerSerializationManager SerializationManager
        {
            get
            {
                return this.serializationManager;
            }

            set
            {
                this.serializationManager = value;
                this.serializationManager.AddSerializationProvider(new WellKnownTypeSerializationProvider());
            }
        }

        public void AddSerializationProvider(IDesignerSerializationProvider provider)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");

            this.serializationManager.AddSerializationProvider(provider);
        }

        public void RemoveSerializationProvider(IDesignerSerializationProvider provider)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");

            this.serializationManager.RemoveSerializationProvider(provider);
        }

        public Assembly LocalAssembly
        {
            get
            {
                return this.localAssembly;
            }
            set
            {
                this.localAssembly = value;
            }
        }

        #region Public Methods
        public virtual XmlQualifiedName GetXmlQualifiedName(Type type, out string prefix)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            string typeNamespace = type.Namespace ?? String.Empty;
            string assemblyName = (type.Assembly != null && type.Assembly != this.localAssembly) ? type.Assembly.FullName : String.Empty;

            int key = typeNamespace.GetHashCode() ^ assemblyName.GetHashCode();
            if (!this.clrNamespaceBasedMappings.TryGetValue(key, out WorkflowMarkupSerializerMapping mappingForType))
            {
                WorkflowMarkupSerializerMapping.GetMappingFromType(this, type, out mappingForType, out IList<WorkflowMarkupSerializerMapping> collectedMappings);
                AddMappings(new List<WorkflowMarkupSerializerMapping>(new WorkflowMarkupSerializerMapping[] { mappingForType }));
                AddMappings(collectedMappings);
            }

            string typeName = WorkflowMarkupSerializer.EnsureMarkupExtensionTypeName(type);

            //Make sure that while writting the workflow namespaces will always be the default
            prefix = (mappingForType.Prefix.Equals(StandardXomlKeys.WorkflowPrefix, StringComparison.Ordinal)) ? String.Empty : mappingForType.Prefix;
            return new XmlQualifiedName(typeName, mappingForType.XmlNamespace);
        }

        public virtual Type GetType(XmlQualifiedName xmlQualifiedName)
        {
            if (xmlQualifiedName == null)
                throw new ArgumentNullException("xmlQualifiedName");

            string xmlns = xmlQualifiedName.Namespace;
            string typeName = WorkflowMarkupSerializer.EnsureMarkupExtensionTypeName(xmlQualifiedName);


            // first check our cache 
            cachedXmlQualifiedNameTypes.TryGetValue(xmlQualifiedName, out Type resolvedType);

            if (resolvedType == null)
            {
                // lookup in well known types
                resolvedType = WorkflowMarkupSerializerMapping.ResolveWellKnownTypes(this, xmlns, typeName);
            }

            if (resolvedType == null)
            {
                //Lookup existing mapping
                if (!this.xmlNamespaceBasedMappings.TryGetValue(xmlns, out List<WorkflowMarkupSerializerMapping> xmlnsMappings))
                {
                    WorkflowMarkupSerializerMapping.GetMappingsFromXmlNamespace(this, xmlns, out IList<WorkflowMarkupSerializerMapping> matchingMappings, out IList<WorkflowMarkupSerializerMapping> collectedMappings);
                    AddMappings(matchingMappings);
                    AddMappings(collectedMappings);

                    xmlnsMappings = new List<WorkflowMarkupSerializerMapping>(matchingMappings);
                }

                foreach (WorkflowMarkupSerializerMapping xmlnsMapping in xmlnsMappings)
                {
                    string assemblyName = xmlnsMapping.AssemblyName;
                    string clrNamespace = xmlnsMapping.ClrNamespace;

                    // append dot net namespace name
                    string fullTypeName = xmlQualifiedName.Name;
                    if (clrNamespace.Length > 0)
                        fullTypeName = clrNamespace + "." + xmlQualifiedName.Name;

                    // Work around  for component model assembly
                    if (assemblyName.Equals(Assembly.GetExecutingAssembly().FullName, StringComparison.Ordinal))
                    {
                        resolvedType = Assembly.GetExecutingAssembly().GetType(fullTypeName);
                    }
                    else if (assemblyName.Length == 0)
                    {
                        if (this.localAssembly != null)
                            resolvedType = this.localAssembly.GetType(fullTypeName);
                    }
                    else
                    {
                        string assemblyQualifiedName = fullTypeName;
                        if (assemblyName.Length > 0)
                            assemblyQualifiedName += (", " + assemblyName);

                        // now grab the actual type
                        try
                        {
                            resolvedType = GetType(assemblyQualifiedName);
                        }
                        catch
                        {
                            // 



                        }

                        if (resolvedType == null)
                        {
                            resolvedType = GetType(fullTypeName);
                            if (resolvedType != null && !resolvedType.AssemblyQualifiedName.Equals(assemblyQualifiedName, StringComparison.Ordinal))
                                resolvedType = null;
                        }
                    }

                    //We found the type
                    if (resolvedType != null)
                    {
                        cachedXmlQualifiedNameTypes[xmlQualifiedName] = resolvedType;
                        break;
                    }
                }
            }

            return resolvedType;
        }
        #endregion

        #region WorkflowMarkupSerializationManager Overrides
        public object GetSerializer(Type objectType, Type serializerType)
        {
            return serializationManager.GetSerializer(objectType, serializerType);
        }


        [SuppressMessage("Microsoft.Globalization", "CA1307:SpecifyStringComparison", MessageId = "System.String.IndexOf(System.String)", Justification = "Not a security threat since it is called in design time scenarios")]
        public virtual Type GetType(string typeName)
        {
            if (typeName == null)
                throw new ArgumentNullException("typeName");

            // try serialization manager
            Type type = null;


            if (this.designMode)
            {
                try
                {
                    type = this.serializationManager.GetType(typeName);
                }
                catch
                {
                    //Debug.Assert(false, "VSIP framwork threw exception on resolving type." + e.ToString());
                }
            }


            if (type != null)
                return type;

            // try loading the assembly directly
            string assemblyName = string.Empty;
            int commaIndex = typeName.IndexOf(",");
            string fullyQualifiedTypeName = typeName;
            if (commaIndex > 0)
            {
                assemblyName = typeName.Substring(commaIndex + 1);
                typeName = typeName.Substring(0, commaIndex);
            }

            Assembly assembly = null;
            assemblyName = assemblyName.Trim();
            if (assemblyName.Length > 0)
            {
                if (assemblyName.IndexOf(',') >= 0)
                {
                    try
                    {
                        assembly = Assembly.Load(assemblyName);
                    }
                    catch
                    {
                        // 


                    }
                }

                typeName = typeName.Trim();
                if (assembly != null)
                    type = assembly.GetType(typeName, false);
                else
                    type = Type.GetType(fullyQualifiedTypeName, false);
            }
            return type;
        }
        #endregion

        #region Helpers
        internal int WriterDepth
        {
            get
            {
                return this.writerDepth;
            }
            set
            {
                this.writerDepth = value;
            }
        }

        internal ContextStack WorkflowMarkupStack
        {
            get
            {
                return this.workflowMarkupStack;
            }
        }

        internal void FireFoundDefTag(WorkflowMarkupElementEventArgs args)
        {
            this.FoundDefTag?.Invoke(this, args);
        }

        internal IDictionary<int, WorkflowMarkupSerializerMapping> ClrNamespaceBasedMappings
        {
            get
            {
                return this.clrNamespaceBasedMappings;
            }
        }

        internal IDictionary<string, List<WorkflowMarkupSerializerMapping>> XmlNamespaceBasedMappings
        {
            get
            {
                return this.xmlNamespaceBasedMappings;
            }
        }

        internal Dictionary<string, List<WorkflowMarkupSerializerMapping>> PrefixBasedMappings
        {
            get
            {
                return this.prefixBasedMappings;
            }
        }

        internal void AddMappings(IList<WorkflowMarkupSerializerMapping> mappingsToAdd)
        {
            foreach (WorkflowMarkupSerializerMapping mapping in mappingsToAdd)
            {
                if (!this.clrNamespaceBasedMappings.ContainsKey(mapping.GetHashCode()))
                    this.clrNamespaceBasedMappings.Add(mapping.GetHashCode(), mapping);

                if (!this.xmlNamespaceBasedMappings.TryGetValue(mapping.XmlNamespace, out List<WorkflowMarkupSerializerMapping> xmlnsMappings))
                {
                    xmlnsMappings = new List<WorkflowMarkupSerializerMapping>();
                    this.xmlNamespaceBasedMappings.Add(mapping.XmlNamespace, xmlnsMappings);
                }
                xmlnsMappings.Add(mapping);

                if (!this.prefixBasedMappings.TryGetValue(mapping.Prefix, out List<WorkflowMarkupSerializerMapping> prefixMappings))
                {
                    prefixMappings = new List<WorkflowMarkupSerializerMapping>();
                    this.prefixBasedMappings.Add(mapping.Prefix, prefixMappings);
                }
                prefixMappings.Add(mapping);
            }
        }

        internal IList<WorkflowMarkupSerializer> ExtendedPropertiesProviders
        {
            get
            {
                if (this.extendedPropertiesProviders == null)
                    this.extendedPropertiesProviders = new List<WorkflowMarkupSerializer>();
                return this.extendedPropertiesProviders;
            }
        }

        internal ExtendedPropertyInfo[] GetExtendedProperties(object extendee)
        {
            List<ExtendedPropertyInfo> extendedProperties = new List<ExtendedPropertyInfo>();
            foreach (WorkflowMarkupSerializer markupSerializer in ExtendedPropertiesProviders)
                extendedProperties.AddRange(markupSerializer.GetExtendedProperties(this, extendee));
            return extendedProperties.ToArray();
        }
        #endregion

        #region IDesignerSerializationManager Implementation
        object IDesignerSerializationManager.CreateInstance(Type type, ICollection arguments, string name, bool addToContainer)
        {
            return this.serializationManager.CreateInstance(type, arguments, name, addToContainer);
        }

        object IDesignerSerializationManager.GetInstance(string name)
        {
            return this.serializationManager.GetInstance(name);
        }

        string IDesignerSerializationManager.GetName(object value)
        {
            return this.serializationManager.GetName(value);
        }

        PropertyDescriptorCollection IDesignerSerializationManager.Properties
        {
            get { return this.serializationManager.Properties; }
        }

        event ResolveNameEventHandler IDesignerSerializationManager.ResolveName { add { } remove { } }

        event EventHandler IDesignerSerializationManager.SerializationComplete { add { } remove { } }

        void IDesignerSerializationManager.SetName(object instance, string name)
        {
            this.serializationManager.SetName(instance, name);
        }
        #endregion

        #region IServiceProvider Members

        public object GetService(Type serviceType)
        {
            if (serviceType == null)
                throw new ArgumentNullException("serviceType");

            return this.serializationManager.GetService(serviceType);
        }

        #endregion

        #region Class WellKnownTypeSerializationProvider
        private sealed class WellKnownTypeSerializationProvider : IDesignerSerializationProvider
        {
            #region IDesignerSerializationProvider Members
            object IDesignerSerializationProvider.GetSerializer(IDesignerSerializationManager manager, object currentSerializer, Type objectType, Type serializerType)
            {
                if (serializerType == typeof(WorkflowMarkupSerializer) && objectType != null)
                {
                    if (typeof(ICollection<string>).IsAssignableFrom(objectType) && objectType.IsAssignableFrom(typeof(List<string>)) && !typeof(Array).IsAssignableFrom(objectType))
                        return new StringCollectionMarkupSerializer();
                    else if (typeof(Color).IsAssignableFrom(objectType))
                        return new ColorMarkupSerializer();
                    else if (typeof(Size).IsAssignableFrom(objectType))
                        return new SizeMarkupSerializer();
                    else if (typeof(Point).IsAssignableFrom(objectType))
                        return new PointMarkupSerializer();
                    else if (objectType == typeof(CodeTypeReference))
                        return new CodeTypeReferenceSerializer();
                }

                return null;
            }
            #endregion
        }
        #endregion
    }
    #endregion
}
