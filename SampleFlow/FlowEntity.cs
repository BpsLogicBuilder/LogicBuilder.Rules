using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
//using System.Linq;

namespace SampleFlow
{
    public class FlowEntity
    {
        public static string DEFAULTSTATE = "NY";
        public static string BoolTestComparison = "false";

        public bool AlwaysTrue { get { return true; } }

        public string State { get; set; }
        public string BoolText { get; set; }
        public ChildEntity ChildEntity { get; set; } = new ChildEntity();

        public string[,] StringList { get; set; } = new string[2, 2];

        public decimal Discount { get; set; }
        public DateTime Date { get; set; }
        public string FirstValue { get; set; }

        public Type TheType { get; set; }
        public Collection<object> MyCollection { get; set; }
        public List<string> MyList { get; set; } = new List<string> { "Apple", "Orange" };
        public string[] MyArray { get; set; }
        public GenericClass<string> GenericString { get; set; }
        public GenericClass<IList<decimal>> GenericListOfDecimal { get; set; }

        public FirstClass FirstClass { get; set; } = new FirstClass();
        public object DClass = new ChildEntity();
        public object DClass2 { get; set; }


        private bool BoolMethod()
        {
            return BoolText == BoolTestComparison;
        }

        private IList<string> GetFirstValue()
        {
            return null;
        }

        private void SetFirstValue(OtherEntity entity)
        {
            this.FirstValue = entity.FirstValue;
        }

        private void SetCollection(Collection<object> obj)
        {
            MyCollection = obj;
        }

        public static void SetDefaultState(string state)
        {
            DEFAULTSTATE = state;
        }

        private void SetGenericObject(GenericClass<string> obj)
        {
            GenericString = obj;
        }

        private void SetGenericObject(GenericClass<IList<decimal>> obj)
        {
            GenericListOfDecimal = obj;
        }
    }
}
