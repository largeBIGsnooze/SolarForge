namespace SolarForge
{

	public partial class SettingsDialog : global::System.Windows.Forms.Form
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
			this.resetSettingsButton = new global::System.Windows.Forms.Button();
			this.closeButton = new global::System.Windows.Forms.Button();
			this.settingsFolderTextBox = new global::System.Windows.Forms.TextBox();
			this.tabControl = new global::System.Windows.Forms.TabControl();
			this.coreTabPage = new global::System.Windows.Forms.TabPage();
			this.corePropertyGrid = new global::System.Windows.Forms.PropertyGrid();
			this.postProcessingTabPage = new global::System.Windows.Forms.TabPage();
			this.postProcessingPropertyGrid = new global::System.Windows.Forms.PropertyGrid();
			this.cameraTabPage = new global::System.Windows.Forms.TabPage();
			this.cameraPropertyGrid = new global::System.Windows.Forms.PropertyGrid();
			this.meshTabPage = new global::System.Windows.Forms.TabPage();
			this.meshPropertyGrid = new global::System.Windows.Forms.PropertyGrid();
			this.unitTabPage = new global::System.Windows.Forms.TabPage();
			this.unitPropertyGrid = new global::System.Windows.Forms.PropertyGrid();
			this.particleEffectTabPage = new global::System.Windows.Forms.TabPage();
			this.particleEffectPropertyGrid = new global::System.Windows.Forms.PropertyGrid();
			this.beamEffectTabPage = new global::System.Windows.Forms.TabPage();
			this.beamEffectPropertyGrid = new global::System.Windows.Forms.PropertyGrid();
			this.skyboxTabPage = new global::System.Windows.Forms.TabPage();
			this.skyboxPropertyGrid = new global::System.Windows.Forms.PropertyGrid();
			this.brushTabPage = new global::System.Windows.Forms.TabPage();
			this.brushPropertyGrid = new global::System.Windows.Forms.PropertyGrid();
			this.scenarioTabPage = new global::System.Windows.Forms.TabPage();
			this.scenarioPropertyGrid = new global::System.Windows.Forms.PropertyGrid();
			this.guiTabPage = new global::System.Windows.Forms.TabPage();
			this.guiPropertyGrid = new global::System.Windows.Forms.PropertyGrid();
			this.deathSequenceTabPage = new global::System.Windows.Forms.TabPage();
			this.deathSequencePropertyGrid = new global::System.Windows.Forms.PropertyGrid();
			this.tabControl.SuspendLayout();
			this.coreTabPage.SuspendLayout();
			this.postProcessingTabPage.SuspendLayout();
			this.cameraTabPage.SuspendLayout();
			this.meshTabPage.SuspendLayout();
			this.unitTabPage.SuspendLayout();
			this.particleEffectTabPage.SuspendLayout();
			this.beamEffectTabPage.SuspendLayout();
			this.skyboxTabPage.SuspendLayout();
			this.brushTabPage.SuspendLayout();
			this.scenarioTabPage.SuspendLayout();
			this.guiTabPage.SuspendLayout();
			this.deathSequenceTabPage.SuspendLayout();
			base.SuspendLayout();
			this.resetSettingsButton.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.resetSettingsButton.Location = new global::System.Drawing.Point(16, 1091);
			this.resetSettingsButton.Margin = new global::System.Windows.Forms.Padding(4, 5, 4, 5);
			this.resetSettingsButton.Name = "resetSettingsButton";
			this.resetSettingsButton.Size = new global::System.Drawing.Size(238, 54);
			this.resetSettingsButton.TabIndex = 1;
			this.resetSettingsButton.Text = "Reset All";
			this.resetSettingsButton.UseVisualStyleBackColor = true;
			this.resetSettingsButton.Click += new global::System.EventHandler(this.resetSettingsButton_Click);
			this.closeButton.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right);
			this.closeButton.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.closeButton.Location = new global::System.Drawing.Point(1150, 1091);
			this.closeButton.Margin = new global::System.Windows.Forms.Padding(4, 5, 4, 5);
			this.closeButton.Name = "closeButton";
			this.closeButton.Size = new global::System.Drawing.Size(207, 54);
			this.closeButton.TabIndex = 3;
			this.closeButton.Text = "Close";
			this.closeButton.UseVisualStyleBackColor = true;
			this.closeButton.Click += new global::System.EventHandler(this.closeButton_Click);
			this.settingsFolderTextBox.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.settingsFolderTextBox.Location = new global::System.Drawing.Point(16, 12);
			this.settingsFolderTextBox.Name = "settingsFolderTextBox";
			this.settingsFolderTextBox.ReadOnly = true;
			this.settingsFolderTextBox.Size = new global::System.Drawing.Size(1348, 26);
			this.settingsFolderTextBox.TabIndex = 4;
			this.tabControl.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.tabControl.Controls.Add(this.coreTabPage);
			this.tabControl.Controls.Add(this.postProcessingTabPage);
			this.tabControl.Controls.Add(this.cameraTabPage);
			this.tabControl.Controls.Add(this.meshTabPage);
			this.tabControl.Controls.Add(this.unitTabPage);
			this.tabControl.Controls.Add(this.particleEffectTabPage);
			this.tabControl.Controls.Add(this.beamEffectTabPage);
			this.tabControl.Controls.Add(this.skyboxTabPage);
			this.tabControl.Controls.Add(this.brushTabPage);
			this.tabControl.Controls.Add(this.scenarioTabPage);
			this.tabControl.Controls.Add(this.guiTabPage);
			this.tabControl.Controls.Add(this.deathSequenceTabPage);
			this.tabControl.Location = new global::System.Drawing.Point(18, 45);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new global::System.Drawing.Size(1346, 1038);
			this.tabControl.TabIndex = 5;
			this.coreTabPage.Controls.Add(this.corePropertyGrid);
			this.coreTabPage.Location = new global::System.Drawing.Point(4, 29);
			this.coreTabPage.Name = "coreTabPage";
			this.coreTabPage.Padding = new global::System.Windows.Forms.Padding(3);
			this.coreTabPage.Size = new global::System.Drawing.Size(1338, 1005);
			this.coreTabPage.TabIndex = 0;
			this.coreTabPage.Text = "Core";
			this.coreTabPage.UseVisualStyleBackColor = true;
			this.corePropertyGrid.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.corePropertyGrid.Location = new global::System.Drawing.Point(3, 3);
			this.corePropertyGrid.Name = "corePropertyGrid";
			this.corePropertyGrid.PropertySort = global::System.Windows.Forms.PropertySort.NoSort;
			this.corePropertyGrid.Size = new global::System.Drawing.Size(1332, 999);
			this.corePropertyGrid.TabIndex = 0;
			this.corePropertyGrid.ToolbarVisible = false;
			this.postProcessingTabPage.Controls.Add(this.postProcessingPropertyGrid);
			this.postProcessingTabPage.Location = new global::System.Drawing.Point(4, 29);
			this.postProcessingTabPage.Name = "postProcessingTabPage";
			this.postProcessingTabPage.Size = new global::System.Drawing.Size(1338, 1005);
			this.postProcessingTabPage.TabIndex = 10;
			this.postProcessingTabPage.Text = "Post Processing";
			this.postProcessingTabPage.UseVisualStyleBackColor = true;
			this.postProcessingPropertyGrid.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.postProcessingPropertyGrid.Location = new global::System.Drawing.Point(0, 0);
			this.postProcessingPropertyGrid.Name = "postProcessingPropertyGrid";
			this.postProcessingPropertyGrid.Size = new global::System.Drawing.Size(1338, 1005);
			this.postProcessingPropertyGrid.TabIndex = 0;
			this.cameraTabPage.Controls.Add(this.cameraPropertyGrid);
			this.cameraTabPage.Location = new global::System.Drawing.Point(4, 29);
			this.cameraTabPage.Name = "cameraTabPage";
			this.cameraTabPage.Padding = new global::System.Windows.Forms.Padding(3);
			this.cameraTabPage.Size = new global::System.Drawing.Size(1338, 1005);
			this.cameraTabPage.TabIndex = 1;
			this.cameraTabPage.Text = "Camera";
			this.cameraTabPage.UseVisualStyleBackColor = true;
			this.cameraPropertyGrid.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.cameraPropertyGrid.Location = new global::System.Drawing.Point(3, 3);
			this.cameraPropertyGrid.Name = "cameraPropertyGrid";
			this.cameraPropertyGrid.PropertySort = global::System.Windows.Forms.PropertySort.NoSort;
			this.cameraPropertyGrid.Size = new global::System.Drawing.Size(1332, 999);
			this.cameraPropertyGrid.TabIndex = 0;
			this.cameraPropertyGrid.ToolbarVisible = false;
			this.meshTabPage.Controls.Add(this.meshPropertyGrid);
			this.meshTabPage.Location = new global::System.Drawing.Point(4, 29);
			this.meshTabPage.Name = "meshTabPage";
			this.meshTabPage.Size = new global::System.Drawing.Size(1338, 1005);
			this.meshTabPage.TabIndex = 2;
			this.meshTabPage.Text = "Meshes";
			this.meshTabPage.UseVisualStyleBackColor = true;
			this.meshPropertyGrid.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.meshPropertyGrid.Location = new global::System.Drawing.Point(0, 0);
			this.meshPropertyGrid.Name = "meshPropertyGrid";
			this.meshPropertyGrid.Size = new global::System.Drawing.Size(1338, 1005);
			this.meshPropertyGrid.TabIndex = 0;
			this.meshPropertyGrid.ToolbarVisible = false;
			this.unitTabPage.Controls.Add(this.unitPropertyGrid);
			this.unitTabPage.Location = new global::System.Drawing.Point(4, 29);
			this.unitTabPage.Name = "unitTabPage";
			this.unitTabPage.Size = new global::System.Drawing.Size(1338, 1005);
			this.unitTabPage.TabIndex = 3;
			this.unitTabPage.Text = "Units";
			this.unitTabPage.UseVisualStyleBackColor = true;
			this.unitPropertyGrid.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.unitPropertyGrid.Location = new global::System.Drawing.Point(0, 0);
			this.unitPropertyGrid.Name = "unitPropertyGrid";
			this.unitPropertyGrid.Size = new global::System.Drawing.Size(1338, 1005);
			this.unitPropertyGrid.TabIndex = 0;
			this.unitPropertyGrid.ToolbarVisible = false;
			this.particleEffectTabPage.Controls.Add(this.particleEffectPropertyGrid);
			this.particleEffectTabPage.Location = new global::System.Drawing.Point(4, 29);
			this.particleEffectTabPage.Name = "particleEffectTabPage";
			this.particleEffectTabPage.Size = new global::System.Drawing.Size(1338, 1005);
			this.particleEffectTabPage.TabIndex = 4;
			this.particleEffectTabPage.Text = "Particle Effects";
			this.particleEffectTabPage.UseVisualStyleBackColor = true;
			this.particleEffectPropertyGrid.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.particleEffectPropertyGrid.Location = new global::System.Drawing.Point(0, 0);
			this.particleEffectPropertyGrid.Name = "particleEffectPropertyGrid";
			this.particleEffectPropertyGrid.Size = new global::System.Drawing.Size(1338, 1005);
			this.particleEffectPropertyGrid.TabIndex = 0;
			this.particleEffectPropertyGrid.ToolbarVisible = false;
			this.beamEffectTabPage.Controls.Add(this.beamEffectPropertyGrid);
			this.beamEffectTabPage.Location = new global::System.Drawing.Point(4, 29);
			this.beamEffectTabPage.Name = "beamEffectTabPage";
			this.beamEffectTabPage.Size = new global::System.Drawing.Size(1338, 1005);
			this.beamEffectTabPage.TabIndex = 5;
			this.beamEffectTabPage.Text = "Beam Effects";
			this.beamEffectTabPage.UseVisualStyleBackColor = true;
			this.beamEffectPropertyGrid.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.beamEffectPropertyGrid.Location = new global::System.Drawing.Point(0, 0);
			this.beamEffectPropertyGrid.Name = "beamEffectPropertyGrid";
			this.beamEffectPropertyGrid.Size = new global::System.Drawing.Size(1338, 1005);
			this.beamEffectPropertyGrid.TabIndex = 0;
			this.beamEffectPropertyGrid.ToolbarVisible = false;
			this.skyboxTabPage.Controls.Add(this.skyboxPropertyGrid);
			this.skyboxTabPage.Location = new global::System.Drawing.Point(4, 29);
			this.skyboxTabPage.Name = "skyboxTabPage";
			this.skyboxTabPage.Size = new global::System.Drawing.Size(1338, 1005);
			this.skyboxTabPage.TabIndex = 6;
			this.skyboxTabPage.Text = "Skyboxes";
			this.skyboxTabPage.UseVisualStyleBackColor = true;
			this.skyboxPropertyGrid.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.skyboxPropertyGrid.Location = new global::System.Drawing.Point(0, 0);
			this.skyboxPropertyGrid.Name = "skyboxPropertyGrid";
			this.skyboxPropertyGrid.Size = new global::System.Drawing.Size(1338, 1005);
			this.skyboxPropertyGrid.TabIndex = 0;
			this.skyboxPropertyGrid.ToolbarVisible = false;
			this.brushTabPage.Controls.Add(this.brushPropertyGrid);
			this.brushTabPage.Location = new global::System.Drawing.Point(4, 29);
			this.brushTabPage.Name = "brushTabPage";
			this.brushTabPage.Size = new global::System.Drawing.Size(1338, 1005);
			this.brushTabPage.TabIndex = 7;
			this.brushTabPage.Text = "Brushes";
			this.brushTabPage.UseVisualStyleBackColor = true;
			this.brushPropertyGrid.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.brushPropertyGrid.Location = new global::System.Drawing.Point(0, 0);
			this.brushPropertyGrid.Name = "brushPropertyGrid";
			this.brushPropertyGrid.Size = new global::System.Drawing.Size(1338, 1005);
			this.brushPropertyGrid.TabIndex = 0;
			this.brushPropertyGrid.ToolbarVisible = false;
			this.scenarioTabPage.Controls.Add(this.scenarioPropertyGrid);
			this.scenarioTabPage.Location = new global::System.Drawing.Point(4, 29);
			this.scenarioTabPage.Name = "scenarioTabPage";
			this.scenarioTabPage.Size = new global::System.Drawing.Size(1338, 1005);
			this.scenarioTabPage.TabIndex = 8;
			this.scenarioTabPage.Text = "Scenarios";
			this.scenarioTabPage.UseVisualStyleBackColor = true;
			this.scenarioPropertyGrid.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.scenarioPropertyGrid.Location = new global::System.Drawing.Point(0, 0);
			this.scenarioPropertyGrid.Name = "scenarioPropertyGrid";
			this.scenarioPropertyGrid.Size = new global::System.Drawing.Size(1338, 1005);
			this.scenarioPropertyGrid.TabIndex = 0;
			this.scenarioPropertyGrid.ToolbarVisible = false;
			this.guiTabPage.Controls.Add(this.guiPropertyGrid);
			this.guiTabPage.Location = new global::System.Drawing.Point(4, 29);
			this.guiTabPage.Name = "guiTabPage";
			this.guiTabPage.Size = new global::System.Drawing.Size(1338, 1005);
			this.guiTabPage.TabIndex = 9;
			this.guiTabPage.Text = "Gui";
			this.guiTabPage.UseVisualStyleBackColor = true;
			this.guiPropertyGrid.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.guiPropertyGrid.Location = new global::System.Drawing.Point(0, 0);
			this.guiPropertyGrid.Name = "guiPropertyGrid";
			this.guiPropertyGrid.Size = new global::System.Drawing.Size(1338, 1005);
			this.guiPropertyGrid.TabIndex = 0;
			this.guiPropertyGrid.ToolbarVisible = false;
			this.deathSequenceTabPage.Controls.Add(this.deathSequencePropertyGrid);
			this.deathSequenceTabPage.Location = new global::System.Drawing.Point(4, 29);
			this.deathSequenceTabPage.Name = "deathSequenceTabPage";
			this.deathSequenceTabPage.Size = new global::System.Drawing.Size(1338, 1005);
			this.deathSequenceTabPage.TabIndex = 11;
			this.deathSequenceTabPage.Text = "Death Sequences";
			this.deathSequenceTabPage.UseVisualStyleBackColor = true;
			this.deathSequencePropertyGrid.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.deathSequencePropertyGrid.Location = new global::System.Drawing.Point(0, 0);
			this.deathSequencePropertyGrid.Name = "deathSequencePropertyGrid";
			this.deathSequencePropertyGrid.Size = new global::System.Drawing.Size(1338, 1005);
			this.deathSequencePropertyGrid.TabIndex = 0;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(9f, 20f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.closeButton;
			base.ClientSize = new global::System.Drawing.Size(1376, 1163);
			base.Controls.Add(this.tabControl);
			base.Controls.Add(this.settingsFolderTextBox);
			base.Controls.Add(this.closeButton);
			base.Controls.Add(this.resetSettingsButton);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			base.Margin = new global::System.Windows.Forms.Padding(4, 5, 4, 5);
			this.MinimumSize = new global::System.Drawing.Size(589, 278);
			base.Name = "SettingsDialog";
			base.SizeGripStyle = global::System.Windows.Forms.SizeGripStyle.Show;
			this.Text = "Settings";
			this.tabControl.ResumeLayout(false);
			this.coreTabPage.ResumeLayout(false);
			this.postProcessingTabPage.ResumeLayout(false);
			this.cameraTabPage.ResumeLayout(false);
			this.meshTabPage.ResumeLayout(false);
			this.unitTabPage.ResumeLayout(false);
			this.particleEffectTabPage.ResumeLayout(false);
			this.beamEffectTabPage.ResumeLayout(false);
			this.skyboxTabPage.ResumeLayout(false);
			this.brushTabPage.ResumeLayout(false);
			this.scenarioTabPage.ResumeLayout(false);
			this.guiTabPage.ResumeLayout(false);
			this.deathSequenceTabPage.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}


		private global::System.ComponentModel.IContainer components;


		private global::System.Windows.Forms.Button resetSettingsButton;


		private global::System.Windows.Forms.Button closeButton;


		private global::System.Windows.Forms.TextBox settingsFolderTextBox;


		private global::System.Windows.Forms.TabControl tabControl;


		private global::System.Windows.Forms.TabPage coreTabPage;


		private global::System.Windows.Forms.PropertyGrid corePropertyGrid;


		private global::System.Windows.Forms.TabPage cameraTabPage;


		private global::System.Windows.Forms.PropertyGrid cameraPropertyGrid;


		private global::System.Windows.Forms.TabPage meshTabPage;


		private global::System.Windows.Forms.PropertyGrid meshPropertyGrid;


		private global::System.Windows.Forms.TabPage unitTabPage;


		private global::System.Windows.Forms.PropertyGrid unitPropertyGrid;


		private global::System.Windows.Forms.TabPage particleEffectTabPage;


		private global::System.Windows.Forms.PropertyGrid particleEffectPropertyGrid;


		private global::System.Windows.Forms.TabPage beamEffectTabPage;


		private global::System.Windows.Forms.TabPage skyboxTabPage;


		private global::System.Windows.Forms.TabPage brushTabPage;


		private global::System.Windows.Forms.TabPage scenarioTabPage;


		private global::System.Windows.Forms.PropertyGrid beamEffectPropertyGrid;


		private global::System.Windows.Forms.PropertyGrid skyboxPropertyGrid;


		private global::System.Windows.Forms.PropertyGrid brushPropertyGrid;


		private global::System.Windows.Forms.PropertyGrid scenarioPropertyGrid;


		private global::System.Windows.Forms.TabPage guiTabPage;


		private global::System.Windows.Forms.PropertyGrid guiPropertyGrid;


		private global::System.Windows.Forms.TabPage postProcessingTabPage;


		private global::System.Windows.Forms.PropertyGrid postProcessingPropertyGrid;


		private global::System.Windows.Forms.TabPage deathSequenceTabPage;


		private global::System.Windows.Forms.PropertyGrid deathSequencePropertyGrid;
	}
}
