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
        public override UnitTypes unitType { get => UnitTypes.Force; }
        public override IUnit currentUnit { get => CommonOperation.GetUnit(unitType, "kN"); }
        public override string unitName { get => "Force"; }
    }
}
