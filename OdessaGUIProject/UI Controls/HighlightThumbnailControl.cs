using System;
using System.Drawing;
using System.Windows.Forms;
using NLog;
using OdessaGUIProject.UI_Helpers;
using OdessaGUIProject.Properties;

namespace OdessaGUIProject.UI_Controls
{
    internal partial class HighlightThumbnailControl : UserControl
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private PublishStatusControl facebookShareWorkerStatusControl;
        private HighlightObject highlightObject;

        private PublishStatusControl saveWorkerStatusControl;

        internal HighlightThumbnailControl()
        {
            Logger.Debug("called");

            InitializeComponent();

            DesignLanguage.ApplyCustomFont(this.Controls);

            /*
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Opaque, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            BackColor = Color.Transparent;
             */
        }

        internal delegate void HighlightRemovedEventHandler(object sender, HighlightEventArgs e);

        internal delegate void HighlightDetailsOpeningEventHandler(object sender, HighlightEventArgs e);

        internal event HighlightRemovedEventHandler HighlightRemoved;

        internal event HighlightDetailsOpeningEventHandler HighlightDetailsOpening;

        internal HighlightObject HighlightObject
        {
            set
            {
                highlightObject = value;

                titleLabel.Text = highlightObject.Title;

                ThumbnailQueueItem thumbnailQueueItem = new ThumbnailQueueItem();
                thumbnailQueueItem.SourceFileInfo = highlightObject.InputFileObject.SourceFileInfo;
                thumbnailQueueItem.SeekInSeconds = highlightObject.BookmarkTime.Subtract(TimeSpan.FromSeconds(5)).TotalSeconds;
                if (thumbnailQueueItem.SeekInSeconds <= 0)
                    thumbnailQueueItem.SeekInSeconds = 1; // default because ffmpeg doesn't like seeking from 0
                thumbnailQueueItem.Size = new Size(270, 153);
                thumbnailQueueItem.ThumbnailGenerated += new ThumbnailQueueItem.ThumbnailGeneratedEventHandler(thumbnailQueueItem_ThumbnailGenerated);
                ThumbnailGenerator.AddToQueue(thumbnailQueueItem);

                highlightObject.TitleChanged += highlightObject_TitleChanged;
                highlightObject.DurationChanged += highlightObject_DurationChanged;

                highlightObject.SaveWorkerCreated += highlightObject_SaveWorkerCreated;
                highlightObject.FacebookShareWorkerCreated += highlightObject_FacebookShareWorkerCreated;

                UpdateDuration();
            }
            get { return highlightObject; }
        }

        internal void ShowPlayOverlay()
        {
            thumbnailBox.Image = Resources.review_video_hover_outlineplay;
            thumbnailMaskPicture.Image = Resources.review_video_thumb_rightside_point_mask_hover;
        }

        private void highlightObject_DurationChanged(object sender, EventArgs e)
        {
            UpdateDuration();
        }

        private void highlightObject_FacebookShareWorkerCreated(object sender, EventArgs e)
        {
            if (facebookShareWorkerStatusControl == null)
            {
                facebookShareWorkerStatusControl = new PublishStatusControl(highlightObject, Workers.PublishWorker.PublishWorkerTypes.Facebook);
                facebookShareWorkerStatusControl.Margin = Padding.Empty;
                statusTableLayoutPanel.Controls.Add(facebookShareWorkerStatusControl);
            }
        }

        private void highlightObject_SaveWorkerCreated(object sender, EventArgs e)
        {
            if (saveWorkerStatusControl == null)
            {
                saveWorkerStatusControl = new PublishStatusControl(highlightObject, Workers.PublishWorker.PublishWorkerTypes.Save);
                saveWorkerStatusControl.Margin = Padding.Empty;
                statusTableLayoutPanel.Controls.Add(saveWorkerStatusControl);
            }
        }

        private void highlightObject_TitleChanged(object sender, EventArgs e)
        {
            titleLabel.Text = highlightObject.Title;
        }

        private void HighlightThumbnailControl_Load(object sender, EventArgs e)
        {
            // to test spacing:
            //var rand = new Random();
            //this.BackColor = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));

            var openHighlightControl = new PublishStatusControl(highlightObject);
            openHighlightControl.HighlightDetailsOpening += new EventHandler(openHighlightControl_HighlightDetailsOpening);
            openHighlightControl.Margin = Padding.Empty;
            statusTableLayoutPanel.Controls.Add(openHighlightControl);
        }

        void openHighlightControl_HighlightDetailsOpening(object sender, EventArgs e)
        {
            if (HighlightDetailsOpening != null)
            {
                HighlightDetailsOpening.Invoke(this, new HighlightEventArgs(HighlightObject));
            }
        }

        private void removePicture_Click(object sender, EventArgs e)
        {
            HighlightRemoved(sender, new HighlightEventArgs(highlightObject));
        }

        private void thumbnailBox_Click(object sender, EventArgs e)
        {
            /* not working because thumbnail controls are created dynamically
            thumbnailBox.Hide();
            videoPlayer.URL = highlightObject.InputFileObject.SourceFileInfo.FullName;
            videoPlayer.CurrentPosition = highlightObject.StartTime.TotalSeconds; // this must come after .URL change
            videoPlayer.Play();
             */

            if (HighlightDetailsOpening != null)
            {
                HighlightDetailsOpening.Invoke(this, new HighlightEventArgs(HighlightObject));
            }
        }

        private void thumbnailBox_MouseEnter(object sender, EventArgs e)
        {
            ShowPlayOverlay();
            
            // We can't figure out how to get the clicking work for removePicture so let's force users to remove through details for now
            // We hacked around this in RawVideoThumbnailControl because the thumbnail didn't need to be clickable
            //removePicture.Visible = true;
        }

        private void thumbnailBox_MouseLeave(object sender, EventArgs e)
        {
            thumbnailBox.Image = null;
            thumbnailMaskPicture.Image = Resources.review_video_thumb_rightside_point_mask;
            removePicture.Visible = false;
        }

        private void thumbnailQueueItem_ThumbnailGenerated(object sender, Image thumbnail)
        {
            if (thumbnail == null)
                return;

            this.BackgroundImage = thumbnail; // set it as background image so we avoid transparency issues

            if (highlightObject.HighlightObjectIndex != 0) // the 0 index will have the overlay shown
                thumbnailBox.Image = null;

        }

        private void titleLabel_Click(object sender, EventArgs e)
        {
            if (HighlightDetailsOpening != null)
            {
                HighlightDetailsOpening.Invoke(this, new HighlightEventArgs(HighlightObject));
            }
        }

        private void UpdateDuration()
        {
            durationLabel.Text = highlightObject.FriendlyDuration;
        }

        /*
        private void removePicture_Click(object sender, EventArgs e)
        {
            if (ThumbnailRemoved != null)
            {
                if (InputFileObject != null)
                    ThumbnailRemoved.Invoke(sender, new ThumbnailClickedEventArgs(InputFileObject));
                else if (HighlightObject != null)
                    ThumbnailRemoved.Invoke(sender, new ThumbnailClickedEventArgs(HighlightObject));
            }
        }
         */

        private void durationLabel_Click(object sender, EventArgs e)
        {
            if (HighlightDetailsOpening != null)
            {
                HighlightDetailsOpening.Invoke(this, new HighlightEventArgs(HighlightObject));
            }
        }
    }
}