using StructureHelper.Infrastructure;
using StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews.DeformedShapes;
using StructureHelper.Windows.Graphs;
using StructureHelper.Windows.PrimitivePropertiesWindow;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Input;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    public class DeformedShapeViewerViewModel : ViewModelBase
    {
        private RelayCommand rebuildCommand;
        private double scaleFactor = 100.0;
        private int divisionNumber = 20;
        private IEnumerable<INdmPrimitive> ndmPrimitives;
        private bool isCapsShown = false;
        private double length = 3.0;
        private bool considerResultCurvature = true;
        private bool considerPrestrainCurvature = false;
        private bool createMesh = false;
        private DeformedShapeSymmetrySet symmetrySet = new();

        public IForceTuple Curvature { get; set; }
        public SelectPrimitivesViewModel SelectPrimitivesViewModel { get; }

        public DeformedShapeSymmetrySetViewModel SymmetrySet { get; set; } = new();

        public double Length
        {
            get => length;
            set
            {
                try
                {
                    length = value;
                    OnPropertyChanged(nameof(Length));
                    Rebuild();
                }
                catch { }
            }
        }

        public double ScaleFactor
        {
            get => scaleFactor;
            set
            {
                try
                {
                    scaleFactor = value;
                    OnPropertyChanged(nameof(ScaleFactor));
                    Rebuild();
                }
                catch { }
            }
        }

        public int DivisionNumber
        {
            get => divisionNumber;
            set
            {
                try
                {
                    divisionNumber = value;
                    OnPropertyChanged(nameof(DivisionNumber));
                    Rebuild();
                }
                catch { }
            }
        }

        public bool IsCapsShown
        {
            get => isCapsShown;
            set
            {
                isCapsShown = value;
                OnPropertyChanged(nameof(IsCapsShown));
                Rebuild();
            }
        }

        public bool ConsiderResultCurvature
        {
            get => considerResultCurvature;
            set
            {
                considerResultCurvature = value;
                OnPropertyChanged(nameof(ConsiderResultCurvature));
                Rebuild();
            }
        }

        public bool ConsiderPrestrainCurvature
        {
            get => considerPrestrainCurvature;
            set
            {
                considerPrestrainCurvature = value;
                OnPropertyChanged(nameof(ConsiderPrestrainCurvature));
                Rebuild();
            }
        }

        public bool CreateMesh
        {
            get => createMesh;
            set
            {
                createMesh = value;
                OnPropertyChanged(nameof(CreateMesh));
                Rebuild();
            }
        }

        public ContourViewportViewModel ViewportViewModel { get; } = new ContourViewportViewModel();
        public SaveCopyFWElementViewModel SaveCopyViewModel { get; private set; } = new();

        public ICommand RebuildCommand => rebuildCommand ??= new RelayCommand(Rebuild);
        public void Rebuild(object? commandParameter = null)
        {
            var selectedNdmPrimitives = SelectPrimitivesViewModel.Items.CollectionItems.Where(x => x.IsSelected == true).Select(x => x.Item.GetNdmPrimitive());
            var logic = new DeformedShapeLogic()
            {
                NdmPrimitives = selectedNdmPrimitives,
                Viewport = ViewportViewModel.Viewport3D,
                ResultCurvature = Curvature,
                ScaleFactor = (float)ScaleFactor,
                DivisionNumber = DivisionNumber,
                IsCapsShown = IsCapsShown,
                Length = (float)Length,
                ConsiderResultCurvature = ConsiderResultCurvature,
                ConsiderPrestrainCurvature = ConsiderPrestrainCurvature,
                CreateMesh = CreateMesh,
                SymmetrySet = SymmetrySet,
            };
            logic.GetModels3d();
        }

        public DeformedShapeViewerViewModel(IEnumerable<INdmPrimitive> ndmPrimitives)
        {
            this.ndmPrimitives = ndmPrimitives;
            SelectPrimitivesViewModel = new(this.ndmPrimitives);

            foreach (var item in symmetrySet.SymmetrySet)
            {
                SymmetrySet.SymmetrySets.Add(new DeformedShapeSymmetryViewModel(item));
            }
        }

    }
}
