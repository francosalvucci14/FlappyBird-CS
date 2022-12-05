namespace FlappyBirdCS
{
    partial class NewPass
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewPass));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textNewP = new System.Windows.Forms.TextBox();
            this.textNewC = new System.Windows.Forms.TextBox();
            this.buttons1 = new FlappyBirdCS.Buttons.Buttons();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(70, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "New Password : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(70, 147);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Confirm Password : ";
            // 
            // textNewP
            // 
            this.textNewP.Location = new System.Drawing.Point(177, 69);
            this.textNewP.Name = "textNewP";
            this.textNewP.Size = new System.Drawing.Size(224, 22);
            this.textNewP.TabIndex = 2;
            this.textNewP.TextChanged += new System.EventHandler(this.changePassText);
            // 
            // textNewC
            // 
            this.textNewC.Location = new System.Drawing.Point(189, 147);
            this.textNewC.Name = "textNewC";
            this.textNewC.Size = new System.Drawing.Size(224, 22);
            this.textNewC.TabIndex = 3;
            this.textNewC.TextChanged += new System.EventHandler(this.changePassText);
            // 
            // buttons1
            // 
            this.buttons1.BorderThickness = 2F;
            this.buttons1.Location = new System.Drawing.Point(189, 206);
            this.buttons1.Name = "buttons1";
            this.buttons1.Size = new System.Drawing.Size(75, 23);
            this.buttons1.TabIndex = 4;
            this.buttons1.Text = "Reset";
            this.buttons1.Click += new System.EventHandler(this.buttons1_Click);
            // 
            // NewPass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Aqua;
            this.ClientSize = new System.Drawing.Size(481, 283);
            this.Controls.Add(this.buttons1);
            this.Controls.Add(this.textNewC);
            this.Controls.Add(this.textNewP);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NewPass";
            this.Text = "NewPass";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textNewP;
        private System.Windows.Forms.TextBox textNewC;
        private Buttons.Buttons buttons1;
    }
}