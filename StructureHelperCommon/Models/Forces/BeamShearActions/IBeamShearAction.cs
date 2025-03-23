using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperCommon.Models.Forces
{
    /// <summary>
    /// Implements properties of force at the and of bar
    /// </summary>
    public interface IBeamShearAction : IAction
    {
        /// <summary>
        /// External force at the end of bar
        /// </summary>
        IFactoredForceTuple ExternalForce { get; }
        /// <summary>
        /// Internal loads on bar
        /// </summary>
        IBeamShearAxisAction SupportAction { get; }
    }
}
