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
        public override UnitTypes unitType { get => UnitTypes.Area; }
        public override IUnit currentUnit { get => CommonOperation.GetUnit(unitType, "mm2"); }
        public override string unitName { get => "Area"; }
    }
}
