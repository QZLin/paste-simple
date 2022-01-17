namespace PasteSimple2
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxServerAddr = new System.Windows.Forms.TextBox();
            this.textBoxServerPort = new System.Windows.Forms.TextBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.statusLabel1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxServerAddr
            // 
            this.textBoxServerAddr.Location = new System.Drawing.Point(32, 99);
            this.textBoxServerAddr.Name = "textBoxServerAddr";
            this.textBoxServerAddr.PlaceholderText = "Server Listen Adress";
            this.textBoxServerAddr.Size = new System.Drawing.Size(187, 23);
            this.textBoxServerAddr.TabIndex = 0;
            this.textBoxServerAddr.Text = "*";
            // 
            // textBoxServerPort
            // 
            this.textBoxServerPort.Location = new System.Drawing.Point(32, 140);
            this.textBoxServerPort.Name = "textBoxServerPort";
            this.textBoxServerPort.PlaceholderText = "Server Listen Port";
            this.textBoxServerPort.Size = new System.Drawing.Size(187, 23);
            this.textBoxServerPort.TabIndex = 1;
            this.textBoxServerPort.Text = "9999";
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(32, 194);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 23);
            this.buttonStart.TabIndex = 2;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(144, 194);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(75, 23);
            this.buttonStop.TabIndex = 3;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // statusLabel1
            // 
            this.statusLabel1.AutoSize = true;
            this.statusLabel1.Font = new System.Drawing.Font("Microsoft YaHei UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.statusLabel1.Location = new System.Drawing.Point(32, 32);
            this.statusLabel1.Name = "statusLabel1";
            this.statusLabel1.Size = new System.Drawing.Size(139, 41);
            this.statusLabel1.TabIndex = 4;
            this.statusLabel1.Text = "Waiting";
            this.statusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(253, 241);
            this.Controls.Add(this.statusLabel1);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.textBoxServerPort);
            this.Controls.Add(this.textBoxServerAddr);
            this.Name = "MainForm";
            this.Text = "PasteSimple";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxServerAddr;
        private System.Windows.Forms.TextBox textBoxServerPort;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Label statusLabel1;
    }
}

