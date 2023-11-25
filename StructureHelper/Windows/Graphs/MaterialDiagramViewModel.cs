using LiveCharts;
using LiveCharts.Wpf;
using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.UI.Converters.Units;
using StructureHelper.Models.Materials;
using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace StructureHelper.Windows.Graphs
{
    public class MaterialDiagramViewModel : ViewModelBase
    {
        class StressEntity
        {
            public LimitStateEntity LimitState { get; set; }
            public CalcTermEntity CalcTerm { get; set; }
            public IHeadMaterial Material { get; set; }
            public double Strain { get; set; }
            public double Stress { get; set; }
        }

        private IHeadMaterial material;
        private ICommand redrawLinesCommand;
        double minValue;
        double maxValue;
        int stepCount;
        bool positiveInTension;
        private ICommand getAreaCommand;

        public string MaterialName => material.Name;
        public GraphVisualProps VisualProps { get; }
        public double MinValue
        {
            get => minValue;
            set
            {
                minValue = Math.Min(value, maxValue - 0.001d);
                OnPropertyChanged(nameof(MinValue));
            }
        }
        public double MaxValue
        {
            get => maxValue;
            set
            {
                maxValue = Math.Max(value, minValue + 0.001d);
                OnPropertyChanged(nameof(MaxValue));
            }
        }
        public int StepCount
        {
            get => stepCount;
            set
            {
                stepCount = value;
                stepCount = Math.Max(stepCount, 10);
                stepCount = Math.Min(stepCount, 500);
                OnPropertyChanged(nameof(StepCount));
            }
        }
        public bool PositiveInTension
        {
            get => positiveInTension;
            set
            {
                positiveInTension = value;
                var tmpMinValue = minValue;
                minValue = maxValue * -1d;
                maxValue = tmpMinValue * -1d;
                OnPropertyChanged(nameof(PositiveInTension));
                OnPropertyChanged(nameof(MinValue));
                OnPropertyChanged(nameof(MaxValue));
            }
        }
        public SelectItemsViewModel<IHeadMaterial> MaterialsModel { get; private set; }
        public SelectItemsViewModel<LimitStateEntity> LimitStatesModel { get; private set; }
        public SelectItemsViewModel<CalcTermEntity> CalcTermsModel { get; private set; }
        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public ICommand RedrawLinesCommand
        {
            get => redrawLinesCommand ??= new RelayCommand(o => DrawLines(), b => IsDrawPossible());
        }

        public ICommand GetAreaCommand
        {
            get => getAreaCommand ??= new RelayCommand(o =>
            {
                var str = GetAreas();
                MessageBox.Show(str);
            }, b => IsDrawPossible());
        }

        public MaterialDiagramViewModel(IEnumerable<IHeadMaterial> headMaterials, IHeadMaterial material)
        {
            MaterialsModel = new SelectItemsViewModel<IHeadMaterial>(headMaterials) { ShowButtons = true };
            LimitStatesModel = new SelectItemsViewModel<LimitStateEntity>(ProgramSetting.LimitStatesList.LimitStates) { ShowButtons = false };
            CalcTermsModel = new SelectItemsViewModel<CalcTermEntity>(ProgramSetting.CalcTermList.CalcTerms) { ShowButtons = false };
            foreach (var item in MaterialsModel.CollectionItems)
            {
                if (item.Item == material)
                {
                    item.IsSelected = true;
                }
                else item.IsSelected = false;
            }
            this.material = material;
            minValue = -0.005d;
            maxValue = 0.005d;
            stepCount = 50;
            positiveInTension = true;
            VisualProps = new();
            SetLines();
        }
        private string GetAreas()
        {
            var materials = MaterialsModel.SelectedItems;
            var limitStates = LimitStatesModel.SelectedItems;
            var calcTerms = CalcTermsModel.SelectedItems;
            double step = (maxValue - minValue) / stepCount;
            var factor = positiveInTension ? 1d : -1d;

            var values = GetValueList();

            string str = string.Empty;

            foreach (var limitState in limitStates)
                {
                foreach (var calcTerm in calcTerms)
                    {
                    foreach (var material in materials)
                    {
                        double sumStress = 0d;
                        var stressList = values
                            .Where(x => x.LimitState == limitState & x.CalcTerm == calcTerm & x.Material == material).ToList();
                        for (int i = 1; i < stressList.Count(); i++)
                        {
                            double midStress = (stressList[i - 1].Stress + stressList[i].Stress) / 2;
                            sumStress += midStress * UnitConstants.Stress * step;
                        }
                        str += $"{material.Name}, {limitState.Name}, {calcTerm.Name}: {sumStress} \n";
                    }
                }
            }
            return str;
        }

        private List<StressEntity> GetValueList()
        {
            var materials = MaterialsModel.SelectedItems;
            var limitStates = LimitStatesModel.SelectedItems;
            var calcTerms = CalcTermsModel.SelectedItems;
            double step = (maxValue - minValue) / stepCount;
            var factor = positiveInTension ? 1d : -1d;

            var result = new List<StressEntity>();
            foreach (var limitState in limitStates)
            {
                foreach (var calcTerm in calcTerms)
                {
                    foreach (var material in materials)
                    {
                        var loaderMaterial = material.GetLoaderMaterial(limitState.LimitState, calcTerm.CalcTerm);
                        for (double s = minValue; s < maxValue; s += step)
                        {
                            double strain = s * factor;
                            double diagramValue = loaderMaterial.Diagram.Invoke(loaderMaterial.DiagramParameters, strain) * factor;
                            StressEntity stressEntity = new()
                            {
                                LimitState = limitState,
                                CalcTerm = calcTerm,
                                Material = material,
                                Strain = strain,
                                Stress = diagramValue
                            };
                            result.Add(stressEntity);
                        }
                    }
                }
            }
            return result;
        }

        private void SetLines()
        {
            var materials = MaterialsModel.SelectedItems;
            var limitStates = LimitStatesModel.SelectedItems;
            var calcTerms = CalcTermsModel.SelectedItems;
            var labels = new List<string>();
            var factor = positiveInTension ? 1d : -1d;

            double step = (maxValue - minValue) / stepCount;

            SeriesCollection = new SeriesCollection();

            foreach (var limitState in limitStates)
            {
                foreach (var calcTerm in calcTerms)
                {
                    foreach (var material in materials)
                    {
                        var loaderMaterial = material.GetLoaderMaterial(limitState.LimitState, calcTerm.CalcTerm);
                        var lineSeries = new LineSeries()
                        {
                            Title = $"{material.Name} ({calcTerm.ShortName} {limitState.ShortName})",
                            //Stroke = new SolidColorBrush(material.Color),
                            Fill = Brushes.Transparent,
                            LineSmoothness = VisualProps.LineSmoothness,
                            PointGeometry = DefaultGeometries.Circle,
                            PointGeometrySize = VisualProps.StrokeSize
                        };
                        if (limitStates.Count() == 1 && calcTerms.Count() == 1)
                        {
                            lineSeries.Stroke = new SolidColorBrush(material.Color);
                        }
                        var chartValues = new ChartValues<double>();
                        for (double s = minValue; s < maxValue; s += step)
                        {
                            //var trueStep = s * factor;
                            double diagramValue = Math.Round(loaderMaterial.Diagram.Invoke(loaderMaterial.DiagramParameters, s * factor)) * factor * UnitConstants.Stress;
                            //var point = new PointF() { X = (float)s, Y = (float)diagramValue };
                            //chartValues.Add(point);
                            chartValues.Add(diagramValue);
                            labels.Add(Convert.ToString(Math.Round(s, 4)));
                        }
                        lineSeries.Values = chartValues;
                        SeriesCollection.Add(lineSeries);
                    }
                }
            }
            Labels = labels;
        }
        private void DrawLines()
        {
            SetLines();
            OnPropertyChanged(nameof(SeriesCollection));
            OnPropertyChanged(nameof(Labels));
        }

        private bool IsDrawPossible()
        {
            if (MaterialsModel.CollectionItems.Count == 0) return false;
            if (LimitStatesModel.CollectionItems.Count == 0) return false;
            if (CalcTermsModel.CollectionItems.Count == 0) return false;
            return true;
        }
    }
}
