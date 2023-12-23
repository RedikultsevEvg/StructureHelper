using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Calculators
{
    public class FindParameterResult : IResult, IiterationResult
    {
        public bool IsValid { get; set; }
        public string Description { get; set; }
        public double Parameter { get; set; }
        public int IterationNumber { get; set; }
        public double CurrentAccuracy { get; set; }
        public int MaxIterationCount { get; set; }
    }
}
