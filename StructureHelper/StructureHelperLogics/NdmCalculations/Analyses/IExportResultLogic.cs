using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses
{
    public interface IExportResultLogic
    {
        void Export(INdmResult ndmResult);
    }
}
