namespace Warchief
{
    partial class DebugWindow
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
            this.debugTB = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // debugTB
            // 
            this.debugTB.Location = new System.Drawing.Point(107, 32);
            this.debugTB.Multiline = true;
            this.debugTB.Name = "debugTB";
            this.debugTB.ReadOnly = true;
            this.debugTB.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.debugTB.Size = new System.Drawing.Size(479, 487);
            this.debugTB.TabIndex = 3;
            // 
            // DebugWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 655);
            this.Controls.Add(this.debugTB);
            this.Name = "DebugWindow";
            this.Text = "Debug Window";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox debugTB;
    }
}