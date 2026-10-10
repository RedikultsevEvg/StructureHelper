using FieldVisualizer.Entities.ColorMaps;
using FieldVisualizer.Entities.ColorMaps.Factories;
using FieldVisualizer.Entities.Values;
using FieldVisualizer.Entities.Values.Primitives;
using FieldVisualizer.Services.ColorServices;
using StructureHelper.Infrastructure.UI.GraphicalPrimitives;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.BeamShears;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StructureHelper.Windows.BeamShears
{
    internal class ActionResultToGraphicalPrimitivesConvertLogic : IObjectConvertStrategy<List<IGraphicalPrimitive>, IBeamShearActionResult>
    {
        private IObjectConvertStrategy<List<IGraphicalPrimitive>, IBeamShearAction> actionLogic;
        private IObjectConvertStrategy<List<IGraphicalPrimitive>, IStirrup> stirrupLogic;
        private IInclinedSection firstInclinedSection;
        public int LineListShorteningStep { get; set; } = 1;

        public List<IGraphicalPrimitive> Convert(IBeamShearActionResult source)
        {
            List<IGraphicalPrimitive> graphicalPrimitives = [];
            if (source.SectionResults.Count == 0) { return graphicalPrimitives; }
            var firstSectionResult = source.SectionResults[0];
            firstInclinedSection = firstSectionResult.ResultInputData.InclinedSection;
            InitializeStrategies();
            BeamShearSectionPrimitive beamShearSectionPrimitive = new(firstInclinedSection.BeamShearSection, firstInclinedSection);
            graphicalPrimitives.Add(beamShearSectionPrimitive);
            graphicalPrimitives.AddRange(actionLogic.Convert(firstSectionResult.ResultInputData.BeamShearAction));
            graphicalPrimitives.AddRange(stirrupLogic.Convert(firstSectionResult.ResultInputData.Stirrup));
            var sortedList = source.SectionResults
                .OrderBy(x => x.FactorOfUsing)
                .ToList();
            var shortList = sortedList
                .Where((item, index) =>
                index % LineListShorteningStep == 0 || index == sortedList.Count - 1)
                .ToList();
            IColorMap colorMap = ColorMapFactory.GetColorMap(ColorMapsTypes.LiraSpectrum);
            ValueRange range = new ValueRange() { BottomValue = sortedList.Min(x => x.FactorOfUsing), TopValue = sortedList.Max(x => x.FactorOfUsing)};
            foreach (var item in shortList)
            {
                InclinedSectionPrimitive inclinedSectionPrimitive = new(item);
                inclinedSectionPrimitive.VisualProperty.IsVisible = false;
                inclinedSectionPrimitive.VisualProperty.Color = ColorOperations.GetColorByValue(range, colorMap, item.FactorOfUsing);
                graphicalPrimitives.Add(inclinedSectionPrimitive);
            }
            return graphicalPrimitives;
        }

        private void InitializeStrategies()
        {
            stirrupLogic ??= new StirrupToGraphicPrimitiveConvertLogic(firstInclinedSection);
            actionLogic ??= new ActionToGraphicPrimitiveConvertLogic(firstInclinedSection);
        }
    }
}
