using LiveCharts.Wpf;
using LiveCharts;
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
using StructureHelperCommon.Models.Functions.Decorator;
using System.Windows.Media;
using StructureHelper.Windows.Graphs;

namespace StructureHelper.Windows.TreeGraph
{
    public class TreeGraphViewModel : ViewModelBase
    {
        private LineSeries lineSeries;
        private SeriesCollection seriesCollection;
        private List<string> labels;
        readonly ObservableCollection<TreeViewItemViewModel> _firstGeneration;
        readonly TreeViewItemViewModel _rootFunction;
        readonly ICommand _searchCommand;
        private RelayCommand _getYCommand;
        private RelayCommand _scaleCommand;
        private RelayCommand _limCommand;
        private RelayCommand _deleteCommand;
        private TreeGraphView _treeGraphView_win;
        private IOneVariableFunction selectedFunction;
        public IOneVariableFunction SelectedFuntion
        {
            get
            {
                return selectedFunction;
            }
            set
            {
                selectedFunction = value;
                OnPropertyChanged(nameof(SelectedFuntion));
            }
        }
        public SeriesCollection SeriesCollection
        {
            get => seriesCollection;
            set
            {
                seriesCollection = value;
                OnPropertyChanged(nameof(SeriesCollection));
            }
        }
        public List<string> Labels
        {
            get => labels;
            set
            {
                labels = value;
                OnPropertyChanged(nameof(labels));
            }
        }
        public LineSeries LineSeries
        {
            get => lineSeries;
            set
            {
                lineSeries = value;
                OnPropertyChanged(nameof(lineSeries));
            }
        }
        public TreeGraphView TreeGraphView_win
        { 
            get => _treeGraphView_win;
            set => _treeGraphView_win = value; 
        }
        public ObservableCollection<TreeViewItemViewModel> FirstGeneration
        {
            get => _firstGeneration;
        }
        public GraphVisualProps VisualProps { get; } = new GraphVisualProps();
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
        public ICommand DeleteCommand
        {
            get => _deleteCommand ??= new RelayCommand(o => Delete());
        }
        public TreeGraphViewModel(IOneVariableFunction rootFunction)
        {
            _rootFunction = new TreeViewItemViewModel(rootFunction, this);

            _firstGeneration = new ObservableCollection<TreeViewItemViewModel>
                (
                    new ObservableCollection<TreeViewItemViewModel>()
                    {
                        _rootFunction,
                    }
                );
        }
        private void GetY()
        {
            var selectedTreeViewItem = TreeGraphView_win.FunctionTreeView.SelectedItem as TreeViewItemViewModel;
            if (selectedTreeViewItem is null)
            {
                return;
            }
            SelectedFuntion = selectedTreeViewItem.Function;
            var vm = new GetValueViewModel(SelectedFuntion);
            var v = new GetValueView();
            v.DataContext = vm;
            v.ShowDialog();
        }
        private void Scale(object parameter)
        {
            var selectedTreeViewItem = TreeGraphView_win.FunctionTreeView.SelectedItem as TreeViewItemViewModel;
            if (selectedTreeViewItem is null)
            {
                return;
            }
            ScaleViewModel vm = null;
            var v = new ScaleView();
            var type = parameter as string;
            if (type.Equals("x"))
            {
                vm = new ScaleViewModel(true);
                v.DataContext = vm;
                if (v.ShowDialog() == true)
                {
                    SelectedFuntion = new ScaleXDecorator(SelectedFuntion, vm.ScaleFactor);
                    var child = new TreeViewItemViewModel(SelectedFuntion, selectedTreeViewItem, this);
                    selectedTreeViewItem.Children.Add(child);
                    selectedTreeViewItem.IsExpanded = true;
                }
            }
            else if (type.Equals("y"))
            {
                vm = new ScaleViewModel(false);
                v.DataContext = vm;
                if (v.ShowDialog() == true)
                {
                    SelectedFuntion = new ScaleYDecorator(SelectedFuntion, vm.ScaleFactor);
                    var child = new TreeViewItemViewModel(SelectedFuntion, selectedTreeViewItem, this);
                    selectedTreeViewItem.Children.Add(child);
                    selectedTreeViewItem.IsExpanded = true;
                }
            }
            else
            {
                return;
            }
        }
        private void Limit(object parameter)
        {
            var selectedTreeViewItem = TreeGraphView_win.FunctionTreeView.SelectedItem as TreeViewItemViewModel;
            if (selectedTreeViewItem is null)
            {
                return;
            }
            LimViewModel vm = null;
            var v = new LimView();
            var type = parameter as string;
            if (type.Equals("x"))
            {
                vm = new LimViewModel(true);
                v.DataContext = vm;
                if (v.ShowDialog() == true)
                {
                    SelectedFuntion = new LimXDecorator(SelectedFuntion, vm.LeftBound, vm.RightBound);
                    var child = new TreeViewItemViewModel(SelectedFuntion, selectedTreeViewItem, this);
                    selectedTreeViewItem.Children.Add(child);
                    selectedTreeViewItem.IsExpanded = true;
                }
            }
            else if (type.Equals("y"))
            {
                vm = new LimViewModel(false);
                v.DataContext = vm;
                if (v.ShowDialog() == true)
                {
                    SelectedFuntion = new LimYDecorator(SelectedFuntion, vm.LeftBound, vm.RightBound);
                    var child = new TreeViewItemViewModel(SelectedFuntion, selectedTreeViewItem, this);
                    selectedTreeViewItem.Children.Add(child);
                    selectedTreeViewItem.IsExpanded = true;
                }
            }
            else
            {
                return;
            }
        }
        private void Delete()
        {
            var selectedTreeViewItem = TreeGraphView_win.FunctionTreeView.SelectedItem as TreeViewItemViewModel;
            if (selectedTreeViewItem is null)
            {
                return;
            }
            var selectedTreeViewItemParent = selectedTreeViewItem.Parent;
            if (selectedTreeViewItemParent is null)
            {
                return;
            }
            selectedTreeViewItemParent.Children.Remove(selectedTreeViewItem);
        }
        public void DrawGraph()
        {
            var labels = new List<string>();
            var lineSeries = new LineSeries();
            var seriesCollection = new SeriesCollection();
            var chartValues = new ChartValues<double>();
            var selectedTreeViewItem = TreeGraphView_win.FunctionTreeView.SelectedItem as TreeViewItemViewModel;
            if (selectedTreeViewItem is null)
            {
                return;
            }
            SelectedFuntion = selectedTreeViewItem.Function;
            var graphSettings = SelectedFuntion.GetGraphSettings();
            Labels = graphSettings.GetLabels();
            LineSeries = graphSettings.GetLineSeries();
            GraphService.SetVisualProps(LineSeries, VisualProps);
            SeriesCollection = new SeriesCollection();
            SeriesCollection.Add(LineSeries);
        }
    }
}
