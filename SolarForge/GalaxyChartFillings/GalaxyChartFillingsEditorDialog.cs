using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Solar.Scenarios;

namespace SolarForge.GalaxyChartFillings
{

	public partial class GalaxyChartFillingsEditorDialog : Form
	{

		public GalaxyChartFillingsEditorDialog()
		{
			this.InitializeComponent();
			this.fillingsTreeControl = new GalaxyChartFillingsTreeControl();
			this.fillingsTreeControl.Dock = DockStyle.Fill;
			this.splitContainer.Panel1.Controls.Add(this.fillingsTreeControl);
			this.randomSkyboxDataControl = new RandomSkyboxDataControl();
			this.randomSkyboxDataControl.Dock = DockStyle.Fill;
			this.splitContainer.Panel2.Controls.Add(this.randomSkyboxDataControl);
			this.randomFixtureDataControl = new RandomFixtureDataControl();
			this.randomFixtureDataControl.Dock = DockStyle.Fill;
			this.splitContainer.Panel2.Controls.Add(this.randomFixtureDataControl);
			this.fillingsSourceComboBox.SelectedValueChanged += this.FillingsSourceComboBox_SelectedValueChanged;
		}



		public GalaxyChartFillingsEditorModel Model
		{
			set
			{
				this.model = value; 
				this.fillingsTreeControl.Model = value; 
				this.randomSkyboxDataControl.Model = value;  
				this.randomFixtureDataControl.Model = value; 
				this.model.SelectedFillingsComponentChanged += this.Model_SelectedFillingsComponentChanged;
				this.model.FillingsSourcesChanged += this.Model_FillingsSourcesChanged;
				this.RefreshFillingsSourceComboBox();
			}
		}


		private void Model_SelectedFillingsComponentChanged()
		{
			object obj = null;
			if (this.model.SelectedFillingsComponent is GalaxyChartNodeFilling || this.model.SelectedFillingsComponent is FixtureFilling)
			{
				obj = this.model.SelectedFillingsComponent;
			}
			this.fillingPropertyGrid.Visible = (obj != null);
			this.fillingPropertyGrid.SelectedObject = obj;
		}


		private void Model_FillingsSourcesChanged()
		{
			this.RefreshFillingsSourceComboBox();
		}


		private void FillingsSourceComboBox_SelectedValueChanged(object sender, EventArgs e)
		{
			this.model.SelectedFillingsSource = (GalaxyChartFillingsSource)this.fillingsSourceComboBox.SelectedItem;
		}


		private void RefreshFillingsSourceComboBox()
		{
			this.fillingsSourceComboBox.Items.Clear();
			foreach (GalaxyChartFillingsSource item in this.model.FillingsSources)
			{
				this.fillingsSourceComboBox.Items.Add(item);
			}
			if (this.fillingsSourceComboBox.Items.Count > 0)
			{
				this.fillingsSourceComboBox.SelectedIndex = 0;
			}
		}


		protected override void OnKeyUp(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				base.Close();
				return;
			}
			base.OnKeyUp(e);
		}


		private void closeButton_Click(object sender, EventArgs e)
		{
			base.Close();
		}


		private GalaxyChartFillingsEditorModel model;


		private GalaxyChartFillingsTreeControl fillingsTreeControl;


		private RandomSkyboxDataControl randomSkyboxDataControl;


		private RandomFixtureDataControl randomFixtureDataControl;
	}
}
