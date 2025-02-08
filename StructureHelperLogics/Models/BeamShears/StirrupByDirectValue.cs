using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    public class StirrupByDirectValue : IStirrupByDirectValue
    {
        public Guid Id { get; }
        public string Name { get; set; }
        public double BearingCapacityValue { get; set; }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
