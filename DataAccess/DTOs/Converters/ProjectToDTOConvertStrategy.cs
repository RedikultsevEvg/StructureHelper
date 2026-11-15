using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Analyses;
using StructureHelperCommon.Models.Projects;

namespace DataAccess.DTOs
{
    public class ProjectToDTOConvertStrategy : ConvertStrategy<ProjectDTO, IProject>
    {
        private IUpdateStrategy<IProject> updateStrategy;
        private IUpdateStrategy<IProject> UpdateStrategy => updateStrategy ??= new ProjectUpdateStrategy() { UpdateChildren = false};
        private IConvertStrategy<VisualAnalysisDTO, IVisualAnalysis> convertLogic;
        private IConvertStrategy<VisualAnalysisDTO, IVisualAnalysis> ConvertLogic => convertLogic ??= new DictionaryConvertStrategy<VisualAnalysisDTO, IVisualAnalysis>
                (this, new VisualAnalysisToDTOConvertStrategy(this));


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
            ProjectDTO newItem = new(source.Id);
            UpdateStrategy.Update(newItem, source);
            newItem.VisualAnalyses.Clear();
            foreach (var item in source.VisualAnalyses)
            {
                var newVisualAnalysis = ConvertLogic.Convert(item);
                newItem.VisualAnalyses.Add(newVisualAnalysis);
            }
            return newItem;
        }
    }
}
