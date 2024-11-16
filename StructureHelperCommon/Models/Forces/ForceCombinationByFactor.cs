using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    /// <inheritdoc/>
    public class ForceCombinationByFactor : IForceCombinationByFactor
    {
        readonly IUpdateStrategy<IAction> updateStrategy = new ActionUpdateStrategy();
        private ForceCombinationList result;
        private List<LimitStates> limitStates;
        private List<CalcTerms> calcTerms;

        /// <inheritdoc/>
        public Guid Id { get; }
        /// <inheritdoc/>
        public string Name { get; set; } = "New Factored Load";
        /// <inheritdoc/>
        public LimitStates LimitState { get; set; } = LimitStates.SLS; //By default create characteristic value of forces
        /// <inheritdoc/>
        public CalcTerms CalcTerm { get; set; } = CalcTerms.ShortTerm; //By defult use full value of load
        public bool SetInGravityCenter { get; set; } = true;
        /// <inheritdoc/>
        public IPoint2D ForcePoint { get; set; } = new Point2D();
        /// <inheritdoc/>
        public IForceTuple FullSLSForces { get; set; } = new ForceTuple();
        /// <inheritdoc/>
        public double ULSFactor { get; set; } = 1.2d;
        /// <inheritdoc/>
        public double LongTermFactor { get; set; } = 1d;

        public ForceCombinationByFactor(Guid id)
        {
            Id = id;
        }
        public ForceCombinationByFactor() : this (Guid.NewGuid()) { }
        public IForceCombinationList GetCombination()
        {
            GetNewResult();
            ProcessResult();
            return result;
        }

        private void ProcessResult()
        {
            foreach (var limitState in limitStates)
            {
                ProcessLimitState(limitState);
            }
        }

        private void ProcessLimitState(LimitStates limitState)
        {
            var stateFactor = limitState is LimitStates.SLS ? 1d : ULSFactor;
            foreach (var calcTerm in calcTerms)
            {
                ProcessCalcTerm(limitState, stateFactor, calcTerm);
            }
        }

        private void ProcessCalcTerm(LimitStates limitState, double stateFactor, CalcTerms calcTerm)
        {
            var termFactor = calcTerm is CalcTerms.ShortTerm ? 1d : LongTermFactor;
            var designForceTuple = new DesignForceTuple()
            {
                LimitState = limitState,
                CalcTerm = calcTerm
            };
            designForceTuple.ForceTuple = ForceTupleService.MultiplyTuples(FullSLSForces, stateFactor * termFactor) as ForceTuple;
            result.DesignForces.Add(designForceTuple);
        }

        private void GetNewResult()
        {
            result = new ForceCombinationList();
            result.SetInGravityCenter = SetInGravityCenter;
            result.ForcePoint = ForcePoint;
            result.DesignForces.Clear();
            limitStates = new List<LimitStates>()
            {
                LimitStates.ULS, LimitStates.SLS
            };
            calcTerms = new List<CalcTerms>()
            {
                CalcTerms.ShortTerm, CalcTerms.LongTerm
            };
        }

        public object Clone()
        {
            var newItem = new ForceCombinationByFactor();
            updateStrategy.Update(newItem, this);
            return newItem;
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
