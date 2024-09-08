using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Infrastructures
{
    public class OpenProjectResult : IResult
    {
        public bool IsValid { get; set; } = true;
        public string? Description { get; set; } = string.Empty;
        public IProject Project { get; set; }
    }
}
