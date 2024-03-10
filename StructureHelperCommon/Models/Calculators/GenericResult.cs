using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Calculators
{
    public class GenericResult<T> : IResult
    {
        public bool IsValid { get; set; }
        public string? Description { get; set; }
        public T? Value { get; set; }
    }
}
