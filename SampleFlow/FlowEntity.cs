using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
//Resubmit

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
        public string SecondValue { get; set; }
        public string ThirdValue { get; set; }
        public int FourthValue { get; set; }
        public int[] TheParams;

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

        private void SetValues(string firstValue, string secondValue, string thirdValue = "", int fourthValue = 0, params int[] theParams)
        {
            this.FirstValue = firstValue;
            this.SecondValue = secondValue;
            this.ThirdValue = thirdValue;
            this.FourthValue = fourthValue;
            this.TheParams = theParams;
        }

        private void SetValuesWithoutParams(string firstValue, string secondValue, string thirdValue = "", int fourthValue = 0)
        {
            this.FirstValue = firstValue;
            this.SecondValue = secondValue;
            this.ThirdValue = thirdValue;
            this.FourthValue = fourthValue;
        }

        private void SetMoreValues(string firstValue, string secondValue)
        {
            this.FirstValue = firstValue;
            this.SecondValue = secondValue;
        }

        private void SetValues(OtherEntity otherEntity)
        {
            this.FirstValue = otherEntity.FirstValue;
            this.SecondValue = otherEntity.SecondValue;
            this.ThirdValue = otherEntity.ThirdValue;
            this.FourthValue = otherEntity.FourthValue;
            this.TheParams = otherEntity.TheParams;
        }

        private void SetValues(YetAnotherEntity otherEntity)
        {
            this.FirstValue = otherEntity.FirstValue;
            this.SecondValue = otherEntity.SecondValue;
        }

        private void SetValues(EntityWithoutParams otherEntity)
        {
            this.FirstValue = otherEntity.FirstValue;
            this.SecondValue = otherEntity.SecondValue;
            this.ThirdValue = otherEntity.ThirdValue;
            this.FourthValue = otherEntity.FourthValue;
        }
    }
}
