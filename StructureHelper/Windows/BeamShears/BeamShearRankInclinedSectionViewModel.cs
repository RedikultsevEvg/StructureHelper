using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.UI.GraphicalPrimitives;
using StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews;
using StructureHelper.Windows.UserControls.WorkPlanes;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.Models.BeamShears;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace StructureHelper.Windows.BeamShears
{
    public class BeamShearRankInclinedSectionViewModel : ViewModelBase
    {
        private IBeamShearActionResult actionResult;

        private IObjectConvertStrategy<List<IGraphicalPrimitive>, IBeamShearSectionLogicResult> sectionLogic;

        private IObjectConvertStrategy<List<IGraphicalPrimitive>, IBeamShearActionResult> actionLogic;
        private RelayCommand rebuildCommand;
        private WorkPlaneRootViewModel workPlaneRoot = new();

        public ICommand RebuildCommand => rebuildCommand ??= new RelayCommand(o => Rebuild());

        private void Rebuild()
        {
            var primitives = actionLogic.Convert(actionResult);
            primitives.ForEach(primitive =>
            {
                WorkPlaneRoot.PrimitiveCollection.Primitives.Add(primitive);
            });
        }

        public ObservableCollection<IGraphicalPrimitive> MainPrimitives { get; } = [];
        public WorkPlaneRootViewModel WorkPlaneRoot
        {
            get => workPlaneRoot;
            set
            {
                workPlaneRoot = value;
                OnPropertyChanged(nameof(WorkPlaneRoot));
            }
        }
        public ContourColorMap ContourColorMap { get; set; }

        public BeamShearRankInclinedSectionViewModel(IBeamShearActionResult actionResult)
        {
            this.actionResult = actionResult;
            ContourColorMap = new ContourColorMap();
            ContourColorMap.Changed += OnColorMapChanged;
            Process();
        }

        private void OnColorMapChanged(object? sender, EventArgs e)
        {
            Process();
        }

        private void Process()
        {
            if (actionResult.SectionResults.Count == 0)
            {
                return;
            }
            AddMainPrimitives();
            AjustColorMap();
            Rebuild();
        }

        private void AjustColorMap()
        {
            ContourColorMap.MinValue = actionResult.SectionResults.Min(x => x.FactorOfUsing);
            ContourColorMap.MaxValue = actionResult.SectionResults.Max(x => x.FactorOfUsing);
            actionLogic = new ActionResultToGraphicalPrimitivesConvertLogic()
            {
                ColorMap = ContourColorMap.ColorMap.SelectedColorMap,
                ValueColorRanges = ContourColorMap.ContourRange.ContourLegend.ValueColorRanges,
                MinValue = ContourColorMap.MinValue,
                MaxValue = ContourColorMap.MaxValue,
            };
        }

        private void AddMainPrimitives()
        {
            WorkPlaneRoot.PrimitiveCollection.Primitives.Clear();
            MainPrimitives.Clear();
            var sectionResult = actionResult.SectionResults.OrderBy(x => x.FactorOfUsing).ToList()[^1];
            sectionLogic = new SectionResultToGraphicalPrimitivesConvertLogic();
            var sectionPrimitives = sectionLogic.Convert(sectionResult);
            sectionPrimitives.ForEach(primitive =>
            {
                MainPrimitives.Add(primitive);
                if (primitive is InclinedSectionPrimitive inclinedSection)
                {
                    inclinedSection.VisualProperty.ZIndex = 1;
                }
                WorkPlaneRoot.PrimitiveCollection.Primitives.Add(primitive);
            });
        }
    }
}
