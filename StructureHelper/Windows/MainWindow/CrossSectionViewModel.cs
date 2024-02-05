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
      private readonly double scaleRate = 1.1d;

      public CrossSectionVisualPropertyVM VisualProperty { get; private set; }


      public PrimitiveBase SelectedPrimitive { get; set; }

      public AnalysisVewModelLogic CalculatorsLogic { get; private set; }
      public ActionsViewModel CombinationsLogic { get; }
      public MaterialsViewModel MaterialsLogic { get; }
      public PrimitiveViewModelLogic PrimitiveLogic { get; }
      public HelpLogic HelpLogic => new HelpLogic();

      private CrossSectionModel Model { get; }

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
         get => VisualProperty.AxisLineThickness / scaleValue;
      }

      public double GridLineThickness
      {
         get => VisualProperty.GridLineThickness / scaleValue;
      }

      public string CanvasViewportSize
      {
         get
         {
            string s = VisualProperty.GridSize.ToString();
            s = s.Replace(',', '.');
            return $"0,0,{s},{s}";
         }

      }

      public double GridSize => VisualProperty.GridSize;

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

      /// <summary>
      /// Right edge of work plane, coordinate X
      /// </summary>
      public double RightLimitX => VisualProperty.WorkPlainWidth;
      /// <summary>
      /// Bottom edge of work plane Y
      /// </summary>
      public double BottomLimitY => VisualProperty.WorkPlainHeight;
      /// <summary>
      /// Middle of coordinate X
      /// </summary>
      public double MiddleLimitX => VisualProperty.WorkPlainWidth / 2d;
      /// <summary>
      /// Middle of coordinate Y
      /// </summary>
      public double MiddleLimitY => VisualProperty.WorkPlainHeight / 2d;

      private int langID = 0;
      public int LangID { get => langID; set { langID = value; OnPropertyChanged(); } }
      public RelayCommand ChangeLanguage
      {
         get
         {
            return changeLanguage ?? (changeLanguage = new RelayCommand(obj => { ChangeLang(); }));
         }
      }

      public void ChangeLang()
      {
         if (LangID == 0)
         {
            LangID = 1;
            SetLanguageDictionary(LangID);
         }
         else
         {
            LangID = 0;
            SetLanguageDictionary(LangID);
         }
      }

      internal ResourceDictionary dict = new ResourceDictionary();
      // Загрузка файлов ресурсов для выбранного языка
      private void SetLanguageDictionary(int local)
      {
         switch (local)
         {
            case 0:
               dict.Source = new Uri("..\\Infrastructure\\UI\\Resources\\Strings.en-US.xaml",
                             UriKind.Relative);
               break;
            case 1:
               dict.Source = new Uri("..\\Infrastructure\\UI\\Resources\\Strings.ru-RU.xaml",
                                  UriKind.Relative);
               break;
         }
         MV.Resources.MergedDictionaries.Add(dict);
      }

      internal CrossSectionView MV;

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
                (showVisualProperty = new RelayCommand(o =>
                {
                   var wnd = new VisualPropertyView(VisualProperty);
                   SetLanguageDictionary(langID);
                   wnd.Resources.MergedDictionaries.Add(dict);
                   wnd.ShowDialog();                 
                   OnPropertyChanged(nameof(AxisLineThickness));
                   OnPropertyChanged(nameof(CanvasViewportSize));
                   OnPropertyChanged(nameof(GridSize));
                   OnPropertyChanged(nameof(RightLimitX));
                   OnPropertyChanged(nameof(BottomLimitY));
                   OnPropertyChanged(nameof(MiddleLimitX));
                   OnPropertyChanged(nameof(MiddleLimitY));
                   PrimitiveLogic.WorkPlaneWidth = VisualProperty.WorkPlainWidth;
                   PrimitiveLogic.WorkPlaneHeight = VisualProperty.WorkPlainHeight;
                   PrimitiveLogic.Refresh();
                }));
         }
      }

      public RelayCommand SelectPrimitiveCommand
      {
         get
         {
            return selectPrimitive ??
                (selectPrimitive = new RelayCommand(obj =>
                {
                   if (obj is PrimitiveBase)
                   {
                      SelectedPrimitive = obj as PrimitiveBase;
                   }
                }));
         }
      }

      private double delta = 0.0005;
      private RelayCommand showVisualProperty;
      private RelayCommand selectPrimitive;
      private RelayCommand changeLanguage;

      public CrossSectionViewModel(CrossSectionModel model)
      {
         VisualProperty = new CrossSectionVisualPropertyVM();
         Model = model;
         section = model.Section;
         CombinationsLogic = new ActionsViewModel(repository);
         MaterialsLogic = new MaterialsViewModel(repository);
         MaterialsLogic.AfterItemsEdit += afterMaterialEdit;
         CalculatorsLogic = new AnalysisVewModelLogic(repository);
         PrimitiveLogic = new PrimitiveViewModelLogic(section)
         {
            WorkPlaneWidth = VisualProperty.WorkPlainWidth,
            WorkPlaneHeight = VisualProperty.WorkPlainHeight
         };
         scaleValue = 500d;

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
            if (CheckMaterials() == false) { return; }
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
            if (!(o is PrimitiveBase primitive)) return;
            primitive.PopupCanBeClosed = false;
         });
      }

      private void afterMaterialEdit(SelectItemVM<IHeadMaterial> sender, CRUDVMEventArgs e)
      {
         foreach (var primitive in PrimitiveLogic.Items)
         {
            primitive.RefreshColor();
         }
      }

      private bool CheckMaterials()
      {
         foreach (var item in PrimitiveLogic.Items)
         {
            if (item.HeadMaterial == null)
            {
               if(langID == 0) { System.Windows.Forms.MessageBox.Show($"Primitive {item.Name} does not has material", "Check data for analisys", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
               else { System.Windows.Forms.MessageBox.Show($"Примитиву {item.Name} не назначен материал", "Проверьте расчетные данные", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
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
            SetLanguageDictionary(langID);
            wnd.Resources.MergedDictionaries.Add(dict);

         }
         else if (template is ICircleTemplate)
         {
            var circleTemplate = template as ICircleTemplate;
            geometryLogic = new CircleGeometryLogic(circleTemplate);
            wnd = new CircleView(circleTemplate);
            SetLanguageDictionary(langID);
            wnd.Resources.MergedDictionaries.Add(dict);
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
               item.RegisterDeltas(VisualProperty.WorkPlainWidth / 2, VisualProperty.WorkPlainHeight / 2);
            }
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
         return new List<PrimitiveBase>();
      }
   }
}