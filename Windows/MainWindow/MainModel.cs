﻿using LoaderCalculator;
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
using StructureHelperCommon.Services.Units;
using StructureHelperLogics.Models.Calculations.CalculationProperties;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.Models.Materials;
using StructureHelperLogics.NdmCalculations.Triangulations;
using StructureHelperLogics.Services;
using StructureHelperLogics.Services.NdmCalculations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace StructureHelper.Windows.MainWindow
{
    public class MainModel
    {
        public ICrossSection Section { get; private set; }
        private IPrimitiveRepository primitiveRepository;
        public IHeadMaterialRepository HeadMaterialRepository { get; }
        public List<IHeadMaterial> HeadMaterials { get; }
        private CalculationService calculationService;
        private UnitSystemService unitSystemService;

        public IPrimitiveRepository PrimitiveRepository => primitiveRepository;

        public ICalculationProperty CalculationProperty { get; private set; }
        
        public MainModel(IPrimitiveRepository primitiveRepository, CalculationService calculationService, UnitSystemService unitSystemService)
        {
            this.primitiveRepository = primitiveRepository;
            this.calculationService = calculationService;
            this.unitSystemService = unitSystemService;

            Section = new CrossSection();
            CalculationProperty = new CalculationProperty();
            HeadMaterials = new List<IHeadMaterial>();
            HeadMaterialRepository = new HeadMaterialRepository(this);
        }
        
        //public IStrainMatrix Calculate(double mx, double my, double nz)
        //{
        //    var unitSystem = unitSystemService.GetCurrentSystem();
        //    return calculationService.GetPrimitiveStrainMatrix(primitiveRepository.Primitives.Select(x => x.GetNdmPrimitive(unitSystem)).ToArray(),
        //        mx, my, nz,
        //        CalculationProperty.LimitState, CalculationProperty.CalcTerm);
        //}

        public IEnumerable<INdm> GetNdms(ICalculationProperty calculationProperty)
        {
            var ndmPrimitives = Section.SectionRepository.Primitives;

            //Настройки триангуляции, пока опции могут быть только такие
            ITriangulationOptions options = new TriangulationOptions { LimiteState = calculationProperty.LimitState, CalcTerm = calculationProperty.CalcTerm };

            //Формируем коллекцию элементарных участков для расчета в библитеке (т.е. выполняем триангуляцию)
            List<INdm> ndmCollection = new List<INdm>();
            ndmCollection.AddRange(Triangulation.GetNdms(ndmPrimitives, options));

            return ndmCollection;
        }

        //public ILoaderResults CalculateResult(IEnumerable<INdm> ndmCollection, IForceMatrix forceMatrix)
        //{
        //    var loaderData = new LoaderOptions
        //    {
        //        Preconditions = new Preconditions
        //        {
        //            ConditionRate = 0.01,
        //            MaxIterationCount = 100,
        //            StartForceMatrix = forceMatrix
        //        },
        //        NdmCollection = ndmCollection
        //    };
        //    var calculator = new Calculator();
        //    calculator.Run(loaderData, new CancellationToken());
        //    return calculator.Result;
        //}
    }
}
