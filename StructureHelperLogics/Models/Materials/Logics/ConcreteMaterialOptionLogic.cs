using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Strings;
using LCMB = LoaderCalculator.Data.Materials.MaterialBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.Design.AxImporter;

namespace StructureHelperLogics.Models.Materials
{
    internal class ConcreteMaterialOptionLogic : IMaterialOptionLogic
    {
        private IConcreteLibMaterial material;
        private LimitStates limitState;
        public ConcreteMaterialOptionLogic(IConcreteLibMaterial material, LimitStates limitState)
        {
            this.material = material;
            this.limitState = limitState;
        }
        public void SetMaterialOptions(LCMB.IMaterialOptions materialOptions)
        {
            Check(materialOptions);
            var concreteOptions = materialOptions as LCMB.ConcreteOptions;
            concreteOptions.WorkInTension = false;
            if (limitState == LimitStates.ULS & material.TensionForULS == true)
            {
                concreteOptions.WorkInTension = true;
            }
            if (limitState == LimitStates.SLS & material.TensionForSLS == true)
            {
                concreteOptions.WorkInTension = true;
            }
        }

        private static void Check(LCMB.IMaterialOptions materialOptions)
        {
            if (materialOptions is null)
            {
                throw new StructureHelperException(ErrorStrings.ParameterIsNull + $": expected {typeof(LCMB.ConcreteOptions)}, but was null");
            }
            if (materialOptions is not LCMB.ConcreteOptions)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown + $": expected {typeof(LCMB.ConcreteOptions)}, but was {materialOptions.GetType()}");
            }
        }
    }
}
