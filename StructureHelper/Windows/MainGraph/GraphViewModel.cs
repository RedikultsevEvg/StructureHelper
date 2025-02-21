using LiveCharts;
using LiveCharts.Wpf;
using StructureHelper.Infrastructure;
using StructureHelper.Windows.Graphs;
using StructureHelper.Windows.TreeGraph;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models.Functions;
using StructureHelperLogics.Models.Graphs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace StructureHelper.Windows.MainGraph
{
    public class GraphViewModel : ViewModelBase
    {
        private LineSeries lineSeries;
        private SeriesCollection seriesCollection;
        private List<string> labels;
        private GraphVisualProps visualProps = new();
        public LineSeries LineSeries
        {
            get => lineSeries;
            set
            {
                lineSeries = value;
                OnPropertyChanged(nameof(lineSeries));
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
        public GraphVisualProps VisualProps
        {
            get => visualProps;
            set
            {
                visualProps = value;
                DrawGraph();
            }
        }
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
                DrawGraph();
                OnPropertyChanged(nameof(SelectedFuntion));
            }
        }
        private ObservableCollection<IOneVariableFunction> functions;
        public ObservableCollection<IOneVariableFunction> Functions { get; set; }

        private RelayCommand addTableCommand;
        private RelayCommand addFormulaCommand;
        private RelayCommand editCommand;
        private RelayCommand deleteCommand;
        private RelayCommand copyCommand;
        private RelayCommand treeCommand;
        private RelayCommand drawGraphCommand;
        private RelayCommand _saveCommand;
        public ICommand AddTableCommand
        {
            get => addTableCommand ??= new RelayCommand(o => AddTable());
        }
        public ICommand AddFormulaCommand
        {
            get => addFormulaCommand ??= new RelayCommand(o => AddFormula());
        }
        public ICommand EditCommand
        {
            get => editCommand ??= new RelayCommand(o => Edit(o));
        }
        public ICommand DeleteCommand
        {
            get => deleteCommand ??= new RelayCommand(o => Delete());
        }
        public ICommand CopyCommand
        {
            get => copyCommand ??= new RelayCommand(o => Copy());
        }
        public ICommand TreeCommand
        {
            get => treeCommand ??= new RelayCommand(o => Tree());
        }
        public ICommand DrawGraphCommand
        {
            get => drawGraphCommand ??= new RelayCommand(o => DrawGraph());
        }
        public ICommand SaveCommand
        {
            get => _saveCommand ??= new RelayCommand(o => Save());
        }
        public GraphViewModel()
        {
            Functions = ProgramSetting.Functions;
        }
        private void AddTable()
        {
            var tableViewModel = new TableViewModel();
            var tableView = new TableView();
            tableView.DataContext = tableViewModel;
            if (tableView.ShowDialog() == true)
            {
                Functions.Add(tableViewModel.Function);
                SelectedFuntion = tableViewModel.Function;
            }
            Save();
        }
        private void AddFormula()
        {
            var formulaViewModel = new FormulaViewModel();
            var formulaView = new FormulaView();
            formulaView.DataContext = formulaViewModel;
            if (formulaView.ShowDialog() == true)
            {
                Functions.Add(formulaViewModel.Function);
                SelectedFuntion = formulaViewModel.Function;
            }
            Save();
        }
        private void Edit(object parameter)
        {
            if (SelectedFuntion is null)
            {
                return;
            }
            if (SelectedFuntion.Type == FunctionType.TableFunction)
            {
                var tableViewModel = new TableViewModel(SelectedFuntion as TableFunction);
                var tableView = new TableView();
                tableView.DataContext = tableViewModel;
                tableView.ShowDialog();
                SelectedFuntion = tableViewModel.Function;
            }
            else if (SelectedFuntion.Type == FunctionType.FormulaFunction)
            {
                var formulaViewModel = new FormulaViewModel(SelectedFuntion as FormulaFunction);
                var formulaView = new FormulaView();
                formulaView.DataContext = formulaViewModel;
                formulaView.ShowDialog();
                SelectedFuntion = formulaViewModel.Function;
            }
            var graphView = parameter as GraphView;
            graphView.Refresh();
            Save();
        }
        private void Delete()
        {
            if (SelectedFuntion is null)
            {
                var lastFunction = Functions[Functions.Count - 1];
                if (lastFunction.IsUser)
                {
                    Functions.Remove(lastFunction);
                }
            }
            else
            {
                Functions.Remove(SelectedFuntion);
            }
            Save();
        }
        private void Copy()
        {
            if (SelectedFuntion is null)
            {
                return;
            }
            else if (SelectedFuntion.Type == FunctionType.TableFunction)
            {
                Functions.Add(SelectedFuntion.Clone() as TableFunction);
            }
            else if (SelectedFuntion.Type == FunctionType.FormulaFunction)
            {
                Functions.Add(SelectedFuntion.Clone() as FormulaFunction);
            }
            Save();
        }
        private void Tree()
        {
            if (SelectedFuntion is null)
            {
                return;
            }
            var treeGraphVM = new TreeGraphViewModel(SelectedFuntion);
            var treeGraph = new TreeGraphView();
            treeGraph.DataContext = treeGraphVM;
            treeGraphVM.TreeGraphView_win = treeGraph;
            treeGraph.ShowDialog();
        }
        private void DrawGraph()
        {
            var graphSettings = SelectedFuntion.GetGraphSettings();
            Labels = graphSettings.GetLabels();
            LineSeries = graphSettings.GetLineSeries();
            GraphService.SetVisualProps(LineSeries, VisualProps);
            SeriesCollection = new SeriesCollection();
            SeriesCollection.Add(LineSeries);
            Save();
        }
        public void Save()
        {
            ProgramSetting.Functions = Functions;
        }
    }
}
