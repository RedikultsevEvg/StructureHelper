using LoaderCalculator.Data.Materials;
using Newtonsoft.Json;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


        public object Clone()
        {
            throw new NotImplementedException();
        }

        public IMaterial GetCrackedLoaderMaterial(LimitStates limitState, CalcTerms calcTerm)
        {
            throw new NotImplementedException();
        }

        public IMaterial GetLoaderMaterial(LimitStates limitState, CalcTerms calcTerm)
        {
            throw new NotImplementedException();
        }
    }
}
