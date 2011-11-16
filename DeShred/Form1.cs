using System;
using System.Collections.Generic;
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
            btnDeshred.Click += btnDeshred_Click;
            Deshredder.EdgeCalculated += Deshredder_EdgeCalculated;
            Deshredder.EdgeCalculationCompleted += Deshredder_EdgeCalculationCompleted;

            for (int i = 0; i < 20; i++)
                comboBox.Items.Add(i);

            comboBox.SelectedIndexChanged += comboBox_SelectedIndexChanged;
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

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int segment = int.Parse(((ComboBox) sender).SelectedItem.ToString());
            List<int> n = Deshredder.GetNeighbors(segment);

            var b = new Bitmap(640, 359);

            Bitmap s = Deshredder.ImageSegment(n[0]);
            for (int j = 0; j < 32; j++)
            {
                for (int k = 0; k < 359; k++)
                {
                    b.SetPixel(j + (0*32), k, s.GetPixel(j, k));
                }
            }

            s = Deshredder.ImageSegment(segment);
            for (int j = 0; j < 32; j++)
            {
                for (int k = 0; k < 359; k++)
                {
                    b.SetPixel(j + (1*32), k, s.GetPixel(j, k));
                }
            }

            s = Deshredder.ImageSegment(n[1]);
            for (int j = 0; j < 32; j++)
            {
                for (int k = 0; k < 359; k++)
                {
                    b.SetPixel(j + (2*32), k, s.GetPixel(j, k));
                }
            }

            pictureBoxNeighbors.Image = b;
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