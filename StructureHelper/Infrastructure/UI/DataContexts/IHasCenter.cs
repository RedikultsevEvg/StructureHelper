using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Infrastructure.UI.DataContexts
{
    internal interface IHasCenter
    {
        double PrimitiveLeft { get; }
        double PrimitiveTop { get; }
    }
}
