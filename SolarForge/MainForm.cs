using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Solar.Scenarios;
using SolarForge.BeamEffects;
using SolarForge.Brushes;
using SolarForge.DeathSequences;
using SolarForge.Dialogs;
using SolarForge.GalaxyChartFillings;
using SolarForge.Gui;
using SolarForge.Meshes;
using SolarForge.ParticleEffects;
using SolarForge.Scenarios;
using SolarForge.Scenes;
using SolarForge.Units;

namespace SolarForge
{

	public partial class MainForm : Form
	{

		public MainForm()
		{
			this.InitializeComponent();
			this.scenarioViewportControl = new ScenarioViewportControl();
			this.scenarioViewportControl.Visible = false;
			this.scenarioViewportControl.Dock = DockStyle.Fill;
			this.innerSplitContainer.Panel1.Controls.Add(this.scenarioViewportControl);
			this.guiViewportControl = new GuiViewportControl();
			this.guiViewportControl.Visible = false;
			this.guiViewportControl.Dock = DockStyle.Fill;
			this.innerSplitContainer.Panel1.Controls.Add(this.guiViewportControl);
			this.viewportControl = new ViewportControl();
			this.viewportControl.Visible = true;
			this.viewportControl.Dock = DockStyle.Fill;
			this.innerSplitContainer.Panel1.Controls.Add(this.viewportControl);
			this.innerSplitContainer.Panel2.Hide();
			this.innerSplitContainer.Panel2Collapsed = true;
			this.particleEffectEditorControl = new ParticleEffectEditorControl();
			this.particleEffectEditorControl.Visible = false;
			this.particleEffectEditorControl.Dock = DockStyle.Fill;
			this.outerSplitContainer.Panel1.Controls.Add(this.particleEffectEditorControl);
			this.particleEffectToolBar = new SceneToolBar();
			this.particleEffectToolBar.Visible = false;
			this.particleEffectToolBar.Dock = DockStyle.Left;
			this.toolBarPanel.Controls.Add(this.particleEffectToolBar);
			this.beamEffectToolBar = new SceneToolBar();
			this.beamEffectToolBar.Visible = false;
			this.beamEffectToolBar.Dock = DockStyle.Left;
			this.toolBarPanel.Controls.Add(this.beamEffectToolBar);
			this.beamEffectEditorControl = new BeamEffectEditorControl();
			this.beamEffectEditorControl.Visible = false;
			this.beamEffectEditorControl.Dock = DockStyle.Fill;
			this.outerSplitContainer.Panel1.Controls.Add(this.beamEffectEditorControl);
			this.meshEditorControl = new MeshEditorControl();
			this.meshEditorControl.Visible = false;
			this.meshEditorControl.Dock = DockStyle.Fill;
			this.outerSplitContainer.Panel1.Controls.Add(this.meshEditorControl);
			this.scenarioEditorControl = new ScenarioEditorControl();
			this.scenarioEditorControl.Visible = false;
			this.scenarioEditorControl.Dock = DockStyle.Fill;
			this.outerSplitContainer.Panel1.Controls.Add(this.scenarioEditorControl);
			this.guiEditorControl = new GuiEditorControl();
			this.guiEditorControl.Visible = false;
			this.guiEditorControl.Dock = DockStyle.Fill;
			this.outerSplitContainer.Panel1.Controls.Add(this.guiEditorControl);
			this.guiToolBar = new GuiToollBar();
			this.guiToolBar.Visible = false;
			this.guiToolBar.Dock = DockStyle.Left;
			this.toolBarPanel.Controls.Add(this.guiToolBar);
			this.brushEditorControl = new BrushEditorControl();
			this.brushEditorControl.Visible = false;
			this.brushEditorControl.Dock = DockStyle.Fill;
			this.outerSplitContainer.Panel1.Controls.Add(this.brushEditorControl);
			this.unitEditorControl = new UnitEditorControl();
			this.unitEditorControl.Visible = false;
			this.unitEditorControl.Dock = DockStyle.Fill;
			this.outerSplitContainer.Panel1.Controls.Add(this.unitEditorControl);
			this.deathSequenceEditorControl = new DeathSequenceEditorControl();
			this.deathSequenceEditorControl.Visible = false;
			this.deathSequenceEditorControl.Dock = DockStyle.Fill;
			this.outerSplitContainer.Panel1.Controls.Add(this.deathSequenceEditorControl);
			this.deathSequenceToolBar = new SceneToolBar();
			this.deathSequenceToolBar.Visible = false;
			this.deathSequenceToolBar.Dock = DockStyle.Left;
			this.toolBarPanel.Controls.Add(this.deathSequenceToolBar);
			int maxEntries = 10;
			this.mruMenu = new MruStripMenu(this.recentFilesToolStripMenuItem, delegate(int number, string fileName)
			{
				this.HandleMruMenuClick(fileName);
			}, MainForm.MRU_REGISTRY_KEY_NAME, maxEntries);
			this.editorStatusLabel.Alignment = ToolStripItemAlignment.Right;
		}


		private void HandleMruMenuClick(string fileName)
		{
			if (Directory.Exists(fileName))
			{
				this.programModel.LoadScenarioFolder(fileName);
				return;
			}
			this.programModel.LoadFile(fileName, true);
		}



		public ProgramModel ProgramModel
		{
			set
			{
				this.programModel = value;
				this.viewportControl.ProgramModel = value;
				this.particleEffectEditorControl.Model = this.programModel.ParticleEffectModel;
				this.particleEffectToolBar.Model = this.programModel.ParticleEffectModel;
				this.beamEffectToolBar.Model = this.programModel.BeamEffectModel;
				this.beamEffectEditorControl.Model = this.programModel.BeamEffectModel;
				this.meshEditorControl.Model = this.programModel.MeshModel;
				this.scenarioEditorControl.Model = this.programModel.ScenarioModel;
				this.scenarioViewportControl.Model = this.programModel.ScenarioModel;
				this.guiEditorControl.Model = this.programModel.GuiModel;
				this.guiViewportControl.Model = this.programModel.GuiModel;
				this.guiToolBar.Model = this.programModel.GuiModel;
				this.brushEditorControl.Model = this.programModel.BrushModel;
				this.unitEditorControl.Model = this.programModel.UnitModel;
				this.deathSequenceEditorControl.Model = this.programModel.DeathSequenceModel;
				this.deathSequenceToolBar.Model = this.programModel.DeathSequenceModel;
				this.SyncTitleTextToModel();
				this.SyncMenuItemsToModel();
				this.SyncControlVisibilityToModel();
				this.programModel.CurrentFileOrFolderPathChanged += delegate(string path)
				{
					this.SyncTitleTextToModel();
				};
				this.programModel.CurrentEditorTypeChanged += delegate(EditorType? editorType)
				{
					this.SyncTitleTextToModel();
					this.SyncMenuItemsToModel();
					this.SyncControlVisibilityToModel();
				};
				this.programModel.ParticleEffectModel.UndoChanged += delegate()
				{
					this.SyncParticleEffectMenuItemsToModel();
				};
				this.programModel.HasUnsavedChangesChanged += delegate()
				{
					this.SyncTitleTextToModel();
				};
				this.programModel.ScenarioModel.SelectedGalaxyChartComponentChanged += this.ScenarioModel_SelectedGalaxyChartComponentChanged;
				this.programModel.ScenarioModel.GalaxyChartChanged += this.ScenarioModel_GalaxyChartChanged;
			}
		}


		private void ScenarioModel_GalaxyChartChanged(GalaxyChart galaxyChart, bool isUndo)
		{
			this.SyncControlVisibilityToModel();
		}


		private void ScenarioModel_SelectedGalaxyChartComponentChanged(object selectedComponent)
		{
			this.SyncMenuItemsToModel();
		}


		private void SyncTitleTextToModel()
		{
			string text = "Solar.Forge";
			if (this.programModel.CurrentEditorType != null)
			{
				text += string.Format(" [{0}]", this.programModel.CurrentEditorType.Value);
			}
			if (this.programModel.CurrentFileOrFolderPath != null)
			{
				text = text + " - " + this.programModel.CurrentFileOrFolderPath;
			}
			if (this.programModel.HasUnsavedChanges)
			{
				text += " *";
			}
			this.Text = text;
		}


		private void SyncMenuItemsToModel()
		{
			this.saveToolStripMenuItem.Enabled = this.programModel.CanSave;
			this.saveAsToolStripMenuItem.Enabled = this.programModel.CanSave;
			this.saveScenarioFolderAsToolStripMenuItem.Enabled = (this.programModel.CurrentEditorType.GetValueOrDefault() == EditorType.Scenario);
			this.pauseTimeToolStripMenuItem.Checked = (this.programModel.CurrentSceneModel != null && this.programModel.CurrentSceneModel.IsPaused);
			this.singleStepToolStripMenuItem.Enabled = this.pauseTimeToolStripMenuItem.Checked;
			this.SyncParticleEffectMenuItemsToModel();
		}


		private void SyncParticleEffectMenuItemsToModel()
		{
			this.particleEffectUndoToolStripMenuItem.Enabled = this.programModel.ParticleEffectModel.CanUndo;
			this.particleEffectRedoToolStripMenuItem.Enabled = this.programModel.ParticleEffectModel.CanRedo;
		}


		private void SyncControlVisibilityToModel()
		{
			EditorType? currentEditorType = this.programModel.CurrentEditorType;
			EditorType? editorType = currentEditorType;
			EditorType editorType2 = EditorType.ParticleEffect;
			bool flag = editorType.GetValueOrDefault() == editorType2 & editorType != null;
			bool flag2 = flag;
			bool flag3 = this.particleEffectEditorControlShouldBeVisible && flag;
			bool flag4 = currentEditorType.GetValueOrDefault() == EditorType.BeamEffect;
			bool flag5 = flag4;
			bool flag6 = this.beamEffectEditorControlShouldBeVisible && flag4;
			bool flag7 = currentEditorType.GetValueOrDefault() == EditorType.Mesh;
			bool flag8 = this.meshEditorControlShouldBeVisible && flag7;
			bool flag9 = currentEditorType.GetValueOrDefault() == EditorType.Scenario;
			bool flag10 = flag9;
			bool flag11 = currentEditorType.GetValueOrDefault() == EditorType.Gui;
			bool flag12 = flag11;
			bool flag13 = flag11;
			bool flag14 = currentEditorType.GetValueOrDefault() == EditorType.Brush;
			bool flag15 = currentEditorType.GetValueOrDefault() == EditorType.Unit;
			bool flag17;
			bool flag16 = flag17 = (currentEditorType.GetValueOrDefault() == EditorType.DeathSequence);
			bool visible = flag2 || flag5 || flag12 || flag17;
			bool anyOuterPanel1ControlsVisible = flag3 || flag6 || flag8 || flag10 || flag13 || flag14 || flag15 || flag16;
			bool anyInnerPanel2ControlsVisible = false;
			this.UpdateSplitContainers(anyOuterPanel1ControlsVisible, anyInnerPanel2ControlsVisible);
			this.toolBarPanel.Visible = visible;
			this.particleEffectMenu.Visible = flag;
			this.particleEffectToolBar.Visible = flag2;
			this.particleEffectEditorControl.Visible = flag3;
			this.beamEffectToolBar.Visible = flag5;
			this.beamEffectEditorControl.Visible = flag6;
			this.meshMenu.Visible = flag7;
			this.meshEditorControl.Visible = flag8;
			this.scenarioMenu.Visible = flag9;
			this.bakeGalaxyChartPreviewMenuItem.Enabled = this.programModel.ScenarioModel.CanBakeGalaxyChartPreview;
			this.scenarioViewportControl.Visible = flag9;
			this.scenarioEditorControl.Visible = flag10;
			this.guiViewportControl.Visible = flag11;
			this.guiEditorControl.Visible = flag13;
			this.guiToolBar.Visible = flag12;
			this.brushEditorControl.Visible = flag14;
			this.unitEditorControl.Visible = flag15;
			this.deathSequenceEditorControl.Visible = flag16;
			this.deathSequenceToolBar.Visible = flag17;
			this.viewParticleEffectEditorMenuItem.Enabled = flag;
			this.viewParticleEffectEditorMenuItem.Checked = this.particleEffectEditorControlShouldBeVisible;
			this.viewMenu.Visible = (this.programModel.CurrentSceneModel3d != null);
			this.timeMenu.Visible = (flag || flag4);
		}



		public string EditorStatusText
		{
			set
			{
				this.editorStatusLabel.Text = value;
			}
		}


		private void UpdateSplitContainers(bool anyOuterPanel1ControlsVisible, bool anyInnerPanel2ControlsVisible)
		{
			if (anyOuterPanel1ControlsVisible)
			{
				this.outerSplitContainer.Panel1Collapsed = false;
				this.outerSplitContainer.Panel1.Show();
			}
			else
			{
				this.outerSplitContainer.Panel1Collapsed = true;
				this.outerSplitContainer.Panel1.Hide();
			}
			if (anyInnerPanel2ControlsVisible)
			{
				this.innerSplitContainer.Panel2Collapsed = false;
				this.innerSplitContainer.Panel2.Show();
				return;
			}
			this.innerSplitContainer.Panel2Collapsed = true;
			this.innerSplitContainer.Panel2.Hide();
		}



		public ViewportControl ViewportControl
		{
			get
			{
				return this.viewportControl;
			}
		}


		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string text = this.programModel.BrowseToAndLoadFile();
			if (text != null)
			{
				this.mruMenu.AddFile(text);
				this.mruMenu.SaveToRegistry();
			}
		}


		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.programModel.Save();
		}


		private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.programModel.SaveFileAs();
		}


		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}


		private void viewParticleEffectEditorMenuItem_Click(object sender, EventArgs e)
		{
			this.particleEffectEditorControlShouldBeVisible = !this.particleEffectEditorControlShouldBeVisible;
			this.SyncControlVisibilityToModel();
		}


		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new AboutDialog().ShowDialog();
		}


		private void addParticleEffectNodeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.programModel.ParticleEffectModel.AddNode();
		}


		private void addParticleEffectEmitterToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.programModel.ParticleEffectModel.AddEmitter();
		}


		private void addParticleEffectModifierToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.programModel.ParticleEffectModel.AddModifier();
		}


		private void newParticleEffectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.programModel.CreateNewParticleEffect();
		}


		private void newBeamEffectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.programModel.CreateNewBeamEffect();
		}


		private void settingsMenuItem_Click(object sender, EventArgs e)
		{
			if (this.settingsDialog == null)
			{
				this.settingsDialog = new SettingsDialog();
				this.settingsDialog.Owner = this;
				this.settingsDialog.Settings = this.programModel.Settings;
				this.settingsDialog.SetActiveTabToFileType(this.programModel.CurrentEditorType);
				this.settingsDialog.FormClosed += delegate(object fcSender, FormClosedEventArgs fcArgs)
				{
					this.settingsDialog = null;
				};
			}
			this.settingsDialog.Show();
		}


		private void bakeGalaxyChartPreviewMenuItem_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show(this, "This operation will PERMANENTLY change the scenario to no longer be randomly generated. Are you sure?", "Bake Preview Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
			{
				this.programModel.ScenarioModel.BakeGalaxyChartPreview();
			}
		}


		private void newScenarioMenuItem_Click(object sender, EventArgs e)
		{
			this.programModel.CreateNewScenario();
		}


		private void modsMenuItem_Click(object sender, EventArgs e)
		{
			if (this.moddingDialog == null)
			{
				this.moddingDialog = new ModdingDialog();
				this.moddingDialog.Owner = this;
				this.moddingDialog.ModdingSystem = this.programModel.Engine.ModdingSystem;
				this.moddingDialog.FormClosed += delegate(object fcSender, FormClosedEventArgs fcArgs)
				{
					this.moddingDialog = null;
				};
			}
			this.moddingDialog.Show();
		}


		private void editFillingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (GalaxyChartFillingsEditorDialog galaxyChartFillingsEditorDialog = new GalaxyChartFillingsEditorDialog())
			{
				galaxyChartFillingsEditorDialog.Model = this.programModel.NewGalaxyChartFillingsEditorModel();
				galaxyChartFillingsEditorDialog.ShowDialog(this);
			}
		}


		private string GetFilePathFromDragEventArgs(DragEventArgs e)
		{
			string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
			if (array.Length != 0 && this.programModel.TryGetFileTypeFromPathExtension(array[0]) != null)
			{
				return array[0];
			}
			return null;
		}


		private void MainForm_DragOver(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop, false) && this.GetFilePathFromDragEventArgs(e) != null)
			{
				e.Effect = DragDropEffects.Move;
			}
		}


		private void MainForm_DragDrop(object sender, DragEventArgs e)
		{
			string filePathFromDragEventArgs = this.GetFilePathFromDragEventArgs(e);
			if (filePathFromDragEventArgs != null)
			{
				this.programModel.LoadFile(filePathFromDragEventArgs, true);
				this.mruMenu.AddFile(filePathFromDragEventArgs);
				this.mruMenu.SaveToRegistry();
			}
		}


		private void newBrushMenuItem_Click(object sender, EventArgs e)
		{
			this.programModel.CreateNewBrush();
		}


		private void removeAllMeshesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.programModel.MeshModel.DisposeMeshes(true);
		}


		private void keyBindingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new KeyBindingsDialog().ShowDialog();
		}


		private void saveMaterialsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.programModel.MeshModel.SaveMaterialChanges();
		}


		private void undoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.programModel.ParticleEffectModel.TryUndo();
		}


		private void redoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.programModel.ParticleEffectModel.TryRedo();
		}


		private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string text = this.programModel.BrowseToAndLoadScenarioFolder();
			if (text != null)
			{
				this.mruMenu.AddFile(text);
				this.mruMenu.SaveToRegistry();
			}
		}


		private void defaultResetCameraToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SceneModel3d currentSceneModel3d = this.programModel.CurrentSceneModel3d;
			if (currentSceneModel3d == null)
			{
				return;
			}
			currentSceneModel3d.ResetCamera();
		}


		private void forwardResetCameraToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SceneModel3d currentSceneModel3d = this.programModel.CurrentSceneModel3d;
			if (currentSceneModel3d == null)
			{
				return;
			}
			currentSceneModel3d.ResetCameraWithPitchYaw(0f, 0f);
		}


		private void rightResetCameraToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SceneModel3d currentSceneModel3d = this.programModel.CurrentSceneModel3d;
			if (currentSceneModel3d == null)
			{
				return;
			}
			currentSceneModel3d.ResetCameraWithPitchYaw(0f, -90f);
		}


		private void leftResetCameraToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SceneModel3d currentSceneModel3d = this.programModel.CurrentSceneModel3d;
			if (currentSceneModel3d == null)
			{
				return;
			}
			currentSceneModel3d.ResetCameraWithPitchYaw(0f, 90f);
		}


		private void topToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SceneModel3d currentSceneModel3d = this.programModel.CurrentSceneModel3d;
			if (currentSceneModel3d == null)
			{
				return;
			}
			currentSceneModel3d.ResetCameraWithPitchYaw(90f, 0f);
		}


		private void resetTimeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SceneModel currentSceneModel = this.programModel.CurrentSceneModel;
			if (currentSceneModel == null)
			{
				return;
			}
			currentSceneModel.ResetTime();
		}


		private void pauseTimeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SceneModel currentSceneModel = this.programModel.CurrentSceneModel;
			if (currentSceneModel != null)
			{
				currentSceneModel.ToggleIsPaused();
			}
			this.SyncMenuItemsToModel();
		}


		private void singleStepToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SceneModel currentSceneModel = this.programModel.CurrentSceneModel;
			if (currentSceneModel == null)
			{
				return;
			}
			currentSceneModel.SingleStep(1);
		}


		private void resetCameraToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SceneModel3d currentSceneModel3d = this.programModel.CurrentSceneModel3d;
			if (currentSceneModel3d == null)
			{
				return;
			}
			currentSceneModel3d.ResetCamera();
		}


		private void downXAxisToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SceneModel3d currentSceneModel3d = this.programModel.CurrentSceneModel3d;
			if (currentSceneModel3d == null)
			{
				return;
			}
			currentSceneModel3d.ResetCameraWithPitchYaw(0f, 90f);
		}


		private void downYAxisToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SceneModel3d currentSceneModel3d = this.programModel.CurrentSceneModel3d;
			if (currentSceneModel3d == null)
			{
				return;
			}
			currentSceneModel3d.ResetCameraWithPitchYaw(90f, 0f);
		}


		private void downZAxisToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SceneModel3d currentSceneModel3d = this.programModel.CurrentSceneModel3d;
			if (currentSceneModel3d == null)
			{
				return;
			}
			currentSceneModel3d.ResetCameraWithPitchYaw(0f, 0f);
		}


		private void saveScenarioFolderAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.programModel.SaveScenarioToFolderAs();
		}


		private static string MRU_REGISTRY_KEY_NAME = "SolarForgeMRU";


		private ProgramModel programModel;


		private ViewportControl viewportControl;


		private MruStripMenu mruMenu;


		private bool particleEffectEditorControlShouldBeVisible = true;


		private ParticleEffectEditorControl particleEffectEditorControl;


		private SceneToolBar particleEffectToolBar;


		private SceneToolBar beamEffectToolBar;


		private bool beamEffectEditorControlShouldBeVisible = true;


		private BeamEffectEditorControl beamEffectEditorControl;


		private bool meshEditorControlShouldBeVisible = true;


		private MeshEditorControl meshEditorControl;


		private ScenarioEditorControl scenarioEditorControl;


		private ScenarioViewportControl scenarioViewportControl;


		private GuiEditorControl guiEditorControl;


		private GuiViewportControl guiViewportControl;


		private GuiToollBar guiToolBar;


		private BrushEditorControl brushEditorControl;


		private UnitEditorControl unitEditorControl;


		private DeathSequenceEditorControl deathSequenceEditorControl;


		private SceneToolBar deathSequenceToolBar;


		private SettingsDialog settingsDialog;


		private ModdingDialog moddingDialog;
	}
}
