namespace StructureHelperCommon.Models.Shapes
{
    public interface ILineShape : IShape
    {
        IPoint2D StartPoint { get; set; }
        IPoint2D EndPoint { get; set; }
        double Thickness { get; set; }
    }
}
