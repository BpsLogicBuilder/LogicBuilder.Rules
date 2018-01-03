namespace LogicBuilder.Workflow.ComponentModel.Serialization
{
    using System;
    using System.Runtime.Serialization;

    #region Class WorkflowMarkupSerializationException
    [Serializable()]
    public class WorkflowMarkupSerializationException : Exception
    {
        private int lineNumber = -1;
        private int columnNumber = -1;

        public WorkflowMarkupSerializationException(string message, int lineNumber, int columnNumber)
            : base(message)
        {
            this.lineNumber = lineNumber;
            this.columnNumber = columnNumber;
        }

        public WorkflowMarkupSerializationException(string message, Exception innerException, int lineNumber, int columnNumber)
            : base(message, innerException)
        {
            this.lineNumber = lineNumber;
            this.columnNumber = columnNumber;
        }

        public WorkflowMarkupSerializationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public WorkflowMarkupSerializationException(string message)
            : base(message)
        {
        }

        public WorkflowMarkupSerializationException()
            : base()
        {
        }

        protected WorkflowMarkupSerializationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info == null)
                throw new ArgumentNullException("info");

            this.lineNumber = info.GetInt32("lineNumber");
            this.columnNumber = info.GetInt32("columnNumber");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info");

            base.GetObjectData(info, context);

            info.AddValue("lineNumber", this.lineNumber, typeof(int));
            info.AddValue("columnNumber", this.columnNumber, typeof(int));
        }

        public int LineNumber
        {
            get
            {
                return this.lineNumber;
            }
        }

        public int LinePosition
        {
            get
            {
                return this.columnNumber;
            }
        }
    }
    #endregion
}
