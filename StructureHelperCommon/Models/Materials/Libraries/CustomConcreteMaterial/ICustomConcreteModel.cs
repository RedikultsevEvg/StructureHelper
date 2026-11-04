using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.States;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Materials
{
    public interface ICustomConcreteModel : ISaveable, ICloneable
    {
        bool IsActive { get; set; }
        string Name { get; set; }
        double YoungModulus { get; set; }
        IStateCalcTermPair StateCalcTermPair { get; }
        ICustomConcreteCompressionOptions CompressionOptions { get; }
        ICustomConcreteTensionOptions TensionOptions { get; }
    }
}
