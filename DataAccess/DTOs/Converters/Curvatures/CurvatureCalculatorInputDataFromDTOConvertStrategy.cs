using DataAccess.DTOs.Converters;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Analyses.Curvatures;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.DTOs
{
    public class CurvatureCalculatorInputDataFromDTOConvertStrategy : ConvertStrategy<CurvatureCalculatorInputData, CurvatureCalculatorInputDataDTO>
    {
        private IProcessLogic<IHasForcesAndPrimitives> forcesAndPrimitivesProcessLogic;
        private IUpdateStrategy<ICurvatureCalculatorInputData> updateStrategy;
        private IConvertStrategy<DeflectionFactor, DeflectionFactorDTO> deflectionConvertStrategy;
        private IProcessLogic<IHasForcesAndPrimitives> ForcesAndPrimitivesProcessLogic => forcesAndPrimitivesProcessLogic ??= new HasForcesAndPrimitivesProcessLogic(ConvertDirection.FromDTO) { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger };
        private IUpdateStrategy<ICurvatureCalculatorInputData> UpdateStrategy => updateStrategy ??= new CurvatureCalculatorInputDataUpdateStrategy() { UpdateChildren = false };
        private IConvertStrategy<DeflectionFactor, DeflectionFactorDTO> DeflectionConvertStrategy => deflectionConvertStrategy ??= new DeflectionFactorFromDTOConvertStrategy(this);
        public CurvatureCalculatorInputDataFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }
        public override CurvatureCalculatorInputData GetNewItem(CurvatureCalculatorInputDataDTO source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            if (source.DeflectionFactor is not DeflectionFactorDTO deflectionFactorDTO)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source.DeflectionFactor) + ": deflection factor");
            }
            NewItem.DeflectionFactor = DeflectionConvertStrategy.Convert(deflectionFactorDTO);
            ProcessForcesAndPrimitives(source);
            return NewItem;
        }


        private void ProcessForcesAndPrimitives(IHasForcesAndPrimitives source)
        {
            ForcesAndPrimitivesProcessLogic.Source = source;
            ForcesAndPrimitivesProcessLogic.Target = NewItem;
            ForcesAndPrimitivesProcessLogic.Process();
        }
    }
}
