using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Materials
{
    public interface IMaterialStrengthByLibMaterial : ISaveable, IMaterialStrength, ICloneStrategy<IMaterialStrengthByLibMaterial>
    {
        ILibMaterial LibMaterial { get; }
    }
}
