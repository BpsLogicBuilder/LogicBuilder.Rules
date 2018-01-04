// ---------------------------------------------------------------------------
// Copyright (C) 2006 Microsoft Corporation All Rights Reserved
// ---------------------------------------------------------------------------

#define CODE_ANALYSIS
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace LogicBuilder.Workflow.Activities.Rules
{
    public class RuleEngine
    {
        private string name;
        private RuleValidation validation;
        private IList<RuleState> analyzedRules;

        public RuleEngine(RuleSet ruleSet, Type objectType)
            : this(ruleSet, new RuleValidation(objectType))
        {
        }

        public RuleEngine(RuleSet ruleSet, RuleValidation validation)
        {
            // now validate it
            if (!ruleSet.Validate(validation))
            {
                string message = string.Format(CultureInfo.CurrentCulture, Messages.RuleSetValidationFailed, ruleSet.name);
                throw new RuleSetValidationException(message, validation.Errors);
            }

            this.name = ruleSet.Name;
            this.validation = validation;
            Tracer tracer = null;
            if (WorkflowActivityTrace.Rules.Switch.ShouldTrace(TraceEventType.Information))
                tracer = new Tracer(ruleSet.Name);
            this.analyzedRules = Executor.Preprocess(ruleSet.ChainingBehavior, ruleSet.Rules, validation, tracer);
        }


        [SuppressMessage("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters", MessageId = "0#")]
        public void Execute(object thisObject)
        {
            Execute(new RuleExecution(validation, thisObject));
        }

        internal void Execute(RuleExecution ruleExecution)
        {
            Tracer tracer = null;
            if (WorkflowActivityTrace.Rules.Switch.ShouldTrace(TraceEventType.Information))
            {
                tracer = new Tracer(name);
                tracer.StartRuleSet();
            }
            Executor.ExecuteRuleSet(analyzedRules, ruleExecution, tracer, RuleSet.RuleSetTrackingKey + name);
        }
    }
}
