using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Solar.Math;

namespace SolarForge.Meshes
{

	public class MeshTrianglesEditorControl : UserControl
	{

		public MeshTrianglesEditorControl()
		{
			this.InitializeComponent();
			foreach (object item in Enum.GetValues(typeof(SolarForge.Meshes.MeshTrianglesView)))
			{
				this.viewComboBox.Items.Add(item);
			}
			foreach (object item2 in Enum.GetValues(typeof(Facing)))
			{
				this.facingComboBox.Items.Add(item2);
			}
		}



		public MeshModel Model
		{
			set
			{
				this.model = value;
				this.model.SelectedMeshInstanceChanged += this.Model_SelectedMeshInstanceChanged;
				this.model.SelectedMeshTrianglesFacingChanged += this.Model_SelectedMeshTrianglesFacingChanged;
				this.viewComboBox.SelectedItem = this.model.SelectedMeshTrianglesView;
				this.facingComboBox.SelectedItem = this.model.SelectedMeshTrianglesFacing;
			}
		}


		private void Model_SelectedMeshTrianglesFacingChanged()
		{
			this.SyncGridCoordListBoxToModel();
		}


		private void Model_SelectedMeshInstanceChanged(MeshInstance meshInstance)
		{
			this.SyncGridCoordListBoxToModel();
		}


		private void SyncGridCoordListBoxToModel()
		{
			this.gridCoordListBox.Items.Clear();
			if (this.model.SelectedMeshInstance != null)
			{
				foreach (Point point in this.model.SelectedMeshInstance.Mesh.Data.GetTriangleGrid(this.model.SelectedMeshTrianglesFacing).GetNonEmptyTriangleGridCoords())
				{
					this.gridCoordListBox.Items.Add(point);
				}
			}
		}


		private void viewComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.model.SelectedMeshTrianglesView = (SolarForge.Meshes.MeshTrianglesView)this.viewComboBox.SelectedItem;
		}


		private void facingComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.model.SelectedMeshTrianglesFacing = (Facing)this.facingComboBox.SelectedItem;
		}


		private void gridCoordListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.model.SelectedMeshTrianglesGridCoord = new Point?((Point)this.gridCoordListBox.SelectedItem);
		}


		private void clearGridCoordButton_Click(object sender, EventArgs e)
		{
			this.model.SelectedMeshTrianglesGridCoord = null;
			this.SyncGridCoordListBoxToModel();
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
			this.viewComboBox = new ComboBox();
			this.facingComboBox = new ComboBox();
			this.gridCoordListBox = new ListBox();
			this.clearGridCoordButton = new Button();
			base.SuspendLayout();
			this.viewComboBox.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.viewComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			this.viewComboBox.FormattingEnabled = true;
			this.viewComboBox.Location = new Point(21, 20);
			this.viewComboBox.Name = "viewComboBox";
			this.viewComboBox.Size = new Size(886, 28);
			this.viewComboBox.TabIndex = 0;
			this.viewComboBox.SelectedIndexChanged += this.viewComboBox_SelectedIndexChanged;
			this.facingComboBox.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.facingComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			this.facingComboBox.FormattingEnabled = true;
			this.facingComboBox.Location = new Point(21, 54);
			this.facingComboBox.Name = "facingComboBox";
			this.facingComboBox.Size = new Size(886, 28);
			this.facingComboBox.TabIndex = 1;
			this.facingComboBox.SelectedIndexChanged += this.facingComboBox_SelectedIndexChanged;
			this.gridCoordListBox.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.gridCoordListBox.FormattingEnabled = true;
			this.gridCoordListBox.ItemHeight = 20;
			this.gridCoordListBox.Location = new Point(21, 148);
			this.gridCoordListBox.Name = "gridCoordListBox";
			this.gridCoordListBox.Size = new Size(886, 424);
			this.gridCoordListBox.TabIndex = 2;
			this.gridCoordListBox.SelectedIndexChanged += this.gridCoordListBox_SelectedIndexChanged;
			this.clearGridCoordButton.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.clearGridCoordButton.Location = new Point(21, 99);
			this.clearGridCoordButton.Name = "clearGridCoordButton";
			this.clearGridCoordButton.Size = new Size(886, 43);
			this.clearGridCoordButton.TabIndex = 3;
			this.clearGridCoordButton.Text = "Clear Grid Coord";
			this.clearGridCoordButton.UseVisualStyleBackColor = true;
			this.clearGridCoordButton.Click += this.clearGridCoordButton_Click;
			base.AutoScaleDimensions = new SizeF(9f, 20f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.clearGridCoordButton);
			base.Controls.Add(this.gridCoordListBox);
			base.Controls.Add(this.facingComboBox);
			base.Controls.Add(this.viewComboBox);
			base.Name = "MeshTrianglesEditorControl";
			base.Size = new Size(929, 586);
			base.ResumeLayout(false);
		}


		private MeshModel model;


		private IContainer components;


		private ComboBox viewComboBox;


		private ComboBox facingComboBox;


		private ListBox gridCoordListBox;


		private Button clearGridCoordButton;
	}
}
