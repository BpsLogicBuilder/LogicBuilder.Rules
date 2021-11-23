using System;
using System.Collections.Generic;
using System.Text;

namespace SampleFlow
{
    public class OtherEntity
    {
        public OtherEntity(string firstValue, string secondValue, string thirdValue = "", int fourthValue = 0, params int[] theParams)
        {
            this.FirstValue = firstValue;
            this.SecondValue = secondValue;
            this.ThirdValue = thirdValue;
            this.FourthValue = fourthValue;
            this.TheParams = theParams;
        }

        public string FirstValue { get; set; }
        public string SecondValue { get; set; }
        public string ThirdValue { get; set; }
        public int FourthValue { get; set; }
        public int[] TheParams;
    }

    public class YetAnotherEntity
    {
        public YetAnotherEntity(string firstValue, string secondValue)
        {
            this.FirstValue = firstValue;
            this.SecondValue = secondValue;
        }

        public string FirstValue { get; set; }
        public string SecondValue { get; set; }
    }

    public class EntityWithoutParams
    {
        public EntityWithoutParams(string firstValue, string secondValue, string thirdValue = "", int fourthValue = 0)
        {
            this.FirstValue = firstValue;
            this.SecondValue = secondValue;
            this.ThirdValue = thirdValue;
            this.FourthValue = fourthValue;
        }

        public string FirstValue { get; set; }
        public string SecondValue { get; set; }
        public string ThirdValue { get; set; }
        public int FourthValue { get; set; }
    }
}
