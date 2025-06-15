using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.Models.BeamShears;

namespace DataAccess.DTOs
{
    public class BeamShearRepositoryFromDTOConvertStrategy : ConvertStrategy<BeamShearRepository, BeamShearRepositoryDTO>
    {
        private IUpdateStrategy<IHasBeamShearActions> actionUpdateStrategy;
        private IUpdateStrategy<IHasBeamShearSections> sectionUpdateStrategy;
        private IUpdateStrategy<IHasStirrups> stirrupUpdateStrategy;
        private IUpdateStrategy<IHasCalculators> calculatorUpdateStrategy;

        public BeamShearRepositoryFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override BeamShearRepository GetNewItem(BeamShearRepositoryDTO source)
        {
            ChildClass = this;
            GetNewRepository(source);
            return NewItem;
        }

        private void GetNewRepository(BeamShearRepositoryDTO source)
        {
            InitializeStrategies();
            NewItem = new(source.Id);
            actionUpdateStrategy.Update(NewItem, source);
            sectionUpdateStrategy.Update(NewItem, source);
            stirrupUpdateStrategy.Update(NewItem, source);
            calculatorUpdateStrategy.Update(NewItem, source);
        }

        private void InitializeStrategies()
        {
            actionUpdateStrategy ??= new HasBeamShearActionsFromDTOUpdateStrategy(ReferenceDictionary, TraceLogger);
            sectionUpdateStrategy ??= new HasBeamShearSectionsFromDTOUpdateStrategy(ReferenceDictionary, TraceLogger);
            stirrupUpdateStrategy ??= new HasStirrupsFromDTOUpdateStrategy(ReferenceDictionary, TraceLogger);
            calculatorUpdateStrategy ??= new HasBeamShearCalculatorsFromDTOUpdateStrategy(ReferenceDictionary, TraceLogger);
        }
    }
}
