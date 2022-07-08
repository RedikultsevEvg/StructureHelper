namespace StructureHelperLogics.NdmCalculations.Entities.LibraryDecorators
{
    public interface INdmDecorator
    {
        double Area { get; set; }
        double Prestrain { get; set; }
        double X { get; set; }
        double Y { get; set; }
        IMaterialDecorator Material { get; set; }
        object UserData { get; set; }

        LoaderCalculator.Data.Ndms.INdm GetNdm();
    }
}