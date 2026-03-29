using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.FeaMaterials.ExportLogics
{
    public class CDPMaterialBlock : IMaterialBlock
    {
        public void Build(IAbaqusContext context)
        {
            var model = context.Get<IModelContext>();

            var b = context.Builder;

            b.AddKeyword($"mdb.models['{model.ModelName}'].Material(name='Concrete')");

            context.Set(new MaterialContext
            {
                MaterialName = "Concrete"
            });
        }
    }
}
