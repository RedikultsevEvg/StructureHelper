﻿using DataAccess.DTOs;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.JsonConverters
{
    public class CrossSectionJsonConverter : BaseJsonConverter<CrossSectionDTO>
    {
        public CrossSectionJsonConverter(IShiftTraceLogger logger) : base(logger)
        {
        }
    }
}
