using StructureHelper.Infrastructure;
using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews
{
    /// <summary>
    /// Summary status bar for collection of results 
    /// </summary>
    public class ValidResultCounterVM : ViewModelBase
    {
        private IEnumerable<IResult> results;
        /// <summary>
        /// Count of valid results in collection
        /// </summary>
        public int ValidResultCount => results.Count(x => x.IsValid == true);
        /// <summary>
        /// Count of invalid results in collection
        /// </summary>
        public int InvalidResultCount => results.Count(x => x.IsValid == false);
        /// <summary>
        /// Total count of results
        /// </summary>
        public int TotalResultCount => results.Count();
        public ValidResultCounterVM(IEnumerable<IResult> results)
        {
            this.results = results;
        }

    }
}
