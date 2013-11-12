using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OdessaGUIProject.UI_Helpers;

namespace OdessaGUIProject.UI_Controls
{
    public partial class TutorialEscapeHatch : UserControl
    {
        public TutorialEscapeHatch()
        {
            InitializeComponent();

            DesignLanguage.ApplyCustomFont(this);
            //progressLabel.ForeColor = DesignLanguage.LightGray;

        }

        public event EventHandler TutorialExited;

        public void ExitTutorial()
        {
            TutorialHelper.Finish();
            OnTutorialExited();
        }

        public void RefreshProgress()
        {
            var progress = TutorialHelper.GetTutorialProgress();
            progressLabel.Text = "You are taking the Highlight Hunter tour (step " + ((int)progress + 1) + " of " +
                                 (int)TutorialProgress.TutorialFinished + ")";
        }

        protected virtual void OnTutorialExited()
        {
            EventHandler handler = TutorialExited;
            if (handler != null) handler(this, EventArgs.Empty);
        }
        private void exitTourButton_Click(object sender, EventArgs e)
        {
            ExitTutorial();
        }

        private void TutorialEscapeHatch_Load(object sender, EventArgs e)
        {
            RefreshProgress();
        }
    }
}
