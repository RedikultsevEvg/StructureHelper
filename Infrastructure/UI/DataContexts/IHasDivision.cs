using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Infrastructure.UI.DataContexts
{
    internal interface IHasDivision
    {
        int NdmMinDivision { get; set; }
        double NdmMaxSize { get; set; }
    }
}
