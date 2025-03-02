using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    public class BeamShearRepository : IBeamShearRepository
    {

        public Guid Id { get; }

        public List<IBeamShearAction> BeamShearActions { get; } = new();

        public List<ICalculator> Calculators { get; } = new();

        public List<IBeamShearSection> ShearSections { get; } = new();

        public List<IStirrup> Stirrups { get; } = new();


        public BeamShearRepository(Guid id)
        {
            Id = id;
        }

        public void DeleteBeamShearAction(IBeamShearAction beamShearAction)
        {
            foreach (var calculator in Calculators)
            {
                if (calculator is IBeamShearCalculator beamShearCalculator)
                {
                    var inputData = beamShearCalculator.InputData;
                    inputData.BeamShearActions.Remove(beamShearAction);
                }
            }
            BeamShearActions.Remove(beamShearAction);
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public void DeleteSection(IBeamShearSection section)
        {
            foreach (var calculator in Calculators)
            {
                if (calculator is IBeamShearCalculator beamShearCalculator)
                {
                    if (beamShearCalculator.InputData.ShearSections.Contains(section))
                    {
                        beamShearCalculator.InputData.DeleteSection(section);
                        beamShearCalculator.InputData.ShearSections.Remove(section);
                    }
                }
            }
        }
        public void DeleteAction(IBeamShearAction action)
        {
            foreach (var calculator in Calculators)
            {
                if (calculator is IBeamShearCalculator beamShearCalculator)
                {
                    if (beamShearCalculator.InputData.BeamShearActions.Contains(action))
                    {
                        beamShearCalculator.InputData.DeleteAction(action);
                        beamShearCalculator.InputData.BeamShearActions.Remove(action);
                    }
                }
            }
        }
        public void DeleteStirrup(IStirrup stirrup)
        {
            foreach (var calculator in Calculators)
            {
                if (calculator is IBeamShearCalculator beamShearCalculator)
                {
                    if (beamShearCalculator.InputData.Stirrups.Contains(stirrup))
                    {
                        beamShearCalculator.InputData.DeleteStirrup(stirrup);
                        beamShearCalculator.InputData.Stirrups.Remove(stirrup);
                    }
                }
            }
        }
    }
}
