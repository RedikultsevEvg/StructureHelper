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
using System.Windows.Controls;


namespace StructureHelper.Windows.TreeGraph
{
    public class TreeGraphViewModel : ViewModelBase
    {
        private LineSeries lineSeries;
        private SeriesCollection seriesCollection;
        private List<string> labels;
        private ObservableCollection<TreeViewItemViewModel> _tree;
        private TreeViewItemViewModel _root;
        readonly ICommand _searchCommand;
        private RelayCommand _getYCommand;
        private RelayCommand _scaleCommand;
        private RelayCommand _limCommand;
        private RelayCommand _deleteCommand;
        private RelayCommand _treeCommand;
        private RelayCommand _renameCommand;
        private RelayCommand _saveCommand;
        private TreeGraphView _treeGraphView_win;
        private IOneVariableFunction selectedFunction;
        private IOneVariableFunction rootFunction;
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
        public IOneVariableFunction RootFunction
        {
            get
            {
                return rootFunction;
            }
            set
            {
                rootFunction = value;
                OnPropertyChanged(nameof(RootFunction));
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
        public ObservableCollection<TreeViewItemViewModel> Tree
        {
            get => _tree;
            set
            {
                _tree = value;
                OnPropertyChanged(nameof(Tree));
            }
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
        public ICommand TreeCommand
        {
            get => _treeCommand ??= new RelayCommand(o => NewTree(o));
        }
        public ICommand RenameCommand
        {
            get => _renameCommand ??= new RelayCommand(o => Rename(o));
        }
        public ICommand SaveCommand
        {
            get => _saveCommand ??= new RelayCommand(o => Save());
        }
        public TreeGraphViewModel(IOneVariableFunction rootFunction)
        {
            RootFunction = rootFunction;
            RunTreeView(rootFunction);
        }
        private void RunTreeView(IOneVariableFunction rootFunction)
        {
            _root = new TreeViewItemViewModel(rootFunction, this);
            Tree = new ObservableCollection<TreeViewItemViewModel>
                (
                    new ObservableCollection<TreeViewItemViewModel>()
                    {
                        _root,
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
                    var newFunction = new ScaleXDecorator(SelectedFuntion, vm.ScaleFactor);
                    var child = new TreeViewItemViewModel(newFunction, selectedTreeViewItem, this);
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
                    var newFunction = new ScaleYDecorator(SelectedFuntion, vm.ScaleFactor);
                    var child = new TreeViewItemViewModel(newFunction, selectedTreeViewItem, this);
                    selectedTreeViewItem.Children.Add(child);
                    selectedTreeViewItem.IsExpanded = true;
                }
            }
            else
            {
                return;
            }
            Save();
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
                    var newFunction = new LimXDecorator(SelectedFuntion, vm.LeftBound, vm.RightBound);
                    var child = new TreeViewItemViewModel(newFunction, selectedTreeViewItem, this);
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
                    var newFunction = new LimYDecorator(SelectedFuntion, vm.LeftBound, vm.RightBound);
                    var child = new TreeViewItemViewModel(newFunction, selectedTreeViewItem, this);
                    selectedTreeViewItem.Children.Add(child);
                    selectedTreeViewItem.IsExpanded = true;
                }
            }
            else
            {
                return;
            }
            Save();
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
            Save();
        }
        private void Rename(object parameter)
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
            var renameViewModel = new RenameViewModel(selectedTreeViewItem);
            var renameView = new RenameView();
            renameView.DataContext = renameViewModel;
            if (renameView.ShowDialog() == true)
            {
                selectedTreeViewItem.Name = renameViewModel.FunctionName;
            }
            Save();
        }
        private void NewTree(object parameter)
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
            var treeGraphVM = new TreeGraphViewModel(SelectedFuntion);
            var treeGraph = new TreeGraphView();
            treeGraph.DataContext = treeGraphVM;
            treeGraphVM.TreeGraphView_win = treeGraph;
            treeGraph.ShowDialog();
            Save();
            RunTreeView(RootFunction);
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
        public void Save()
        {
            GetFunctionTree(Tree, RootFunction);
        }
        private void GetFunctionTree(ObservableCollection<TreeViewItemViewModel> tree, IOneVariableFunction function)
        {
            function.Functions.Clear();
            foreach (TreeViewItemViewModel item in tree)
            {
                if (item.Function is null)
                {
                    return;
                }
                function.Functions.Add(item.Function);
                if (item.Children.Count > 0)
                {
                    var newTree = item.Children;
                    GetFunctionTree(newTree, item.Function);
                }
            }
        }
    }
}
