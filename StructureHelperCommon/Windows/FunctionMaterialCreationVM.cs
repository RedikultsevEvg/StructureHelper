using FunctionParser;
using StructureHelperCommon.Infrastructures.Commands;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models.Functions;
using StructureHelperCommon.Models.Materials;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StructureHelperCommon.Windows
{
    public class FunctionMaterialCreationVM
    {
        public string CREATE_MATERIAL { get; } = "Create Function Material";
        public string MODULUS_OF_ELASTICITY { get; } = "Modulus of elasticity = ";
       
        private const string ERROR_TEXT_1 = "Not all material states have functions ";
        private RelayCommand createFunctionMaterialCommand;
        public ICommand CreateFunctionMaterialCommand
        {
            get => createFunctionMaterialCommand ??= new RelayCommand(o => CreateFunctionMaterial(o));
        }
        public double Modulus { get; set; } = 2e11d;
        public ObservableCollection<IOneVariableFunction> Functions { get; set; } = new ObservableCollection<IOneVariableFunction>();
        public ObservableCollection<MaterialSettings> MaterialSettingsList { get; set; } = new() { new MaterialSettings() };
        public FunctionMaterialCreationVM()
        {
            var stressStrainFunctions = new ObservableCollection<IOneVariableFunction>
                (
                ProgramSetting.Functions
                .Where(x => x.FunctionPurpose == Infrastructures.Enums.FunctionPurpose.StressStrain)
                );
            GetFunctionsFromTree(stressStrainFunctions);
        }
        private void GetFunctionsFromTree(ObservableCollection<IOneVariableFunction> stressStrainFunctions)
        {
            foreach (IOneVariableFunction func in stressStrainFunctions)
            {
                Functions.Add(func);
                if (func.Functions.Count > 0)
                {
                    GetFunctionsFromTree(func.Functions);
                }
            }
        }
        private void CreateFunctionMaterial(object parameter)
        {
        //    var window = parameter as Window;
        //    if (Func_ST_ULS == null ||
        //        Func_ST_SLS == null ||
        //        Func_ST_Special == null ||
        //        Func_LT_ULS == null ||
        //        Func_LT_SLS == null ||
        //        Func_LT_Special == null)
        //    {
        //        MessageBox.Show($"{ERROR_TEXT_1}");
        //        return;
        //    }
        //    FunctionStorage.Func_ST_ULS = Func_ST_ULS;
        //    FunctionStorage.Func_ST_SLS = Func_ST_SLS;
        //    FunctionStorage.Func_ST_Special = Func_ST_Special;
        //    FunctionStorage.Func_LT_ULS = Func_LT_ULS;
        //    FunctionStorage.Func_LT_SLS = Func_LT_SLS;
        //    FunctionStorage.Func_LT_Special = Func_LT_Special;
        //    window.DialogResult = true;
        //    window.Close();
        }

    }
}
