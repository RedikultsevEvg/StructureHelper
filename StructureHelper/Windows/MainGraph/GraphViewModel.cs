using LiveCharts;
using LiveCharts.Wpf;
using StructureHelper.Infrastructure;
using StructureHelper.Windows.TreeGraph;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Functions;
using StructureHelperLogics.Models.Graphs;
using System.Collections.Generic;
using System.Windows.Input;

namespace StructureHelper.Windows.MainGraph
{
    public class GraphViewModel : ViewModelBase
    {
        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Labels { get; set; }



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
        private List<IOneVariableFunction> functions;
        public List<IOneVariableFunction> Functions { get; set; }







        private RelayCommand addTableCommand;
        private RelayCommand addFormulaCommand;
        private RelayCommand editCommand;
        private RelayCommand deleteCommand;
        private RelayCommand copyCommand;
        private RelayCommand treeCommand;
        public ICommand AddTableCommand
        { 
            get => addTableCommand ??= new RelayCommand(o => AddTable());
        }
        public ICommand AddFormulaCommand
        {
            get => addFormulaCommand ??= new RelayCommand(o => AddTable());
        }
        public ICommand EditCommand
        {
            get => editCommand ??= new RelayCommand(o => AddTable());
        }
        public ICommand DeleteCommand
        {
            get => deleteCommand ??= new RelayCommand(o => AddTable());
        }
        public ICommand CopyCommand
        {
            get => copyCommand ??= new RelayCommand(o => AddTable());
        }
        public ICommand TreeCommand
        {
            get => treeCommand ??= new RelayCommand(o => Tree());
        }
        public GraphViewModel()
        {
            Functions = new List<IOneVariableFunction>();
            var f1 = new TableFunction();
            f1.Name = "Пробная табличная системная функция 1";
            f1.IsUser = false;
            f1.Description = "Описание 1";
            var f2 = new TableFunction();
            f2.Name = "Пробная табличная пользовательская функция 2";
            f2.IsUser = true;
            f2.Description = "Описание 2";
            var f3 = new FormulaFunction();
            f3.Name = "Пробная формульная системная функция 3";
            f3.IsUser = false;
            f3.Description = "Описание 3";
            Functions.Add(f1);
            Functions.Add(f2);
            Functions.Add(f3);


            Labels = new List<string>();
            Labels.Add("1");
            Labels.Add("2");
            Labels.Add("2");
            Labels.Add("3");
            var chartValues = new ChartValues<double>();
            chartValues.Add(1);
            chartValues.Add(10);
            chartValues.Add(100);
            chartValues.Add(25);
            chartValues.Add(150);
            chartValues.Add(100);
            chartValues.Add(200);
            chartValues.Add(50);
            var lineSeries = new LineSeries();
            lineSeries.Values = chartValues;
            SeriesCollection = new SeriesCollection();
            SeriesCollection.Add(lineSeries);
        }
        /*public GraphViewModel(IGraph graph) 
        {
        }*/
        private void AddTable()
        {

        }
        private void AddFormula()
        {

        }
        private void Edit()
        {

        }
        private void Delete()
        {

        }
        private void Cppy()
        {

        }
        private void Tree()
        {
            var treeGraphVM = new TreeGraphViewModel(SelectedFuntion);
            var treeGraph = new TreeGraph.TreeGraph();
            treeGraph.DataContext = treeGraphVM;
            treeGraph.ShowDialog();
        }
    }
}
