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
            concreteOptions.WorkInTension = options.WorkInTension;
            concreteOptions.RelativeHumidity = options.RelativeHumidity;
            concreteOptions.Age = options.Age;
        }
    }
}
