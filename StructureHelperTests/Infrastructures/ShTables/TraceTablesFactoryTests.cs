using NUnit.Framework;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;

namespace StructureHelperTests.Infrastructures.ShTables
{
    [TestFixture]
    public class TraceTablesFactoryTests
    {
        [Test]
        public void GetByPoint2D_WithSinglePoint_ShouldReturnTableWithHeaderAndSinglePointRow()
        {
            // Arrange
            var factory = new TraceTablesFactory();
            var mockPoint = new MockPoint2D(1.0, 2.0);

            // Act
            var result = factory.GetByPoint2D(mockPoint);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Table.RowCount);
            Assert.AreEqual("X", (result.Table.GetCell(0, 0).Value as StringLogEntry).Message);
            Assert.AreEqual("Y", (result.Table.GetCell(0, 1).Value as StringLogEntry).Message);
            Assert.AreEqual("1", (result.Table.GetCell(1, 0).Value as StringLogEntry).Message);
            Assert.AreEqual("2", (result.Table.GetCell(1, 1).Value as StringLogEntry).Message);
        }

        [Test]
        public void GetByPoint2D_WithMultiplePoints_ShouldReturnTableWithHeaderAndMultiplePointRows()
        {
            // Arrange
            var factory = new TraceTablesFactory();
            var mockPoints = new List<IPoint2D>
        {
            new MockPoint2D(1.0, 2.0),
            new MockPoint2D(3.0, 4.0),
            new MockPoint2D(5.0, 6.0)
        };

            // Act
            var result = factory.GetByPoint2D(mockPoints);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Table.RowCount); // Header + 3 Point rows
            Assert.AreEqual("X", (result.Table.GetCell(0, 0).Value as StringLogEntry).Message);
            Assert.AreEqual("Y", (result.Table.GetCell(0, 1).Value as StringLogEntry).Message);
            Assert.AreEqual("1", (result.Table.GetCell(1, 0).Value as StringLogEntry).Message);
            Assert.AreEqual("2", (result.Table.GetCell(1, 1).Value as StringLogEntry).Message);
            Assert.AreEqual("3", (result.Table.GetCell(2, 0).Value as StringLogEntry).Message);
            Assert.AreEqual("4", (result.Table.GetCell(2, 1).Value as StringLogEntry).Message);
            Assert.AreEqual("5", (result.Table.GetCell(3, 0).Value as StringLogEntry).Message);
            Assert.AreEqual("6", (result.Table.GetCell(3, 1).Value as StringLogEntry).Message);
        }

        [Test]
        public void GetByPoint2D_WithEmptyPointsCollection_ShouldReturnTableWithHeaderOnly()
        {
            // Arrange
            var factory = new TraceTablesFactory();
            var emptyPoints = new List<IPoint2D>();

            // Act
            var result = factory.GetByPoint2D(emptyPoints);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Table.RowCount); // Only Header row
            Assert.AreEqual("X", (result.Table.GetCell(0, 0).Value as StringLogEntry).Message);
            Assert.AreEqual("Y", (result.Table.GetCell(0, 1).Value as StringLogEntry).Message);
        }

        // Add more test cases for different scenarios and edge cases
    }

    // Define a mock implementation of IPoint2D for testing purposes
    public class MockPoint2D : IPoint2D
    {
        public MockPoint2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; set; }

        public double Y { get; set; }

        public Guid Id => throw new NotImplementedException();

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}



