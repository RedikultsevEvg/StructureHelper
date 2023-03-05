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
        public IPoint2D ForcePoint { get; private set; }
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
        }
        public List<IDesignForceTuple> GetCombination()
        {
            var result = new List<IDesignForceTuple>();
            var limitStates = new List<LimitStates>() { LimitStates.ULS, LimitStates.SLS };
            var calcTerms = new List<CalcTerms>() { CalcTerms.ShortTerm, CalcTerms.LongTerm };
            foreach (var limitState in limitStates)
            {
                var stateFactor = limitState is LimitStates.SLS ? 1d : ULSFactor; 
                foreach (var calcTerm in calcTerms)
                {
                    var termFactor = calcTerm is CalcTerms.ShortTerm ? 1d : LongTermFactor;
                    var designForceTuple = new DesignForceTuple() { LimitState = limitState, CalcTerm = calcTerm };
                    designForceTuple.ForceTuple = ForceTupleService.MultiplyTuples(designForceTuple.ForceTuple, stateFactor * termFactor);
                    result.Add(designForceTuple);
                }
            }
            return result;
        }
    }
}
