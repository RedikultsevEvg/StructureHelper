using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.Enums;
using StructureHelper.Services.Exports;
using StructureHelper.Windows.Graphs;
using StructureHelper.Windows.ViewModels;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.FeaMaterials;
using StructureHelperCommon.Models.FeaMaterials.ExportLogics;
using StructureHelperCommon.Services;
using StructureHelperCommon.Services.Exports;
using StructureHelperCommon.Services.Exports.Factories;
using System;
using System.Collections.Generic;

namespace StructureHelper.Windows.FeaMaterials
{
    public class FeaMaterialsListViewModel : SelectItemVM<IFeaMaterial>
    {
        private RelayCommand showDiagram;
        private RelayCommand exportMaterialToPyCommand;
        private RelayCommand prismTestCommand;
        private RelayCommand cubeTestCommand;
        private RelayCommand cylinderTestCommand;

        public RelayCommand ShowDiagram => showDiagram ??= new RelayCommand(o => ShowDigramMethod(), o => SelectedItem != null);
        public RelayCommand ExportMaterialToPyCommand => exportMaterialToPyCommand ??= new RelayCommand(o => ExportMaterialToPy(), o => SelectedItem != null);
        public RelayCommand PrismTestCommand => prismTestCommand ??= new RelayCommand(o => PrismTest(), o => SelectedItem != null);
        public RelayCommand CubeTestCommand => cubeTestCommand ??= new RelayCommand(o => CubeTest(), o => SelectedItem != null);
        public RelayCommand CylinderTestCommand => cylinderTestCommand ??= new RelayCommand(o => CylinderTest(), o => SelectedItem != null);

        private void CylinderTest()
        {
            SafetyProcessor.RunSafeProcess(ProcessCylinder, "Error of creating of script of cube");
        }

        private void ProcessCylinder()
        {
            if (SelectedItem is null) { return; }
            ProcessModel(ModelType.Cylinder);
        }

        private void CubeTest()
        {
            SafetyProcessor.RunSafeProcess(ProcessCube, "Error of creating of script of cube");
        }

        private void ProcessCube()
        {
            if (SelectedItem is null) { return; }
            ProcessModel(ModelType.Cube);
        }

        private void ProcessModel(ModelType cube)
        {
            string script = AbaqusModelFactory.GetScript(SelectedItem, cube);

            FileIOInputData inputData = FileInputDataFactory.GetFileIOInputData(FileInputDataType.Py);
            var logic = new ExportTextToFileLogic()
            {
                FileName = SelectedItem.Name,
                Text = script
            };
            var exportService = new ExportToFileService(inputData, logic);
            exportService.Export();
        }

        private void PrismTest()
        {
            SafetyProcessor.RunSafeProcess(ProcessPrism, "Error of creating of script of cube");
        }

        private void ProcessPrism()
        {
            if (SelectedItem is null) { return; }
            ProcessModel(ModelType.Prism);
        }

        private void ExportMaterialToPy()
        {
            if (SelectedItem is null) { return; }
            try
            {
                var builder = new FeaMaterialPyBuilder();
                var script = builder.Build(SelectedItem);
                FileIOInputData inputData = FileInputDataFactory.GetFileIOInputData(FileInputDataType.Py);
                var logic = new ExportTextToFileLogic()
                {
                    FileName = SelectedItem.Name,
                    Text = script
                };
                var exportService = new ExportToFileService(inputData, logic);
                exportService.Export();
            }
            catch (Exception ex)
            {
                SafetyProcessor.ShowMessage("Some errors occured during export, see detailed information", ex.Message);
            }
        }

        private void ShowDigramMethod()
        {
            if (SelectedItem is null) return;
            if (SelectedItem is IConcreteFeaMaterial concrete)
            {
                var logic = new ConcreteToCDPConvertStrategy();
                var cdp = logic.Convert(concrete);
                var convertLogic = new CdpToChartSeriesConvertStrategy();
                var series = convertLogic.Convert(cdp);
                var vm = new GraphViewModel(series);
                var wnd = new GraphView(vm);
                wnd.ShowDialog();
            }
        }

        public FeaMaterialsListViewModel(List<IFeaMaterial> collection) : base(collection)
        {
            
        }

        public override void AddMethod(object parameter)
        {
            CheckObject.ThrowIfNull(parameter);
            SafetyProcessor.RunSafeProcess<object>(parameter, GetMaterial, $"Error of adding of FEA material");
        }

        public override void EditMethod(object parameter)
        {
            if (SelectedItem is null) { return; }
            SafetyProcessor.RunSafeProcess(EditSelectedItem, $"Error of editing of material Name = {SelectedItem.Name}");
            base.EditMethod(parameter);
        }

        private void EditSelectedItem()
        {
            if (SelectedItem is IElasticFeaMaterial elastic)
            {
                var cloneLogic = new ElasticFeaMaterialCloneStrategy();
                var clone = cloneLogic.GetClone(elastic);
                var window = new ElasticFeaMaterialView(elastic);
                window.ShowDialog();
                if (window.DialogResult != true)
                {
                    var updateLogic = new ElasticFeaMaterialUpdateStrategy();
                    updateLogic.Update(elastic, clone);
                }
            }
            else if (SelectedItem is IConcreteFeaMaterial concrete)
            {
                var cloneLogic = new ConcreteFeaMaterialCloneStrategy();
                var clone = cloneLogic.GetClone(concrete);
                var window = new ConcreteFeaMaterialView(concrete);
                window.ShowDialog();
                if (window.DialogResult != true)
                {
                    var updateLogic = new ConcreteFeaMaterialUpdateStrategy() { UpdateChildren = true};
                    updateLogic.Update(concrete, clone);
                }
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(SelectedItem));
            }
        }

        private void GetMaterial(object parameter)
        {
            if (parameter is MaterialType type)
            {
                NewItem = FeaMaterialFactory.GetFeaMaterial(type);
                base.AddMethod(parameter);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(parameter));
            }
        }
    }
}
