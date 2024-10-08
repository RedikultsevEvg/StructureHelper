using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Analyses
{
    public class VersionProcessor : IVersionProcessor
    {
        public List<IDateVersion> Versions { get; }

        public Guid Id { get; }

        public VersionProcessor(Guid id)
        {

            Id = id;
            Versions = new();
        }
        public VersionProcessor() : this (Guid.NewGuid())
        {
            
        }

        private void AddVersion(IDateVersion version)
        {
            Versions.Add(version);
        }

        public void AddVersion(ISaveable newItem)
        {
            var version = new DateVersion()
            {
                DateTime = DateTime.Now,
                AnalysisVersion = newItem
            };
            AddVersion(version);
        }


        public IDateVersion GetCurrentVersion()
        {
            return Versions[^1];
        }
    }
}
