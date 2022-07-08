using LoaderCalculator.Data.Matrix;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Entities.LibraryDecorators
{
    public class ForceMatrixDecorator
    {
        public double Mx { get { return _forceMatrix.Mx; } set { _forceMatrix.Mx = value; } }
        public double My { get { return _forceMatrix.My; } set { _forceMatrix.My = value; } }
        public double Nz { get { return _forceMatrix.Nz; } set { _forceMatrix.Nz = value; } }

        private IForceMatrix _forceMatrix;
        public ForceMatrixDecorator()
        {
            _forceMatrix = new ForceMatrix();
        }

        public IForceMatrix GetForceMatrix()
        {
            return _forceMatrix;
        }
    }
}
