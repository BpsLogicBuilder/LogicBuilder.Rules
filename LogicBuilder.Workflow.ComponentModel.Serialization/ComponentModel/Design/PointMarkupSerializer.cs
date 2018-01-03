using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using LogicBuilder.Workflow.ComponentModel.Serialization;
using System;

namespace LogicBuilder.Workflow.ComponentModel.Design
{
    internal sealed class PointMarkupSerializer : WorkflowMarkupSerializer
    {
        protected internal override bool CanSerializeToString(WorkflowMarkupSerializationManager serializationManager, object value)
        {
            return (value is Point);
        }

        protected internal override PropertyInfo[] GetProperties(WorkflowMarkupSerializationManager serializationManager, object obj)
        {
            List<PropertyInfo> properties = new List<PropertyInfo>();
            if (obj is Point)
            {
                properties.Add(typeof(Point).GetProperty("X"));
                properties.Add(typeof(Point).GetProperty("Y"));
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
            object point = Point.Empty;

            string pointValue = value as string;
            if (!String.IsNullOrEmpty(pointValue))
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(Point));
                if (converter != null && converter.CanConvertFrom(typeof(string)) && !IsValidCompactAttributeFormat(pointValue))
                    point = converter.ConvertFrom(value);
                else
                    point = base.SerializeToString(serializationManager, value);
            }

            return point;
        }
    }
}
