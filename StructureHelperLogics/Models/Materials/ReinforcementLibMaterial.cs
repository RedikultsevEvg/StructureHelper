using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LCM = LoaderCalculator.Data.Materials;
using LCMB = LoaderCalculator.Data.Materials.MaterialBuilders;

namespace StructureHelperLogics.Models.Materials
{
    public class ReinforcementLibMaterial : IReinforcementLibMaterial
    {
        public ILibMaterialEntity MaterialEntity { get; set; }

        private IMaterialOptionLogic optionLogic;

        public ReinforcementLibMaterial()
        {
            optionLogic = new MaterialOptionLogic(new LCMB.ReinforcementOptions());
        }

        public object Clone()
        {
            return new ReinforcementLibMaterial() { MaterialEntity = MaterialEntity};
        }

        public LCM.IMaterial GetLoaderMaterial(LimitStates limitState, CalcTerms calcTerm)
        {
            var materialOptions = optionLogic.SetMaterialOptions(MaterialEntity, limitState, calcTerm);
            LCMB.IMaterialBuilder builder = new LCMB.ReinforcementBuilder(materialOptions);
            LCMB.IBuilderDirector director = new LCMB.BuilderDirector(builder);
            return director.BuildMaterial();
        }
    }
}
