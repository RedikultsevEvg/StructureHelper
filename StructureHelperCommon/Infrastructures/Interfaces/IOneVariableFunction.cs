using LiveCharts;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Functions;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    public interface IOneVariableFunction : ICloneable, ISaveable, ILogic
    {
        public const string GROUP_TYPE_1 = "System function";
        public const string GROUP_TYPE_2 = "User function";
        public bool IsUser { get; set; }
        public string Group { get; set; }
        public FunctionType Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double MinArg { get; set; }
        public double MaxArg { get; set; }
        public Color Color { get; set; }
        public string Trace { get; set; }
        public ObservableCollection<IOneVariableFunction> Functions { get; set; }
        public bool Check();
        public double GetByX(double xValue);
        public GraphSettings GetGraphSettings();
        public string GetTrace();
    }
}
