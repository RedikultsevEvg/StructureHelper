using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace StructureHelperCommon.Models.Analyses
{
    public abstract class Analysis<T> : IAnalysis where T : ISaveable
    {
        public string Name { get; set; } = string.Empty;
        public string Tags { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public Color Color { get; set; } = Color.FromRgb(128, 0, 0);
        public IVersionProcessor VersionProcessor { get; set; } = new VersionProcessor(Guid.NewGuid());

        public Guid Id { get; }

        public Analysis(Guid id, T fstVersion)
        {
            Id = id;
            VersionProcessor.AddVersion(fstVersion);
        }

        public abstract object Clone();
    }
}
