namespace StructureHelperCommon.Infrastructures.Interfaces
{
    public interface ISaveable
    {
        int Id { get; set; }
        void Save();
    }
}
