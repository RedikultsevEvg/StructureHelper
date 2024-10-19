﻿using StructureHelper.Infrastructure;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Functions;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StructureHelper.Windows.MainGraph
{
    public class TableViewModel : ViewModelBase
    {
        private const string DEFAULT_NAME = "Put function name here...";
        private const string DEFAULT_DESCRIPTION = "Put function description here...";
        private RelayCommand drawGraphCommand;
        private RelayCommand addPointCommand;
        private RelayCommand deletePointCommand;
        public ICommand DrawGraphCommand
        {
            get => drawGraphCommand ??= new RelayCommand(o => Save(o));
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
        private string description;
        public string Description
        {
            get => description;
            set
            {
                description = value;
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
        }
        public TableViewModel(TableFunction tableFunction)
        {
            Function = tableFunction;
            Table = new ObservableCollection<GraphPoint>((Function as TableFunction).Table);
            Name = Function.Name;
            Description = Function.Description;
        }
        private void Save(object parameter)
        {
            if (Function is null)
            {
                Function = new TableFunction();
            }
            Function.Name = Name;
            Function.Description = Description;
            Function.IsUser = true;
            (Function as TableFunction).Table = Table.ToList();
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
