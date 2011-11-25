using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace DeShred
{
    public partial class FormDeshred : Form
    {
        #region Fields

        private ShreddedImage shreddedImage;

        private readonly Stopwatch stpw = new Stopwatch();

        #endregion

        #region C'tors

        public FormDeshred()
        {
            InitializeComponent();
            btnGo.Click += btnGo_Click;
            comboBoxTaskSelect.SelectedIndex = 0;
        }

        #endregion

        #region Instance Methods

        private void CalculationComplete()
        {
            stpw.Stop();
            pictureBoxResult.Image = shreddedImage.OutputBitmap;
            btnGo.Text = @"Process";
            btnGo.Enabled = true;
            pbDeshred.Value = 0;
            labelSegmentTime.Text = string.Format(@"{0}mS", (stpw.ElapsedMilliseconds/1000.0));
            stpw.Reset();
        }

        private void CalculationCompleteSegmentWidth()
        {
            var b = new Bitmap(@"unshred.png");

            for (int i = shreddedImage.SegmentWidth; i < b.Width; i += shreddedImage.SegmentWidth)
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

        #endregion

        #region Event Handling

        private void btnGo_Click(object sender, EventArgs e)
        {
            shreddedImage = new ShreddedImage(new Bitmap(@"unshred.png"));
            shreddedImage.EdgeCalculationCompleted += shreddedImage_EdgeCalculationCompleted;

            if (comboBoxTaskSelect.SelectedIndex == 0)
            {
                var EdgeCalculationThread = new Thread(shreddedImage.CalculateEdges);
                btnGo.Text = @"Processing";
                btnGo.Enabled = false;
                stpw.Start();
                EdgeCalculationThread.Start();
            }

            if (comboBoxTaskSelect.SelectedIndex == 1)
            {
                var SegmentWidthThread =
                    new Thread(() => shreddedImage.CalculateSegmentWidth(new Bitmap(@"unshred.png"), .55));
                btnGo.Text = @"Processing";
                btnGo.Enabled = false;
                SegmentWidthThread.Start();
            }
        }

        void shreddedImage_EdgeCalculationCompleted(object sender, EventArgs e)
        {
            CrossThreadGuiDelegate d = CalculationComplete;
            shreddedImage.SortEdges();
            Invoke(d, null);
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