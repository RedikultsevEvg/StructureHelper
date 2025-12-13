using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.CrossSections
{
    public interface IRepositoryOperationsLogic
    {
        IRepositoryOperation<ICrossSectionRepository, INdmPrimitive> Primitives { get; }
    }
}
