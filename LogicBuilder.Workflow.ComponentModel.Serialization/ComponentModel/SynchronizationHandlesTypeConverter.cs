using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace LogicBuilder.Workflow.ComponentModel
{
    internal sealed class SynchronizationHandlesTypeConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is ICollection<String>)
                return Stringify(value as ICollection<String>);

            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
                return UnStringify(value as string);

            return base.ConvertFrom(context, culture, value);
        }

        internal static string Stringify(ICollection<String> synchronizationHandles)
        {
            string stringifiedValue = string.Empty;
            if (synchronizationHandles == null)
                return stringifiedValue;

            foreach (string handle in synchronizationHandles)
            {
                if (handle == null)
                    continue;
                if (stringifiedValue != string.Empty)
                    stringifiedValue += ", ";
                stringifiedValue += handle.Replace(",", "\\,");
            }

            return stringifiedValue;
        }

        internal static ICollection<String> UnStringify(string stringifiedValue)
        {
            ICollection<String> synchronizationHandles = new List<String>();
            stringifiedValue = stringifiedValue.Replace("\\,", ">");
            foreach (string handle in stringifiedValue.Split(new char[] { ',', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string realHandle = handle.Trim().Replace('>', ',');
                if (realHandle != string.Empty && !synchronizationHandles.Contains(realHandle))
                    synchronizationHandles.Add(realHandle);
            }

            return synchronizationHandles;
        }
    }
}
