using LoaderCalculator.Data.Materials;
using StructureHelperCommon.Services;
using StructureHelperLogics.MaterialBuilders.Decorators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.MaterialBuilders
{
    internal class RestrictStrainDecorator : IMaterialBuilder
    {
        IMaterialBuilder builder;
        public IMaterialOption MaterialOption { get; set; }

        public RestrictStrainDecorator(IMaterialBuilder builder)
        {
            this.builder = builder;
        }

        public IMaterial GetMaterial()
        {
            CheckOptions();
            var option = (RestrictStrainOption)MaterialOption;
            var material = new Material();
            material.InitModulus = builder.GetMaterial().InitModulus;
            material.DiagramParameters = new List<double>()
            {
                option.MaxTensileStrain,
                option.MaxCompessionStrain
            };
            material.Diagram = GetStressDiagram;
            return material;
        }

        private void CheckOptions()
        {
            CheckObject.CompareTypes(typeof(RestrictStrainOption), MaterialOption.GetType());
        }

        private double GetStressDiagram(IEnumerable<double> parameters, double strain)
        {
            var maxTensileStrain = parameters.ToList()[0];
            var maxCompressionStrain = parameters.ToList()[1];
            if (strain > maxTensileStrain || strain < maxCompressionStrain)
            {
                return 0d;
            }
            else
            {
                var material = builder.GetMaterial();
                return material.Diagram.Invoke(parameters, strain);
            }
        }
    }
}
