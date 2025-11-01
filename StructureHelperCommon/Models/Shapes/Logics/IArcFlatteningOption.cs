namespace StructureHelperCommon.Models.Shapes
{
    public interface IArcFlatteningOption
    {
        double AngleStepRadians { get; set; }
        double MaxSegmentLength { get; set; }
        ArcFlatteningMode Mode { get; set; }
        int SegmentCount { get; set; }
    }
}