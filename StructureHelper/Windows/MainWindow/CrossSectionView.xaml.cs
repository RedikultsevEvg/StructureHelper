using System;
using System.Windows;
using System.Windows.Controls;

using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Services;
using StructureHelper.Services.Primitives;

namespace StructureHelper.Windows.MainWindow
{
   public partial class CrossSectionView : Window
   {
      private CrossSectionViewModel viewModel;
      public IPrimitiveRepository PrimitiveRepository { get; }

      public CrossSectionView(IPrimitiveRepository primitiveRepository, CrossSectionViewModel viewModel)
      {
         PrimitiveRepository = primitiveRepository;
         this.viewModel = viewModel;
         DataContext = this.viewModel;
         viewModel.MV = this;
         ResourceDictionary dict = new ResourceDictionary();
         dict.Source = new Uri("..\\Infrastructure\\UI\\Resources\\Strings.en-US.xaml",
                             UriKind.Relative);
         this.Resources.MergedDictionaries.Add(dict);
         InitializeComponent();
      }

      private void ContentPresenter_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
      {
         var contentPresenter = sender as ContentPresenter;
         var item = contentPresenter?.Content as PrimitiveBase;
         viewModel.PrimitiveLogic.SelectedItem = item;
      }
   }
}
