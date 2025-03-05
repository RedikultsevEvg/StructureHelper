using StructureHelper.Infrastructure;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Functions;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services.ColorServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace StructureHelper.Windows.MainGraph
{
    public class TableViewModel : ViewModelBase
    {
        private const string DEFAULT_NAME = "Put function name here...";
        private const string DEFAULT_DESCRIPTION = "Put function description here...";
        private RelayCommand saveCommand;
        private RelayCommand addPointCommand;
        private RelayCommand deletePointCommand;
        private RelayCommand editColor;
        public ICommand EditColorCommand
        {
            get => editColor ??= new RelayCommand(o => EditColor());
        }
        public ICommand SaveCommand
        {
            get => saveCommand ??= new RelayCommand(o => Save(o));
        }
        public ICommand AddPointCommand
        {
            get => addPointCommand ??= new RelayCommand(o => AddPoint());
        }
        public ICommand DeletePointCommand
        {
            get => deletePointCommand ??= new RelayCommand(o => DeletePoint());
        }

        private ObservableCollection<GraphPoint> table;

        public ObservableCollection<GraphPoint> Table
        {
            get => table;
            set
            {
                table = value;
            }
        }
        private GraphPoint selectedPoint;
        public GraphPoint SelectedPoint
        {
            get => selectedPoint;
            set
            {
                selectedPoint = value;
                OnPropertyChanged(nameof(SelectedPoint));
            }
        }
        private IOneVariableFunction function;
        public IOneVariableFunction Function 
        { 
            get => function;
            set
            {
                function = value;
            }
        }
        private string name;
        public string Name 
        {
            get => name;
            set
            {
                name = value;
            }
        }
        private FunctionPurpose functionPurpose;
        public FunctionPurpose FunctionPurpose
        {
            get => functionPurpose;
            set
            {
                functionPurpose = value;
            }
        }
        private string description;
        public string Description
        {
            get => description;
            set
            {
                description = value;
            }
        }
        private Color color;
        public Color Color
        {
            get => color;
            set
            {
                color = value;
                OnPropertyChanged(nameof(Color));
            }
        }
        public TableViewModel()
        {
            Table = new ObservableCollection<GraphPoint>()
            {
                new GraphPoint(0, 0),
                new GraphPoint(0, 0),
            };
            Name = DEFAULT_NAME;
            Description = DEFAULT_DESCRIPTION;
            Color = Brushes.Red.Color;
        }
        public TableViewModel(TableFunction tableFunction)
        {
            Function = tableFunction;
            Table = new ObservableCollection<GraphPoint>((Function as TableFunction).Table);
            Name = Function.Name;
            FunctionPurpose = Function.FunctionPurpose;
            Description = Function.Description;
            Color = Function.Color;
        }
        private void EditColor()
        {
            Color color = new Color();
            ColorProcessor.EditColor(ref color);
            Color = color;
        }
        private void Save(object parameter)
        {
            if (Function is null)
            {
                Function = new TableFunction(isUser: true);
            }
            Function.Name = Name;
            Function.Description = Description;
            Function.IsUser = true;
            (Function as TableFunction).Table = Table.OrderBy(x => x.X).ToList();
            Function.Color = Color;
            Function.FunctionPurpose = FunctionPurpose;
            var window = parameter as Window;
            window.DialogResult = true;
            window.Close();
        }
        private void AddPoint()
        {
            var point = new GraphPoint(0, 0);
            if (SelectedPoint is null)
            {
                Table.Add(point);
            }
            else
            {
                var selectedPointIndex = Table.IndexOf(SelectedPoint);
                Table.Insert(selectedPointIndex + 1, point);
            }
        }
        private void DeletePoint()
        {
            if (Table.Count < 3)
            {
                return;
            }
            if (SelectedPoint is null)
            {
                Table.RemoveAt(Table.Count - 1);
            }
            else
            {
                Table.Remove(SelectedPoint);
            }
        }
    }
}
