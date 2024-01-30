using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models.Shapes;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces.LimitCurve
{
    public enum ForceCurveLogic
    {
        N_Mx,
        N_My,
        Mx_My
    }
    public static class ConvertLogicFactory
    {
        public static ConstOneDirectionConverter GetLogic(ForceCurveLogic logic)
        {
            var strings = ProgramSetting.GeometryNames;
            if (logic == ForceCurveLogic.N_Mx)
            {
                return new ConstOneDirectionConverter(Directions.Y, Guid.Parse("4d8637b6-98bc-4e7f-8006-01fc679c21ee"))
                {
                    Name = "N-Mx",
                    XAxisName = strings.FullMomFstName,
                    YAxisName = strings.FullLongForceName,
                    ConstAxisName = strings.FullMomSndName,
                    XForceType = ForceTypes.MomentMx,
                    YForceType = ForceTypes.Force,
                    ZForceType = ForceTypes.MomentMy,
                };
            }
            else if (logic == ForceCurveLogic.N_My)
            {
                return new ConstOneDirectionConverter(Directions.X, Guid.Parse("119ff666-7121-4a34-b76c-5ada3bd490f3"))
                {
                    Name = "N-My",
                    XAxisName = strings.FullMomSndName,
                    YAxisName = strings.FullLongForceName,
                    ConstAxisName = strings.FullMomFstName,
                    XForceType = ForceTypes.MomentMy,
                    YForceType = ForceTypes.Force,
                    ZForceType = ForceTypes.MomentMx,
                };
            }
            else if (logic == ForceCurveLogic.Mx_My)
            {
                return new ConstOneDirectionConverter(Directions.Z, Guid.Parse("58020401-bedf-4b79-9131-86f7078d6c49"))
                {
                    Name = "Mx-My",
                    XAxisName = strings.FullMomFstName,
                    YAxisName = strings.FullMomSndName,
                    ConstAxisName = strings.FullLongForceName,
                    XForceType = ForceTypes.MomentMx,
                    YForceType = ForceTypes.MomentMy,
                    ZForceType = ForceTypes.Force,
                };
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(logic));
            }
        }
    }
}
