using StructureHelperCommon.Infrastructures.Exceptions;
using System;
using System.Collections.Generic;

namespace StructureHelperCommon.Models.Shapes
{
    public class PolygonShape : IPolygonShape
    {
        private readonly List<IVertex> _vertices = new();

        public Guid Id { get; }
        public IReadOnlyList<IVertex> Vertices => _vertices;
        public bool IsClosed { get; set; } = true;

        public PolygonShape(Guid id)
        {
            Id = id;
        }

        public IVertex AddVertex(IVertex vertex)
        {
            _vertices.Add(vertex);
            return vertex;
        }

        public IVertex InsertVertex(int index, IVertex vertex)
        {
            _vertices.Insert(index, vertex);
            return vertex;
        }

        public IVertex AddVertexBefore(IVertex existing, IVertex vertex)
        {
            int index = CheckVertexExists(existing);
            _vertices.Insert(index, vertex);
            return vertex;
        }
        public IVertex AddVertexAfter(IVertex existing, IVertex vertex)
        {
            int index = CheckVertexExists(existing);
            _vertices.Insert(index + 1, vertex);
            return vertex;
        }

        public void RemoveVertex(IVertex vertex)
        {
            if (!_vertices.Remove(vertex))
                throw new InvalidOperationException("The specified vertex was not found in the polygon.");
        }

        public void Clear()
        {
            _vertices.Clear();
        }



        private int CheckVertexExists(IVertex existing)
        {
            int index = _vertices.IndexOf(existing);
            if (index == -1)
            {
                throw new StructureHelperException("The specified vertex was not found in the polygon.");
            }
            return index;
        }

    }

}
