using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class RebarSectionToDTOConvertStrategy : ConvertStrategy<RebarSectionDTO, IRebarSection>
    {
        private IUpdateStrategy<IRebarSection> updateStrategy;
        private ReinforcementLibMaterialToDTOConvertStrategy reinforcementConvertStrategy;
        private HelperMaterialDTOSafetyFactorUpdateStrategy safetyFactorUpdateStrategy;

        public RebarSectionToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override RebarSectionDTO GetNewItem(IRebarSection source)
        {
            InitializeStrategies();
            ChildClass = this;
            NewItem = new(source.Id);
            updateStrategy.Update(NewItem, source);
            NewItem.Material = reinforcementConvertStrategy.Convert(source.Material);
            safetyFactorUpdateStrategy.Update(NewItem.Material, source.Material);
            return NewItem;
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new RebarSectionUpdateStrategy();
            reinforcementConvertStrategy = new ReinforcementLibMaterialToDTOConvertStrategy()
            {
                ReferenceDictionary = ReferenceDictionary,
                TraceLogger = TraceLogger
            };
            safetyFactorUpdateStrategy = new HelperMaterialDTOSafetyFactorUpdateStrategy(new MaterialSafetyFactorToDTOLogic());
        }
    }
}
