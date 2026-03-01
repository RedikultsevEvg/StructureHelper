using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Services;
using StructureHelperCommon.Services.Units;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelper.Infrastructure.UI.Converters.Units
{
    internal class FractureEnergy : UnitBase
    {

        public override UnitTypes UnitType { get => UnitTypes.FractureEnergy; }
        public override IUnit CurrentUnit { get => UnitLogic.GetUnit(UnitType, "N/mm"); }
        public override string UnitName { get => "FractureEnergy"; }
        public FractureEnergy()
        {
            OperationLogic = new ConvertUnitLogic()
            {
                MathRoundLogic = new SmartRoundLogic()
                {
                    DigitQuant = 3
                }
            };
        }
    }
}
