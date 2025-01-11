using StructureHelperCommon.Infrastructures.Interfaces;
using System.Collections.Generic;

namespace StructureHelperCommon.Models.Forces.Logics
{
    public interface IGetTupleFromFileLogic : ILogic
    {
        IForceFileProperty ForceFileProperty { get; set; }

        List<IForceTuple> GetTuples();
    }
}