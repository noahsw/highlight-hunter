using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using NLog;
using OdessaGUIProject.DRM_Helpers;
using OdessaGUIProject.UI_Helpers;
using OdessaGUIProject.Properties;

namespace OdessaGUIProject.UI_Controls
{
    internal partial class RawVideoThumbnailControl : UserControl
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private InputFileObject inputFileObject;

        private bool isSampleVideo;

        //internal delegate void ThumbnailClickedEventHandler(object sender, InputFileObjectEventArgs e);
        //internal event ThumbnailClickedEventHandler ThumbnailClicked;

        private Size thumbnailSize = new Size(272, 153);

        internal RawVideoThumbnailControl()
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

        internal delegate void ThumbnailRemovedEventHandler(object sender, InputFileObjectEventArgs e);

        internal event ThumbnailRemovedEventHandler ThumbnailRemoved;

        internal InputFileObject InputFileObject
        {
            set
            {
                inputFileObject = value;
                titleLabel.Text = Path.GetFileNameWithoutExtension(inputFileObject.SourceFileInfo.FullName);
                isSampleVideo = (MainModel.GetPathToSampleVideo() == inputFileObject.SourceFileInfo.FullName);

                var thumbnailQueueItem = new ThumbnailQueueItem();
                thumbnailQueueItem.SourceFileInfo = inputFileObject.SourceFileInfo;
                thumbnailQueueItem.Size = thumbnailSize;
                thumbnailQueueItem.ThumbnailGenerated += new ThumbnailQueueItem.ThumbnailGeneratedEventHandler(thumbnailQueueItem_ThumbnailGenerated);
                ThumbnailGenerator.AddToQueue(thumbnailQueueItem);
            }
            get { return inputFileObject; }
        }

        public void UpdateTitle(string title)
        {
            titleLabel.Text = title;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (this.ClientRectangle.Contains(this.PointToClient(Control.MousePosition)))
                return;

            base.OnMouseLeave(e);
        }

        private void RawVideoThumbnailControl_MouseEnter(object sender, EventArgs e)
        {
            if (isSampleVideo)
            {
                var progress = TutorialHelper.GetTutorialProgress();
                if (progress != TutorialProgress.TutorialFinished)
                    return; // no remove button available
            }

            removePicture.Visible = true;
        }

        private void RawVideoThumbnailControl_MouseLeave(object sender, EventArgs e)
        {
            removePicture.Visible = false;
        }

        private void RawVideoThumbnailControl_Paint(object sender, PaintEventArgs e)
        {
            if (this.BackgroundImage == null)
            {
                // draw question mark
                var p = new Point((this.Width - Resources.review_novideos_question_mark_icon.Width) / 2,
                    (thumbnailSize.Height - Resources.review_novideos_question_mark_icon.Height) / 2);

                e.Graphics.DrawImageUnscaled(Resources.review_novideos_question_mark_icon, p);
            }
        }

        private void removePicture_Click(object sender, EventArgs e)
        {
            if (ThumbnailRemoved != null)
            {
                ThumbnailRemoved.Invoke(sender, new InputFileObjectEventArgs(InputFileObject));
            }
        }

        private void thumbnailBox_MouseEnter(object sender, EventArgs e)
        {
            removePicture.Visible = true;
            removePicture.BringToFront();
        }

        private void thumbnailBox_MouseLeave(object sender, EventArgs e)
        {
            removePicture.Visible = false;
        }

        private void thumbnailQueueItem_ThumbnailGenerated(object sender, Image thumbnail)
        {
            if (thumbnail == null)
                return;

            this.BackgroundImage = thumbnail;
        }
    }
}