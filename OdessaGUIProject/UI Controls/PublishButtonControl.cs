using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using NLog;
using OdessaGUIProject.Other_Helpers;
using OdessaGUIProject.Properties;
using OdessaGUIProject.UI_Helpers;
using OdessaGUIProject.Workers;

namespace OdessaGUIProject.UI_Controls
{
    public partial class PublishButtonControl : UserControl
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        internal OdessaGUIProject.Workers.PublishWorker.PublishWorkerTypes PublishWorkerType;
        private HighlightObject currentHighlight;
        private Color progressColor = Color.FromArgb(32, 111, 183);
        private PublishWorker publishWorker;

        public event EventHandler AdvanceTutorial;

        protected virtual void OnAdvanceTutorial()
        {
            EventHandler handler = AdvanceTutorial;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        private enum WorkerState
        {
            Inactive,
            Working,
            Completed,
            Error
        }

        private WorkerState workerState;

        public PublishButtonControl()
        {
            InitializeComponent();
        }

        internal HighlightObject CurrentHighlight
        {
            get { return currentHighlight; }
            set
            {
                currentHighlight = value;

                switch (PublishWorkerType)
                {
                    case OdessaGUIProject.Workers.PublishWorker.PublishWorkerTypes.Facebook:
                        publishWorker = currentHighlight.FacebookShareWorker;
                        iconPicture.Image = Resources.icons_42px_facebook;
                        break;

                    case OdessaGUIProject.Workers.PublishWorker.PublishWorkerTypes.Save:
                        publishWorker = currentHighlight.SaveWorker;
                        iconPicture.Image = Resources.icons_42px_savetocomputer;
                        break;
                }

                // if we're already scanning or saving, let's see what the progress is
                if (publishWorker != null)
                {
                    publishWorker.ProgressChanged += new ProgressChangedEventHandler(publishWorker_ProgressChanged);
                    publishWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(publishWorker_RunWorkerCompleted);
                }

                UpdateUI();
            }
        }

        internal void UnsubscribeToEvents()
        {
            if (publishWorker != null)
            {
                publishWorker.ProgressChanged -= publishWorker_ProgressChanged;
                publishWorker.RunWorkerCompleted -= publishWorker_RunWorkerCompleted;
            }
        }

        private void ButtonClick()
        {
            if (publishWorker == null)
            {
                Publish();
            }
            else
            {
                switch (publishWorker.PublishWorkerResult)
                {
                    case PublishWorker.PublishWorkerResults.Cancelled:
                    case PublishWorker.PublishWorkerResults.UnableToAuthenticate:
                        Publish();
                        break;

                    case PublishWorker.PublishWorkerResults.NotFinished:
                        // Do nothing. User must press Cancel button to cancel
                        break;

                    case PublishWorker.PublishWorkerResults.Success:
                        string errorMessage = "";
                        if (publishWorker.ViewResult(ref errorMessage) == false)
                        {
                            MessageBox.Show(errorMessage, "Error opening highlight", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        break;

                    case PublishWorker.PublishWorkerResults.UnableToShare:
                    case PublishWorker.PublishWorkerResults.UnableToSplice:
                        if (MessageBox.Show("Bummer -- we ran into some trouble. Here's the error message:" + Environment.NewLine + Environment.NewLine +
                            publishWorker.ErrorMessage + Environment.NewLine + Environment.NewLine +
                            "Would you like to try again?",
                            "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                        {
                            Publish();
                        }
                        break;
                }
            }
        }

        private void CancelledClick()
        {
            string message;
            switch (PublishWorkerType)
            {
                case OdessaGUIProject.Workers.PublishWorker.PublishWorkerTypes.Facebook:
                    message = "Cancel upload to Facebook?";
                    break;

                case OdessaGUIProject.Workers.PublishWorker.PublishWorkerTypes.Save:
                    message = "Cancel save?";
                    break;
                default:
                    throw new NotImplementedException();
            }

            if (MessageBox.Show(message, "Cancel?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                System.Windows.Forms.DialogResult.Yes)
            {
                if (publishWorker != null &&
                    publishWorker.IsBusy)
                    publishWorker.CancelAsync();
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            CancelledClick();
        }

        private void iconPictureButtonControl_Click(object sender, EventArgs e)
        {
            ButtonClick();
        }

        private void Publish()
        {
            string backgroundWorkerArgument = "";

            if (PublishWorkerType == PublishWorker.PublishWorkerTypes.Facebook)
            {
                var progress = TutorialHelper.GetTutorialProgress();
                if (progress == TutorialProgress.TutorialShareButton)
                { // don't actually share
                    Logger.Info("Simulating facebook sharing due to tutorial");
                    OnAdvanceTutorial();
                    return;
                }

                // make sure we have access token
                if (Properties.Settings.Default.FacebookAccessToken.Length == 0 ||
                    Properties.Settings.Default.FacebookAccessTokenExpiration < DateTime.Now)
                {
                    if (FacebookHelper.LoginToFacebook() == false)
                    {
                        return;
                    }
                }

                if (CurrentHighlight.FacebookShareWorker != null)
                {
                    CurrentHighlight.FacebookShareWorker.Dispose();
                    CurrentHighlight.FacebookShareWorker = null;
                }

                CurrentHighlight.FacebookShareWorker = new FacebookShareWorker(CurrentHighlight);
                publishWorker = CurrentHighlight.FacebookShareWorker;
            }
            else if (PublishWorkerType == OdessaGUIProject.Workers.PublishWorker.PublishWorkerTypes.Save)
            {
                using (var saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Title = "Where should we save your highlight?";

                    if (Properties.Settings.Default.LastSavedDirectory.Length == 0)
                        saveFileDialog.InitialDirectory = MainModel.GetMyVideosDirectory();
                    else
                        saveFileDialog.InitialDirectory = Properties.Settings.Default.LastSavedDirectory;

                    string originalExtension = CurrentHighlight.InputFileObject.SourceFileInfo.Extension; // includes dot
                    saveFileDialog.FileName = CurrentHighlight.Title + originalExtension;

                    SaveWorker.OutputFormats outputFormat = (SaveWorker.OutputFormats)Enum.Parse(typeof(SaveWorker.OutputFormats), Properties.Settings.Default.SaveOutputFormat);
                    if (outputFormat == SaveWorker.OutputFormats.Original)
                        saveFileDialog.Filter = "Videos (*" + originalExtension + ")|*" + originalExtension + "|All files (*.*)|*.*";
                    else if (outputFormat == SaveWorker.OutputFormats.ProRes)
                        saveFileDialog.Filter = "ProRes Videos (*.mov)|*.mov|All files (*.*)|*.*";

                    if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    Properties.Settings.Default.LastSavedDirectory = Path.GetDirectoryName(saveFileDialog.FileName);
                    Properties.Settings.Default.Save();

                    currentHighlight.Title = Path.GetFileNameWithoutExtension(saveFileDialog.FileName);

                    backgroundWorkerArgument = saveFileDialog.FileName;

                    if (CurrentHighlight.SaveWorker != null)
                    {
                        CurrentHighlight.SaveWorker.Dispose();
                        CurrentHighlight.SaveWorker = null;
                    }

                    CurrentHighlight.SaveWorker = new SaveWorker(CurrentHighlight);
                    CurrentHighlight.SaveWorker.OutputFormat = outputFormat;
                    publishWorker = CurrentHighlight.SaveWorker;
                }
            }

            publishWorker.ProgressChanged += publishWorker_ProgressChanged;
            publishWorker.RunWorkerCompleted += publishWorker_RunWorkerCompleted;
            publishWorker.RunWorkerAsync(backgroundWorkerArgument);

            UpdateUI();
        }

        private void PublishButtonControl_Load(object sender, EventArgs e)
        {
            DesignLanguage.ApplyCustomFont(this.Controls);
        }

        private void publishWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            UpdateUI();
            //UpdateStatusAndProgress(e.UserState as string, e.ProgressPercentage);
        }

        private void publishWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            UpdateUI();
        }

        private void ShowCancelled(string status)
        {
            statusLabel.Text = status;
            this.BackgroundImage = Resources.editpanel_share_element;
            cancelButton.Visible = false;
            statusLabel.Cursor = Cursors.Hand;
            iconPicture.Cursor = Cursors.Hand;
            workerState = WorkerState.Inactive;
        }

        private void ShowCompleted(string status)
        {
            statusLabel.Text = status;
            this.BackgroundImage = Resources.editpanel_share_element_green;
            cancelButton.Visible = false;
            statusLabel.Cursor = Cursors.Hand;
            iconPicture.Cursor = Cursors.Hand;
            workerState = WorkerState.Completed;
        }

        private void ShowError(string status)
        {
            statusLabel.Text = status;
            this.BackgroundImage = Resources.editpanel_share_element_red;
            cancelButton.Visible = false;
            statusLabel.Cursor = Cursors.Hand;
            iconPicture.Cursor = Cursors.Hand;
            workerState = WorkerState.Error;
        }

        private void ShowNotStarted(string status)
        {
            statusLabel.Text = status;
            this.BackgroundImage = Resources.editpanel_share_element;
            cancelButton.Visible = false;
            workerState = WorkerState.Inactive;
        }

        private void ShowWorking(string status)
        {
            statusLabel.Text = status;
            this.BackgroundImage = Resources.editpanel_share_element_blue;
            cancelButton.Visible = true;
            statusLabel.Cursor = Cursors.Default;
            iconPicture.Cursor = Cursors.Default;
            workerState = WorkerState.Working;
        }

        private void statusLabel_Click(object sender, EventArgs e)
        {
            ButtonClick();
        }

        private void UpdateStatusAndProgress(string status, int progress)
        {
            statusLabel.Text = status;
            //progressPictureBox.Width = iconPictureBox.Width + (int)(progress * progressBarWidth / 100.0);
        }

        private void UpdateUI()
        {
            if (PublishWorkerType == OdessaGUIProject.Workers.PublishWorker.PublishWorkerTypes.Facebook)
            {
                if (publishWorker == null)
                {
                    ShowNotStarted("Share highlight to facebook");
                }
                else
                {
                    switch (publishWorker.PublishWorkerResult)
                    {
                        case PublishWorker.PublishWorkerResults.Cancelled:
                            ShowCancelled("Cancelled sharing (click to retry)");
                            break;

                        case PublishWorker.PublishWorkerResults.NotFinished:
                            ShowWorking("Sharing to facebook (" + publishWorker.Progress + "%)...");
                            break;

                        case PublishWorker.PublishWorkerResults.Success:
                            ShowCompleted("Shared to facebook (click to view)");
                            break;

                        case PublishWorker.PublishWorkerResults.UnableToAuthenticate:
                            ShowError("Click to login to facebook");
                            break;

                        case PublishWorker.PublishWorkerResults.UnableToShare:
                        case PublishWorker.PublishWorkerResults.UnableToSplice:
                            ShowError("Error sharing (details...)");
                            break;
                    }
                }
            }
            else if (PublishWorkerType == OdessaGUIProject.Workers.PublishWorker.PublishWorkerTypes.Save)
            {
                if (publishWorker == null)
                {
                    ShowNotStarted("Save highlight as video file");
                }
                else
                {
                    switch (publishWorker.PublishWorkerResult)
                    {
                        case PublishWorker.PublishWorkerResults.Cancelled:
                            ShowCancelled("Cancelled saving (click to retry)");
                            break;

                        case PublishWorker.PublishWorkerResults.NotFinished:
                            ShowWorking("Saving as video file (" + publishWorker.Progress + "%)...");
                            break;

                        case PublishWorker.PublishWorkerResults.Success:
                            ShowCompleted("Saved as video file (click to view)");
                            break;

                        case PublishWorker.PublishWorkerResults.UnableToSplice:
                            ShowError("Error saving (details...)");
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            }
        }

       
        private void statusLabel_MouseEnter(object sender, EventArgs e)
        {
            showHoverState();
        }

        private void showHoverState()
        {
            switch (workerState)
            {
                case WorkerState.Inactive:
                    this.BackgroundImage = Resources.editpanel_share_element_hover;
                    break;
                case WorkerState.Working:
                    // this state isn't clickable so don't show hover state
                    //this.BackgroundImage = Resources.editpanel_share_element_blue_hover;
                    break;
                case WorkerState.Error:
                    this.BackgroundImage = Resources.editpanel_share_element_red_hover;
                    break;
                case WorkerState.Completed:
                    this.BackgroundImage = Resources.editpanel_share_element_green_hover;
                    break;
            }
        }

        private void statusLabel_MouseLeave(object sender, EventArgs e)
        {
            showRestingState();
        }

        private void showRestingState()
        {
            switch (workerState)
            {
                case WorkerState.Inactive:
                    this.BackgroundImage = Resources.editpanel_share_element;
                    break;
                case WorkerState.Working:
                    // this state isn't clickable so don't show hover state
                    //this.BackgroundImage = Resources.editpanel_share_element_blue;
                    break;
                case WorkerState.Error:
                    this.BackgroundImage = Resources.editpanel_share_element_red;
                    break;
                case WorkerState.Completed:
                    this.BackgroundImage = Resources.editpanel_share_element_green;
                    break;
            }
        }

        private void iconPictureButtonControl_MouseEnter(object sender, EventArgs e)
        {
            showHoverState();
        }

        private void iconPictureButtonControl_MouseLeave(object sender, EventArgs e)
        {
            showRestingState();
        }

        private void iconPicture_MouseEnter(object sender, EventArgs e)
        {
            showHoverState();
        }

        private void iconPicture_MouseLeave(object sender, EventArgs e)
        {
            showRestingState();
        }

        private void iconPicture_Click(object sender, EventArgs e)
        {
            ButtonClick();
        }

        
    }
}