namespace StructureHelperCommon.Models.Shapes
{
    public interface ICenterShape
    {
        IPoint2D Center {get;}
        IShape Shape { get;}
    }
}
