using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Infrastructure.UI.Converters
{
    internal interface IStringDoublePair
    {
        double Digit { get; }
        string Text { get; }
    }
}
