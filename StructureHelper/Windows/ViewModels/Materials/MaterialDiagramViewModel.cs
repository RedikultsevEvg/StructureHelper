using LiveCharts;
using LiveCharts.Wpf;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace StructureHelper.Windows.ViewModels.Materials
{
    public class MaterialDiagramViewModel
    {
        private IHeadMaterial material;
        public string MaterialName => material.Name; 
        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public MaterialDiagramViewModel(IHeadMaterial material)
        {
            this.material = material;
            SetLines();
        }

        private void SetLines()
        {
            var titles = new List<string>() { "ULS Short Term", "ULS Long Term", "SLS Short Term","SLS Long Term"};
            var limitStates = new List<LimitStates> { LimitStates.ULS, LimitStates.SLS };
            var calcTerms = new List<CalcTerms> { CalcTerms.ShortTerm, CalcTerms.LongTerm };
            var labels = new List<string>();

            double minValue = -0.005d;
            double maxValue = 0.005d;
            if (material.HelperMaterial is IConcreteLibMaterial)
            {
                maxValue = 0.0005d;
            }
            int stepCount = 100;
            double step = (maxValue - minValue) / stepCount;

            SeriesCollection = new SeriesCollection();

            for (int i = 0; i < limitStates.Count(); i++)
            {
                for (int j = 0; j < calcTerms.Count(); j++)
                {
                    int n = i * 2 + j;
                    var line = new LineSeries() { Title = titles[n], PointGeometry = null, Fill = Brushes.Transparent};
                    var chartValues = new ChartValues<double>();

                    var loaderMaterial = material.GetLoaderMaterial(limitStates[i], calcTerms[j]);
                    var title = titles[n];
                    for (double s = minValue; s < maxValue; s += step)
                    {
                        double diagramValue = loaderMaterial.Diagram.Invoke(loaderMaterial.DiagramParameters,s);
                        chartValues.Add(diagramValue);
                        labels.Add(Convert.ToString(Math.Round(s,4)));
                    }
                    line.Values = chartValues;
                    SeriesCollection.Add(line);
                }
            }
            Labels = labels;
        }
    }
}
