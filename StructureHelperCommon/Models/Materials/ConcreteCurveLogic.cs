using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Materials.MaterialBuilders;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;

namespace StructureHelperCommon.Models.Materials
{
    public class ConcreteCurveLogic : IMaterialLogic
    {
        private ConcreteOptions concreteOptions;
        private IMaterialOptionLogic optionLogic;
        private ConcreteLogicOptions options;


        public string Name { get; set; }
        public IMaterialLogicOptions Options
        {
            get => options;
            set
            {
                if (value is not ConcreteLogicOptions)
                {
                    throw new StructureHelperException($"{ErrorStrings.ExpectedWas(typeof(ConcreteLogicOptions), value)}");
                }
                options = (ConcreteLogicOptions)value;
            }
        }

        public MaterialTypes MaterialType { get; set; }
        public DiagramType DiagramType { get; set; }

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
            IMaterialBuilder builder = new ConcreteBuilder(concreteOptions);
            IBuilderDirector director = new BuilderDirector(builder);
            return director;
        }

        private void GetLoaderOptions()
        {
            concreteOptions = new ConcreteOptions();
            optionLogic = new ConcreteMaterialOptionLogic(options);
            optionLogic.SetMaterialOptions(concreteOptions);
        }
    }
}
