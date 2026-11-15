using LoaderCalculator.Logics.Geometry;
using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Models.Materials;
using StructureHelper.Services.Exports;
using StructureHelper.Windows.PrimitiveTemplates.RCs.Beams;
using StructureHelper.Windows.PrimitiveTemplates.RCs.RectangleBeam;
using StructureHelper.Windows.ViewModels;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelper.Windows.ViewModels.Forces;
using StructureHelper.Windows.ViewModels.Materials;
using StructureHelper.Windows.ViewModels.NdmCrossSections;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.History;
using StructureHelperCommon.Services.Exports;
using StructureHelperCommon.Services.Exports.Factories;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.Models.Materials;
using StructureHelperLogics.Models.Templates.CrossSections.RCs;
using StructureHelperLogics.Models.Templates.RCs;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.Services.NdmPrimitives;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;


//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.


namespace StructureHelper.Windows.MainWindow
{
    public class CrossSectionViewModel : ViewModelBase
    {
        private IUndoRedoService? undoRedoService;
        private readonly Guid? analysisId;
        private RelayCommand? undoCommand;
        private RelayCommand? redoCommand;

        public ICrossSection Section { get; set; }
        private ICrossSectionRepository repository => Section.SectionRepository;
        private ITriangulatePrimitiveLogic triangulateLogic;

        /// <summary>
        /// Optional project-level history service supplied by the application host.
        /// When it is null, Undo and Redo are disabled.
        /// </summary>
        public IUndoRedoService? UndoRedoService
        {
            get => undoRedoService;
            set
            {
                undoRedoService = value;
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ICommand UndoCommand => undoCommand ??= new RelayCommand(
            _ => ExecuteUndo(),
            _ => undoRedoService?.CanUndo == true);

        public ICommand RedoCommand => redoCommand ??= new RelayCommand(
            _ => ExecuteRedo(),
            _ => undoRedoService?.CanRedo == true);

        public CrossSectionVisualPropertyVM VisualProperty { get; private set; }

        public PrimitiveBase SelectedPrimitive { get; set; }

        public AnalysisViewModelLogic CalculatorsLogic { get; private set; }
        public ActionsViewModel CombinationsLogic { get; }
        public MaterialsViewModel MaterialsLogic { get; }
        public PrimitiveViewModelLogic PrimitiveLogic { get; }
        public HelpLogic HelpLogic => new HelpLogic();


        public ObservableCollection<IHeadMaterial> HeadMaterials
        {
            get
            {
                var collection = new ObservableCollection<IHeadMaterial>();
                foreach (var obj in Section.SectionRepository.HeadMaterials)
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

        public ICommand ExportToDxfCommand => exportToDxfCommand ??= new RelayCommand(ExportToDxf);
        public ICommand ImportFromDxfCommand => importFromDxfCommand ??= new RelayCommand(ImportFromDxf);

        private void ImportFromDxf(object obj)
        {
            SafetyProcessor.RunSafeProcess(ImportFromDxfFile, "Error of opening file");
        }

        private void ImportFromDxfFile()
        {
            FileIOInputData inputData = FileInputDataFactory.GetFileIOInputData(FileInputDataType.Dxf);
            var logic = new GetPrimitivesByFile();
            var importService = new ImportFromFileService(inputData, logic);
            importService.Import();
            var ndmPrimitives = logic.Primitives;
            var primitives = PrimitiveOperations.ConvertNdmPrimitivesToPrimitiveBase(ndmPrimitives);
            foreach (var ndmPrimitive in ndmPrimitives)
            {
                repository.AddPrimitive(ndmPrimitive);
            }
            PrimitiveLogic.AddItems(primitives);
            PrimitiveLogic.Refresh();
        }

        private void ExportToDxf(object commandParameter)
        {
            FileIOInputData inputData = FileInputDataFactory.GetFileIOInputData(FileInputDataType.Dxf);
            var convertLogic = new NdmPrimitivesToDxfEntitiesConvertStrategy();
            var entities = convertLogic.Convert(repository.Primitives);
            var logic = new EntitiesToDxfExportLogic() { Entities = entities };
            var exportService = new ExportToFileService(inputData, logic);
            exportService.Export();
        }

        private RelayCommand showVisualProperty;
        private RelayCommand selectPrimitive;
        private RelayCommand fileDroppedCommand;
        private string fileName;
        private RelayCommand exportToDxfCommand;
        private RelayCommand importFromDxfCommand;

        public ICommand FileDroppedCommand => fileDroppedCommand ??= new RelayCommand(OnFileDropped);

        private void OnFileDropped(object obj)
        {
            if (obj is string[] files && files.Length > 0)
            {
                foreach (var filenameString in files)
                {
                    fileName = filenameString;
                    ProcessDxfFile();
                }
            }
            else
            {
                System.Windows.MessageBox.Show($"Error of file");
            }
        }

        private void ProcessDxfFile()
        {
            string extension = Path.GetExtension(fileName).ToLowerInvariant();
            if (extension == ".dxf")
            {
                SafetyProcessor.RunSafeProcess(GetPrimitivesFromFile, "Error of obtaining of primitives, see detail information");
            }
            else
            {
                System.Windows.MessageBox.Show($"Unsupported file type: {extension}");
            }
        }

        private void GetPrimitivesFromFile()
        {
            var importFromFileLogic = new GetPrimitivesByFile() { FileName = fileName };
            importFromFileLogic.Import();
            var ndmPrimitives = importFromFileLogic.Primitives;
            var primitives = PrimitiveOperations.ConvertNdmPrimitivesToPrimitiveBase(ndmPrimitives);
            foreach (var ndmPrimitive in ndmPrimitives)
            {
                repository.AddPrimitive(ndmPrimitive);
            }
            PrimitiveLogic.AddItems(primitives);
            PrimitiveLogic.Refresh();
        }

        public CrossSectionViewModel(ICrossSection section)
            : this(section, null, null)
        {
        }

        public CrossSectionViewModel(ICrossSection section, IUndoRedoService? undoRedoService, Guid? analysisId = null)
        {
            Section = section;
            this.undoRedoService = undoRedoService;
            this.analysisId = analysisId;
            VisualProperty = new CrossSectionVisualPropertyVM(Section.WorkPlaneProperty)
            {
                ScaleValue = 500d,
                ParentViewModel = this
            };
            CombinationsLogic = new ActionsViewModel(repository);
            MaterialsLogic = new MaterialsViewModel(repository);
            MaterialsLogic.AfterItemsEdit += AfterMaterialEdit;
            CalculatorsLogic = new AnalysisViewModelLogic(repository);
            PrimitiveLogic = new PrimitiveViewModelLogic(section)
            {
                Width = VisualProperty.Width,
                Height = VisualProperty.Height
            };
            PrimitiveLogic.Refresh();

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
                triangulateLogic = new TriangulatePrimitiveLogic()
                {
                    Primitives = repository.Primitives,
                    LimitState = LimitStates.SLS,
                    CalcTerm = CalcTerms.ShortTerm
                };
                var ndms = triangulateLogic.GetNdms();
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

        private void ExecuteUndo()
        {
            if (undoRedoService is null || !undoRedoService.CanUndo)
            {
                return;
            }

            undoRedoService.Undo();
            RefreshAfterHistoryRestore();
        }

        private void ExecuteRedo()
        {
            if (undoRedoService is null || !undoRedoService.CanRedo)
            {
                return;
            }

            undoRedoService.Redo();
            RefreshAfterHistoryRestore();
        }

        private void RefreshAfterHistoryRestore()
        {
            // The host is responsible for restoring the project document.
            // These refreshes keep the current editor consistent with that document.
            PrimitiveLogic.Refresh();
            MaterialsLogic.Refresh();
            OnPropertyChanged(nameof(HeadMaterials));
            CommandManager.InvalidateRequerySuggested();
        }

        private IUndoTransaction BeginExampleTransaction(string name)
        {
            if (undoRedoService is null)
            {
                throw new InvalidOperationException(
                    "An undo/redo service must be supplied by the application host.");
            }

            return analysisId is Guid id
                ? undoRedoService.BeginTransaction(
                    name,
                    HistoryScope.Analysis,
                    new[] { id })
                : undoRedoService.BeginTransaction(name, HistoryScope.Project);
        }

        /// <summary>
        /// Example of an operation wrapped in an explicit undo transaction.
        /// When the host supplies an analysis ID, the transaction is analysis-scoped.
        /// </summary>
        public void AddExampleMaterial()
        {
            if (undoRedoService is null)
            {
                throw new InvalidOperationException(
                    "An undo/redo service must be supplied by the application host.");
            }

            var transaction = BeginExampleTransaction("Add example material");

            try
            {
                var material = new HeadMaterial(Guid.NewGuid())
                {
                    Name = "Example material",
                    HelperMaterial = new ElasticMaterial(Guid.NewGuid())
                };
                repository.AddHeadMaterial(material);
                transaction.Commit();
                RefreshAfterHistoryRestore();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Example of a command that should be wrapped in an explicit transaction.
        /// </summary>
        public void DeleteExampleMaterial(IHeadMaterial material)
        {
            if (undoRedoService is null)
            {
                throw new InvalidOperationException(
                    "An undo/redo service must be supplied by the application host.");
            }

            var transaction = BeginExampleTransaction("Delete example material");

            try
            {
                repository.RemoveHeadMaterial(material);
                transaction.Commit();
                RefreshAfterHistoryRestore();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
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
                foreach (var material in newRepository.HeadMaterials)
                {
                    repository.AddHeadMaterial(material);
                }
                foreach (var primitive in newRepository.Primitives)
                {
                    repository.AddPrimitive(primitive);
                }
                foreach (var action in newRepository.ForceActions)
                {
                    repository.AddForceAction(action);
                }
                foreach (var calculator in newRepository.Calculators)
                {
                    repository.AddCalculator(calculator);
                }
                OnPropertyChanged(nameof(HeadMaterials));
                CombinationsLogic.AddItems(newRepository.ForceActions);
                CalculatorsLogic.AddItems(newRepository.Calculators);
                var primitives = PrimitiveOperations.ConvertNdmPrimitivesToPrimitiveBase(newRepository.Primitives);
                PrimitiveLogic.Refresh();
            return primitives;
        }
    }
}