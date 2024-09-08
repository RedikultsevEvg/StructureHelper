using DataAccess.DTOs;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.JsonConverters
{
    public class FileVersionDTOJsonConverter : BaseJsonConverter<FileVersionDTO>
    {
        public FileVersionDTOJsonConverter(IShiftTraceLogger logger) : base(logger)
        {
        }
    }
}
