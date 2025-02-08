using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    public class StirrupByDensity : IStirrupByDensity
    {
        public Guid Id { get; }
        public string Name { get; set; } = string.Empty;
        public double StirrupDensity { get; set; }

        public StirrupByDensity(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
