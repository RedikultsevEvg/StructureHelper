using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams
{
    public interface IValueDiagramEntityLogic : ILogic
    {
        IValueDiagramEntity ValueDiagramEntity { get; set; }
        IValueDiagramEntityResult Result { get; }
        void Run();
    }
}
