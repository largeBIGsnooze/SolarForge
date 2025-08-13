namespace SolarForge.Dialogs
{

	public partial class KeyBindingsDialog : global::System.Windows.Forms.Form
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
			this.okButton = new global::System.Windows.Forms.Button();
			this.contentTextBox = new global::System.Windows.Forms.TextBox();
			base.SuspendLayout();
			this.okButton.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right);
			this.okButton.DialogResult = global::System.Windows.Forms.DialogResult.OK;
			this.okButton.Location = new global::System.Drawing.Point(711, 811);
			this.okButton.Margin = new global::System.Windows.Forms.Padding(3, 2, 3, 2);
			this.okButton.Name = "okButton";
			this.okButton.Size = new global::System.Drawing.Size(193, 42);
			this.okButton.TabIndex = 0;
			this.okButton.Text = "Close";
			this.okButton.UseVisualStyleBackColor = true;
			this.contentTextBox.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.contentTextBox.Font = new global::System.Drawing.Font("Consolas", 12f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.contentTextBox.Location = new global::System.Drawing.Point(11, 10);
			this.contentTextBox.Margin = new global::System.Windows.Forms.Padding(3, 2, 3, 2);
			this.contentTextBox.Multiline = true;
			this.contentTextBox.Name = "contentTextBox";
			this.contentTextBox.ReadOnly = true;
			this.contentTextBox.Size = new global::System.Drawing.Size(895, 798);
			this.contentTextBox.TabIndex = 1;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(8f, 16f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(915, 863);
			base.Controls.Add(this.contentTextBox);
			base.Controls.Add(this.okButton);
			base.Margin = new global::System.Windows.Forms.Padding(3, 2, 3, 2);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "KeyBindingsDialog";
			base.ShowInTaskbar = false;
			this.Text = "Key Bindings";
			base.ResumeLayout(false);
			base.PerformLayout();
		}


		private global::System.ComponentModel.IContainer components;


		private global::System.Windows.Forms.Button okButton;


		private global::System.Windows.Forms.TextBox contentTextBox;
	}
}
