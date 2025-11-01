using StructureHelperCommon.Infrastructures.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Services.Exports.Factories
{
    public enum FileInputDataType
    {
        Csv,
        Png,
        Dxf
    }
    public static class FileInputDataFactory
    {
        public static FileIOInputData GetFileIOInputData(FileInputDataType dataType)
        {
            if (dataType == FileInputDataType.Csv)
            {
                return new FileIOInputData
                {
                    Filter = "csv |*.csv",
                    Title = "Save in csv File"
                };
            }
            else if (dataType == FileInputDataType.Png)
            {
                return new FileIOInputData
                {
                    Filter = "png |*.png",
                    Title = "Save in *.png File"
                };
            }
            else if (dataType == FileInputDataType.Dxf)
            {
                return new FileIOInputData
                {
                    Filter = "dxf |*.dxf",
                    Title = "Save in *.dxf File"
                };
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(dataType));
            }
        }
    }
}
