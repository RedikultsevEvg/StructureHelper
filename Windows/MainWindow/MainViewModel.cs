using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Logics.Geometry;
using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.MaterialCatalogWindow;
using StructureHelper.Models.Materials;
using StructureHelper.Windows.CalculationWindows.CalculationPropertyWindow;
using StructureHelper.Windows.CalculationWindows.CalculationResultWindow;
using StructureHelper.Windows.ColorPickerWindow;
using StructureHelper.Windows.MainWindow.Materials;
using StructureHelper.Windows.PrimitiveTemplates.RCs.RectangleBeam;
using StructureHelper.Windows.ViewModels.Calculations.CalculationProperies;
using StructureHelper.Windows.ViewModels.Calculations.CalculationResult;
using StructureHelper.Windows.ViewModels.Forces;
using StructureHelper.Windows.ViewModels.NdmCrossSections;
using StructureHelperCommon.Infrastructures.Strings;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.Calculations.CalculationProperties;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.Models.Templates.CrossSections.RCs;
using StructureHelperLogics.Models.Templates.RCs;
using StructureHelperLogics.Services.NdmCalculations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

namespace StructureHelper.Windows.MainWindow
{
    public class MainViewModel : ViewModelBase
    {
        const double scale = 1d;

        ICrossSection section;
        ICrossSectionRepository repository => section.SectionRepository;

        private double ConstAxisLineThickness = 2d * scale;
        private double ConstGridLineThickness = 0.25d * scale;

        private readonly double scaleRate = 1.1d;

        public PrimitiveBase SelectedPrimitive { get; set; }
        public IForceCombinationList SelectedForceCombinationList { get; set; }

        private readonly ICalculatorsViewModelLogic calculatorsLogic;
        public ICalculatorsViewModelLogic CalculatorsLogic { get => calculatorsLogic;}
        public ActionsViewModel CombinationsLogic { get => combinationsLogic; }
        public IPrimitiveViewModelLogic PrimitiveLogic => primitiveLogic;

        private MainModel Model { get; }

        private double panelX, panelY, scrollPanelX, scrollPanelY;
        private CalculationProperty calculationProperty;

        public double PanelX
        {
            get => panelX;
            set => OnPropertyChanged(value, ref panelX);
        }
        public double PanelY
        {
            get => panelY;
            set => OnPropertyChanged(value, ref panelY);
        }
        public double ScrollPanelX
        {
            get => scrollPanelX;
            set => OnPropertyChanged(value, ref scrollPanelX);
        }
        public double ScrollPanelY
        {
            get => scrollPanelY;
            set => OnPropertyChanged(value, ref scrollPanelY);
        }

        private double scaleValue;

        public double ScaleValue
        {
            get => Math.Round(scaleValue);
            set
            {
                OnPropertyChanged(value, ref scaleValue);
                axisLineThickness = ConstAxisLineThickness / scaleValue;
                OnPropertyChanged(nameof(AxisLineThickness));
                gridLineThickness = ConstGridLineThickness / scaleValue;
                OnPropertyChanged(nameof(GridLineThickness));
            }
        }

        public double AxisLineThickness
        { 
            get => axisLineThickness;
        }

        public double GridLineThickness
        {
            get => gridLineThickness;
        }

        private double canvasWidth, canvasHeight, xX2, xY1, yX1, yY2;
        public double CanvasWidth
        {
            get => canvasWidth;
            set => OnPropertyChanged(value, ref canvasWidth);
        }

        public double CanvasHeight
        {
            get => canvasHeight;
            set => OnPropertyChanged(value, ref canvasHeight);
        }
   
        public ObservableCollection<IHeadMaterial> HeadMaterials
        {
            get
            {
                var collection = new ObservableCollection<IHeadMaterial>();
                foreach (var obj in Model.Section.SectionRepository.HeadMaterials)
                {
                    collection.Add(obj);
                }
                return collection;
            }
        }

        public double XX2
        {
            get => xX2;
            set => OnPropertyChanged(value, ref xX2);
        }
        public double XY1
        {
            get => xY1;
            set => OnPropertyChanged(value, ref xY1);
        }
        public double YX1
        {
            get => yX1;
            set => OnPropertyChanged(value, ref yX1);
        }
        public double YY2
        {
            get => yY2;
            set => OnPropertyChanged(value, ref yY2);
        }

        public ICommand Calculate { get; }
        public ICommand EditCalculationPropertyCommand { get; }
        public ICommand EditHeadMaterialsCommand { get; }
        public ICommand AddBeamCase { get; }
        public ICommand AddColumnCase { get; }
        public ICommand AddSlabCase { get; }
        public ICommand LeftButtonDown { get; }
        public ICommand LeftButtonUp { get; }
        public ICommand MovePrimitiveToGravityCenterCommand { get; }
        public ICommand PreviewMouseMove { get; }
        public ICommand ClearSelection { get; }
        public ICommand OpenMaterialCatalog { get; }
        public ICommand OpenMaterialCatalogWithSelection { get; }
        public ICommand OpenUnitsSystemSettings { get; }
        public ICommand SetColor { get; }
        public ICommand SetInFrontOfAll { get; }
        public ICommand SetInBackOfAll { get; }
        public ICommand ScaleCanvasDown { get; }
        public ICommand ScaleCanvasUp { get; }
        public ICommand SetPopupCanBeClosedTrue { get; }
        public ICommand SetPopupCanBeClosedFalse { get; }

        private double delta = 0.0005;
        private double axisLineThickness;
        private double gridLineThickness;
        private ActionsViewModel combinationsLogic;
        private IPrimitiveViewModelLogic primitiveLogic;

        public MainViewModel(MainModel model)
        {
            Model = model;
            section = model.Section;
            combinationsLogic = new ActionsViewModel(repository);
            calculatorsLogic = new CalculatorsViewModelLogic(repository);
            CanvasWidth = 2d * scale;
            CanvasHeight = 1.5d * scale;
            primitiveLogic = new PrimitiveViewModelLogic(repository) { CanvasWidth = CanvasWidth, CanvasHeight = CanvasHeight };
            XX2 = CanvasWidth;
            XY1 = CanvasHeight / 2d;
            YX1 = CanvasWidth / 2d;
            YY2 = CanvasHeight;
            scaleValue = 400d / scale;
            axisLineThickness = ConstAxisLineThickness / scaleValue;
            gridLineThickness = ConstGridLineThickness / scaleValue;
            calculationProperty = new CalculationProperty();

            LeftButtonUp = new RelayCommand(o =>
            {
                if (o is RectangleViewPrimitive rect) rect.BorderCaptured = false;
            });
            LeftButtonDown = new RelayCommand(o =>
            {
                if (o is RectangleViewPrimitive rect) rect.BorderCaptured = true;
            });
            PreviewMouseMove = new RelayCommand(o =>
            {
                if (o is RectangleViewPrimitive rect && rect.BorderCaptured && !rect.ElementLock)
                {
                    if (rect.PrimitiveWidth % 10d < delta || rect.PrimitiveWidth % 10d >= delta)
                        rect.PrimitiveWidth = Math.Round(PanelX / 10d) * 10d - rect.PrimitiveLeft + 10d;
                    else
                        rect.PrimitiveWidth = PanelX - rect.PrimitiveLeft + 10d;

                    if (rect.PrimitiveHeight % 10d < delta || rect.PrimitiveHeight % 10d >= delta)
                        rect.PrimitiveHeight = Math.Round(PanelY / 10d) * 10d - rect.PrimitiveTop + 10d;
                    else
                        rect.PrimitiveHeight = PanelY - rect.PrimitiveTop + 10d;
                }
            });
            EditHeadMaterialsCommand = new RelayCommand(o => EditHeadMaterials());
            OpenMaterialCatalog = new RelayCommand(o =>
            {
                var materialCatalogView = new MaterialCatalogView();
                materialCatalogView.ShowDialog();
            });
            OpenMaterialCatalogWithSelection = new RelayCommand(o =>
            {
                var primitive = o as PrimitiveBase;
                var materialCatalogView = new MaterialCatalogView(true, primitive);
                materialCatalogView.ShowDialog();
            });

            SetColor = new RelayCommand(o =>
            {
                var primitive = o as PrimitiveBase;
                var colorPickerView = new ColorPickerView(primitive);
                colorPickerView.ShowDialog();
            });

            ScaleCanvasDown = new RelayCommand(o =>
            {
                ScrollPanelX = PanelX;
                ScrollPanelY = PanelY;
                ScaleValue *= scaleRate;
            });

            ScaleCanvasUp = new RelayCommand(o =>
            {
                ScrollPanelX = PanelX;
                ScrollPanelY = PanelY;
                ScaleValue /= scaleRate;
            });

            AddBeamCase = new RelayCommand(o =>
            {
                PrimitiveLogic.AddItems(GetBeamCasePrimitives());
                //OnPropertyChanged(nameof(PrimitivesCount));
            });

            AddColumnCase = new RelayCommand(o =>
            {
                PrimitiveLogic.AddItems(GetColumnCasePrimitives());
            });

            AddSlabCase = new RelayCommand(o =>
            {
                PrimitiveLogic.AddItems(GetSlabCasePrimitives());
            });

            Calculate = new RelayCommand(o =>
            {
                CalculateResult();
            },
            o => repository.Primitives.Count() > 0);

            EditCalculationPropertyCommand = new RelayCommand (o => EditCalculationProperty());

            MovePrimitiveToGravityCenterCommand = new RelayCommand(o =>
            {
                if (CheckMaterials() == false) { return;}
                IEnumerable<INdm> ndms = Model.GetNdms(calculationProperty);
                double[] center = GeometryOperations.GetGravityCenter(ndms);
                foreach (var primitive in Model.PrimitiveRepository.Primitives)
                {
                    primitive.CenterX -= center[0];
                    primitive.CenterY -= center[1];
                }
            },
            o => repository.Primitives.Count() > 0
            );

            SetPopupCanBeClosedTrue = new RelayCommand(o =>
            {
                if (!(o is PrimitiveBase primitive)) return;
                primitive.PopupCanBeClosed = true;
            });

            SetPopupCanBeClosedFalse = new RelayCommand(o =>
            {
                if (!(o is PrimitiveBase primitive)) return;
                primitive.PopupCanBeClosed = false;
            });
        }

        private void EditHeadMaterials()
        {
            var wnd = new HeadMaterialsView(repository);
            wnd.ShowDialog();
            OnPropertyChanged(nameof(HeadMaterials));
            foreach (var primitive in primitiveLogic.Items)
            {
                primitive.RefreshColor();
            }
        }

        private void CalculateResult()
        {
            bool check = CheckAnalisysOptions();
            if (check == false)
            {
                MessageBox.Show(ErrorStrings.DataIsInCorrect, "Check data for analisys", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                IEnumerable<INdm> ndms = Model.GetNdms(calculationProperty);
                CalculationService calculationService = new CalculationService(calculationProperty);
                var loaderResults = calculationService.GetCalculationResults(ndms);
                var wnd = new CalculationResultView(new CalculationResultViewModel(loaderResults, ndms));
                wnd.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ErrorStrings.UnknownError}: {ex}", "Check data for analisys", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private bool CheckAnalisysOptions()
        {
            if (CheckMaterials() == false) { return false; }
            return true;
        }
        private bool CheckMaterials()
        {
            foreach (var item in primitiveLogic.Items)
            {
                if (item.HeadMaterial == null)
                {
                    MessageBox.Show($"Primitive {item.Name} does not has material", "Check data for analisys", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            return true;
        }
        private IEnumerable<PrimitiveBase> GetBeamCasePrimitives()
        {
            var template = new RectangleBeamTemplate();
            return GetCasePrimitives(template);
        }
        private IEnumerable<PrimitiveBase> GetColumnCasePrimitives()
        {
            var template = new RectangleBeamTemplate(0.5d, 0.5d) { CoverGap = 0.05, WidthCount = 3, HeightCount = 3, TopDiameter = 0.025d, BottomDiameter = 0.025d };
            return GetCasePrimitives(template);
        }
        private IEnumerable<PrimitiveBase> GetSlabCasePrimitives()
        {
            var template = new RectangleBeamTemplate(1d, 0.2d) { CoverGap = 0.04, WidthCount = 5, HeightCount = 2, TopDiameter = 0.012d, BottomDiameter = 0.012d };
            return GetCasePrimitives(template);
        }
        private void EditCalculationProperty()
        {
            CalculationPropertyViewModel viewModel = new CalculationPropertyViewModel(calculationProperty);
            var view = new CalculationPropertyView(viewModel);
            view.ShowDialog();
        }
        private IEnumerable<PrimitiveBase> GetCasePrimitives(RectangleBeamTemplate template)
        {
            var wnd = new RectangleBeamView(template);
            wnd.ShowDialog();
            if (wnd.DialogResult == true)
            {
                var newSection = new SectionTemplate(new RectGeometryLogic(template)).GetCrossSection();
                var newRepository = newSection.SectionRepository;
                repository.HeadMaterials.AddRange(newRepository.HeadMaterials);
                repository.Primitives.AddRange(newRepository.Primitives);
                repository.ForceCombinationLists.AddRange(newRepository.ForceCombinationLists);
                repository.CalculatorsList.AddRange(newRepository.CalculatorsList);
                OnPropertyChanged(nameof(HeadMaterials));
                CombinationsLogic.AddItems(newRepository.ForceCombinationLists);
                CalculatorsLogic.AddItems(newRepository.CalculatorsList);
                var primitives = PrimitiveOperations.ConvertNdmPrimitivesToPrimitiveBase(newRepository.Primitives);
                foreach (var item in primitives)
                {
                    item.RegisterDeltas(CanvasWidth / 2, CanvasHeight / 2);
                }
                PrimitiveLogic.Refresh();
                return primitives;
            }
            return new List<PrimitiveBase>();
        }
    }
}