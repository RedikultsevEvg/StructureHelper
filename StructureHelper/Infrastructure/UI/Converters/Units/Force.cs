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
    internal class Force : UnitBase
    {
        public override UnitTypes UnitType { get => UnitTypes.Force; }
        public override IUnit CurrentUnit { get => UnitLogic.GetUnit(UnitType, "kN"); }
        public override string UnitName { get => "Force"; }
    }
}
