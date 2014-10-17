namespace SPC.Base.Control
{
    partial class CustomGroupStringBuildForm
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
            this.customGroupStringBuilder1 = new SPC.Base.Control.CustomGroupStringBuilder();
            this.SuspendLayout();
            // 
            // customGroupStringBuilder1
            // 
            this.customGroupStringBuilder1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customGroupStringBuilder1.Location = new System.Drawing.Point(0, 0);
            this.customGroupStringBuilder1.Name = "customGroupStringBuilder1";
            this.customGroupStringBuilder1.Size = new System.Drawing.Size(253, 198);
            this.customGroupStringBuilder1.TabIndex = 0;
            this.customGroupStringBuilder1.GroupStringDetermined += new System.EventHandler<SPC.Base.Control.CustomGroupStringBuilder.GroupStringDeterminedEventArgs>(this.customGroupStringBuilder1_GroupStringDetermined);
            this.customGroupStringBuilder1.GroupStringCanceled += new System.EventHandler(this.customGroupStringBuilder1_GroupStringCanceled);
            // 
            // CustomGroupStringBuildForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(253, 198);
            this.Controls.Add(this.customGroupStringBuilder1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "CustomGroupStringBuildForm";
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);

        }

        #endregion

        private CustomGroupStringBuilder customGroupStringBuilder1;
    }
}