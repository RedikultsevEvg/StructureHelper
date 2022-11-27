using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Services.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Infrastructure.UI.Converters.Units
{
    internal class Moment : UnitBase
    {
        public override UnitTypes unitType { get => UnitTypes.Moment; }
        public override IUnit currentUnit { get => CommonOperation.GetUnit(unitType, "kNm"); }
        public override string unitName { get => "Moment"; }
    }
}
