namespace Serwis
{
    partial class Form1
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
            this.lewy = new System.Windows.Forms.GroupBox();
            this.prawyBox = new System.Windows.Forms.GroupBox();
            this.prawy = new System.Windows.Forms.Panel();
            this.prawyBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // lewy
            // 
            this.lewy.Dock = System.Windows.Forms.DockStyle.Left;
            this.lewy.Location = new System.Drawing.Point(0, 0);
            this.lewy.Name = "lewy";
            this.lewy.Size = new System.Drawing.Size(200, 324);
            this.lewy.TabIndex = 0;
            this.lewy.TabStop = false;
            // 
            // prawyBox
            // 
            this.prawyBox.Controls.Add(this.prawy);
            this.prawyBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prawyBox.Location = new System.Drawing.Point(200, 0);
            this.prawyBox.Name = "prawyBox";
            this.prawyBox.Size = new System.Drawing.Size(333, 324);
            this.prawyBox.TabIndex = 1;
            this.prawyBox.TabStop = false;
            // 
            // prawy
            // 
            this.prawy.AutoScroll = true;
            this.prawy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prawy.Location = new System.Drawing.Point(3, 16);
            this.prawy.Name = "prawy";
            this.prawy.Size = new System.Drawing.Size(327, 305);
            this.prawy.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 324);
            this.Controls.Add(this.prawyBox);
            this.Controls.Add(this.lewy);
            this.Name = "Form1";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.prawyBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox lewy;
        private System.Windows.Forms.GroupBox prawyBox;
        private System.Windows.Forms.Panel prawy;
    }
}

