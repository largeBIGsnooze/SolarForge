using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Solar.Rendering;
using Solar.Simulations;

namespace SolarForge.Units
{

	public class UnitSkinsEditorControl : UserControl
	{

		public UnitSkinsEditorControl()
		{
			this.InitializeComponent();
		}



		public UnitModel Model
		{
			set
			{
				this.model = value;
				this.model.UnitSkinDefinitionsChanged += this.Model_UnitSkinDefinitionsChanged;
				this.model.SelectedUnitSkinDefinitionChanged += this.Model_SelectedUnitSkinDefinitionChanged;
				this.model.SelectedUnitSkinStageDefinitionChanged += this.Model_SelectedUnitSkinStageDefinitionChanged;
				this.model.SkinStageMeshChanged += this.Model_SkinStageMeshChanged;
			}
		}


		private void Model_SkinStageMeshChanged(Mesh skinStageMesh)
		{
			this.meshPropertyGrid.SelectedObject = ((skinStageMesh != null) ? skinStageMesh.Data : null);
			this.meshPointsListBox.Items.Clear();
			if (skinStageMesh != null)
			{
				foreach (MeshPoint meshPoint in skinStageMesh.Data.Points)
				{
					this.meshPointsListBox.Items.Add(meshPoint.Name);
				}
			}
		}


		private void Model_SelectedUnitSkinStageDefinitionChanged(UnitSkinStageDefinition selectedUnitSkinStageDefinition)
		{
			if (((selectedUnitSkinStageDefinition != null) ? selectedUnitSkinStageDefinition.UnitMesh : null) != null)
			{
				this.meshDefinitionPropertyGrid.SelectedObject = selectedUnitSkinStageDefinition.UnitMesh;
				return;
			}
			if (((selectedUnitSkinStageDefinition != null) ? selectedUnitSkinStageDefinition.PlanetMesh : null) != null)
			{
				this.meshDefinitionPropertyGrid.SelectedObject = selectedUnitSkinStageDefinition.PlanetMesh;
				return;
			}
			this.meshDefinitionPropertyGrid.SelectedObject = null;
		}


		private void Model_SelectedUnitSkinDefinitionChanged(UnitSkinDefinition selectedUnitSkinDefinition)
		{
			List<int> list = new List<int>();
			if (selectedUnitSkinDefinition != null)
			{
				for (int i = 0; i < selectedUnitSkinDefinition.SkinStages.Count; i++)
				{
					list.Add(i);
				}
			}
			this.skinStageComboBox.DataSource = list;
			this.SyncModelSelectedUnitSkinStageToSkinStageComboBox();
		}


		private void Model_UnitSkinDefinitionsChanged(IEnumerable<UnitSkinDefinition> unitSkinDefinitions)
		{
			this.skinComboBox.DataSource = unitSkinDefinitions;
			this.skinComboBox.SelectedItem = unitSkinDefinitions.FirstOrDefault<UnitSkinDefinition>();
			this.SyncModelSelectedUnitSkinDefinitionToSkinComboBox();
		}


		public void SyncModelSelectedUnitSkinDefinitionToSkinComboBox()
		{
			this.model.SelectedUnitSkinDefinition = (UnitSkinDefinition)this.skinComboBox.SelectedItem;
		}


		private void skinComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.SyncModelSelectedUnitSkinDefinitionToSkinComboBox();
		}


		public void SyncModelSelectedUnitSkinStageToSkinStageComboBox()
		{
			int? selectedUnitSkinStageIndex = null;
			object selectedItem = this.skinStageComboBox.SelectedItem;
			if (selectedItem != null)
			{
				selectedUnitSkinStageIndex = new int?((int)selectedItem);
			}
			this.model.SelectedUnitSkinStageIndex = selectedUnitSkinStageIndex;
		}


		private void skinStageComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.SyncModelSelectedUnitSkinStageToSkinStageComboBox();
		}


		private void meshPointsListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.model.SelectedSkinStageMeshPointIndex = this.meshPointsListBox.SelectedIndex;
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
			this.label1 = new Label();
			this.skinComboBox = new ComboBox();
			this.skinStageComboBox = new ComboBox();
			this.label2 = new Label();
			this.meshDefinitionGroupBox = new GroupBox();
			this.meshDefinitionPropertyGrid = new PropertyGrid();
			this.meshPointsGroupBox = new GroupBox();
			this.meshPointsListBox = new ListBox();
			this.meshPropertiesGroupBox = new GroupBox();
			this.meshPropertyGrid = new PropertyGrid();
			this.meshDefinitionGroupBox.SuspendLayout();
			this.meshPointsGroupBox.SuspendLayout();
			this.meshPropertiesGroupBox.SuspendLayout();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Location = new Point(4, 9);
			this.label1.Margin = new Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new Size(40, 20);
			this.label1.TabIndex = 4;
			this.label1.Text = "Skin";
			this.skinComboBox.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.skinComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			this.skinComboBox.FormattingEnabled = true;
			this.skinComboBox.Location = new Point(66, 5);
			this.skinComboBox.Margin = new Padding(4, 5, 4, 5);
			this.skinComboBox.Name = "skinComboBox";
			this.skinComboBox.Size = new Size(388, 28);
			this.skinComboBox.TabIndex = 3;
			this.skinComboBox.SelectedIndexChanged += this.skinComboBox_SelectedIndexChanged;
			this.skinStageComboBox.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.skinStageComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			this.skinStageComboBox.FormattingEnabled = true;
			this.skinStageComboBox.Location = new Point(66, 46);
			this.skinStageComboBox.Margin = new Padding(4, 5, 4, 5);
			this.skinStageComboBox.Name = "skinStageComboBox";
			this.skinStageComboBox.Size = new Size(388, 28);
			this.skinStageComboBox.TabIndex = 5;
			this.skinStageComboBox.SelectedIndexChanged += this.skinStageComboBox_SelectedIndexChanged;
			this.label2.AutoSize = true;
			this.label2.Location = new Point(4, 51);
			this.label2.Margin = new Padding(4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new Size(52, 20);
			this.label2.TabIndex = 6;
			this.label2.Text = "Stage";
			this.meshDefinitionGroupBox.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.meshDefinitionGroupBox.Controls.Add(this.meshDefinitionPropertyGrid);
			this.meshDefinitionGroupBox.Location = new Point(9, 88);
			this.meshDefinitionGroupBox.Margin = new Padding(4, 5, 4, 5);
			this.meshDefinitionGroupBox.Name = "meshDefinitionGroupBox";
			this.meshDefinitionGroupBox.Padding = new Padding(4, 5, 4, 5);
			this.meshDefinitionGroupBox.Size = new Size(446, 154);
			this.meshDefinitionGroupBox.TabIndex = 10;
			this.meshDefinitionGroupBox.TabStop = false;
			this.meshDefinitionGroupBox.Text = "Mesh Definition";
			this.meshDefinitionPropertyGrid.Dock = DockStyle.Fill;
			this.meshDefinitionPropertyGrid.HelpVisible = false;
			this.meshDefinitionPropertyGrid.Location = new Point(4, 24);
			this.meshDefinitionPropertyGrid.Margin = new Padding(4, 5, 4, 5);
			this.meshDefinitionPropertyGrid.Name = "meshDefinitionPropertyGrid";
			this.meshDefinitionPropertyGrid.PropertySort = PropertySort.NoSort;
			this.meshDefinitionPropertyGrid.Size = new Size(438, 125);
			this.meshDefinitionPropertyGrid.TabIndex = 10;
			this.meshDefinitionPropertyGrid.ToolbarVisible = false;
			this.meshPointsGroupBox.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.meshPointsGroupBox.Controls.Add(this.meshPointsListBox);
			this.meshPointsGroupBox.Location = new Point(13, 437);
			this.meshPointsGroupBox.Name = "meshPointsGroupBox";
			this.meshPointsGroupBox.Size = new Size(438, 359);
			this.meshPointsGroupBox.TabIndex = 11;
			this.meshPointsGroupBox.TabStop = false;
			this.meshPointsGroupBox.Text = "Mesh Points";
			this.meshPointsListBox.Dock = DockStyle.Fill;
			this.meshPointsListBox.FormattingEnabled = true;
			this.meshPointsListBox.ItemHeight = 20;
			this.meshPointsListBox.Location = new Point(3, 22);
			this.meshPointsListBox.Name = "meshPointsListBox";
			this.meshPointsListBox.Size = new Size(432, 334);
			this.meshPointsListBox.TabIndex = 0;
			this.meshPointsListBox.SelectedIndexChanged += this.meshPointsListBox_SelectedIndexChanged;
			this.meshPropertiesGroupBox.Controls.Add(this.meshPropertyGrid);
			this.meshPropertiesGroupBox.Location = new Point(13, 250);
			this.meshPropertiesGroupBox.Name = "meshPropertiesGroupBox";
			this.meshPropertiesGroupBox.Size = new Size(435, 181);
			this.meshPropertiesGroupBox.TabIndex = 12;
			this.meshPropertiesGroupBox.TabStop = false;
			this.meshPropertiesGroupBox.Text = "Mesh Properties";
			this.meshPropertyGrid.Dock = DockStyle.Fill;
			this.meshPropertyGrid.HelpVisible = false;
			this.meshPropertyGrid.Location = new Point(3, 22);
			this.meshPropertyGrid.Name = "meshPropertyGrid";
			this.meshPropertyGrid.PropertySort = PropertySort.NoSort;
			this.meshPropertyGrid.Size = new Size(429, 156);
			this.meshPropertyGrid.TabIndex = 0;
			this.meshPropertyGrid.ToolbarVisible = false;
			base.AutoScaleDimensions = new SizeF(9f, 20f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.meshPropertiesGroupBox);
			base.Controls.Add(this.meshPointsGroupBox);
			base.Controls.Add(this.meshDefinitionGroupBox);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.skinStageComboBox);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.skinComboBox);
			base.Margin = new Padding(4, 5, 4, 5);
			base.Name = "UnitSkinsEditorControl";
			base.Size = new Size(475, 815);
			this.meshDefinitionGroupBox.ResumeLayout(false);
			this.meshPointsGroupBox.ResumeLayout(false);
			this.meshPropertiesGroupBox.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}


		private UnitModel model;


		private IContainer components;


		private Label label1;


		private ComboBox skinComboBox;


		private ComboBox skinStageComboBox;


		private Label label2;


		private GroupBox meshDefinitionGroupBox;


		private PropertyGrid meshDefinitionPropertyGrid;


		private GroupBox meshPointsGroupBox;


		private ListBox meshPointsListBox;


		private GroupBox meshPropertiesGroupBox;


		private PropertyGrid meshPropertyGrid;
	}
}
