namespace StructureHelperCommon.Infrastructures.Interfaces
{
    public interface IHasParent
    {
        object Parent { get; }
        void RegisterParent();
    }
}
