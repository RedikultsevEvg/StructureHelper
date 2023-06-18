using StructureHelperCommon.Infrastructures.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Codes
{
    public class CodeEntity : ICodeEntity
    {
        public Guid Id { get; }
        public NatSystems NatSystem { get; }
        public string Name { get; set; }
        public string FullName { get; set; }


        public CodeEntity(Guid id, NatSystems natSystem)
        {
            Id = id;
            NatSystem = natSystem;
            Name = "";
            FullName = "";
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
