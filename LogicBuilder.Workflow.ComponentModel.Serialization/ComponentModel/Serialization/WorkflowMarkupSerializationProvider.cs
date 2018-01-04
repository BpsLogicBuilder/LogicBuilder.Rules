namespace LogicBuilder.Workflow.ComponentModel.Serialization
{
    using System;
    using System.Collections;
    using System.ComponentModel.Design.Serialization;

    #region Class WorkflowMarkupSerializationProvider
    internal class WorkflowMarkupSerializationProvider : IDesignerSerializationProvider
    {
        public virtual object GetSerializer(IDesignerSerializationManager manager, object currentSerializer, Type objectType, Type serializerType)
        {
            // If this isn't a serializer type we recognize, do nothing.  Also, if metadata specified
            // a custom serializer, then use it.
            if (serializerType != typeof(WorkflowMarkupSerializer) || currentSerializer != null)
                return null;

            //DO NOT CHANGE THIS ORDER ELSE DICTIONARY WILL START GETTING SERIALIZED AS COLLECTION
            if (typeof(IDictionary).IsAssignableFrom(objectType))
                return new DictionaryMarkupSerializer();

            if (CollectionMarkupSerializer.IsValidCollectionType(objectType))
                return new CollectionMarkupSerializer();

            return new WorkflowMarkupSerializer();
        }
    }
    #endregion
}
