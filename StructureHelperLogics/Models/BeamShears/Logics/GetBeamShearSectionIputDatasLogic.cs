using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;

namespace StructureHelperLogics.Models.BeamShears
{
    public class GetBeamShearSectionIputDatasLogic : IGetBeamShearSectionIputDatasLogic
    {
        private const LimitStates CollapseLimitState = LimitStates.ULS;
        private IDirectShearForceLogicInputData directShearForceLogicInputData;
        private (double Compressive, double Tensile) strength;

        public IBeamShearAction Action { get; set; }
        public CalcTerms CalcTerm { get; set; }
        public IBeamShearSection Section { get; set; }
        public IStirrup Stirrup { get; set; }
        public List<IInclinedSection> InclinedSectionList { get; set; }

        public List<IBeamShearSectionLogicInputData> GetBeamShearSectionInputDatas()
        {
            List<IBeamShearSectionLogicInputData> sectionInputDatas = new();
            var material = Section.ConcreteMaterial;
            strength = material.GetStrength(CollapseLimitState, CalcTerm);
            foreach (var inclinedSection in InclinedSectionList)
            {
                PrepareInclinedSection(inclinedSection);
                BeamShearSectionLogicInputData newInputData = GetNewInputData(inclinedSection);
                sectionInputDatas.Add(newInputData);
            }

            return sectionInputDatas;
        }

        private void PrepareInclinedSection(IInclinedSection inclinedSection)
        {
            inclinedSection.LimitState = CollapseLimitState;
            inclinedSection.CalcTerm = CalcTerm;
            inclinedSection.ConcreteCompressionStrength = strength.Compressive;
            inclinedSection.ConcreteTensionStrength = strength.Tensile;
        }

        private BeamShearSectionLogicInputData GetNewInputData(IInclinedSection inclinedSection)
        {
            IForceTuple forceTuple = GetForceTupleByShearAction(inclinedSection);
            BeamShearSectionLogicInputData beamShearSectionLogicInputData = new(Guid.NewGuid())
            {
                InclinedSection = inclinedSection,
                BeamShearAction = Action,
                BeamShearSection = Section,
                Stirrup = Stirrup,
                ForceTuple = forceTuple,
                LimitState = CollapseLimitState,
                CalcTerm = CalcTerm
            };
            BeamShearSectionLogicInputData newInputData = beamShearSectionLogicInputData;
            return newInputData;
        }

        private IForceTuple GetForceTupleByShearAction(IInclinedSection inclinedSection)
        {
            directShearForceLogicInputData = new DirectShearForceLogicInputData()
            {
                BeamShearAction = Action,
                InclinedSection = inclinedSection,
                LimitState = CollapseLimitState,
                CalcTerm = CalcTerm,
            };
            IGetDirectShearForceLogic getDirectShearForceLogic = new GetDirectShearForceLogic(directShearForceLogicInputData, null);
            return getDirectShearForceLogic.CalculateShearForceTuple();
        }
    }
}
