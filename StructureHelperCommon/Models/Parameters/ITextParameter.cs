using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Parameters
{
    public interface ITextParameter
    {
        bool IsValid { get; set; }
        string Name { get; set; }
        string ShortName { get; set; }
        string MeasurementUnit { get; set; }
        double Value { get; set; }
        string Description { get; set; }
    }
}
