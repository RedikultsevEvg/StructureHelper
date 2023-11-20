using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Materials.MaterialBuilders;
using StructureHelperCommon.Infrastructures.Exceptions;

namespace StructureHelperCommon.Models.Materials
{
    public class ReinforcementBiLinearLogic : IMaterialLogic
    {
        private ReinforcementOptions materialOptions;
        private IMaterialOptionLogic optionLogic;
        private ReinforcementLogicOptions options;
        private IMaterialLogicOptions options1;

        public string Name { get; set; }
        public IMaterialLogicOptions Options
        {
            get => options;
            set
            {
                if (value is not ReinforcementLogicOptions)
                {
                    throw new StructureHelperException($"{ErrorStrings.ExpectedWas(typeof(ReinforcementLogicOptions), value)}");
                }
                options = (ReinforcementLogicOptions)value;
            }
        }

        public IMaterial GetLoaderMaterial()
        {
            GetLoaderOptions();
            IBuilderDirector director = GetMaterialDirector();
            var material = director.BuildMaterial();
            return material;
        }

        private IBuilderDirector GetMaterialDirector()
        {
            IMaterialBuilder builder = new ReinforcementBuilder(materialOptions);
            IBuilderDirector director = new BuilderDirector(builder);
            return director;
        }

        private void GetLoaderOptions()
        {
            materialOptions = new ReinforcementOptions();
            optionLogic = new MaterialCommonOptionLogic(options);
            optionLogic.SetMaterialOptions(materialOptions);
        }
    }
}
