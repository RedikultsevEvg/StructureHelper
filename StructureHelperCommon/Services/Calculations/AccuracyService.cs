using LoaderCalculator;
using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Services.Calculations
{
    public static class AccuracyService
    {
        public static void CopyProperties(IAccuracy source, IAccuracy target)
        {
            target.IterationAccuracy = source.IterationAccuracy;
            target.MaxIterationCount = source.MaxIterationCount;
        }
    }
}
