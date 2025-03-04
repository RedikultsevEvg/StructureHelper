using StructureHelperCommon.Infrastructures.Commands;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models.Functions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StructureHelperCommon.Windows
{
    public class FunctionSelectionVM
    {
        private RelayCommand createFunctionMaterialCommand;
        public ICommand CreateFunctionMaterialCommand
        {
            get => createFunctionMaterialCommand ??= new RelayCommand(o => CreateFunctionMaterial(o));
        }
        public string SHORT_TERM { get; } = "Short Term";
        public string LONG_TERM { get; } = "Long Term";
        public string ULS { get; } = "Ultimate Limit State (ULS)";
        public string SLS { get; } = "Serviceability Limit State (SLS)";
        public string SPECIAL { get; } = "Special Limit State";
        public string CREATE_MATERIAL { get; } = "Create Function Material";
        private const string ERROR_TEXT_1 = "Not all material states have functions ";
        public ObservableCollection<IOneVariableFunction> Functions { get; set; } = ProgramSetting.Functions;
        public IOneVariableFunction Func_ST_ULS { get; set; }
        public IOneVariableFunction Func_ST_SLS { get; set; }
        public IOneVariableFunction Func_ST_Special { get; set; }
        public IOneVariableFunction Func_LT_ULS { get; set; }
        public IOneVariableFunction Func_LT_SLS { get; set; }
        public IOneVariableFunction Func_LT_Special { get; set; }
        public FunctionStorage FunctionStorage { get; set; } = new();
        private void CreateFunctionMaterial(object parameter)
        {
            var window = parameter as Window;
            if (Func_ST_ULS == null ||
                Func_ST_SLS == null ||
                Func_ST_Special == null ||
                Func_LT_ULS == null ||
                Func_LT_SLS == null ||
                Func_LT_Special == null)
            {
                MessageBox.Show($"{ERROR_TEXT_1}");
                return;
            }
            FunctionStorage.Func_ST_ULS = Func_ST_ULS;
            FunctionStorage.Func_ST_SLS = Func_ST_SLS;
            FunctionStorage.Func_ST_Special = Func_ST_Special;
            FunctionStorage.Func_LT_ULS = Func_LT_ULS;
            FunctionStorage.Func_LT_SLS = Func_LT_SLS;
            FunctionStorage.Func_LT_Special = Func_LT_Special;
            window.DialogResult = true;
            window.Close();
        }
    } 
}
