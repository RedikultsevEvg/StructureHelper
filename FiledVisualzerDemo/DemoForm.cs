using FieldVisualizer.Entities.Values.Primitives;
using FieldVisualizer.Services.PrimitiveServices;
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
            var primitiveSets = TestPrimitivesOperation.AddTestPrimitives();
            FieldViewerOperation.ShowViewer(primitiveSets);
        }
    }
}
