﻿using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Ndms;
using System;
using System.Collections.Generic;
using LoaderCalculator.Data.Ndms.Transformations;
using LoaderCalculator.Data.Matrix;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    public class RectangleTriangulationLogic : IRectangleTriangulationLogic
    {
        public ITriangulationLogicOptions Options { get; }

        public IEnumerable<INdm> GetNdmCollection(IMaterial material)
        {
            IRectangleTriangulationLogicOptions rectangleOptions = Options as IRectangleTriangulationLogicOptions;
            double width = rectangleOptions.Rectangle.Width;
            double height = rectangleOptions.Rectangle.Height;
            double ndmMaxSize = rectangleOptions.NdmMaxSize;
            int ndmMinDivision = rectangleOptions.NdmMinDivision;
            LoaderCalculator.Triangulations.RectangleTriangulationLogicOptions logicOptions = new LoaderCalculator.Triangulations.RectangleTriangulationLogicOptions(width, height, ndmMaxSize, ndmMinDivision);
            var logic = LoaderCalculator.Triangulations.Triangulation.GetLogicInstance(logicOptions);
            var ndmCollection = logic.GetNdmCollection(new LoaderCalculator.Data.Planes.RectangularPlane { Material = material });
            double dX = rectangleOptions.Center.X;
            double dY = rectangleOptions.Center.Y;
            NdmTransform.Move(ndmCollection, dX, dY);
            double angle = rectangleOptions.Rectangle.Angle;
            NdmTransform.Rotate(ndmCollection, angle);
            NdmTransform.SetPrestrain(ndmCollection, new StrainMatrix() { Kx = Options.PrestrainKx, Ky = Options.PrestrainKy, EpsZ = Options.PrestrainEpsZ });
            return ndmCollection;
        }

        public void ValidateOptions(ITriangulationLogicOptions options)
        {
            throw new NotImplementedException();
        }

        public RectangleTriangulationLogic(ITriangulationLogicOptions options)
        {
            Options = options;
        }
    }
}
