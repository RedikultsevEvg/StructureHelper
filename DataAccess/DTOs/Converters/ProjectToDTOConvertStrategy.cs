using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Analyses;
using StructureHelperCommon.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class ProjectToDTOConvertStrategy : IConvertStrategy<ProjectDTO, IProject>
    {
        private IUpdateStrategy<IProject> updateStrategy;
        private IConvertStrategy<VisualAnalysisDTO, IVisualAnalysis> convertStrategy;

        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public ProjectToDTOConvertStrategy(IUpdateStrategy<IProject> updateStrategy, IConvertStrategy<VisualAnalysisDTO, IVisualAnalysis> convertStrategy)
        {
            this.updateStrategy = updateStrategy;
            this.convertStrategy = convertStrategy;
        }

        public ProjectToDTOConvertStrategy() : this(new ProjectUpdateStrategy(), new VisualAnalysisToDTOConvertStrategy())
        {
            
        }

        public ProjectDTO Convert(IProject source)
        {
            Check();
            ProjectDTO newItem = new()
            {
                Id = source.Id
            };
            updateStrategy.Update(newItem, source);
            convertStrategy.ReferenceDictionary = ReferenceDictionary;
            convertStrategy.TraceLogger = TraceLogger;
            var convertLogic = new DictionaryConvertStrategy<VisualAnalysisDTO, IVisualAnalysis>()
            {
                ReferenceDictionary = ReferenceDictionary,
                ConvertStrategy = convertStrategy,
                TraceLogger = TraceLogger
            };
            newItem.VisualAnalyses.Clear();
            foreach (var item in source.VisualAnalyses)
            {
                var newVisualAnalysis = convertLogic.Convert(item);
                newItem.VisualAnalyses.Add(newVisualAnalysis);
            }
            return newItem;
        }

        private void Check()
        {
            var checkLogic = new CheckConvertLogic<ProjectDTO, IProject>();
            checkLogic.ConvertStrategy = this;
            checkLogic.TraceLogger = TraceLogger;
            checkLogic.Check();
        }
    }
}
