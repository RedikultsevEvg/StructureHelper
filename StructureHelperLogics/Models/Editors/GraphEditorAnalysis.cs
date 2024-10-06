using StructureHelperCommon.Models.Analyses;
using StructureHelperLogic.Models.Analyses;
using StructureHelperLogics.Models.Analyses;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.Models.Graphs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Editors
{
    public class GraphEditorAnalysis : IAnalysis
    {
        public Guid Id { get; private set; }
        public string Name { get; set; }
        public string Tags { get; set; }
        public IVersionProcessor VersionProcessor { get; private set; }
        public GraphEditorAnalysis(Guid Id, IVersionProcessor versionProcessor)
        {
            this.Id = Id;
            VersionProcessor = versionProcessor;
        }
        public GraphEditorAnalysis() : this(Guid.NewGuid(), new VersionProcessor())
        {
            Graph graph = new Graph();
            VersionProcessor.AddVersion(graph);
        }
        public object Clone()
        {
            GraphEditorAnalysis newAnalysis = new();
            //updateStrategy.Update(newAnalysis, this);
            return newAnalysis;
        }
    }
}
