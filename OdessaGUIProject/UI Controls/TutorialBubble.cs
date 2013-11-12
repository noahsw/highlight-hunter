using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using OdessaGUIProject.UI_Helpers;

namespace OdessaGUIProject.UI_Controls
{
    [DefaultEvent("Advance")]
    public sealed partial class TutorialBubble : UserControl
    {
        public event EventHandler Advance;
        private TutorialProgress _tutorialProgress;

        public TutorialBubble()
        {
            _tutorialProgress = TutorialProgress.TutorialAddSampleVideo;

            InitializeComponent();

            BackColor = Color.Transparent;

            nextButton.BackColor = Color.FromArgb(187, 187, 187); // color of tooltip gray

            DesignLanguage.ApplyCustomFont(this);
        }

        public TutorialProgress TutorialProgress
        {
            get { return _tutorialProgress; }
            set
            {
                _tutorialProgress = value;
                LoadTooltip();
            }
        }

        

        internal void OnAdvance()
        {
            EventHandler handler = Advance;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        /*
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle = 0x00000020; //WS_EX_TRANSPARENT
                return cp;
            }
        }
        
        protected override void OnMove(EventArgs e)
        {
            RecreateHandle();
        }
         

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            //do nothing
        }
        */
        protected override void OnPaint(PaintEventArgs e)
        {
            PaintParentBackground(e);
        }

        private void PaintParentBackground(PaintEventArgs e)
        {
            if (Parent != null)
            {
                var rect = new Rectangle(Left, Top, Width, Height);

                e.Graphics.TranslateTransform(-rect.X, -rect.Y);

                try
                {
                    using (var pea = new PaintEventArgs(e.Graphics, rect))
                    {
                        pea.Graphics.SetClip(rect);
                        InvokePaintBackground(Parent, pea);
                        InvokePaint(Parent, pea);
                    }
                }
                finally
                {
                    e.Graphics.TranslateTransform(rect.X, rect.Y);
                }
            }
            else
            {
                e.Graphics.FillRectangle(SystemBrushes.Control, ClientRectangle);
            }
        }

        private static int GetOriginXForCenterPoint(Control anchorControl, Control slaveControl)
        {
            if (anchorControl == null)
                return 0;

            if (slaveControl == null)
                return 0;

            return (anchorControl.Width - slaveControl.Width) / 2;
        }

        private string GetBubbleText()
        {
            switch (_tutorialProgress)
            {
                case TutorialProgress.TutorialAddSampleVideo:
                    return
                        "Here is a sample video for you to try out.\n\nJustin marked two separate highlights by covering the lens after each one.";

                case TutorialProgress.TutorialScanButton:
                    return
                        "Click \"Scan All Videos\" to scan the sample video for highlights.\n\nIf this were your own footage, every video you dropped in would be scanned.";

                case TutorialProgress.TutorialHighlightsFound:
                    return
                        "These are the highlights that were detected in the footage.\n\nClick this first highlight to edit and share it.";

                case TutorialProgress.TutorialBookmarkFlag:
                    return
                        "This marker represents when the lens was covered.\n\nBy default, the highlight ends just before this marker.";

                case TutorialProgress.TutorialHandles:
                    return
                        "Move these handles to adjust where the highlight starts and ends.\n\nDrag each handle to the very end to include even more video.";

                case TutorialProgress.TutorialShareButton:
                    return
                        "Click this button to share the highlight to Facebook.\n\n(Don't worry, we won't really share this demo highlight).";

                case TutorialProgress.TutorialCloseDetails:
                    return
                        "Nice work. You've completed the tutorial!\n\nClose this window when you're ready to move on.";

                case TutorialProgress.TutorialStartOver:
                    return "Click \"Start over\" to go back to the beginning.\n\nIt's time to use your own footage!";

                default:
                    Debug.Assert(false, "Invalid progress");
                    return "";
            }
        }

        private Point GetNextButtonLocation()
        {
            int x = GetOriginXForCenterPoint(this, nextButton);
            int y;

            switch (GetTooltipDirection())
            {
                case TooltipDirection.Bottom:
                case TooltipDirection.BottomLeft:
                    y = 136;
                    break;

                case TooltipDirection.Left:
                    y = 136;
                    break;

                default:
                    Debug.Assert(false, "Unaccounted case");
                    y = 0;
                    break;
            }

            return new Point(x, y);
        }

        private Point GetTextLocation()
        {
            var isNextButtonVisible = IsNextButtonVisible();
            var tooltipDirection = GetTooltipDirection();

            if (isNextButtonVisible)
            {
                switch (tooltipDirection)
                {
                    case TooltipDirection.Bottom:
                    case TooltipDirection.BottomLeft:
                        return new Point(10, 10);
                    case TooltipDirection.Left:
                        return new Point(20, 10);
                    default:
                        Debug.Assert(false, "Unaccounted case");
                        return new Point(0, 0);
                }
            }

            switch (tooltipDirection)
            {
                case TooltipDirection.Bottom:
                    return new Point(10, 10);
                case TooltipDirection.Left:
                    return new Point(20, 10);
                case TooltipDirection.TopRight:
                    return new Point(8, 20);
                default:
                    Debug.Assert(false, "Unaccounted case");
                    return new Point(0, 0);
            }
        }

        private TooltipDirection GetTooltipDirection()
        {
            switch (_tutorialProgress)
            {
                case TutorialProgress.TutorialAddSampleVideo:
                    return TooltipDirection.Left;
                case TutorialProgress.TutorialScanButton:
                    return TooltipDirection.Bottom;
                case TutorialProgress.TutorialHighlightsFound:
                    return TooltipDirection.Left;
                case TutorialProgress.TutorialBookmarkFlag:
                    return TooltipDirection.Bottom;
                case TutorialProgress.TutorialHandles:
                    return TooltipDirection.BottomLeft;
                case TutorialProgress.TutorialShareButton:
                    return TooltipDirection.Bottom;
                case TutorialProgress.TutorialCloseDetails:
                    return TooltipDirection.TopRight;
                case TutorialProgress.TutorialStartOver:
                    return TooltipDirection.Bottom;
                default:
                    Debug.Assert(false, "Invalid progress");
                    return TooltipDirection.Bottom;
            }
        }

        private Image GetTooltipImage()
        {
            var isNextButtonVisible = IsNextButtonVisible();
            var tooltipDirection = GetTooltipDirection();

            if (isNextButtonVisible)
            {
                switch (tooltipDirection)
                {
                    case TooltipDirection.Bottom:
                        return Properties.Resources.tooltip_large_bottom;

                    case TooltipDirection.Left:
                        return Properties.Resources.tooltip_large_left;

                    case TooltipDirection.BottomLeft:
                        return Properties.Resources.tooltip_large_bottomleft;
                    default:
                        Debug.Assert(false, "Image not set!");
                        return null;
                }
            }
            switch (tooltipDirection)
            {
                case TooltipDirection.Bottom:
                    return Properties.Resources.tooltip_medium_bottom;

                case TooltipDirection.Left:
                    return Properties.Resources.tooltip_medium_left;

                case TooltipDirection.TopRight:
                    return Properties.Resources.tooltip_medium_topright;
                default:
                    Debug.Assert(false, "Image not set!");
                    return null;
            }
        }
        private bool IsNextButtonVisible()
        {
            switch (_tutorialProgress)
            {
                case TutorialProgress.TutorialAddSampleVideo:
                    return true;
                case TutorialProgress.TutorialScanButton:
                    return false;
                case TutorialProgress.TutorialHighlightsFound:
                    return false;
                case TutorialProgress.TutorialBookmarkFlag:
                    return true;
                case TutorialProgress.TutorialHandles:
                    return true;
                case TutorialProgress.TutorialShareButton:
                    return false;
                case TutorialProgress.TutorialCloseDetails:
                    return false;
                case TutorialProgress.TutorialStartOver:
                    return false;
                default:
                    Debug.Assert(false, "Invalid progress");
                    return true;
            }
        }

        private void LoadTooltip()
        {
            tooltipImage.Image = GetTooltipImage();
            Size = tooltipImage.Size; // new Size(tooltipImage.Size.Width, tooltipImage.Size.Height + 50);

            bubbleLabel.Text = GetBubbleText();
            bubbleLabel.Location = GetTextLocation();

            nextButton.Visible = IsNextButtonVisible();
            if (nextButton.Visible)
                nextButton.Location = GetNextButtonLocation();
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            OnAdvance();
        }

        private void TutorialBubble_Load(object sender, EventArgs e)
        {
            
        }
    }
}