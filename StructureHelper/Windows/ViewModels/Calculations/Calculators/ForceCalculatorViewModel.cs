using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Windows.ViewModels.NdmCrossSections;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;


//Copyright (c) 2023 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.


namespace StructureHelper.Windows.ViewModels.Calculations.Calculators
{
    public class ForceCalculatorViewModel : OkCancelViewModelBase
    {
        IEnumerable<INdmPrimitive> allowedPrimitives;
        IEnumerable<IForceAction> allowedForceCombinations;
        ForceCalculator forcesCalculator;
        SecondOrderViewModel secondOrderViewModel;

        public string Name
        {
            get { return forcesCalculator.Name; }
            set { forcesCalculator.Name = value; }
        }

        public double IterationAccuracy
        {
            get { return forcesCalculator.Accuracy.IterationAccuracy; }
            set { forcesCalculator.Accuracy.IterationAccuracy = value;}
        }

        public int MaxIterationCount
        {
            get { return forcesCalculator.Accuracy.MaxIterationCount; }
            set { forcesCalculator.Accuracy.MaxIterationCount = value; }
        }

        public SecondOrderViewModel SecondOrder => secondOrderViewModel;

        public bool ULS { get; set; }
        public bool SLS { get; set; }
        public bool ShortTerm { get; set; }
        public bool LongTerm { get; set; }

        public ISourceToTargetViewModel<IForceAction> CombinationViewModel { get; }
        public ISourceToTargetViewModel<PrimitiveBase> PrimitivesViewModel { get; }

        public PrimitiveBase SelectedAllowedPrimitive { get; set; }
        public PrimitiveBase SelectedPrimitive { get; set; }

        public ObservableCollection<PrimitiveBase> AllowedPrimitives
        {
            get
            {
                var sourceItems = forcesCalculator.Primitives;
                var rejectedItems = allowedPrimitives.Where(x => sourceItems.Contains(x));
                var filteredItems = allowedPrimitives.Except(rejectedItems);
                return PrimitiveOperations.ConvertNdmPrimitivesToPrimitiveBase(filteredItems);
            }
        }
        public ObservableCollection<PrimitiveBase> Primitives
        {
            get
            {
                var sourceItems = forcesCalculator.Primitives;
                return PrimitiveOperations.ConvertNdmPrimitivesToPrimitiveBase(sourceItems);
            }
        }

        private ICommand addAllPrimitivesCommand;
        private ICommand clearAllPrimitivesCommand;
        private RelayCommand addSelectedPrimitiveCommand;
        private RelayCommand removeSelectedPrimitive;

        public ICommand AddAllPrimitivesCommand
        {
            get
            {
                return addAllPrimitivesCommand ??
                    (
                    addAllPrimitivesCommand = new RelayCommand(o =>
                    {
                        AddAllPrimitives();
                        OnPropertyChanged(nameof(AllowedPrimitives));
                        OnPropertyChanged(nameof(Primitives));
                    },o => allowedPrimitives.Count() > 0
                    ));
            }
        }
        private void AddAllPrimitives()
        {
            forcesCalculator.Primitives.Clear();
            forcesCalculator.Primitives.AddRange(allowedPrimitives);
        }
        public ICommand ClearAllPrimitivesCommand
        {
            get
            {
                return clearAllPrimitivesCommand ??
                    (
                    clearAllPrimitivesCommand = new RelayCommand(o =>
                    {
                        forcesCalculator.Primitives.Clear();
                        OnPropertyChanged(nameof(AllowedPrimitives));
                        OnPropertyChanged(nameof(Primitives));
                    }, o => forcesCalculator.Primitives.Count > 0 ));
            }
        }
        public ICommand AddSelectedPrimitiveCommand
        {
            get
            {
                return addSelectedPrimitiveCommand ??
                    (
                    addSelectedPrimitiveCommand = new RelayCommand(o =>
                    {
                        forcesCalculator.Primitives.Add(SelectedAllowedPrimitive.GetNdmPrimitive());
                        OnPropertyChanged(nameof(AllowedPrimitives));
                        OnPropertyChanged(nameof(Primitives));
                    }, o => SelectedAllowedPrimitive != null));
            }
        }
        public RelayCommand RemoveSelectedPrimitiveCommand
        {
            get
            {
                return removeSelectedPrimitive ??
                    (
                    removeSelectedPrimitive = new RelayCommand(o =>
                    {
                        forcesCalculator.Primitives.Remove(SelectedPrimitive.GetNdmPrimitive());
                        OnPropertyChanged(nameof(AllowedPrimitives));
                        OnPropertyChanged(nameof(Primitives));
                    }, o => SelectedPrimitive != null));
            }
        }

        public ForceCalculatorViewModel(IEnumerable<INdmPrimitive> _allowedPrimitives, IEnumerable<IForceAction> _allowedForceCombinations, ForceCalculator _forcesCalculator)
        {
            allowedPrimitives = _allowedPrimitives;
            allowedForceCombinations = _allowedForceCombinations;
            forcesCalculator = _forcesCalculator;
            secondOrderViewModel = new SecondOrderViewModel(forcesCalculator.CompressedMember);

            CombinationViewModel = new SourceToTargetViewModel<IForceAction>();
            CombinationViewModel.SetTargetItems(forcesCalculator.ForceActions);
            CombinationViewModel.SetSourceItems(allowedForceCombinations);
            CombinationViewModel.ItemDataDemplate = Application.Current.Resources["SimpleItemTemplate"] as DataTemplate;

            PrimitivesViewModel = new SourceToTargetViewModel<PrimitiveBase>();
            var targetItems = forcesCalculator.Primitives;
            var sourceViewPrimitives = PrimitiveOperations.ConvertNdmPrimitivesToPrimitiveBase(allowedPrimitives);
            var viewPrimitives = sourceViewPrimitives.Where(x => targetItems.Contains(x.GetNdmPrimitive()));
            PrimitivesViewModel.SetTargetItems(viewPrimitives);
            PrimitivesViewModel.SetSourceItems(sourceViewPrimitives);
            PrimitivesViewModel.ItemDataDemplate = Application.Current.Resources["ColoredItemTemplate"] as DataTemplate;

            InputRefresh();
        }

        public void InputRefresh()
        {
            ULS = forcesCalculator.LimitStatesList.Contains(LimitStates.ULS);
            SLS = forcesCalculator.LimitStatesList.Contains(LimitStates.SLS);
            ShortTerm = forcesCalculator.CalcTermsList.Contains(CalcTerms.ShortTerm);
            LongTerm = forcesCalculator.CalcTermsList.Contains(CalcTerms.LongTerm);
        }

        public void Refresh()
        {
            var combinations = CombinationViewModel.GetTargetItems();
            forcesCalculator.ForceActions.Clear();
            foreach (var item in combinations)
            {
                forcesCalculator.ForceActions.Add(item);
            }
            forcesCalculator.Primitives.Clear();
            foreach (var item in PrimitivesViewModel.GetTargetItems())
            {
                forcesCalculator.Primitives.Add(item.GetNdmPrimitive());
            }
            forcesCalculator.LimitStatesList.Clear();
            if (ULS == true) { forcesCalculator.LimitStatesList.Add(LimitStates.ULS); }
            if (SLS == true) { forcesCalculator.LimitStatesList.Add(LimitStates.SLS); }
            forcesCalculator.CalcTermsList.Clear();
            if (ShortTerm == true) { forcesCalculator.CalcTermsList.Add(CalcTerms.ShortTerm); }
            if (LongTerm == true) { forcesCalculator.CalcTermsList.Add(CalcTerms.LongTerm); }
        }
    }
}
