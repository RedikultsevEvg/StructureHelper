using SharpDX.Direct2D1.Effects;
using StructureHelper.Infrastructure.UI.Converters.Units;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews.DeformedShapes
{
    public class ModifyPathByCurvatureLogic : IModifyPathByCurvatureLogic
    {
        public IDeformedPath ProcessPath(IDeformedPath path, IForceTuple curvature, double scale = 1)
        {
            DeformedPath resultPath = new();

            float kx = (float)(curvature.Mx * scale);
            float ky = (float)(curvature.My * scale);
            float nz = (float)(curvature.Nz * scale);

            foreach (var vector in path.Vectors)
            {
                float z0 = vector.Z;

                // axial deformation
                float z = z0 * (1f + nz - vector.X * ky + vector.Y * kx);

                // bending displacement
                float dx = 0.5f * ky * z * z;
                float dy = - 0.5f * kx * z * z;

                float x = vector.X + dx;
                float y = vector.Y + dy;

                resultPath.Vectors.Add(new Vector3(x, y, z));
            }

            return resultPath;
        }

        public IDeformedPath ProcessPath(IDeformedPath path, float dx, float dy, float dz)
        {
            DeformedPath resultPath = new();

            foreach (var vector in path.Vectors)
            {
                float x = vector.X + dx;
                float y = vector.Y + dy;
                float z = vector.Z + dz;
                resultPath.Vectors.Add(new Vector3(x, y, z));
            }

            return resultPath;
        }
    }
}
