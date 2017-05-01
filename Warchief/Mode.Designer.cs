namespace Warchief
{
    partial class Mode
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Mode));
            this.ValidateBTN = new System.Windows.Forms.Button();
            this.BotPB = new System.Windows.Forms.PictureBox();
            this.AdvisorPB = new System.Windows.Forms.PictureBox();
            this.WarchiefCB = new System.Windows.Forms.CheckBox();
            this.BotRB = new System.Windows.Forms.RadioButton();
            this.AdvisorRB = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.BotPB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AdvisorPB)).BeginInit();
            this.SuspendLayout();
            // 
            // ValidateBTN
            // 
            this.ValidateBTN.Location = new System.Drawing.Point(435, 231);
            this.ValidateBTN.Margin = new System.Windows.Forms.Padding(2);
            this.ValidateBTN.Name = "ValidateBTN";
            this.ValidateBTN.Size = new System.Drawing.Size(69, 23);
            this.ValidateBTN.TabIndex = 0;
            this.ValidateBTN.Text = "Validate";
            this.ValidateBTN.UseVisualStyleBackColor = true;
            // 
            // BotPB
            // 
            this.BotPB.AccessibleDescription = "";
            this.BotPB.Image = ((System.Drawing.Image)(resources.GetObject("BotPB.Image")));
            this.BotPB.InitialImage = null;
            this.BotPB.Location = new System.Drawing.Point(89, 36);
            this.BotPB.Margin = new System.Windows.Forms.Padding(2);
            this.BotPB.Name = "BotPB";
            this.BotPB.Size = new System.Drawing.Size(86, 105);
            this.BotPB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.BotPB.TabIndex = 1;
            this.BotPB.TabStop = false;
            // 
            // AdvisorPB
            // 
            this.AdvisorPB.Image = ((System.Drawing.Image)(resources.GetObject("AdvisorPB.Image")));
            this.AdvisorPB.InitialImage = ((System.Drawing.Image)(resources.GetObject("AdvisorPB.InitialImage")));
            this.AdvisorPB.Location = new System.Drawing.Point(343, 36);
            this.AdvisorPB.Margin = new System.Windows.Forms.Padding(2);
            this.AdvisorPB.Name = "AdvisorPB";
            this.AdvisorPB.Size = new System.Drawing.Size(195, 105);
            this.AdvisorPB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.AdvisorPB.TabIndex = 2;
            this.AdvisorPB.TabStop = false;
            // 
            // WarchiefCB
            // 
            this.WarchiefCB.AutoSize = true;
            this.WarchiefCB.Checked = true;
            this.WarchiefCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.WarchiefCB.Location = new System.Drawing.Point(89, 225);
            this.WarchiefCB.Margin = new System.Windows.Forms.Padding(2);
            this.WarchiefCB.Name = "WarchiefCB";
            this.WarchiefCB.Size = new System.Drawing.Size(70, 17);
            this.WarchiefCB.TabIndex = 3;
            this.WarchiefCB.Text = "WarChief";
            this.WarchiefCB.UseVisualStyleBackColor = true;
            // 
            // BotRB
            // 
            this.BotRB.AutoSize = true;
            this.BotRB.Checked = true;
            this.BotRB.Location = new System.Drawing.Point(90, 150);
            this.BotRB.Name = "BotRB";
            this.BotRB.Size = new System.Drawing.Size(71, 17);
            this.BotRB.TabIndex = 6;
            this.BotRB.TabStop = true;
            this.BotRB.Text = "Bot Mode";
            this.BotRB.UseVisualStyleBackColor = true;
            // 
            // AdvisorRB
            // 
            this.AdvisorRB.AutoSize = true;
            this.AdvisorRB.Location = new System.Drawing.Point(389, 150);
            this.AdvisorRB.Name = "AdvisorRB";
            this.AdvisorRB.Size = new System.Drawing.Size(115, 17);
            this.AdvisorRB.TabIndex = 7;
            this.AdvisorRB.Text = "Turn Advisor Mode";
            this.AdvisorRB.UseVisualStyleBackColor = true;
            // 
            // Mode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(583, 282);
            this.Controls.Add(this.AdvisorRB);
            this.Controls.Add(this.BotRB);
            this.Controls.Add(this.WarchiefCB);
            this.Controls.Add(this.AdvisorPB);
            this.Controls.Add(this.BotPB);
            this.Controls.Add(this.ValidateBTN);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Mode";
            this.Text = "\"";
            ((System.ComponentModel.ISupportInitialize)(this.BotPB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AdvisorPB)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ValidateBTN;
        private System.Windows.Forms.PictureBox BotPB;
        private System.Windows.Forms.PictureBox AdvisorPB;
        private System.Windows.Forms.CheckBox WarchiefCB;
        private System.Windows.Forms.RadioButton BotRB;
        private System.Windows.Forms.RadioButton AdvisorRB;
    }
}