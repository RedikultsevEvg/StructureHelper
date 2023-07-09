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
        /// <inheritdoc/>
        public Guid Id { get; }
        /// <inheritdoc/>
        public string Name { get; set; }
        public bool SetInGravityCenter { get; set; }
        /// <inheritdoc/>
        public IPoint2D ForcePoint { get; set; }
        /// <inheritdoc/>
        public IForceTuple FullSLSForces { get; private set; }
        /// <inheritdoc/>
        public double ULSFactor { get; set; }
        /// <inheritdoc/>
        public double LongTermFactor { get; set; }


        public ForceCombinationByFactor(Guid id)
        {
            Id = id;
            Name = "New Factored Load";
            SetInGravityCenter = true;
            ForcePoint = new Point2D();
            FullSLSForces = new ForceTuple();
            LongTermFactor = 1d;
            ULSFactor = 1.2d;
        }
        public ForceCombinationByFactor() : this (Guid.NewGuid()) { }
        public IForceCombinationList GetCombinations()
        {
            var result = new ForceCombinationList();
            result.SetInGravityCenter = this.SetInGravityCenter;
            result.ForcePoint = this.ForcePoint;
            result.DesignForces.Clear();
            var limitStates = new List<LimitStates>() { LimitStates.ULS, LimitStates.SLS };
            var calcTerms = new List<CalcTerms>() { CalcTerms.ShortTerm, CalcTerms.LongTerm };
            foreach (var limitState in limitStates)
            {
                var stateFactor = limitState is LimitStates.SLS ? 1d : ULSFactor; 
                foreach (var calcTerm in calcTerms)
                {
                    var termFactor = calcTerm is CalcTerms.ShortTerm ? 1d : LongTermFactor;
                    var designForceTuple = new DesignForceTuple() { LimitState = limitState, CalcTerm = calcTerm };
                    designForceTuple.ForceTuple = ForceTupleService.MultiplyTuples(FullSLSForces, stateFactor * termFactor) as ForceTuple;
                    result.DesignForces.Add(designForceTuple);
                }
            }
            return result;
        }

        public object Clone()
        {
            var newItem = new ForceCombinationByFactor();
            updateStrategy.Update(newItem, this);
            return newItem;
        }
    }
}
