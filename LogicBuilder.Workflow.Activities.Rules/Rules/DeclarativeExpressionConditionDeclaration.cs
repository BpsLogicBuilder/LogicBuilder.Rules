// ---------------------------------------------------------------------------
// Copyright (C) 2005 Microsoft Corporation - All Rights Reserved
// ---------------------------------------------------------------------------

using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using LogicBuilder.Workflow.Activities.Common;
using LogicBuilder.Workflow.ComponentModel.Compiler;
using System;

namespace LogicBuilder.Workflow.Activities.Rules
{
    #region RuleCondition base class
    [Serializable]
    public abstract class RuleCondition
    {
        public abstract bool Validate(RuleValidation validation);
        public abstract bool Evaluate(RuleExecution execution);
        public abstract ICollection<string> GetDependencies(RuleValidation validation);

        public abstract string Name { get; set; }
        public virtual void OnRuntimeInitialized() { }

        public abstract RuleCondition Clone();
    }
    #endregion

    #region RuleExpressionCondition Class
    [Serializable]
    public sealed class RuleExpressionCondition : RuleCondition
    {
        #region Properties
        private CodeExpression _expression;
        private string _name;
        private bool _runtimeInitialized;
        [NonSerialized]
        private object _expressionLock = new object();

        public override string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                if (this._runtimeInitialized)
                    throw new InvalidOperationException(SR.GetString(SR.Error_CanNotChangeAtRuntime));

                this._name = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public CodeExpression Expression
        {
            get
            {
                return _expression;
            }
            set
            {
                if (this._runtimeInitialized)
                    throw new InvalidOperationException(SR.GetString(SR.Error_CanNotChangeAtRuntime));

                lock (this._expressionLock)
                {
                    _expression = value;
                }
            }
        }

        #endregion

        #region Constructors
        public RuleExpressionCondition()
        {
        }

        public RuleExpressionCondition(string conditionName)
        {
            _name = conditionName ?? throw new ArgumentNullException("conditionName");
        }

        public RuleExpressionCondition(string conditionName, CodeExpression expression)
            : this(conditionName)
        {
            _expression = expression;
        }

        public RuleExpressionCondition(CodeExpression expression)
        {
            _expression = expression;
        }
        #endregion

        #region Public Methods

        public override void OnRuntimeInitialized()
        {
            if (this._runtimeInitialized)

                return;

            _runtimeInitialized = true;
        }


        public override bool Equals(object obj)
        {
            bool equals = false;

            if (obj is RuleExpressionCondition declarativeConditionDefinition)
            {
                equals = ((this.Name == declarativeConditionDefinition.Name) &&
                          ((this._expression == null && declarativeConditionDefinition.Expression == null) ||
                           (this._expression != null && RuleExpressionWalker.Match(this._expression, declarativeConditionDefinition.Expression))));
            }

            return equals;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            if (_expression != null)
            {
                StringBuilder decompilation = new StringBuilder();
                RuleExpressionWalker.Decompile(decompilation, _expression, null);
                return decompilation.ToString();
            }
            else
            {
                return "";
            }
        }

        #endregion

        #region RuleExpressionCondition methods

        public override bool Validate(RuleValidation validation)
        {
            if (validation == null)
                throw new ArgumentNullException("validation");

            bool valid = true;

            if (_expression == null)
            {
                valid = false;

                string message = string.Format(CultureInfo.CurrentCulture, Messages.ConditionExpressionNull, typeof(CodePrimitiveExpression).ToString());
                ValidationError error = new ValidationError(message, ErrorNumbers.Error_EmptyExpression);
                error.UserData[RuleUserDataKeys.ErrorObject] = this;
                validation.AddError(error);

            }
            else
            {
                valid = validation.ValidateConditionExpression(_expression);
            }

            return valid;
        }

        public override bool Evaluate(RuleExecution execution)
        {
            if (_expression == null)
                return true;

            return Executor.EvaluateBool(_expression, execution);
        }

        public override ICollection<string> GetDependencies(RuleValidation validation)
        {
            RuleAnalysis analyzer = new RuleAnalysis(validation, false);
            if (_expression != null)
                RuleExpressionWalker.AnalyzeUsage(analyzer, _expression, true, false, null);
            return analyzer.GetSymbols();
        }
        #endregion

        #region Cloning

        public override RuleCondition Clone()
        {
            RuleExpressionCondition ruleCondition = (RuleExpressionCondition)this.MemberwiseClone();
            ruleCondition._runtimeInitialized = false;
            ruleCondition._expression = RuleExpressionWalker.Clone(this._expression);
            return ruleCondition;
        }

        #endregion
    }
    #endregion
}
