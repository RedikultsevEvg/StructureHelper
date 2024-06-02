namespace StructureHelperCommon.Models.Parameters
{
    public interface IProcessValuePairLogic<T>
    {
        ValuePair<T> GetValuePairByString(string s);
    }
}