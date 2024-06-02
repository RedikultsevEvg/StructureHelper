using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Copyright (c) 2023 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class PredicateEntry
    {
        public string Name { get; set; }
        public PredicateTypes PredicateType { get; set; }
        public override bool Equals(object? obj)
        {
            if (obj is null) { return false; } 
            var item = obj as PredicateEntry;
            if (item.PredicateType == PredicateType & item.Name == Name) { return true; }
            return false;
        }
    }
}
