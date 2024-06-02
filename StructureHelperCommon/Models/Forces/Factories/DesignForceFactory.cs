using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;

namespace StructureHelperCommon.Models.Forces
{
    public enum ForceType
    {
        Force_zero,
        Force_Mx50My50Nz100,
    }

    public static class DesignForceFactory
    {
        public static IDesignForceTuple GetDesignForce(ForceType forceType, LimitStates limitState, CalcTerms calcTerm)
        {
            if (forceType == ForceType.Force_zero)
            {
                return new DesignForceTuple(limitState, calcTerm);
            }
            else if (forceType == ForceType.Force_Mx50My50Nz100)
            {
                var tuple = new DesignForceTuple(limitState, calcTerm);
                var forceTuple = tuple.ForceTuple;
                forceTuple.Mx = -50e3d;
                forceTuple.My = -50e3d;
                forceTuple.Nz = -100e3d;
                return tuple;
            }
            else throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown);
        }
    }
}
