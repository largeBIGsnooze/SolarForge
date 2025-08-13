namespace SolarForge
{

	internal partial class AboutDialog : global::System.Windows.Forms.Form
	{

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}


		private void InitializeComponent()
		{
			this.labelVersion = new global::System.Windows.Forms.Label();
			this.labelCopyright = new global::System.Windows.Forms.Label();
			this.okButton = new global::System.Windows.Forms.Button();
			this.labelProductName = new global::System.Windows.Forms.Label();
			base.SuspendLayout();
			this.labelVersion.AutoSize = true;
			this.labelVersion.Location = new global::System.Drawing.Point(14, 40);
			this.labelVersion.Margin = new global::System.Windows.Forms.Padding(9, 0, 4, 0);
			this.labelVersion.MaximumSize = new global::System.Drawing.Size(0, 26);
			this.labelVersion.Name = "labelVersion";
			this.labelVersion.Size = new global::System.Drawing.Size(63, 20);
			this.labelVersion.TabIndex = 21;
			this.labelVersion.Text = "Version";
			this.labelVersion.TextAlign = global::System.Drawing.ContentAlignment.MiddleLeft;
			this.labelCopyright.AutoSize = true;
			this.labelCopyright.Location = new global::System.Drawing.Point(14, 66);
			this.labelCopyright.Margin = new global::System.Windows.Forms.Padding(9, 0, 4, 0);
			this.labelCopyright.MaximumSize = new global::System.Drawing.Size(0, 26);
			this.labelCopyright.Name = "labelCopyright";
			this.labelCopyright.Size = new global::System.Drawing.Size(76, 20);
			this.labelCopyright.TabIndex = 22;
			this.labelCopyright.Text = "Copyright";
			this.labelCopyright.TextAlign = global::System.Drawing.ContentAlignment.MiddleLeft;
			this.okButton.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right);
			this.okButton.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.okButton.Location = new global::System.Drawing.Point(494, 62);
			this.okButton.Margin = new global::System.Windows.Forms.Padding(4, 5, 4, 5);
			this.okButton.Name = "okButton";
			this.okButton.Size = new global::System.Drawing.Size(112, 34);
			this.okButton.TabIndex = 25;
			this.okButton.Text = "&OK";
			this.labelProductName.AutoSize = true;
			this.labelProductName.Location = new global::System.Drawing.Point(14, 14);
			this.labelProductName.Name = "labelProductName";
			this.labelProductName.Size = new global::System.Drawing.Size(110, 20);
			this.labelProductName.TabIndex = 26;
			this.labelProductName.Text = "Product Name";
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(9f, 20f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(624, 115);
			base.Controls.Add(this.labelProductName);
			base.Controls.Add(this.okButton);
			base.Controls.Add(this.labelCopyright);
			base.Controls.Add(this.labelVersion);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.Margin = new global::System.Windows.Forms.Padding(4, 5, 4, 5);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "AboutDialog";
			base.Padding = new global::System.Windows.Forms.Padding(14, 14, 14, 14);
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "About Solar Forge";
			base.ResumeLayout(false);
			base.PerformLayout();
		}


		private global::System.ComponentModel.IContainer components;


		private global::System.Windows.Forms.Label labelVersion;


		private global::System.Windows.Forms.Label labelCopyright;


		private global::System.Windows.Forms.Button okButton;


		private global::System.Windows.Forms.Label labelProductName;
	}
}
