using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Shapes;

namespace StructureHelperLogics.Models.BeamShears
{
    internal class StirrupStrengthLogic : IBeamShearStrengthLogic
    {
        private readonly IBeamShearSectionLogicInputData inputData;
        private IStirrup stirrup => inputData.Stirrup;
        private IInclinedSection inclinedSection => inputData.InclinedCrack;
        private IBeamShearStrengthLogic stirrupByDensityStrengthLogic;
        private IBeamShearStrengthLogic stirrupGroupStrengthLogic;
        private IBeamShearStrengthLogic stirrupByInclinedRebarStrengthLogic;
        private IStirrupEffectiveness stirrupEffectiveness;

        public StirrupStrengthLogic(IBeamShearSectionLogicInputData inputData, IShiftTraceLogger? traceLogger)
        {
            this.inputData = inputData;
            TraceLogger = traceLogger;
        }

        public IShiftTraceLogger? TraceLogger { get; set; }

        public double CalculateShearStrength()
        {
            GetStirrupEffectiveness();
            if (stirrup is IStirrupByRebar stirrupByRebar)
            {
                TraceLogger?.AddMessage($"Stirrups type is stirrup by rebar Name = {stirrupByRebar.Name}");
                stirrupByDensityStrengthLogic = new StirrupByRebarStrengthLogic(stirrupEffectiveness, stirrupByRebar, inclinedSection, inputData.ForceTuple, TraceLogger);
                return stirrupByDensityStrengthLogic.CalculateShearStrength();
            }
            else if (stirrup is IStirrupGroup stirrupGroup)
            {
                TraceLogger?.AddMessage($"Stirrups type is stirrup group Name = {stirrupGroup.Name}");
                stirrupGroupStrengthLogic ??= new StirrupGroupStrengthLogic(inputData, stirrupGroup, TraceLogger);
                return stirrupGroupStrengthLogic.CalculateShearStrength();
            }
            else if (stirrup is IStirrupByDensity stirrupByDensity)
            {
                TraceLogger?.AddMessage($"Stirrups type is stirrup by density Name = {stirrupByDensity.Name}");
                stirrupByDensityStrengthLogic = new StirrupByDensityStrengthLogic(stirrupEffectiveness, stirrupByDensity, inclinedSection, TraceLogger);
                return stirrupByDensityStrengthLogic.CalculateShearStrength();
            }
            else if (stirrup is IStirrupByInclinedRebar inclinedRebar)
            {
                TraceLogger?.AddMessage($"Stirrups type is inclined rebar Name = {inclinedRebar.Name}");
                stirrupByInclinedRebarStrengthLogic ??= new StirrupByInclinedRebarStrengthLogic(inclinedSection, inclinedRebar, TraceLogger);
                return stirrupByInclinedRebarStrengthLogic.CalculateShearStrength();
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(stirrup));
            }

        }

        private void GetStirrupEffectiveness()
        {
            if (inclinedSection.BeamShearSection.Shape is IRectangleShape)
            {
                stirrupEffectiveness = StirrupEffectivenessFactory.GetEffectiveness(BeamShearSectionType.Rectangle);
            }
            else if (inclinedSection.BeamShearSection.Shape is ICircleShape)
            {
                stirrupEffectiveness = StirrupEffectivenessFactory.GetEffectiveness(BeamShearSectionType.Circle);

            }
        }
    }
}
