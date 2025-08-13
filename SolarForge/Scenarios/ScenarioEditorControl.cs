using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Solar.Scenarios;

namespace SolarForge.Scenarios
{

	public class ScenarioEditorControl : UserControl
	{

		public ScenarioEditorControl()
		{
			this.InitializeComponent();
			this.galaxyChartGeneratorControl = new GalaxyChartGeneratorControl();
			this.galaxyChartGeneratorControl.Visible = false;
			this.galaxyChartGeneratorControl.Dock = DockStyle.Fill;
			this.panel.Controls.Add(this.galaxyChartGeneratorControl);
			this.galaxyChartEditorControl = new GalaxyChartEditorControl();
			this.galaxyChartEditorControl.Visible = false;
			this.galaxyChartEditorControl.Dock = DockStyle.Fill;
			this.panel.Controls.Add(this.galaxyChartEditorControl);
		}



		public ScenarioModel Model
		{
			set
			{
				this.model = value;
				this.model.ScenarioChanged += this.Model_ScenarioChanged;
				this.galaxyChartGeneratorControl.Model = value;
				this.galaxyChartEditorControl.Model = value;
			}
		}


		private void Model_ScenarioChanged(Scenario scenario, bool isUndo)
		{
			this.scenarioInfoPropertyGrid.SelectedObject = scenario.ScenarioInfo;
			this.galaxyChartGeneratorControl.Visible = (((scenario != null) ? scenario.GalaxyChartGeneratorParams : null) != null);
			this.galaxyChartEditorControl.Visible = (((scenario != null) ? scenario.GalaxyChart : null) != null);
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
			this.scenarioInfoPropertyGrid = new PropertyGrid();
			this.panel = new Panel();
			base.SuspendLayout();
			this.scenarioInfoPropertyGrid.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.scenarioInfoPropertyGrid.Location = new Point(3, 3);
			this.scenarioInfoPropertyGrid.Name = "scenarioInfoPropertyGrid";
			this.scenarioInfoPropertyGrid.PropertySort = PropertySort.NoSort;
			this.scenarioInfoPropertyGrid.Size = new Size(373, 177);
			this.scenarioInfoPropertyGrid.TabIndex = 0;
			this.scenarioInfoPropertyGrid.ToolbarVisible = false;
			this.panel.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.panel.Location = new Point(3, 188);
			this.panel.Name = "panel";
			this.panel.Size = new Size(372, 272);
			this.panel.TabIndex = 1;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.panel);
			base.Controls.Add(this.scenarioInfoPropertyGrid);
			base.Name = "ScenarioEditorControl";
			base.Size = new Size(379, 463);
			base.ResumeLayout(false);
		}


		private ScenarioModel model;


		private GalaxyChartGeneratorControl galaxyChartGeneratorControl;


		private GalaxyChartEditorControl galaxyChartEditorControl;


		private IContainer components;


		private PropertyGrid scenarioInfoPropertyGrid;


		private Panel panel;
	}
}
