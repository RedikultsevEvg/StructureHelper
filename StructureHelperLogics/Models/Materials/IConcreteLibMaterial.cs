using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Materials.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.


namespace StructureHelperLogics.Models.Materials
{
    /// <summary>
    /// Implements logic for library concrete material
    /// </summary>
    public interface IConcreteLibMaterial : ILibMaterial, ICrackedMaterial
    {
        /// <summary>
        /// Humidity of concrete
        /// </summary>
        double RelativeHumidity { get; set; }
        double MinAge { get; set; }
        double MaxAge { get; set; }
    }
}
