using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Services.Units;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace StructureHelper.Infrastructure.UI.Converters.Units
{
    internal class Stress : UnitBase
    {
        public override UnitTypes UnitType { get => UnitTypes.Stress; }
        public override IUnit CurrentUnit { get => CommonOperation.GetUnit(UnitType, "MPa"); }
        public override string UnitName { get => "Stress"; }
    }
}
