using StructureHelper.Infrastructure;
using StructureHelper.Services.Exports;
using StructureHelperCommon.Models.Parameters;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Analyses;
using StructureHelperLogics.NdmCalculations.Analyses.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using StructureHelperCommon.Services.Exports;
using StructureHelperCommon.Services.Exports.Factories;

namespace StructureHelper.Windows.ViewModels.Calculations.Calculators.GeometryCalculatorVMs
{
    internal class GeometryCalculatorResultViewModel : ViewModelBase
    {
        IGeometryResult result;
        private ICommand exportToCSVCommand;

        public List<IValueParameter<string>> TextParameters
        { 
            get => result.TextParameters;
        }
        public ICommand ExportToCSVCommand
        {
            get => exportToCSVCommand ??= new RelayCommand(o => ExportToCSV());
        }
        public GeometryCalculatorResultViewModel(IGeometryResult geometryResult)
        {
            this.result = geometryResult;
        }
        private void ExportToCSV()
        {
            var inputData = FileInputDataFactory.GetFileIOInputData(FileInputDataType.Csv);
            var logic = new ExportGeometryResultToCSVLogic(result);
            var exportService = new ExportToFileService(inputData, logic);
            exportService.Export();
        }
    }
}
