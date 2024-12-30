using System.Collections.Generic;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperCommon.Models
{
    public interface ITraceCollectionLogic<T> : ITraceEntityLogic
    {
        IEnumerable<T>? Collection { get; set; }
        int Priority { get; set; }
    }
}