using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Analyses
{
    public class AnalysisUpdateStrategy : IUpdateStrategy<IAnalysis>
    {
        public void Update(IAnalysis targetObject, IAnalysis sourceObject)
        {
            CheckObject.IsNull(targetObject, sourceObject, "Analysis Properties");
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Name = sourceObject.Name;
            targetObject.Tags = sourceObject.Tags;
        }
    }
}
