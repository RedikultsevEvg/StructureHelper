namespace StructureHelperCommon.Models.FeaMaterials
{
    public interface IAbaqusScript
    {
        AbaqusScript Add(IAbaqusScriptBlock block);
        string Build();
    }
}