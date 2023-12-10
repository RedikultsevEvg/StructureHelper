using StructureHelper.Infrastructure.UI.Converters.Units;
using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services.Units;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.LimitCurve;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

//Copyright (c) 2023 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews.ForceResultLogic
{
    public class LimitCurveDataViewModel : OkCancelViewModelBase
    {
        //public SurroundDataViewModel SurroundDataViewModel { get; private set; }
        public SurroundData SurroundData { get; set; }

        public LimitCurveDataViewModel(SurroundData surroundData)
        {
            //SurroundDataViewModel = new(surroundData);
            SurroundData = surroundData;
        }

    }
}
