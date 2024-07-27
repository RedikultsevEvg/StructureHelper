using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Windows.ViewModels.NdmCrossSections;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;


//Copyright (c) 2023 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.


namespace StructureHelper.Windows.ViewModels.Calculations.Calculators
{
    public class ForceCalculatorViewModel : OkCancelViewModelBase
    {
        ForceCalculator forcesCalculator;

        public string Name
        {
            get { return forcesCalculator.Name; }
            set { forcesCalculator.Name = value; }
        }

        public ForceCalculatorInputDataVM InputData { get; }

        public ForceCalculatorViewModel(IEnumerable<INdmPrimitive> allowedPrimitives, IEnumerable<IForceAction> allowedCombinations, ForceCalculator forcesCalculator)
        {
            this.forcesCalculator = forcesCalculator;
            InputData = new ForceCalculatorInputDataVM(this.forcesCalculator.InputData, allowedPrimitives, allowedCombinations);
        }

        internal void Refresh()
        {
            InputData.Refresh();
        }
    }
}
