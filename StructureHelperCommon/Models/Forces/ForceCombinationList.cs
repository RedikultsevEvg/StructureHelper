using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services.Forces;

namespace StructureHelperCommon.Models.Forces
{
    /// <inheritdoc/>
    public class ForceCombinationList : IForceCombinationList
    {
        /// <inheritdoc/>
        public string Name { get; set; }
        /// <inheritdoc/>
        public bool SetInGravityCenter { get; set; }
        /// <inheritdoc/>
        public IPoint2D ForcePoint { get; set; }
        /// <inheritdoc/>
        public List<IDesignForceTuple> DesignForces { get; private set; }
        
        public ForceCombinationList()
        {
            SetInGravityCenter = true;
            ForcePoint = new Point2D() { X = 0, Y = 0 };
            DesignForces = new List<IDesignForceTuple>
            {
                new DesignForceTuple(LimitStates.ULS, CalcTerms.ShortTerm),
                new DesignForceTuple(LimitStates.ULS, CalcTerms.LongTerm),
                new DesignForceTuple(LimitStates.SLS, CalcTerms.ShortTerm),
                new DesignForceTuple(LimitStates.SLS, CalcTerms.LongTerm)
            };
        }
        /// <inheritdoc/>
        public object Clone()
        {
            var newItem = new ForceCombinationList();
            ForceActionService.CopyActionProps(this, newItem);
            newItem.DesignForces.Clear();
            foreach (var item in DesignForces)
            {
                var newForce = item.Clone() as IDesignForceTuple;
                newItem.DesignForces.Add(newForce);
            }
            return newItem;
        }
        /// <inheritdoc/>
        public IForceCombinationList GetCombinations()
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
                        designForceTuple.ForceTuple = ForceTupleService.SumTuples(designForceTuple.ForceTuple, item.ForceTuple);
                    }
                    result.DesignForces.Add(designForceTuple);
                }
            }
            return result;
        }
    }
}
