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
        public override UnitTypes UnitType { get => UnitTypes.Length; }
        public override IUnit CurrentUnit { get => CommonOperation.GetUnit(UnitType, "mm"); }
        public override string UnitName { get => "Length"; }
    }
}
