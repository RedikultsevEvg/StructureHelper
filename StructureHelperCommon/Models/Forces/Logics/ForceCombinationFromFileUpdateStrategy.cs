using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces.Logics;
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
        private IUpdateStrategy<IColumnedFileProperty> filePropertyUpdateStrategy;
        private IUpdateStrategy<IFactoredCombinationProperty> combinationPropertyUpdateStrategy;

        public ForceCombinationFromFileUpdateStrategy(IUpdateStrategy<IForceAction> baseUpdateStrategy,
            IUpdateStrategy<IColumnedFileProperty> filePropertyUpdateStrategy,
            IUpdateStrategy<IFactoredCombinationProperty> combinationPropertyUpdateStrategy)
        {
            this.baseUpdateStrategy = baseUpdateStrategy;
            this.filePropertyUpdateStrategy = filePropertyUpdateStrategy;
            this.combinationPropertyUpdateStrategy = combinationPropertyUpdateStrategy;
        }

        public ForceCombinationFromFileUpdateStrategy()
        {   
        }

        public void Update(IForceCombinationFromFile targetObject, IForceCombinationFromFile sourceObject)
        {
            CheckObject.IsNull(targetObject);
            CheckObject.IsNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            InitializeLogics();
            baseUpdateStrategy.Update(targetObject, sourceObject);
            targetObject.SkipWrongRows = sourceObject.SkipWrongRows;
            CheckObject.IsNull(targetObject.CombinationProperty, "Target object combination property");
            CheckObject.IsNull(sourceObject.CombinationProperty, "Source object combination property");
            combinationPropertyUpdateStrategy.Update(targetObject.CombinationProperty, sourceObject.CombinationProperty);
            targetObject.ForceFiles.Clear();
            foreach (var file in sourceObject.ForceFiles)
            {
                ColumnedFileProperty newProperty = new();
                filePropertyUpdateStrategy.Update(newProperty, file);
                targetObject.ForceFiles.Add(newProperty);
            }
        }

        private void InitializeLogics()
        {
            baseUpdateStrategy ??= new ForceActionBaseUpdateStrategy();
            filePropertyUpdateStrategy ??= new ColumnedFilePropertyUpdateStrategy();
            combinationPropertyUpdateStrategy ??= new FactoredCombinationPropertyUpdateStrategy();
        }
    }
}
