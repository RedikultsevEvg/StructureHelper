using System.Windows;

namespace StructureHelper.Windows.Points
{
    /// <summary>
    /// Window for editing a collection of 2D points.
    /// </summary>
    public partial class PointsCollectionView : Window
    {
        public PointsCollectionView()
        {
            InitializeComponent();
            DataContext = new PointsCollectionViewModel();
        }
    }
}
