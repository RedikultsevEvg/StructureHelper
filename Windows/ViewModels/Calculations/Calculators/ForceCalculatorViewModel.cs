using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Strings;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.Primitives;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StructureHelper.Windows.ViewModels.Calculations.Calculators
{
    public class ForceCalculatorViewModel : ViewModelBase
    {
        IEnumerable<INdmPrimitive> allowedPrimitives;
        IEnumerable<IForceCombinationList> allowedForceCombinations;
        ForceCalculator forcesCalculator;

        public PrimitiveBase SelectedAllowedPrimitive { get; set; }
        public PrimitiveBase SelectedPrimitive { get; set; }

        public ObservableCollection<PrimitiveBase> AllowedPrimitives
        {
            get
            {
                var sourceItems = forcesCalculator.NdmPrimitives;
                var rejectedItems = allowedPrimitives.Where(x => sourceItems.Contains(x));
                var filteredItems = allowedPrimitives.Except(rejectedItems);
                return ConvertNdmPrimitivesToPrimitiveBase(filteredItems);
            }
        }
        public ObservableCollection<PrimitiveBase> Primitives
        {
            get
            {
                var sourceItems = forcesCalculator.NdmPrimitives;
                return ConvertNdmPrimitivesToPrimitiveBase(sourceItems);
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
            forcesCalculator.NdmPrimitives.Clear();
            forcesCalculator.NdmPrimitives.AddRange(allowedPrimitives);
        }

        public ICommand ClearAllPrimitivesCommand
        {
            get
            {
                return clearAllPrimitivesCommand ??
                    (
                    clearAllPrimitivesCommand = new RelayCommand(o =>
                    {
                        forcesCalculator.NdmPrimitives.Clear();
                        OnPropertyChanged(nameof(AllowedPrimitives));
                        OnPropertyChanged(nameof(Primitives));
                    }, o => forcesCalculator.NdmPrimitives.Count > 0 ));
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
                        forcesCalculator.NdmPrimitives.Add(SelectedAllowedPrimitive.GetNdmPrimitive());
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
                        forcesCalculator.NdmPrimitives.Remove(SelectedPrimitive.GetNdmPrimitive());
                        OnPropertyChanged(nameof(AllowedPrimitives));
                        OnPropertyChanged(nameof(Primitives));
                    }, o => SelectedPrimitive != null));
            }
        }

        public ForceCalculatorViewModel(IEnumerable<INdmPrimitive> _allowedPrimitives, IEnumerable<IForceCombinationList> _allowedForceCombinations, ForceCalculator _forcesCalculator)
        {
            allowedPrimitives = _allowedPrimitives;
            allowedForceCombinations = _allowedForceCombinations;
            forcesCalculator = _forcesCalculator;
        }

        private ObservableCollection<PrimitiveBase> ConvertNdmPrimitivesToPrimitiveBase(IEnumerable<INdmPrimitive> primitives)
        {
            ObservableCollection<PrimitiveBase> viewItems = new ObservableCollection<PrimitiveBase>();
            foreach (var item in primitives)
            {
                if (item is IPointPrimitive)
                {
                    var point = item as IPointPrimitive;
                    viewItems.Add(new PointViewPrimitive(point));
                }
                else if (item is IRectanglePrimitive)
                {
                    var rect = item as IRectanglePrimitive;
                    viewItems.Add(new RectangleViewPrimitive(rect));
                }
                else throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown);
            }
            return viewItems;
        }
    }
}
