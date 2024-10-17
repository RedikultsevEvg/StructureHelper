using NLog.Common;
using StructureHelper.Infrastructure;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Functions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StructureHelper.Windows.TreeGraph
{
    public class TreeGraphViewModel : ViewModelBase
    {
        private RelayCommand _getYCommand;
        private RelayCommand _scaleCommand;
        private RelayCommand _limCommand;
        private RelayCommand _editCommand;
        private RelayCommand _deleteCommand;
        public ICommand GetYCommand
        {
            get => _getYCommand ??= new RelayCommand(o => GetY());
        }
        public ICommand ScaleCommand
        {
            get => _scaleCommand ??= new RelayCommand(o => Scale(o));
        }
        public ICommand LimitCommand
        {
            get => _limCommand ??= new RelayCommand(o => Limit(o));
        }
        public ICommand EditCommand
        {
            get => _editCommand ??= new RelayCommand(o => Edit());
        }
        public ICommand DeleteCommand
        {
            get => _deleteCommand ??= new RelayCommand(o => Delete());
        }
        private ObservableCollection<IOneVariableFunction> nodes;
        public ObservableCollection<IOneVariableFunction> Nodes { get; set; }
        public TreeGraphViewModel(IOneVariableFunction function)
        {
            Nodes = new ObservableCollection<IOneVariableFunction>();
            Nodes.Add(function);
        }
        private void GetY()
        {
            var vm = new GetValueViewModel(new TableFunction());
            var v = new GetValueView();
            v.DataContext = vm;
            v.ShowDialog();
        }
        private void Scale(object parameter)
        {
            var vm = new ScaleViewModel();
            var v = new ScaleView();
            v.DataContext = vm;
            v.ShowDialog();
        }
        private void Limit(object parameter)
        {
            var vm = new LimViewModel();
            var v = new LimView();
            v.DataContext = vm;
            v.ShowDialog();
        }
        private void Edit()
        {
        }
        private void Delete()
        {
        
        }
    }
}
