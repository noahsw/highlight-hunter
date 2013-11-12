using System;
using System.Drawing;
using System.Windows.Forms;
using OdessaGUIProject.Properties;
using OdessaGUIProject.UI_Helpers;
using OdessaGUIProject.Workers;

namespace OdessaGUIProject.UI_Controls
{
    public sealed partial class PublishStatusControl : UserControl
    {
        private readonly HighlightObject _highlightObject;
        private PublishWorker _publishWorker;
        private PublishWorker.PublishWorkerTypes publishWorkerType;

        internal event EventHandler HighlightDetailsOpening;

        internal PublishStatusControl(HighlightObject highlightObject)
        {
            InitializeComponent();

            this.BackColor = Color.Transparent; // so we can see it in the designer

            DesignLanguage.ApplyCustomFont(this.Controls);

            this._highlightObject = highlightObject;

            ShowOpenHighlight();
        }

        internal PublishStatusControl(HighlightObject highlightObject, PublishWorker.PublishWorkerTypes publishWorkerType)
            : this(highlightObject)
        {
            
            this.publishWorkerType = publishWorkerType;

            ConnectEvents();
        }

        internal void ConnectEvents()
        {
            switch (publishWorkerType)
            {
                case PublishWorker.PublishWorkerTypes.Save:
                    _highlightObject.SaveWorkerUpdated += new EventHandler(highlightObject_SaveWorkerUpdated);
                    break;

                case PublishWorker.PublishWorkerTypes.Facebook:
                    _highlightObject.FacebookShareWorkerUpdated += new EventHandler(highlightObject_FacebookShareWorkerUpdated);
                    break;
            }
        }

        private void highlightObject_FacebookShareWorkerUpdated(object sender, EventArgs e)
        {
            _publishWorker = _highlightObject.FacebookShareWorker;
            switch (_publishWorker.PublishWorkerResult)
            {
                case PublishWorker.PublishWorkerResults.Cancelled:
                    ShowCancelled("Cancelled sharing to facebook");
                    break;

                case PublishWorker.PublishWorkerResults.NotFinished:
                    ShowWorking("Sharing to facebook (" + _publishWorker.Progress + "%)");
                    break;

                case PublishWorker.PublishWorkerResults.Success:
                    ShowCompleted("Shared to facebook", new LinkArea(10, 8));
                    break;

                case PublishWorker.PublishWorkerResults.UnableToSplice:
                case PublishWorker.PublishWorkerResults.UnableToShare:
                    ShowError("Error saving to facebook");
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void highlightObject_SaveWorkerUpdated(object sender, EventArgs e)
        {
            _publishWorker = _highlightObject.SaveWorker;
            switch (_publishWorker.PublishWorkerResult)
            {
                case PublishWorker.PublishWorkerResults.Cancelled:
                    ShowCancelled("Cancelled saving as video file");
                    break;

                case PublishWorker.PublishWorkerResults.NotFinished:
                    ShowWorking("Saving as video file (" + _publishWorker.Progress + "%)...");
                    break;

                case PublishWorker.PublishWorkerResults.Success:
                    ShowCompleted("Saved as video file", new LinkArea(9, 10));
                    break;

                case PublishWorker.PublishWorkerResults.UnableToSplice:
                    ShowError("Error saving as video file");
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void ShowWorking(string status)
        {
            statusLinkLabel.Text = status;
            statusLinkLabel.LinkArea = new LinkArea(0, 0);
            statusPictureBox.Image = Resources.review_sharestatus_icon_spinner;
        }

        private void ShowCompleted(string status, LinkArea linkArea)
        {
            statusLinkLabel.Text = status;
            statusLinkLabel.LinkArea = linkArea;
            statusPictureBox.Image = Resources.review_sharestatus_icon_success;
        }

        private void ShowCancelled(string status)
        {
            statusLinkLabel.Text = status;
            statusLinkLabel.LinkArea = new LinkArea(0, 0);
            statusPictureBox.Image = Resources.review_sharestatus_icon_error;
        }

        private void ShowError(string status)
        {
            statusLinkLabel.Text = status;
            statusLinkLabel.LinkArea = new LinkArea(0, 0);
            statusPictureBox.Image = Resources.review_sharestatus_icon_error;
        }

        private void ShowOpenHighlight()
        {
            var status = "Save and share this highlight...";
            statusLinkLabel.Text = status;
            statusLinkLabel.LinkArea = new LinkArea(0, status.Length);
            statusPictureBox.Image = Resources.review_sharestatus_icon_share;
        }

        private void statusLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            TakeAction();
        }

        private void statusLinkLabel_MouseEnter(object sender, EventArgs e)
        {
            statusLinkLabel.LinkColor = DesignLanguage.SecondaryBlue;
        }

        private void statusLinkLabel_MouseLeave(object sender, EventArgs e)
        {
            statusLinkLabel.LinkColor = DesignLanguage.LightGray;
        }

        private void statusPictureBox_Click(object sender, EventArgs e)
        {
            TakeAction();
        }

        private void TakeAction()
        {
            if (_publishWorker == null)
            { // this is a link to open the highlight
                HighlightDetailsOpening(null, null);
            }
            else
            {
                string errorMessage = "";
                if (_publishWorker.ViewResult(ref errorMessage) == false)
                {
                    MessageBox.Show(errorMessage, "Error opening highlight", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}