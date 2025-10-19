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
        private readonly IPoint2D center;
        private readonly ILinePolygonShape polygonShape;
        private IObjectConvertStrategy<List<IGraphicalPrimitive>, ILinePolygonShape> logic;
        public Point2DViewModel Center { get; }

        public PolygonShapeViewModel(ILinePolygonShape polygonShape) : this(polygonShape, new Point2D() { X = 0, Y = 0 }) { }
        public VertexViewModel SelectedVertex { get; set; }
        public ObservableCollection<VertexViewModel> Vertices { get;} = new();
        public WorkPlaneRootViewModel WorkPlaneRoot { get;} = new();

        public PolygonShapeViewModel(ILinePolygonShape polygonShape, IPoint2D center)
        {
            this.polygonShape = polygonShape;
            this.center = center;
            Center = new(this.center);
            foreach (var item in this.polygonShape.Vertices)
            {
                Vertices.Add(new VertexViewModel(item, this.center));
            }
            Redraw(null);
        }

        private RelayCommand addVertexCommand;
        public ICommand AddVertexCommand => addVertexCommand ??= new RelayCommand(AddVertex);
        public ILinePolygonShape GetPolygonShape()
        {
            ILinePolygonShape polygonShape = new LinePolygonShape(Guid.NewGuid());
            polygonShape.Clear();
            foreach (var item in Vertices)
            {
                Vertex vertex = new(Guid.NewGuid());
                vertex.Point.X = item.Point.X;
                vertex.Point.Y = item.Point.Y;
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
