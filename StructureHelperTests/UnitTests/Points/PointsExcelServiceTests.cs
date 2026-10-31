using NUnit.Framework;
using StructureHelper.Services.Points;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.IO;

namespace StructureHelperTests.UnitTests.Points
{
    [TestFixture]
    public class PointsExcelServiceTests
    {
        [Test]
        public void ExportThenImport_ShouldRoundTripPoints()
        {
            string fileName = Path.Combine(Path.GetTempPath(), $"structurehelper_points_{Guid.NewGuid():N}.xlsx");
            try
            {
                var points = new List<IPoint2D>
                {
                    new Point2D(0.001d, 0.002d),
                    new Point2D(-3.5d, 4.25d)
                };
                var service = new PointsExcelService();

                service.Export(fileName, points);
                IReadOnlyList<Point2D> importedPoints = service.Import(fileName);

                Assert.Multiple(() =>
                {
                    Assert.That(File.Exists(fileName), Is.True);
                    Assert.That(importedPoints, Has.Count.EqualTo(2));
                    Assert.That(importedPoints[0].X, Is.EqualTo(0.001d).Within(1e-12));
                    Assert.That(importedPoints[0].Y, Is.EqualTo(0.002d).Within(1e-12));
                    Assert.That(importedPoints[1].X, Is.EqualTo(-3.5d).Within(1e-12));
                    Assert.That(importedPoints[1].Y, Is.EqualTo(4.25d).Within(1e-12));
                });
            }
            finally
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
            }
        }

        [Test]
        public void Import_WhenFileDoesNotExist_ShouldThrowStructureHelperException()
        {
            var service = new PointsExcelService();
            string fileName = Path.Combine(Path.GetTempPath(), $"structurehelper_missing_{Guid.NewGuid():N}.xlsx");

            Assert.That(() => service.Import(fileName), Throws.TypeOf<StructureHelperException>());
        }
    }
}
