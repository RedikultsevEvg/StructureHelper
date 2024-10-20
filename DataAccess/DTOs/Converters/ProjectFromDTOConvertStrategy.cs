using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Analyses;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class ProjectFromDTOConvertStrategy : IConvertStrategy<Project, ProjectDTO>
    {
        private IUpdateStrategy<IProject> updateStrategy;
        private IConvertStrategy<IVisualAnalysis, IVisualAnalysis> visualAnalysisConvertStrategy = new VisualAnaysisFromDTOConvertStrategy();

        public ProjectFromDTOConvertStrategy(IUpdateStrategy<IProject> updateStrategy)
        {
            this.updateStrategy = updateStrategy;
        }

        public ProjectFromDTOConvertStrategy() : this (new ProjectUpdateStrategy())
        {
            
        }

        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public Project Convert(ProjectDTO source)
        {
            Check();
            TraceLogger?.AddMessage("Converting project is started", TraceLogStatuses.Info);
            try
            {
                Project newItem = GetProject(source);
                return newItem;
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Error);
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
                throw;
            }
        }

        private Project GetProject(ProjectDTO source)
        {
            Project newItem = new();
            updateStrategy.Update(newItem, source);
            visualAnalysisConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            visualAnalysisConvertStrategy.TraceLogger = TraceLogger;
            var convertLogic = new DictionaryConvertStrategy<IVisualAnalysis, IVisualAnalysis>(this, visualAnalysisConvertStrategy);
            newItem.VisualAnalyses.Clear();
            foreach (var item in source.VisualAnalyses)
            {
                var visualAnalysis = convertLogic.Convert(item);
                newItem.VisualAnalyses.Add(visualAnalysis);
            }
            TraceLogger?.AddMessage("Converting project has completed succesfully", TraceLogStatuses.Info);
            return newItem;
        }

        private void Check()
        {
            var checkLogic = new CheckConvertLogic<Project, ProjectDTO>(this);
            checkLogic.Check();
        }
    }
}
