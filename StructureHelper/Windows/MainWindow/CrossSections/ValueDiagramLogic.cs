using LoaderCalculator.Data.Matrix;
using StructureHelper.Services.ResultViewers;
using StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews;
using StructureHelper.Windows.Graphs;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Parameters;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StructureHelper.Windows.MainWindow.CrossSections
{
    internal class ValueDiagramLogic
    {
        private IValueDiagramCalculatorResult valueDiagramResult;
        private List<ForceResultFunc> resultFuncs = [];
        private List<string> labels;

        public ValueDiagramLogic(IValueDiagramCalculatorResult valueDiagramResult)
        {
            this.valueDiagramResult = valueDiagramResult;
        }

        internal void Show()
        {
            resultFuncs.AddRange(ForceResultFuncFactory.GetResultFuncs(FuncsTypes.Strain));
            resultFuncs.AddRange(ForceResultFuncFactory.GetResultFuncs(FuncsTypes.Stress));
            GetLabels();
            if (valueDiagramResult.IsValid == false ||
                valueDiagramResult.ForceTupleResults
                .Where(x => x.IsValid)
                .Count() == 0)
            {
                //
            }
            List<Series> seriesList = [];
            foreach (var forceTupleResult in valueDiagramResult.ForceTupleResults)
            {
                if (forceTupleResult.IsValid == false) { continue; }
                foreach (var entityResult in valueDiagramResult.EntityResults)
                {
                    var primitives = valueDiagramResult.InputData.Primitives
                    .Where(x => x is IHasDivisionSize);
                    Series series = GetSeries(forceTupleResult, valueDiagramResult.InputData.Primitives, entityResult.PointList);
                    if (valueDiagramResult.EntityResults.Count > 1)
                    {
                        series.Name = $"${entityResult.ValueDiagramEntity.Name} ({series.Name})";
                    }
                    seriesList.Add(series);
                }                
            }
            var vm = new GraphViewModel(seriesList);
            var wnd = new GraphView(vm);
            wnd.ShowDialog();
        }

        private Series GetSeries(IForceTupleCalculatorResult tupleResult, List<INdmPrimitive> ndmPrimitives, List<IPoint2D> points)
        {
            List<(INdmPrimitive ndmPrimitive, IPoint2D point)> pointPrimitives = GetPrimitivePoints(ndmPrimitives, points);
            if (pointPrimitives.Count == 0)
            {
                throw new StructureHelperException("There are not points for drawings");
            }
            ArrayParameter<double> arrayParameter = new(pointPrimitives.Count + 3, labels);
            var data = arrayParameter.Data;
            IPoint2D startPoint = pointPrimitives[0].point;
            IPoint2D endPoint = pointPrimitives[^1].point;
            IStrainMatrix strainMatrix = tupleResult.LoaderResults.StrainMatrix;
            for (int i = 0; i < pointPrimitives.Count; i++)
            {
                IPoint2D currentPoint = pointPrimitives[i].point;
                double distance = GetDistance(startPoint, currentPoint);
                data[i, 0] = distance;
                data[i, 1] = currentPoint.X;
                data[i, 2] = currentPoint.Y;
                for (int j = 0; j < resultFuncs.Count; j++)
                {
                    data[i, j + 3] = GetValueByPoint(strainMatrix, pointPrimitives[i].ndmPrimitive, pointPrimitives[i].point, resultFuncs[j]);
                }

            }
            data[pointPrimitives.Count, 0] = GetDistance(startPoint, endPoint);
            data[pointPrimitives.Count, 1] = endPoint.X;
            data[pointPrimitives.Count, 2] = endPoint.Y;

            data[pointPrimitives.Count + 1, 0] = 0.0;
            data[pointPrimitives.Count + 1, 1] = startPoint.X;
            data[pointPrimitives.Count + 1, 2] = startPoint.Y;

            data[pointPrimitives.Count, 0] = 0.0;
            data[pointPrimitives.Count + 2, 1] = startPoint.X;
            data[pointPrimitives.Count + 2, 2] = startPoint.Y;
            for (int j = 0; j < resultFuncs.Count; j++)
            {
                data[pointPrimitives.Count, j+3] = 0.0;
                data[pointPrimitives.Count + 1, j+3] = 0.0;
                data[pointPrimitives.Count + 2, j + 3] = GetValueByPoint(strainMatrix, pointPrimitives[0].ndmPrimitive, pointPrimitives[0].point, resultFuncs[j]);
            }

            StructureHelperCommon.Models.Forces.IForceTuple inputForceTuple = tupleResult.InputData.ForceTuple;
            Series series = new Series(arrayParameter)
            {
                Name = $"Mx = {inputForceTuple.Mx}, My = {inputForceTuple.My}, Nz = {inputForceTuple.Nz}",
            };
            return series;
        }

        private static double GetDistance(IPoint2D startPoint, IPoint2D currentPoint)
        {
            double dx = currentPoint.X - startPoint.X;
            double dy = currentPoint.Y - startPoint.Y;
            double distance = Math.Sqrt(dx * dx + dy * dy);
            return distance;
        }

        private List<(INdmPrimitive ndmPrimitive, IPoint2D point)> GetPrimitivePoints(List<INdmPrimitive> primitives, List<IPoint2D> points)
        {
            List<(INdmPrimitive ndmPrimitive, IPoint2D point)> values = [];
            for (int i = 0; i < points.Count; i++)
            {
                var currentPoint = points[i];

                var areaPrimitives = GetPrimitivesInPoint(primitives, currentPoint);

                if (areaPrimitives.Count() == 1)
                {
                    (INdmPrimitive ndmPrimitive, IPoint2D point) newValue = (areaPrimitives[0], points[i]);
                    values.Add(newValue);
                }
                else if (areaPrimitives.Count() > 1)
                {
                    values.AddRange(GetPrimitiveByFewPoints(areaPrimitives, points, i));
                }
            }
            return values;
        }

        private static List<INdmPrimitive> GetPrimitivesInPoint(List<INdmPrimitive> primitives, IPoint2D point)
        {
            return primitives
                .Where(x => x is  IHasDivisionSize)
                .Where(x => (x as IHasDivisionSize).IsPointInside(point) == true)
                .ToList();
        }

        private List<(INdmPrimitive ndmPrimitive, IPoint2D point)> GetPrimitiveByFewPoints(List<INdmPrimitive> areaPrimitives, List<IPoint2D> points, int i)
        {
            List<(INdmPrimitive ndmPrimitive, IPoint2D point)> values = [];
            var primitives = areaPrimitives
                .OrderBy(x=> x.VisualProperty.ZIndex)
                .ToList();
            if (primitives[0] is IHasDivisionSize firstPrimitiveHasSize)
            {
                if (firstPrimitiveHasSize.DivisionSize.ClearUnderlying == true)
                {
                    if (primitives[0].NdmElement.Triangulate == true)
                    {
                        (INdmPrimitive ndmPrimitive, IPoint2D point) newValue = (primitives[0], points[i]);
                        values.Add(newValue);
                    }
                }
                else
                {
                    if (primitives.Count > 2)
                    {
                        throw new StructureHelperException($"Too many primitives in point X = {points[i].X}, Y = {points[i].Y}");
                    }
                    if (i == 0)
                    {
                        var primitive = GetPrimitiveInAjacentPoint(primitives, points, i, 1);
                        (INdmPrimitive ndmPrimitive, IPoint2D point) newValue = (primitive, points[i]);
                        values.Add(newValue);
                    }
                    else if (i == points.Count - 1)
                    {
                        var primitive = GetPrimitiveInAjacentPoint(primitives, points, i, -1);
                        (INdmPrimitive ndmPrimitive, IPoint2D point) newValue = (primitive, points[i]);
                        values.Add(newValue);
                    }
                    else
                    {
                        foreach (var primitive in primitives)
                        {
                            (INdmPrimitive ndmPrimitive, IPoint2D point) newValue = (primitive, points[i]);
                            values.Add(newValue);
                        }
                    }
                }
            }
            return values;
        }

        private INdmPrimitive GetPrimitiveInAjacentPoint(List<INdmPrimitive> primitives, List<IPoint2D> points, int i, int sign)
        {
            var nextPointPrimitives = GetPrimitivesInPoint(primitives, points[i + 1 * sign]);
            try
            {
                var primitive = nextPointPrimitives.Single();
                return primitive;
            }
            catch (Exception ex)
            {
                throw new StructureHelperException($"Error of obtaining of primitive X = {points[i].X}, Y = {points[i].Y}");
            }
        }

        private double GetValueByPoint(IStrainMatrix strainMatrix, INdmPrimitive primitive, IPoint2D point, ForceResultFunc resultFunc)
        {
            var logic = new GetMoqNdmLogic();
            var moqNdm = logic.GetMockNdm(primitive, valueDiagramResult.InputData.StateTermPair, point);
            return resultFunc.ResultFunction(strainMatrix, moqNdm) * resultFunc.UnitFactor;
        }

        private void GetLabels()
        {
            labels = [];
            labels.Add("Distance");
            labels.Add("Global X");
            labels.Add("Global Y");
            foreach (var resultFunc in resultFuncs)
            {
                labels.Add($"{resultFunc.Name}, {resultFunc.UnitName}");
            }
        }
    }
}
