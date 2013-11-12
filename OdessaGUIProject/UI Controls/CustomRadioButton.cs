using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using NLog;
using OdessaGUIProject.Properties;
using OdessaGUIProject.UI_Helpers;

namespace OdessaGUIProject
{
    [DefaultEvent("CheckedChanged")]
    public sealed partial class CustomRadioButton : UserControl
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private bool _checked;

        public CustomRadioButton()
        {
            InitializeComponent();

            Checked = false;

            DesignLanguage.ApplyCustomFont(this.Controls);
        }

        public event EventHandler CheckedChanged;

        public bool Checked
        {
            get { return _checked; }
            set
            {
#if DEBUG
                Logger.Trace("Setting state of {0} to {1}", Name, value);
#endif
                bool isCheckedChanged = (_checked != value);

                _checked = value;
                if (_checked)
                {
                    imgState.Image = Resources.radio_selected;

                    // uncheck the sister controls
                    if (Parent != null)
                    {
                        foreach (Control control in Parent.Controls)
                        {
                            if (control == this)
                                continue;

                            if (control is CustomRadioButton)
                                ((CustomRadioButton)control).Checked = false;
                        }
                    }
                }
                else
                {
                    imgState.Image = Resources.radio_unselected;
                }

                if (isCheckedChanged && CheckedChanged != null)
                {
#if DEBUG
                    Logger.Trace("Firing CheckedEvent handler for " + Name);
#endif
                    CheckedChanged(null, null);
                }
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Bindable(true)]
        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                labelMain.Font = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Bindable(true)]
        public override string Text
        {
            get { return base.Text; }

            set
            {
                base.Text = value;
                labelMain.Text = value;
            }
        }

        private void ChangeState()
        {
            if (Enabled && Checked == false)
            {
                Checked = !Checked;
            }
        }

        private void imgState_MouseDown(object sender, MouseEventArgs e)
        {
            //imgState.Image = Properties.Resources.radio_press;
        }

        private void imgState_MouseUp(object sender, MouseEventArgs e)
        {
            ChangeState();
        }

        private void labelMain_MouseDown(object sender, MouseEventArgs e)
        {
            //imgState.Image = Properties.Resources.radio_press;
        }

        private void labelMain_MouseUp(object sender, MouseEventArgs e)
        {
            ChangeState();
        }
    }
}