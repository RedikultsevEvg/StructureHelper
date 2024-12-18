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
            Assert.That(result, Is.Not.Null);
            Assert.That(2, Is.EqualTo(result.Table.RowCount));
            Assert.That((result.Table.GetCell(0, 0).Value as StringLogEntry).Message, Is.EqualTo("X"));
            Assert.That((result.Table.GetCell(0, 1).Value as StringLogEntry).Message, Is.EqualTo("Y"));
            Assert.That((result.Table.GetCell(1, 0).Value as StringLogEntry).Message, Is.EqualTo("1"));
            Assert.That((result.Table.GetCell(1, 1).Value as StringLogEntry).Message, Is.EqualTo("2"));
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
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Table.RowCount, Is.EqualTo(4));// Header + 3 Point rows
            Assert.That((result.Table.GetCell(0, 0).Value as StringLogEntry).Message, Is.EqualTo("X"));
            Assert.That((result.Table.GetCell(0, 1).Value as StringLogEntry).Message, Is.EqualTo("Y"));
            Assert.That((result.Table.GetCell(1, 0).Value as StringLogEntry).Message, Is.EqualTo("1"));
            Assert.That((result.Table.GetCell(1, 1).Value as StringLogEntry).Message, Is.EqualTo("2"));
            Assert.That((result.Table.GetCell(2, 0).Value as StringLogEntry).Message, Is.EqualTo("3"));
            Assert.That((result.Table.GetCell(2, 1).Value as StringLogEntry).Message, Is.EqualTo("4"));
            Assert.That((result.Table.GetCell(3, 0).Value as StringLogEntry).Message, Is.EqualTo("5"));
            Assert.That((result.Table.GetCell(3, 1).Value as StringLogEntry).Message, Is.EqualTo("6"));
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
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Table.RowCount, Is.EqualTo(1));// Only Header row
            Assert.That((result.Table.GetCell(0, 0).Value as StringLogEntry).Message, Is.EqualTo("X"));
            Assert.That((result.Table.GetCell(0, 1).Value as StringLogEntry).Message, Is.EqualTo("Y"));
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



