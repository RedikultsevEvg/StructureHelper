using LiveCharts;
using LiveCharts.Wpf;
using StructureHelper.Infrastructure;
using StructureHelper.Windows.TreeGraph;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
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
        private SeriesCollection seriesCollection;
        private List<string> labels;
        public SeriesCollection SeriesCollection
        {
            get => seriesCollection;
            set
            {
                seriesCollection = value;
                OnPropertyChanged(nameof(seriesCollection));
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
        public GraphViewModel()
        {
            //Пример 1
            Functions = new ObservableCollection<IOneVariableFunction>();
            var f1 = new TableFunction();
            f1.Name = "Табличная системная функция";
            f1.Table = new List<GraphPoint>()
            {
                new GraphPoint(1, 1),
                new GraphPoint(2, 2),
                new GraphPoint(3, 3),
                new GraphPoint(4, 4),
                new GraphPoint(5, 5),
                new GraphPoint(6, 6),
            };
            f1.IsUser = false;
            f1.Description = "Описание табличной системной функции";
            //Пример 2
            var f2 = new FormulaFunction();
            f2.Name = "Формульная системная функция";
            f2.Formula = "x^2";
            f2.Step = 100;
            f2.MinArg = 1;
            f2.MaxArg = 1000;
            f2.IsUser = false;
            f2.Description = "Описание формульной системной функции";

            Functions.Add(f1);
            Functions.Add(f2);
        }
        /*public GraphViewModel(IGraph graph) 
        {
        }*/
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
        }
        private void Tree()
        {
            if (SelectedFuntion is null)
            {
                return;
            }

            //var testFunction = Database.GetFunctionTree();
            //var treeGraphVM = new TreeGraphViewModel(testFunction);

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
            SeriesCollection = graphSettings.GetSeriesCollection();
        }
    }
}
