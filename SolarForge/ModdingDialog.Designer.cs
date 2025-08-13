namespace SolarForge
{

	public partial class ModdingDialog : global::System.Windows.Forms.Form
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
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::SolarForge.ModdingDialog));
			this.closeButton = new global::System.Windows.Forms.Button();
			this.label1 = new global::System.Windows.Forms.Label();
			this.modsPathTextBox = new global::System.Windows.Forms.TextBox();
			this.browseRootFolderButton = new global::System.Windows.Forms.Button();
			this.splitContainer = new global::System.Windows.Forms.SplitContainer();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.toolStrip1 = new global::System.Windows.Forms.ToolStrip();
			this.enableModButton = new global::System.Windows.Forms.ToolStripButton();
			this.availableModPropertyGrid = new global::System.Windows.Forms.PropertyGrid();
			this.availableModsListBox = new global::System.Windows.Forms.ListBox();
			this.groupBox2 = new global::System.Windows.Forms.GroupBox();
			this.toolStrip2 = new global::System.Windows.Forms.ToolStrip();
			this.disableModButton = new global::System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new global::System.Windows.Forms.ToolStripSeparator();
			this.moveEnabledModDownButton = new global::System.Windows.Forms.ToolStripButton();
			this.moveEnabledModUpButton = new global::System.Windows.Forms.ToolStripButton();
			this.enabledModPropertyGrid = new global::System.Windows.Forms.PropertyGrid();
			this.enabledModsListBox = new global::System.Windows.Forms.ListBox();
			this.applyChangesButton = new global::System.Windows.Forms.Button();
			((global::System.ComponentModel.ISupportInitialize)this.splitContainer).BeginInit();
			this.splitContainer.Panel1.SuspendLayout();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.toolStrip2.SuspendLayout();
			base.SuspendLayout();
			this.closeButton.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right);
			this.closeButton.Location = new global::System.Drawing.Point(611, 571);
			this.closeButton.Name = "closeButton";
			this.closeButton.Size = new global::System.Drawing.Size(166, 37);
			this.closeButton.TabIndex = 0;
			this.closeButton.Text = "Close";
			this.closeButton.UseVisualStyleBackColor = true;
			this.closeButton.Click += new global::System.EventHandler(this.closeButton_Click);
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(13, 13);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(58, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Root Path:";
			this.modsPathTextBox.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.modsPathTextBox.Location = new global::System.Drawing.Point(80, 10);
			this.modsPathTextBox.Name = "modsPathTextBox";
			this.modsPathTextBox.ReadOnly = true;
			this.modsPathTextBox.Size = new global::System.Drawing.Size(666, 20);
			this.modsPathTextBox.TabIndex = 2;
			this.browseRootFolderButton.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Right);
			this.browseRootFolderButton.Location = new global::System.Drawing.Point(752, 8);
			this.browseRootFolderButton.Name = "browseRootFolderButton";
			this.browseRootFolderButton.Size = new global::System.Drawing.Size(25, 23);
			this.browseRootFolderButton.TabIndex = 3;
			this.browseRootFolderButton.Text = "...";
			this.browseRootFolderButton.UseVisualStyleBackColor = true;
			this.browseRootFolderButton.Click += new global::System.EventHandler(this.browseRootFolderButton_Click);
			this.splitContainer.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.splitContainer.Location = new global::System.Drawing.Point(16, 49);
			this.splitContainer.Name = "splitContainer";
			this.splitContainer.Panel1.Controls.Add(this.groupBox1);
			this.splitContainer.Panel2.Controls.Add(this.groupBox2);
			this.splitContainer.Size = new global::System.Drawing.Size(761, 516);
			this.splitContainer.SplitterDistance = 385;
			this.splitContainer.TabIndex = 6;
			this.groupBox1.Controls.Add(this.toolStrip1);
			this.groupBox1.Controls.Add(this.availableModPropertyGrid);
			this.groupBox1.Controls.Add(this.availableModsListBox);
			this.groupBox1.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new global::System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new global::System.Drawing.Size(385, 516);
			this.groupBox1.TabIndex = 5;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Available Mods";
			this.toolStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.enableModButton
			});
			this.toolStrip1.Location = new global::System.Drawing.Point(3, 16);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new global::System.Drawing.Size(379, 25);
			this.toolStrip1.TabIndex = 5;
			this.toolStrip1.Text = "toolStrip1";
			this.enableModButton.DisplayStyle = global::System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.enableModButton.Image = (global::System.Drawing.Image)componentResourceManager.GetObject("enableModButton.Image");
			this.enableModButton.ImageTransparentColor = global::System.Drawing.Color.Magenta;
			this.enableModButton.Name = "enableModButton";
			this.enableModButton.Size = new global::System.Drawing.Size(74, 22);
			this.enableModButton.Text = "Enable Mod";
			this.enableModButton.Click += new global::System.EventHandler(this.enableModButton_Click);
			this.availableModPropertyGrid.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.availableModPropertyGrid.Location = new global::System.Drawing.Point(6, 373);
			this.availableModPropertyGrid.Name = "availableModPropertyGrid";
			this.availableModPropertyGrid.PropertySort = global::System.Windows.Forms.PropertySort.NoSort;
			this.availableModPropertyGrid.Size = new global::System.Drawing.Size(373, 137);
			this.availableModPropertyGrid.TabIndex = 3;
			this.availableModPropertyGrid.ToolbarVisible = false;
			this.availableModsListBox.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.availableModsListBox.FormattingEnabled = true;
			this.availableModsListBox.Location = new global::System.Drawing.Point(6, 43);
			this.availableModsListBox.Name = "availableModsListBox";
			this.availableModsListBox.Size = new global::System.Drawing.Size(373, 329);
			this.availableModsListBox.TabIndex = 1;
			this.groupBox2.Controls.Add(this.toolStrip2);
			this.groupBox2.Controls.Add(this.enabledModPropertyGrid);
			this.groupBox2.Controls.Add(this.enabledModsListBox);
			this.groupBox2.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.groupBox2.Location = new global::System.Drawing.Point(0, 0);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new global::System.Drawing.Size(372, 516);
			this.groupBox2.TabIndex = 6;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Enabled Mods";
			this.toolStrip2.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.disableModButton,
				this.toolStripSeparator1,
				this.moveEnabledModDownButton,
				this.moveEnabledModUpButton
			});
			this.toolStrip2.Location = new global::System.Drawing.Point(3, 16);
			this.toolStrip2.Name = "toolStrip2";
			this.toolStrip2.Size = new global::System.Drawing.Size(366, 25);
			this.toolStrip2.TabIndex = 6;
			this.toolStrip2.Text = "toolStrip2";
			this.disableModButton.DisplayStyle = global::System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.disableModButton.Image = (global::System.Drawing.Image)componentResourceManager.GetObject("disableModButton.Image");
			this.disableModButton.ImageTransparentColor = global::System.Drawing.Color.Magenta;
			this.disableModButton.Name = "disableModButton";
			this.disableModButton.Size = new global::System.Drawing.Size(77, 22);
			this.disableModButton.Text = "Disable Mod";
			this.disableModButton.Click += new global::System.EventHandler(this.disableModButton_Click);
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new global::System.Drawing.Size(6, 25);
			this.moveEnabledModDownButton.DisplayStyle = global::System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.moveEnabledModDownButton.Image = (global::System.Drawing.Image)componentResourceManager.GetObject("moveEnabledModDownButton.Image");
			this.moveEnabledModDownButton.ImageTransparentColor = global::System.Drawing.Color.Magenta;
			this.moveEnabledModDownButton.Name = "moveEnabledModDownButton";
			this.moveEnabledModDownButton.Size = new global::System.Drawing.Size(75, 22);
			this.moveEnabledModDownButton.Text = "Move Down";
			this.moveEnabledModDownButton.Click += new global::System.EventHandler(this.moveEnabledModDownButton_Click);
			this.moveEnabledModUpButton.DisplayStyle = global::System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.moveEnabledModUpButton.Image = (global::System.Drawing.Image)componentResourceManager.GetObject("moveEnabledModUpButton.Image");
			this.moveEnabledModUpButton.ImageTransparentColor = global::System.Drawing.Color.Magenta;
			this.moveEnabledModUpButton.Name = "moveEnabledModUpButton";
			this.moveEnabledModUpButton.Size = new global::System.Drawing.Size(59, 22);
			this.moveEnabledModUpButton.Text = "Move Up";
			this.moveEnabledModUpButton.Click += new global::System.EventHandler(this.moveEnabledModUpButton_Click);
			this.enabledModPropertyGrid.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.enabledModPropertyGrid.Location = new global::System.Drawing.Point(0, 373);
			this.enabledModPropertyGrid.Name = "enabledModPropertyGrid";
			this.enabledModPropertyGrid.PropertySort = global::System.Windows.Forms.PropertySort.NoSort;
			this.enabledModPropertyGrid.Size = new global::System.Drawing.Size(373, 137);
			this.enabledModPropertyGrid.TabIndex = 4;
			this.enabledModPropertyGrid.ToolbarVisible = false;
			this.enabledModsListBox.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.enabledModsListBox.FormattingEnabled = true;
			this.enabledModsListBox.Location = new global::System.Drawing.Point(0, 42);
			this.enabledModsListBox.Name = "enabledModsListBox";
			this.enabledModsListBox.Size = new global::System.Drawing.Size(373, 329);
			this.enabledModsListBox.TabIndex = 2;
			this.applyChangesButton.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right);
			this.applyChangesButton.Location = new global::System.Drawing.Point(439, 571);
			this.applyChangesButton.Name = "applyChangesButton";
			this.applyChangesButton.Size = new global::System.Drawing.Size(166, 37);
			this.applyChangesButton.TabIndex = 7;
			this.applyChangesButton.Text = "Apply Changes";
			this.applyChangesButton.UseVisualStyleBackColor = true;
			this.applyChangesButton.Click += new global::System.EventHandler(this.applyChangesButton_Click);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(789, 620);
			base.Controls.Add(this.applyChangesButton);
			base.Controls.Add(this.splitContainer);
			base.Controls.Add(this.browseRootFolderButton);
			base.Controls.Add(this.modsPathTextBox);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.closeButton);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.MinimumSize = new global::System.Drawing.Size(400, 200);
			base.Name = "ModdingDialog";
			base.SizeGripStyle = global::System.Windows.Forms.SizeGripStyle.Show;
			this.Text = "Mods";
			this.splitContainer.Panel1.ResumeLayout(false);
			this.splitContainer.Panel2.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.splitContainer).EndInit();
			this.splitContainer.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.toolStrip2.ResumeLayout(false);
			this.toolStrip2.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}


		private global::System.ComponentModel.IContainer components;


		private global::System.Windows.Forms.Button closeButton;


		private global::System.Windows.Forms.Label label1;


		private global::System.Windows.Forms.TextBox modsPathTextBox;


		private global::System.Windows.Forms.Button browseRootFolderButton;


		private global::System.Windows.Forms.SplitContainer splitContainer;


		private global::System.Windows.Forms.GroupBox groupBox1;


		private global::System.Windows.Forms.ToolStrip toolStrip1;


		private global::System.Windows.Forms.ToolStripButton enableModButton;


		private global::System.Windows.Forms.PropertyGrid availableModPropertyGrid;


		private global::System.Windows.Forms.ListBox availableModsListBox;


		private global::System.Windows.Forms.GroupBox groupBox2;


		private global::System.Windows.Forms.PropertyGrid enabledModPropertyGrid;


		private global::System.Windows.Forms.ListBox enabledModsListBox;


		private global::System.Windows.Forms.ToolStrip toolStrip2;


		private global::System.Windows.Forms.ToolStripButton disableModButton;


		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator1;


		private global::System.Windows.Forms.ToolStripButton moveEnabledModDownButton;


		private global::System.Windows.Forms.ToolStripButton moveEnabledModUpButton;


		private global::System.Windows.Forms.Button applyChangesButton;
	}
}
