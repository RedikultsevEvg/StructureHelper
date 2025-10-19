using Newtonsoft.Json;
using StructureHelperCommon.Models.Shapes;

namespace DataAccess.DTOs
{
    public class LinePolygonShapeDTO : ILinePolygonShape
    {
        private readonly List<IVertex> _vertices = [];
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("Vertices")]
        public IReadOnlyList<IVertex> Vertices => _vertices;
        [JsonProperty("IsClosed")]
        public bool IsClosed { get; set; }


        public LinePolygonShapeDTO(Guid id)
        {
            Id = id;
        }

        public IVertex AddVertex(IVertex vertex)
        {
            _vertices.Add(vertex);
            return vertex;
        }

        public IVertex AddVertexAfter(IVertex existing, IVertex vertex)
        {
            throw new NotImplementedException();
        }

        public IVertex AddVertexBefore(IVertex existing, IVertex vertex)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            _vertices.Clear();
        }

        public IVertex InsertVertex(int index, IVertex vertex)
        {
            throw new NotImplementedException();
        }

        public void RemoveVertex(IVertex vertex)
        {
            throw new NotImplementedException();
        }
    }
}
