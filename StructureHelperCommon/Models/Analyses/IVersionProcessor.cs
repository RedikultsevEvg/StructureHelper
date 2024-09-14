using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Analyses
{
    public interface IVersionProcessor : ISaveable
    {
        void AddVersion(ISaveable newItem);
        List<IDateVersion> Versions { get; }
        IDateVersion GetCurrentVersion();
    }
}
