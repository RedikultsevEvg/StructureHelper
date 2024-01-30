using StructureHelper.Windows.Errors;
using StructureHelperCommon.Infrastructures.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.ViewModels.Errors
{
    internal static class SafetyProcessor
    {
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
