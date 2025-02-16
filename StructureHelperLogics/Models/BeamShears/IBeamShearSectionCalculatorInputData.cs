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
    public interface IBeamShearSectionCalculatorInputData : IInputData, ISaveable
    {
        IBeamShearSection? BeamShearSection { get; set; }
        IBeamShearAction? BeamShearAction { get; set; }
        IStirrup? Stirrup { get; set; }
    }
}
