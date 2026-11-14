using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces.Logics;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;

namespace StructureHelperCommon.Models.Forces
{
    public class ForceCombinationFromFile : IForceCombinationFromFile
    {
        IUpdateStrategy<IForceCombinationFromFile> updateStrategy;
        IUpdateStrategy<IFactoredCombinationProperty> propertyUpdateStrategy;
        IGetTuplesFromFileLogic getTupleFromFileLogic;
        
        private IForceFactoredList factoredCombination;
        public Guid Id { get; }

        public ForceCombinationFromFile(Guid id)
        {
            Id = id;
        }

        public ForceCombinationFromFile() : this(Guid.NewGuid())
        {
            
        }

        public string Name { get; set; } = string.Empty;
        public List<IColumnedFileProperty> ForceFiles { get; set; } = new();
        public bool SetInGravityCenter { get; set; } = true;
        public IPoint2D ForcePoint { get; set; } = new Point2D();

        public IFactoredCombinationProperty CombinationProperty { get; set; } = new FactoredCombinationProperty(Guid.NewGuid());
        public bool SkipWrongRows { get; set; } = false;


        public List<IForceCombinationList> GetCombinations()
        {
            getTupleFromFileLogic = new GetTuplesFromFileLogic()
            {
                TraceLogger = new ShiftTraceLogger(),
                SkipWrongRows = SkipWrongRows,
            };
            factoredCombination = new ForceFactoredList();
            factoredCombination.ForceTuples.Clear();
            propertyUpdateStrategy ??= new FactoredCombinationPropertyUpdateStrategy();
            propertyUpdateStrategy.Update(factoredCombination.CombinationProperty, CombinationProperty);
            foreach (var file in ForceFiles)
            {
                getTupleFromFileLogic.ForceFileProperty = file;
                var tuples = getTupleFromFileLogic.GetTuples();
                factoredCombination.ForceTuples.AddRange(tuples);
            }
            return factoredCombination.GetCombinations();
        }

        public IForceCombinationFromFile GetClone(CloneMode cloningMode, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloningMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloningMode);
            ForceCombinationFromFile newItem = new(id);
            updateStrategy ??= new ForceCombinationFromFileUpdateStrategy();
            updateStrategy.Update(newItem, this);
            return newItem;
        }
    }
}
