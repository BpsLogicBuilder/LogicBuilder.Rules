using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace UnitTests.Uwp
{
    public static class Helpers
    {
        public static string GetResourceString(this string id)
        {
            ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView();
            return resourceLoader.GetString(id);
        }
    }
}
