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
        public List<IVersion> Versions { get; }

        public Guid Id { get; }

        public VersionProcessor(Guid id)
        {

            Id = id;
            Versions = new();
        }
        public VersionProcessor() : this (new Guid())
        {
            
        }

        private void AddVersion(IVersion version)
        {
            Versions.Add(version);
        }

        public void AddVersion(ISaveable newItem)
        {
            var version = new Version()
            {
                DateTime = DateTime.Now,
                Item = newItem
            };
            AddVersion(version);
        }


        public IVersion GetCurrentVersion()
        {
            return Versions[^1];
        }
    }
}
