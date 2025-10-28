using netDxf;
using netDxf.Entities;
using netDxf.Header;
using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.UI.GraphicalPrimitives;
using StructureHelper.Windows.Shapes.Logics;
using StructureHelper.Windows.UserControls.WorkPlanes;
using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.BeamShears;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace StructureHelper.Windows.Shapes
{
    public class PolygonShapeViewModel : OkCancelViewModelBase
    {
        private const int minVertexCount = 3;
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

        private void ImportFromDxf(object commandParameter)
        {
            // your DXF file name
            string file = "sample.dxf";
            // this check is optional but recommended before loading a DXF file
            DxfVersion dxfVersion = DxfDocument.CheckDxfFileVersion(file);
            // netDxf is only compatible with AutoCad2000 and higher DXF versions
            if (dxfVersion < DxfVersion.AutoCad2000) return;
            // load file
            DxfDocument loaded = DxfDocument.Load(file);
        }

        public ICommand ExportToDxfCommand => exportToDxfCommand ??= new RelayCommand(ExportToDxf);

        private void ExportToDxf(object commandParameter)
        {
            // your DXF file name
            string file = "sample.dxf";

            // create a new document, by default it will create an AutoCad2000 DXF version
            DxfDocument doc = new DxfDocument();
            // an entity
            List<Polyline2DVertex> polylineVertices = [];
            foreach (var item in vertices)
            {
                Polyline2DVertex vertex = new Polyline2DVertex(item.Point.X, item.Point.Y);
                polylineVertices.Add(vertex);
            }
            Polyline2D polyline2D = new Polyline2D(polylineVertices) { IsClosed = true};
            //polyline2D.Layer = 
            // add your entities here
            doc.Entities.Add(polyline2D);
            // save to file
            doc.Save(file);
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
            WorkPlaneRoot.PrimitiveCollection.Primitives.Add(logic.Convert(polygon)[0]);
            //foreach (var item in Vertices)
            //{
            //    item.Refresh();
            //}
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

        public ICommand DeleteVertexCommand => deleteVertexCommand ??= new RelayCommand(DeleteVertex,
            o => SelectedVertex is not null && Vertices.Count >= minVertexCount);

        private void DeleteVertex(object commandParameter)
        {
            if (SelectedVertex is not null)
            {
                Vertices.Remove(SelectedVertex);
            }
            Redraw(null);
        }
    }
}
