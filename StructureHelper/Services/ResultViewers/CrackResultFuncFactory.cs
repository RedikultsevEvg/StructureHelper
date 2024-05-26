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
        static IUnit unitStress = CommonOperation.GetUnit(UnitTypes.Stress);
        static IUnit unitLength = CommonOperation.GetUnit(UnitTypes.Length, "mm");

        public static List<CrackResultFunc> GetResultFuncs()
        {
            List<CrackResultFunc> results = new()
            {
                new()
                {
                    Name = "Long crack width",
                    ResultFunction = (RebarCrackResult rebar) => rebar.LongTermResult.CrackWidth,
                    UnitFactor = unitLength.Multiplyer,
                    UnitName = unitLength.Name
                },
                new()
                {
                    Name = "Short crack width",
                    ResultFunction = (RebarCrackResult rebar) => rebar.ShortTermResult.CrackWidth,
                    UnitFactor = unitLength.Multiplyer,
                    UnitName = unitLength.Name
                },
                new()
                {
                    Name = "Long softening factor",
                    ResultFunction = (RebarCrackResult rebar) => rebar.LongTermResult.SofteningFactor,
                    UnitFactor = unitLength.Multiplyer,
                    UnitName = unitLength.Name
                },
                new()
                {
                    Name = "Short softening factor",
                    ResultFunction = (RebarCrackResult rebar) => rebar.ShortTermResult.SofteningFactor,
                    UnitFactor = unitLength.Multiplyer,
                    UnitName = unitLength.Name
                },
                new()
                {
                    Name = "Long rebar stress",
                    ResultFunction = (RebarCrackResult rebar) => rebar.LongTermResult.RebarStressResult.RebarStress,
                    UnitFactor = unitStress.Multiplyer,
                    UnitName = unitStress.Name
                },
                new()
                {
                    Name = "Short rebar stress",
                    ResultFunction = (RebarCrackResult rebar) => rebar.ShortTermResult.RebarStressResult.RebarStress,
                    UnitFactor = unitStress.Multiplyer,
                    UnitName = unitStress.Name
                },
                new()
                {
                    Name = "Long rebar strain",
                    ResultFunction = (RebarCrackResult rebar) => rebar.LongTermResult.RebarStressResult.RebarStrain,
                    UnitFactor = 1d,
                    UnitName = string.Empty
                },
                new()
                {
                    Name = "Short rebar strain",
                    ResultFunction = (RebarCrackResult rebar) => rebar.ShortTermResult.RebarStressResult.RebarStrain,
                    UnitFactor = 1d,
                    UnitName = string.Empty
                },
                new()
                {
                    Name = "Long concrete strain",
                    ResultFunction = (RebarCrackResult rebar) => rebar.LongTermResult.RebarStressResult.ConcreteStrain,
                    UnitFactor = 1d,
                    UnitName = string.Empty
                },
                new()
                {
                    Name = "Short concrete strain",
                    ResultFunction = (RebarCrackResult rebar) => rebar.ShortTermResult.RebarStressResult.ConcreteStrain,
                    UnitFactor = 1d,
                    UnitName = string.Empty
                }
            };
            return results;
        }
    }
}
