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

        public List<IBeamShearAction> BeamShearActions {get;}

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
    }
}
