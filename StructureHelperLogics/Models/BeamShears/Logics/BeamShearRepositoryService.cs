using StructureHelperCommon.Models.Forces;

namespace StructureHelperLogics.Models.BeamShears
{
    public static class BeamShearRepositoryService
    {
        public static void DeleteAction(IBeamShearRepository repository, IBeamShearAction action)
        {
            foreach (var calculator in repository.Calculators)
            {
                if (calculator is IBeamShearCalculator beamShearCalculator)
                {
                    var inputData = beamShearCalculator.InputData;
                    inputData.Actions.Remove(action);
                }
            }
            repository.Actions.Remove(action);
        }
        public static void DeleteSection(IBeamShearRepository repository, IBeamShearSection section)
        {
            foreach (var calculator in repository.Calculators)
            {
                if (calculator is IBeamShearCalculator beamShearCalculator)
                {
                    if (beamShearCalculator.InputData.Sections.Contains(section))
                    {
                        beamShearCalculator.InputData.Sections.Remove(section);
                    }
                }
            }
        }
        public static void DeleteStirrup(IBeamShearRepository repository, IStirrup stirrup)
        {
            foreach (var calculator in repository.Calculators)
            {
                if (calculator is IBeamShearCalculator beamShearCalculator)
                {
                    if (beamShearCalculator.InputData.Stirrups.Contains(stirrup))
                    {
                        beamShearCalculator.InputData.Stirrups.Remove(stirrup);
                    }
                }
            }
        }
    }
}
