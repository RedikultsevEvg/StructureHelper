using StructureHelperCommon.Infrastructures.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class AbaqusScript : IAbaqusScript
    {
        private readonly List<IAbaqusScriptBlock> blocks = new();

        public AbaqusScript Add(IAbaqusScriptBlock block)
        {
            blocks.Add(block);
            return this;
        }

        public string Build()
        {
            var context = new AbaqusContext();

            EnsureDefaults();

            var ordered = BlockDependencyResolver.Sort(blocks);

            foreach (var block in ordered)
            {
                block.Build(context);
            }

            return context.Builder.ToString();
        }

        private void EnsureDefaults()
        {
            if (!blocks.Any(b => b is IProvides<IModelContext>))
            {
                blocks.Insert(0, new ModelBlock("DefaultModel"));
            }
        }
    }
}
