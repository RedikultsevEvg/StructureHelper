using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Parameters;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces.Logics
{
    internal class LimitCurveInputDataUpdateStrategy : IUpdateStrategy<LimitCurveInputData>
    {
        SurroundDataUpdateStrategy surroundDataUpdateStrategy => new();
        public void Update(LimitCurveInputData targetObject, LimitCurveInputData sourceObject)
        {
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.LimitStates.Clear();
            targetObject.LimitStates.AddRange(sourceObject.LimitStates);
            targetObject.CalcTerms.Clear();
            targetObject.CalcTerms.AddRange(sourceObject.CalcTerms);
            targetObject.PredicateEntries.Clear();
            targetObject.PredicateEntries.AddRange(sourceObject.PredicateEntries);
            targetObject.PointCount = sourceObject.PointCount;
            targetObject.PrimitiveSeries.Clear();
            foreach (var item in sourceObject.PrimitiveSeries)
            {
                var newItem = new NamedCollection<Primitives.INdmPrimitive>()
                {
                    Name = item.Name
                };
                newItem.Collection.AddRange(item.Collection);
                targetObject.PrimitiveSeries.Add(newItem);
            }
            surroundDataUpdateStrategy.Update(targetObject.SurroundData, sourceObject.SurroundData);
        }
    }
}
