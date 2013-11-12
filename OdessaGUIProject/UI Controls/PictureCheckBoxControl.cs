using System;
using System.ComponentModel;
using System.Windows.Forms;
using OdessaGUIProject.Properties;

namespace OdessaGUIProject.UI_Controls
{
    [DefaultEvent("CheckedChanged")]
    public partial class PictureCheckBoxControl : UserControl
    {
        private bool isChecked;

        public PictureCheckBoxControl()
        {
            InitializeComponent();
        }

        public event EventHandler CheckedChanged;

        public bool Checked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                if (isChecked)
                    BackgroundImage = Resources.settings_switch_on;
                else
                    BackgroundImage = Resources.settings_switch_off;
            }
        }

        private void PictureCheckBoxControl_Click(object sender, EventArgs e)
        {
            Checked = !Checked;

            if (CheckedChanged != null)
                CheckedChanged(sender, e);
        }

        private void PictureCheckBoxControl_Load(object sender, EventArgs e)
        {
        }
    }
}