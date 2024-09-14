using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Analyses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Analyses
{
    public class VisualAnalysisUpdateStrategy : IUpdateStrategy<IVisualAnalysis>
    {
        public void Update(IVisualAnalysis targetObject, IVisualAnalysis sourceObject)
        {
            throw new NotImplementedException();
        }
    }
}
