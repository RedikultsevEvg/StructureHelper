using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Materials
{
    /// <summary>
    /// Implements options for dreating tension part of concrete material diagram
    /// </summary>
    public interface ICustomConcreteTensionOptions : ISaveable, ICloneable
    {
        /// <summary>
        /// Strength, Pa (positive in tenssion)
        /// </summary>
        double Strength { get; set; }
        /// <summary>
        /// Strain of collaps of concrete in tension
        /// </summary>
        double UltimateStrain { get; set; }
        /// <summary>
        /// Limit strain in tension, i.e. strain where calculation will be stopped, usual positive infiniti
        /// </summary>
        double LimitStrain { get; set; }
        /// <summary>
        /// Strain of cracking, usual equal to Ultimate strain
        /// </summary>
        //double CrackStrain { get; set; }
    }

}
