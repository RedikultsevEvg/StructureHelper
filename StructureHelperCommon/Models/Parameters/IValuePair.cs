namespace StructureHelperCommon.Models.Parameters
{
    /// <summary>
    /// Represent pair of value with text
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IValuePair<T>
    {
        string Text { get; set; }
        T Value { get; set; }
    }
}