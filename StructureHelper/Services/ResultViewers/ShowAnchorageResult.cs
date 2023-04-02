using FieldVisualizer.Entities.Values.Primitives;
using LoaderCalculator.Data.Matrix;
using StructureHelper.Infrastructure.UI.Converters.Units;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperLogics.NdmCalculations.Analyses.RC;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;

namespace StructureHelper.Services.ResultViewers
{
    public static class ShowAnchorageResult
    {
        internal static List<IPrimitiveSet> GetPrimitiveSets(IStrainMatrix strainMatrix, LimitStates limitState, CalcTerms calcTerm, IEnumerable<INdmPrimitive> ndmPrimitives)
        {
            var primitiveSets = new List<IPrimitiveSet>();
            PrimitiveSet primitiveSet;
            primitiveSet = GetBaseDevelopmentLength(strainMatrix, limitState, calcTerm, ndmPrimitives);
            primitiveSets.Add(primitiveSet);
            primitiveSet = GetDevelopmentLength(strainMatrix, limitState, calcTerm, ndmPrimitives, true);
            primitiveSet.Name = "Development length full strength";
            primitiveSets.Add(primitiveSet);
            primitiveSet = GetDevelopmentLength(strainMatrix, limitState, calcTerm, ndmPrimitives,false);
            primitiveSet.Name = "Development length actual stress";
            primitiveSets.Add(primitiveSet);
            primitiveSet = GetFullStrengthLapLength(strainMatrix, limitState, calcTerm, ndmPrimitives, 0.5d, true);
            primitiveSet.Name = "Lapping length full strength, r=50%";
            primitiveSets.Add(primitiveSet);
            primitiveSet = GetFullStrengthLapLength(strainMatrix, limitState, calcTerm, ndmPrimitives, 1d, true);
            primitiveSet.Name = "Lapping length full strength, r=100%";
            primitiveSets.Add(primitiveSet);
            primitiveSet = GetFullStrengthLapLength(strainMatrix, limitState, calcTerm, ndmPrimitives, 0.5d, false);
            primitiveSet.Name = "Lapping length actual stress, r=50%";
            primitiveSets.Add(primitiveSet);
            primitiveSet = GetFullStrengthLapLength(strainMatrix, limitState, calcTerm, ndmPrimitives, 1d, false);
            primitiveSet.Name = "Lapping length actual stress, r=100%";
            primitiveSets.Add(primitiveSet);
            primitiveSet = GetStrength(strainMatrix, limitState, calcTerm, ndmPrimitives, true);
            primitiveSet.Name = "Full strength";
            primitiveSets.Add(primitiveSet);
            primitiveSet = GetStrength(strainMatrix, limitState, calcTerm, ndmPrimitives, false);
            primitiveSet.Name = "Actual stress";
            primitiveSets.Add(primitiveSet);
            return primitiveSets;
        }

        private static PrimitiveSet GetStrength(IStrainMatrix strainMatrix, LimitStates limitState, CalcTerms calcTerm, IEnumerable<INdmPrimitive> ndmPrimitives, bool fullStrength)
        {
            PrimitiveSet primitiveSet = new PrimitiveSet();
            List<IValuePrimitive> primitives = new List<IValuePrimitive>();
            foreach (var item in ndmPrimitives)
            {
                if (item is ReinforcementPrimitive)
                {
                    var primitive = item as ReinforcementPrimitive;
                    var inputData = InputDataFactory.GetInputData(primitive, strainMatrix, limitState, calcTerm, 1d);
                    if (fullStrength == true)
                    {
                        inputData.ReinforcementStress = inputData.ReinforcementStrength * Math.Sign(inputData.ReinforcementStress);
                    }
                    var val = inputData.ReinforcementStress * UnitConstants.Stress;
                    var valuePrimitive = GetValuePrimitive(primitive, val);
                    primitives.Add(valuePrimitive);
                }
            }
            primitiveSet.ValuePrimitives = primitives;
            return primitiveSet;
        }

        private static PrimitiveSet GetBaseDevelopmentLength(IStrainMatrix strainMatrix, LimitStates limitState, CalcTerms calcTerm, IEnumerable<INdmPrimitive> ndmPrimitives)
        {
            PrimitiveSet primitiveSet = new PrimitiveSet() { Name = "Base Development Length"};
            List<IValuePrimitive> primitives = new List<IValuePrimitive>();
            foreach (var item in ndmPrimitives)
            {
                if (item is ReinforcementPrimitive)
                {
                    var primitive = item as ReinforcementPrimitive;
                    var inputData = InputDataFactory.GetInputData(primitive, strainMatrix, limitState, calcTerm, 1d);
                    var calculator = new AnchorageCalculator(inputData);
                    var val = calculator.GetBaseDevLength() * UnitConstants.Length;
                    var valuePrimitive = GetValuePrimitive(primitive, val);
                    primitives.Add(valuePrimitive);
                }
            }
            primitiveSet.ValuePrimitives = primitives;
            return primitiveSet;
        }
        private static PrimitiveSet GetDevelopmentLength(IStrainMatrix strainMatrix, LimitStates limitState, CalcTerms calcTerm, IEnumerable<INdmPrimitive> ndmPrimitives, bool fullStrength)
        {
            PrimitiveSet primitiveSet = new PrimitiveSet();
            List<IValuePrimitive> primitives = new List<IValuePrimitive>();
            foreach (var item in ndmPrimitives)
            {
                if (item is ReinforcementPrimitive)
                {
                    var primitive = item as ReinforcementPrimitive;
                    var inputData = InputDataFactory.GetInputData(primitive, strainMatrix, limitState, calcTerm, 1d);
                    if (fullStrength == true)
                    {
                        inputData.ReinforcementStress = inputData.ReinforcementStrength * Math.Sign(inputData.ReinforcementStress);
                    }
                    var calculator = new AnchorageCalculator(inputData);
                    var val = calculator.GetDevLength() * UnitConstants.Length;
                    var valuePrimitive = GetValuePrimitive(primitive, val);
                    primitives.Add(valuePrimitive);
                }
            }
            primitiveSet.ValuePrimitives = primitives;
            return primitiveSet;
        }

        private static PrimitiveSet GetFullStrengthLapLength(IStrainMatrix strainMatrix, LimitStates limitState, CalcTerms calcTerm, IEnumerable<INdmPrimitive> ndmPrimitives, double lapperdCountRate, bool fullStrength)
        {
            PrimitiveSet primitiveSet = new PrimitiveSet();
            List<IValuePrimitive> primitives = new List<IValuePrimitive>();
            foreach (var item in ndmPrimitives)
            {
                if (item is ReinforcementPrimitive)
                {
                    var primitive = item as ReinforcementPrimitive;
                    var inputData = InputDataFactory.GetInputData(primitive, strainMatrix, limitState, calcTerm, lapperdCountRate);
                    if (fullStrength == true)
                    {
                        inputData.ReinforcementStress = inputData.ReinforcementStrength * Math.Sign(inputData.ReinforcementStress);
                    }
                    var calculator = new AnchorageCalculator(inputData);
                    var val = calculator.GetLapLength() * UnitConstants.Length;
                    var valuePrimitive = GetValuePrimitive(primitive, val);
                    primitives.Add(valuePrimitive);
                }
            }
            primitiveSet.ValuePrimitives = primitives;
            return primitiveSet;
        }

        private static FieldVisualizer.Entities.Values.Primitives.CirclePrimitive GetValuePrimitive(IPointPrimitive primitive, double val)
        {
            var valuePrimitive = new FieldVisualizer.Entities.Values.Primitives.CirclePrimitive()
            {
                CenterX = primitive.CenterX,
                CenterY = primitive.CenterY,
                Diameter = Math.Sqrt(primitive.Area / Math.PI) * 2,
                Value = val
            };
            return valuePrimitive;
        }
    }
}
