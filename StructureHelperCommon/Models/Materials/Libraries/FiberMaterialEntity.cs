using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    internal class FiberMaterialEntity : IFiberMaterialEntity
    {
        ///<inheritdoc/>
        public Guid Id { get; }
        public CodeTypes CodeType { get; }
        public ICodeEntity Code { get; set; }
        public string Name { get; }
        ///<inheritdoc/>
        public double YoungsModulus { get; set; }
        ///<inheritdoc/>
        public double MainStrength { get; }
        public FiberMaterialEntity(Guid id)
        {
            Id = id;
            Name = "";
        }
        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
