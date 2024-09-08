using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Analyses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Projects
{
    public class Project : IProject
    {
        public Guid Id { get; }
        public string FullFileName { get; set; } = string.Empty;
        public bool IsActual { get; set; } = true;
        public List<IVisualAnalysis> VisualAnalyses { get; } = new();
        public bool IsNewFile { get; set; } = true;
        public string FileName => Path.GetFileName(FullFileName);

        public Project(Guid id)
        {
            Id = id;
        }

        public Project() : this(Guid.NewGuid())
        {
            
        }
    }
}
