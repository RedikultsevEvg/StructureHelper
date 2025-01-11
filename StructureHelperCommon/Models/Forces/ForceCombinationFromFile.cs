using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces.Logics;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    public class ForceCombinationFromFile : IForceCombinationFromFile
    {
        IUpdateStrategy<IForceCombinationFromFile> updateStrategy;
        IUpdateStrategy<IFactoredCombinationProperty> propertyUpdateStrategy;
        IGetTupleFromFileLogic getTupleFromFileLogic;
        
        private IForceFactoredList factoredCombination;
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<IForceFileProperty> ForceFiles { get; set; } = new();
        public bool SetInGravityCenter { get; set; } = true;
        public IPoint2D ForcePoint { get; set; } = new Point2D();

        public IFactoredCombinationProperty CombinationProperty { get; } = new FactoredCombinationProperty();

        public object Clone()
        {
            ForceCombinationFromFile newItem = new();
            updateStrategy ??= new ForceCombinationFromFileUpdateStrategy();
            updateStrategy.Update(newItem, this);
            return newItem;
        }

        public List<IForceCombinationList> GetCombinations()
        {
            getTupleFromFileLogic ??= new GetTupleFromFileLogic() { TraceLogger = new ShiftTraceLogger()};
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
    }
}
