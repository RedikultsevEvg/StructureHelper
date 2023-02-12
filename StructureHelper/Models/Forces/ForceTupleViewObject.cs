using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Models.Forces
{
    internal class ForceTupleViewObject
    {
        IForceTuple forceTuple;

        private double mx;

        public double Mx
        {
            get { return mx; }
            set { mx = value; }
        }
        private double my;

        public double My
        {
            get { return my; }
            set { my = value; }
        }
        private double nz;

        public double Nz
        {
            get { return nz; }
            set { nz = value; }
        }
        private double qx;

        public double Qx
        {
            get { return qx; }
            set { qx = value; }
        }
        private double qy;

        public double Qy
        {
            get { return qy; }
            set { qy = value; }
        }
        private double mz;

        public double Mz
        {
            get { return mz; }
            set { mz = value; }
        }


    }
}
