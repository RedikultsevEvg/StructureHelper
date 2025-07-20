using StructureHelper.Infrastructure.Enums;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.BeamShears;
using StructureHelperLogics.Models.Primitives;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace StructureHelper.Windows.BeamShears
{
    /// <summary>
    /// Interaction logic for SectionView.xaml
    /// </summary>
    public partial class SectionView : Window
    {
        private readonly SectionViewModel viewModel;
        public SectionView(SectionViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            this.viewModel.ParentWindow = this;
            this.DataContext = this.viewModel;
        }
        public SectionView(IBeamShearSection section) : this(new SectionViewModel(section))
        {
            AddPrimitiveProperties(section.Shape, ShapeStackPanel);
        }

        private void AddPrimitiveProperties(IShape shape, StackPanel stackPanel)
        {
            List<string> templateNames = new List<string>();
            if (shape is IRectangleShape) { templateNames.Add("RectangleShapeEdit"); }
            else if (shape is ICircleShape) { templateNames.Add("CircleShapeEdit"); }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(shape));
            }
                foreach (var name in templateNames)
                {
                    ContentControl contentControl = new ContentControl();
                    contentControl.SetResourceReference(ContentTemplateProperty, name);
                    Binding binding = new Binding { Source = viewModel.Shape };
                    contentControl.SetBinding(ContentProperty, binding);
                    stackPanel.Children.Add(contentControl);
                }
        }
    }
}
