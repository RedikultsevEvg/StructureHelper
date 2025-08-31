using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;

namespace StructureHelperLogics.Models.BeamShears
{
    public class GetSectionLogicFactory : IGetSectionLogicFactory
    {
        ShearCodeTypes shearCodeType;
        private IBeamShearSectionLogic sectionLogic;
        public IShiftTraceLogger? TraceLogger { get; set; }

        public GetSectionLogicFactory(IShiftTraceLogger? traceLogger)
        {
            TraceLogger = traceLogger;
        }

        public IBeamShearSectionLogic GetSectionLogic(ShearCodeTypes shearCodeType)
        {
            this.shearCodeType = shearCodeType;
            if (shearCodeType == ShearCodeTypes.SP_63_13330_2018_3)
            {
                GetSPLogic();
            }
            else if (shearCodeType == ShearCodeTypes.StructureHelper_0)
            {
                GetSHLogic();
            }
            else
            {
                string errorString = ErrorStrings.ObjectTypeIsUnknownObj(shearCodeType) + $": design code type {shearCodeType} for shear calculation is unknown";
                TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
                throw new StructureHelperException(errorString);
            }
            return sectionLogic;
        }

        private void GetSHLogic()
        {
            BeamShearSectionLogic logic = GetBaseLogic();
            logic.RestrictStirrupCalculator = new RestrictStirrupBySearchCalculator();
            sectionLogic = logic;
        }
        private void GetSPLogic()
        {
            BeamShearSectionLogic logic = GetBaseLogic();
            logic.RestrictStirrupCalculator = new RestrictStirrupByValueCalculator();
            sectionLogic = logic;
        }

        private BeamShearSectionLogic GetBaseLogic()
        {
            BeamShearSectionLogic logic = new(TraceLogger);
            logic.ShearCodeType = shearCodeType;
            logic.GetSectionEffectivenessLogic = new GetSectionEffectivenessLogic();
            return logic;
        }



    }
}
