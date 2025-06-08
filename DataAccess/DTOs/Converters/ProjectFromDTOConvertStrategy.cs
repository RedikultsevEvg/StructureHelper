using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Analyses;
using StructureHelperCommon.Models.Projects;

namespace DataAccess.DTOs
{
    public class ProjectFromDTOConvertStrategy : ConvertStrategy<Project, ProjectDTO>
    {
        private IUpdateStrategy<IProject> _updateStrategy;
        private IConvertStrategy<IVisualAnalysis, IVisualAnalysis> _convertLogic;

        public ProjectFromDTOConvertStrategy(IUpdateStrategy<IProject> updateStrategy,
            IConvertStrategy<IVisualAnalysis, IVisualAnalysis> convertLogic,
            Dictionary<(Guid id, Type type), ISaveable> refDictinary,
            IShiftTraceLogger? traceLogger)
        {
            _updateStrategy = updateStrategy;
            _convertLogic = convertLogic;
            ReferenceDictionary = refDictinary;
            TraceLogger = traceLogger;
        }

        public ProjectFromDTOConvertStrategy(
            Dictionary<(Guid id, Type type), ISaveable> refDictinary,
            IShiftTraceLogger? traceLogger)
            : this(
                new ProjectUpdateStrategy(),
                new DictionaryConvertStrategy<IVisualAnalysis, IVisualAnalysis>(
                    refDictinary,
                    new VisualAnaysisFromDTOConvertStrategy(refDictinary, traceLogger),
                    traceLogger),
                refDictinary,
                traceLogger)
        {     }

        public override Project GetNewItem(ProjectDTO source)
{
            TraceLogger?.AddMessage("Converting of project has been started");
            Project newItem = new(source.Id);
            List<IVisualAnalysis> analyses = GetAnalyses(source, newItem);
            newItem.VisualAnalyses.Clear();
            newItem.VisualAnalyses.AddRange(analyses);
            if (newItem.VisualAnalyses.Any())
            {
                TraceLogger?.AddMessage($"Totaly {newItem.VisualAnalyses.Count} were(was) obtained");
            }
            else
            {
                TraceLogger?.AddMessage($"Project does not have any analyses, it is possible to work with project", TraceLogStatuses.Warning);
            }
            TraceLogger?.AddMessage("Converting of project has been finished successfully");
            return newItem;
        }

        public List<IVisualAnalysis> GetAnalyses(ProjectDTO source, Project newItem)
        {
            _updateStrategy.Update(newItem, source);
            TraceLogger?.AddMessage($"Source project has {source.VisualAnalyses.Count} analyses");
            List<IVisualAnalysis> analyses = new();
            foreach (var item in source.VisualAnalyses)
            {
                var visualAnalysis = _convertLogic.Convert(item);
                analyses.Add(visualAnalysis);
            }
            return analyses;
        }
    }
}
