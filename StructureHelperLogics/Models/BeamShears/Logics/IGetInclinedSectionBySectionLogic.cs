using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    public interface IGetInclinedSectionLogic : ILogic
    {
        IInclinedSection GetInclinedSection();
    }
}
