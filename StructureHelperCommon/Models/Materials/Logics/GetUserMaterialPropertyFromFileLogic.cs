using ExcelDataReader;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace StructureHelperCommon.Models.Materials.Logics
{
    public class GetUserMaterialPropertyFromFileLogic : IGetUserMaterialPropertyFromFileLogic
    {
        private const string modulus = "Young's Modulus";
        private const string limitNegStrain = "Limit Compressive Strain";
        private const string limitPosStrain = "Limit Tensile Strain";
        private const string strain = "Table of strain";
        private UserMaterialProperty result;
        public string FilePath { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public IUserMaterialProperty GetUserMaterialProperty()
        {
            if (File.Exists(FilePath) == false)
            {
                throw new StructureHelperException(ErrorStrings.FileDoesNotExsist + $": {FilePath}");
            }
            // Ensure ExcelDataReader's encoding provider is registered
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            ReadDataFromFile();
            return result;
        }

        private void ReadDataFromFile()
        {
            result = new();
            // Open the Excel file stream
            using (var stream = File.Open(FilePath, FileMode.Open, FileAccess.Read))
            {
                // Create an Excel reader
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    result.YoungModulus = GetValueByName(reader, modulus);
                    result.LimitNegativeStrain = GetValueByName(reader, limitNegStrain);
                    result.LimitPositiveStrain = GetValueByName(reader, limitPosStrain);
                    result.StressStrainPairs.AddRange(GetStressDiagram(reader));
                }
            }
        }

        private IEnumerable<IStressStrainTuple> GetStressDiagram(IExcelDataReader reader)
        {
            List<IStressStrainTuple> stressStrainTuples = [];
            int skipRows = GetStressFirstRowIndex(reader);
            // Skip the first header rows if necessary (adjust based on structure)
            for (int i = 0; i < skipRows; i++)
            {
                reader.Read(); // Skip  row
            }
            while (reader.Read())
            {
                StressStrainTuple stressStrain = new()
                {
                    Strain = reader.GetDouble(0),
                    Stress = reader.GetDouble(1)
                };
                stressStrainTuples.Add(stressStrain);
            }
            return stressStrainTuples;
        }

        private int GetStressFirstRowIndex(IExcelDataReader reader)
        {
            int i = 0;
            while (reader.Read())
            {
                if (reader.GetString(0).ToLower() == strain.ToLower())
                {
                    return i;
                }
            }
            throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": row for stress does not exsist");
        }

        private double GetValueByName(IExcelDataReader reader, string name)
        {
            // Loop through the rows
            while (reader.Read())
            {
                if (reader.GetString(0).ToLower() == name.ToLower())
                {
                    return reader.GetDouble(1);
                }
            }
            throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": row for {name} does not exsist");
        }
    }
}
