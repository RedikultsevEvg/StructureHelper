using StructureHelperCommon.Models.Parameters;
using StructureHelperCommon.Services.ColorServices;
using StructureHelperLogics.NdmCalculations.Analyses;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.Graphs
{
    public class ExportChartToCSVLogic : ExportToCSVLogicBase
    {
        private List<Series> series;

        public ExportChartToCSVLogic(IEnumerable<Series> series)
        {
            this.series = series.ToList();
        }

        public override void ExportBoby()
        {
            foreach (var ser in series)
            {
                output.AppendLine(string.Join(separator, ser.Name));
                var arrayParameter = ser.ArrayParameter;
                output.AppendLine(string.Join(separator, arrayParameter.ColumnLabels));
                var data = arrayParameter.Data;
                int columnCount = data.GetLength(1);
                int rowCount = data.GetLength(0);
                for (int j = 0; j < rowCount; j++)
                {
                    string[] values = new string[columnCount];
                    for (int i = 0; i < columnCount; i++)
                    {
                        values[i] = data[j, i].ToString();
                    }
                    output.AppendLine(string.Join(separator, values));
                }

            }
        }

        public override void ExportHeadings()
        {
            //int headerCount = series.Sum(x => x.YItems.CollectionItems.Count);
            //string[] headings = new string[headerCount];
            //int counter = 0;
            //foreach (var ser in series)
            //{
            //    foreach (var serCollection in ser.YItems.CollectionItems)
            //    {
            //        headings[counter] = serCollection.Item.Name;
            //        counter++;
            //    }
            //}
            //output.AppendLine(string.Join(separator, headings));
        }
    }
}
