using System;
using System.Collections.Generic;
using System.Text;


namespace StructureHelperLogics.NdmCalculations.Entities.LibraryDecorators
{
    public class NdmDecorator : INdmDecorator
    {
        public double Area { get { return _ndm.Area; } set { } }
        public double Prestrain { get { return _ndm.Prestrain; } set { } }
        public double X { get { return _ndm.CenterX; } set { _ndm.CenterX = value; } }
        public double Y { get { return _ndm.CenterY; } set { _ndm.CenterY = value; } }
        public IMaterialDecorator Material { get; set; }
        public object UserData { get { return _ndm.UserData; } set { _ndm.UserData = value; } }
        private LoaderCalculator.Data.Ndms.INdm _ndm;

        public NdmDecorator()
        {
            _ndm = new LoaderCalculator.Data.Ndms.Ndm();
        }

        public LoaderCalculator.Data.Ndms.INdm GetNdm()
        {
            Refresh();
            return _ndm;
        }

        private void Refresh()
        {
            _ndm.Material = Material.GetMaterial();
        }
    }
}
