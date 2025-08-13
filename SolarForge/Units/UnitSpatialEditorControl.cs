using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Solar.Simulations;

namespace SolarForge.Units
{

	public class UnitSpatialEditorControl : UserControl
	{

		public UnitSpatialEditorControl()
		{
			this.InitializeComponent();
		}



		public UnitModel Model
		{
			set
			{
				this.model = value;
				this.model.UnitDefinitionChanged += this.Model_UnitDefinitionChanged;
			}
		}


		private void Model_UnitDefinitionChanged(UnitDefinition unitDefinition)
		{
			PropertyGrid propertyGrid = this.spatialPropertyGrid;
			UnitDefinition unitDefinition2 = this.model.UnitDefinition;
			propertyGrid.SelectedObject = ((unitDefinition2 != null) ? unitDefinition2.Spatial : null);
		}


		private void syncToMeshButton_Click(object sender, EventArgs e)
		{
			this.model.SyncSpatialPropertiesToMesh();
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
			this.spatialPropertyGrid = new PropertyGrid();
			this.syncToMeshButton = new Button();
			base.SuspendLayout();
			this.spatialPropertyGrid.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.spatialPropertyGrid.Location = new Point(15, 20);
			this.spatialPropertyGrid.Name = "spatialPropertyGrid";
			this.spatialPropertyGrid.PropertySort = PropertySort.NoSort;
			this.spatialPropertyGrid.Size = new Size(592, 390);
			this.spatialPropertyGrid.TabIndex = 0;
			this.syncToMeshButton.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.syncToMeshButton.Location = new Point(15, 416);
			this.syncToMeshButton.Name = "syncToMeshButton";
			this.syncToMeshButton.Size = new Size(592, 35);
			this.syncToMeshButton.TabIndex = 1;
			this.syncToMeshButton.Text = "Sync to Mesh";
			this.syncToMeshButton.UseVisualStyleBackColor = true;
			this.syncToMeshButton.Click += this.syncToMeshButton_Click;
			base.AutoScaleDimensions = new SizeF(9f, 20f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.syncToMeshButton);
			base.Controls.Add(this.spatialPropertyGrid);
			base.Name = "UnitSpatialEditorControl";
			base.Size = new Size(624, 933);
			base.ResumeLayout(false);
		}


		private UnitModel model;


		private IContainer components;


		private PropertyGrid spatialPropertyGrid;


		private Button syncToMeshButton;
	}
}
