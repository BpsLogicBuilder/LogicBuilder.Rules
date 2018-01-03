using System;
using System.Collections.Generic;
using System.Text;

namespace SampleFlow
{
    public class GenericClass<T>
    {
        public GenericClass(int Id, string VariableName, T CurrentValue, object QuestionData)
        {
            this.Id = Id;
            this.VariableName = VariableName;
            this.QuestionData = QuestionData;
            this.CurrentValue = CurrentValue;
        }

        public GenericClass()
        {
        }

        public int Id { get; set; }
        public string VariableName { get; set; }
        public object QuestionData { get; set; }

        public T CurrentValue { get; set; }

        public string Type
        {
            get { return typeof(T).FullName; }
        }
    }
}
