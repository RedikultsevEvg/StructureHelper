using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models.Functions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Windows
{
    public class FunctionSelectionVM
    {
        public string SHORT_TERM { get; } = "Short Term";
        public string LONG_TERM { get; } = "Long Term";
        public string ULS { get; } = "Ultimate Limit State (ULS)";
        public string SLS { get; } = "Serviceability Limit State (SLS)";
        public string SPECIAL { get; } = "Special Limit State";
        public string CREATE_MATERIAL { get; } = "Create Function Material";
        public ObservableCollection<IOneVariableFunction> Functions { get; set; } = ProgramSetting.Functions;
    } 
}
