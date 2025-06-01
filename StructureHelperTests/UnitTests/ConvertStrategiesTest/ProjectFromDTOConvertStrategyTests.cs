using DataAccess.DTOs;
using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Analyses;
using StructureHelperCommon.Models.Projects;

namespace StructureHelperTests.UnitTests.ConvertStrategiesTest
{


    [TestFixture]
    public class ProjectFromDTOConvertStrategyTests
    {
        private Mock<IUpdateStrategy<IProject>> _mockUpdateStrategy;
        private Mock<IDictionaryConvertStrategy<IVisualAnalysis, IVisualAnalysis>> _mockConvertLogic;
        private Mock<IShiftTraceLogger> _mockTraceLogger;
        private Dictionary<(Guid id, Type type), ISaveable> _referenceDictionary;
        private ProjectFromDTOConvertStrategy _convertStrategy;

        [SetUp]
        public void SetUp()
        {
            _mockUpdateStrategy = new Mock<IUpdateStrategy<IProject>>();
            _mockConvertLogic = new Mock<IDictionaryConvertStrategy<IVisualAnalysis, IVisualAnalysis>>();
            _mockTraceLogger = new Mock<IShiftTraceLogger>();
            _referenceDictionary = new Dictionary<(Guid, Type), ISaveable>();

            _convertStrategy = new ProjectFromDTOConvertStrategy(
                _mockUpdateStrategy.Object,
                _mockConvertLogic.Object,
                _referenceDictionary,
                _mockTraceLogger.Object);
        }

        [Test]
        public void GetNewItem_ShouldLogStartAndEndMessages()
        {
            // Arrange
            var projectDto = new ProjectDTO(Guid.Empty)
            {
                VisualAnalyses = new List<IVisualAnalysis> { new Mock<IVisualAnalysis>().Object }
            };

            _mockConvertLogic
                .Setup(s => s.Convert(It.IsAny<IVisualAnalysis>()))
                .Returns(new Mock<IVisualAnalysis>().Object);

            // Act
            var result = _convertStrategy.GetNewItem(projectDto);

            // Assert
            _mockTraceLogger.Verify(logger => logger.AddMessage("Converting of project is started"), Times.Once);
            _mockTraceLogger.Verify(logger => logger.AddMessage("Converting of project has been finished successfully"), Times.Once);
        }

        [Test]
        public void GetNewItem_ShouldLogWarningIfNoAnalyses()
        {
            // Arrange
            var projectDto = new ProjectDTO(Guid.Empty)
            {
                VisualAnalyses = Enumerable.Empty<IVisualAnalysis>().ToList()
            };

            // Act
            var result = _convertStrategy.GetNewItem(projectDto);

            // Assert
            _mockTraceLogger.Verify(logger => logger.AddMessage("Project does not have any analyses, it is possible to work with project", TraceLogStatuses.Warning), Times.Once);
        }

        [Test]
        public void GetAnalyses_ShouldConvertEachVisualAnalysisAndLogCount()
        {
            // Arrange
            var projectDto = new ProjectDTO(Guid.Empty)
            {
                VisualAnalyses = new List<IVisualAnalysis> { new Mock<IVisualAnalysis>().Object, new Mock<IVisualAnalysis>().Object }
            };
            var newItem = new Project();

            _mockConvertLogic
                .Setup(s => s.Convert(It.IsAny<IVisualAnalysis>()))
                .Returns(new Mock<IVisualAnalysis>().Object);

            // Act
            var analyses = _convertStrategy.GetAnalyses(projectDto, newItem);

            // Assert
            Assert.That(analyses.Count, Is.EqualTo(projectDto.VisualAnalyses.Count));
            _mockConvertLogic.Verify(s => s.Convert(It.IsAny<IVisualAnalysis>()), Times.Exactly(projectDto.VisualAnalyses.Count));
            _mockTraceLogger.Verify(logger => logger.AddMessage($"Source project has {projectDto.VisualAnalyses.Count} analyses"), Times.Once);
        }

        [Test]
        public void GetAnalyses_ShouldLogConvertedAnalysisCount()
        {
            // Arrange
            var projectDto = new ProjectDTO(Guid.Empty)
            {
                VisualAnalyses = new List<IVisualAnalysis> { new Mock<IVisualAnalysis>().Object }
            };
            var newItem = new Project();

            _mockConvertLogic
                .Setup(s => s.Convert(It.IsAny<IVisualAnalysis>()))
                .Returns(new Mock<IVisualAnalysis>().Object);

            // Act
            var result = _convertStrategy.GetNewItem(projectDto);

            // Assert
            _mockTraceLogger.Verify(logger => logger.AddMessage("Totaly 1 were(was) obtained"), Times.Once);
        }
    }

}
