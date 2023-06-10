using StructureHelperCommon.Infrastructures.Enums;
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
        public ForceCombinationByFactor()
        {
            Name = "New Factored Load";
            SetInGravityCenter = true;
            ForcePoint = new Point2D();
            FullSLSForces = new ForceTuple();
            LongTermFactor = 1d;
            ULSFactor = 1.2d;
        }
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
            ForceActionService.CopyActionProps(this, newItem);
            newItem.FullSLSForces = FullSLSForces.Clone() as IForceTuple;
            newItem.LongTermFactor = LongTermFactor;
            newItem.ULSFactor = ULSFactor;
            return newItem;
        }
    }
}
