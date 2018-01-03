using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using LogicBuilder.Workflow.ComponentModel.Serialization;
using System;

namespace LogicBuilder.Workflow.ComponentModel.Design
{
    internal sealed class SizeMarkupSerializer : WorkflowMarkupSerializer
    {
        protected internal override bool CanSerializeToString(WorkflowMarkupSerializationManager serializationManager, object value)
        {
            return (value is System.Drawing.Size);
        }

        protected internal override PropertyInfo[] GetProperties(WorkflowMarkupSerializationManager serializationManager, object obj)
        {
            List<PropertyInfo> properties = new List<PropertyInfo>();
            if (obj is Size)
            {
                properties.Add(typeof(Size).GetProperty("Width"));
                properties.Add(typeof(Size).GetProperty("Height"));
            }
            return properties.ToArray();
        }

        protected internal override string SerializeToString(WorkflowMarkupSerializationManager serializationManager, object value)
        {
            string convertedValue = String.Empty;

            TypeConverter converter = TypeDescriptor.GetConverter(value);
            if (converter != null && converter.CanConvertTo(typeof(string)))
                convertedValue = converter.ConvertTo(value, typeof(string)) as string;
            else
                convertedValue = base.SerializeToString(serializationManager, value);
            return convertedValue;
        }

        protected internal override object DeserializeFromString(WorkflowMarkupSerializationManager serializationManager, Type propertyType, string value)
        {
            object size = Size.Empty;

            string sizeValue = value as string;
            if (!String.IsNullOrEmpty(sizeValue))
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(Size));
                if (converter != null && converter.CanConvertFrom(typeof(string)) && !IsValidCompactAttributeFormat(sizeValue))
                    size = converter.ConvertFrom(value);
                else
                    size = base.SerializeToString(serializationManager, value);
            }

            return size;
        }
    }
}
