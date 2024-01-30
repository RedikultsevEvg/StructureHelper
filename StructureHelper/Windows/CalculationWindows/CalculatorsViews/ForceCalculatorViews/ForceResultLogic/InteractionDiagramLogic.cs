using StructureHelper.Windows.Graphs;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models.Parameters;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services.Units;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.LimitCurve;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.Services.NdmPrimitives;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

//Copyright (c) 2023 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    internal class InteractionDiagramLogic : ILongProcessLogic
    {
        const string ForceUnitString = "kN";
        const string MomentUnitString = "kNm";

        //private List<ArrayParameter<double>> arrayParameters;
        private IResult result;
        private IUnit unitForce = CommonOperation.GetUnit(UnitTypes.Force, ForceUnitString);
        private IUnit unitMoment = CommonOperation.GetUnit(UnitTypes.Moment, MomentUnitString);
        private int stepCount;

        private static GeometryNames GeometryNames => ProgramSetting.GeometryNames;
        public LimitCurveInputData InputData { get; set; }
        public int StepCount { get => stepCount; set => stepCount = value; }

        public Action<int> SetProgress { get; set; }
        public bool Result { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public InteractionDiagramLogic(LimitCurveInputData inputData)
        {
            InputData = inputData;
            stepCount = InputData.PointCount;
            stepCount *= inputData.PrimitiveSeries.Count;
            stepCount *= InputData.LimitStates.Count();
            stepCount *= InputData.CalcTerms.Count();
            stepCount *= InputData.PredicateEntries.Count();
            //arrayParameters = new();
        }

        private void DoCalculations()
        {
            var convertLogic = InputData.SurroundData.ConvertLogicEntity;
            var calculator = new LimitCurvesCalculator()
            {
                InputData = InputData,
            };
            if (TraceLogger is not null) { calculator.TraceLogger = TraceLogger; }
            calculator.ActionToOutputResults = SetProgressByResult;
            SafetyProcessor.RunSafeProcess(() =>
            {
                CalcResult(calculator);
            }, "Errors appeared during showing a graph, see detailed information");
        }

        private void CalcResult(LimitCurvesCalculator calculator)
        {
            calculator.Run();
            var curvesResult = calculator.Result as LimitCurvesResult;
            if (curvesResult.IsValid == false) { return; }
            result = curvesResult;
            foreach (var curveResult in curvesResult.LimitCurveResults)
            {
                ProcessCurveResult(curveResult);
            }
        }

        private void ProcessCurveResult(LimitCurveResult curveResult)
        {
            if (curveResult.IsValid == false)
            {
                SafetyProcessor.ShowMessage("Calculation error", curveResult.Description);
                return;
            }
            //var arrayParameter = GetParametersByCurveResult(curveResult);
            //arrayParameters.Add(arrayParameter);
        }

        private ArrayParameter<double> GetParametersByCurveResult(LimitCurveResult curveResult)
        {
            string[] labels = GetLabels();
            var items = curveResult.Points;
            var arrayParameter = new ArrayParameter<double>(items.Count(), labels.Count(), labels);
            var data = arrayParameter.Data;
            for (int i = 0; i < items.Count(); i++)
            {
                var valueList = new List<double>
                    {
                    // to do repair multiplay by surround data    
                    items[i].X * unitForce.Multiplyer,
                        items[i].Y * unitMoment.Multiplyer
                    };
                for (int j = 0; j < valueList.Count; j++)
                {
                    data[i, j] = valueList[j];
                }
            }
            return arrayParameter;
        }

        private void SetProgressByResult(IResult calcResult)
        {
            if (calcResult is not LimitCurvesResult)
            {
                throw new StructureHelperException(ErrorStrings.ExpectedWas(typeof(LimitCurvesResult), calcResult));
            }
            var parameterResult = calcResult as LimitCurvesResult;
            StepCount = stepCount;// parameterResult.MaxIterationCount;
            SetProgress?.Invoke(parameterResult.IterationNumber);
        }

        private string[] GetLabels()
        {
            string[] strings = new string[2];
            strings[0] = GetLabel(InputData.SurroundData.ConvertLogicEntity.XForceType);
            strings[1] = GetLabel(InputData.SurroundData.ConvertLogicEntity.YForceType);
            return strings;
        }

        private string GetLabel(ForceTypes forceType)
        {
            if (forceType == ForceTypes.Force)
            {
                return $"{GeometryNames.LongForceName}, {unitForce.Name}";
            }
            else if (forceType == ForceTypes.MomentMx)
            {
                return $"{GeometryNames.MomFstName}, {unitMoment.Name}";
            }
            else if (forceType == ForceTypes.MomentMy)
            {
                return $"{GeometryNames.MomSndName}, {unitMoment.Name}";
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(forceType));
            }
        }

        public void ShowWindow()
        {
            Result = true;
            SafetyProcessor.RunSafeProcess(() =>
            {
                if (result.IsValid == true)
                {
                    var curveResult = result as LimitCurvesResult;
                    var seriesList = new List<Series>();
                    foreach (var item in curveResult.LimitCurveResults)
                    {
                        var series = new Series(GetParametersByCurveResult(item)) { Name = item.Name };
                        seriesList.Add(series);
                    }
                    var vm = new GraphViewModel(seriesList);
                    var wnd = new GraphView(vm);
                    wnd.ShowDialog();
                }
                else
                {
                    MessageBox.Show(result.Description);
                }
            },
            "Errors appeared during showing a graph, see detailed information");
        }

        public void WorkerDoWork(object sender, DoWorkEventArgs e)
        {
            DoCalculations();
            Result = true;
        }

        public void WorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //nothing to do
        }

        public void WorkerRunWorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //nothing to do
        }
    }
}
