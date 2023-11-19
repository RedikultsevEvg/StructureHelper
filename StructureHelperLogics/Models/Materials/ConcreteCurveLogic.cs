using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Materials.MaterialBuilders;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Materials.Libraries;
using System.Windows.Media.Media3D;
using static System.Windows.Forms.Design.AxImporter;
using LMBuilders = LoaderCalculator.Data.Materials.MaterialBuilders;

namespace StructureHelperLogics.Models.Materials
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

        public IMaterial GetLoaderMaterial()
        {
            GetLoaderOptions();
            IBuilderDirector director = GetMaterialDirector();
            var material = director.BuildMaterial();
            return material;
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
