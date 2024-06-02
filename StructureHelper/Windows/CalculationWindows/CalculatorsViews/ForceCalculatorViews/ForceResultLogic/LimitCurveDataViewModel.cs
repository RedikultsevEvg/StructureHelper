using StructureHelper.Infrastructure.UI.Converters.Units;
using StructureHelper.Infrastructure.UI.DataContexts;
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
        private LimitCurveInputData inputData;


        //public SurroundDataViewModel SurroundDataViewModel { get; private set; }
        public SurroundData SurroundData { get => inputData.SurroundData; }
        public SelectPrimitivesSourceTarget PrimitiveSeries { get; private set; }
        public SelectItemsVM<PredicateEntry> PredicateItems { get; private set; }
        public SelectItemsVM<LimitStateEntity> LimitStateItems { get; private set; }
        public SelectItemsVM<CalcTermEntity> CalcTermITems { get; private set; }

        public int PointCount
        {
            get => inputData.PointCount; set
            {
                try
                {
                    inputData.PointCount = value;
                }
                catch (Exception)
                {
                    inputData.PointCount = 40;
                }
                OnPropertyChanged(nameof(PointCount));
            }
        }

        public IEnumerable<INdmPrimitive> AllowedPrimitives { get; set; }

        public LimitCurveDataViewModel(LimitCurveInputData inputData, IEnumerable<INdmPrimitive> allowedPrimitives)
        {
            this.inputData = inputData;
            AllowedPrimitives = allowedPrimitives;
            List<NamedValue<SourceTargetVM<PrimitiveBase>>> namedCollection = new List<NamedValue<SourceTargetVM<PrimitiveBase>>>();
            foreach (var item in inputData.PrimitiveSeries)
            {
                var viewModel = SourceTargetFactory.GetSourceTargetVM(AllowedPrimitives, item.Collection);
                var namedViewModel = new NamedValue<SourceTargetVM<PrimitiveBase>>()
                {
                    Name = item.Name,
                    Value = viewModel
                };
                namedCollection.Add(namedViewModel);
            }
            PrimitiveSeries = new SelectPrimitivesSourceTarget(namedCollection);
            PrimitiveSeries.AllowedPrimitives = allowedPrimitives.ToList();
            GetPredicates();
            GetLimitStates();
            GetCalcTerms();
        }

        public void RefreshInputData()
        {
            inputData.LimitStates.Clear();
            inputData.LimitStates.AddRange(LimitStateItems.SelectedItems.Select(x => x.LimitState));
            inputData.CalcTerms.Clear();
            inputData.CalcTerms.AddRange(CalcTermITems.SelectedItems.Select(x => x.CalcTerm));
            inputData.PredicateEntries.Clear();
            inputData.PredicateEntries.AddRange(PredicateItems.SelectedItems);
            inputData.PrimitiveSeries.Clear();
            foreach (var item in PrimitiveSeries.Collection)
            {
                var selectesPrimitives = item.Value.TargetItems.Select(x => x.NdmPrimitive).ToList();
                inputData.PrimitiveSeries.Add
                    (
                        new NamedCollection<INdmPrimitive>()
                        {
                            Name = item.Name,
                            Collection = selectesPrimitives
                        }
                    );
            }
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
        private void GetCalcTerms()
        {
            CalcTermITems = new SelectItemsVM<CalcTermEntity>(ProgramSetting.CalcTermList.CalcTerms);
            var selectedItems = ProgramSetting.CalcTermList.CalcTerms.Where(x => inputData.CalcTerms.Contains(x.CalcTerm));
            CalcTermITems.SelectedItems = selectedItems;
            CalcTermITems.ShowButtons = true;
        }
        private void GetLimitStates()
        {
            LimitStateItems = new SelectItemsVM<LimitStateEntity>(ProgramSetting.LimitStatesList.LimitStates);
            var selectedItems = ProgramSetting.LimitStatesList.LimitStates.Where(x => inputData.LimitStates.Contains(x.LimitState));
            LimitStateItems.SelectedItems = selectedItems;
            LimitStateItems.ShowButtons = true;

        }
        private void GetPredicates()
        {
            PredicateItems = new SelectItemsVM<PredicateEntry>(
            new List<PredicateEntry>()
            {
                new PredicateEntry()
                { Name = "Strength", PredicateType = PredicateTypes.Strength },
                new PredicateEntry()
                { Name = "Cracking", PredicateType = PredicateTypes.Cracking },
            }
                );
            PredicateItems.SelectedItems = inputData.PredicateEntries;
            PredicateItems.ShowButtons = true;
        }
    }
}
