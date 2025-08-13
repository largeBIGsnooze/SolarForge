using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SolarForge
{

	public partial class SettingsDialog : Form
	{

		public SettingsDialog()
		{
			this.InitializeComponent();
		}



		public ProgramSettings Settings
		{
			set
			{
				this.settings = value;
				this.corePropertyGrid.SelectedObject = value;
				this.postProcessingPropertyGrid.SelectedObject = value.PostProcessingParams;
				this.cameraPropertyGrid.SelectedObject = value.CameraSettings;
				this.meshPropertyGrid.SelectedObject = value.MeshSettings;
				this.unitPropertyGrid.SelectedObject = value.UnitSettings;
				this.brushPropertyGrid.SelectedObject = value.BrushSettings;
				this.skyboxPropertyGrid.SelectedObject = value.SkyboxSettings;
				this.scenarioPropertyGrid.SelectedObject = value.ScenarioSettings;
				this.particleEffectPropertyGrid.SelectedObject = value.ParticleEffectSettings;
				this.beamEffectPropertyGrid.SelectedObject = value.BeamEffectSettings;
				this.guiPropertyGrid.SelectedObject = value.GuiSettings;
				this.deathSequencePropertyGrid.SelectedObject = value.DeathSequenceSettings;
				this.settingsFolderTextBox.Text = Path.GetDirectoryName(value.LoadedValuesFilePath);
			}
		}


		public void SetActiveTabToFileType(EditorType? editorType)
		{
			if (editorType != null)
			{
				switch (editorType.Value)
				{
				case EditorType.ParticleEffect:
					this.tabControl.SelectedTab = this.particleEffectTabPage;
					return;
				case EditorType.BeamEffect:
					this.tabControl.SelectedTab = this.beamEffectTabPage;
					return;
				case EditorType.Mesh:
					this.tabControl.SelectedTab = this.meshTabPage;
					return;
				case EditorType.Skybox:
					this.tabControl.SelectedTab = this.skyboxTabPage;
					break;
				case EditorType.Scenario:
					this.tabControl.SelectedTab = this.scenarioTabPage;
					return;
				case EditorType.Gui:
					this.tabControl.SelectedTab = this.guiTabPage;
					return;
				case EditorType.Brush:
					this.tabControl.SelectedTab = this.brushTabPage;
					return;
				case EditorType.Unit:
					this.tabControl.SelectedTab = this.unitTabPage;
					return;
				default:
					return;
				}
			}
		}


		private void resetSettingsButton_Click(object sender, EventArgs e)
		{
			this.settings.Reset();
			this.corePropertyGrid.Refresh();
			this.postProcessingPropertyGrid.Refresh();
			this.cameraPropertyGrid.Refresh();
			this.meshPropertyGrid.Refresh();
			this.unitPropertyGrid.Refresh();
			this.brushPropertyGrid.Refresh();
			this.skyboxPropertyGrid.Refresh();
			this.scenarioPropertyGrid.Refresh();
			this.particleEffectPropertyGrid.Refresh();
			this.beamEffectPropertyGrid.Refresh();
			this.guiPropertyGrid.Refresh();
			this.deathSequencePropertyGrid.Refresh();
		}


		private void closeButton_Click(object sender, EventArgs e)
		{
			base.Close();
		}


		protected override void OnKeyUp(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				base.Close();
				return;
			}
			base.OnKeyUp(e);
		}


		private ProgramSettings settings;
	}
}
