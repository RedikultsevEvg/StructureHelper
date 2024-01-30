using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Settings;
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

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class LimitCurveInputData : IInputData, ICloneable
    {
        public List<LimitStates> LimitStates { get; private set; }
        public List<CalcTerms> CalcTerms { get; private set; }
        public List<NamedCollection<INdmPrimitive>> PrimitiveSeries {get; private set; }
        public List<PredicateEntry> PredicateEntries { get; private set; }
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

        public object Clone()
        {
            var newItem = new LimitCurveInputData()
            {
                LimitStates = LimitStates.ToList(),
                CalcTerms = CalcTerms.ToList(),
                PredicateEntries = PredicateEntries.ToList(),
                SurroundData = SurroundData.Clone() as SurroundData,
                PointCount = PointCount
            };
            foreach (var item in PrimitiveSeries)
            {
                var collection = item.Collection.ToList();
                newItem.PrimitiveSeries.Add
                    (
                    new NamedCollection<INdmPrimitive>()
                    {
                        Name = item.Name,
                        Collection = collection
                    }
                    );
            }
            return newItem;
            
        }
    }
}
