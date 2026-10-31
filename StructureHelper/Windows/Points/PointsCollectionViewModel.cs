using StructureHelper.Infrastructure;
using StructureHelper.Services.Points;
using StructureHelper.Windows.Graphs;
using StructureHelperCommon.Models.Parameters;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services.FileServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace StructureHelper.Windows.Points
{
    /// <summary>
    /// View model for editing a collection of 2D points, importing/exporting them and showing them as a chart.
    /// </summary>
    public class PointsCollectionViewModel : ViewModelBase
    {
        private readonly PointsExcelService excelService;
        private Point2D? selectedPoint;
        private RelayCommand? addPointCommand;
        private RelayCommand? deletePointCommand;
        private RelayCommand? copyToClipboardCommand;
        private RelayCommand? pasteFromClipboardCommand;
        private RelayCommand? importFromExcelCommand;
        private RelayCommand? exportToExcelCommand;
        private RelayCommand? showChartCommand;

        public PointsCollectionViewModel() : this(new PointsExcelService())
        {
        }

        public PointsCollectionViewModel(PointsExcelService excelService)
        {
            this.excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            Points.CollectionChanged += (_, _) =>
            {
                OnPropertyChanged(nameof(PointsCount));
                CommandManager.InvalidateRequerySuggested();
            };
        }

        /// <summary>
        /// Points of the collection. Coordinates are stored in metres and displayed in millimetres.
        /// </summary>
        public ObservableCollection<Point2D> Points { get; } = [];

        public int PointsCount => Points.Count;

        public Point2D? SelectedPoint
        {
            get => selectedPoint;
            set
            {
                selectedPoint = value;
                OnPropertyChanged(nameof(SelectedPoint));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ICommand AddPointCommand => addPointCommand ??= new RelayCommand(o => AddPoint());

        public ICommand DeletePointCommand => deletePointCommand ??= new RelayCommand(
            o => DeletePoint(),
            o => SelectedPoint is not null);

        public ICommand CopyToClipboardCommand => copyToClipboardCommand ??= new RelayCommand(
            o => CopyToClipboard(),
            o => Points.Count > 0);

        public ICommand PasteFromClipboardCommand => pasteFromClipboardCommand ??= new RelayCommand(o => PasteFromClipboard());

        public ICommand ImportFromExcelCommand => importFromExcelCommand ??= new RelayCommand(o => ImportFromExcel());

        public ICommand ExportToExcelCommand => exportToExcelCommand ??= new RelayCommand(
            o => ExportToExcel(),
            o => Points.Count > 0);

        public ICommand ShowChartCommand => showChartCommand ??= new RelayCommand(
            o => ShowChart(),
            o => Points.Count > 0);

        public void AddPoint()
        {
            var point = new Point2D();
            Points.Add(point);
            SelectedPoint = point;
        }

        public void DeletePoint()
        {
            if (SelectedPoint is null)
            {
                return;
            }

            int index = Points.IndexOf(SelectedPoint);
            if (index < 0)
            {
                SelectedPoint = null;
                return;
            }

            Points.RemoveAt(index);
            SelectedPoint = Points.Count == 0 ? null : Points[Math.Min(index, Points.Count - 1)];
        }

        public void CopyToClipboard()
        {
            IEnumerable<IPoint2D> points = SelectedPoint is null
                ? Points
                : new IPoint2D[] { SelectedPoint };

            try
            {
                Clipboard.SetText(PointTextSerializer.Serialize(points));
            }
            catch (Exception ex)
            {
                ShowError("Points cannot be copied to the clipboard", ex);
            }
        }

        public void PasteFromClipboard()
        {
            string text;
            try
            {
                if (Clipboard.ContainsText() == false)
                {
                    ShowInfo("The clipboard does not contain text.");
                    return;
                }
                text = Clipboard.GetText();
            }
            catch (Exception ex)
            {
                ShowError("The clipboard cannot be read", ex);
                return;
            }

            List<Point2D> importedPoints = PointTextSerializer.Deserialize(text);
            if (importedPoints.Count == 0)
            {
                ShowInfo("The clipboard does not contain valid points. Expected two numbers per line: X and Y in millimetres.");
                return;
            }

            AddPoints(importedPoints);
        }

        public void ImportFromExcel()
        {
            var inputData = new OpenFileInputData
            {
                FilterString = "Excel workbook (*.xlsx)|*.xlsx|Excel 97-2003 workbook (*.xls)|*.xls|All files (*.*)|*.*",
                Title = "Import points from Excel"
            };
            OpenFileResult openResult = new FileOpener(inputData).OpenFile();
            if (openResult.IsValid == false)
            {
                return;
            }

            IReadOnlyList<Point2D> importedPoints;
            try
            {
                importedPoints = excelService.Import(openResult.FilePath);
            }
            catch (Exception ex)
            {
                ShowError("Points cannot be imported from the selected file", ex);
                return;
            }

            if (importedPoints.Count == 0)
            {
                ShowInfo("The selected file does not contain valid points. Expected two numeric columns: X and Y in millimetres.");
                return;
            }

            if (Points.Count > 0)
            {
                MessageBoxResult choice = MessageBox.Show(
                    "Replace the current points with the imported ones?",
                    "Import points",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question);
                if (choice == MessageBoxResult.Cancel)
                {
                    return;
                }
                if (choice == MessageBoxResult.Yes)
                {
                    Points.Clear();
                }
            }

            AddPoints(importedPoints);
        }

        public void ExportToExcel()
        {
            var saver = new FileDialogSaver
            {
                InputData = new SaveDialogInputData
                {
                    FilterString = "Excel workbook (*.xlsx)|*.xlsx"
                }
            };
            SaveFileResult saveResult = saver.SaveFile();
            if (saveResult.IsValid == false || string.IsNullOrWhiteSpace(saveResult.FileName))
            {
                return;
            }

            try
            {
                excelService.Export(saveResult.FileName, Points);
            }
            catch (Exception ex)
            {
                ShowError("Points cannot be exported to Excel", ex);
            }
        }

        public void ShowChart()
        {
            if (Points.Count == 0)
            {
                return;
            }

            var labels = new List<string> { "X, mm", "Y, mm" };
            var parameter = new ArrayParameter<double>(Points.Count, labels);
            for (int i = 0; i < Points.Count; i++)
            {
                parameter.Data[i, 0] = PointUnits.ToMillimeters(Points[i].X);
                parameter.Data[i, 1] = PointUnits.ToMillimeters(Points[i].Y);
            }

            var graphViewModel = new GraphViewModel(parameter);
            var series = graphViewModel.Series.First();
            series.XItems.SelectedItem = series.XItems.Collection.First(x => x.Name == labels[0]);
            var yColumn = series.YItems.CollectionItems.First(x => x.Item.Name == labels[1]);
            series.YItems.SelectedItems = new List<IValueParameter<double>> { yColumn.Item };
            graphViewModel.RedrawLinesCommand.Execute(null);

            var window = new GraphView(graphViewModel);
            window.ShowDialog();
        }

        private void AddPoints(IEnumerable<Point2D> points)
        {
            foreach (Point2D point in points)
            {
                Points.Add(point);
            }
            SelectedPoint = Points[^1];
        }

        private static void ShowError(string message, Exception exception)
        {
            MessageBox.Show(
                $"{message}.{Environment.NewLine}{exception.Message}",
                "Points",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        private static void ShowInfo(string message)
        {
            MessageBox.Show(message, "Points", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
