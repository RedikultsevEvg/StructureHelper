using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Services;
using StructureHelperCommon.Services.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Infrastructure.UI.Converters.Units
{
    internal class CrackWidth : UnitBase
    {
        public CrackWidth()
        {
            OperationLogic = new ConvertUnitLogic()
            {
                MathRoundLogic = new FixedRoundLogic()
                {
                    DigitQuant = 3
                }
            };
        }
        public override UnitTypes UnitType { get => UnitTypes.Length; }
        public override IUnit CurrentUnit { get => UnitLogic.GetUnit(UnitType, "mm"); }
        public override string UnitName { get => "Length"; }
    }
}
