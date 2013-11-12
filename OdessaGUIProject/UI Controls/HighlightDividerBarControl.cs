using System.IO;
using System.Windows.Forms;
using OdessaGUIProject.UI_Helpers;

namespace OdessaGUIProject.UI_Controls
{
    public partial class HighlightDividerBarControl : UserControl
    {
        private InputFileObject inputFileObject;

        public HighlightDividerBarControl()
        {
            InitializeComponent();

            DesignLanguage.ApplyCustomFont(this.Controls);
        }

        internal InputFileObject InputFileObject
        {
            get { return inputFileObject; }
            set
            {
                inputFileObject = value;
                SetInputVideoTitle();

                RefreshHighlightCount();
            }
        }

        internal void RefreshHighlightCount()
        {
            var highlightCount = 0;
            foreach (var highlight in MainModel.HighlightObjects)
            {
                if (highlight.InputFileObject == inputFileObject)
                    highlightCount++;
            }

            if (highlightCount == 1)
                highlightCountLabel.Text = "1 highlight found";
            else
                highlightCountLabel.Text = highlightCount + " highlights found";
        }

        private void SetInputVideoTitle()
        {
            string title = Path.GetFileNameWithoutExtension(inputFileObject.SourceFileInfo.FullName);

            inputVideoNameLabel.Text = title;
            //inputVideoNameLabel.AutoSize = true; // so it resizes to appropriate width
            //inputVideoNameLabel.Refresh();
            //inputVideoNameLabel.AutoSize = false;
            //inputVideoNameLabel.Height = this.Height; // vertically align
            dividerLabel.Left = inputVideoNameLabel.Right - 2;
            highlightCountLabel.Left = dividerLabel.Right - 2;
        }
    }
}