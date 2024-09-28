using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs.Converters
{
    public class HeadMaterialToDTOConvertStrategy : IConvertStrategy<HeadMaterialDTO, IHeadMaterial>
    {
        private IUpdateStrategy<IHeadMaterial> updateStrategy;

        public HeadMaterialToDTOConvertStrategy(IUpdateStrategy<IHeadMaterial> updateStrategy)
        {
            this.updateStrategy = updateStrategy;
        }

        public HeadMaterialToDTOConvertStrategy() : this (new HeadMaterialUpdateStrategy())
        {
            
        }

        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public HeadMaterialDTO Convert(IHeadMaterial source)
        {
            HeadMaterialDTO newItem = new() { Id = source.Id};
            updateStrategy.Update(newItem, source);
            return newItem;
        }
    }
}
