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

namespace StructureHelper.Windows.ViewModels.Calculations.Calculators.GeometryCalculatorVMs
{
    internal class GeometryCalculatorResultViewModel : ViewModelBase
    {
        IGeometryResult result;
        private ICommand exportToCSVCommand;

        public List<ITextParameter> TextParameters
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
            var inputData = new ExportToFileInputData();
            inputData.FileName = "New File";
            inputData.Filter = "csv |*.csv";
            inputData.Title = "Save in csv File";
            var logic = new ExportGeometryResultToCSVLogic(result);
            var exportService = new ExportToFileService(inputData, logic);
            exportService.Export();
        }
    }
}
