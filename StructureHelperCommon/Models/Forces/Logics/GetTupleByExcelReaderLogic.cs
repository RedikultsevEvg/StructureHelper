using ExcelDataReader;
using StructureHelperCommon.Infrastructures.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    public class GetTupleByExcelReaderLogic : IGetTupleByExcelReaderLogic
    {
        IFillTupleArrayByColumnNameLogic fillArrayLogic;

        public GetTupleByExcelReaderLogic(IFillTupleArrayByColumnNameLogic fillArrayLogic)
        {
            this.fillArrayLogic = fillArrayLogic;
        }

        public GetTupleByExcelReaderLogic()
        {
            
        }

        public IShiftTraceLogger? TraceLogger { get; set; }
        public IColumnedFileProperty ForceFileProperty { get; set; }

        public IForceTuple GetForceTuple(IExcelDataReader? reader)
        {
            // Ensure ExcelDataReader's encoding provider is registered
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            double[] nDouble = new double[3];
            foreach (var item in ForceFileProperty.ColumnProperties)
            {
                if (reader is null)
                {
                    throw new StructureHelperException(ErrorStrings.NullReference + $": reader value for column {item.Name} in file {ForceFileProperty.FilePath}");
                }
                var readerValue = reader.GetValue(item.Index);
                var strValue = readerValue.ToString();
                double factor = ForceFileProperty.GlobalFactor * item.Factor * 1000d;
                var doubleValue = Convert.ToDouble(strValue) * factor;
                fillArrayLogic ??= new FillTupleArrayByColumnNameLogic() { TraceLogger = TraceLogger };
                fillArrayLogic.ProceeForceTupleArray(nDouble, doubleValue, item.Name, ForceFileProperty.FilePath);
            }
            TraceLogger?.AddMessage($"String values: Nz = {nDouble[0]}, Mx = {nDouble[1]}, My = {nDouble[2]} were gained", TraceLogStatuses.Debug);
            ForceTuple newTuple = new() { Nz = nDouble[0], Mx = nDouble[1], My = nDouble[2] };
            return newTuple;
        }
    }
}
