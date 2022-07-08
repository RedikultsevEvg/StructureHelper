using System.Windows;
using System.Windows.Controls;
using StructureHelper.Infrastructure.Enums;

namespace StructureHelper.Infrastructure.UI.UserControls
{
    /// <summary>
    /// Interaction logic for PrimitivePopup.xaml
    /// </summary>
    public partial class PrimitivePopup : UserControl
    {
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(PrimitiveType), typeof(PrimitivePopup));

        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(PrimitivePopup));

        private PrimitiveType type;
        private bool isOpen;
        public PrimitiveType Type
        {
            get => type;
            set
            {
                if (value != type)
                {
                    type = value;
                }
            }
        }

        public bool IsOpen
        {
            get => isOpen;
            set
            {
                if (value != isOpen)
                    isOpen = value;
            }
        }

        public PrimitivePopup()
        {
            InitializeComponent();
        }
    }
}
