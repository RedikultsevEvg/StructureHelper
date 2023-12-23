using StructureHelper.Infrastructure.UI.Converters.Units;
using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services.Units;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.LimitCurve;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

//Copyright (c) 2023 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews.ForceResultLogic
{
    public class LimitCurveDataViewModel : OkCancelViewModelBase, IDataErrorInfo
    {
        private int pointCount;

        //public SurroundDataViewModel SurroundDataViewModel { get; private set; }
        public SurroundData SurroundData { get; set; }
        public List<INdmPrimitive> Primitives { get; set; }
        public int PointCount
        {
            get => pointCount; set
            {
                try
                {
                    pointCount = value;
                }
                catch (Exception)
                {
                    pointCount = 40;
                }
                OnPropertyChanged(nameof(PointCount));
            }
        }

        public LimitCurveDataViewModel(SurroundData surroundData)
        {
            //SurroundDataViewModel = new(surroundData);
            SurroundData = surroundData;
            pointCount = 80;
        }

        public LimitCurveDataViewModel() : this (new SurroundData())
        {
        }

        public LimitCurveInputData GetLimitCurveInputData()
        {
            LimitCurveInputData inputData = new()
            {
                SurroundData = SurroundData,
                PointCount = pointCount
            };
            inputData.LimitStates.Add(LimitStates.ULS);
            inputData.LimitStates.Add(LimitStates.SLS);
            inputData.CalcTerms.Add(CalcTerms.ShortTerm);
            inputData.CalcTerms.Add(CalcTerms.LongTerm);
            inputData.PredicateEntries.Add(new PredicateEntry() { Name = "Strength", PredicateType = PredicateTypes.Strength });
            inputData.PredicateEntries.Add(new PredicateEntry() { Name = "Cracking", PredicateType = PredicateTypes.Cracking });
            inputData.Primitives = Primitives;
            return inputData;
        }

        public string Error => throw new NotImplementedException();

        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
                {
                    case nameof(PointCount):
                        if (PointCount < 24)
                        {
                            error = "Point count must be greater than 24";
                        }
                        break;
                }
                return error;
            }
        }
    }
}
