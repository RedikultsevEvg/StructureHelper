using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    public class BeamShear : IBeamShear
    {

        public Guid Id { get; }
        public IBeamShearRepository Repository { get; } = new BeamShearRepository(Guid.NewGuid());
        public BeamShear(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
