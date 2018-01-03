using System;
using System.Collections.Generic;
using System.Text;

namespace SampleFlow
{
    public class OtherEntity 
    {
        public OtherEntity(string firstValue, string secondValue)
        {
            this.FirstValue = firstValue;
            this.SecondValue = secondValue;
        }

        public string FirstValue { get; set; }
        public string SecondValue { get; set; }
    }
}
