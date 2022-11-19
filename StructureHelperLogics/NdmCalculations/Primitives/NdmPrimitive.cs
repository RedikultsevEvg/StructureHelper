using StructureHelperLogics.Models.Materials;
using StructureHelperCommon.Models.Shapes;
using StructureHelper.Models.Materials;
using System.Collections.Generic;
using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Data.Materials;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;

namespace StructureHelperLogics.Models.Primitives
{
    public class NdmPrimitive : INdmPrimitive, ISaveable, ICloneable
    {
        public ICenter Center { get; set; }
        public IShape Shape { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public IHeadMaterial HeadMaterial { get; private set; }
        public double NdmMaxSize { get; set; }
        public int NdmMinDivision { get; set; }
        public double PrestrainKx { get; set; }
        public double PrestrainKy { get; set; }
        public double PrestrainEpsZ { get; set; }
        

        public NdmPrimitive(IHeadMaterial material)
        {
            HeadMaterial = material;
        }

        public IEnumerable<INdm> GetNdms(IMaterial material)
        {
            throw new System.NotImplementedException();
        }

        public void Save()
        {
            throw new System.NotImplementedException();
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
