using StructureHelper.Models.Materials;
using StructureHelper.Windows.ViewModels.Materials;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Materials;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StructureHelper.Windows.MainWindow.Materials
{
    /// <summary>
    /// Логика взаимодействия для HeadMaterialView.xaml
    /// </summary>
    public partial class HeadMaterialView : Window
    {
        private readonly IHeadMaterial headMaterial;
        private readonly HeadMaterialViewModel viewModel;
        private readonly Dictionary<string, Binding> bindings = new();
        private readonly IHelperMaterial helperMaterial;
        string templateName;

        public HeadMaterialView(HeadMaterialViewModel viewModel)
        {
            this.viewModel = viewModel;
        }


        public HeadMaterialView(IHeadMaterial headMaterial)
        {
            InitializeComponent();
            this.headMaterial = headMaterial;
            helperMaterial = this.headMaterial.HelperMaterial;
            viewModel = new HeadMaterialViewModel(this.headMaterial)
            {
                ParentWindow = this
            };
            DataContext = viewModel;
            AddDataTemplates();
        }

        private void AddDataTemplates()
        {
            StpMaterialProperties.Children.Clear();
            GetByndingsByMaterial();
            SetContentControls();
        }

        private void GetByndingsByMaterial()
        {
            if (helperMaterial is IConcreteLibMaterial)
            {
                SetConcreteLibraryMaterial();
            }
            else if (helperMaterial is IReinforcementLibMaterial)
            {
                SetReinforcementLibraryMaterial();
            }
            else if (helperMaterial is IElasticMaterial)
            {
                SetElasticMaterial();
            }
            else
            {
                string errorString = ErrorStrings.ObjectTypeIsUnknown + $". Expected: {typeof(IHelperMaterial)}, but was: {helperMaterial.GetType()}";
                throw new StructureHelperException(errorString);
            }
        }

        private void SetContentControls()
        {
            foreach (var item in bindings)
            {
                ContentControl contentControl = new();
                contentControl.SetResourceReference(ContentTemplateProperty, item.Key);
                contentControl.SetBinding(ContentProperty, item.Value);
                StpMaterialProperties.Children.Add(contentControl);
            }
        }
        private void SetElasticMaterial()
        {
            templateName = "ElasticMaterial";
            var binding = new Binding();
            binding.Source = viewModel.HelperMaterialViewModel;
            bindings.Add(templateName, binding);
            if (helperMaterial is IFRMaterial)
            {
                templateName = "CarbonProperties";
                var carbonBinding = new Binding();
                carbonBinding.Source = viewModel.HelperMaterialViewModel as FRViewModel;
                bindings.Add(templateName, carbonBinding);
            }
            templateName = "DirectSafetyFactors";
            var frBinding = new Binding();
            frBinding.Source = (viewModel.HelperMaterialViewModel as ElasticViewModel).SafetyFactors;
            bindings.Add(templateName, frBinding);
        }
        private void SetReinforcementLibraryMaterial()
        {
            templateName = "ReinforcementMaterial";
            var binding = new Binding
            {
                Source = viewModel.HelperMaterialViewModel
            };
            bindings.Add(templateName, binding);
        }
        private void SetConcreteLibraryMaterial()
        {
            templateName = "ConcreteMaterial";
            var binding = new Binding();
            binding.Source = viewModel.HelperMaterialViewModel;
            bindings.Add(templateName, binding);
        }
    }
}
