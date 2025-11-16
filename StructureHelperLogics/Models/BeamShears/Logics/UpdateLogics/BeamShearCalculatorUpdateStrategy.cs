using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.BeamShears
{
    public class BeamShearCalculatorUpdateStrategy : IUpdateStrategy<IBeamShearCalculator>
    {
        private IUpdateStrategy<IBeamShearCalculatorInputData>? inputDataUpdateStrategy;
        public void Update(IBeamShearCalculator targetObject, IBeamShearCalculator sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject, ErrorStrings.SourceObject);
            CheckObject.ThrowIfNull(targetObject, ErrorStrings.TargetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; };
            targetObject.Name = sourceObject.Name;
            targetObject.ShowTraceData = sourceObject.ShowTraceData;
            targetObject.InputData ??= new BeamShearCalculatorInputData(Guid.NewGuid());
            InitializeStrategies();
            inputDataUpdateStrategy.Update(targetObject.InputData, sourceObject.InputData);
        }

        private void InitializeStrategies()
        {
            inputDataUpdateStrategy ??= new BeamShearCalculatorInputDataUpdateStrategy();
        }
    }
}
