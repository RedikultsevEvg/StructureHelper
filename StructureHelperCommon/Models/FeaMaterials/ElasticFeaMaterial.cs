using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class ElasticFeaMaterial : IElasticFeaMaterial
    {
        public Guid Id { get; }
        public string Name { get; set; }
        public double YoungModulus { get; set; }
        public double PoissonRatio { get; set; }


        public ElasticFeaMaterial(Guid id)
        {
            Id = id;
        }
    }
}
