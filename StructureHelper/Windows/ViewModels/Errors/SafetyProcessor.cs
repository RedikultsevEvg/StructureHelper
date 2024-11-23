using StructureHelper.Windows.Errors;
using StructureHelperCommon.Infrastructures.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.ViewModels.Errors
{
    /// <summary>
    /// Provides safety runing of some action
    /// </summary>
    internal static class SafetyProcessor
    {
        /// <summary>
        /// Invokes action and wrap it in safety try-catch block 
        /// </summary>
        /// <param name="action">Action wich will be invoked</param>
        /// <param name="shortText">Short text of error</param>
        public static void RunSafeProcess(Action action, string shortText = "")
        {
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                var vm = new ErrorProcessor()
                {
                    ShortText = shortText,
                    DetailText = $"{ex}"
                };
                new ErrorMessage(vm).ShowDialog();
            }
        }
        public static void ShowMessage(string shortText, string detailText)
        {
            var vm = new ErrorProcessor()
            {
                ShortText = shortText,
                DetailText = detailText
            };
            new ErrorMessage(vm).ShowDialog();
        }
    }
}
