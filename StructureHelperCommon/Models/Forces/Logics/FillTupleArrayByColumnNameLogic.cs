using StructureHelperCommon.Infrastructures.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    public class FillTupleArrayByColumnNameLogic : IFillTupleArrayByColumnNameLogic
    {
        public IShiftTraceLogger? TraceLogger { get; set; }

        public void ProceeForceTupleArray(double[] nDouble, double doubleValue, string columnName, string filePath)
        {
            if (columnName == "Nz")
            {
                nDouble[0] = doubleValue;
            }
            else if (columnName == "Mx")
            {
                nDouble[1] = doubleValue;
            }
            else if (columnName == "My")
            {
                nDouble[2] = doubleValue;
            }
            else
            {
                string errorString = ErrorStrings.DataIsInCorrect + $": column {columnName} in file {filePath}, ";
                TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
                throw new StructureHelperException(errorString);
            }
        }
    }
}
