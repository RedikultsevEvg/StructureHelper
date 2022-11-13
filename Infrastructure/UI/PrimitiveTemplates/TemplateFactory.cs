using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.Templates.RCs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shapes = StructureHelperCommon.Models.Shapes;

namespace StructureHelper.Infrastructure.UI.PrimitiveTemplates
{
    internal static class TemplateFactory
    {
        //public static IEnumerable<PrimitiveBase> RectangleBeam(IRectangleBeamTemplate properties)
        //{
        //    var rect = properties.Shape as Shapes.Rectangle;
        //    var width = rect.Width;
        //    var height = rect.Height;
        //    var area1 = Math.PI * properties.BottomDiameter * properties.BottomDiameter / 4d;
        //    var area2 = Math.PI * properties.TopDiameter * properties.TopDiameter / 4d;
        //    var gap = properties.CoverGap;

            //IHeadMaterial concrete = new HeadMaterial() { Name = "Concrete 40" };
            //concrete.HelperMaterial = Model.HeadMaterialRepository.LibMaterials.Where(x => (x.MaterialType == MaterialTypes.Concrete & x.Name.Contains("40"))).First();
            //IHeadMaterial reinforcement = new HeadMaterial() { Name = "Reinforcement 400" };
            //reinforcement.HelperMaterial = Model.HeadMaterialRepository.LibMaterials.Where(x => (x.MaterialType == MaterialTypes.Reinforcement & x.Name.Contains("400"))).First();
            //headMaterials.Add(concrete);
            //headMaterials.Add(reinforcement);
            //OnPropertyChanged(nameof(headMaterials));

            //yield return new Rectangle(width, height, 0, 0, this) { HeadMaterial = concrete };
            //yield return new Point(area1, -width / 2 + gap, -height / 2 + gap, this) { HeadMaterial = reinforcement };
            //yield return new Point(area1, width / 2 - gap, -height / 2 + gap, this) { HeadMaterial = reinforcement };
            //yield return new Point(area2, -width / 2 + gap, height / 2 - gap, this) { HeadMaterial = reinforcement };
            //yield return new Point(area2, width / 2 - gap, height / 2 - gap, this) { HeadMaterial = reinforcement };
        //}
    }
}
