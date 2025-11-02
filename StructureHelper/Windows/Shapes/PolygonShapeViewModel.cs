using netDxf.Entities;
using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.UI.GraphicalPrimitives;
using StructureHelper.Services.Exports;
using StructureHelper.Windows.Shapes.Logics;
using StructureHelper.Windows.UserControls.WorkPlanes;
using StructureHelper.Windows.ViewModels;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services.Exports;
using StructureHelperCommon.Services.Exports.Factories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace StructureHelper.Windows.Shapes
{
    public class PolygonShapeViewModel : OkCancelViewModelBase
    {
        private const int minVertexCount = 3;
        private const string ErrorOfUpdatingOfPolygon = "Error of updating of polygon";
        private const string ErrorOfObtainigOfPolyline = "Error of obtaining of dxf polyline";
        private readonly IPoint2D absoluteCenter;
        private readonly IPoint2D localCenter;
        private readonly ILinePolygonShape polygonShape;
        private IReadOnlyList<IVertex> vertices => polygonShape.Vertices;
        private IObjectConvertStrategy<List<IGraphicalPrimitive>, ILinePolygonShape> logic;
        private RelayCommand importFromDxfCommand;
        private RelayCommand exportToDxfCommand;
        public Point2DViewModel Center { get; }

        public VertexViewModel SelectedVertex { get; set; }
        public ObservableCollection<VertexViewModel> Vertices { get;} = new();
        public WorkPlaneRootViewModel WorkPlaneRoot { get;} = new();

        public PolygonShapeViewModel(ILinePolygonShape polygonShape, IPoint2D center)
        {
            this.polygonShape = polygonShape;
            this.absoluteCenter = center;
            this.localCenter = new Point2D();
            Center = new(this.absoluteCenter);
            ReloadVertices();
            Redraw(null);
        }

        private void ReloadVertices()
        {
            Vertices.Clear();
            foreach (var item in this.polygonShape.Vertices)
            {
                Vertices.Add(new VertexViewModel(item, localCenter));
            }
        }

        private RelayCommand addVertexCommand;
        public ICommand AddVertexCommand => addVertexCommand ??= new RelayCommand(AddVertex);
        public ICommand FlipVerticalCommand => flipVerticalCommand ??= new RelayCommand(FlipVertical);
        public ICommand FlipHorizontalCommand => flipHorizontalCommand ??= new RelayCommand(FlipHorizontal);

        public ICommand ImportFromDxfCommand => importFromDxfCommand ??= new RelayCommand(ImportFromDxf);
        public ICommand FileDroppedCommand => fileDroppedCommand ??= new RelayCommand(OnFileDropped);

        private void OnFileDropped(object obj)
        {
            if (obj is string[] files && files.Length > 0)
            {
                fileName = files.First();
                string extension = Path.GetExtension(fileName).ToLowerInvariant();
                if (extension == ".dxf")
                {
                    SafetyProcessor.RunSafeProcess(GetPolyline2DFromFile, ErrorOfObtainigOfPolyline);
                    SafetyProcessor.RunSafeProcess(UpdatePolygon, ErrorOfUpdatingOfPolygon);
                }
                else
                {
                    MessageBox.Show($"Unsupported file type: {extension}");
                }         
            }
            else
            {
                MessageBox.Show($"Error of file");
            }
        }

        private void GetPolyline2DFromFile()
        {
            var logic = new SinglePolyline2DImportFromDxfLogic() { FileName = fileName };
            logic.Import();
            var polylines = logic.Polyline2Ds;
            GetPolyline(polylines);
        }

        private void GetPolyline(List<Polyline2D> polylines)
        {
            if (polylines.Count > 0)
            {
                if (polylines.Count == 1)
                {
                    polyline = polylines[0];
                }
                else
                {
                    MessageBox.Show($"File: {fileName} has {polylines.Count} polylines, but one expected");
                }
            }
            else
            {
                MessageBox.Show($"File: {fileName} does not has suitable polylines");
            }
        }

        private void ImportFromDxf(object commandParameter)
        {
            SafetyProcessor.RunSafeProcess(GetPolyline2D, ErrorOfObtainigOfPolyline);
            SafetyProcessor.RunSafeProcess(UpdatePolygon, ErrorOfUpdatingOfPolygon);
        }

        private void UpdatePolygon()
        {
            var convertLogic = new Polyline2DToLinePoligonConvertLogic();
            LinePolygonShape newPolygonShape = convertLogic.Convert(polyline);
            newPolygonShape.IsClosed = true;
            var updateLogic = new LinePolygonShapeUpdateStrategy();
            updateLogic.Update(polygonShape, newPolygonShape);
            ReloadVertices();
            Redraw(null);
        }

        private void GetPolyline2D()
        {
            FileIOInputData inputData = FileInputDataFactory.GetFileIOInputData(FileInputDataType.Dxf);
            var logic = new SinglePolyline2DImportFromDxfLogic();
            var importService = new ImportFromFileService(inputData, logic);
            importService.Import();
            var polylines = logic.Polyline2Ds;
            GetPolyline(polylines);
        }

        public ICommand ExportToDxfCommand => exportToDxfCommand ??= new RelayCommand(ExportToDxf);

        private void ExportToDxf(object commandParameter)
        {
            FileIOInputData inputData = FileInputDataFactory.GetFileIOInputData(FileInputDataType.Dxf);
            var logic = new ShapesExportToDxfLogic(polygonShape, LayerNames.StructiralPrimitives);
            var exportService = new ExportToFileService(inputData, logic);
            exportService.Export();
        }

        private void FlipHorizontal(object obj)
        {
            foreach (var item in vertices)
            {
                item.Point.Y = - item.Point.Y;
            }
            ReloadVertices();
            Redraw(null);
        }

        private void FlipVertical(object obj)
        {
            foreach (var item in vertices)
            {
                item.Point.X = - item.Point.X;
            }
            ReloadVertices();
            Redraw(null);
        }

        public ILinePolygonShape GetPolygonShape()
        {
            ILinePolygonShape polygonShape = new LinePolygonShape(Guid.NewGuid());
            polygonShape.Clear();
            foreach (var item in Vertices)
            {
                Vertex vertex = new(Guid.NewGuid());
                vertex.Point.X = item.Point.X - localCenter.X;
                vertex.Point.Y = item.Point.Y - localCenter.Y;
                polygonShape.AddVertex(vertex);
            }
            return polygonShape;
        }

        private void AddVertex(object commandParameter)
        {
            VertexViewModel vertexViewModel = GetNewVertexViewModel();
            Vertices.Add(vertexViewModel);
            Redraw(null);
        }

        private static VertexViewModel GetNewVertexViewModel()
        {
            Vertex vertex = new(Guid.Empty);
            VertexViewModel vertexViewModel = new(vertex);
            return vertexViewModel;
        }

        private RelayCommand redrawCommand;
        public ICommand RedrawCommand => redrawCommand ??= new RelayCommand(Redraw);

        private void Redraw(object commandParameter)
        {
            logic = new PolygonShapeToGraphicPrimitveConvertStrategy(this);
            WorkPlaneRoot.PrimitiveCollection.Primitives.Clear();
            var polygon = GetPolygonShape();
            var updateStrategy = new LinePolygonShapeUpdateStrategy();
            updateStrategy.Update(polygonShape, polygon);
            WorkPlaneRoot.PrimitiveCollection.Primitives.Add(logic.Convert(polygon)[0]);
        }

        private RelayCommand addVertexBeforeCommand;
        public ICommand AddVertexBeforeCommand => addVertexBeforeCommand ??= new RelayCommand(AddVertexBefore,
            o => SelectedVertex is not null);

        private void AddVertexBefore(object commandParameter)
        {
            int index = CheckVertexExists(SelectedVertex);
            VertexViewModel vertexViewModel = GetNewVertexViewModel();
            Vertices.Insert(index, vertexViewModel);
            Redraw(null);
        }

        private int CheckVertexExists(VertexViewModel vertex)
        {
            int index = Vertices.IndexOf(vertex);
            if (index == -1)
            {
                throw new StructureHelperException("The specified vertex was not found in the polygon.");
            }
            return index;
        }

        private RelayCommand addVertexAfterCommand;
        public ICommand AddVertexAfterCommand => addVertexAfterCommand ??= new RelayCommand(AddVertexAfter,
            o => SelectedVertex is not null);

        private void AddVertexAfter(object commandParameter)
        {
            int index = CheckVertexExists(SelectedVertex);
            VertexViewModel vertexViewModel = GetNewVertexViewModel();
            Vertices.Insert(index+1, vertexViewModel);
            Redraw(null);
        }

        private RelayCommand deleteVertexCommand;
        private RelayCommand flipVerticalCommand;
        private RelayCommand flipHorizontalCommand;
        private static Polyline2D polyline;
        private RelayCommand fileDroppedCommand;
        private string fileName;

        public ICommand DeleteVertexCommand => deleteVertexCommand ??= new RelayCommand(DeleteVertex,
            o => SelectedVertex is not null && Vertices.Count >= minVertexCount);

        private void DeleteVertex(object commandParameter)
        {
            if (SelectedVertex is null) { return; }
            Vertices.Remove(SelectedVertex);
            Redraw(null);
        }
    }
}
