using StructureHelperCommon.Infrastructures.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces.Logics
{
    public interface IGetForceTupleLogic
    {
        /// <summary>
        /// Calculates force tuple
        /// </summary>
        /// <returns></returns>
        IForceTuple GetForceTuple();
    }
}
