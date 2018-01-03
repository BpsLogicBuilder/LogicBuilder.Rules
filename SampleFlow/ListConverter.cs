using System;
using System.Collections.Generic;
using System.Text;

namespace SampleFlow
{
    public static class ListConverter<T>
    {
        public static T[] StaticMethod(IList<T> list)
        {
            return System.Linq.Enumerable.ToArray<T>(list);
        }
    }
}
