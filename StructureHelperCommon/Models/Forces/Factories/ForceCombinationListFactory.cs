using System.Collections.Generic;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;

namespace StructureHelperCommon.Models.Forces
{
    public enum DesignForceType
    {
        Suit_1,
        Suit_2
    }

    public static class ForceCombinationListFactory
    {
        public static List<IDesignForceTuple> GetDesignForces(DesignForceType forceType)
        {
            if (forceType == DesignForceType.Suit_1)
            {
                var designForces = new List<IDesignForceTuple>();
                designForces.Add(DesignForceFactory.GetDesignForce(ForceType.Force_Mx50My50Nz100, LimitStates.ULS, CalcTerms.ShortTerm));
                designForces.Add(DesignForceFactory.GetDesignForce(ForceType.Force_Mx50My50Nz100, LimitStates.ULS, CalcTerms.LongTerm));
                designForces.Add(DesignForceFactory.GetDesignForce(ForceType.Force_Mx50My50Nz100, LimitStates.SLS, CalcTerms.ShortTerm));
                designForces.Add(DesignForceFactory.GetDesignForce(ForceType.Force_Mx50My50Nz100, LimitStates.SLS, CalcTerms.LongTerm));
                return designForces;
            }
            else throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown);
        }
    }
}
