using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Shapes;

namespace StructureHelperLogics.Models.BeamShears
{
    internal class StirrupStrengthLogic : IBeamShearStrenghLogic
    {
        private readonly IBeamShearSectionLogicInputData inputData;
        private IStirrup stirrup => inputData.Stirrup;
        private IInclinedSection inclinedSection => inputData.InclinedSection;
        private IBeamShearStrenghLogic stirrupByDensityStrengthLogic;
        private IBeamShearStrenghLogic stirrupGroupStrengthLogic;
        private IBeamShearStrenghLogic stirrupByInclinedRebarStrengthLogic;
        private IStirrupEffectiveness stirrupEffectiveness;

        public StirrupStrengthLogic(IBeamShearSectionLogicInputData inputData, IShiftTraceLogger? traceLogger)
        {
            this.inputData = inputData;
            TraceLogger = traceLogger;
        }

        public IShiftTraceLogger? TraceLogger { get; set; }

        public double GetShearStrength()
        {
            GetStirrupEffectiveness();
            if (stirrup is IStirrupByRebar stirrupByRebar)
            {
                TraceLogger?.AddMessage($"Stirrups type is stirrup by rebar Name = {stirrupByRebar.Name}");
                stirrupByDensityStrengthLogic = new StirrupByRebarStrengthLogic(stirrupEffectiveness, stirrupByRebar, inclinedSection, inputData.ForceTuple, TraceLogger);
                return stirrupByDensityStrengthLogic.GetShearStrength();
            }
            else if (stirrup is IStirrupGroup stirrupGroup)
            {
                TraceLogger?.AddMessage($"Stirrups type is stirrup group Name = {stirrupGroup.Name}");
                stirrupGroupStrengthLogic ??= new StirrupGroupStrengthLogic(inputData, stirrupGroup, TraceLogger);
                return stirrupGroupStrengthLogic.GetShearStrength();
            }
            else if (stirrup is IStirrupByDensity stirrupByDensity)
            {
                TraceLogger?.AddMessage($"Stirrups type is stirrup by density Name = {stirrupByDensity.Name}");
                stirrupByDensityStrengthLogic = new StirrupByDensityStrengthLogic(stirrupEffectiveness, stirrupByDensity, inclinedSection, TraceLogger);
                return stirrupByDensityStrengthLogic.GetShearStrength();
            }
            else if (stirrup is IStirrupByInclinedRebar inclinedRebar)
            {
                TraceLogger?.AddMessage($"Stirrups type is inclined rebar Name = {inclinedRebar.Name}");
                stirrupByInclinedRebarStrengthLogic ??= new StirrupByInclinedRebarStrengthLogic(inclinedSection, inclinedRebar, TraceLogger);
                return stirrupByInclinedRebarStrengthLogic.GetShearStrength();
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
