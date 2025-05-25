using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears.Logics
{
    internal class StirrupStrengthLogic : IBeamShearStrenghLogic
    {
        private readonly IBeamShearSectionLogicInputData inputData;
        private IStirrup stirrup => inputData.Stirrup;
        private IInclinedSection inclinedSection => inputData.InclinedSection;
        private IBeamShearStrenghLogic stirrupDensityStrengthLogic;

        public StirrupStrengthLogic(IBeamShearSectionLogicInputData inputData, IShiftTraceLogger? traceLogger)
        {
            this.inputData = inputData;
            TraceLogger = traceLogger;
        }

        public IShiftTraceLogger? TraceLogger { get; set; }

        public double GetShearStrength()
        {
            var stirrupEffectiveness = StirrupEffectivenessFactory.GetEffectiveness(BeamShearSectionType.Rectangle);
            if (stirrup is IStirrupByDensity stirrupByDensity)
            {
                TraceLogger?.AddMessage("Stirrups type is stirrup by density");
                stirrupDensityStrengthLogic = new StirrupByDensityStrengthLogic(stirrupEffectiveness, stirrupByDensity, inclinedSection,TraceLogger);
                return stirrupDensityStrengthLogic.GetShearStrength();
            }
            else if (stirrup is IStirrupByRebar stirrupByRebar)
            {
                TraceLogger?.AddMessage("Stirrups type is stirrup by rebar");
                stirrupDensityStrengthLogic = new StirrupByRebarStrengthLogic(stirrupEffectiveness, stirrupByRebar, inclinedSection, inputData.ForceTuple, TraceLogger);
                return stirrupDensityStrengthLogic.GetShearStrength();
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(stirrup));
            }

        }
    }
}
