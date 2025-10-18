using StructureHelperCommon.Infrastructures.Interfaces;
using System.Collections.Generic;

namespace StructureHelperCommon.Models.Forces
{
    public interface IGetTuplesFromFileLogic : ILogic
    {
        bool SkipWrongRows { get; set; }
        IColumnedFileProperty ForceFileProperty { get; set; }

        List<IForceTuple> GetTuples();
    }
}