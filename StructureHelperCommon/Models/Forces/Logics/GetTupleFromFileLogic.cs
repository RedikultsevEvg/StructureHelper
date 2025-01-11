using ExcelDataReader;
using StructureHelperCommon.Infrastructures.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces.Logics
{
    public class GetTupleFromFileLogic : IGetTupleFromFileLogic
    {
        public IForceFileProperty ForceFileProperty { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public List<IForceTuple> GetTuples()
        {
            // Ensure ExcelDataReader's encoding provider is registered
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            var result = ReadDataFromFile();
            return result;
        }

        private List<IForceTuple> ReadDataFromFile()
        {
            List<IForceTuple> result = new();
            // Open the Excel file stream
            using (var stream = File.Open(ForceFileProperty.FilePath, FileMode.Open, FileAccess.Read))
            {
                // Create an Excel reader
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    // Skip the first two header rows if necessary (adjust based on structure)
                    int skipRows = ForceFileProperty.SkipRowBeforeHeaderCount + ForceFileProperty.SkipRowHeaderCount;
                    for (int i = 0; i < skipRows; i++)
                    {
                        reader.Read(); // Skip  row
                    }

                    // Loop through the rows
                    while (reader.Read())
                    {
                        var nValue = reader.GetValue(ForceFileProperty.Nz.ColumnIndex)?.ToString();
                        var mxValue = reader.GetValue(ForceFileProperty.Mx.ColumnIndex)?.ToString();
                        var myValue = reader.GetValue(ForceFileProperty.My.ColumnIndex)?.ToString();
                        TraceLogger?.AddMessage($"Values: Nz = {nValue}(N), Mx = {mxValue}(N*m), My = {myValue}(N*m) were gained", TraceLogStatuses.Debug);
                        if (nValue is not null && mxValue is not null && myValue is not null)
                        {
                            ForceTuple newTuple = GetForceTuple(nValue, mxValue, myValue);
                            result.Add(newTuple);
                        }
                    }
                }
            }
            return result;
        }

        private ForceTuple GetForceTuple(string? nValue, string? mxValue, string? myValue)
        {
            try
            {
                double nDouble = Convert.ToDouble(nValue);
                double mxDouble = Convert.ToDouble(mxValue);
                double myDouble = Convert.ToDouble(myValue);
                TraceLogger?.AddMessage($"Values: Nz = {nDouble}(N), Mx = {mxDouble}(N*m), My = {myDouble}(N*m) were converted successfully", TraceLogStatuses.Debug);
                ForceTuple newTuple = new() { Nz = nDouble, Mx = mxDouble, My = myDouble };
                return newTuple;
            }
            catch (Exception ex)
            {
                string errorString = ErrorStrings.DataIsInCorrect + $": incorrect data in file {ForceFileProperty.FilePath}, " + ex.Message;
                TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
                throw new StructureHelperException(errorString);
            }
        }
    }
}
