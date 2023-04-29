using LoaderCalculator.Logics.Geometry;
using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Models.Materials;
using StructureHelper.Windows.ColorPickerWindow;
using StructureHelper.Windows.MainWindow.Materials;
using StructureHelper.Windows.PrimitiveTemplates.RCs.Beams;
using StructureHelper.Windows.PrimitiveTemplates.RCs.RectangleBeam;
using StructureHelper.Windows.ViewModels;
using StructureHelper.Windows.ViewModels.Forces;
using StructureHelper.Windows.ViewModels.Materials;
using StructureHelper.Windows.ViewModels.NdmCrossSections;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Strings;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.Models.Templates.CrossSections.RCs;
using StructureHelperLogics.Models.Templates.RCs;
using StructureHelperLogics.Services.NdmPrimitives;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace StructureHelper.Windows.MainWindow
{
    public class MainViewModel : ViewModelBase
    {
        ICrossSection section;
        ICrossSectionRepository repository => section.SectionRepository;

        private CrossSectionViewVisualProperty visualProperty;

        private readonly double scaleRate = 1.1d;

        public PrimitiveBase SelectedPrimitive { get; set; }
        //public IForceCombinationList SelectedForceCombinationList { get; set; }

        private readonly AnalysisVewModelLogic calculatorsLogic;
        public AnalysisVewModelLogic CalculatorsLogic { get => calculatorsLogic;}
        public ActionsViewModel CombinationsLogic { get => combinationsLogic; }
        public MaterialsViewModel MaterialsLogic { get => materialsLogic; }
        public PrimitiveViewModelLogic PrimitiveLogic => primitiveLogic;
        public HelpLogic HelpLogic => new HelpLogic();

        private MainModel Model { get; }

        private double panelX, panelY, scrollPanelX, scrollPanelY;

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
                OnPropertyChanged(nameof(AxisLineThickness));
                OnPropertyChanged(nameof(GridLineThickness));
            }
        }

        public double AxisLineThickness
        { 
            get => visualProperty.AxisLineThickness / scaleValue;
        }

        public double GridLineThickness
        {
            get => visualProperty.GridLineThickness / scaleValue;
        }

        private double xX2, xY1, yX1, yY2;
        public double CanvasWidth
        {
            get => visualProperty.WorkPlainWidth;
        }

        public double CanvasHeight
        {
            get => visualProperty.WorkPlainHeight;
        }

        public string CanvasViewportSize
        {
            get
            {
                string s = visualProperty.GridSize.ToString();
                s = s.Replace(',', '.');
                return $"0,0,{s},{s}";
            }

        }

        public double GridSize { get => visualProperty.GridSize; }
   
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
        public ICommand AddRCCircleCase
        { 
            get
            {
                return new RelayCommand(o =>
                {
                    PrimitiveLogic.AddItems(GetRCCirclePrimitives());
                    materialsLogic.Refresh();
                });
            }
        }
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
        public RelayCommand ShowVisualProperty
        {
            get
            {
                return showVisualProperty ??
                    (showVisualProperty = new RelayCommand(o=>
                    {
                        var wnd = new VisualPropertyView(visualProperty);
                        wnd.ShowDialog();
                        OnPropertyChanged(nameof(AxisLineThickness));
                        OnPropertyChanged(nameof(CanvasViewportSize));
                        OnPropertyChanged(nameof(GridSize));
                    }));
            }
        }

        public RelayCommand SelectPrimitiveCommand
        {
            get
            {
                return selectPrimitive ??
                    (selectPrimitive = new RelayCommand(obj=>
                    {
                        if (obj is PrimitiveBase)
                        {
                            SelectedPrimitive = obj as PrimitiveBase;
                        }
                    }));
            }
        }

        private double delta = 0.0005;
        private ActionsViewModel combinationsLogic;
        private PrimitiveViewModelLogic primitiveLogic;
        private RelayCommand showVisualProperty;
        private RelayCommand selectPrimitive;
        private MaterialsViewModel materialsLogic;

        public MainViewModel(MainModel model)
        {
            visualProperty = new CrossSectionViewVisualProperty();
            Model = model;
            section = model.Section;
            combinationsLogic = new ActionsViewModel(repository);
            materialsLogic = new MaterialsViewModel(repository);
            materialsLogic.AfterItemsEdit += afterMaterialEdit;
            calculatorsLogic = new AnalysisVewModelLogic(repository);
            primitiveLogic = new PrimitiveViewModelLogic(section) { CanvasWidth = CanvasWidth, CanvasHeight = CanvasHeight };
            XX2 = CanvasWidth;
            XY1 = CanvasHeight / 2d;
            YX1 = CanvasWidth / 2d;
            YY2 = CanvasHeight;
            scaleValue = 300d;

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
                materialsLogic.Refresh();
            });

            AddColumnCase = new RelayCommand(o =>
            {
                PrimitiveLogic.AddItems(GetColumnCasePrimitives());
                materialsLogic.Refresh();
            });

            AddSlabCase = new RelayCommand(o =>
            {
                PrimitiveLogic.AddItems(GetSlabCasePrimitives());
                materialsLogic.Refresh();
            });

            MovePrimitiveToGravityCenterCommand = new RelayCommand(o =>
            {
                if (CheckMaterials() == false) { return;}
                var ndms = NdmPrimitivesService.GetNdms(repository.Primitives, LimitStates.SLS, CalcTerms.ShortTerm);
                var center = GeometryOperations.GetGravityCenter(ndms);
                foreach (var item in PrimitiveLogic.Items)
                {
                    item.CenterX -= center.CenterX;
                    item.CenterY -= center.CenterY;
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

        private void afterMaterialEdit(CRUDViewModelBase<IHeadMaterial> sender, CRUDVMEventArgs e)
        {
            foreach (var primitive in primitiveLogic.Items)
            {
                primitive.RefreshColor();
            }
        }

        private bool CheckMaterials()
        {
            foreach (var item in primitiveLogic.Items)
            {
                if (item.HeadMaterial == null)
                {
                    System.Windows.Forms.MessageBox.Show($"Primitive {item.Name} does not has material", "Check data for analisys", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            return true;
        }
        private IEnumerable<PrimitiveBase> GetRCCirclePrimitives()
        {
            var template = new CircleTemplate();
            return GetCasePrimitives(template);
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

        private IEnumerable<PrimitiveBase> GetCasePrimitives(IRCSectionTemplate template)
        {
            Window wnd;
            IRCGeometryLogic geometryLogic;
            if (template is IRectangleBeamTemplate)
            {
                var rectTemplate = template as IRectangleBeamTemplate;
                geometryLogic = new RectGeometryLogic(rectTemplate);
                wnd = new RectangleBeamView(rectTemplate);
            }
            else if (template is ICircleTemplate)
            {
                var circleTemplate = template as ICircleTemplate;
                geometryLogic = new CircleGeometryLogic(circleTemplate);
                wnd = new CircleView(circleTemplate);
            }
            else { throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown + $"Was: {nameof(template)}"); }
            wnd.ShowDialog();
            if (wnd.DialogResult == true)
            {
                
                var newSection = new SectionTemplate(geometryLogic).GetCrossSection();
                var newRepository = newSection.SectionRepository;
                repository.HeadMaterials.AddRange(newRepository.HeadMaterials);
                repository.Primitives.AddRange(newRepository.Primitives);
                repository.ForceActions.AddRange(newRepository.ForceActions);
                repository.CalculatorsList.AddRange(newRepository.CalculatorsList);
                OnPropertyChanged(nameof(HeadMaterials));
                CombinationsLogic.AddItems(newRepository.ForceActions);
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