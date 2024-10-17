﻿using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Functions
{
    public class TableFunction : IOneVariableFunction
    {
        private string name;
        public bool IsUser { get; set; }
        public FunctionType Type { get; set; }
        public string Name 
        {
            get => name;
            set
            {
                name = value;
            }
        }
        public string Description { get; set; }
        public List<GraphPoint> Table { get; set; }

        public Guid Id => throw new NotImplementedException();

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool Check()
        {
            throw new NotImplementedException();
        }

        public object Clone()
        {
            var tableFunction = new TableFunction();

            //Здесь будет стратегия
            tableFunction.Type = Type;
            tableFunction.Name = Name;
            tableFunction.Description = Description;
            tableFunction.Table = Table;
            tableFunction.IsUser = true;

            return tableFunction;
        }

        public double GetByX(double xValue)
        {
            throw new NotImplementedException();
        }
    }
}
