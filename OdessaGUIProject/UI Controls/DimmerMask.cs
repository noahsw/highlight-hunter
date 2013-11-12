using System;
using System.Drawing;
using System.Windows.Forms;

namespace OdessaGUIProject.UI_Helpers
{
    internal sealed class DimmerMask : Form
    {
        public DimmerMask(Form parent)
        {
            //InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.Black;
            this.Opacity = 0.70;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.Manual;
            this.Size = parent.ClientSize;
            this.Location = parent.PointToScreen(Point.Empty);
            parent.Move += AdjustPosition;
            parent.SizeChanged += AdjustPosition;
        }

        private void AdjustPosition(object sender, EventArgs e)
        {
            var parent = sender as Form;
            if (parent != null)
            {
                this.Location = parent.PointToScreen(Point.Empty);
                this.ClientSize = parent.ClientSize;
            }
        }
    }
}