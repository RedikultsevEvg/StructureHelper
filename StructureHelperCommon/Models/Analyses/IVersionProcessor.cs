using System.Collections.Generic;

namespace StructureHelperCommon.Models.Analyses
{
    public interface IVersionProcessor : ISaveable
    {
        void AddVersion(ISaveable newItem);
        List<IDateVersion> Versions { get; }
        IDateVersion GetCurrentVersion();
    }
}
