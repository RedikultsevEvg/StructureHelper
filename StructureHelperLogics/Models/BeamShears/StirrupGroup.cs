using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    public class StirrupGroup : IStirrupGroup
    {
        public Guid Id { get; }
        public string Name { get; set; } = string.Empty;
        public List<IStirrup> Stirrups { get; } = new();
        public double CompressedGap { get; set; }

        public StirrupGroup(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            var updateStrategy = new StirrupGroupUpdateStrategy();
            StirrupGroup newItem = new(Guid.NewGuid());
            updateStrategy.Update(newItem, this);
            return newItem;
        }

    }
}
