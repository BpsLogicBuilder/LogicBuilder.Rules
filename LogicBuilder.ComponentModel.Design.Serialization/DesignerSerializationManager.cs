using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

[assembly: CLSCompliant(true)]
namespace LogicBuilder.ComponentModel.Design.Serialization
{
    /// <summary>Provides an implementation of the <see cref="T:System.ComponentModel.Design.Serialization.IDesignerSerializationManager" /> interface.</summary>
    public class DesignerSerializationManager : IDesignerSerializationManager, IServiceProvider
    {
        private sealed class SerializationSession : IDisposable
        {
            private DesignerSerializationManager serializationManager;

            internal SerializationSession(DesignerSerializationManager serializationManager)
            {
                this.serializationManager = serializationManager;
            }

            public void Dispose()
            {
                this.serializationManager.OnSessionDisposed(EventArgs.Empty);
            }
        }

        private sealed class ReferenceComparer : IEqualityComparer
        {
            bool IEqualityComparer.Equals(object x, object y)
            {
                return x == y;
            }

            int IEqualityComparer.GetHashCode(object x)
            {
                if (x != null)
                {
                    return x.GetHashCode();
                }
                return 0;
            }
        }

        private sealed class WrappedPropertyDescriptor : PropertyDescriptor
        {
            private object target;

            private PropertyDescriptor property;

            public override AttributeCollection Attributes
            {
                get
                {
                    return this.property.Attributes;
                }
            }

            public override Type ComponentType
            {
                get
                {
                    return this.property.ComponentType;
                }
            }

            public override bool IsReadOnly
            {
                get
                {
                    return this.property.IsReadOnly;
                }
            }

            public override Type PropertyType
            {
                get
                {
                    return this.property.PropertyType;
                }
            }

            internal WrappedPropertyDescriptor(PropertyDescriptor property, object target) : base(property.Name, null)
            {
                this.property = property;
                this.target = target;
            }

            public override bool CanResetValue(object component)
            {
                return this.property.CanResetValue(this.target);
            }

            public override object GetValue(object component)
            {
                return this.property.GetValue(this.target);
            }

            public override void ResetValue(object component)
            {
                this.property.ResetValue(this.target);
            }

            public override void SetValue(object component, object value)
            {
                this.property.SetValue(this.target, value);
            }

            public override bool ShouldSerializeValue(object component)
            {
                return this.property.ShouldSerializeValue(this.target);
            }
        }

        private IServiceProvider provider;

        private ITypeResolutionService typeResolver;

        private bool searchedTypeResolver;

        private bool recycleInstances;

        private bool validateRecycledTypes;

        private bool preserveNames;

        private IContainer container;

        private IDisposable session;

        private ResolveNameEventHandler resolveNameEventHandler;

        private EventHandler serializationCompleteEventHandler;

        private EventHandler sessionCreatedEventHandler;

        private EventHandler sessionDisposedEventHandler;

        private ArrayList designerSerializationProviders;

        private Hashtable defaultProviderTable;

        private Hashtable instancesByName;

        private Hashtable namesByInstance;

        private Hashtable serializers;

        private ArrayList errorList;

        private ContextStack contextStack;

        private PropertyDescriptorCollection properties;

        private object propertyProvider;

        /// <summary>Occurs when a session is created. </summary>
        public event EventHandler SessionCreated
        {
            add
            {
                this.sessionCreatedEventHandler = (EventHandler)Delegate.Combine(this.sessionCreatedEventHandler, value);
            }
            remove
            {
                this.sessionCreatedEventHandler = (EventHandler)Delegate.Remove(this.sessionCreatedEventHandler, value);
            }
        }

        /// <summary>Occurs when a session is disposed.</summary>
        public event EventHandler SessionDisposed
        {
            add
            {
                this.sessionDisposedEventHandler = (EventHandler)Delegate.Combine(this.sessionDisposedEventHandler, value);
            }
            remove
            {
                this.sessionDisposedEventHandler = (EventHandler)Delegate.Remove(this.sessionDisposedEventHandler, value);
            }
        }

        /// <summary>Occurs when <see cref="M:System.ComponentModel.Design.Serialization.DesignerSerializationManager.System#ComponentModel#Design#Serialization#IDesignerSerializationManager#GetName(System.Object)" /> cannot locate the specified name in the serialization manager's name table. </summary>
        /// <exception cref="T:System.InvalidOperationException">The serialization manager does not have an active serialization session.</exception>
        event ResolveNameEventHandler IDesignerSerializationManager.ResolveName
        {
            add
            {
                this.CheckSession();
                this.resolveNameEventHandler = (ResolveNameEventHandler)Delegate.Combine(this.resolveNameEventHandler, value);
            }
            remove
            {
                this.resolveNameEventHandler = (ResolveNameEventHandler)Delegate.Remove(this.resolveNameEventHandler, value);
            }
        }

        /// <summary>Occurs when serialization is complete.</summary>
        /// <exception cref="T:System.InvalidOperationException">The serialization manager does not have an active serialization session.</exception>
        event EventHandler IDesignerSerializationManager.SerializationComplete
        {
            add
            {
                this.CheckSession();
                this.serializationCompleteEventHandler = (EventHandler)Delegate.Combine(this.serializationCompleteEventHandler, value);
            }
            remove
            {
                this.serializationCompleteEventHandler = (EventHandler)Delegate.Remove(this.serializationCompleteEventHandler, value);
            }
        }

        /// <summary>Gets or sets to the container for this serialization manager.</summary>
        /// <returns>The <see cref="T:System.ComponentModel.IContainer" /> to which the serialization manager will add components.</returns>
        /// <exception cref="T:System.InvalidOperationException">The serialization manager has an active serialization session.</exception>
        public IContainer Container
        {
            get
            {
                if (this.container == null)
                {
                    if (this.GetService(typeof(IDesignerHost)) is IDesignerHost designerHost)
                    {
                        this.container = designerHost.Container;
                    }
                }
                return this.container;
            }
            set
            {
                this.CheckNoSession();
                this.container = value;
            }
        }

        /// <summary>Gets the list of errors that occurred during serialization or deserialization.</summary>
        /// <returns>The list of errors that occurred during serialization or deserialization.</returns>
        /// <exception cref="T:System.InvalidOperationException">This property was accessed outside of a serialization session.</exception>
        public IList Errors
        {
            get
            {
                this.CheckSession();
                if (this.errorList == null)
                {
                    this.errorList = new ArrayList();
                }
                return this.errorList;
            }
        }

        /// <summary>Gets or sets a value indicating whether the <see cref="M:System.ComponentModel.Design.Serialization.DesignerSerializationManager.CreateInstance(System.Type,System.Collections.ICollection,System.String,System.Boolean)" /> method should check for the presence of the given name in the container.</summary>
        /// <returns>true if <see cref="M:System.ComponentModel.Design.Serialization.DesignerSerializationManager.CreateInstance(System.Type,System.Collections.ICollection,System.String,System.Boolean)" /> will pass the given component name; false if <see cref="M:System.ComponentModel.Design.Serialization.DesignerSerializationManager.CreateInstance(System.Type,System.Collections.ICollection,System.String,System.Boolean)" /> will check for the presence of the given name in the container. The default is true.</returns>
        /// <exception cref="T:System.InvalidOperationException">This property was changed from within a serialization session.</exception>
        public bool PreserveNames
        {
            get
            {
                return this.preserveNames;
            }
            set
            {
                this.CheckNoSession();
                this.preserveNames = value;
            }
        }

        /// <summary>Gets the object that should be used to provide properties to the serialization manager's <see cref="P:System.ComponentModel.Design.Serialization.IDesignerSerializationManager.Properties" /> property.</summary>
        /// <returns>The object that should be used to provide properties to the serialization manager's <see cref="P:System.ComponentModel.Design.Serialization.IDesignerSerializationManager.Properties" /> property.</returns>
        public object PropertyProvider
        {
            get
            {
                return this.propertyProvider;
            }
            set
            {
                if (this.propertyProvider != value)
                {
                    this.propertyProvider = value;
                    this.properties = null;
                }
            }
        }

        /// <summary>Gets or sets a flag indicating whether <see cref="M:System.ComponentModel.Design.Serialization.DesignerSerializationManager.CreateInstance(System.Type,System.Collections.ICollection,System.String,System.Boolean)" /> will always create a new instance of a type. </summary>
        /// <returns>true if <see cref="M:System.ComponentModel.Design.Serialization.DesignerSerializationManager.CreateInstance(System.Type,System.Collections.ICollection,System.String,System.Boolean)" /> will return the existing instance; false if <see cref="M:System.ComponentModel.Design.Serialization.DesignerSerializationManager.CreateInstance(System.Type,System.Collections.ICollection,System.String,System.Boolean)" /> will create a new instance of a type. The default is false.</returns>
        /// <exception cref="T:System.InvalidOperationException">The serialization manager has an active serialization session.</exception>
        public bool RecycleInstances
        {
            get
            {
                return this.recycleInstances;
            }
            set
            {
                this.CheckNoSession();
                this.recycleInstances = value;
            }
        }

        /// <summary>Gets or sets a flag indicating whether the <see cref="M:System.ComponentModel.Design.Serialization.DesignerSerializationManager.CreateInstance(System.Type,System.Collections.ICollection,System.String,System.Boolean)" /> method will verify that matching names refer to the same type.</summary>
        /// <returns>true if <see cref="M:System.ComponentModel.Design.Serialization.DesignerSerializationManager.CreateInstance(System.Type,System.Collections.ICollection,System.String,System.Boolean)" /> verifies types; otherwise, false if it does not. The default is true.</returns>
        /// <exception cref="T:System.InvalidOperationException">The serialization manager has an active serialization session.</exception>
        public bool ValidateRecycledTypes
        {
            get
            {
                return this.validateRecycledTypes;
            }
            set
            {
                this.CheckNoSession();
                this.validateRecycledTypes = value;
            }
        }

        /// <summary>Gets the context stack for this serialization session. </summary>
        /// <returns>A <see cref="T:System.ComponentModel.Design.Serialization.ContextStack" /> that stores data.</returns>
        /// <exception cref="T:System.InvalidOperationException">This property was accessed outside of a serialization session.</exception>
        ContextStack IDesignerSerializationManager.Context
        {
            get
            {
                if (this.contextStack == null)
                {
                    this.CheckSession();
                    this.contextStack = new ContextStack();
                }
                return this.contextStack;
            }
        }

        /// <summary>Implements the <see cref="P:System.ComponentModel.Design.Serialization.IDesignerSerializationManager.Properties" /> property. </summary>
        /// <returns>A <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> containing the properties to be serialized.</returns>
        PropertyDescriptorCollection IDesignerSerializationManager.Properties
        {
            get
            {
                if (this.properties == null)
                {
                    object obj = this.PropertyProvider;
                    PropertyDescriptor[] array;
                    if (obj == null)
                    {
                        array = new PropertyDescriptor[0];
                    }
                    else
                    {
                        PropertyDescriptorCollection propertyDescriptorCollection = TypeDescriptor.GetProperties(obj);
                        array = new PropertyDescriptor[propertyDescriptorCollection.Count];
                        for (int i = 0; i < array.Length; i++)
                        {
                            array[i] = this.WrapProperty(propertyDescriptorCollection[i], obj);
                        }
                    }
                    this.properties = new PropertyDescriptorCollection(array);
                }
                return this.properties;
            }
        }

        internal ArrayList SerializationProviders
        {
            get
            {
                if (this.designerSerializationProviders == null)
                {
                    return new ArrayList();
                }
                return this.designerSerializationProviders.Clone() as ArrayList;
            }
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.Design.Serialization.DesignerSerializationManager" /> class.</summary>
        public DesignerSerializationManager()
        {
            this.preserveNames = true;
            this.validateRecycledTypes = true;
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.Design.Serialization.DesignerSerializationManager" /> class with the given service provider.</summary>
        /// <param name="provider">An <see cref="T:System.IServiceProvider" />.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="provider" /> is null.</exception>
        public DesignerSerializationManager(IServiceProvider provider)
        {
            this.provider = provider ?? throw new ArgumentNullException("provider");
            this.preserveNames = true;
            this.validateRecycledTypes = true;
        }

        private void CheckNoSession()
        {
            if (this.session != null)
            {
                throw new InvalidOperationException(SR.GetString("SerializationManagerWithinSession"));
            }
        }

        private void CheckSession()
        {
            if (this.session == null)
            {
                throw new InvalidOperationException(SR.GetString("SerializationManagerNoSession"));
            }
        }

        /// <summary>Creates an instance of a type.</summary>
        /// <returns>A new instance of the type specified by <paramref name="type" />.</returns>
        /// <param name="type">The type to create an instance of.</param>
        /// <param name="arguments">The parameters of the type’s constructor. This can be null or an empty collection to invoke the default constructor.</param>
        /// <param name="name">A name to give the object. If null, the object will not be given a name, unless the object is added to a container and the container gives the object a name.</param>
        /// <param name="addToContainer">true to add the object to the container if the object implements <see cref="T:System.ComponentModel.IComponent" />; otherwise, false.</param>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        ///   <paramref name="type" /> does not have a constructor that takes parameters contained in <paramref name="arguments" />.</exception>
        protected virtual object CreateInstance(Type type, ICollection arguments, string name, bool addToContainer)
        {
            object[] array = null;
            if (arguments != null && arguments.Count > 0)
            {
                array = new object[arguments.Count];
                arguments.CopyTo(array, 0);
            }
            object obj = null;
            if (this.RecycleInstances && name != null)
            {
                if (this.instancesByName != null)
                {
                    obj = this.instancesByName[name];
                }
                if ((obj == null & addToContainer) && this.Container != null)
                {
                    obj = this.Container.Components[name];
                }
                if (obj != null && this.ValidateRecycledTypes && obj.GetType() != type)
                {
                    obj = null;
                }
            }
            if ((obj == null & addToContainer) && typeof(IComponent).IsAssignableFrom(type) && (array == null || array.Length == 0 || (array.Length == 1 && array[0] == this.Container)))
            {
                if (this.GetService(typeof(IDesignerHost)) is IDesignerHost designerHost && designerHost.Container == this.Container)
                {
                    bool flag = false;
                    if (!this.PreserveNames && name != null && this.Container.Components[name] != null)
                    {
                        flag = true;
                    }
                    if (name == null | flag)
                    {
                        obj = designerHost.CreateComponent(type);
                    }
                    else
                    {
                        obj = designerHost.CreateComponent(type, name);
                    }
                }
            }
            if (obj == null)
            {
                try
                {
                    try
                    {
                        obj = TypeDescriptor.CreateInstance(this.provider, type, null, array);
                    }
                    catch (MissingMethodException ex)
                    {
                        Type[] array2 = new Type[array.Length];
                        for (int i = 0; i < array.Length; i++)
                        {
                            if (array[i] != null)
                            {
                                array2[i] = array[i].GetType();
                            }
                        }
                        object[] array3 = new object[array.Length];
                        ConstructorInfo[] constructors = TypeDescriptor.GetReflectionType(type).GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance);
                        for (int j = 0; j < constructors.Length; j++)
                        {
                            ConstructorInfo constructorInfo = constructors[j];
                            ParameterInfo[] parameters = constructorInfo.GetParameters();
                            if (parameters != null && parameters.Length == array2.Length)
                            {
                                bool flag2 = true;
                                for (int k = 0; k < array2.Length; k++)
                                {
                                    if (!(array2[k] == null) && !parameters[k].ParameterType.IsAssignableFrom(array2[k]))
                                    {
                                        if (array[k] is IConvertible)
                                        {
                                            try
                                            {
                                                array3[k] = ((IConvertible)array[k]).ToType(parameters[k].ParameterType, null);
                                                goto IL_219;
                                            }
                                            catch (InvalidCastException)
                                            {
                                            }
                                        }
                                        flag2 = false;
                                        break;
                                    }
                                    array3[k] = array[k];
                                    IL_219:;
                                }
                                if (flag2)
                                {
                                    obj = TypeDescriptor.CreateInstance(this.provider, type, null, array3);
                                    break;
                                }
                            }
                        }
                        if (obj == null)
                        {
                            throw ex;
                        }
                    }
                }
                catch (MissingMethodException)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    object[] array4 = array;
                    for (int l = 0; l < array4.Length; l++)
                    {
                        object obj2 = array4[l];
                        if (stringBuilder.Length > 0)
                        {
                            stringBuilder.Append(", ");
                        }
                        if (obj2 != null)
                        {
                            stringBuilder.Append(obj2.GetType().Name);
                        }
                        else
                        {
                            stringBuilder.Append("null");
                        }
                    }
                    throw new SerializationException(SR.GetString("SerializationManagerNoMatchingCtor", new object[]
                    {
                        type.FullName,
                        stringBuilder.ToString()
                    }))
                    {
                        HelpLink = "SerializationManagerNoMatchingCtor"
                    };
                }
                if (addToContainer && obj is IComponent && this.Container != null)
                {
                    bool flag3 = false;
                    if (!this.PreserveNames && name != null && this.Container.Components[name] != null)
                    {
                        flag3 = true;
                    }
                    if (name == null | flag3)
                    {
                        this.Container.Add((IComponent)obj);
                    }
                    else
                    {
                        this.Container.Add((IComponent)obj, name);
                    }
                }
            }
            return obj;
        }

        /// <summary>Creates a new serialization session.</summary>
        /// <returns>An <see cref="T:System.IDisposable" /> that represents a new serialization session.</returns>
        /// <exception cref="T:System.InvalidOperationException">The serialization manager is already within a session. This version of <see cref="T:System.ComponentModel.Design.Serialization.DesignerSerializationManager" /> does not support simultaneous sessions.</exception>
        public IDisposable CreateSession()
        {
            if (this.session != null)
            {
                throw new InvalidOperationException(SR.GetString("SerializationManagerAreadyInSession"));
            }
            this.session = new DesignerSerializationManager.SerializationSession(this);
            this.OnSessionCreated(EventArgs.Empty);
            return this.session;
        }

        /// <summary>Gets the serializer for the given object type.</summary>
        /// <returns>The serializer for <paramref name="objectType" />, or null, if not found.</returns>
        /// <param name="objectType">The type of object for which to retrieve the serializer.</param>
        /// <param name="serializerType">The type of serializer to retrieve.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="objectType" /> or <paramref name="serializerType" /> is null.</exception>
        public object GetSerializer(Type objectType, Type serializerType)
        {
            if (serializerType == null)
            {
                throw new ArgumentNullException("serializerType");
            }
            object obj = null;
            if (objectType != null)
            {
                if (this.serializers != null)
                {
                    obj = this.serializers[objectType];
                    if (obj != null && !serializerType.IsAssignableFrom(obj.GetType()))
                    {
                        obj = null;
                    }
                }
                if (obj == null)
                {
                    AttributeCollection attributes = TypeDescriptor.GetAttributes(objectType);
                    foreach (Attribute attribute in attributes)
                    {
                        if (attribute is DesignerSerializerAttribute designerSerializerAttribute)
                        {
                            string serializerBaseTypeName = designerSerializerAttribute.SerializerBaseTypeName;
                            if (serializerBaseTypeName != null)
                            {
                                Type runtimeType = this.GetRuntimeType(serializerBaseTypeName);
                                if (runtimeType == serializerType && designerSerializerAttribute.SerializerTypeName != null && designerSerializerAttribute.SerializerTypeName.Length > 0)
                                {
                                    Type runtimeType2 = this.GetRuntimeType(designerSerializerAttribute.SerializerTypeName);
                                    if (runtimeType2 != null)
                                    {
                                        obj = Activator.CreateInstance(runtimeType2, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, null, null);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    if (obj != null && this.session != null)
                    {
                        if (this.serializers == null)
                        {
                            this.serializers = new Hashtable();
                        }
                        this.serializers[objectType] = obj;
                    }
                }
            }
            if (this.defaultProviderTable == null || !this.defaultProviderTable.ContainsKey(serializerType))
            {
                Type type = null;
                DefaultSerializationProviderAttribute defaultSerializationProviderAttribute = (DefaultSerializationProviderAttribute)TypeDescriptor.GetAttributes(serializerType)[typeof(DefaultSerializationProviderAttribute)];
                if (defaultSerializationProviderAttribute != null)
                {
                    type = this.GetRuntimeType(defaultSerializationProviderAttribute.ProviderTypeName);
                    if (type != null && typeof(IDesignerSerializationProvider).IsAssignableFrom(type))
                    {
                        IDesignerSerializationProvider designerSerializationProvider = (IDesignerSerializationProvider)Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, null, null);
                        ((IDesignerSerializationManager)this).AddSerializationProvider(designerSerializationProvider);
                    }
                }
                if (this.defaultProviderTable == null)
                {
                    this.defaultProviderTable = new Hashtable();
                }
                this.defaultProviderTable[serializerType] = type;
            }
            if (this.designerSerializationProviders != null)
            {
                bool flag = true;
                int num = 0;
                while (flag && num < this.designerSerializationProviders.Count)
                {
                    flag = false;
                    foreach (IDesignerSerializationProvider designerSerializationProvider2 in this.designerSerializationProviders)
                    {
                        object serializer = designerSerializationProvider2.GetSerializer(this, obj, objectType, serializerType);
                        if (serializer != null)
                        {
                            flag = (obj != serializer);
                            obj = serializer;
                        }
                    }
                    num++;
                }
            }
            return obj;
        }

        /// <summary>Gets the requested service.</summary>
        /// <returns>The requested service, or null if the service cannot be resolved.</returns>
        /// <param name="serviceType">The type of service to retrieve.</param>
        protected virtual object GetService(Type serviceType)
        {
            if (serviceType == typeof(IContainer))
            {
                return this.Container;
            }
            if (this.provider != null)
            {
                return this.provider.GetService(serviceType);
            }
            return null;
        }

        /// <summary>Gets the requested type.</summary>
        /// <returns>The requested type, or null if the type cannot be resolved.</returns>
        /// <param name="typeName">The name of the type to retrieve.</param>
        protected virtual Type GetType(string typeName)
        {
            Type type = this.GetRuntimeType(typeName);
            if (type != null)
            {
                if (this.GetService(typeof(TypeDescriptionProviderService)) is TypeDescriptionProviderService typeDescriptionProviderService)
                {
                    TypeDescriptionProvider typeDescriptionProvider = typeDescriptionProviderService.GetProvider(type);
                    if (!typeDescriptionProvider.IsSupportedType(type))
                    {
                        type = null;
                    }
                }
            }
            return type;
        }

        /// <summary>Gets the type corresponding to the specified type name.</summary>
        /// <returns>The specified type.</returns>
        /// <param name="typeName">The name of the type to get.</param>
        public Type GetRuntimeType(string typeName)
        {
            if (this.typeResolver == null && !this.searchedTypeResolver)
            {
                this.typeResolver = (this.GetService(typeof(ITypeResolutionService)) as ITypeResolutionService);
                this.searchedTypeResolver = true;
            }
            Type type;
            if (this.typeResolver == null)
            {
                type = Type.GetType(typeName);
            }
            else
            {
                type = this.typeResolver.GetType(typeName);
            }
            return type;
        }

        /// <summary>Raises the <see cref="E:System.ComponentModel.Design.Serialization.IDesignerSerializationManager.ResolveName" /> event. </summary>
        /// <param name="e">A <see cref="T:System.ComponentModel.Design.Serialization.ResolveNameEventArgs" /> that contains the event data. </param>
        protected virtual void OnResolveName(ResolveNameEventArgs e)
        {
            this.resolveNameEventHandler?.Invoke(this, e);
        }

        /// <summary>Raises the <see cref="E:System.ComponentModel.Design.Serialization.DesignerSerializationManager.SessionCreated" /> event. </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
        protected virtual void OnSessionCreated(EventArgs e)
        {
            this.sessionCreatedEventHandler?.Invoke(this, e);
        }

        /// <summary>Raises the <see cref="E:System.ComponentModel.Design.Serialization.DesignerSerializationManager.SessionDisposed" /> event. </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected virtual void OnSessionDisposed(EventArgs e)
        {
            try
            {
                try
                {
                    this.sessionDisposedEventHandler?.Invoke(this, e);
                }
                finally
                {
                    this.serializationCompleteEventHandler?.Invoke(this, EventArgs.Empty);
                }
            }
            finally
            {
                this.resolveNameEventHandler = null;
                this.serializationCompleteEventHandler = null;
                this.instancesByName = null;
                this.namesByInstance = null;
                this.serializers = null;
                this.contextStack = null;
                this.errorList = null;
                this.session = null;
            }
        }

        private PropertyDescriptor WrapProperty(PropertyDescriptor property, object owner)
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }
            return new DesignerSerializationManager.WrappedPropertyDescriptor(property, owner);
        }

        /// <summary>Adds a custom serialization provider to the serialization manager.</summary>
        /// <param name="provider">The serialization provider to add.</param>
        void IDesignerSerializationManager.AddSerializationProvider(IDesignerSerializationProvider provider)
        {
            if (this.designerSerializationProviders == null)
            {
                this.designerSerializationProviders = new ArrayList();
            }
            if (!this.designerSerializationProviders.Contains(provider))
            {
                this.designerSerializationProviders.Add(provider);
            }
        }

        /// <summary>Implements the <see cref="M:System.ComponentModel.Design.Serialization.IDesignerSerializationManager.CreateInstance(System.Type,System.Collections.ICollection,System.String,System.Boolean)" /> method.</summary>
        /// <returns>The newly created object instance.</returns>
        /// <param name="type">The data type to create. </param>
        /// <param name="arguments">The arguments to pass to the constructor for this type. </param>
        /// <param name="name">The name of the object. This name can be used to access the object later through <see cref="M:System.ComponentModel.Design.Serialization.IDesignerSerializationManager.GetInstance(System.String)" />. If null is passed, the object is still created but cannot be accessed by name. </param>
        /// <param name="addToContainer">true to add this object to the design container. The object must implement <see cref="T:System.ComponentModel.IComponent" /> for this to have any effect. </param>
        object IDesignerSerializationManager.CreateInstance(Type type, ICollection arguments, string name, bool addToContainer)
        {
            this.CheckSession();
            if (name != null && this.instancesByName != null && this.instancesByName.ContainsKey(name))
            {
                throw new SerializationException(SR.GetString("SerializationManagerDuplicateComponentDecl", new object[]
                {
                    name
                }))
                {
                    HelpLink = "SerializationManagerDuplicateComponentDecl"
                };
            }
            object obj = this.CreateInstance(type, arguments, name, addToContainer);
            if (name != null && (!(obj is IComponent) || !this.RecycleInstances))
            {
                if (this.instancesByName == null)
                {
                    this.instancesByName = new Hashtable();
                    this.namesByInstance = new Hashtable(new DesignerSerializationManager.ReferenceComparer());
                }
                this.instancesByName[name] = obj;
                this.namesByInstance[obj] = name;
            }
            return obj;
        }

        /// <summary>Retrieves an instance of a created object of the specified name.</summary>
        /// <returns>An instance of the object with the given name, or null if no object by that name can be found.</returns>
        /// <param name="name">The name of the object to retrieve.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="name" /> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">This property was accessed outside of a serialization session.</exception>
        object IDesignerSerializationManager.GetInstance(string name)
        {
            object obj = null;
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            this.CheckSession();
            if (this.instancesByName != null)
            {
                obj = this.instancesByName[name];
            }
            if (obj == null && this.PreserveNames && this.Container != null)
            {
                obj = this.Container.Components[name];
            }
            if (obj == null)
            {
                ResolveNameEventArgs resolveNameEventArgs = new ResolveNameEventArgs(name);
                this.OnResolveName(resolveNameEventArgs);
                obj = resolveNameEventArgs.Value;
            }
            return obj;
        }

        /// <summary>Retrieves a name for the specified object.</summary>
        /// <returns>The name of the object, or null if the object is unnamed.</returns>
        /// <param name="value">The object for which to retrieve the name.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="value" /> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">This property was accessed outside of a serialization session.</exception>
        string IDesignerSerializationManager.GetName(object value)
        {
            string text = null;
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            this.CheckSession();
            if (this.namesByInstance != null)
            {
                text = (string)this.namesByInstance[value];
            }
            if (text == null && value is IComponent)
            {
                ISite site = ((IComponent)value).Site;
                if (site != null)
                {
                    if (site is INestedSite nestedSite)
                    {
                        text = nestedSite.FullName;
                    }
                    else
                    {
                        text = site.Name;
                    }
                }
            }
            return text;
        }

        /// <summary>Gets a serializer of the requested type for the specified object type.</summary>
        /// <returns>An instance of the requested serializer, or null if no appropriate serializer can be located.</returns>
        /// <param name="objectType">The type of the object to get the serializer for.</param>
        /// <param name="serializerType">The type of the serializer to retrieve.</param>
        object IDesignerSerializationManager.GetSerializer(Type objectType, Type serializerType)
        {
            return this.GetSerializer(objectType, serializerType);
        }

        /// <summary>Gets a type of the specified name.</summary>
        /// <returns>An instance of the type, or null if the type cannot be loaded.</returns>
        /// <param name="typeName">The fully qualified name of the type to load.</param>
        /// <exception cref="T:System.InvalidOperationException">This property was accessed outside of a serialization session.</exception>
        Type IDesignerSerializationManager.GetType(string typeName)
        {
            this.CheckSession();
            Type type = null;
            while (type == null)
            {
                type = this.GetType(typeName);
                if (type == null && typeName != null && typeName.Length > 0)
                {
                    int num = typeName.LastIndexOf('.');
                    if (num == -1 || num == typeName.Length - 1)
                    {
                        break;
                    }
                    typeName = typeName.Substring(0, num) + "+" + typeName.Substring(num + 1, typeName.Length - num - 1);
                }
            }
            return type;
        }

        /// <summary>Removes a previously added serialization provider.</summary>
        /// <param name="provider">The <see cref="T:System.ComponentModel.Design.Serialization.IDesignerSerializationProvider" /> to remove.</param>
        void IDesignerSerializationManager.RemoveSerializationProvider(IDesignerSerializationProvider provider)
        {
            if (this.designerSerializationProviders != null)
            {
                this.designerSerializationProviders.Remove(provider);
            }
        }

        /// <summary>Used to report a recoverable error in serialization.</summary>
        /// <param name="errorInformation">An object containing the error information, usually of type <see cref="T:System.String" /> or <see cref="T:System.Exception" />.</param>
        /// <exception cref="T:System.InvalidOperationException">This property was accessed outside of a serialization session.</exception>
        void IDesignerSerializationManager.ReportError(object errorInformation)
        {
            this.CheckSession();
            if (errorInformation != null)
            {
                this.Errors.Add(errorInformation);
            }
        }

        /// <summary>Sets the name for the specified object.</summary>
        /// <param name="instance">The object to set the name.</param>
        /// <param name="name">A <see cref="T:System.String" /> used as the name of the object.</param>
        /// <exception cref="T:System.ArgumentNullException">One or both of the parameters are null.</exception>
        /// <exception cref="T:System.ArgumentException">The object specified by instance already has a name, or <paramref name="name" /> is already used by another named object.</exception>
        /// <exception cref="T:System.InvalidOperationException">This property was accessed outside of a serialization session.</exception>
        void IDesignerSerializationManager.SetName(object instance, string name)
        {
            this.CheckSession();
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (this.instancesByName == null)
            {
                this.instancesByName = new Hashtable();
                this.namesByInstance = new Hashtable(new DesignerSerializationManager.ReferenceComparer());
            }
            if (this.instancesByName[name] != null)
            {
                throw new ArgumentException(SR.GetString("SerializationManagerNameInUse", new object[]
                {
                    name
                }));
            }
            if (this.namesByInstance[instance] != null)
            {
                throw new ArgumentException(SR.GetString("SerializationManagerObjectHasName", new object[]
                {
                    name,
                    (string)this.namesByInstance[instance]
                }));
            }
            this.instancesByName[name] = instance;
            this.namesByInstance[instance] = name;
        }

        /// <summary>For a description of this member, see the <see cref="M:System.IServiceProvider.GetService(System.Type)" /> method.</summary>
        /// <returns>A service object of type <paramref name="serviceType" />.-or-null if there is no service object of type <paramref name="serviceType" />.</returns>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        object IServiceProvider.GetService(Type serviceType)
        {
            return this.GetService(serviceType);
        }
    }
}
