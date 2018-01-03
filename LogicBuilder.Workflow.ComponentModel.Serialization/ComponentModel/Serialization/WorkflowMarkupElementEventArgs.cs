namespace LogicBuilder.Workflow.ComponentModel.Serialization
{
    using System;
    using System.Xml;

    #region Element deserialization hooks

    #region WorkflowMarkupElementEventArgs
    internal sealed class WorkflowMarkupElementEventArgs : EventArgs
    {
        private XmlReader reader = null;

        internal WorkflowMarkupElementEventArgs(XmlReader reader)
        {
            this.reader = reader;
        }

        public XmlReader XmlReader
        {
            get
            {
                return this.reader;
            }
        }
    }
    #endregion

    #endregion
}
