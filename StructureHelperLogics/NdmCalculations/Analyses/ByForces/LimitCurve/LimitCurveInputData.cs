using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Parameters;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Copyright (c) 2023 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces.LimitCurve
{
    public class LimitCurveInputData : IInputData
    {
        public List<LimitStates> LimitStates { get; }
        public List<CalcTerms> CalcTerms { get; }
        public List<NamedCollection<INdmPrimitive>> PrimitiveSeries {get;}
        public List<PredicateEntry> PredicateEntries { get; }
        public SurroundData SurroundData { get; set; }
        public int PointCount { get; set; }
        public LimitCurveInputData()
        {
            LimitStates = new();
            CalcTerms = new();
            PredicateEntries = new();
            SurroundData = new();
            PointCount = 80;
            PrimitiveSeries = new List<NamedCollection<INdmPrimitive>>();
        }
        public LimitCurveInputData(IEnumerable<INdmPrimitive> primitives) : this()
        {
            PrimitiveSeries.Add
                (new NamedCollection<INdmPrimitive>()
                {
                    Name = "V1",
                    Collection = primitives.ToList()
                }
                );
        }
    }
}
