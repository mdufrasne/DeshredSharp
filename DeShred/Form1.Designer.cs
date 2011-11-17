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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxResult)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.ErrorImage = null;
            this.pictureBox.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox.Image")));
            this.pictureBox.InitialImage = null;
            this.pictureBox.Location = new System.Drawing.Point(3, 3);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(640, 359);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // btnDeshred
            // 
            this.btnDeshred.Location = new System.Drawing.Point(3, 368);
            this.btnDeshred.Name = "btnDeshred";
            this.btnDeshred.Size = new System.Drawing.Size(75, 23);
            this.btnDeshred.TabIndex = 1;
            this.btnDeshred.Text = "Deshred";
            this.btnDeshred.UseVisualStyleBackColor = true;
            // 
            // pbDeshred
            // 
            this.pbDeshred.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(184)))), ((int)(((byte)(4)))));
            this.pbDeshred.Location = new System.Drawing.Point(84, 368);
            this.pbDeshred.Name = "pbDeshred";
            this.pbDeshred.Size = new System.Drawing.Size(559, 23);
            this.pbDeshred.TabIndex = 2;
            // 
            // pictureBoxResult
            // 
            this.pictureBoxResult.ErrorImage = null;
            this.pictureBoxResult.InitialImage = null;
            this.pictureBoxResult.Location = new System.Drawing.Point(3, 397);
            this.pictureBoxResult.Name = "pictureBoxResult";
            this.pictureBoxResult.Size = new System.Drawing.Size(640, 359);
            this.pictureBoxResult.TabIndex = 3;
            this.pictureBoxResult.TabStop = false;
            // 
            // FormDeshred
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(647, 759);
            this.Controls.Add(this.pictureBoxResult);
            this.Controls.Add(this.pbDeshred);
            this.Controls.Add(this.btnDeshred);
            this.Controls.Add(this.pictureBox);
            this.Name = "FormDeshred";
            this.Text = "Deshred";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxResult)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button btnDeshred;
        private System.Windows.Forms.ProgressBar pbDeshred;
        private System.Windows.Forms.PictureBox pictureBoxResult;
    }
}

