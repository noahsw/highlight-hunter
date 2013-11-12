using System;
using System.Drawing;
using System.Windows.Forms;
using OdessaGUIProject.UI_Helpers;

namespace OdessaGUIProject.UI_Controls
{
    public partial class SensitivitySliderControl : UserControl
    {
        /// <summary>
        /// How far between the left margin and the first tick
        /// </summary>
        private const int gridLeft = 49;

        /// <summary>
        /// The distance between ticks
        /// </summary>
        private const int gridWidth = 75;

        private MainModel.DetectionThresholds detectionThreshold;

        /// <summary>
        /// Whether user is currently dragging slider
        /// </summary>
        private bool isDragging;

        /// <summary>
        /// The starting position of the drag
        /// </summary>
        private int startPositionX;

        public SensitivitySliderControl()
        {
            InitializeComponent();

            DesignLanguage.ApplyCustomFont(this.Controls);

            this.BackColor = Color.Transparent; // set here so we can see labels on the designer
        }

        internal MainModel.DetectionThresholds DetectionThreshold
        {
            get { return detectionThreshold; }
            set
            {
                this.detectionThreshold = value;
                MoveToValue(this.detectionThreshold);
            }
        }

        private void handlePictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left)
                return;

            isDragging = true;
            startPositionX = e.X;
        }

        private void handlePictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDragging) return;

            var delta = e.X - startPositionX;

            if (delta < 0 && (handlePictureBox.Left + delta) <= 0)
                this.handlePictureBox.Left = 0;
            else if (delta > 0 && (this.handlePictureBox.Left + this.handlePictureBox.Width + delta) >= this.Width)
                handlePictureBox.Left = this.Width - handlePictureBox.Width;
            else
                handlePictureBox.Left += delta;
        }

        private void handlePictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;

            var handlePosition = handlePictureBox.Left + (handlePictureBox.Width / 2);

            var mod = (handlePosition - gridLeft) % gridWidth;
            var newVal = (handlePosition - gridLeft) / gridWidth;
            if (mod > gridWidth / 2)
                newVal += 1;

            Console.WriteLine(newVal);

            DetectionThreshold = (MainModel.DetectionThresholds)Math.Abs(4 - newVal);
        }

        private void MoveToValue(MainModel.DetectionThresholds detectionThreshold)
        {
            // MainModel.DetectionThresholds is actually backwards from sliderValue (strictest = 0)

            var sliderValue = Math.Abs(4 - (int)detectionThreshold);

            handlePictureBox.Left = (gridLeft + sliderValue * gridWidth) - (handlePictureBox.Width / 2);
        }
    }
}