using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    public class StirrupGroup : IStirrupGroup
    {
        public Guid Id { get; }
        public string Name { get; set; }
        public List<IStirrup> Stirrups { get; } = new();
        public double CompressedGap { get; set; }

        public double GetShearBearingCapacity(IInclinedSection inclinedSection)
        {
            throw new NotImplementedException();
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }

    }
}
