using ExcelDataReader;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    public interface IGetTupleByExcelReaderLogic : ILogic
    {
        IColumnedFileProperty ForceFileProperty { get; set;}
        IForceTuple GetForceTuple(IExcelDataReader? reader);
    }
}
