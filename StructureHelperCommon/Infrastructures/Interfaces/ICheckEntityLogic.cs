using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    public interface ICheckEntityLogic<TEntity> : ICheckLogic 
    {
        TEntity Entity { get; set; }
    }
}
