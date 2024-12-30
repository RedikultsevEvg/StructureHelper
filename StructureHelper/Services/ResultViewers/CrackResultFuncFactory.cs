using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Services.Units;
using StructureHelperLogics.NdmCalculations.Cracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Services.ResultViewers
{
    
    public static class CrackResultFuncFactory
    {
        private static readonly IConvertUnitLogic operationLogic = new ConvertUnitLogic();
        private static readonly IGetUnitLogic UnitLogic = new GetUnitLogic();

        static IUnit unitStress = UnitLogic.GetUnit(UnitTypes.Stress);
        static IUnit unitLength = UnitLogic.GetUnit(UnitTypes.Length, "mm");

        public static List<CrackResultFunc> GetResultFuncs()
        {
            List<CrackResultFunc> results = new()
            {
                new()
                {
                    Name = "Long crack width",
                    ResultFunction = (IRebarCrackResult rebar) => rebar.LongTermResult.CrackWidth,
                    UnitFactor = unitLength.Multiplyer,
                    UnitName = unitLength.Name
                },
                new()
                {
                    Name = "Short crack width",
                    ResultFunction = (IRebarCrackResult rebar) => rebar.ShortTermResult.CrackWidth,
                    UnitFactor = unitLength.Multiplyer,
                    UnitName = unitLength.Name
                },
                new()
                {
                    Name = "Long softening factor",
                    ResultFunction = (IRebarCrackResult rebar) => rebar.LongTermResult.SofteningFactor,
                    UnitFactor = 1,
                    UnitName = "Dimensionless"
                },
                new()
                {
                    Name = "Short softening factor",
                    ResultFunction = (IRebarCrackResult rebar) => rebar.ShortTermResult.SofteningFactor,
                    UnitFactor = 1,
                    UnitName = "Dimensionless"
                },
                new()
                {
                    Name = "Long rebar stress",
                    ResultFunction = (IRebarCrackResult rebar) => rebar.LongTermResult.RebarStressResult.RebarStress,
                    UnitFactor = unitStress.Multiplyer,
                    UnitName = unitStress.Name
                },
                new()
                {
                    Name = "Short rebar stress",
                    ResultFunction = (IRebarCrackResult rebar) => rebar.ShortTermResult.RebarStressResult.RebarStress,
                    UnitFactor = unitStress.Multiplyer,
                    UnitName = unitStress.Name
                },
                new()
                {
                    Name = "Long rebar strain",
                    ResultFunction = (IRebarCrackResult rebar) => rebar.LongTermResult.RebarStressResult.RebarStrain,
                    UnitFactor = 1d,
                    UnitName = string.Empty
                },
                new()
                {
                    Name = "Short rebar strain",
                    ResultFunction = (IRebarCrackResult rebar) => rebar.ShortTermResult.RebarStressResult.RebarStrain,
                    UnitFactor = 1d,
                    UnitName = string.Empty
                },
                new()
                {
                    Name = "Long concrete strain",
                    ResultFunction = (IRebarCrackResult rebar) => rebar.LongTermResult.RebarStressResult.ConcreteStrain,
                    UnitFactor = 1d,
                    UnitName = string.Empty
                },
                new()
                {
                    Name = "Short concrete strain",
                    ResultFunction = (IRebarCrackResult rebar) => rebar.ShortTermResult.RebarStressResult.ConcreteStrain,
                    UnitFactor = 1d,
                    UnitName = string.Empty
                }
            };
            return results;
        }
    }
}
