using System.Collections.Generic;

namespace StructureHelper.Infrastructure
{
    public class NamedList<T> : List<T>
    {
        public string Name { get; set; }
    }
}
