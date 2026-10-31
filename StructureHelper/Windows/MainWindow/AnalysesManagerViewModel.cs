using StructureHelper.Infrastructure;
using StructureHelper.Windows.Points;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StructureHelper.Windows.MainWindow
{
    public class AnalysesManagerViewModel : ViewModelBase
    {
        private RelayCommand showAboutCommand;
        private RelayCommand showPointsCommand;

        public FileLogic FileLogic { get; }
        public DiagramLogic DiagramLogic { get; }
        public AnalysesLogic AnalysesLogic { get; }

        public RelayCommand ShowAboutCommand
        {
            get
            {
                return showAboutCommand ??= new RelayCommand(obj =>
                {
                    ShowAbout();
                });
            }
        }

        public RelayCommand ShowPointsCommand
        {
            get
            {
                return showPointsCommand ??= new RelayCommand(obj =>
                {
                    ShowPoints();
                });
            }
        }

        private void ShowAbout()
        {
            var wnd = new AboutView();
            wnd.ShowDialog();
        }

        private void ShowPoints()
        {
            var wnd = new PointsCollectionView();
            wnd.ShowDialog();
        }

        public AnalysesManagerViewModel()
        {
            FileLogic = new() { ParentVM = this };
            FileLogic.CreateNewFile();
            DiagramLogic = new();
            AnalysesLogic = new();
        }
    }
}
