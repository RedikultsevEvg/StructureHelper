namespace StructureHelperCommon.Models.Shapes
{
    public interface ICenterShape
    {
        IPoint2D Center {get;}
        double AngleRadians { get; }
        IShape Shape { get;}
    }
}
