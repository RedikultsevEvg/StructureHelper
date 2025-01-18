using StructureHelperCommon.Infrastructures.Interfaces;
using System.Collections.Generic;

namespace StructureHelperCommon.Models.Forces
{
    public interface IGetTuplesFromFileLogic : ILogic
    {
        IColumnedFileProperty ForceFileProperty { get; set; }

        List<IForceTuple> GetTuples();
    }
}