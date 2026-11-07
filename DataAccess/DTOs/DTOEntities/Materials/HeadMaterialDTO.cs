using LoaderCalculator.Data.Materials;
using Newtonsoft.Json;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials;
using StructureHelperLogics.Models.Materials;
using System.Windows.Media;

namespace DataAccess.DTOs
{
    public class HeadMaterialDTO : IHeadMaterial
    {

        [JsonProperty("Id")]
        public Guid Id { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Color")]
        public Color Color { get; set; }
        [JsonProperty("HelperMaterial")]
        public IHelperMaterial HelperMaterial { get; set; }


        public HeadMaterialDTO()
            : this(Guid.NewGuid())
        {
        }

        public HeadMaterialDTO(Guid id)
        {
            Id = id;
        }

        //public object Clone()
        //{
        //    HeadMaterialDTO newItem = new(Id);
        //    new HeadMaterialUpdateStrategy().Update(newItem, this);
        //    return newItem;
        //}

        //public IMaterial GetCrackedLoaderMaterial(LimitStates limitState, CalcTerms calcTerm)
        //{
        //    throw new NotImplementedException();
        //}

        //public IMaterial GetLoaderMaterial(LimitStates limitState, CalcTerms calcTerm)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
