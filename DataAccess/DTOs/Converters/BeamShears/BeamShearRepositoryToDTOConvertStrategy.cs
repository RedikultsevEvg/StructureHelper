using DataAccess.DTOs.Converters;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Forces.BeamShearActions;
using StructureHelperLogics.Models.BeamShears;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace DataAccess.DTOs
{
    public class BeamShearRepositoryToDTOConvertStrategy : ConvertStrategy<BeamShearRepositoryDTO, IBeamShearRepository>
    {
        private IUpdateStrategy<IHasBeamShearActions> actionUpdateStrategy;
        private IUpdateStrategy<IHasBeamShearSections> sectionUpdateStrategy;
        private IUpdateStrategy<IHasStirrups> stirrupUpdateStrategy;
        private IUpdateStrategy<IHasCalculators> calculatorUpdateStrategy;

        public BeamShearRepositoryToDTOConvertStrategy(Dictionary<(Guid id, Type type), ISaveable> referenceDictionary, IShiftTraceLogger traceLogger) : base(referenceDictionary, traceLogger)
        {
        }

        public override BeamShearRepositoryDTO GetNewItem(IBeamShearRepository source)
        {
            try
            {
                GetNewBeamRepository(source);
                return NewItem;
            }
            catch (Exception ex)
            {
                TraceErrorByEntity(this, ex.Message);
                throw;
            }
        }

        private void GetNewBeamRepository(IBeamShearRepository source)
        {
            TraceLogger?.AddMessage($"Converting of beam shear repository Id = {source.Id} has been started", TraceLogStatuses.Debug);
            InitializeStrategies();
            NewItem = new(source.Id);
            actionUpdateStrategy.Update(NewItem, source);
            sectionUpdateStrategy.Update(NewItem, source);
            stirrupUpdateStrategy.Update(NewItem, source);
            calculatorUpdateStrategy.Update(NewItem, source);
            TraceLogger?.AddMessage($"Converting of beam shear repository Id = {NewItem.Id} has been finished", TraceLogStatuses.Service);

        }

        private void InitializeStrategies()
        {
            actionUpdateStrategy ??= new HasBeamShearActionToDTOConvertStrategy(ReferenceDictionary, TraceLogger);
            sectionUpdateStrategy ??= new HasBeamShearSectionToDTOConvertStrategy(ReferenceDictionary, TraceLogger);
            stirrupUpdateStrategy ??= new HasStirrupToDTOConvertStrategy(ReferenceDictionary, TraceLogger);
            calculatorUpdateStrategy ??= new HasBeamShearCalculatorToDTOConvertStrategy(ReferenceDictionary, TraceLogger);
        }
    }
}
