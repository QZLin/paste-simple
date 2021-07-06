﻿namespace PasteSimple {
	partial class PasteSimpleMainForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PasteSimpleMainForm));
            this.loginButton = new System.Windows.Forms.Button();
            this.consoleTextBox = new System.Windows.Forms.RichTextBox();
            this.clientGroupBox = new System.Windows.Forms.GroupBox();
            this.connectServerPortTextBox = new System.Windows.Forms.TextBox();
            this.connectUidTextBox = new System.Windows.Forms.TextBox();
            this.uidLabel = new System.Windows.Forms.Label();
            this.connectPortLabel = new System.Windows.Forms.Label();
            this.connectServerAddressTextBox = new System.Windows.Forms.TextBox();
            this.connectServerAddressLabel = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.serverGroupBox = new System.Windows.Forms.GroupBox();
            this.OpenPortButton = new System.Windows.Forms.Button();
            this.serverAddressTextBox = new System.Windows.Forms.TextBox();
            this.startServerButton = new System.Windows.Forms.Button();
            this.serverPortTextBox = new System.Windows.Forms.TextBox();
            this.serverPort = new System.Windows.Forms.Label();
            this.serverAddress = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.clientGroupBox.SuspendLayout();
            this.serverGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(120, 142);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(75, 21);
            this.loginButton.TabIndex = 3;
            this.loginButton.Text = "Login";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.LoginButton_Click);
            // 
            // consoleTextBox
            // 
            this.consoleTextBox.Location = new System.Drawing.Point(12, 243);
            this.consoleTextBox.Name = "consoleTextBox";
            this.consoleTextBox.ReadOnly = true;
            this.consoleTextBox.Size = new System.Drawing.Size(721, 273);
            this.consoleTextBox.TabIndex = 6;
            this.consoleTextBox.Text = "";
            // 
            // clientGroupBox
            // 
            this.clientGroupBox.Controls.Add(this.connectServerPortTextBox);
            this.clientGroupBox.Controls.Add(this.connectUidTextBox);
            this.clientGroupBox.Controls.Add(this.uidLabel);
            this.clientGroupBox.Controls.Add(this.connectPortLabel);
            this.clientGroupBox.Controls.Add(this.connectServerAddressTextBox);
            this.clientGroupBox.Controls.Add(this.connectServerAddressLabel);
            this.clientGroupBox.Controls.Add(this.loginButton);
            this.clientGroupBox.Location = new System.Drawing.Point(412, 11);
            this.clientGroupBox.Name = "clientGroupBox";
            this.clientGroupBox.Size = new System.Drawing.Size(321, 169);
            this.clientGroupBox.TabIndex = 7;
            this.clientGroupBox.TabStop = false;
            this.clientGroupBox.Text = "Connect";
            // 
            // connectServerPortTextBox
            // 
            this.connectServerPortTextBox.Location = new System.Drawing.Point(54, 66);
            this.connectServerPortTextBox.Name = "connectServerPortTextBox";
            this.connectServerPortTextBox.Size = new System.Drawing.Size(200, 21);
            this.connectServerPortTextBox.TabIndex = 10;
            this.connectServerPortTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.connectServerPortTextBox_KeyPress);
            // 
            // connectUidTextBox
            // 
            this.connectUidTextBox.Location = new System.Drawing.Point(54, 105);
            this.connectUidTextBox.Name = "connectUidTextBox";
            this.connectUidTextBox.Size = new System.Drawing.Size(200, 21);
            this.connectUidTextBox.TabIndex = 6;
            this.connectUidTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.connectUidTextBox_KeyPress);
            // 
            // uidLabel
            // 
            this.uidLabel.AutoSize = true;
            this.uidLabel.Location = new System.Drawing.Point(52, 93);
            this.uidLabel.Name = "uidLabel";
            this.uidLabel.Size = new System.Drawing.Size(23, 12);
            this.uidLabel.TabIndex = 5;
            this.uidLabel.Text = "UID";
            // 
            // connectPortLabel
            // 
            this.connectPortLabel.AutoSize = true;
            this.connectPortLabel.Location = new System.Drawing.Point(50, 54);
            this.connectPortLabel.Name = "connectPortLabel";
            this.connectPortLabel.Size = new System.Drawing.Size(71, 12);
            this.connectPortLabel.TabIndex = 9;
            this.connectPortLabel.Text = "Server Port";
            // 
            // connectServerAddressTextBox
            // 
            this.connectServerAddressTextBox.Location = new System.Drawing.Point(54, 30);
            this.connectServerAddressTextBox.Name = "connectServerAddressTextBox";
            this.connectServerAddressTextBox.Size = new System.Drawing.Size(200, 21);
            this.connectServerAddressTextBox.TabIndex = 8;
            // 
            // connectServerAddressLabel
            // 
            this.connectServerAddressLabel.AutoSize = true;
            this.connectServerAddressLabel.Location = new System.Drawing.Point(51, 15);
            this.connectServerAddressLabel.Name = "connectServerAddressLabel";
            this.connectServerAddressLabel.Size = new System.Drawing.Size(89, 12);
            this.connectServerAddressLabel.TabIndex = 7;
            this.connectServerAddressLabel.Text = "Server Address";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(54, 71);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(76, 21);
            this.textBox1.TabIndex = 10;
            // 
            // serverGroupBox
            // 
            this.serverGroupBox.Controls.Add(this.OpenPortButton);
            this.serverGroupBox.Controls.Add(this.serverAddressTextBox);
            this.serverGroupBox.Controls.Add(this.startServerButton);
            this.serverGroupBox.Controls.Add(this.serverPortTextBox);
            this.serverGroupBox.Controls.Add(this.serverPort);
            this.serverGroupBox.Controls.Add(this.serverAddress);
            this.serverGroupBox.Location = new System.Drawing.Point(55, 11);
            this.serverGroupBox.Name = "serverGroupBox";
            this.serverGroupBox.Size = new System.Drawing.Size(286, 146);
            this.serverGroupBox.TabIndex = 7;
            this.serverGroupBox.TabStop = false;
            this.serverGroupBox.Text = "Server";
            // 
            // OpenPortButton
            // 
            this.OpenPortButton.Location = new System.Drawing.Point(168, 105);
            this.OpenPortButton.Name = "OpenPortButton";
            this.OpenPortButton.Size = new System.Drawing.Size(75, 21);
            this.OpenPortButton.TabIndex = 4;
            this.OpenPortButton.Text = "Open Port";
            this.OpenPortButton.UseVisualStyleBackColor = true;
            this.OpenPortButton.Click += new System.EventHandler(this.OpenPortButton_Click);
            // 
            // serverAddressTextBox
            // 
            this.serverAddressTextBox.Location = new System.Drawing.Point(48, 30);
            this.serverAddressTextBox.Name = "serverAddressTextBox";
            this.serverAddressTextBox.Size = new System.Drawing.Size(200, 21);
            this.serverAddressTextBox.TabIndex = 1;
            // 
            // startServerButton
            // 
            this.startServerButton.Location = new System.Drawing.Point(49, 105);
            this.startServerButton.Name = "startServerButton";
            this.startServerButton.Size = new System.Drawing.Size(75, 21);
            this.startServerButton.TabIndex = 3;
            this.startServerButton.Text = "Start Server";
            this.startServerButton.UseVisualStyleBackColor = true;
            this.startServerButton.Click += new System.EventHandler(this.StartServerButton_Click);
            // 
            // serverPortTextBox
            // 
            this.serverPortTextBox.Location = new System.Drawing.Point(48, 66);
            this.serverPortTextBox.Name = "serverPortTextBox";
            this.serverPortTextBox.Size = new System.Drawing.Size(200, 21);
            this.serverPortTextBox.TabIndex = 2;
            this.serverPortTextBox.Text = "6262";
            this.serverPortTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.serverPortTextBox_KeyPress);
            // 
            // serverPort
            // 
            this.serverPort.AutoSize = true;
            this.serverPort.Location = new System.Drawing.Point(46, 54);
            this.serverPort.Name = "serverPort";
            this.serverPort.Size = new System.Drawing.Size(71, 12);
            this.serverPort.TabIndex = 2;
            this.serverPort.Text = "Server Port";
            // 
            // serverAddress
            // 
            this.serverAddress.AutoSize = true;
            this.serverAddress.Location = new System.Drawing.Point(45, 15);
            this.serverAddress.Name = "serverAddress";
            this.serverAddress.Size = new System.Drawing.Size(89, 12);
            this.serverAddress.TabIndex = 2;
            this.serverAddress.Text = "Server Address";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(136, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "UID";
            // 
            // PasteSimpleMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(750, 528);
            this.Controls.Add(this.serverGroupBox);
            this.Controls.Add(this.clientGroupBox);
            this.Controls.Add(this.consoleTextBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PasteSimpleMainForm";
            this.Text = "PasteSimple";
            this.Load += new System.EventHandler(this.PasteSimpleMainForm_Load);
            this.clientGroupBox.ResumeLayout(false);
            this.clientGroupBox.PerformLayout();
            this.serverGroupBox.ResumeLayout(false);
            this.serverGroupBox.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.GroupBox clientGroupBox;
        private System.Windows.Forms.GroupBox serverGroupBox;
        private System.Windows.Forms.TextBox serverAddressTextBox;
        private System.Windows.Forms.Button startServerButton;
        private System.Windows.Forms.TextBox serverPortTextBox;
        private System.Windows.Forms.Label serverPort;
        private System.Windows.Forms.Label serverAddress;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label connectPortLabel;
        private System.Windows.Forms.TextBox connectServerAddressTextBox;
        private System.Windows.Forms.Label connectServerAddressLabel;
        private System.Windows.Forms.TextBox connectUidTextBox;
        private System.Windows.Forms.Label uidLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox connectServerPortTextBox;
        private System.Windows.Forms.RichTextBox consoleTextBox;
        private System.Windows.Forms.Button OpenPortButton;
    }
}