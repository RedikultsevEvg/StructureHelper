using FieldVisualizer.Entities.Values.Primitives;
using FieldVisualizer.WindowsOperation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FiledVisualzerDemo
{
    public partial class DemoForm : Form
    {
        public DemoForm()
        {
            InitializeComponent();
        }

        private void RectanglesSetDemo_Click(object sender, EventArgs e)
        {
            List<IPrimitiveSet> primitiveSets = new List<IPrimitiveSet>();
            int imax = 100;
            int jmax = 100;
            PrimitiveSet primitiveSet = new PrimitiveSet();
            primitiveSets.Add(primitiveSet);
            List<IValuePrimitive> primitives = new List<IValuePrimitive>();
            primitiveSet.ValuePrimitives = primitives;
            IValuePrimitive primitive;
            for (int i = 0; i < imax; i++)
            {
                for (int j = 0; j < jmax; j++)
                {
                    primitive = new RectanglePrimitive() { Height = 10, Width = 20, CenterX = 20 * i, CenterY = 10 * j, Value = -(i + j) };
                    primitives.Add(primitive);
                }
            }
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    primitive = new CirclePrimitive() { Diameter = 150, CenterX = -200 * i, CenterY = -100 * j, Value = i * 100 + j * 200 };
                    primitives.Add(primitive);
                }
            }
            primitiveSet = new PrimitiveSet();
            primitives = new List<IValuePrimitive>();
            primitive = new RectanglePrimitive() { Height = 100, Width = 200, CenterX = 0, CenterY = 0, Value = 100 };
            primitives.Add(primitive);
            primitive = new CirclePrimitive() { Diameter = 50, CenterX = 0, CenterY = 0, Value = -100 };
            primitives.Add(primitive);
            primitiveSet.ValuePrimitives = primitives;
            primitiveSets.Add(primitiveSet);
            FieldViewerOperation.ShowViewer(primitiveSets);
        }
    }
}
