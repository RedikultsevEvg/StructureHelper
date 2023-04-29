using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Parameters
{
    public class TextParameter : ITextParameter
    {
        public bool IsValid { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string MeasurementUnit { get; set; }
        public double Value { get; set; }
        public string Description { get; set; }
    }
}
