using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Solar.Rendering;

namespace SolarForge.Meshes
{

	public class MeshDefaultEditorControl : UserControl
	{

		public MeshDefaultEditorControl()
		{
			this.InitializeComponent();
		}



		public MeshModel Model
		{
			set
			{
				this.model = value;
				this.model.SelectedMeshInstanceChanged += this.Model_SelectedMeshInstanceChanged;
				this.model.SelectedPointChanged += this.Model_SelectedPointChanged;
				this.model.SelectedMaterialChanged += this.Model_SelectedMaterialChanged;
			}
		}


		private void Model_SelectedMaterialChanged()
		{
			this.meshMaterialPropertyGrid.SelectedObject = this.model.SelectedMaterial;
		}


		private void Model_SelectedPointChanged()
		{
		}


		private void Model_SelectedMeshInstanceChanged(MeshInstance meshInstance)
		{
			MeshData meshData = null;
			if (meshInstance != null)
			{
				meshData = meshInstance.Mesh.Data;
			}
			this.meshPropertiesPropertyGrid.SelectedObject = meshData;
			this.SyncMeshPointListBoxToMeshData(meshData);
			this.SyncMeshMaterialListBoxToMeshData(meshData);
		}


		private void SyncMeshPointListBoxToMeshData(MeshData meshData)
		{
			this.meshPointsListBox.Items.Clear();
			if (meshData != null)
			{
				foreach (MeshPoint meshPoint in meshData.Points)
				{
					this.meshPointsListBox.Items.Add(meshPoint.Name);
				}
			}
		}


		private void SyncMeshMaterialListBoxToMeshData(MeshData meshData)
		{
			this.meshMaterialsListBox.Items.Clear();
			if (meshData != null)
			{
				foreach (MeshMaterial meshMaterial in meshData.Materials)
				{
					this.meshMaterialsListBox.Items.Add(meshMaterial.Name);
				}
			}
		}


		private void meshPointsListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.model.SetSelectedPointIndex(new int?(this.meshPointsListBox.SelectedIndex), true);
		}


		private void meshPointsListBox_DoubleClick(object sender, EventArgs e)
		{
			if (this.model != null)
			{
				this.model.FocusOnSelectedMeshPoint();
			}
		}


		private void meshMaterialsListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.model.SetSelectedMaterialIndex(new int?(this.meshMaterialsListBox.SelectedIndex), true);
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
			this.meshMaterialPropertyGrid = new PropertyGrid();
			this.meshMaterialsListBox = new ListBox();
			this.meshPropertiesPropertyGrid = new PropertyGrid();
			this.groupBox2 = new GroupBox();
			this.meshPointsListBox = new ListBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			base.SuspendLayout();
			this.groupBox1.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.groupBox1.Controls.Add(this.meshMaterialPropertyGrid);
			this.groupBox1.Controls.Add(this.meshMaterialsListBox);
			this.groupBox1.Location = new Point(12, 208);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(921, 376);
			this.groupBox1.TabIndex = 10;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Materials";
			this.meshMaterialPropertyGrid.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.meshMaterialPropertyGrid.HelpVisible = false;
			this.meshMaterialPropertyGrid.Location = new Point(6, 155);
			this.meshMaterialPropertyGrid.Name = "meshMaterialPropertyGrid";
			this.meshMaterialPropertyGrid.Size = new Size(907, 215);
			this.meshMaterialPropertyGrid.TabIndex = 1;
			this.meshMaterialPropertyGrid.ToolbarVisible = false;
			this.meshMaterialsListBox.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.meshMaterialsListBox.FormattingEnabled = true;
			this.meshMaterialsListBox.ItemHeight = 20;
			this.meshMaterialsListBox.Location = new Point(6, 25);
			this.meshMaterialsListBox.Name = "meshMaterialsListBox";
			this.meshMaterialsListBox.Size = new Size(907, 124);
			this.meshMaterialsListBox.TabIndex = 0;
			this.meshMaterialsListBox.SelectedIndexChanged += this.meshMaterialsListBox_SelectedIndexChanged;
			this.meshPropertiesPropertyGrid.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.meshPropertiesPropertyGrid.DisabledItemForeColor = SystemColors.ControlText;
			this.meshPropertiesPropertyGrid.HelpVisible = false;
			this.meshPropertiesPropertyGrid.Location = new Point(18, 5);
			this.meshPropertiesPropertyGrid.Margin = new Padding(4, 5, 4, 5);
			this.meshPropertiesPropertyGrid.Name = "meshPropertiesPropertyGrid";
			this.meshPropertiesPropertyGrid.PropertySort = PropertySort.Alphabetical;
			this.meshPropertiesPropertyGrid.Size = new Size(915, 195);
			this.meshPropertiesPropertyGrid.TabIndex = 8;
			this.meshPropertiesPropertyGrid.ToolbarVisible = false;
			this.groupBox2.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.groupBox2.Controls.Add(this.meshPointsListBox);
			this.groupBox2.Location = new Point(18, 592);
			this.groupBox2.Margin = new Padding(4, 5, 4, 5);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Padding = new Padding(4, 5, 4, 5);
			this.groupBox2.Size = new Size(907, 527);
			this.groupBox2.TabIndex = 9;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Points";
			this.meshPointsListBox.Dock = DockStyle.Fill;
			this.meshPointsListBox.Font = new Font("Courier New", 8f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.meshPointsListBox.FormattingEnabled = true;
			this.meshPointsListBox.ItemHeight = 18;
			this.meshPointsListBox.Location = new Point(4, 24);
			this.meshPointsListBox.Margin = new Padding(4, 5, 4, 5);
			this.meshPointsListBox.Name = "meshPointsListBox";
			this.meshPointsListBox.Size = new Size(899, 498);
			this.meshPointsListBox.TabIndex = 2;
			this.meshPointsListBox.SelectedIndexChanged += this.meshPointsListBox_SelectedIndexChanged;
			this.meshPointsListBox.DoubleClick += this.meshPointsListBox_DoubleClick;
			base.AutoScaleDimensions = new SizeF(9f, 20f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.meshPropertiesPropertyGrid);
			base.Controls.Add(this.groupBox2);
			base.Name = "MeshDefaultEditorControl";
			base.Size = new Size(952, 1124);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			base.ResumeLayout(false);
		}


		private MeshModel model;


		private IContainer components;


		private GroupBox groupBox1;


		private PropertyGrid meshMaterialPropertyGrid;


		private ListBox meshMaterialsListBox;


		private PropertyGrid meshPropertiesPropertyGrid;


		private GroupBox groupBox2;


		private ListBox meshPointsListBox;
	}
}
