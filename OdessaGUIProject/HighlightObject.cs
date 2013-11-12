using System;
using System.Globalization;
using System.IO;
using OdessaGUIProject.Workers;

namespace OdessaGUIProject
{
    internal class HighlightObject
    {
        private const string DefaultTitle = "My highlight";

        private TimeSpan endTime;

        private TimeSpan startTime;

        private string title;

        internal HighlightObject()
        {
            Title = DefaultTitle;

            HighlightObjectIndex = MainModel.HighlightObjects.Count;
        }

        internal event EventHandler DurationChanged;

        internal event EventHandler TitleChanged;

        internal TimeSpan BookmarkTime { get; set; }

        internal TimeSpan EndTime
        {
            get { return endTime; }
            set
            {
                endTime = value;
                if (DurationChanged != null)
                    DurationChanged(null, null);
            }
        }

        internal bool HasBeenReviewed { get; set; }

        /// <summary>
        /// 0-based
        /// Used for displaying "1 out of 20" on Highlight Details
        /// </summary>
        internal int HighlightObjectIndex { get; set; }

        internal InputFileObject InputFileObject { get; set; }

        internal TimeSpan StartTime
        {
            get { return startTime; }
            set
            {
                startTime = value;
                if (DurationChanged != null)
                    DurationChanged(null, null);
            }
        }

        internal string Title
        {
            get { return title; }
            set
            {
                title = value;
                if (TitleChanged != null)
                    TitleChanged(null, null);
            }
        }

        #region SaveWorker

        // Save to computer
        private SaveWorker saveWorker;

        internal event EventHandler SaveWorkerCreated;

        internal event EventHandler SaveWorkerUpdated;

        internal SaveWorker SaveWorker
        {
            get { return saveWorker; }
            set
            {
                if (value == null)
                {
                    saveWorker.ProgressChanged -= saveWorker_ProgressChanged;
                    saveWorker.RunWorkerCompleted -= saveWorker_RunWorkerCompleted;
                }

                saveWorker = value;

                if (value != null)
                {
                    HasBeenReviewed = true;
                    if (SaveWorkerCreated != null)
                        SaveWorkerCreated(null, EventArgs.Empty);
                    if (SaveWorkerUpdated != null)
                        SaveWorkerUpdated(null, EventArgs.Empty);
                    saveWorker.ProgressChanged += saveWorker_ProgressChanged;
                    saveWorker.RunWorkerCompleted += saveWorker_RunWorkerCompleted;
                }
            }
        }

        private void saveWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            if (SaveWorkerUpdated != null)
                SaveWorkerUpdated(null, EventArgs.Empty);
        }

        private void saveWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (SaveWorkerUpdated != null)
                SaveWorkerUpdated(null, EventArgs.Empty);
        }

        #endregion SaveWorker

        #region FacebookShareWorker

        private FacebookShareWorker facebookShareWorker;

        internal event EventHandler FacebookShareWorkerCreated;

        internal event EventHandler FacebookShareWorkerUpdated;

        // Share to Facebook
        /// <summary>
        /// Set when Facebook upload is done. Go to www.facebook.com/{activityId} to open video.
        /// Leave this here so we aren't dependent on keeping the FacebookShareWorker around
        /// </summary>
        internal string FacebookActivityId { get; set; }

        internal FacebookShareWorker FacebookShareWorker
        {
            get { return facebookShareWorker; }
            set
            {
                if (value == null)
                {
                    facebookShareWorker.ProgressChanged -= facebookShareWorker_ProgressChanged;
                    facebookShareWorker.RunWorkerCompleted -= facebookShareWorker_RunWorkerCompleted;
                }

                facebookShareWorker = value;

                if (value != null)
                {
                    HasBeenReviewed = true;
                    if (FacebookShareWorkerCreated != null)
                        FacebookShareWorkerCreated(null, null);
                    if (FacebookShareWorkerUpdated != null)
                        FacebookShareWorkerUpdated(null, null);

                    facebookShareWorker.ProgressChanged += facebookShareWorker_ProgressChanged;
                    facebookShareWorker.RunWorkerCompleted += facebookShareWorker_RunWorkerCompleted;
                }
            }
        }

        private void facebookShareWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            if (FacebookShareWorkerUpdated != null)
                FacebookShareWorkerUpdated(null, null);
        }

        private void facebookShareWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (FacebookShareWorkerUpdated != null)
                FacebookShareWorkerUpdated(null, null);
        }

        #endregion FacebookShareWorker

        internal void GenerateHighlightTitle()
        {
            int highlightCount = 1;
            foreach (var highlight in MainModel.HighlightObjects)
            {
                if (highlight.InputFileObject == InputFileObject)
                {
                    highlight.Title = Path.GetFileNameWithoutExtension(highlight.InputFileObject.SourceFileInfo.Name) +
                        " highlight #" + highlightCount++;
                }
            }

            if (highlightCount > 1)
                Title = Path.GetFileNameWithoutExtension(InputFileObject.SourceFileInfo.Name) +
                    " highlight #" + highlightCount;
            else
                Title = Path.GetFileNameWithoutExtension(InputFileObject.SourceFileInfo.Name) +
                    " highlight";
        }

        internal bool IsDefaultTitle()
        {
            return (Title == DefaultTitle);
        }

        #region Friendly strings

        internal string FriendlyBookmarkTime
        {
            get
            {
                return Math.Floor(BookmarkTime.TotalMinutes) + ":" + BookmarkTime.Seconds.ToString("00", CultureInfo.CurrentCulture);
            }
        }

        internal string FriendlyDuration
        {
            get
            {
                var duration = EndTime - StartTime;
                return Math.Floor(duration.TotalMinutes) + ":" + duration.Seconds.ToString("00", CultureInfo.CurrentCulture);
                //return Math.Round((EndTime - StartTime).TotalSeconds, 0).ToString() + " secs";
            }
        }

        internal string FriendlyEndTime
        {
            get
            {
                return Math.Floor(EndTime.TotalMinutes) + ":" + EndTime.Seconds.ToString("00", CultureInfo.CurrentCulture);
            }
        }

        internal string FriendlyStartTime
        {
            get
            {
                return Math.Floor(StartTime.TotalMinutes) + ":" + StartTime.Seconds.ToString("00", CultureInfo.CurrentCulture);
            }
        }

        #endregion Friendly strings
    }
}