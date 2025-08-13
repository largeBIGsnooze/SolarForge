namespace SolarForge
{

	public partial class MainForm : global::System.Windows.Forms.Form
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
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::SolarForge.MainForm));
			this.statusStrip = new global::System.Windows.Forms.StatusStrip();
			this.editorStatusLabel = new global::System.Windows.Forms.ToolStripStatusLabel();
			this.menuStrip = new global::System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.newToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.newParticleEffectToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.newBeamEffectToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.newScenarioMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.newBrushMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.openScenarioFolderToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.recentFilesToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new global::System.Windows.Forms.ToolStripSeparator();
			this.saveToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.saveAsToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new global::System.Windows.Forms.ToolStripSeparator();
			this.settingsMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new global::System.Windows.Forms.ToolStripSeparator();
			this.modsMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new global::System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.viewMenu = new global::System.Windows.Forms.ToolStripMenuItem();
			this.resetCameraToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.faceCameraToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.downXAxisToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.downYAxisToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.downZAxisToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.timeMenu = new global::System.Windows.Forms.ToolStripMenuItem();
			this.resetTimeToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.pauseTimeToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.singleStepToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.meshMenu = new global::System.Windows.Forms.ToolStripMenuItem();
			this.removeAllMeshesToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.saveMaterialsToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.particleEffectMenu = new global::System.Windows.Forms.ToolStripMenuItem();
			this.particleEffectUndoToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.particleEffectRedoToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator8 = new global::System.Windows.Forms.ToolStripSeparator();
			this.addParticleEffectNodeToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.addParticleEffectEmitterToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.addParticleEffectModifierToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new global::System.Windows.Forms.ToolStripSeparator();
			this.viewParticleEffectEditorMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.scenarioMenu = new global::System.Windows.Forms.ToolStripMenuItem();
			this.editFillingsToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator6 = new global::System.Windows.Forms.ToolStripSeparator();
			this.bakeGalaxyChartPreviewMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.keyBindingsToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator7 = new global::System.Windows.Forms.ToolStripSeparator();
			this.aboutToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolBarPanel = new global::System.Windows.Forms.Panel();
			this.outerSplitContainer = new global::System.Windows.Forms.SplitContainer();
			this.innerSplitContainer = new global::System.Windows.Forms.SplitContainer();
			this.saveScenarioFolderAsToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.statusStrip.SuspendLayout();
			this.menuStrip.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.outerSplitContainer).BeginInit();
			this.outerSplitContainer.Panel2.SuspendLayout();
			this.outerSplitContainer.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.innerSplitContainer).BeginInit();
			this.innerSplitContainer.SuspendLayout();
			base.SuspendLayout();
			this.statusStrip.ImageScalingSize = new global::System.Drawing.Size(24, 24);
			this.statusStrip.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.editorStatusLabel
			});
			this.statusStrip.LayoutStyle = global::System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this.statusStrip.Location = new global::System.Drawing.Point(0, 1160);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Padding = new global::System.Windows.Forms.Padding(2, 0, 21, 0);
			this.statusStrip.Size = new global::System.Drawing.Size(1779, 22);
			this.statusStrip.TabIndex = 1;
			this.editorStatusLabel.Name = "editorStatusLabel";
			this.editorStatusLabel.RightToLeft = global::System.Windows.Forms.RightToLeft.No;
			this.editorStatusLabel.Size = new global::System.Drawing.Size(0, 15);
			this.menuStrip.GripMargin = new global::System.Windows.Forms.Padding(2, 2, 0, 2);
			this.menuStrip.ImageScalingSize = new global::System.Drawing.Size(24, 24);
			this.menuStrip.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.fileToolStripMenuItem,
				this.viewMenu,
				this.timeMenu,
				this.meshMenu,
				this.particleEffectMenu,
				this.scenarioMenu,
				this.helpToolStripMenuItem
			});
			this.menuStrip.Location = new global::System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Padding = new global::System.Windows.Forms.Padding(6, 2, 0, 2);
			this.menuStrip.Size = new global::System.Drawing.Size(1779, 33);
			this.menuStrip.TabIndex = 2;
			this.menuStrip.Text = "menuStrip1";
			this.fileToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.newToolStripMenuItem,
				this.openToolStripMenuItem,
				this.openScenarioFolderToolStripMenuItem,
				this.recentFilesToolStripMenuItem,
				this.toolStripSeparator3,
				this.saveToolStripMenuItem,
				this.saveAsToolStripMenuItem,
				this.saveScenarioFolderAsToolStripMenuItem,
				this.toolStripSeparator2,
				this.settingsMenuItem,
				this.toolStripSeparator5,
				this.modsMenuItem,
				this.toolStripSeparator1,
				this.exitToolStripMenuItem
			});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new global::System.Drawing.Size(54, 29);
			this.fileToolStripMenuItem.Text = "File";
			this.newToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.newParticleEffectToolStripMenuItem,
				this.newBeamEffectToolStripMenuItem,
				this.newScenarioMenuItem,
				this.newBrushMenuItem
			});
			this.newToolStripMenuItem.Name = "newToolStripMenuItem";
			this.newToolStripMenuItem.Size = new global::System.Drawing.Size(315, 34);
			this.newToolStripMenuItem.Text = "New";
			this.newParticleEffectToolStripMenuItem.Name = "newParticleEffectToolStripMenuItem";
			this.newParticleEffectToolStripMenuItem.Size = new global::System.Drawing.Size(218, 34);
			this.newParticleEffectToolStripMenuItem.Text = "Particle Effect";
			this.newParticleEffectToolStripMenuItem.Click += new global::System.EventHandler(this.newParticleEffectToolStripMenuItem_Click);
			this.newBeamEffectToolStripMenuItem.Name = "newBeamEffectToolStripMenuItem";
			this.newBeamEffectToolStripMenuItem.Size = new global::System.Drawing.Size(218, 34);
			this.newBeamEffectToolStripMenuItem.Text = "Beam Effect";
			this.newBeamEffectToolStripMenuItem.Click += new global::System.EventHandler(this.newBeamEffectToolStripMenuItem_Click);
			this.newScenarioMenuItem.Name = "newScenarioMenuItem";
			this.newScenarioMenuItem.Size = new global::System.Drawing.Size(218, 34);
			this.newScenarioMenuItem.Text = "Scenario";  
			this.newScenarioMenuItem.Click += new global::System.EventHandler(this.newScenarioMenuItem_Click); 
			this.newBrushMenuItem.Name = "newBrushMenuItem";
			this.newBrushMenuItem.Size = new global::System.Drawing.Size(218, 34);
			this.newBrushMenuItem.Text = "Brush";
			this.newBrushMenuItem.Click += new global::System.EventHandler(this.newBrushMenuItem_Click);
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Size = new global::System.Drawing.Size(315, 34);
			this.openToolStripMenuItem.Text = "Open...";
			this.openToolStripMenuItem.Click += new global::System.EventHandler(this.openToolStripMenuItem_Click);
			this.openScenarioFolderToolStripMenuItem.Name = "openScenarioFolderToolStripMenuItem";
			this.openScenarioFolderToolStripMenuItem.Size = new global::System.Drawing.Size(315, 34);
			this.openScenarioFolderToolStripMenuItem.Text = "Open Scenario Folder...";
			this.openScenarioFolderToolStripMenuItem.Click += new global::System.EventHandler(this.openFolderToolStripMenuItem_Click);
			this.recentFilesToolStripMenuItem.Name = "recentFilesToolStripMenuItem";
			this.recentFilesToolStripMenuItem.Size = new global::System.Drawing.Size(315, 34);
			this.recentFilesToolStripMenuItem.Text = "Recent Files";
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new global::System.Drawing.Size(312, 6);
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.ShortcutKeys = (global::System.Windows.Forms.Keys)131155;
			this.saveToolStripMenuItem.Size = new global::System.Drawing.Size(315, 34);
			this.saveToolStripMenuItem.Text = "Save";
			this.saveToolStripMenuItem.Click += new global::System.EventHandler(this.saveToolStripMenuItem_Click);
			this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			this.saveAsToolStripMenuItem.Size = new global::System.Drawing.Size(315, 34);
			this.saveAsToolStripMenuItem.Text = "Save As...";
			this.saveAsToolStripMenuItem.Click += new global::System.EventHandler(this.saveAsToolStripMenuItem_Click);
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new global::System.Drawing.Size(312, 6);
			this.settingsMenuItem.Name = "settingsMenuItem";
			this.settingsMenuItem.Size = new global::System.Drawing.Size(315, 34);
			this.settingsMenuItem.Text = "Settings...";
			this.settingsMenuItem.Click += new global::System.EventHandler(this.settingsMenuItem_Click);
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new global::System.Drawing.Size(312, 6);
			this.modsMenuItem.Name = "modsMenuItem";
			this.modsMenuItem.Size = new global::System.Drawing.Size(315, 34);
			this.modsMenuItem.Text = "Mods...";
			this.modsMenuItem.Click += new global::System.EventHandler(this.modsMenuItem_Click);
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new global::System.Drawing.Size(312, 6);
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.ShortcutKeys = (global::System.Windows.Forms.Keys)262259;
			this.exitToolStripMenuItem.Size = new global::System.Drawing.Size(315, 34);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new global::System.EventHandler(this.exitToolStripMenuItem_Click);
			this.viewMenu.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.resetCameraToolStripMenuItem,
				this.faceCameraToolStripMenuItem
			});
			this.viewMenu.Name = "viewMenu";
			this.viewMenu.Size = new global::System.Drawing.Size(65, 29);
			this.viewMenu.Text = "View";
			this.resetCameraToolStripMenuItem.Name = "resetCameraToolStripMenuItem";
			this.resetCameraToolStripMenuItem.ShortcutKeys = global::System.Windows.Forms.Keys.F1;
			this.resetCameraToolStripMenuItem.Size = new global::System.Drawing.Size(252, 34);
			this.resetCameraToolStripMenuItem.Text = "Reset Camera";
			this.resetCameraToolStripMenuItem.Click += new global::System.EventHandler(this.resetCameraToolStripMenuItem_Click);
			this.faceCameraToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.downXAxisToolStripMenuItem,
				this.downYAxisToolStripMenuItem,
				this.downZAxisToolStripMenuItem
			});
			this.faceCameraToolStripMenuItem.Name = "faceCameraToolStripMenuItem";
			this.faceCameraToolStripMenuItem.Size = new global::System.Drawing.Size(252, 34);
			this.faceCameraToolStripMenuItem.Text = "Face Camera";
			this.downXAxisToolStripMenuItem.Name = "downXAxisToolStripMenuItem";
			this.downXAxisToolStripMenuItem.ShortcutKeys = global::System.Windows.Forms.Keys.F2;
			this.downXAxisToolStripMenuItem.Size = new global::System.Drawing.Size(245, 34);
			this.downXAxisToolStripMenuItem.Text = "Down X Axis";
			this.downXAxisToolStripMenuItem.Click += new global::System.EventHandler(this.downXAxisToolStripMenuItem_Click);
			this.downYAxisToolStripMenuItem.Name = "downYAxisToolStripMenuItem";
			this.downYAxisToolStripMenuItem.ShortcutKeys = global::System.Windows.Forms.Keys.F3;
			this.downYAxisToolStripMenuItem.Size = new global::System.Drawing.Size(245, 34);
			this.downYAxisToolStripMenuItem.Text = "Down Y Axis";
			this.downYAxisToolStripMenuItem.Click += new global::System.EventHandler(this.downYAxisToolStripMenuItem_Click);
			this.downZAxisToolStripMenuItem.Name = "downZAxisToolStripMenuItem";
			this.downZAxisToolStripMenuItem.ShortcutKeys = global::System.Windows.Forms.Keys.F4;
			this.downZAxisToolStripMenuItem.Size = new global::System.Drawing.Size(245, 34);
			this.downZAxisToolStripMenuItem.Text = "Down Z Axis";
			this.downZAxisToolStripMenuItem.Click += new global::System.EventHandler(this.downZAxisToolStripMenuItem_Click);
			this.timeMenu.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.resetTimeToolStripMenuItem,
				this.pauseTimeToolStripMenuItem,
				this.singleStepToolStripMenuItem
			});
			this.timeMenu.Name = "timeMenu";
			this.timeMenu.Size = new global::System.Drawing.Size(66, 29);
			this.timeMenu.Text = "Time";
			this.resetTimeToolStripMenuItem.Name = "resetTimeToolStripMenuItem";
			this.resetTimeToolStripMenuItem.ShortcutKeys = global::System.Windows.Forms.Keys.F5;
			this.resetTimeToolStripMenuItem.Size = new global::System.Drawing.Size(233, 34);
			this.resetTimeToolStripMenuItem.Text = "Reset";
			this.resetTimeToolStripMenuItem.Click += new global::System.EventHandler(this.resetTimeToolStripMenuItem_Click);
			this.pauseTimeToolStripMenuItem.Name = "pauseTimeToolStripMenuItem";
			this.pauseTimeToolStripMenuItem.ShortcutKeys = global::System.Windows.Forms.Keys.F6;
			this.pauseTimeToolStripMenuItem.Size = new global::System.Drawing.Size(233, 34);
			this.pauseTimeToolStripMenuItem.Text = "Paused";
			this.pauseTimeToolStripMenuItem.Click += new global::System.EventHandler(this.pauseTimeToolStripMenuItem_Click);
			this.singleStepToolStripMenuItem.Name = "singleStepToolStripMenuItem";
			this.singleStepToolStripMenuItem.ShortcutKeys = global::System.Windows.Forms.Keys.F7;
			this.singleStepToolStripMenuItem.Size = new global::System.Drawing.Size(233, 34);
			this.singleStepToolStripMenuItem.Text = "Single Step";
			this.singleStepToolStripMenuItem.Click += new global::System.EventHandler(this.singleStepToolStripMenuItem_Click);
			this.meshMenu.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.removeAllMeshesToolStripMenuItem,
				this.saveMaterialsToolStripMenuItem
			});
			this.meshMenu.Name = "meshMenu";
			this.meshMenu.Size = new global::System.Drawing.Size(71, 29);
			this.meshMenu.Text = "Mesh";
			this.removeAllMeshesToolStripMenuItem.Name = "removeAllMeshesToolStripMenuItem";
			this.removeAllMeshesToolStripMenuItem.Size = new global::System.Drawing.Size(292, 34);
			this.removeAllMeshesToolStripMenuItem.Text = "Remove All";
			this.removeAllMeshesToolStripMenuItem.Click += new global::System.EventHandler(this.removeAllMeshesToolStripMenuItem_Click);
			this.saveMaterialsToolStripMenuItem.Name = "saveMaterialsToolStripMenuItem";
			this.saveMaterialsToolStripMenuItem.Size = new global::System.Drawing.Size(292, 34);
			this.saveMaterialsToolStripMenuItem.Text = "Save Material Changes";
			this.saveMaterialsToolStripMenuItem.Click += new global::System.EventHandler(this.saveMaterialsToolStripMenuItem_Click);
			this.particleEffectMenu.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.particleEffectUndoToolStripMenuItem,
				this.particleEffectRedoToolStripMenuItem,
				this.toolStripSeparator8,
				this.addParticleEffectNodeToolStripMenuItem,
				this.addParticleEffectEmitterToolStripMenuItem,
				this.addParticleEffectModifierToolStripMenuItem,
				this.toolStripSeparator4,
				this.viewParticleEffectEditorMenuItem
			});
			this.particleEffectMenu.Name = "particleEffectMenu";
			this.particleEffectMenu.Size = new global::System.Drawing.Size(132, 29);
			this.particleEffectMenu.Text = "Particle Effect";
			this.particleEffectUndoToolStripMenuItem.Name = "particleEffectUndoToolStripMenuItem";
			this.particleEffectUndoToolStripMenuItem.ShortcutKeys = (global::System.Windows.Forms.Keys)131162;
			this.particleEffectUndoToolStripMenuItem.Size = new global::System.Drawing.Size(220, 34);
			this.particleEffectUndoToolStripMenuItem.Text = "Undo";
			this.particleEffectUndoToolStripMenuItem.Click += new global::System.EventHandler(this.undoToolStripMenuItem_Click);
			this.particleEffectRedoToolStripMenuItem.Name = "particleEffectRedoToolStripMenuItem";
			this.particleEffectRedoToolStripMenuItem.ShortcutKeys = (global::System.Windows.Forms.Keys)131161;
			this.particleEffectRedoToolStripMenuItem.Size = new global::System.Drawing.Size(220, 34);
			this.particleEffectRedoToolStripMenuItem.Text = "Redo";
			this.particleEffectRedoToolStripMenuItem.Click += new global::System.EventHandler(this.redoToolStripMenuItem_Click);
			this.toolStripSeparator8.Name = "toolStripSeparator8";
			this.toolStripSeparator8.Size = new global::System.Drawing.Size(217, 6);
			this.addParticleEffectNodeToolStripMenuItem.Name = "addParticleEffectNodeToolStripMenuItem";
			this.addParticleEffectNodeToolStripMenuItem.Size = new global::System.Drawing.Size(220, 34);
			this.addParticleEffectNodeToolStripMenuItem.Text = "Add Node";
			this.addParticleEffectNodeToolStripMenuItem.Click += new global::System.EventHandler(this.addParticleEffectNodeToolStripMenuItem_Click);
			this.addParticleEffectEmitterToolStripMenuItem.Name = "addParticleEffectEmitterToolStripMenuItem";
			this.addParticleEffectEmitterToolStripMenuItem.Size = new global::System.Drawing.Size(220, 34);
			this.addParticleEffectEmitterToolStripMenuItem.Text = "Add Emitter";
			this.addParticleEffectEmitterToolStripMenuItem.Click += new global::System.EventHandler(this.addParticleEffectEmitterToolStripMenuItem_Click);
			this.addParticleEffectModifierToolStripMenuItem.Name = "addParticleEffectModifierToolStripMenuItem";
			this.addParticleEffectModifierToolStripMenuItem.Size = new global::System.Drawing.Size(220, 34);
			this.addParticleEffectModifierToolStripMenuItem.Text = "Add Modifier";
			this.addParticleEffectModifierToolStripMenuItem.Click += new global::System.EventHandler(this.addParticleEffectModifierToolStripMenuItem_Click);
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new global::System.Drawing.Size(217, 6);
			this.viewParticleEffectEditorMenuItem.Name = "viewParticleEffectEditorMenuItem";
			this.viewParticleEffectEditorMenuItem.Size = new global::System.Drawing.Size(220, 34);
			this.viewParticleEffectEditorMenuItem.Text = "Show Editor";
			this.viewParticleEffectEditorMenuItem.Click += new global::System.EventHandler(this.viewParticleEffectEditorMenuItem_Click);
			this.scenarioMenu.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.editFillingsToolStripMenuItem,
				this.toolStripSeparator6,
				this.bakeGalaxyChartPreviewMenuItem
			});
			this.scenarioMenu.Name = "scenarioMenu";
			this.scenarioMenu.Size = new global::System.Drawing.Size(95, 29);
			this.scenarioMenu.Text = "Scenario";
			this.editFillingsToolStripMenuItem.Name = "editFillingsToolStripMenuItem";
			this.editFillingsToolStripMenuItem.Size = new global::System.Drawing.Size(216, 34);
			this.editFillingsToolStripMenuItem.Text = "Edit Fillings";
			this.editFillingsToolStripMenuItem.Click += new global::System.EventHandler(this.editFillingsToolStripMenuItem_Click);
			this.toolStripSeparator6.Name = "toolStripSeparator6";
			this.toolStripSeparator6.Size = new global::System.Drawing.Size(213, 6);
			this.bakeGalaxyChartPreviewMenuItem.Name = "bakeGalaxyChartPreviewMenuItem";
			this.bakeGalaxyChartPreviewMenuItem.Size = new global::System.Drawing.Size(216, 34);
			this.bakeGalaxyChartPreviewMenuItem.Text = "Bake Preview";
			this.bakeGalaxyChartPreviewMenuItem.Click += new global::System.EventHandler(this.bakeGalaxyChartPreviewMenuItem_Click);
			this.helpToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.keyBindingsToolStripMenuItem,
				this.toolStripSeparator7,
				this.aboutToolStripMenuItem
			});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new global::System.Drawing.Size(65, 29);
			this.helpToolStripMenuItem.Text = "Help";
			this.keyBindingsToolStripMenuItem.Name = "keyBindingsToolStripMenuItem";
			this.keyBindingsToolStripMenuItem.Size = new global::System.Drawing.Size(215, 34);
			this.keyBindingsToolStripMenuItem.Text = "Key Bindings";
			this.keyBindingsToolStripMenuItem.Click += new global::System.EventHandler(this.keyBindingsToolStripMenuItem_Click);
			this.toolStripSeparator7.Name = "toolStripSeparator7";
			this.toolStripSeparator7.Size = new global::System.Drawing.Size(212, 6);
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new global::System.Drawing.Size(215, 34);
			this.aboutToolStripMenuItem.Text = "About";
			this.aboutToolStripMenuItem.Click += new global::System.EventHandler(this.aboutToolStripMenuItem_Click);
			this.toolBarPanel.Dock = global::System.Windows.Forms.DockStyle.Top;
			this.toolBarPanel.Location = new global::System.Drawing.Point(0, 33);
			this.toolBarPanel.Margin = new global::System.Windows.Forms.Padding(4, 5, 4, 5);
			this.toolBarPanel.Name = "toolBarPanel";
			this.toolBarPanel.Size = new global::System.Drawing.Size(1779, 62);
			this.toolBarPanel.TabIndex = 4;
			this.outerSplitContainer.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.outerSplitContainer.Location = new global::System.Drawing.Point(0, 95);
			this.outerSplitContainer.Margin = new global::System.Windows.Forms.Padding(4, 5, 4, 5);
			this.outerSplitContainer.Name = "outerSplitContainer";
			this.outerSplitContainer.Panel2.Controls.Add(this.innerSplitContainer);
			this.outerSplitContainer.Size = new global::System.Drawing.Size(1779, 1065);
			this.outerSplitContainer.SplitterDistance = 591;
			this.outerSplitContainer.SplitterWidth = 6;
			this.outerSplitContainer.TabIndex = 5;
			this.innerSplitContainer.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.innerSplitContainer.Location = new global::System.Drawing.Point(0, 0);
			this.innerSplitContainer.Margin = new global::System.Windows.Forms.Padding(4, 5, 4, 5);
			this.innerSplitContainer.Name = "innerSplitContainer";
			this.innerSplitContainer.Orientation = global::System.Windows.Forms.Orientation.Horizontal;
			this.innerSplitContainer.Size = new global::System.Drawing.Size(1182, 1065);
			this.innerSplitContainer.SplitterDistance = 510;
			this.innerSplitContainer.SplitterWidth = 6;
			this.innerSplitContainer.TabIndex = 0;
			this.saveScenarioFolderAsToolStripMenuItem.Name = "saveScenarioFolderAsToolStripMenuItem";
			this.saveScenarioFolderAsToolStripMenuItem.Size = new global::System.Drawing.Size(315, 34);
			this.saveScenarioFolderAsToolStripMenuItem.Text = "Save Scenario Folder As...";
			this.saveScenarioFolderAsToolStripMenuItem.Click += new global::System.EventHandler(this.saveScenarioFolderAsToolStripMenuItem_Click);
			this.AllowDrop = true;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(9f, 20f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(1779, 1182);
			base.Controls.Add(this.outerSplitContainer);
			base.Controls.Add(this.toolBarPanel);
			base.Controls.Add(this.menuStrip);
			base.Controls.Add(this.statusStrip);
			base.Icon = (global::System.Drawing.Icon)componentResourceManager.GetObject("$this.Icon");
			base.KeyPreview = true;
			base.MainMenuStrip = this.menuStrip;
			base.Margin = new global::System.Windows.Forms.Padding(4, 5, 4, 5);
			this.MinimumSize = new global::System.Drawing.Size(136, 114);
			base.Name = "MainForm";
			this.Text = "Solar.Forge";
			base.DragDrop += new global::System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
			base.DragOver += new global::System.Windows.Forms.DragEventHandler(this.MainForm_DragOver);
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.outerSplitContainer.Panel2.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.outerSplitContainer).EndInit();
			this.outerSplitContainer.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.innerSplitContainer).EndInit();
			this.innerSplitContainer.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}


		private global::System.ComponentModel.IContainer components;


		private global::System.Windows.Forms.StatusStrip statusStrip;


		private global::System.Windows.Forms.MenuStrip menuStrip;


		private global::System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;


		private global::System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;


		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator1;


		private global::System.Windows.Forms.ToolStripMenuItem recentFilesToolStripMenuItem;


		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator2;


		private global::System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;


		private global::System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;


		private global::System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;


		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator3;


		private global::System.Windows.Forms.Panel toolBarPanel;


		private global::System.Windows.Forms.SplitContainer outerSplitContainer;


		private global::System.Windows.Forms.SplitContainer innerSplitContainer;


		private global::System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;


		private global::System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;


		private global::System.Windows.Forms.ToolStripMenuItem particleEffectMenu;


		private global::System.Windows.Forms.ToolStripMenuItem addParticleEffectNodeToolStripMenuItem;


		private global::System.Windows.Forms.ToolStripMenuItem addParticleEffectEmitterToolStripMenuItem;


		private global::System.Windows.Forms.ToolStripMenuItem addParticleEffectModifierToolStripMenuItem;


		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator4;


		private global::System.Windows.Forms.ToolStripMenuItem viewParticleEffectEditorMenuItem;


		private global::System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;


		private global::System.Windows.Forms.ToolStripMenuItem newParticleEffectToolStripMenuItem;


		private global::System.Windows.Forms.ToolStripMenuItem newBeamEffectToolStripMenuItem;


		private global::System.Windows.Forms.ToolStripMenuItem settingsMenuItem;


		private global::System.Windows.Forms.ToolStripMenuItem scenarioMenu;


		private global::System.Windows.Forms.ToolStripMenuItem bakeGalaxyChartPreviewMenuItem;


		private global::System.Windows.Forms.ToolStripMenuItem newScenarioMenuItem;


		private global::System.Windows.Forms.ToolStripMenuItem modsMenuItem;


		private global::System.Windows.Forms.ToolStripMenuItem editFillingsToolStripMenuItem;


		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator6;


		private global::System.Windows.Forms.ToolStripMenuItem newBrushMenuItem;


		private global::System.Windows.Forms.ToolStripStatusLabel editorStatusLabel;


		private global::System.Windows.Forms.ToolStripMenuItem meshMenu;


		private global::System.Windows.Forms.ToolStripMenuItem removeAllMeshesToolStripMenuItem;


		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator5;


		private global::System.Windows.Forms.ToolStripMenuItem keyBindingsToolStripMenuItem;


		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator7;


		private global::System.Windows.Forms.ToolStripMenuItem saveMaterialsToolStripMenuItem;


		private global::System.Windows.Forms.ToolStripMenuItem particleEffectUndoToolStripMenuItem;


		private global::System.Windows.Forms.ToolStripMenuItem particleEffectRedoToolStripMenuItem;


		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator8;


		private global::System.Windows.Forms.ToolStripMenuItem openScenarioFolderToolStripMenuItem;


		private global::System.Windows.Forms.ToolStripMenuItem viewMenu;


		private global::System.Windows.Forms.ToolStripMenuItem resetCameraToolStripMenuItem;


		private global::System.Windows.Forms.ToolStripMenuItem timeMenu;


		private global::System.Windows.Forms.ToolStripMenuItem resetTimeToolStripMenuItem;


		private global::System.Windows.Forms.ToolStripMenuItem pauseTimeToolStripMenuItem;


		private global::System.Windows.Forms.ToolStripMenuItem singleStepToolStripMenuItem;


		private global::System.Windows.Forms.ToolStripMenuItem faceCameraToolStripMenuItem;


		private global::System.Windows.Forms.ToolStripMenuItem downXAxisToolStripMenuItem;


		private global::System.Windows.Forms.ToolStripMenuItem downYAxisToolStripMenuItem;


		private global::System.Windows.Forms.ToolStripMenuItem downZAxisToolStripMenuItem;


		private global::System.Windows.Forms.ToolStripMenuItem saveScenarioFolderAsToolStripMenuItem;
	}
}
