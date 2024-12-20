using StructureHelper.Infrastructure;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StructureHelper.Windows.TreeGraph
{
    public class GetValueViewModel : ViewModelBase
    {
        public IOneVariableFunction function;
        private double argument;
        private double value;
        private string trace = "Press \"Get value\" to calculation...\n\n\n";
        public IOneVariableFunction Function
        {
            get => function; 
            set => function = value;
        }
        public double Argument 
        { 
            get => argument; 
            set => argument = value; 
        }
        public double Value
        {
            get => value;
            set
            {
                this.value = value;
                OnPropertyChanged(nameof(Value));
            }
        }
        public string Trace
        {
            get => trace;
            set
            {
                trace = value;
                OnPropertyChanged(nameof(Trace));
            }
        }
        private RelayCommand _getValueCommand;
        public ICommand GetValueCommand
        {
            get => _getValueCommand ??= new RelayCommand(o => GetValue());
        }
        public GetValueViewModel(IOneVariableFunction function)
        {
            Function = function;
        }
        private void GetValue()
        {
            Value = Function.GetByX(Argument);
            Trace += Function.GetTrace();
            Trace += "\n\n";
        }
    }
}
