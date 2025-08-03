using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    /// <summary>
    /// Implement logic for calculation of bearing capacity of stirrups by value of their density
    /// </summary>
    public interface IStirrupByDensity : IStirrup, IHasStartEndCoordinate
    {
        /// <summary>
        /// Direct density of stirrups, N/m
        /// </summary>
        double StirrupDensity { get; set; }
    }
}
