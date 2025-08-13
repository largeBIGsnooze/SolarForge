using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Solar.Math;

namespace SolarForge.Meshes
{

	public class MeshEditorControl : UserControl, IDisposable
	{

		public MeshEditorControl()
		{
			this.InitializeComponent();
			this.viewTabControl.SelectedIndexChanged += this.ViewTabControl_SelectedIndexChanged;
			this.defaultEditorControl = new MeshDefaultEditorControl();
			this.defaultEditorControl.Dock = DockStyle.Fill;
			this.defaultTabPage.Controls.Add(this.defaultEditorControl);
			this.defaultTabPage.Tag = MeshView.Default;
			this.trianglesEditorControl = new MeshTrianglesEditorControl();
			this.trianglesEditorControl.Dock = DockStyle.Fill;
			this.trianglesTabPage.Controls.Add(this.trianglesEditorControl);
			this.trianglesTabPage.Tag = MeshView.Triangles;
		}


		private void ViewTabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.model.SelectedMeshView = (MeshView)this.viewTabControl.SelectedTab.Tag;
		}



		public MeshModel Model
		{
			set
			{
				this.model = value;
				this.defaultEditorControl.Model = this.model;
				this.trianglesEditorControl.Model = this.model;
				this.model.SelectedMeshInstanceChanged += this.Model_SelectedMeshInstanceChanged;
				this.model.MeshInstancesChanged += this.Model_MeshInstancesChanged;
			}
		}


		private void Model_MeshInstancesChanged(IEnumerable<MeshInstance> meshInstances)
		{
			this.meshInstanceComboBox.DisplayMember = "Name";
			this.meshInstanceComboBox.Items.Clear();
			foreach (MeshInstance item in meshInstances)
			{
				this.meshInstanceComboBox.Items.Add(item);
			}
			if (this.model.SelectedMeshIndex != null)
			{
				this.meshInstanceComboBox.SelectedIndex = this.model.SelectedMeshIndex.Value;
			}
		}


		private void Model_SelectedMeshInstanceChanged(MeshInstance meshInstance)
		{
			Basis selectedObject = null;
			if (meshInstance != null)
			{
				selectedObject = meshInstance.Basis;
			}
			this.meshInstanceBasisPropertyGrid.SelectedObject = selectedObject;
		}


		private void meshInstanceListBox_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (this.model != null)
			{
				this.model.FocusOnSelectedMeshInstance();
			}
		}


		private void meshInstanceComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.model != null)
			{
				this.model.SetSelectedMeshIndex(new int?(this.meshInstanceComboBox.SelectedIndex), true);
			}
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
			this.meshInstanceBasisPropertyGrid = new PropertyGrid();
			this.meshInstanceComboBox = new ComboBox();
			this.viewTabControl = new TabControl();
			this.defaultTabPage = new TabPage();
			this.trianglesTabPage = new TabPage();
			this.viewTabControl.SuspendLayout();
			base.SuspendLayout();
			this.meshInstanceBasisPropertyGrid.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.meshInstanceBasisPropertyGrid.HelpVisible = false;
			this.meshInstanceBasisPropertyGrid.Location = new Point(17, 49);
			this.meshInstanceBasisPropertyGrid.Name = "meshInstanceBasisPropertyGrid";
			this.meshInstanceBasisPropertyGrid.PropertySort = PropertySort.NoSort;
			this.meshInstanceBasisPropertyGrid.Size = new Size(699, 68);
			this.meshInstanceBasisPropertyGrid.TabIndex = 3;
			this.meshInstanceBasisPropertyGrid.ToolbarVisible = false;
			this.meshInstanceComboBox.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.meshInstanceComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			this.meshInstanceComboBox.FormattingEnabled = true;
			this.meshInstanceComboBox.Location = new Point(17, 15);
			this.meshInstanceComboBox.Name = "meshInstanceComboBox";
			this.meshInstanceComboBox.Size = new Size(700, 28);
			this.meshInstanceComboBox.TabIndex = 6;
			this.meshInstanceComboBox.SelectedIndexChanged += this.meshInstanceComboBox_SelectedIndexChanged;
			this.viewTabControl.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.viewTabControl.Controls.Add(this.defaultTabPage);
			this.viewTabControl.Controls.Add(this.trianglesTabPage);
			this.viewTabControl.Location = new Point(17, 123);
			this.viewTabControl.Name = "viewTabControl";
			this.viewTabControl.SelectedIndex = 0;
			this.viewTabControl.Size = new Size(700, 872);
			this.viewTabControl.TabIndex = 7;
			this.defaultTabPage.Location = new Point(4, 29);
			this.defaultTabPage.Name = "defaultTabPage";
			this.defaultTabPage.Padding = new Padding(3);
			this.defaultTabPage.Size = new Size(692, 839);
			this.defaultTabPage.TabIndex = 0;
			this.defaultTabPage.Text = "Properties";
			this.defaultTabPage.UseVisualStyleBackColor = true;
			this.trianglesTabPage.Location = new Point(4, 29);
			this.trianglesTabPage.Name = "trianglesTabPage";
			this.trianglesTabPage.Padding = new Padding(3);
			this.trianglesTabPage.Size = new Size(692, 839);
			this.trianglesTabPage.TabIndex = 1;
			this.trianglesTabPage.Text = "Triangles";
			this.trianglesTabPage.UseVisualStyleBackColor = true;
			base.AutoScaleDimensions = new SizeF(9f, 20f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.viewTabControl);
			base.Controls.Add(this.meshInstanceBasisPropertyGrid);
			base.Controls.Add(this.meshInstanceComboBox);
			base.Margin = new Padding(4, 5, 4, 5);
			base.Name = "MeshEditorControl";
			base.Size = new Size(736, 1013);
			this.viewTabControl.ResumeLayout(false);
			base.ResumeLayout(false);
		}


		private MeshModel model;


		private MeshDefaultEditorControl defaultEditorControl;


		private MeshTrianglesEditorControl trianglesEditorControl;


		private IContainer components;


		private PropertyGrid meshInstanceBasisPropertyGrid;


		private ComboBox meshInstanceComboBox;


		private TabControl viewTabControl;


		private TabPage defaultTabPage;


		private TabPage trianglesTabPage;
	}
}
