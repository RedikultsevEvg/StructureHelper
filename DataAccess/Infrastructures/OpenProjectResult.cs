using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Projects;

namespace DataAccess.Infrastructures
{
    public class OpenProjectResult : IResult
    {
        public bool IsValid { get; set; } = true;
        public string? Description { get; set; } = string.Empty;
        public IProject Project { get; set; }
    }
}
