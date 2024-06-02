using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace StructureHelperCommon.Models.Parameters
{
    public class ValueParameter<T> : IValueParameter<T>
    {
        public bool IsValid { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public Color Color { get; set; }
        public string Text { get; set; }
        public T Value { get; set; }
        public string Description { get; set; }
    }
}
