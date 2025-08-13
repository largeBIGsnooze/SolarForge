using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SolarForge.Scenarios
{

	public class GalaxyChartEditorControl : UserControl
	{

		public GalaxyChartEditorControl()
		{
			this.InitializeComponent();
			this.galaxyChartTreeControl = new GalaxyChartTreeControl();
			this.galaxyChartTreeControl.Visible = true;
			this.galaxyChartTreeControl.Dock = DockStyle.Fill;
			this.splitContainer.Panel1.Controls.Add(this.galaxyChartTreeControl);
			this.propertyGrid = new PropertyGrid();
			this.propertyGrid.Visible = true;
			this.propertyGrid.Dock = DockStyle.Fill;
			this.propertyGrid.ToolbarVisible = false;
			this.propertyGrid.PropertySort = PropertySort.NoSort;
			this.splitContainer.Panel2.Controls.Add(this.propertyGrid);
		}



		public ScenarioModel Model
		{
			set
			{
				this.model = value;
				this.model.SelectedGalaxyChartComponentChanged += this.Model_SelectedGalaxyChartComponentChanged;
				this.galaxyChartTreeControl.Model = value;
				this.propertyGrid.PropertyValueChanged += this.PropertyGrid_PropertyValueChanged;
			}
		}


		private void PropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
		{
			this.model.OnSelectedGalaxyChartComponentPropertyChanged(e);
		}


		private void Model_SelectedGalaxyChartComponentChanged(object selectedComponent)
		{
			this.propertyGrid.SelectedObject = selectedComponent;
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
			this.splitContainer = new SplitContainer();
			((ISupportInitialize)this.splitContainer).BeginInit();
			this.splitContainer.SuspendLayout();
			base.SuspendLayout();
			this.splitContainer.Dock = DockStyle.Fill;
			this.splitContainer.Location = new Point(0, 0);
			this.splitContainer.Name = "splitContainer";
			this.splitContainer.Orientation = Orientation.Horizontal;
			this.splitContainer.Size = new Size(406, 332);
			this.splitContainer.SplitterDistance = 135;
			this.splitContainer.TabIndex = 0;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.splitContainer);
			base.Name = "GalaxyChartEditorControl";
			base.Size = new Size(406, 332);
			((ISupportInitialize)this.splitContainer).EndInit();
			this.splitContainer.ResumeLayout(false);
			base.ResumeLayout(false);
		}


		private ScenarioModel model;


		private GalaxyChartTreeControl galaxyChartTreeControl;


		private PropertyGrid propertyGrid;


		private IContainer components;


		private SplitContainer splitContainer;
	}
}
