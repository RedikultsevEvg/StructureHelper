using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Codes;
using System;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public class ConcreteMaterialEntity : IConcreteMaterialEntity
    {
        public Guid Id { get; }
        public CodeTypes CodeType { get; set; }
        public ICodeEntity Code { get; set; }
        public string Name { get; set; }
        public double MainStrength { get; set; }

        public ConcreteMaterialEntity(Guid id)
        {
            Id = id;
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
