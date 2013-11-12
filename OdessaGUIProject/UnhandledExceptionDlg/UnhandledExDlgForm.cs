using System;
using System.Windows.Forms;

namespace Zayko.Dialogs.UnhandledExceptionDlg
{
    public partial class UnhandledExDlgForm : Form
    {
        public UnhandledExDlgForm()
        {
            InitializeComponent();
        }

        private void UnhandledExDlgForm_Load(object sender, EventArgs e)
        {
            buttonNotSend.Focus();
            labelExceptionDate.Text = String.Format(labelExceptionDate.Text, DateTime.Now);
            linkLabelData.Left = labelLinkTitle.Right;
        }
    }
}