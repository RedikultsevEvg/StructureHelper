using System.Windows;

namespace StructureHelper.Windows.LoadHistory
{
    /// <summary>
    /// Логика взаимодействия для ConstructionStageView.xaml
    /// </summary>
    public partial class ConstructionStageView : Window
    {
        private ConstructionStageViewModel vm;

        public ConstructionStageView(ConstructionStageViewModel vm)
        {
            InitializeComponent();
            this.vm = vm;
            vm.ParentWindow = this;
            this.DataContext = vm;
        }
    }
}
