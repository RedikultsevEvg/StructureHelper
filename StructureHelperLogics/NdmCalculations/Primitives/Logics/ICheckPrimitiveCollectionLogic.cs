using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Primitives.Logics
{
    public interface ICheckPrimitiveCollectionLogic : ICheckLogic
    {
        IHasPrimitives HasPrimitives { get; set; }
    }
}
