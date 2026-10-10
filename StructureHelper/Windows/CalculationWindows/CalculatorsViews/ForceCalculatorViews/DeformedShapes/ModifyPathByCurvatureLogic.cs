using SharpDX.Direct2D1.Effects;
using StructureHelper.Infrastructure.UI.Converters.Units;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews.DeformedShapes
{
    public class ModifyPathByCurvatureLogic : IModifyPathByCurvatureLogic
    {
        public IDeformedPath ProcessPath(IDeformedPath path, IForceTuple curvature, float scale = 1.0f)
        {
            DeformedPath resultPath = new();

     
            float kx = (float)curvature.Mx * scale;
            float ky = (float)curvature.My * scale;
            float nz = (float)curvature.Nz * scale;


            for (int i = 0; i < path.Vectors.Count; i++)
            {
                Vector3 vector = path.Vectors[i];
                Vector3 referenceVector;
                if (i == 0)
                {
                    referenceVector = path.Vectors[i + 1];
                }
                else
                {
                    referenceVector = path.Vectors[i - 1];
                }

                float z0 = vector.Z;

                float localKx = ProcessCurvature(kx, vector.Y);
                float localKy = ProcessCurvature(ky, vector.X);

                // axial deformation
                //float z = z0 * (1f + nz) - 0.5f * vector.X * localKy + 0.5f * vector.Y * localKx;
                float z = z0;
                float factorZ = 1f + nz;
                z *= factorZ;
                float dz = (vector.Z - referenceVector.Z) * factorZ;

                // bending displacement
                float dx = ky * z0 * z0;
                float referenceDx = ky * referenceVector.Z * referenceVector.Z;
                float deltaX = dx - referenceDx;
                float dy = -1f * kx * z0 * z0;
                float referenceDy = -1f * kx * referenceVector.Z * referenceVector.Z;
                float deltaY = dy - referenceDy;
                double angleY = Math.Atan2(deltaX, dz);
                double angleX = Math.Atan2(deltaY, dz);
                if (i == 0)
                {
                    angleY *= -1f;
                    angleX *= -1f;
                }
                z -= vector.X * (float)Math.Sin(angleY); 
                z -= vector.Y * (float)Math.Sin(angleX);
                dx -= vector.X * (1 - (float)Math.Abs(Math.Cos(angleY)));
                dy -= vector.Y * (1 - (float)Math.Abs(Math.Cos(angleX)));


                float x = vector.X + dx;
                float y = vector.Y + dy;

                resultPath.Vectors.Add(new Vector3(x, y, z));
            }

            return resultPath;
        }

        private static float ProcessCurvature(float curvature, float deltaRadius)
        {
            float resultCurvature = curvature;
            if (resultCurvature != 0f)
            {
                float radius = 1f / resultCurvature;
                radius += deltaRadius;
                resultCurvature = 1f / radius;
            }
            return resultCurvature;
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
