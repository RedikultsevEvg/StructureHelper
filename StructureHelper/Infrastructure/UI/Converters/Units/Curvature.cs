using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Services.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Infrastructure.UI.Converters.Units
{
    internal class Curvature : UnitBase
    {
        public override UnitTypes UnitType { get => UnitTypes.Curvature; }
        public override IUnit CurrentUnit { get => CommonOperation.GetUnit(UnitType, "1/mm"); }
        public override string UnitName { get => "Curvature"; }
    }
}
