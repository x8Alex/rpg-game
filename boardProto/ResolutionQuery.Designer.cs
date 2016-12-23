namespace boardProto
{
    partial class ResolutionQuery
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
            this.buttonLaunch = new System.Windows.Forms.Button();
            this.dropBoxResolution = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonExit = new System.Windows.Forms.Button();
            this.checkBoxFullscreen = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // buttonLaunch
            // 
            this.buttonLaunch.Location = new System.Drawing.Point(12, 90);
            this.buttonLaunch.Name = "buttonLaunch";
            this.buttonLaunch.Size = new System.Drawing.Size(75, 23);
            this.buttonLaunch.TabIndex = 0;
            this.buttonLaunch.Text = "Launch";
            this.buttonLaunch.UseVisualStyleBackColor = true;
            this.buttonLaunch.Click += new System.EventHandler(this.buttonLaunch_Click);
            // 
            // dropBoxResolution
            // 
            this.dropBoxResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dropBoxResolution.FormattingEnabled = true;
            this.dropBoxResolution.Location = new System.Drawing.Point(12, 25);
            this.dropBoxResolution.Name = "dropBoxResolution";
            this.dropBoxResolution.Size = new System.Drawing.Size(180, 21);
            this.dropBoxResolution.TabIndex = 1;
            this.dropBoxResolution.SelectedIndexChanged += new System.EventHandler(this.dropBoxResolution_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Screen resolution:";
            // 
            // buttonExit
            // 
            this.buttonExit.Location = new System.Drawing.Point(117, 90);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(75, 23);
            this.buttonExit.TabIndex = 3;
            this.buttonExit.Text = "Exit";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // checkBoxFullscreen
            // 
            this.checkBoxFullscreen.AutoSize = true;
            this.checkBoxFullscreen.Checked = true;
            this.checkBoxFullscreen.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxFullscreen.Location = new System.Drawing.Point(12, 52);
            this.checkBoxFullscreen.Name = "checkBoxFullscreen";
            this.checkBoxFullscreen.Size = new System.Drawing.Size(74, 17);
            this.checkBoxFullscreen.TabIndex = 4;
            this.checkBoxFullscreen.Text = "Fullscreen";
            this.checkBoxFullscreen.UseVisualStyleBackColor = true;
            this.checkBoxFullscreen.CheckedChanged += new System.EventHandler(this.checkBoxFullscreen_CheckedChanged);
            // 
            // ResolutionQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(204, 125);
            this.Controls.Add(this.checkBoxFullscreen);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dropBoxResolution);
            this.Controls.Add(this.buttonLaunch);
            this.Name = "ResolutionQuery";
            this.Text = "ResolutionQuery";
            this.Load += new System.EventHandler(this.ResolutionQuery_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonLaunch;
        private System.Windows.Forms.ComboBox dropBoxResolution;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.CheckBox checkBoxFullscreen;


    }
}