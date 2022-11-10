using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.Enums;
using StructureHelper.Infrastructure.Extensions;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.MaterialCatalogWindow;
using StructureHelper.Services;
using StructureHelper.Windows.ColorPickerWindow;
using StructureHelper.UnitSystem;
using StructureHelper.Models.Materials;
using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using StructureHelper.Services.ResultViewers;
using StructureHelper.Windows.ViewModels.Calculations.CalculationProperies;
using StructureHelperLogics.Models.Calculations.CalculationProperties;
using StructureHelper.Windows.CalculationWindows.CalculationPropertyWindow;
using StructureHelperLogics.Services;
using StructureHelper.Windows.CalculationWindows.CalculationResultWindow;
using StructureHelper.Windows.ViewModels.Calculations.CalculationResult;
using StructureHelper.Services.Primitives;
using StructureHelper.Windows.PrimitiveProperiesWindow;
using StructureHelper.Windows.MainWindow.Materials;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Strings;
using StructureHelperLogics.Models.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperLogics.Models.Materials.Factories;

namespace StructureHelper.Windows.MainWindow
{
    public class MainViewModel : ViewModelBase
    {
        private List<IHeadMaterial> headMaterials;
        private readonly double scaleRate = 1.1;
        
        private IPrimitiveRepository PrimitiveRepository { get; }
        public PrimitiveBase SelectedPrimitive { get; set; }

        private readonly UnitSystemService unitSystemService;

        private MainModel Model { get; }
        public ObservableCollection<PrimitiveBase> Primitives { get; set; }

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

        public int PrimitivesCount => Primitives.Count;
        private double scaleValue = 1.0;

        public double ScaleValue
        {
            get => scaleValue;
            set => OnPropertyChanged(value, ref scaleValue);
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
                foreach (var obj in headMaterials)
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
        public ICommand AddPrimitive { get; }
        public ICommand Calculate { get; }
        public ICommand DeletePrimitive { get; }
        public ICommand EditCalculationPropertyCommand { get; }
        public ICommand EditHeadMaterialsCommand { get; }
        public ICommand EditPrimitive { get; }
        public ICommand AddTestCase { get; }
        public ICommand LeftButtonDown { get; }
        public ICommand LeftButtonUp { get; }
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

        public string UnitsSystemName => unitSystemService.GetCurrentSystem().Name;

        private double delta = 0.0005;

        public MainViewModel(MainModel model, IPrimitiveRepository primitiveRepository, UnitSystemService unitSystemService)
        {
            PrimitiveRepository = primitiveRepository;
            Model = model;
            headMaterials = Model.HeadMaterialRepository.HeadMaterials;
            this.unitSystemService = unitSystemService;
            CanvasWidth = 1500;
            CanvasHeight = 1000;
            XX2 = CanvasWidth;
            XY1 = CanvasHeight / 2d;
            YX1 = CanvasWidth / 2d;
            YY2 = CanvasHeight;
            calculationProperty = new CalculationProperty();

            LeftButtonUp = new RelayCommand(o =>
            {
                if (o is Rectangle rect) rect.BorderCaptured = false;
            });
            LeftButtonDown = new RelayCommand(o =>
            {
                if (o is Rectangle rect) rect.BorderCaptured = true;
            });
            PreviewMouseMove = new RelayCommand(o =>
            {
                if (o is Rectangle rect && rect.BorderCaptured && !rect.ElementLock)
                {
                    if (rect.PrimitiveWidth % 10d < delta || rect.PrimitiveWidth % 10d >= delta)
                        rect.PrimitiveWidth = Math.Round(PanelX / 10d) * 10d - rect.X + 10d;
                    else
                        rect.PrimitiveWidth = PanelX - rect.X + 10d;

                    if (rect.PrimitiveHeight % 10d < delta || rect.PrimitiveHeight % 10d >= delta)
                        rect.PrimitiveHeight = Math.Round(PanelY / 10d) * 10d - rect.Y + 10d;
                    else
                        rect.PrimitiveHeight = PanelY - rect.Y + 10d;
                }
            });
            ClearSelection = new RelayCommand(o =>
            {
                var primitive = Primitives?.FirstOrDefault(x => x.ParamsPanelVisibilty);
                if (primitive != null && primitive.PopupCanBeClosed)
                {
                    primitive.ParamsPanelVisibilty = false;
                    primitive.ParameterCaptured = false;
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
            OpenUnitsSystemSettings = new RelayCommand(o =>
            {
                OnPropertyChanged(nameof(UnitsSystemName));
            });
            SetColor = new RelayCommand(o =>
            {
                var primitive = o as PrimitiveBase;
                var colorPickerView = new ColorPickerView(primitive);
                colorPickerView.ShowDialog();
            });
            SetInFrontOfAll = new RelayCommand(o =>
            {
                if (!(o is PrimitiveBase primitive)) return;
                foreach (var primitiveDefinition in Primitives)
                    if (primitiveDefinition.ShowedZIndex > primitive.ShowedZIndex && primitiveDefinition != primitive)
                        primitiveDefinition.ShowedZIndex--;
                primitive.ShowedZIndex = PrimitivesCount;
                OnPropertyChanged(nameof(primitive.ShowedZIndex));
            });
            SetInBackOfAll = new RelayCommand(o =>
            {
                if (!(o is PrimitiveBase primitive)) return;
                foreach (var primitiveDefinition in Primitives)
                    if (primitiveDefinition != primitive && primitiveDefinition.ShowedZIndex < primitive.ShowedZIndex)
                        primitiveDefinition.ShowedZIndex++;
                primitive.ShowedZIndex = 1;
                OnPropertyChanged(nameof(primitive.ShowedZIndex));
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

            Primitives = new ObservableCollection<PrimitiveBase>();

            AddPrimitive = new RelayCommand(o =>
            {
                if (!(o is PrimitiveType primitiveType)) return;
                PrimitiveBase primitive;
                if (primitiveType ==  PrimitiveType.Rectangle)
                {
                    primitive = new Rectangle(0.60, 0.40, 0, 0, this);
                }
                else if (primitiveType == PrimitiveType.Point)
                {
                    primitive = new Point(0.0005d, 0d, 0d, this);
                }
                else { throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown + nameof(primitiveType)); }
                Primitives.Add(primitive);
                PrimitiveRepository.Add(primitive);
            });

            DeletePrimitive = new RelayCommand(
                o=>DeleteSelectedPrimitive(),
                o => SelectedPrimitive != null
            );

            EditPrimitive = new RelayCommand(
                o => EditSelectedPrimitive(),
                o => SelectedPrimitive != null
            );

            AddTestCase = new RelayCommand(o =>
            {
                foreach (var primitive in GetTestCasePrimitives())
                {
                    Primitives.Add(primitive);
                    PrimitiveRepository.Add(primitive);
                }
                AddTestLoads();
            });

            Calculate = new RelayCommand(o =>
            {
                CalculateResult();
            });

            EditCalculationPropertyCommand = new RelayCommand (o => EditCalculationProperty());

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
            var wnd = new HeadMaterialsView(Model.HeadMaterialRepository);
            wnd.ShowDialog();
            headMaterials = Model.HeadMaterialRepository.HeadMaterials;
            OnPropertyChanged(nameof(headMaterials));
        }

        private void DeleteSelectedPrimitive()
        {
            if (! (SelectedPrimitive is null))
            {
                var dialogResult = MessageBox.Show("Delete primitive?", "Please, confirm deleting", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    Primitives.Remove(SelectedPrimitive);
                    PrimitiveRepository.Delete(SelectedPrimitive);
                }
            }
            else { MessageBox.Show("Selection is changed", "Please, select primitive", MessageBoxButtons.YesNo, MessageBoxIcon.Warning); }
        }

        private void EditSelectedPrimitive()
        {
            if (!(SelectedPrimitive is null))
            {
                var wnd = new PrimitiveProperties(SelectedPrimitive, Model.HeadMaterialRepository);
                wnd.ShowDialog();
                OnPropertyChanged(nameof(headMaterials));
            }
            else { MessageBox.Show("Selection is changed", "Please, select primitive", MessageBoxButtons.YesNo, MessageBoxIcon.Warning); }
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
                IEnumerable<INdm> ndms = Model.GetNdms();
                CalculationService calculationService = new CalculationService();
                var loaderResults = calculationService.GetCalculationResults(calculationProperty, ndms);
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
            foreach (var item in PrimitiveRepository.Primitives)
            {
                if (item.HeadMaterial == null)
                {
                    MessageBox.Show($"Primitive {item.Name} does not has material", "Check data for analisys", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            return true;
        }

        private IEnumerable<PrimitiveBase> GetTestCasePrimitives()
        {
            var width = 0.4d;
            var height = 0.6d;
            var area1 = Math.PI * 0.012d * 0.012d / 4d;
            var area2 = Math.PI * 0.025d * 0.025d / 4d;
            var gap = 0.05d;

            var rectMaterial = new ConcreteDefinition("C40", 0, 40, 0, 1.3, 1.5);
            var pointMaterial = new RebarDefinition("S400", 2, 400, 400, 1.15, 1.15);

            IHeadMaterial concrete = new HeadMaterial() { Name = "Concrete 40"};
            concrete.HelperMaterial = Model.HeadMaterialRepository.LibMaterials.Where(x => (x.MaterialType == MaterialTypes.Concrete & x.Name.Contains("40"))).First();
            IHeadMaterial reinforcement = new HeadMaterial() { Name = "Reinforcement 400"};
            reinforcement.HelperMaterial = Model.HeadMaterialRepository.LibMaterials.Where(x => (x.MaterialType == MaterialTypes.Reinforcement & x.Name.Contains("400"))).First();
            headMaterials.Add(concrete);
            headMaterials.Add(reinforcement);
            OnPropertyChanged(nameof(headMaterials));

            yield return new Rectangle(width, height, 0, 0, this) { Material = rectMaterial, MaterialName = rectMaterial.MaterialClass, HeadMaterial = concrete };
            yield return new Point(area1, -width / 2 + gap, -height / 2 + gap, this) { Material = pointMaterial, MaterialName = pointMaterial.MaterialClass, HeadMaterial = reinforcement };
            yield return new Point(area1, width / 2 - gap, -height / 2 + gap, this) { Material = pointMaterial, MaterialName = pointMaterial.MaterialClass, HeadMaterial = reinforcement };
            yield return new Point(area2, -width / 2 + gap, height / 2 - gap, this) { Material = pointMaterial, MaterialName = pointMaterial.MaterialClass, HeadMaterial = reinforcement };
            yield return new Point(area2, width / 2 - gap, height / 2 - gap, this) { Material = pointMaterial, MaterialName = pointMaterial.MaterialClass, HeadMaterial = reinforcement };
        }
        private void EditCalculationProperty()
        {
            CalculationPropertyViewModel viewModel = new CalculationPropertyViewModel(calculationProperty);
            var view = new CalculationPropertyView(viewModel);
            view.ShowDialog();
        }
        private void AddTestLoads()
        {
            calculationProperty.ForceCombinations.Clear();
            calculationProperty.ForceCombinations.Add(new ForceCombination());
            calculationProperty.ForceCombinations[0].ForceMatrix.Mx = 40e3d;
            calculationProperty.ForceCombinations[0].ForceMatrix.My = 20e3d;
            calculationProperty.ForceCombinations[0].ForceMatrix.Nz = 0d;
            calculationProperty.ForceCombinations.Add(new ForceCombination());
            calculationProperty.ForceCombinations[1].ForceMatrix.Mx = 200e3d;
            calculationProperty.ForceCombinations[1].ForceMatrix.My = 0d;
            calculationProperty.ForceCombinations[1].ForceMatrix.Nz = 0d;
            calculationProperty.ForceCombinations.Add(new ForceCombination());
            calculationProperty.ForceCombinations[2].ForceMatrix.Mx = 50e3d;
            calculationProperty.ForceCombinations[2].ForceMatrix.My = 50e3d;
            calculationProperty.ForceCombinations[2].ForceMatrix.Nz = 0d;
        }
    }
}