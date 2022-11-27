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
        public override UnitTypes unitType { get => UnitTypes.Stress; }
        public override IUnit currentUnit { get => CommonOperation.GetUnit(unitType, "MPa"); }
        public override string unitName { get => "Stress"; }
    }
}
