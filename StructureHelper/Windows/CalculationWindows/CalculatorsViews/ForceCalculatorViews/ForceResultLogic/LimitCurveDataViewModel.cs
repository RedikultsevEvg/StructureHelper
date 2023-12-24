using StructureHelper.Infrastructure.UI.Converters.Units;
using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Parameters;
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
        public SelectItemsViewModel<PredicateEntry> PredicateItems { get; private set; }
        public SelectItemsViewModel<LimitStateEntity> LimitStateItems { get; private set; }
        public SelectItemsViewModel<CalcTermEntity> CalcTermITems { get; private set; }
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
            GetPredicates();
            GetLimitStates();
            GetCalcTerms();
            pointCount = 80;
        }

        private void GetCalcTerms()
        {
            CalcTermITems = new SelectItemsViewModel<CalcTermEntity>(ProgramSetting.CalcTermList.CalcTerms);
            CalcTermITems.ShowButtons = true;
        }

        private void GetLimitStates()
        {
            LimitStateItems = new SelectItemsViewModel<LimitStateEntity>(ProgramSetting.LimitStatesList.LimitStates);
            LimitStateItems.ShowButtons = true;
        }

        private void GetPredicates()
        {
            PredicateItems = new SelectItemsViewModel<PredicateEntry>(
            new List<PredicateEntry>()
            {
                new PredicateEntry()
                { Name = "Strength", PredicateType = PredicateTypes.Strength },
                new PredicateEntry()
                { Name = "Cracking", PredicateType = PredicateTypes.Cracking },
            }
                );
            PredicateItems.ShowButtons = true;
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
            inputData.LimitStates.AddRange(LimitStateItems.SelectedItems.Select(x => x.LimitState));
            inputData.CalcTerms.AddRange(CalcTermITems.SelectedItems.Select(x => x.CalcTerm));
            inputData.PredicateEntries.AddRange(PredicateItems.SelectedItems);
            inputData.Primitives = Primitives;
            return inputData;
        }

        public bool Check()
        {
            if (PredicateItems.SelectedCount == 0 ||
                LimitStateItems.SelectedCount == 0 ||
                CalcTermITems.SelectedCount == 0)
            {
                return false;
            }
            return true;
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
