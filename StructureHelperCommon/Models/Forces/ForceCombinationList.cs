using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    public class ForceCombinationList : IForceCombinationList
    {

        public string Name { get; set; }
        public Point2D ForcePoint { get; private set; }
        public List<IDesignForceTuple> DesignForces { get; private set; }

        public ForceCombinationList()
        {
            DesignForces = new List<IDesignForceTuple>();
            DesignForces.Add(new DesignForceTuple(LimitStates.ULS, CalcTerms.ShortTerm));
            DesignForces.Add(new DesignForceTuple(LimitStates.ULS, CalcTerms.LongTerm));
            DesignForces.Add(new DesignForceTuple(LimitStates.SLS, CalcTerms.ShortTerm));
            DesignForces.Add(new DesignForceTuple(LimitStates.SLS, CalcTerms.LongTerm));
        }
    }
}
