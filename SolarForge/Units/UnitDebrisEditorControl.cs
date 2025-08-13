using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Solar.Simulations;

namespace SolarForge.Units
{

	public class UnitDebrisEditorControl : UserControl
	{

		public UnitDebrisEditorControl()
		{
			this.InitializeComponent();
		}



		public UnitModel Model
		{
			set
			{
				this.model = value;
				this.model.UnitDefinitionChanged += this.Model_UnitDefinitionChanged;
				this.model.SelectedSpawnCustomDebrisDefinitionChanged += this.Model_SelectedSpawnCustomDebrisDefinitionChanged;
			}
		}


		private void Model_SelectedSpawnCustomDebrisDefinitionChanged(SpawnCustomDebrisDefinition definition)
		{
			this.customDebrisPropertyGrid.SelectedObject = definition;
		}


		private void SyncCustomDebrisListBoxToModel()
		{
			this.customDebrisListBox.Items.Clear();
			if (this.model.UnitDefinition != null && this.model.UnitDefinition.SpawnDebris != null)
			{
				foreach (SpawnCustomDebrisDefinition spawnCustomDebrisDefinition in this.model.UnitDefinition.SpawnDebris.CustomDebrisList)
				{
					this.customDebrisListBox.Items.Add(spawnCustomDebrisDefinition.Unit.ToString());
				}
			}
			this.SyncModelSelectedSpawnCustomDebrisIndexToControl();
		}


		private void Model_UnitDefinitionChanged(UnitDefinition unitDefinition)
		{
			this.SyncCustomDebrisListBoxToModel();
			this.addCustomDebrisButton.Enabled = (this.model.UnitDefinition != null && this.model.UnitDefinition.SpawnDebris != null);
			this.removeCustomDebrisButton.Enabled = (this.model.UnitDefinition != null && this.model.UnitDefinition.SpawnDebris != null);
		}


		private void addCustomDebrisButton_Click(object sender, EventArgs e)
		{
			this.model.UnitDefinition.SpawnDebris.AddCustomDebris();
			this.SyncCustomDebrisListBoxToModel();
		}


		private void removeCustomDebrisButton_Click(object sender, EventArgs e)
		{
			if (this.customDebrisListBox.SelectedIndex != -1)
			{
				this.model.UnitDefinition.SpawnDebris.RemoveCustomDebrisAt(this.customDebrisListBox.SelectedIndex);
				this.SyncCustomDebrisListBoxToModel();
			}
		}


		private void customDebrisListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.SyncModelSelectedSpawnCustomDebrisIndexToControl();
		}


		private void SyncModelSelectedSpawnCustomDebrisIndexToControl()
		{
			int selectedIndex = this.customDebrisListBox.SelectedIndex;
			this.model.SelectedSpawnCustomDebrisDefinitionIndex = ((selectedIndex != -1) ? new int?(selectedIndex) : null);
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
			this.groupBox1 = new GroupBox();
			this.removeCustomDebrisButton = new Button();
			this.addCustomDebrisButton = new Button();
			this.customDebrisPropertyGrid = new PropertyGrid();
			this.customDebrisListBox = new ListBox();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.groupBox1.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.groupBox1.Controls.Add(this.removeCustomDebrisButton);
			this.groupBox1.Controls.Add(this.addCustomDebrisButton);
			this.groupBox1.Controls.Add(this.customDebrisPropertyGrid);
			this.groupBox1.Controls.Add(this.customDebrisListBox);
			this.groupBox1.Location = new Point(3, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(442, 532);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Specific Debris";
			this.removeCustomDebrisButton.Location = new Point(187, 26);
			this.removeCustomDebrisButton.Name = "removeCustomDebrisButton";
			this.removeCustomDebrisButton.Size = new Size(174, 53);
			this.removeCustomDebrisButton.TabIndex = 3;
			this.removeCustomDebrisButton.Text = "Remove";
			this.removeCustomDebrisButton.UseVisualStyleBackColor = true;
			this.removeCustomDebrisButton.Click += this.removeCustomDebrisButton_Click;
			this.addCustomDebrisButton.Location = new Point(7, 26);
			this.addCustomDebrisButton.Name = "addCustomDebrisButton";
			this.addCustomDebrisButton.Size = new Size(174, 53);
			this.addCustomDebrisButton.TabIndex = 2;
			this.addCustomDebrisButton.Text = "Add";
			this.addCustomDebrisButton.UseVisualStyleBackColor = true;
			this.addCustomDebrisButton.Click += this.addCustomDebrisButton_Click;
			this.customDebrisPropertyGrid.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.customDebrisPropertyGrid.Location = new Point(6, 255);
			this.customDebrisPropertyGrid.Name = "customDebrisPropertyGrid";
			this.customDebrisPropertyGrid.Size = new Size(430, 271);
			this.customDebrisPropertyGrid.TabIndex = 1;
			this.customDebrisListBox.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.customDebrisListBox.FormattingEnabled = true;
			this.customDebrisListBox.ItemHeight = 20;
			this.customDebrisListBox.Location = new Point(6, 85);
			this.customDebrisListBox.Name = "customDebrisListBox";
			this.customDebrisListBox.Size = new Size(430, 164);
			this.customDebrisListBox.TabIndex = 0;
			this.customDebrisListBox.SelectedIndexChanged += this.customDebrisListBox_SelectedIndexChanged;
			base.AutoScaleDimensions = new SizeF(9f, 20f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.groupBox1);
			base.Name = "UnitDebrisEditorControl";
			base.Size = new Size(462, 538);
			this.groupBox1.ResumeLayout(false);
			base.ResumeLayout(false);
		}


		private UnitModel model;


		private IContainer components;


		private GroupBox groupBox1;


		private Button removeCustomDebrisButton;


		private Button addCustomDebrisButton;


		private PropertyGrid customDebrisPropertyGrid;


		private ListBox customDebrisListBox;
	}
}
