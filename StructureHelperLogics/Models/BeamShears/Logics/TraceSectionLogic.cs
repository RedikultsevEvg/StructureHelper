using StructureHelper.Models.Materials;
using StructureHelperCommon.Models;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    public class TraceSectionLogic : ITraceSectionLogic
    {
        public TraceSectionLogic(IShiftTraceLogger? traceLogger)
        {
            TraceLogger = traceLogger;
        }

        public IShiftTraceLogger? TraceLogger { get; set; }

        public void TraceSection(IBeamShearSection section)
        {
            List<IHeadMaterial> headMaterials = new()
                {
                    new HeadMaterial(Guid.Empty)
                    {
                        Name = $"{section.Name}.Concrete",
                        HelperMaterial = section.ConcreteMaterial
                    },
                    new HeadMaterial(Guid.Empty)
                    {
                        Name = $"{section.Name}.Reinforcement",
                        HelperMaterial = section.ReinforcementMaterial
                    },
                };
            var traceLogic = new TraceMaterialsFactory()
            {
                Collection = headMaterials
            };
            traceLogic.AddEntriesToTraceLogger(TraceLogger);
        }
    }
}
