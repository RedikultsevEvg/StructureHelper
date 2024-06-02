using StructureHelperCommon.Infrastructures.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Services.Units
{
    public static class ProcessString
    {
        public static double ConvertCommaToCultureSettings(string s)
        {
            double result;
            if (!double.TryParse(s, NumberStyles.Any, CultureInfo.CurrentCulture, out result) &&
                !double.TryParse(s, NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out result) &&
                !double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
            {
                throw new StructureHelperException($"{ErrorStrings.IncorrectValue}: {s}");
            }
            return result;
        }
    }
}
