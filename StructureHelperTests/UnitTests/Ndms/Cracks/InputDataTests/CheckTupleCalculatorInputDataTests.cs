using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperLogics.Models.Primitives;
using StructureHelperLogics.NdmCalculations.Cracking;
using StructureHelperLogics.NdmCalculations.Primitives;
using System.Collections.Generic;

namespace StructureHelperTests.UnitTests.Ndms.Cracks.InputDataTests;

[TestFixture]
public class CheckTupleCalculatorInputDataTests
{
    private CheckTupleCalculatorInputDataLogic _checkTupleCalculatorInputData;

    [SetUp]
    public void SetUp()
    {
        _checkTupleCalculatorInputData = new CheckTupleCalculatorInputDataLogic();
    }

    [Test]
    public void Check_InputDataIsNull_ReturnsFalse()
    {
        // Arrange
        _checkTupleCalculatorInputData.InputData = null;

        // Act
        var result = _checkTupleCalculatorInputData.Check();

        // Assert
        Assert.IsFalse(result);
        Assert.That(_checkTupleCalculatorInputData.CheckResult, Is.EqualTo(ErrorStrings.ParameterIsNull + ": InputData"));
    }

    [Test]
    public void Check_PrimitivesIsNullOrEmpty_ReturnsFalse()
    {
        // Arrange
        _checkTupleCalculatorInputData.InputData = new TupleCrackInputData
        {
            Primitives = null,
            UserCrackInputData = new UserCrackInputData() // Assuming this is not null
        };

        // Act
        var result = _checkTupleCalculatorInputData.Check();

        // Assert
        Assert.IsFalse(result);
        Assert.That(_checkTupleCalculatorInputData.CheckResult, Is.EqualTo("Collection does not have any primitives"));
    }

    [Test]
    public void Check_UserCrackInputDataIsNull_ReturnsFalse()
    {
        // Arrange
        _checkTupleCalculatorInputData.InputData = new TupleCrackInputData
        {
            Primitives = new List<INdmPrimitive> { new EllipseNdmPrimitive() }, // Assuming at least one valid primitive
            UserCrackInputData = null
        };

        // Act
        var result = _checkTupleCalculatorInputData.Check();

        // Assert
        Assert.IsFalse(result);
        Assert.That(_checkTupleCalculatorInputData.CheckResult, Is.EqualTo("User crack input data is null"));
    }

    [Test]
    public void Check_AllValidInputData_ReturnsTrue()
    {
        // Arrange
        _checkTupleCalculatorInputData.InputData = new TupleCrackInputData
        {
            Primitives = new List<INdmPrimitive> { new EllipseNdmPrimitive() }, // Assuming at least one valid primitive
            UserCrackInputData = new UserCrackInputData() // Assuming this is valid
        };

        // Act
        var result = _checkTupleCalculatorInputData.Check();

        // Assert
        Assert.IsTrue(result);
        Assert.That(_checkTupleCalculatorInputData.CheckResult, Is.EqualTo(string.Empty));
    }
};

