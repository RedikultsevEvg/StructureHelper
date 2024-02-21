using LoaderCalculator.Logics.Geometry;
using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Models.Materials;
using StructureHelper.Services.Settings;
using StructureHelper.Windows.PrimitiveTemplates.RCs.Beams;
using StructureHelper.Windows.PrimitiveTemplates.RCs.RectangleBeam;
using StructureHelper.Windows.ViewModels;
using StructureHelper.Windows.ViewModels.Forces;
using StructureHelper.Windows.ViewModels.Materials;
using StructureHelper.Windows.ViewModels.NdmCrossSections;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
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
    public class CrossSectionViewModel : ViewModelBase
    {
        private ICrossSection section;
        private ICrossSectionRepository repository => section.SectionRepository;

        public CrossSectionVisualPropertyVM VisualProperty { get; private set; }


        public PrimitiveBase SelectedPrimitive { get; set; }

        public AnalysisViewModelLogic CalculatorsLogic { get; private set; }
        public ActionsViewModel CombinationsLogic { get; }
        public MaterialsViewModel MaterialsLogic { get; }
        public PrimitiveViewModelLogic PrimitiveLogic { get; }
        public HelpLogic HelpLogic => new HelpLogic();

        private CrossSectionModel Model { get; }


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
                    MaterialsLogic.Refresh();
                });
            }
        }
        public ICommand AddBeamCase { get; }
        public ICommand AddColumnCase { get; }
        public ICommand AddSlabCase { get; }
        public ICommand LeftButtonDown { get; }
        public ICommand LeftButtonUp { get; }
        public ICommand MovePrimitiveToGravityCenterCommand { get; }

        public ICommand ClearSelection { get; }
        public ICommand OpenMaterialCatalog { get; }
        public ICommand OpenMaterialCatalogWithSelection { get; }
        public ICommand OpenUnitsSystemSettings { get; }
        public ICommand SetColor { get; }
        public ICommand SetInFrontOfAll { get; }
        public ICommand SetInBackOfAll { get; }

        public ICommand SetPopupCanBeClosedTrue { get; }
        public ICommand SetPopupCanBeClosedFalse { get; }
        public RelayCommand ShowVisualProperty
        {
            get
            {
                return showVisualProperty ??
                    (showVisualProperty = new RelayCommand(o=>
                    {
                        var wnd = new AxisCanvasView(VisualProperty.AxisCanvasVM);
                        wnd.ShowDialog();
                        if (wnd.DialogResult == false) { return; }
                        VisualProperty.Refresh();
                        PrimitiveLogic.Width = VisualProperty.Width;
                        PrimitiveLogic.Height = VisualProperty.Height;
                        PrimitiveLogic.Refresh();
                    }));
            }
        }

        public RelayCommand SelectPrimitiveCommand
        {
            get
            {
                return selectPrimitive ??= new RelayCommand(obj=>
                    {
                        if (obj is PrimitiveBase)
                        {
                            SelectedPrimitive = obj as PrimitiveBase;
                        }
                    });
            }
        }

        private RelayCommand showVisualProperty;
        private RelayCommand selectPrimitive;

        public CrossSectionViewModel(CrossSectionModel model)
        {
            VisualProperty = new CrossSectionVisualPropertyVM()
            {
                ScaleValue = 500d,
                ParentViewModel = this
            };
            Model = model;
            section = model.Section;
            CombinationsLogic = new ActionsViewModel(repository);
            MaterialsLogic = new MaterialsViewModel(repository);
            MaterialsLogic.AfterItemsEdit += AfterMaterialEdit;
            CalculatorsLogic = new AnalysisViewModelLogic(repository);
            PrimitiveLogic = new PrimitiveViewModelLogic(section)
            {
                Width = VisualProperty.Width,
                Height = VisualProperty.Height
            };

            LeftButtonUp = new RelayCommand(o =>
            {
                if (o is RectangleViewPrimitive rect) rect.BorderCaptured = false;
            });
            LeftButtonDown = new RelayCommand(o =>
            {
                if (o is RectangleViewPrimitive rect) rect.BorderCaptured = true;
            });

            AddBeamCase = new RelayCommand(o =>
            {
                PrimitiveLogic.AddItems(GetBeamCasePrimitives());
                MaterialsLogic.Refresh();
            });

            AddColumnCase = new RelayCommand(o =>
            {
                PrimitiveLogic.AddItems(GetColumnCasePrimitives());
                MaterialsLogic.Refresh();
            });

            AddSlabCase = new RelayCommand(o =>
            {
                PrimitiveLogic.AddItems(GetSlabCasePrimitives());
                MaterialsLogic.Refresh();
            });

            MovePrimitiveToGravityCenterCommand = new RelayCommand(o =>
            {
                if (CheckMaterials() == false) { return;}
                var ndms = NdmPrimitivesService.GetNdms(repository.Primitives, LimitStates.SLS, CalcTerms.ShortTerm);
                var center = GeometryOperations.GetGravityCenter(ndms);
                foreach (var item in PrimitiveLogic.Items)
                {
                    item.CenterX -= center.Cx;
                    item.CenterY -= center.Cy;
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
                if (o is not PrimitiveBase primitive) return;
                primitive.PopupCanBeClosed = false;
            });
        }

        private void AfterMaterialEdit(SelectItemVM<IHeadMaterial> sender, CRUDVMEventArgs e)
        {
            PrimitiveLogic.Refresh();
        }

        private bool CheckMaterials()
        {
            foreach (var item in PrimitiveLogic.Items)
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
            var template = new RectangleBeamTemplate(0.5d, 0.5d)
            {
                CoverGap = 0.05,
                WidthCount = 3,
                HeightCount = 3,
                TopDiameter = 0.025d,
                BottomDiameter = 0.025d
            };
            return GetCasePrimitives(template);
        }
        private IEnumerable<PrimitiveBase> GetSlabCasePrimitives()
        {
            var template = new RectangleBeamTemplate(1d, 0.2d)
            {
                CoverGap = 0.04,
                WidthCount = 5,
                HeightCount = 2,
                TopDiameter = 0.012d,
                BottomDiameter = 0.012d
            };
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
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(template));
            }
            wnd.ShowDialog();
            if (wnd.DialogResult == false)
            {
                return new List<PrimitiveBase>();
            }
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
                PrimitiveLogic.Refresh();
                foreach (var item in newRepository.HeadMaterials)
                {
                    GlobalRepository.Materials.Create(item);
                }
                foreach (var item in newRepository.ForceActions)
                {
                    GlobalRepository.Actions.Create(item);
                }
                return primitives;
            
        }
    }
}