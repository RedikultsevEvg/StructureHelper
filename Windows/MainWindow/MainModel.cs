using LoaderCalculator.Data.Matrix;
using StructureHelper.Services;
using StructureHelper.UnitSystem;
using StructureHelperLogics.Services;
using System.Linq;

namespace StructureHelper.Windows.MainWindow
{
    public class MainModel
    {
        private IPrimitiveRepository primitiveRepository;
        private CalculationService calculationService;
        private UnitSystemService unitSystemService;
        
        public MainModel(IPrimitiveRepository primitiveRepository, CalculationService calculationService, UnitSystemService unitSystemService)
        {
            this.primitiveRepository = primitiveRepository;
            this.calculationService = calculationService;
            this.unitSystemService = unitSystemService;
        }
        
        public IStrainMatrix Calculate(double mx, double my, double nz)
        {
            var unitSystem = unitSystemService.GetCurrentSystem();
            return calculationService.GetPrimitiveStrainMatrix(primitiveRepository.GetPoints()
                .Select(x => x.GetNdmPrimitive(unitSystem))
                .Concat(primitiveRepository.GetRectangles().Select(x => x.GetNdmPrimitive(unitSystem))).ToArray(), mx, my, nz);
        }
    }
}
