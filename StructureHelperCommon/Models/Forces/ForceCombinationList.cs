using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services.Forces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StructureHelperCommon.Models.Forces
{
    /// <inheritdoc/>
    public class ForceCombinationList : IForceCombinationList
    {
        readonly IUpdateStrategy<IAction> updateStrategy = new ActionUpdateStrategy();
        /// <inheritdoc/>
        public Guid Id { get; }
        /// <inheritdoc/>
        public string Name { get; set; }
        /// <inheritdoc/>
        public bool SetInGravityCenter { get; set; } = true;
        /// <inheritdoc/>
        public IPoint2D ForcePoint { get; set; } = new Point2D();
        /// <inheritdoc/>
        public List<IDesignForceTuple> DesignForces { get; set; }


        public ForceCombinationList(Guid id)
        {
            Id = id;
            DesignForces = new List<IDesignForceTuple>
            {
                new DesignForceTuple()
                {
                    LimitState = LimitStates.ULS,
                    CalcTerm = CalcTerms.ShortTerm
                },
                new DesignForceTuple()
                {
                    LimitState = LimitStates.ULS,
                    CalcTerm = CalcTerms.LongTerm
                },
                new DesignForceTuple()
                {
                    LimitState = LimitStates.SLS,
                    CalcTerm = CalcTerms.ShortTerm
                },
                new DesignForceTuple()
                {
                    LimitState = LimitStates.SLS,
                    CalcTerm = CalcTerms.LongTerm
                }
            };
        }
        public ForceCombinationList() : this (Guid.NewGuid()) { }
        /// <inheritdoc/>
        public object Clone()
        {
            var newItem = new ForceCombinationList();
            updateStrategy.Update(newItem, this);
            return newItem;
        }
        /// <inheritdoc/>
        public IForceCombinationList GetCombination()
        {
            var result = Clone() as IForceCombinationList;
            result.DesignForces.Clear();
            var limitStates = new List<LimitStates>() { LimitStates.ULS, LimitStates.SLS };
            var calcTerms = new List<CalcTerms>() { CalcTerms.ShortTerm, CalcTerms.LongTerm };
            foreach (var limitState in limitStates)
            {
                foreach (var calcTerm in calcTerms)
                {
                    var designForceTuple = new DesignForceTuple() { LimitState = limitState, CalcTerm = calcTerm };
                    var forceTupleList = DesignForces.Where(x => x.LimitState == limitState & x.CalcTerm == calcTerm);
                    foreach (var item in forceTupleList)
                    {
                        designForceTuple.ForceTuple = ForceTupleService.SumTuples(designForceTuple.ForceTuple, item.ForceTuple) as ForceTuple;
                    }
                    result.DesignForces.Add(designForceTuple);
                }
            }
            return result;
        }

        public List<IForceCombinationList> GetCombinations()
        {
            var listResult = new List<IForceCombinationList>
            {
                GetCombination()
            };
            return listResult;
        }
    }
}
