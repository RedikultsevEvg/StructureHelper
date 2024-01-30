using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using LCMB = LoaderCalculator.Data.Materials.MaterialBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.Design.AxImporter;
using LoaderCalculator.Data.Materials.MaterialBuilders;
using StructureHelperCommon.Models.Materials.Libraries;

namespace StructureHelperCommon.Models.Materials
{
    internal class ConcreteMaterialOptionLogic : IMaterialOptionLogic
    {
        private ConcreteLogicOptions options;
        private MaterialCommonOptionLogic optionLogic;
        private FactorLogic factorLogic;

        public ConcreteMaterialOptionLogic(ConcreteLogicOptions options)
        {
            this.options = options;
        }

        public void SetMaterialOptions(IMaterialOptions materialOptions)
        {
            if (materialOptions is not ConcreteOptions)
            {
                throw new StructureHelperException(ErrorStrings.ExpectedWas(typeof(ConcreteOptions), materialOptions.GetType()));
            }
            var concreteOptions = materialOptions as ConcreteOptions;
            optionLogic = new MaterialCommonOptionLogic(options);
            optionLogic.SetMaterialOptions(concreteOptions);
            factorLogic = new FactorLogic(options.SafetyFactors);
            var strength = factorLogic.GetTotalFactor(options.LimitState, options.CalcTerm);
            concreteOptions.ExternalFactor.Compressive = strength.Compressive;
            concreteOptions.ExternalFactor.Tensile = strength.Tensile;
            concreteOptions.WorkInTension = options.WorkInTension;
            concreteOptions.RelativeHumidity = options.RelativeHumidity;
        }
    }
}
