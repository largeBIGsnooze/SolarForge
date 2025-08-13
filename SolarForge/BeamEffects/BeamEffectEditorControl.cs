using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Solar.Rendering;

namespace SolarForge.BeamEffects
{

	public class BeamEffectEditorControl : UserControl
	{

		public BeamEffectEditorControl()
		{
			this.InitializeComponent();
			this.particleEffectListBox.SelectedValueChanged += this.ParticleEffectListBox_SelectedValueChanged;
		}


		private void ParticleEffectListBox_SelectedValueChanged(object sender, EventArgs e)
		{
			this.model.SelectedParticleEffectDefinition = (BeamEffectParticleEffectDefinition)this.particleEffectListBox.SelectedItem;
		}



		public BeamEffectModel Model
		{
			set
			{
				this.model = value;
				this.designPropertyGrid.SelectedObject = this.model.Settings;
				this.model.BeamEffectChanged += delegate(BeamEffectDefinition beamEffectDefinition)
				{
					this.definitionPropertyGrid.SelectedObject = beamEffectDefinition;
				};
				this.model.ParticleEffectsChanged += this.Model_ParticleEffectsChanged;
				this.model.SelectedParticleEffectChanged += this.Model_SelectedParticleEffectChanged;
			}
		}


		private void Model_SelectedParticleEffectChanged(BeamEffectParticleEffectDefinition selectedParticleEffect)
		{
			this.particleEffectPropertyGrid.SelectedObject = selectedParticleEffect;
			this.removeParticleEffectButton.Enabled = (selectedParticleEffect != null);
		}


		private void Model_ParticleEffectsChanged(BeamEffectDefinition beamEffectDefinition)
		{
			this.particleEffectListBox.Items.Clear();
			for (int i = 0; i < beamEffectDefinition.ParticleEffects.Count; i++)
			{
				this.particleEffectListBox.Items.Add(beamEffectDefinition.ParticleEffects[i]);
			}
		}


		private void addParticleEffectButton_Click(object sender, EventArgs e)
		{
			this.model.AddParticleEffect();
		}


		private void removeParticleEffectButton_Click(object sender, EventArgs e)
		{
			this.model.RemoveParticleEffect(this.model.SelectedParticleEffectDefinition);
		}


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
			this.tabControl = new TabControl();
			this.settingsTabPage = new TabPage();
			this.beamPropertiesTabPage = new TabPage();
			this.designPropertyGrid = new PropertyGrid();
			this.definitionPropertyGrid = new PropertyGrid();
			this.particleEffectsTabPage = new TabPage();
			this.addParticleEffectButton = new Button();
			this.removeParticleEffectButton = new Button();
			this.particleEffectListBox = new ListBox();
			this.particleEffectPropertyGrid = new PropertyGrid();
			this.tabControl.SuspendLayout();
			this.settingsTabPage.SuspendLayout();
			this.beamPropertiesTabPage.SuspendLayout();
			this.particleEffectsTabPage.SuspendLayout();
			base.SuspendLayout();
			this.tabControl.Controls.Add(this.settingsTabPage);
			this.tabControl.Controls.Add(this.beamPropertiesTabPage);
			this.tabControl.Controls.Add(this.particleEffectsTabPage);
			this.tabControl.Dock = DockStyle.Fill;
			this.tabControl.Location = new Point(0, 0);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new Size(544, 635);
			this.tabControl.TabIndex = 3;
			this.settingsTabPage.Controls.Add(this.designPropertyGrid);
			this.settingsTabPage.Location = new Point(4, 29);
			this.settingsTabPage.Name = "settingsTabPage";
			this.settingsTabPage.Padding = new Padding(3);
			this.settingsTabPage.Size = new Size(536, 602);
			this.settingsTabPage.TabIndex = 0;
			this.settingsTabPage.Text = "Settings";
			this.settingsTabPage.UseVisualStyleBackColor = true;
			this.beamPropertiesTabPage.Controls.Add(this.definitionPropertyGrid);
			this.beamPropertiesTabPage.Location = new Point(4, 29);
			this.beamPropertiesTabPage.Name = "beamPropertiesTabPage";
			this.beamPropertiesTabPage.Padding = new Padding(3);
			this.beamPropertiesTabPage.Size = new Size(536, 602);
			this.beamPropertiesTabPage.TabIndex = 1;
			this.beamPropertiesTabPage.Text = "Beam Properties";
			this.beamPropertiesTabPage.UseVisualStyleBackColor = true;
			this.designPropertyGrid.Dock = DockStyle.Fill;
			this.designPropertyGrid.Location = new Point(3, 3);
			this.designPropertyGrid.Margin = new Padding(4, 5, 4, 5);
			this.designPropertyGrid.Name = "designPropertyGrid";
			this.designPropertyGrid.PropertySort = PropertySort.NoSort;
			this.designPropertyGrid.Size = new Size(530, 596);
			this.designPropertyGrid.TabIndex = 1;
			this.designPropertyGrid.ToolbarVisible = false;
			this.definitionPropertyGrid.Dock = DockStyle.Fill;
			this.definitionPropertyGrid.Location = new Point(3, 3);
			this.definitionPropertyGrid.Margin = new Padding(4, 5, 4, 5);
			this.definitionPropertyGrid.Name = "definitionPropertyGrid";
			this.definitionPropertyGrid.Size = new Size(530, 596);
			this.definitionPropertyGrid.TabIndex = 2;
			this.definitionPropertyGrid.ToolbarVisible = false;
			this.particleEffectsTabPage.Controls.Add(this.particleEffectPropertyGrid);
			this.particleEffectsTabPage.Controls.Add(this.particleEffectListBox);
			this.particleEffectsTabPage.Controls.Add(this.removeParticleEffectButton);
			this.particleEffectsTabPage.Controls.Add(this.addParticleEffectButton);
			this.particleEffectsTabPage.Location = new Point(4, 29);
			this.particleEffectsTabPage.Name = "particleEffectsTabPage";
			this.particleEffectsTabPage.Size = new Size(536, 602);
			this.particleEffectsTabPage.TabIndex = 2;
			this.particleEffectsTabPage.Text = "Particle Effects";
			this.particleEffectsTabPage.UseVisualStyleBackColor = true;
			this.addParticleEffectButton.Location = new Point(19, 14);
			this.addParticleEffectButton.Name = "addParticleEffectButton";
			this.addParticleEffectButton.Size = new Size(125, 38);
			this.addParticleEffectButton.TabIndex = 0;
			this.addParticleEffectButton.Text = "Add";
			this.addParticleEffectButton.UseVisualStyleBackColor = true;
			this.addParticleEffectButton.Click += this.addParticleEffectButton_Click;
			this.removeParticleEffectButton.Location = new Point(160, 14);
			this.removeParticleEffectButton.Name = "removeParticleEffectButton";
			this.removeParticleEffectButton.Size = new Size(125, 38);
			this.removeParticleEffectButton.TabIndex = 1;
			this.removeParticleEffectButton.Text = "Remove";
			this.removeParticleEffectButton.UseVisualStyleBackColor = true;
			this.removeParticleEffectButton.Click += this.removeParticleEffectButton_Click;
			this.particleEffectListBox.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.particleEffectListBox.FormattingEnabled = true;
			this.particleEffectListBox.ItemHeight = 20;
			this.particleEffectListBox.Location = new Point(27, 71);
			this.particleEffectListBox.Name = "particleEffectListBox";
			this.particleEffectListBox.Size = new Size(478, 244);
			this.particleEffectListBox.TabIndex = 2;
			this.particleEffectPropertyGrid.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.particleEffectPropertyGrid.Location = new Point(27, 321);
			this.particleEffectPropertyGrid.Name = "particleEffectPropertyGrid";
			this.particleEffectPropertyGrid.Size = new Size(478, 263);
			this.particleEffectPropertyGrid.TabIndex = 3;
			base.AutoScaleDimensions = new SizeF(9f, 20f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.tabControl);
			base.Margin = new Padding(4, 5, 4, 5);
			base.Name = "BeamEffectEditorControl";
			base.Size = new Size(544, 635);
			this.tabControl.ResumeLayout(false);
			this.settingsTabPage.ResumeLayout(false);
			this.beamPropertiesTabPage.ResumeLayout(false);
			this.particleEffectsTabPage.ResumeLayout(false);
			base.ResumeLayout(false);
		}


		private BeamEffectModel model;


		private IContainer components;


		private TabControl tabControl;


		private TabPage settingsTabPage;


		private PropertyGrid designPropertyGrid;


		private TabPage beamPropertiesTabPage;


		private PropertyGrid definitionPropertyGrid;


		private TabPage particleEffectsTabPage;


		private PropertyGrid particleEffectPropertyGrid;


		private ListBox particleEffectListBox;


		private Button removeParticleEffectButton;


		private Button addParticleEffectButton;
	}
}
