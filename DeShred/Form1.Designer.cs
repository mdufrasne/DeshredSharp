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
            this.btnGo = new System.Windows.Forms.Button();
            this.pbDeshred = new System.Windows.Forms.ProgressBar();
            this.pictureBoxResult = new System.Windows.Forms.PictureBox();
            this.comboBoxTaskSelect = new System.Windows.Forms.ComboBox();
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
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(225, 366);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(77, 23);
            this.btnGo.TabIndex = 1;
            this.btnGo.Text = "Process";
            this.btnGo.UseVisualStyleBackColor = true;
            // 
            // pbDeshred
            // 
            this.pbDeshred.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(184)))), ((int)(((byte)(4)))));
            this.pbDeshred.Location = new System.Drawing.Point(308, 368);
            this.pbDeshred.Name = "pbDeshred";
            this.pbDeshred.Size = new System.Drawing.Size(335, 23);
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
            // comboBoxTaskSelect
            // 
            this.comboBoxTaskSelect.FormattingEnabled = true;
            this.comboBoxTaskSelect.Items.AddRange(new object[] {
            "Unshred (Assume 32 pixel width)",
            "Derive segments only",
            "Unshred (Determine width algorithmically)"});
            this.comboBoxTaskSelect.Location = new System.Drawing.Point(3, 368);
            this.comboBoxTaskSelect.Name = "comboBoxTaskSelect";
            this.comboBoxTaskSelect.Size = new System.Drawing.Size(216, 21);
            this.comboBoxTaskSelect.TabIndex = 4;
            // 
            // FormDeshred
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(647, 759);
            this.Controls.Add(this.comboBoxTaskSelect);
            this.Controls.Add(this.pictureBoxResult);
            this.Controls.Add(this.pbDeshred);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.pictureBox);
            this.Name = "FormDeshred";
            this.Text = "Deshred";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxResult)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.ProgressBar pbDeshred;
        private System.Windows.Forms.PictureBox pictureBoxResult;
        private System.Windows.Forms.ComboBox comboBoxTaskSelect;
    }
}

