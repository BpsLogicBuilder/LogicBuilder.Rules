using System;
using System.Runtime.Serialization;

namespace LogicBuilder.RuleSetToolkit.Exceptions
{
    /// <summary>
    /// Exceptions thrown from this application
    /// </summary>
    [Serializable()]
    public class ToolkitException : System.Exception
    {
        public ToolkitException()
            : base(Properties.Resources.anErrorHasOccurred)
        {
            // Add any type-specific logic, and supply the default message.
        }


        public ToolkitException(string logicBuilderMessage)
            : base(logicBuilderMessage)
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public ToolkitException(string logicBuilderMessage, Exception ex)
            : base(logicBuilderMessage, ex)
        {
            //
            // TODO: Add constructor logic here
            //
        }

        protected ToolkitException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // Implement type-specific serialization constructor logic.
        }
    }
}
