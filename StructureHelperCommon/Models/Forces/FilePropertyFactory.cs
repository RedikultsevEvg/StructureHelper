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
                List<IColumnProperty> columnProperties = GetForceColumns();
                fileProperty.ColumnProperties.AddRange(columnProperties);
                return fileProperty;
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(propertyType));
            }
        }

        private static List<IColumnProperty> GetForceColumns()
        {
            List<IColumnProperty> columnProperties = new();
            columnProperties.Add(new ColumnProperty("Nz") { SearchingName = "N", Index = 6 });
            columnProperties.Add(new ColumnProperty("Mx") { SearchingName = "My", Index = 8 });
            columnProperties.Add(new ColumnProperty("My") { SearchingName = "Mz", Index = 10 });
            return columnProperties;
        }
    }
}
