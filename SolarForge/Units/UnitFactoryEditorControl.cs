using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Solar.Simulations;

namespace SolarForge.Units
{

	public class UnitFactoryEditorControl : UserControl
	{

		public UnitFactoryEditorControl()
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
			UnitFactoryDefinition unitFactoryDefinition = null;
			if (unitDefinition != null)
			{
				unitFactoryDefinition = unitDefinition.UnitFactory;
			}
			this.propertyGrid.SelectedObject = unitFactoryDefinition;
			this.syncToMeshPointButton.Enabled = (unitFactoryDefinition != null);
		}


		private void syncToMeshPointButton_Click(object sender, EventArgs e)
		{
			this.model.SyncUnitFactoryBuildPointToMesh();
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
			this.propertyGrid = new PropertyGrid();
			this.syncToMeshPointButton = new Button();
			base.SuspendLayout();
			this.propertyGrid.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.propertyGrid.Location = new Point(3, 69);
			this.propertyGrid.Name = "propertyGrid";
			this.propertyGrid.Size = new Size(487, 489);
			this.propertyGrid.TabIndex = 0;
			this.syncToMeshPointButton.Location = new Point(15, 16);
			this.syncToMeshPointButton.Name = "syncToMeshPointButton";
			this.syncToMeshPointButton.Size = new Size(305, 47);
			this.syncToMeshPointButton.TabIndex = 1;
			this.syncToMeshPointButton.Text = "Sync To Mesh Point";
			this.syncToMeshPointButton.UseVisualStyleBackColor = true;
			this.syncToMeshPointButton.Click += this.syncToMeshPointButton_Click;
			base.AutoScaleDimensions = new SizeF(9f, 20f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.syncToMeshPointButton);
			base.Controls.Add(this.propertyGrid);
			base.Name = "UnitFactoryEditorControl";
			base.Size = new Size(493, 561);
			base.ResumeLayout(false);
		}


		private UnitModel model;


		private IContainer components;


		private PropertyGrid propertyGrid;


		private Button syncToMeshPointButton;
	}
}
