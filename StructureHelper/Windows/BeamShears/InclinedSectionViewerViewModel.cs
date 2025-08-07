using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.UI.GraphicalPrimitives;
using StructureHelper.Windows.UserControls.WorkPlanes;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.Models.BeamShears;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.BeamShears
{
    public class InclinedSectionViewerViewModel : ViewModelBase
    {
        private IBeamShearSectionLogicResult sectionResult;
        private IObjectConvertStrategy<List<IGraphicalPrimitive>, IBeamShearSectionLogicResult> logic;

        public WorkPlaneRootViewModel WorkPlaneRoot { get; private set; }

        public InclinedSectionViewerViewModel(IBeamShearSectionLogicResult sectionResult)
        {
            this.sectionResult = sectionResult;
            WorkPlaneRoot = new();
            logic = new SectionResultToGraphicalPrimitivesConvertLogic();
            var primitives = logic.Convert(sectionResult);
            primitives.ForEach(primitive =>
            {
                WorkPlaneRoot.PrimitiveCollection.Primitives.Add(primitive);
            });

        }
    }
}
