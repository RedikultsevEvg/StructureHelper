using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Parameters
{
    public interface IArrayParameter<T>
    {
        T[,] Data { get; }
        string[] ColumnLabels { get; set; }
    }
}
