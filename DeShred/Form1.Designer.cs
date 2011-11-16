namespace DeShred
{
    partial class FormDeshred
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDeshred));
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.btnDeshred = new System.Windows.Forms.Button();
            this.pbDeshred = new System.Windows.Forms.ProgressBar();
            this.pictureBoxResult = new System.Windows.Forms.PictureBox();
            this.comboBox = new System.Windows.Forms.ComboBox();
            this.pictureBoxNeighbors = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxNeighbors)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.ErrorImage = null;
            this.pictureBox.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox.Image")));
            this.pictureBox.InitialImage = null;
            this.pictureBox.Location = new System.Drawing.Point(23, 24);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(640, 359);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // btnDeshred
            // 
            this.btnDeshred.Location = new System.Drawing.Point(23, 389);
            this.btnDeshred.Name = "btnDeshred";
            this.btnDeshred.Size = new System.Drawing.Size(75, 23);
            this.btnDeshred.TabIndex = 1;
            this.btnDeshred.Text = "Deschred";
            this.btnDeshred.UseVisualStyleBackColor = true;
            // 
            // pbDeshred
            // 
            this.pbDeshred.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(184)))), ((int)(((byte)(4)))));
            this.pbDeshred.Location = new System.Drawing.Point(104, 389);
            this.pbDeshred.Name = "pbDeshred";
            this.pbDeshred.Size = new System.Drawing.Size(433, 23);
            this.pbDeshred.TabIndex = 2;
            // 
            // pictureBoxResult
            // 
            this.pictureBoxResult.ErrorImage = null;
            this.pictureBoxResult.InitialImage = null;
            this.pictureBoxResult.Location = new System.Drawing.Point(23, 418);
            this.pictureBoxResult.Name = "pictureBoxResult";
            this.pictureBoxResult.Size = new System.Drawing.Size(640, 359);
            this.pictureBoxResult.TabIndex = 3;
            this.pictureBoxResult.TabStop = false;
            // 
            // comboBox
            // 
            this.comboBox.FormattingEnabled = true;
            this.comboBox.Location = new System.Drawing.Point(719, 24);
            this.comboBox.Name = "comboBox";
            this.comboBox.Size = new System.Drawing.Size(121, 21);
            this.comboBox.TabIndex = 4;
            // 
            // pictureBoxNeighbors
            // 
            this.pictureBoxNeighbors.ErrorImage = null;
            this.pictureBoxNeighbors.InitialImage = null;
            this.pictureBoxNeighbors.Location = new System.Drawing.Point(846, 24);
            this.pictureBoxNeighbors.Name = "pictureBoxNeighbors";
            this.pictureBoxNeighbors.Size = new System.Drawing.Size(96, 359);
            this.pictureBoxNeighbors.TabIndex = 5;
            this.pictureBoxNeighbors.TabStop = false;
            // 
            // FormDeshred
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1132, 795);
            this.Controls.Add(this.pictureBoxNeighbors);
            this.Controls.Add(this.comboBox);
            this.Controls.Add(this.pictureBoxResult);
            this.Controls.Add(this.pbDeshred);
            this.Controls.Add(this.btnDeshred);
            this.Controls.Add(this.pictureBox);
            this.Name = "FormDeshred";
            this.Text = "Deshred";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxNeighbors)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button btnDeshred;
        private System.Windows.Forms.ProgressBar pbDeshred;
        private System.Windows.Forms.PictureBox pictureBoxResult;
        private System.Windows.Forms.ComboBox comboBox;
        private System.Windows.Forms.PictureBox pictureBoxNeighbors;
    }
}

