﻿using System.Collections.Generic;

namespace StructureHelperCommon.Models.Tables
{
    public interface IShTableRow<T>
    {
        List<IShTableCell<T>> Elements { get; }
        int RowSize { get; }
    }
}