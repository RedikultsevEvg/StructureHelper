using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Materials.MaterialBuilders;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Services;

namespace StructureHelperCommon.Models.Materials
{
    public class ReinforcementByBuilderLogic : IMaterialLogic
    {
        private ReinforcementLogicOptions options;
        private ReinforcementOptions materialOptions;
        private IMaterialOptionLogic optionLogic;

        public string Name { get; set; }
        public DiagramType DiagramType { get; set; }
        public IMaterialLogicOptions Options
        {
            get => options;
            set
            {
                CheckObject.CheckType(value, typeof(ReinforcementLogicOptions));
                options = (ReinforcementLogicOptions)value;
            }
        }

        public MaterialTypes MaterialType { get; set; }

        public IMaterial GetLoaderMaterial()
        {
            GetLoaderOptions();
            IBuilderDirector director = GetMaterialDirector();
            try
            {
                var material = director.BuildMaterial();
                return material;
            }
            catch (System.Exception)
            {

                throw;
            }
            
        }

        private IBuilderDirector GetMaterialDirector()
        {
            IMaterialBuilder builder = new ReinforcementBuilder(materialOptions);
            IBuilderDirector director = new BuilderDirector(builder);
            return director;
        }

        private void GetLoaderOptions()
        {
            materialOptions = new ReinforcementOptions()
            {
                DiagramType = DiagramType,
            };
            optionLogic = new MaterialCommonOptionLogic(options);
            optionLogic.SetMaterialOptions(materialOptions);
        }
    }
}
