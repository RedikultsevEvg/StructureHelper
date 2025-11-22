using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public class CurvatureCalculatorUpdateStrategy : IParentUpdateStrategy<ICurvatureCalculator>
    {
        private IUpdateStrategy<ICurvatureCalculatorInputData> inputDataUpdateStrategy;
        private IUpdateStrategy<ICurvatureCalculatorInputData> InputDataUpdateStrategy => inputDataUpdateStrategy ??= new CurvatureCalculatorInputDataUpdateStrategy();

        public CurvatureCalculatorUpdateStrategy(IUpdateStrategy<ICurvatureCalculatorInputData> inputDataUpdateStrategy)
        {
            this.inputDataUpdateStrategy = inputDataUpdateStrategy ?? throw new StructureHelperNullReferenceException(ErrorStrings.NullReference + ": input data of curvature calculator can not be null");
        }

        public CurvatureCalculatorUpdateStrategy()
        {
            
        }

        public bool UpdateChildren { get; set; } = true;

        public void Update(ICurvatureCalculator targetObject, ICurvatureCalculator sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject, nameof(sourceObject));
            CheckObject.ThrowIfNull(targetObject, nameof(targetObject));
            if (ReferenceEquals(targetObject, sourceObject))
                return;
            targetObject.Name = sourceObject.Name;
            targetObject.ShowTraceData = sourceObject.ShowTraceData;
            if (UpdateChildren == true)
            {
                CheckObject.ThrowIfNull(sourceObject.InputData);
                CheckObject.ThrowIfNull(targetObject.InputData);
                InputDataUpdateStrategy.Update(targetObject.InputData, sourceObject.InputData);
            }
        }
    }
}
