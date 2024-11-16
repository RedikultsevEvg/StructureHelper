using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    public class ForceCombinationFromFileUpdateStrategy : IUpdateStrategy<IForceCombinationFromFile>
    {
        private IUpdateStrategy<IForceAction> baseUpdateStrategy;
        private IUpdateStrategy<IForceFileProperty> fileUpdateStrategy;

        public ForceCombinationFromFileUpdateStrategy(IUpdateStrategy<IForceAction> baseUpdateStrategy, IUpdateStrategy<IForceFileProperty> fileUpdateStrategy)
        {
            this.baseUpdateStrategy = baseUpdateStrategy;
            this.fileUpdateStrategy = fileUpdateStrategy;
        }

        public ForceCombinationFromFileUpdateStrategy() : this (
            new ForceActionUpdateStrategy(),
            new ForceFilePropertyUpdateStrategy())
        {
            
        }

        void IUpdateStrategy<IForceCombinationFromFile>.Update(IForceCombinationFromFile targetObject, IForceCombinationFromFile sourceObject)
        {
            CheckObject.IsNull(targetObject);
            CheckObject.IsNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            baseUpdateStrategy.Update(targetObject, sourceObject);
            targetObject.ForceFiles.Clear();
            foreach (var file in sourceObject.ForceFiles)
            {
                throw new NotImplementedException();
            }
        }
    }
}
