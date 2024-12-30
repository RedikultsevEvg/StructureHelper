using System.Collections.Generic;
using StructureHelperCommon.Models.Forces;


//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    /// <summary>
    /// Interface for entities which has collection of force actions
    /// </summary>
    public interface IHasForceActions
    {
        /// <summary>
        /// Collection of force actions
        /// </summary>
        List<IForceAction> ForceActions { get; }
    }
}
