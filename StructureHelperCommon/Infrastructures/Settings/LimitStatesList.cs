using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace StructureHelperCommon.Infrastructures.Settings
{
    public class LimitStatesList
    {
        public List<LimitStateEntity> LimitStates { get; private set; }
        public LimitStatesList()
        {
            LimitStates = new List<LimitStateEntity>()
            {
               new LimitStateEntity()
                   {LimitState = Enums.LimitStates.ULS,
                   Name = "Ultimate limit state",
                   ShortName = "ULS",
                   Color = (Color)ColorConverter.ConvertFromString("Red")},
               new LimitStateEntity()
                   {LimitState = Enums.LimitStates.SLS,
                   Name = "Serviceability limit state",
                   ShortName = "SLS",
                   Color = (Color)ColorConverter.ConvertFromString("Green")}
            };
        }
    }
}
