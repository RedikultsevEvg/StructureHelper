using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public static class BlockDependencyResolver
    {
        public static List<IAbaqusScriptBlock> Sort(List<IAbaqusScriptBlock> blocks)
        {
            var result = new List<IAbaqusScriptBlock>();
            var provided = new HashSet<Type>();

            var remaining = new List<IAbaqusScriptBlock>(blocks);

            while (remaining.Any())
            {
                var progress = false;

                foreach (var block in remaining.ToList())
                {
                    var requires = GetRequiredTypes(block);

                    if (requires.All(r => provided.Contains(r)))
                    {
                        result.Add(block);

                        foreach (var p in GetProvidedTypes(block))
                            provided.Add(p);

                        remaining.Remove(block);
                        progress = true;
                    }
                }

                if (!progress)
                {
                    throw new Exception("Cannot resolve dependencies between blocks.");
                }
            }

            return result;
        }

        private static IEnumerable<Type> GetRequiredTypes(object block)
        {
            return block.GetType().GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequires<>))
                .Select(i => i.GetGenericArguments()[0]);
        }

        private static IEnumerable<Type> GetProvidedTypes(object block)
        {
            return block.GetType().GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IProvides<>))
                .Select(i => i.GetGenericArguments()[0]);
        }
    }
}
