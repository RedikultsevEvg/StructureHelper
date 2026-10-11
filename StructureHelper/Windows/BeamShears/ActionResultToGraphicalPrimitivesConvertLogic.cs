using FieldVisualizer.Entities.ColorMaps;
using FieldVisualizer.Entities.Values;
using FieldVisualizer.Services.ColorServices;
using StructureHelper.Infrastructure.UI.GraphicalPrimitives;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.BeamShears;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace StructureHelper.Windows.BeamShears
{
    internal class ActionResultToGraphicalPrimitivesConvertLogic : IObjectConvertStrategy<List<IGraphicalPrimitive>, IBeamShearActionResult>
    {
        public int LineListShorteningStep { get; set; } = 1;
        public IColorMap ColorMap { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public ObservableCollection<IValueColorRange>? ValueColorRanges { get; internal set; }

        public List<IGraphicalPrimitive> Convert(IBeamShearActionResult source)
        {
            List<IGraphicalPrimitive> graphicalPrimitives = [];
            var sortedList = source.SectionResults
                .OrderBy(x => x.FactorOfUsing)
                .ToList();
            var shortList = sortedList
                .Where((item, index) =>
                index % LineListShorteningStep == 0 || index == sortedList.Count - 10)
                .ToList();
            ValueRange range = new ValueRange()
            {
                BottomValue = MinValue,
                TopValue = MaxValue
            };
            foreach (var item in shortList)
            {
                InclinedCrackPrimitive inclinedCrackPrimitive = new(item);
                inclinedCrackPrimitive.VisualProperty.Color = ColorOperations.GetColorByValue(ValueColorRanges, range, ColorMap, item.FactorOfUsing);
                graphicalPrimitives.Add(inclinedCrackPrimitive);
            }
            return graphicalPrimitives;
        }
    }
}
