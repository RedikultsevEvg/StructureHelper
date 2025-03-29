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

        public List<IBeamShearAction> Actions { get; } = new();

        public List<ICalculator> Calculators { get; } = new();

        public List<IBeamShearSection> Sections { get; } = new();

        public List<IStirrup> Stirrups { get; } = new();


        public BeamShearRepository(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
