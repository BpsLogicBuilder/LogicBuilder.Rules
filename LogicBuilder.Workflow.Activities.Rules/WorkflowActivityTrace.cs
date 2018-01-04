using System;
using System.Diagnostics;

[assembly: CLSCompliant(true)]
namespace LogicBuilder.Workflow.Activities
{
    internal static class WorkflowActivityTrace
    {
        static TraceSource rules;

        internal static TraceSource Rules
        {
            get { return rules; }
        }

        /// <summary>
        /// Statically set up trace sources
        /// 
        /// To enable logging to a file, add lines like the following to your app config file.
        /*
            <system.diagnostics>
                <switches>
                    <add name="LogicBuilder.Workflow LogToFile" value="1" />
                </switches>
            </system.diagnostics>
        */
        /// To enable tracing to default trace listeners, add lines like the following
        /*
            <system.diagnostics>
                <switches>
                    <add name="LogicBuilder.Workflow LogToTraceListener" value="1" />
                </switches>
            </system.diagnostics>
        */
        /// </summary>
        static WorkflowActivityTrace()
        {
            rules = new TraceSource("LogicBuilder.Workflow.Activities.Rules")
            {
                Switch = new SourceSwitch("LogicBuilder.Workflow.Activities.Rules", SourceLevels.Off.ToString())
            };

            foreach (TraceListener listener in Trace.Listeners)
            {
                if (!(listener is DefaultTraceListener))
                {
                    rules.Listeners.Add(listener);
                }
            }
        }
    }
}
