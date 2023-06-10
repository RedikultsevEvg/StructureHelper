using StructureHelper.Models.Materials;
using StructureHelper.Windows.ViewModels.Materials;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Strings;
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
        IHeadMaterial headMaterial;
        HeadMaterialViewModel vm;

        public HeadMaterialView(IHeadMaterial headMaterial)
        {
            InitializeComponent();
            this.headMaterial = headMaterial;
            vm = new HeadMaterialViewModel(this.headMaterial);
            DataContext = vm;
            AddDataTemplates();
        }

        private void AddDataTemplates()
        {
            StpMaterialProperties.Children.Clear();
            var bindings = new Dictionary<string, Binding>();
            var helperMaterial = headMaterial.HelperMaterial;
            string templateName;
            if (helperMaterial is IConcreteLibMaterial)
            {
                templateName = "ConcreteMaterial";
                var binding = new Binding();
                binding.Source = vm.HelperMaterialViewModel;
                bindings.Add(templateName, binding);
            }
            else if (helperMaterial is IReinforcementLibMaterial)
            {
                templateName = "ReinforcementMaterial";
                var binding = new Binding();
                binding.Source = vm.HelperMaterialViewModel;
                bindings.Add(templateName, binding);
            }
            else if (helperMaterial is IElasticMaterial)
            {
                templateName = "ElasticMaterial";
                var binding = new Binding();
                binding.Source = vm.HelperMaterialViewModel;
                bindings.Add(templateName, binding);
                templateName = "DirectSafetyFactors";
                var frBinding = new Binding();
                frBinding.Source = (vm.HelperMaterialViewModel as ElasticViewModel).SafetyFactors;
                bindings.Add(templateName, frBinding);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown + $". Expected: {typeof(IHelperMaterial)}, but was: {helperMaterial.GetType()}");
            }
                
            foreach (var item in bindings)
            {
                ContentControl contentControl = new ContentControl();
                contentControl.SetResourceReference(ContentTemplateProperty, item.Key);
                contentControl.SetBinding(ContentProperty, item.Value);
                StpMaterialProperties.Children.Add(contentControl);
            }
        }
    }
}
