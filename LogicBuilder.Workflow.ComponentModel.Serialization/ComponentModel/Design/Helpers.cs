using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Xml;

namespace LogicBuilder.Workflow.ComponentModel.Design
{
    internal static class Helpers
    {
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal static XmlWriter CreateXmlWriter(object output)
        {
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = ("\t"),
                OmitXmlDeclaration = true,
                CloseOutput = true
            };

            if (output is string)
                return XmlWriter.Create(output as string, settings);
            else if (output is TextWriter)
                return XmlWriter.Create(output as TextWriter, settings);
            else
            {
                Debug.Assert(false, "Invalid argument type.  'output' must either be string or TextWriter.");
                return null;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal static DesignerSerializationVisibility GetSerializationVisibility(MemberInfo memberInfo)
        {

            if (memberInfo == null)
                throw new ArgumentNullException("memberInfo");

            DesignerSerializationVisibility designerSerializationVisibility = DesignerSerializationVisibility.Visible;

            // Calling GetCustomAttributes on PropertyInfo or EventInfo when the inherit parameter of GetCustomAttributes 
            // is true does not walk the type hierarchy. But System.Attribute.GetCustomAttributes causes perf issues.
            object[] attributes = memberInfo.GetCustomAttributes(typeof(DesignerSerializationVisibilityAttribute), true);
            if (attributes.Length > 0)
                designerSerializationVisibility = (attributes[0] as DesignerSerializationVisibilityAttribute).Visibility;
            else if (Attribute.IsDefined(memberInfo, typeof(DesignerSerializationVisibilityAttribute)))
                designerSerializationVisibility = (Attribute.GetCustomAttribute(memberInfo, typeof(DesignerSerializationVisibilityAttribute)) as DesignerSerializationVisibilityAttribute).Visibility;

            return designerSerializationVisibility;
        }
    }
}
