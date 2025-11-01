using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Shapes
{
    public enum ArcFlatteningMode
    {
        ByAngleStep,
        BySegmentCount,
        ByMaxSegmentLength,
        Combined
    }

    public class ArcFlatteningOption : IArcFlatteningOption
    {
        public ArcFlatteningMode Mode { get; set; } = ArcFlatteningMode.Combined;
        public double AngleStepRadians { get; set; } = Math.PI / 8; // 22.5°
        public int SegmentCount { get; set; } = 2;
        public double MaxSegmentLength { get; set; } = 0.05; // (m)
    }
}
