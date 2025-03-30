using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.Enums;
using StructureHelper.Windows.CalculationWindows.ProgressViews;
using StructureHelper.Windows.ViewModels;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.Models.BeamShears;
using System;
using System.Windows;
using System.Windows.Input;

namespace StructureHelper.Windows.BeamShears
{
    public class BeamShearCalculatorsViewModel : SelectItemVM<ICalculator>
    {
        private object parameter;
        private readonly IBeamShearRepository shearRepository;
        private RelayCommand runCommand;

        public ICommand Run
        {
            get
            {
                return runCommand ??= new RelayCommand(param =>
                    {
                        RunMethod(param);
                    }, o => SelectedItem != null
                    );
            }
        }

        public override void AddMethod(object parameter)
        {
            this.parameter = parameter;
            SafetyProcessor.RunSafeProcess(AddCalculator, "Error of creating calculator");
        }


        public override void EditMethod(object parameter)
        {
            SafetyProcessor.RunSafeProcess(EditCalculator, $"Error of calculator {SelectedItem.Name}");
            base.EditMethod(parameter);
        }

        public BeamShearCalculatorsViewModel(IBeamShearRepository shearRepository) : base(shearRepository.Calculators)
        {
            this.shearRepository = shearRepository;
        }
        private void AddCalculator()
        {
            if (parameter is CalculatorTypes.BeamShearCalculator)
            {
                NewItem  = new BeamShearCalculator(Guid.NewGuid())
                {
                    Name = "New shear calculator",
                    TraceLogger = new ShiftTraceLogger()
                };
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(parameter));
            }
            base.AddMethod(parameter);
        }
        private void EditCalculator()
        {
            Window window; 
            if (SelectedItem is IBeamShearCalculator beamShearCalculator)
            {
                var viewModel = new BeamShearCalculatorViewModel(shearRepository, beamShearCalculator);
                window = new BeamShearCalculatorView(viewModel);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(SelectedItem));
            }
            window.ShowDialog();
        }
        private void RunMethod(object param)
        {
            SafetyProcessor.RunSafeProcess(RunCalculator, $"Error of calculator {SelectedItem.Name}");
        }
        private void RunCalculator()
        {
            if (SelectedItem.TraceLogger is not null)
            {
                SelectedItem.TraceLogger.TraceLoggerEntries.Clear();
            }
            else
            {
                SelectedItem.TraceLogger = new ShiftTraceLogger();
            }
            if (SelectedItem is IBeamShearCalculator beamShearCalculator)
            {
                beamShearCalculator.Run();
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(SelectedItem));
            }
            TraceDocumentService.ShowDocument(SelectedItem.TraceLogger.TraceLoggerEntries);
        }
    }
}
