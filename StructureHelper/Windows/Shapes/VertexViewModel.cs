using StructureHelper.Infrastructure;
using StructureHelperCommon.Models.Shapes;

namespace StructureHelper.Windows.Shapes
{
    public class VertexViewModel : ViewModelBase
    {
        private readonly IVertex vertex;


        public Point2DViewModel Point { get; private set; }
        public VertexViewModel(IVertex vertex) : this(vertex, new Point2D()) { }
        public VertexViewModel(IVertex vertex, IPoint2D center)
        {
            this.vertex = vertex;
            Point = new(this.vertex.Point, center);
        }

        public void Refresh()
        {
            Point.Refresh();
        }

        public VertexViewModel(Vertex vertex) : this(vertex, new Point2D())
        {
        }
    }
}
