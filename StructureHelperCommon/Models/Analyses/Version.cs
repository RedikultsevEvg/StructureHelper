﻿using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Analyses
{
    public class Version : IVersion
    {
        public DateTime DateTime { get; set; }

        public ISaveable Item { get; set; }
    }
}
