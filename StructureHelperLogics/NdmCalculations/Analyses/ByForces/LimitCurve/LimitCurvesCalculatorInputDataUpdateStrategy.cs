using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Parameters;
using StructureHelperCommon.Services;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces.LimitCurve
{
    public class LimitCurvesCalculatorInputDataUpdateStrategy : IUpdateStrategy<ILimitCurvesCalculatorInputData>
    {
        public void Update(ILimitCurvesCalculatorInputData targetObject, ILimitCurvesCalculatorInputData sourceObject)
        {
            CheckObject.IsNull(targetObject, sourceObject, "Limit curve calculator input data");
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.LimitStates.Clear();
            targetObject.CalcTerms.Clear();
            targetObject.PrimitiveSeries.Clear();
            targetObject.PredicateEntries.Clear();
            targetObject.LimitStates.AddRange(sourceObject.LimitStates);
            targetObject.CalcTerms.AddRange(sourceObject.CalcTerms);
            targetObject.PredicateEntries.AddRange(sourceObject.PredicateEntries);
            targetObject.SurroundData = sourceObject.SurroundData.Clone() as ISurroundData;
            targetObject.PointCount = sourceObject.PointCount;
            foreach (var item in sourceObject.PrimitiveSeries)
            {
                var collection = item.Collection.ToList();
                targetObject.PrimitiveSeries.Add
                    (
                    new NamedCollection<INdmPrimitive>()
                    {
                        Name = item.Name,
                        Collection = collection
                    }
                    );
            }
        }
    }
}
