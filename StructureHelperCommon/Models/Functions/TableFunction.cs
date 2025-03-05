using LiveCharts;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureHelperCommon.Services;
using StructureHelperCommon.Services.ColorServices;

namespace StructureHelperCommon.Models.Functions
{
    public class TableFunction : IOneVariableFunction
    {
        private const string COPY = "copy";
        public const string GROUP_TYPE_1 = "System function";
        public const string GROUP_TYPE_2 = "User function";

        private string name;

        public bool IsUser { get; set; }
        public FunctionType Type { get; set; }
        public FunctionPurpose FunctionPurpose { get; set; }
        public string Group { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<GraphPoint> Table { get; set; }
        public Guid Id => throw new NotImplementedException();
        public ObservableCollection<IOneVariableFunction> Functions { get; set; } = new ObservableCollection<IOneVariableFunction>();
        public double MinArg { get; set; }
        public double MaxArg { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }
        public Color Color { get; set; }
        public string Trace { get; set; }

        public TableFunction(bool isUser = false)
        {
            Color = ColorProcessor.GetRandomColor();
            Type = FunctionType.TableFunction;
            if (isUser)
            {
                IsUser = true;
                Group = GROUP_TYPE_2;
            }
            else
            {
                IsUser = false;
                Group = GROUP_TYPE_1;
            }
        }
        public bool Check()
        {
            throw new NotImplementedException();
        }

        public object Clone()
        {
            var tableFunction = new TableFunction();
            tableFunction.Type = Type;
            tableFunction.Name = $"{Name} {COPY}";
            tableFunction.Description = Description;
            var newTable = new List<GraphPoint>();
            Table.ForEach(x => newTable.Add(x.Clone() as GraphPoint));
            tableFunction.Table = newTable;
            tableFunction.IsUser = true;
            tableFunction.Group = GROUP_TYPE_2;
            tableFunction.FunctionPurpose = FunctionPurpose;
            return tableFunction;
        }

        public double GetByX(double xValue)
        {
            double yValue = 0;
            GraphPoint leftBound = null;
            GraphPoint rightBound = null;
            for (int i = 0; i < Table.Count - 1; i++)
            {
                leftBound = Table[i];
                rightBound = Table[i + 1];
                if (xValue == leftBound.X)
                {
                    yValue = leftBound.Y;
                }
                else if (xValue == rightBound.X)
                {
                    yValue = rightBound.Y;
                }
                else
                {
                    yValue = MathUtils.Interpolation(xValue, leftBound.X, rightBound.X, leftBound.Y, rightBound.Y);
                }
            }
            Trace = string.Empty;
            Trace += $"From table, Input: {xValue}, Output: {yValue};\n";
            return yValue;
        }
        public GraphSettings GetGraphSettings()
        {
            var graphSettings = new GraphSettings(Color, Color);
            foreach (GraphPoint point in Table)
            {
                var graphPoint = new GraphPoint(point.X, GetByX(point.X));
                graphSettings.GraphPoints.Add(graphPoint);
            }            
            return graphSettings;
        }
        public string GetTrace()
        {
            return Trace;
        }
    }
}
