using System;
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
            btnDeshred.Click += btnDeshred_Click;
            Deshredder.EdgeCalculated += Deshredder_EdgeCalculated;
            Deshredder.EdgeCalculationCompleted += Deshredder_EdgeCalculationCompleted;
        }

        #endregion

        #region Instance Methods

        private void CalculationComplete()
        {
            btnDeshred.Text = @"Deshred";
            btnDeshred.Enabled = true;
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
            Invoke(d, null);
            Deshredder.SortEdges();
            pictureBoxResult.Image = Deshredder.OutputBitmap;
        }

        private void btnDeshred_Click(object sender, EventArgs e)
        {
            var EdgeCalculationThread = new Thread(Deshredder.CalculateEdges);
            btnDeshred.Text = @"Processing";
            btnDeshred.Enabled = false;

            EdgeCalculationThread.Start();
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