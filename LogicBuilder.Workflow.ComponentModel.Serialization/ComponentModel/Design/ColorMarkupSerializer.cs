using LogicBuilder.Workflow.ComponentModel.Serialization;
using System;

namespace LogicBuilder.Workflow.ComponentModel.Design
{
    internal sealed class ColorMarkupSerializer : WorkflowMarkupSerializer
    {
        protected internal override bool CanSerializeToString(WorkflowMarkupSerializationManager serializationManager, object value)
        {
            return (value is System.Drawing.Color);
        }

        protected internal override string SerializeToString(WorkflowMarkupSerializationManager serializationManager, object value)
        {
            if (serializationManager == null)
                throw new ArgumentNullException("serializationManager");
            if (value == null)
                throw new ArgumentNullException("value");

            string stringValue = String.Empty;
            if (value is System.Drawing.Color color)
            {
                long colorValue = (long)((uint)(color.A << 24 | color.R << 16 | color.G << 8 | color.B)) & 0xFFFFFFFF;
                stringValue = "0X" + colorValue.ToString("X08", System.Globalization.CultureInfo.InvariantCulture);
            }
            return stringValue;
        }

        protected internal override object DeserializeFromString(WorkflowMarkupSerializationManager serializationManager, Type propertyType, string value)
        {
            if (propertyType.IsAssignableFrom(typeof(System.Drawing.Color)))
            {
                string colorValue = value as string;
                if (!String.IsNullOrEmpty(colorValue))
                {
                    if (colorValue.StartsWith("0X", StringComparison.OrdinalIgnoreCase))
                    {
                        long propertyValue = Convert.ToInt64((string)value, 16) & 0xFFFFFFFF;
                        return System.Drawing.Color.FromArgb((Byte)(propertyValue >> 24), (Byte)(propertyValue >> 16), (Byte)(propertyValue >> 8), (Byte)(propertyValue));
                    }
                    else
                    {
                        return base.DeserializeFromString(serializationManager, propertyType, value);
                    }
                }
            }

            return null;
        }
    }
}
