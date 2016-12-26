namespace CSCOUpdater
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
			this.components = new System.ComponentModel.Container();
			this.button1 = new System.Windows.Forms.Button();
			this.installed_v = new System.Windows.Forms.Label();
			this.latest_v = new System.Windows.Forms.Label();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(12, 60);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(312, 51);
			this.button1.TabIndex = 0;
			this.button1.Text = "Update";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// installed_v
			// 
			this.installed_v.AutoSize = true;
			this.installed_v.Location = new System.Drawing.Point(9, 9);
			this.installed_v.Name = "installed_v";
			this.installed_v.Size = new System.Drawing.Size(116, 17);
			this.installed_v.TabIndex = 1;
			this.installed_v.Text = "Installed Version:";
			this.installed_v.Click += new System.EventHandler(this.label1_Click);
			// 
			// latest_v
			// 
			this.latest_v.AutoSize = true;
			this.latest_v.Location = new System.Drawing.Point(9, 26);
			this.latest_v.Name = "latest_v";
			this.latest_v.Size = new System.Drawing.Size(103, 17);
			this.latest_v.TabIndex = 2;
			this.latest_v.Text = "Latest Version:";
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(15, 117);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(309, 23);
			this.progressBar1.TabIndex = 3;
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Interval = 50;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			this.label1.Location = new System.Drawing.Point(12, 143);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(298, 17);
			this.label1.TabIndex = 4;
			this.label1.Text = "Updater by Tatjam. CS:CO by ZooL and Dayik";
			this.label1.Click += new System.EventHandler(this.label1_Click_1);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(336, 169);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.latest_v);
			this.Controls.Add(this.installed_v);
			this.Controls.Add(this.button1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "Form1";
			this.Text = "CS:CO Autoupdater";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label installed_v;
		private System.Windows.Forms.Label latest_v;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Label label1;
	}
}

