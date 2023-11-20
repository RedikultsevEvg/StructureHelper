using NUnit.Framework;
using StructureHelper.Windows.Graphs;
using StructureHelperCommon.Models.Parameters;

namespace StructureHelperTests.ViewModelTests
{
    internal class GraphViewModelTest
    {
        [TestCase(2, 3)]
        [TestCase(2, 4)]
        [TestCase(3, 3)]
        public void RunShouldPass(int rowCount, int columnCount)
        {
            //Arrange
            string[] labels = new string[columnCount];
            for (int i = 0; i < columnCount; i++)
            {
                labels[i] = $"Column{i}";
            }
            var array = new ArrayParameter<double>(rowCount, columnCount, labels);
            for (int i = 0; i < columnCount; i++)
            {
                for (int j = 0; j < rowCount; j++)
                {
                    array.Data[j, i] = i + 2 * j;
                }
            }
            //Act
            var vm = new GraphViewModel(array);
            //Assert
            Assert.IsNotNull(vm);
            Assert.AreEqual(columnCount, vm.XItems.Collection.Count());
            Assert.AreEqual(columnCount, vm.YItems.CollectionItems.Count());
        }
    }
}
