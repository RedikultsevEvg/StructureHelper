using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Parameters
{
    public class NamedValue<T> : ISaveable
    {
        public Guid Id { get; }
        public string Name { get; set; }
        public T Value { get; set; }
        public NamedValue(Guid id)
        {
            Id = id;
        }
        public NamedValue() : this (Guid.NewGuid())
        { }
    }
}
