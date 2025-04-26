using FunctionParser;
using StructureHelperCommon.Infrastructures.Commands;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models.Functions;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Sections.Logics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StructureHelperCommon.Windows
{
    public class FunctionMaterialCreationVM
    {
        private ObservableCollection<MaterialSettings> materialSettingsList = new()
          {
            new MaterialSettings()
            {
                Number = 1,
                IsActive = true,
                LimitState = Infrastructures.Enums.LimitStates.ULS,
                CalcTerm = Infrastructures.Enums.CalcTerms.ShortTerm,
            },
            new MaterialSettings()
            {
                Number = 2,
                IsActive = true,
                LimitState = Infrastructures.Enums.LimitStates.ULS,
                CalcTerm = Infrastructures.Enums.CalcTerms.LongTerm,
            },
            new MaterialSettings()
            {
                Number = 3,
                IsActive = true,
                LimitState = Infrastructures.Enums.LimitStates.SLS,
                CalcTerm = Infrastructures.Enums.CalcTerms.ShortTerm,
            },
            new MaterialSettings()
            {
                Number = 4,
                IsActive = true,
                LimitState = Infrastructures.Enums.LimitStates.SLS,
                CalcTerm = Infrastructures.Enums.CalcTerms.LongTerm,
            },

        };
        public string CREATE_MATERIAL { get; } = "Create Function Material";
        public string MODULUS_OF_ELASTYCITY { get; } = "Modulus =";
        public string ADD { get; } = "Add";
        public string DELETE { get; } = "Delete";
        private const string ERROR_TEXT_1 = "There are no active calculation states. Please activate at least one table row.";
        private const string ERROR_TEXT_2_1 = "Not all active material states have functions:";
        private const string ERROR_TEXT_2_2 = "please fill in functions in these rows.";
        private const string ERROR_TEXT_3_1 = "The material cannot be created because some calculation states are duplicated:";
        private const string ERROR_TEXT_3_2 = "please remove or disable some duplicated rows, each state must be unique.";
        private RelayCommand createFunctionMaterialCommand;
        private RelayCommand addCommand;
        private RelayCommand deleteCommand;
        public ICommand CreateFunctionMaterialCommand
        {
            get => createFunctionMaterialCommand ??= new RelayCommand(o => CreateFunctionMaterial(o));
        }
        public ICommand AddCommand
        {
            get => addCommand ??= new RelayCommand(o => Add());
        }
        public ICommand DeleteCommand
        {
            get => deleteCommand ??= new RelayCommand(o => Delete(o));
        }
        public double Modulus { get; set; } = 2e11d;
        public ObservableCollection<IOneVariableFunction> Functions { get; set; } = new ObservableCollection<IOneVariableFunction>();
        public ObservableCollection<MaterialSettings> MaterialSettingsList
        {
            get
            {
                return materialSettingsList;
            }
            set
            {
                materialSettingsList = value;
            }
        }
        public FunctionMaterialCreationVM()
        {
            var stressStrainFunctions = new List<IOneVariableFunction>
                (
                ProgramSetting.Functions
                .Where(x => x.FunctionPurpose == Infrastructures.Enums.FunctionPurpose.StressStrain)
                );
            GetFunctionsFromTree(stressStrainFunctions);
        }
        private void GetFunctionsFromTree(List<IOneVariableFunction> stressStrainFunctions)
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
        private void Add()
        {
            var ms = new MaterialSettings()
            {
                Number = MaterialSettingsList.Count + 1,
                IsActive = true,
                LimitState = Infrastructures.Enums.LimitStates.ULS,
                CalcTerm = Infrastructures.Enums.CalcTerms.ShortTerm,
            };
            MaterialSettingsList.Add(ms);
        }
        private void Delete(object parameter)
        {
            var dataGrid = parameter as DataGrid;
            var items = dataGrid.SelectedItems;
            var removeList = new List<MaterialSettings>();
            if (items.Count > 0)
            {
                if (items.Count == MaterialSettingsList.Count)
                {
                    var firstElement = MaterialSettingsList.First();
                    MaterialSettingsList.Clear();
                    MaterialSettingsList.Add(firstElement);
                }
                foreach (MaterialSettings item in items)
                {
                    removeList.Add(item);
                }
                foreach (MaterialSettings item in removeList)
                {
                    MaterialSettingsList.Remove(item);
                }
            }
            else
            {
                if (MaterialSettingsList.Count > 1)
                {
                    MaterialSettingsList.RemoveAt(MaterialSettingsList.Count - 1);
                }
            }
            for (int i = 0; i < MaterialSettingsList.Count; i++)
            {
                MaterialSettingsList[i].Number = i + 1;
            }
            dataGrid.Items.Refresh();
        }
        private void CreateFunctionMaterial(object parameter)
        {
            var window = parameter as Window;
            var activeMaterialSettingsList = MaterialSettingsList.Where(x => x.IsActive == true).ToList();
            if (activeMaterialSettingsList.Count() < 1)
            {
                MessageBox.Show($"{ERROR_TEXT_1}");
                return;
            }
            var emptyFunctions = activeMaterialSettingsList.Where(x => x.Function == null).ToList();
            if (emptyFunctions.Count > 0) 
            {
                var errorMessage = String.Empty;
                foreach (MaterialSettings ms in emptyFunctions)
                {
                    errorMessage += $"{ms.Number}, ";
                }
                MessageBox.Show($"{ERROR_TEXT_2_1}\n{errorMessage}\n{ERROR_TEXT_2_2}");
                return;
            }
            var duplicates = activeMaterialSettingsList
                .GroupBy(x => new { x.LimitState, x.CalcTerm })
                .Where(g => g.Count() > 1)
                .ToList();
            if (duplicates.Count > 0)
            {
                var errorMessage = String.Empty;
                for (int i = 0; i < duplicates.Count; i++)
                {
                    var group = duplicates[i];
                    var errorNumbers = String.Empty;
                    foreach (MaterialSettings ms in group)
                    {
                        errorNumbers += $"{ms.Number}, ";
                    }
                    errorMessage += $"{errorNumbers}\n"; 
                }
                MessageBox.Show($"{ERROR_TEXT_3_1}\n{errorMessage}\n{ERROR_TEXT_3_2}");
                return;
            }
            window.DialogResult = true;
            window.Close();
        }

    }
}
