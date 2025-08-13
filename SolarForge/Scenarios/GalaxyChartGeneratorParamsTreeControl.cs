using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Solar.Scenarios;
using SolarForge.Utility;

namespace SolarForge.Scenarios
{

	public class GalaxyChartGeneratorParamsTreeControl : UserControl
	{

		public GalaxyChartGeneratorParamsTreeControl()
		{
			this.InitializeComponent();
			this.treeView.AfterSelect += this.TreeView_AfterSelect;
			this.treeView.KeyUp += this.TreeView_KeyUp;
			this.SyncButtonStateToSelectedNode();
		}


		private void TreeView_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
			{
				if (this.SelectedPlanetRangeIndex != null)
				{
					this.model.GalaxyChartGeneratorParams.SolarSystems[this.SelectedSolarSystemIndex.Value].RemovePlanetRange(this.SelectedPlanetRangeIndex.Value);
					this.UpdateTreeView(true);
				}
				else if (this.SelectedSolarSystemIndex != null)
				{
					this.model.GalaxyChartGeneratorParams.RemoveSolarSystem(this.SelectedSolarSystemIndex.Value);
					this.UpdateTreeView(true);
				}
				e.Handled = true;
			}
		}



		public int? SelectedSolarSystemIndex
		{
			get
			{
				if (this.treeView.SelectedNode != null)
				{
					return ((GalaxyChartGeneratorParamsTreeControl.TreeNodeTag)this.treeView.SelectedNode.Tag).SolarSystemIndex;
				}
				return null;
			}
		}



		public int? SelectedPlanetRangeIndex
		{
			get
			{
				if (this.treeView.SelectedNode != null)
				{
					return ((GalaxyChartGeneratorParamsTreeControl.TreeNodeTag)this.treeView.SelectedNode.Tag).PlanetRangeIndex;
				}
				return null;
			}
		}


		private void SyncButtonStateToSelectedNode()
		{
			this.addPlanetRangeButton.Enabled = (this.SelectedSolarSystemIndex != null);
		}


		private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			GalaxyChartGeneratorParamsTreeControl.TreeNodeTag tag = (GalaxyChartGeneratorParamsTreeControl.TreeNodeTag)e.Node.Tag;
			if (this.model != null)
			{
				this.model.SelectedGalaxyChartGeneratorParamsComponent = this.TryGetGalaxyChartGeneratorParamsComponentFromTag(tag);
			}
			this.SyncButtonStateToSelectedNode();
		}



		public ScenarioModel Model
		{
			set
			{
				this.model = value;
				this.model.ScenarioChanged += this.Model_ScenarioChanged;
			}
		}


		private void Model_ScenarioChanged(Scenario scenario, bool isUndo)
		{
			this.UpdateTreeView(false);
		}


		public void HandleAnyFillingNameChanged()
		{
			if (this.SelectedSolarSystemIndex != null || this.SelectedPlanetRangeIndex != null)
			{
				this.UpdateTreeView(true);
			}
		}


		private void UpdateTreeView(bool restoreUiState)
		{
			TreeViewUiState<GalaxyChartGeneratorParamsTreeControl.TreeNodeTag> treeViewUiState = new TreeViewUiState<GalaxyChartGeneratorParamsTreeControl.TreeNodeTag>(this.treeView);
			this.model.SelectedGalaxyChartGeneratorParamsComponent = null;
			this.treeView.BeginUpdate();
			this.treeView.Nodes.Clear();
			if (this.model.GalaxyChartGeneratorParams != null)
			{
				this.treeView.Nodes.Add("Properties").Tag = new GalaxyChartGeneratorParamsTreeControl.TreeNodeTag(GalaxyChartGeneratorParamsTreeControl.TreeNodeTagType.Properties);
				TreeNode treeNode = this.treeView.Nodes.Add("Solar Systems");
				treeNode.Tag = new GalaxyChartGeneratorParamsTreeControl.TreeNodeTag(GalaxyChartGeneratorParamsTreeControl.TreeNodeTagType.SolarSystemsRoot);
				for (int i = 0; i < this.model.GalaxyChartGeneratorParams.SolarSystems.Count; i++)
				{
					GalaxyChartGeneratorSolarSystemParams galaxyChartGeneratorSolarSystemParams = this.model.GalaxyChartGeneratorParams.SolarSystems[i];
					TreeNode treeNode2 = treeNode.Nodes.Add(string.Format("{0} : {1}", i, galaxyChartGeneratorSolarSystemParams.StarFillingName));
					treeNode2.Tag = new GalaxyChartGeneratorParamsTreeControl.TreeNodeTag(GalaxyChartGeneratorParamsTreeControl.TreeNodeTagType.SolarSystem)
					{
						SolarSystemIndex = new int?(i)
					};
					TreeNode treeNode3 = treeNode2.Nodes.Add("Planet Ranges");
					treeNode3.Tag = new GalaxyChartGeneratorParamsTreeControl.TreeNodeTag(GalaxyChartGeneratorParamsTreeControl.TreeNodeTagType.SolarSystemPlanetRangesRoot)
					{
						SolarSystemIndex = new int?(i)
					};
					for (int j = 0; j < galaxyChartGeneratorSolarSystemParams.PlanetRanges.Count; j++)
					{
						GalaxyChartGeneratorPlanetRangeParams galaxyChartGeneratorPlanetRangeParams = galaxyChartGeneratorSolarSystemParams.PlanetRanges[j];
						treeNode3.Nodes.Add(string.Format("{0} : {1}", j, galaxyChartGeneratorPlanetRangeParams.FillingName)).Tag = new GalaxyChartGeneratorParamsTreeControl.TreeNodeTag(GalaxyChartGeneratorParamsTreeControl.TreeNodeTagType.SolarSystemPlanetRange)
						{
							SolarSystemIndex = new int?(i),
							PlanetRangeIndex = new int?(j)
						};
					}
				}
			}
			this.treeView.EndUpdate();
			if (restoreUiState)
			{
				treeViewUiState.ApplyTo(this.treeView);
				if (treeViewUiState.SelectedTag != null)
				{
					this.model.SelectedGalaxyChartGeneratorParamsComponent = this.TryGetGalaxyChartGeneratorParamsComponentFromTag(treeViewUiState.SelectedTag.Value);
				}
			}
		}


		private object TryGetGalaxyChartGeneratorParamsComponentFromTag(GalaxyChartGeneratorParamsTreeControl.TreeNodeTag tag)
		{
			if (tag.Type == GalaxyChartGeneratorParamsTreeControl.TreeNodeTagType.Properties)
			{
				return this.model.GalaxyChartGeneratorParams;
			}
			GalaxyChartGeneratorSolarSystemParams galaxyChartGeneratorSolarSystemParams = null;
			if (tag.SolarSystemIndex != null && tag.SolarSystemIndex.Value < this.model.GalaxyChartGeneratorParams.SolarSystems.Count)
			{
				galaxyChartGeneratorSolarSystemParams = this.model.GalaxyChartGeneratorParams.SolarSystems[tag.SolarSystemIndex.Value];
			}
			if (galaxyChartGeneratorSolarSystemParams != null)
			{
				GalaxyChartGeneratorPlanetRangeParams galaxyChartGeneratorPlanetRangeParams = null;
				if (tag.PlanetRangeIndex != null && tag.PlanetRangeIndex.Value < galaxyChartGeneratorSolarSystemParams.PlanetRanges.Count)
				{
					galaxyChartGeneratorPlanetRangeParams = galaxyChartGeneratorSolarSystemParams.PlanetRanges[tag.PlanetRangeIndex.Value];
				}
				if (galaxyChartGeneratorPlanetRangeParams != null)
				{
					return galaxyChartGeneratorPlanetRangeParams;
				}
			}
			return galaxyChartGeneratorSolarSystemParams;
		}


		private void addSolarSystemButton_Click(object sender, EventArgs e)
		{
			this.model.AddNewGalaxyChartGeneratorSolarSystem();
			this.UpdateTreeView(true);
		}


		private void addPlanetRangeButton_Click(object sender, EventArgs e)
		{
			this.model.AddNewGalaxyChartGeneratorPlanetRange(this.model.GalaxyChartGeneratorParams.SolarSystems[this.SelectedSolarSystemIndex.Value]);
			this.UpdateTreeView(true);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(GalaxyChartGeneratorParamsTreeControl));
			this.treeView = new TreeView();
			this.toolStrip1 = new ToolStrip();
			this.addSolarSystemButton = new ToolStripButton();
			this.addPlanetRangeButton = new ToolStripButton();
			this.toolStrip1.SuspendLayout();
			base.SuspendLayout();
			this.treeView.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.treeView.HideSelection = false;
			this.treeView.Location = new Point(0, 28);
			this.treeView.Name = "treeView";
			this.treeView.Size = new Size(285, 260);
			this.treeView.TabIndex = 0;
			this.toolStrip1.Items.AddRange(new ToolStripItem[]
			{
				this.addSolarSystemButton,
				this.addPlanetRangeButton
			});
			this.toolStrip1.Location = new Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new Size(285, 25);
			this.toolStrip1.TabIndex = 1;
			this.toolStrip1.Text = "toolStrip1";
			this.addSolarSystemButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
			this.addSolarSystemButton.Image = (Image)componentResourceManager.GetObject("addSolarSystemButton.Image");
			this.addSolarSystemButton.ImageTransparentColor = Color.Magenta;
			this.addSolarSystemButton.Name = "addSolarSystemButton";
			this.addSolarSystemButton.Size = new Size(103, 22);
			this.addSolarSystemButton.Text = "Add Solar System";
			this.addSolarSystemButton.Click += this.addSolarSystemButton_Click;
			this.addPlanetRangeButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
			this.addPlanetRangeButton.Image = (Image)componentResourceManager.GetObject("addPlanetRangeButton.Image");
			this.addPlanetRangeButton.ImageTransparentColor = Color.Magenta;
			this.addPlanetRangeButton.Name = "addPlanetRangeButton";
			this.addPlanetRangeButton.Size = new Size(105, 22);
			this.addPlanetRangeButton.Text = "Add Planet Range";
			this.addPlanetRangeButton.Click += this.addPlanetRangeButton_Click;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.toolStrip1);
			base.Controls.Add(this.treeView);
			base.Name = "GalaxyChartGeneratorParamsTreeControl";
			base.Size = new Size(285, 288);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}


		private ScenarioModel model;


		private IContainer components;


		private TreeView treeView;


		private ToolStrip toolStrip1;


		private ToolStripButton addSolarSystemButton;


		private ToolStripButton addPlanetRangeButton;


		private enum TreeNodeTagType
		{

			Properties,

			SolarSystemsRoot,

			SolarSystem,

			SolarSystemPlanetRangesRoot,

			SolarSystemPlanetRange
		}


		private struct TreeNodeTag
		{



			public GalaxyChartGeneratorParamsTreeControl.TreeNodeTagType Type { get; set; }




			public int? SolarSystemIndex { get; set; }




			public int? PlanetRangeIndex { get; set; }


			public TreeNodeTag(GalaxyChartGeneratorParamsTreeControl.TreeNodeTagType type)
			{
				this.Type = type;
				this.SolarSystemIndex = null;
				this.PlanetRangeIndex = null;
			}
		}
	}
}
