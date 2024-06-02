using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Parameters
{
    public class NamedCollection<T> : ISaveable
    {
        public Guid Id { get; }
        public string Name { get; set; }
        public List<T> Collection { get; set; }
        public NamedCollection(Guid id)
        {
            Id = id;
            Name = string.Empty;
            Collection = new List<T>();
        }
        public NamedCollection() : this(Guid.NewGuid())
        {
        }
    }
}
