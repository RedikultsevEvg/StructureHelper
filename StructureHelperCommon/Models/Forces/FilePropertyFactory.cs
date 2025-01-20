using StructureHelperCommon.Infrastructures.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    public enum FilePropertyType
    {
        Forces
    }
    public static class FilePropertyFactory
    {
        public static IColumnedFileProperty GetForceFileProperty(FilePropertyType propertyType)
        {
            if (propertyType == FilePropertyType.Forces)
            {
                ColumnedFileProperty fileProperty = new();
                List<IColumnFileProperty> columnProperties = GetForceColumns();
                fileProperty.ColumnProperties.AddRange(columnProperties);
                return fileProperty;
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(propertyType));
            }
        }

        private static List<IColumnFileProperty> GetForceColumns()
        {
            List<IColumnFileProperty> columnProperties = new();
            columnProperties.Add(new ColumnFileProperty("Nz") { SearchingName = "N", Index = 6 });
            columnProperties.Add(new ColumnFileProperty("Mx") { SearchingName = "My", Index = 8 });
            columnProperties.Add(new ColumnFileProperty("My") { SearchingName = "Mz", Index = 10 });
            return columnProperties;
        }
    }
}
