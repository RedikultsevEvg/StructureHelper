using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Parameters
{
    /// <inheritdoc/>
    public class ValuePair<T> : IValuePair<T>
    {
        public string Text { get; set; }
        public T Value { get; set; }
    }
}
