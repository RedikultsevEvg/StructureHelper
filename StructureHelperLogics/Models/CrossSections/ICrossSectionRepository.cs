using StructureHelper.Models.Materials;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.Materials;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.Models.CrossSections
{
    /// <summary>
    /// Repository of members of cross-section
    /// </summary>
    public interface ICrossSectionRepository : ISaveable, IHasHeadMaterials, IHasForcesAndPrimitives, IHasCalculators, ISelfCloneStrategy<ICrossSectionRepository>
    {
        /// <summary>
        /// Operations for removing force actions and NDM primitives
        /// </summary>
        IRepositoryOperationsLogic Operations { get; }

        /// <summary>
        /// Adds a head material. Null items, duplicate IDs and duplicate instances are rejected.
        /// </summary>
        void AddHeadMaterial(IHeadMaterial item);

        /// <summary>
        /// Removes a head material if it is not referenced by any primitive.
        /// </summary>
        bool RemoveHeadMaterial(IHeadMaterial item);

        /// <summary>
        /// Removes a head material by ID if it is not referenced by any primitive.
        /// </summary>
        bool RemoveHeadMaterial(Guid id);

        /// <summary>
        /// Adds a force action. Null items, duplicate IDs and duplicate instances are rejected.
        /// </summary>
        void AddForceAction(IForceAction item);

        /// <summary>
        /// Removes a force action and cleans calculator input data. The operation is idempotent.
        /// </summary>
        bool RemoveForceAction(IForceAction item);

        /// <summary>
        /// Removes a force action by ID and cleans calculator input data. The operation is idempotent.
        /// </summary>
        bool RemoveForceAction(Guid id);

        /// <summary>
        /// Adds an NDM primitive. Null items, duplicate IDs and duplicate instances are rejected.
        /// </summary>
        void AddPrimitive(INdmPrimitive item);

        /// <summary>
        /// Removes an NDM primitive, detaches hosted primitives and cleans calculator input data.
        /// </summary>
        bool RemovePrimitive(INdmPrimitive item);

        /// <summary>
        /// Removes an NDM primitive by ID, detaches hosted primitives and cleans calculator input data.
        /// </summary>
        bool RemovePrimitive(Guid id);

        /// <summary>
        /// Adds a calculator. Null items, duplicate IDs and duplicate instances are rejected.
        /// </summary>
        void AddCalculator(ICalculator item);

        /// <summary>
        /// Removes a calculator without removing its input data. The operation is idempotent.
        /// </summary>
        bool RemoveCalculator(ICalculator item);

        /// <summary>
        /// Removes a calculator by ID without removing its input data. The operation is idempotent.
        /// </summary>
        bool RemoveCalculator(Guid id);

        /// <summary>
        /// Clears calculators, force actions, primitives and materials, including calculator input data.
        /// </summary>
        void ClearAll();
    }
}
