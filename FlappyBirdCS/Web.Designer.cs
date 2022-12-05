namespace FlappyBirdCS
{
    partial class Web
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Web));
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.buttons1 = new FlappyBirdCS.Buttons.Buttons();
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(800, 451);
            this.webBrowser1.TabIndex = 0;
            this.webBrowser1.Url = new System.Uri("https://salvuccif.altervista.org/FirstPage/index.html", System.UriKind.Absolute);
            // 
            // buttons1
            // 
            this.buttons1.BackColor = System.Drawing.SystemColors.Control;
            this.buttons1.BorderThickness = 2F;
            this.buttons1.Location = new System.Drawing.Point(12, 12);
            this.buttons1.Name = "buttons1";
            this.buttons1.Size = new System.Drawing.Size(94, 23);
            this.buttons1.TabIndex = 1;
            this.buttons1.Text = "Open in Web";
            this.buttons1.Click += new System.EventHandler(this.buttons1_Click);
            // 
            // Web
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Aqua;
            this.ClientSize = new System.Drawing.Size(800, 451);
            this.Controls.Add(this.buttons1);
            this.Controls.Add(this.webBrowser1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Web";
            this.Text = "Web";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser1;
        private Buttons.Buttons buttons1;
    }
}