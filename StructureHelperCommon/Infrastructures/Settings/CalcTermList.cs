using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace StructureHelperCommon.Infrastructures.Settings
{
    public class CalcTermList
    {
        public List<CalcTermEntity> CalcTerms { get; private set; }
        public CalcTermList()
        {
            CalcTerms = new List<CalcTermEntity>()
            {
                new CalcTermEntity()
                {CalcTerm = Enums.CalcTerms.ShortTerm,
                   Name = "Short term",
                   ShortName = "Short term",
                   Color = (Color)ColorConverter.ConvertFromString("Red")},
                new CalcTermEntity()
                {CalcTerm = Enums.CalcTerms.LongTerm,
                   Name = "Long term",
                   ShortName = "Long term",
                   Color = (Color)ColorConverter.ConvertFromString("Green")}
            };
        }
    }
}
