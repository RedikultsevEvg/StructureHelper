using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Analyses;
using StructureHelperCommon.Models.Projects;

namespace DataAccess.DTOs
{
    public class ProjectToDTOConvertStrategy : ConvertStrategy<ProjectDTO, IProject>
    {
        private IUpdateStrategy<IProject> updateStrategy;
        private IConvertStrategy<VisualAnalysisDTO, IVisualAnalysis> convertLogic;


        public ProjectToDTOConvertStrategy()
        {
            
        }

        public ProjectToDTOConvertStrategy(
            IUpdateStrategy<IProject> updateStrategy,
            IConvertStrategy<VisualAnalysisDTO, IVisualAnalysis> convertLogic)
        {
            this.updateStrategy = updateStrategy;
            this.convertLogic = convertLogic;
        }

        public override ProjectDTO GetNewItem(IProject source)
        {
            InitializeStrategies();
            ProjectDTO newItem = new(source.Id);
            updateStrategy.Update(newItem, source);
            newItem.VisualAnalyses.Clear();
            foreach (var item in source.VisualAnalyses)
            {
                var newVisualAnalysis = convertLogic.Convert(item);
                newItem.VisualAnalyses.Add(newVisualAnalysis);
            }
            return newItem;
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new ProjectUpdateStrategy();
            convertLogic ??= new DictionaryConvertStrategy<VisualAnalysisDTO, IVisualAnalysis>
                (this, new VisualAnalysisToDTOConvertStrategy(this));
        }
    }
}
