using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Strings;
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
    internal class Length : UnitBase
    {
        public override UnitTypes unitType { get => UnitTypes.Length; }
        public override IUnit currentUnit { get => CommonOperation.GetUnit(unitType, "mm"); }
        public override string unitName { get => "Length"; }
    }
}
