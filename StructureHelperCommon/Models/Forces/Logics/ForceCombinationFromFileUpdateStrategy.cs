using StructureHelperCommon.Models.Forces.Logics;
using StructureHelperCommon.Services;

namespace StructureHelperCommon.Models.Forces
{
    public class ForceCombinationFromFileUpdateStrategy : IParentUpdateStrategy<IForceCombinationFromFile>
    {
        private IUpdateStrategy<IForceAction> baseUpdateStrategy;
        private IUpdateStrategy<IColumnedFileProperty> filePropertyUpdateStrategy;
        private IUpdateStrategy<IFactoredCombinationProperty> combinationPropertyUpdateStrategy;

        public bool UpdateChildren { get; set; } = true;

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
            CheckObject.ThrowIfNull(targetObject);
            CheckObject.ThrowIfNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            InitializeLogics();
            baseUpdateStrategy.Update(targetObject, sourceObject);
            targetObject.SkipWrongRows = sourceObject.SkipWrongRows;
            CheckObject.ThrowIfNull(targetObject.CombinationProperty, "Target object combination property");
            CheckObject.ThrowIfNull(sourceObject.CombinationProperty, "Source object combination property");
            combinationPropertyUpdateStrategy.Update(targetObject.CombinationProperty, sourceObject.CombinationProperty);
            if (UpdateChildren == true)
            {
                targetObject.ForceFiles.Clear();
                foreach (var file in sourceObject.ForceFiles)
                {
                    ColumnedFileProperty newProperty = new();
                    filePropertyUpdateStrategy.Update(newProperty, file);
                    targetObject.ForceFiles.Add(newProperty);
                }
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
