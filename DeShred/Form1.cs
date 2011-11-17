using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace DeShred
{
    public partial class FormDeshred : Form
    {
        #region C'tors

        public FormDeshred()
        {
            InitializeComponent();
            btnGo.Click += btnGo_Click;
            Deshredder.EdgeCalculated += Deshredder_EdgeCalculated;
            Deshredder.EdgeCalculationCompleted += Deshredder_EdgeCalculationCompleted;

            Deshredder.SegmentWidthCalculationCompleted += Deshredder_SegmentWidthCalculationCompleted;
            comboBoxTaskSelect.SelectedIndex = 0;
        }

        private void Deshredder_SegmentWidthCalculationCompleted(object sender, EventArgs e)
        {
            CrossThreadGuiDelegate d = CalculationCompleteSegmentWidth;
            Invoke(d, null);
        }

        #endregion

        #region Instance Methods

        private void CalculationCompleteSegmentWidth()
        {
            var b = new Bitmap(@"unshred.png");

            for (int i = Deshredder.segmentWidth; i < b.Width; i += Deshredder.segmentWidth)
            {
                for (int j = 0; j < b.Height; j++)
                {
                    if (i > 0) b.SetPixel(i - 1, j, Color.GreenYellow);
                    b.SetPixel(i, j, Color.GreenYellow);
                    if (i < Width - 1) b.SetPixel(i + 1, j, Color.GreenYellow);
                }
            }


            pictureBoxResult.Image = b;
            btnGo.Text = @"Process";
            btnGo.Enabled = true;
            pbDeshred.Value = 0;
        }

        private void CalculationComplete()
        {
            pictureBoxResult.Image = Deshredder.OutputBitmap;
            btnGo.Text = @"Process";
            btnGo.Enabled = true;
            pbDeshred.Value = 0;
        }

        private void SetProgressBar()
        {
            pbDeshred.Value += 5;
        }

        #endregion

        #region Event Handling

        private void Deshredder_EdgeCalculated(object sender, EventArgs e)
        {
            SetProgressBarDelegate d = SetProgressBar;
            Invoke(d, null);
        }

        private void Deshredder_EdgeCalculationCompleted(object sender, EventArgs e)
        {
            CrossThreadGuiDelegate d = CalculationComplete;
            Deshredder.SortEdges();
            Invoke(d, null);
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            if (comboBoxTaskSelect.SelectedIndex == 0)
            {
                var EdgeCalculationThread = new Thread(Deshredder.CalculateEdges);
                btnGo.Text = @"Processing";
                btnGo.Enabled = false;
                EdgeCalculationThread.Start();
            }

            if (comboBoxTaskSelect.SelectedIndex == 1)
            {
                var SegmentWidthThread = new Thread(() => Deshredder.SegmentWidth(new Bitmap(@"unshred.png"), .55));
                btnGo.Text = @"Processing";
                btnGo.Enabled = false;
                SegmentWidthThread.Start();
            }
        }

        #endregion

        #region Nested type: CrossThreadGuiDelegate

        private delegate void CrossThreadGuiDelegate();

        #endregion

        #region Nested type: SetProgressBarDelegate

        private delegate void SetProgressBarDelegate();

        #endregion
    }
}