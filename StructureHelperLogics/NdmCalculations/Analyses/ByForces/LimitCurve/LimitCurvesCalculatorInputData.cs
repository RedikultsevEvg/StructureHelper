using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Parameters;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.LimitCurve;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Copyright (c) 2023 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class LimitCurvesCalculatorInputData : ILimitCurvesCalculatorInputData
    {
        private ICloneStrategy<ILimitCurvesCalculatorInputData> cloneStrategy = new LimitCurvesCalculatorInputDataCloneStrategy();
        public List<LimitStates> LimitStates { get; private set; } = new();
        public List<CalcTerms> CalcTerms { get; private set; } = new();
        public List<NamedCollection<INdmPrimitive>> PrimitiveSeries { get; private set; } = new();
        public List<PredicateEntry> PredicateEntries { get; private set; } = new();
        public ISurroundData SurroundData { get; set; } = new SurroundData();
        public int PointCount { get; set; } = 80;
        public LimitCurvesCalculatorInputData()
        {
        }
        public LimitCurvesCalculatorInputData(IEnumerable<INdmPrimitive> primitives)
        {
            PrimitiveSeries.Add
                (new NamedCollection<INdmPrimitive>()
                {
                    Name = "V1",
                    Collection = primitives.ToList()
                }
                );
        }

        public object Clone()
        {
            var newItem = cloneStrategy.GetClone(this);
            return newItem;

        }
    }
}
