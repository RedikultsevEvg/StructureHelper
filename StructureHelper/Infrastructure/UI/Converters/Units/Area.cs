using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Services.Units;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace StructureHelper.Infrastructure.UI.Converters.Units
{
    internal class Area : UnitBase
    {
        public override UnitTypes UnitType { get => UnitTypes.Area; }
        public override IUnit CurrentUnit { get => UnitLogic.GetUnit(UnitType, "mm2"); }
        public override string UnitName { get => "Area"; }
    }
}
