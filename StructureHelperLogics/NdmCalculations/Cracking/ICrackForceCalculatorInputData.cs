using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public interface ICrackForceCalculatorInputData: IInputData
    {
        
        IForceTuple StartTuple { get; set; }
        IForceTuple EndTuple { get; set; }
        IEnumerable<INdm> CheckedNdmCollection { get; set; }
        IEnumerable<INdm> SectionNdmCollection { get; set; }
    }
}
