using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    public interface IStirrupGroup : IStirrup
    {
        List<IStirrup> Stirrups { get; }
    }
}
