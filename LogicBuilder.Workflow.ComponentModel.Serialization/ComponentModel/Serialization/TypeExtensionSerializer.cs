namespace LogicBuilder.Workflow.ComponentModel.Serialization
{
    using System;
    using System.ComponentModel.Design.Serialization;

    #region Class TypeExtensionSerializer
    internal class TypeExtensionSerializer : MarkupExtensionSerializer
    {
        protected override InstanceDescriptor GetInstanceDescriptor(WorkflowMarkupSerializationManager serializationManager, object value)
        {
            TypeExtension typeExtension = value as TypeExtension;
            if (typeExtension == null)
                throw new ArgumentException(SR.GetString(SR.Error_UnexpectedArgumentType, typeof(TypeExtension).FullName), "value");
            if (typeExtension.Type != null)
                return new InstanceDescriptor(typeof(TypeExtension).GetConstructor(new Type[] { typeof(System.Type) }),
                    new object[] { typeExtension.Type });
            return new InstanceDescriptor(typeof(TypeExtension).GetConstructor(new Type[] { typeof(string) }),
                new object[] { typeExtension.TypeName });
        }
    }
    #endregion
}
