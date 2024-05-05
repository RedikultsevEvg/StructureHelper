using LoaderCalculator;
using LoaderCalculator.Data.Materials.MaterialBuilders;
using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Data.ResultData;
using LoaderCalculator.Data.SourceData;
using StructureHelper.Models.Materials;
using StructureHelper.Services;
using StructureHelper.Services.Primitives;
using StructureHelper.UnitSystem;
using StructureHelper.UnitSystem.Systems;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models;
using StructureHelperCommon.Services.Units;
using StructureHelperLogics.Models.Calculations.CalculationProperties;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.Models.Materials;
using StructureHelperLogics.NdmCalculations.Triangulations;
using StructureHelperLogics.Services;
using StructureHelperLogics.Services.NdmCalculations;
using StructureHelperLogics.Services.NdmPrimitives;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace StructureHelper.Windows.MainWindow
{
    public class CrossSectionModel
    {
        private ITriangulatePrimitiveLogic triangulateLogic;

        public ICrossSection Section { get; private set; }
        private IPrimitiveRepository primitiveRepository;
        public IHeadMaterialRepository HeadMaterialRepository { get; }
        public List<IHeadMaterial> HeadMaterials { get; }
        private CalculationService calculationService;
        private UnitSystemService unitSystemService;

        public IPrimitiveRepository PrimitiveRepository => primitiveRepository;

        public ICalculationProperty CalculationProperty { get; private set; }

        public CrossSectionModel(IPrimitiveRepository primitiveRepository, CalculationService calculationService, UnitSystemService unitSystemService)
        {
            this.primitiveRepository = primitiveRepository;
            this.calculationService = calculationService;
            this.unitSystemService = unitSystemService;

            Section = new CrossSection();
            CalculationProperty = new CalculationProperty();
            HeadMaterials = new List<IHeadMaterial>();
            HeadMaterialRepository = new HeadMaterialRepository(this);
        }

        public IEnumerable<INdm> GetNdms(ICalculationProperty calculationProperty)
        {
            var ndmPrimitives = Section.SectionRepository.Primitives;
            triangulateLogic = new TriangulatePrimitiveLogic()
            {
                Primitives = ndmPrimitives,
                LimitState = calculationProperty.LimitState,
                CalcTerm = calculationProperty.CalcTerm
            };
            return triangulateLogic.GetNdms();
            ////Настройки триангуляции, пока опции могут быть только такие
            //ITriangulationOptions options = new TriangulationOptions { LimiteState = calculationProperty.LimitState, CalcTerm = calculationProperty.CalcTerm };

            ////Формируем коллекцию элементарных участков для расчета в библитеке (т.е. выполняем триангуляцию)
            //return ndmPrimitives.SelectMany(x => x.GetNdms(options));
        }
    }
}
