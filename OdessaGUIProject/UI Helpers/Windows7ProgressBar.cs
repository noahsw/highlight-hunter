using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;

//Windows7ProgressBar v1.0, created by Wyatt O'Day
//Visit: http://wyday.com/windows-7-progress-bar/
//License: http://wyday.com/bsd-license.php

namespace OdessaGUIProject
{
    /// <summary>
    /// The progress bar state for Windows Vista & 7
    /// </summary>
    public enum ProgressBarState
    {
        /// <summary>
        /// Indicates normal progress
        /// </summary>
        Normal = 1,

        /// <summary>
        /// Indicates an error in the progress
        /// </summary>
        Error = 2,

        /// <summary>
        /// Indicates paused progress
        /// </summary>
        Pause = 3
    }

    /// <summary>
    /// A Windows progress bar control with Windows Vista & 7 functionality.
    /// </summary>
    [ToolboxBitmap(typeof(ProgressBar))]
    public class Windows7ProgressBar : ProgressBar
    {
        private ProgressBarState _mState = ProgressBarState.Normal;
        private ContainerControl _ownerForm;
        private bool _showInTaskbar;

        public Windows7ProgressBar() { }

        public Windows7ProgressBar(ContainerControl parentControl)
        {
            ContainerControl = parentControl;
        }

        public ContainerControl ContainerControl
        {
            get { return _ownerForm; }
            set
            {
                _ownerForm = value;

                if (!_ownerForm.Visible)
                    ((Form)_ownerForm).Shown += Windows7ProgressBar_Shown;
            }
        }

        /// <summary>
        /// Show progress in taskbar
        /// </summary>
        [DefaultValue(false)]
        public bool ShowInTaskbar
        {
            get
            {
                return _showInTaskbar;
            }
            set
            {
                if (_showInTaskbar != value)
                {
                    _showInTaskbar = value;

                    // send signal to the taskbar.
                    if (_ownerForm != null)
                    {
                        if (Style != ProgressBarStyle.Marquee)
                            SetValueInTaskBar();

                        SetStateInTaskBar();
                    }
                }
            }
        }

        public override ISite Site
        {
            set
            {
                // Runs at design time, ensures designer initializes ContainerControl
                base.Site = value;
                if (value == null) return;
                var service = value.GetService(typeof(IDesignerHost)) as IDesignerHost;
                if (service == null) return;
                IComponent rootComponent = service.RootComponent;

                ContainerControl = rootComponent as ContainerControl;
            }
        }

        /// <summary>
        /// The progress bar state for Windows Vista & 7
        /// </summary>
        [DefaultValue(ProgressBarState.Normal)]
        public ProgressBarState State
        {
            get { return _mState; }
            set
            {
                _mState = value;

                bool wasMarquee = Style == ProgressBarStyle.Marquee;

                if (wasMarquee)
                    // sets the state to normal (and implicity calls SetStateInTaskBar() )
                    Style = ProgressBarStyle.Blocks;

                // set the progress bar state (Normal, Error, Paused)
                Windows7Taskbar.SendMessage(Handle, 0x410, (int)value, 0);

                if (wasMarquee)
                    // the Taskbar PB value needs to be reset
                    SetValueInTaskBar();
                else
                    // there wasn't a marquee, thus we need to update the taskbar
                    SetStateInTaskBar();
            }
        }

        /// <summary>
        /// Gets or sets the manner in which progress should be indicated on the progress bar.
        /// </summary>
        /// <returns>One of the ProgressBarStyle values. The default is ProgressBarStyle.Blocks</returns>
        public new ProgressBarStyle Style
        {
            get
            {
                return base.Style;
            }
            set
            {
                base.Style = value;

                // set the style of the progress bar
                if (_showInTaskbar && _ownerForm != null)
                {
                    SetStateInTaskBar();
                }
            }
        }

        /// <summary>
        /// Gets or sets the current position of the progress bar.
        /// </summary>
        /// <returns>The position within the range of the progress bar. The default is 0.</returns>
        public new int Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                base.Value = value;

                // send signal to the taskbar.
                SetValueInTaskBar();
            }
        }

        /// <summary>
        /// Advances the current position of the progress bar by the specified amount.
        /// </summary>
        /// <param name="value">The amount by which to increment the progress bar's current position.</param>
        public new void Increment(int value)
        {
            base.Increment(value);

            // send signal to the taskbar.
            SetValueInTaskBar();
        }

        /// <summary>
        /// Advances the current position of the progress bar by the amount of the System.Windows.Forms.ProgressBar.Step property.
        /// </summary>
        public new void PerformStep()
        {
            base.PerformStep();

            // send signal to the taskbar.
            SetValueInTaskBar();
        }

        private void SetStateInTaskBar()
        {
            if (_ownerForm == null) return;

            ThumbnailProgressState thmState = ThumbnailProgressState.Normal;

            if (!_showInTaskbar)
                thmState = ThumbnailProgressState.NoProgress;
            else if (Style == ProgressBarStyle.Marquee)
                thmState = ThumbnailProgressState.Indeterminate;
            else if (_mState == ProgressBarState.Error)
                thmState = ThumbnailProgressState.Error;
            else if (_mState == ProgressBarState.Pause)
                thmState = ThumbnailProgressState.Paused;

            Windows7Taskbar.SetProgressState(_ownerForm.Handle, thmState);
        }

        private void SetValueInTaskBar()
        {
            if (_showInTaskbar)
            {
                var maximum = (ulong)(Maximum - Minimum);
                var progress = (ulong)(Value - Minimum);

                Windows7Taskbar.SetProgressValue(_ownerForm.Handle, progress, maximum);
            }
        }

        private void Windows7ProgressBar_Shown(object sender, System.EventArgs e)
        {
            if (ShowInTaskbar)
            {
                if (Style != ProgressBarStyle.Marquee)
                    SetValueInTaskBar();

                SetStateInTaskBar();
            }

            ((Form)_ownerForm).Shown -= Windows7ProgressBar_Shown;
        }
    }
}