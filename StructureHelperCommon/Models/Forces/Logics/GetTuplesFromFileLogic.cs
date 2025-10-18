using ExcelDataReader;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.


namespace StructureHelperCommon.Models.Forces
{
    public class GetTuplesFromFileLogic : IGetTuplesFromFileLogic
    {
        private IGetTupleByExcelReaderLogic excelReaderLogic;
        private ICheckEntityLogic<IColumnedFileProperty> checkEntityLogic;

        public GetTuplesFromFileLogic(IGetTupleByExcelReaderLogic excelReaderLogic,
            ICheckEntityLogic<IColumnedFileProperty> checkEntityLogic)
        {
            this.excelReaderLogic = excelReaderLogic;
            this.checkEntityLogic = checkEntityLogic;
        }

        public GetTuplesFromFileLogic()
        {
            
        }

        private List<IForceTuple> result;
        private int skipRows;

        public IColumnedFileProperty ForceFileProperty { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }
        public bool SkipWrongRows { get; set; } = false;

        public List<IForceTuple> GetTuples()
        {
            Check();
            // Ensure ExcelDataReader's encoding provider is registered
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            result = ReadDataFromFile();
            return result;
        }

        private List<IForceTuple> ReadDataFromFile()
        {
            result = new();
            skipRows = ForceFileProperty.SkipRowBeforeHeaderCount + ForceFileProperty.SkipRowHeaderCount;
            // Open the Excel file stream
            using (var stream = File.Open(ForceFileProperty.FilePath, FileMode.Open, FileAccess.Read))
            {
                // Create an Excel reader
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    // Skip the first header rows if necessary (adjust based on structure)
                    for (int i = 0; i < skipRows; i++)
                    {
                        reader.Read(); // Skip  row
                    }

                    excelReaderLogic ??= new GetTupleByExcelReaderLogic()
                    {
                        ForceFileProperty = ForceFileProperty,
                        TraceLogger = TraceLogger,
                        SkipWrongRows = SkipWrongRows
                    };

                    // Loop through the rows
                    while (reader.Read())
                    {
                        try
                        {
                            IForceTuple newTuple = excelReaderLogic.GetForceTuple(reader);
                            if (! result.Any(x => x.Mx == newTuple.Mx && x.My == newTuple.My && x.Nz == newTuple.Nz))
                            {
                                result.Add(newTuple);
                            }
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
            return result;
        }

        private void Check()
        {
            checkEntityLogic ??= new CheckColumnedFilePropertyLogic()
            {
                Entity = ForceFileProperty,
                TraceLogger = TraceLogger
            };
            if (checkEntityLogic.Check() == false)
            {
                throw new StructureHelperException(checkEntityLogic.CheckResult);
            }
        }
    }
}
