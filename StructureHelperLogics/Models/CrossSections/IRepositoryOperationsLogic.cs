using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.CrossSections
{
    public interface IRepositoryOperationsLogic
    {
        IRepositoryOperation<ICrossSectionRepository, IForceAction> Actions { get; }
        IRepositoryOperation<ICrossSectionRepository, INdmPrimitive> Primitives { get; }
    }
}
