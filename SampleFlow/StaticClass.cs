using LogicBuilder.Workflow.Activities.Rules;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleFlow
{
    public static class StaticClass
    {
        public static void SetDiscount(FlowEntity ent, decimal discount)
        {
            ent.Discount = discount;
        }

        public static decimal GetDiscount(FlowEntity ent)
        {
            return ent.Discount;
        }

        public static void SetState(FlowEntity ent, string state)
        {
            ent.State = state;
        }

        public static string GetState(FlowEntity ent)
        {
            return ent.State;
        }
    }
}
