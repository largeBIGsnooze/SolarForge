using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Solar.Scenarios;
using SolarForge.Utility;

namespace SolarForge.GalaxyChartFillings
{

	public class GalaxyChartFillingsTreeControl : UserControl
	{

		public GalaxyChartFillingsTreeControl()
		{
			this.InitializeComponent();
			this.treeView.AfterSelect += this.TreeView_AfterSelect;
			this.treeView.LabelEdit = true;
			this.treeView.BeforeLabelEdit += this.TreeView_BeforeLabelEdit;
			this.treeView.AfterLabelEdit += this.TreeView_AfterLabelEdit;
		}  
         

		private void TreeView_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
		{
			GalaxyChartFillingsTreeControl.TreeNodeTag treeNodeTag = (GalaxyChartFillingsTreeControl.TreeNodeTag)e.Node.Tag;
			bool flag = treeNodeTag.Type == GalaxyChartFillingsTreeControl.TreeNodeTagType.Fixture || treeNodeTag.Type == GalaxyChartFillingsTreeControl.TreeNodeTagType.Moon || treeNodeTag.Type == GalaxyChartFillingsTreeControl.TreeNodeTagType.GalaxyChartNode || treeNodeTag.Type == GalaxyChartFillingsTreeControl.TreeNodeTagType.RandomFixture || treeNodeTag.Type == GalaxyChartFillingsTreeControl.TreeNodeTagType.RandomSkybox;
			e.CancelEdit = !flag;
		}


        private void TreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Label))
            {
                GalaxyChartFillingsTreeControl.TreeNodeTag? treeNodeTag = null;
                GalaxyChartFillingsTreeControl.TreeNodeTag treeNodeTag2 = (GalaxyChartFillingsTreeControl.TreeNodeTag)e.Node.Tag;
                if (treeNodeTag2.Type == GalaxyChartFillingsTreeControl.TreeNodeTagType.GalaxyChartNode)
                {
                    GalaxyChartNodeFillingName name = this.model.SelectedFillingsSource.Fillings.RenameGalaxyChartNodeFilling(treeNodeTag2.GalaxyChartNodeFillingName, new GalaxyChartNodeFillingName(e.Label));
                    treeNodeTag = new GalaxyChartFillingsTreeControl.TreeNodeTag?(GalaxyChartFillingsTreeControl.TreeNodeTag.TagWith(name));
                }
                else if (treeNodeTag2.Type == GalaxyChartFillingsTreeControl.TreeNodeTagType.RandomSkybox)
                {
                    RandomSkyboxFillingName name2 = this.model.SelectedFillingsSource.Fillings.RenameRandomSkyboxFilling(treeNodeTag2.RandomSkyboxFillingName, new RandomSkyboxFillingName(e.Label));
                    treeNodeTag = new GalaxyChartFillingsTreeControl.TreeNodeTag?(GalaxyChartFillingsTreeControl.TreeNodeTag.TagWith(name2));
                }
                else if (treeNodeTag2.Type == GalaxyChartFillingsTreeControl.TreeNodeTagType.RandomFixture)
                {
                    RandomFixtureFillingName name3 = this.model.SelectedFillingsSource.Fillings.RenameRandomFixtureFilling(treeNodeTag2.RandomFixtureFillingName, new RandomFixtureFillingName(e.Label));
                    treeNodeTag = new GalaxyChartFillingsTreeControl.TreeNodeTag?(GalaxyChartFillingsTreeControl.TreeNodeTag.TagWith(name3));
                }
                else if (treeNodeTag2.Type == GalaxyChartFillingsTreeControl.TreeNodeTagType.Fixture)
                {
                    FixtureFillingName name4 = this.model.SelectedFillingsSource.Fillings.RenameFixtureFilling(treeNodeTag2.FixtureFillingName, new FixtureFillingName(e.Label));
                    this.model.OnFixtureFillingsChanged();
                    treeNodeTag = new GalaxyChartFillingsTreeControl.TreeNodeTag?(GalaxyChartFillingsTreeControl.TreeNodeTag.TagWith(name4));
                }
                else if (treeNodeTag2.Type == GalaxyChartFillingsTreeControl.TreeNodeTagType.Moon)
                {
                    MoonFillingName name5 = this.model.SelectedFillingsSource.Fillings.RenameMoonFilling(treeNodeTag2.MoonFillingName, new MoonFillingName(e.Label));
                    this.model.OnFixtureFillingsChanged();
                    treeNodeTag = new GalaxyChartFillingsTreeControl.TreeNodeTag?(GalaxyChartFillingsTreeControl.TreeNodeTag.TagWith(name5));
                }
                if (treeNodeTag != null)
                {
                    this.UpdateTreeView(true);
                    TreeViewUiState<GalaxyChartFillingsTreeControl.TreeNodeTag>.SelectAndExpandToNodeWithTag(this.treeView, treeNodeTag.Value);
                }
            }
        }


        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.model.SelectedFillingsComponent = this.TryGetFillingFromTag((GalaxyChartFillingsTreeControl.TreeNodeTag)e.Node.Tag);
        }



        public GalaxyChartFillingsEditorModel Model
        {
            set
            {
                this.model = value;
                this.model.SelectedFillingsSourceChanged += this.Model_SelectedFillingsSourceChanged;
                this.UpdateTreeView(false);
            }
        }


        private void Model_SelectedFillingsSourceChanged()
        {
            this.UpdateTreeView(true);
        }


        private void UpdateTreeView(bool restoreUiState)
        {
            TreeViewUiState<GalaxyChartFillingsTreeControl.TreeNodeTag> treeViewUiState = new TreeViewUiState<GalaxyChartFillingsTreeControl.TreeNodeTag>(this.treeView);
            this.model.SelectedFillingsComponent = null;
            this.treeView.BeginUpdate();
            this.treeView.Nodes.Clear(); 
            GalaxyChartFillingsSource selectedFillingsSource = this.model.SelectedFillingsSource;
            Solar.Scenarios.GalaxyChartFillings galaxyChartFillings = (selectedFillingsSource != null) ? selectedFillingsSource.Fillings : null;
            if (galaxyChartFillings != null)
            {
                TreeNode treeNode = this.treeView.Nodes.Add("Galaxy Chart Nodes");
                treeNode.Tag = new GalaxyChartFillingsTreeControl.TreeNodeTag(GalaxyChartFillingsTreeControl.TreeNodeTagType.GalaxyChartNodesRoot);
                foreach (GalaxyChartNodeFillingName galaxyChartNodeFillingName in galaxyChartFillings.GalaxyChartNodeFillingNames)
                {
                    treeNode.Nodes.Add(galaxyChartNodeFillingName.ToString()).Tag = GalaxyChartFillingsTreeControl.TreeNodeTag.TagWith(galaxyChartNodeFillingName);
                }
                TreeNode treeNode2 = this.treeView.Nodes.Add("Random Skyboxes");
                treeNode2.Tag = new GalaxyChartFillingsTreeControl.TreeNodeTag(GalaxyChartFillingsTreeControl.TreeNodeTagType.RandomSkyboxesRoot);
                foreach (RandomSkyboxFillingName randomSkyboxFillingName in galaxyChartFillings.RandomSkyboxFillingNames)
                {
                    treeNode2.Nodes.Add(randomSkyboxFillingName.ToString()).Tag = GalaxyChartFillingsTreeControl.TreeNodeTag.TagWith(randomSkyboxFillingName);
                }
                TreeNode treeNode3 = this.treeView.Nodes.Add("Random Fixtures");
                treeNode3.Tag = new GalaxyChartFillingsTreeControl.TreeNodeTag(GalaxyChartFillingsTreeControl.TreeNodeTagType.RandomFixturesRoot);
                foreach (RandomFixtureFillingName randomFixtureFillingName in galaxyChartFillings.RandomFixtureFillingNames)
                {
                    treeNode3.Nodes.Add(randomFixtureFillingName.ToString()).Tag = GalaxyChartFillingsTreeControl.TreeNodeTag.TagWith(randomFixtureFillingName);
                }
                TreeNode treeNode4 = this.treeView.Nodes.Add("Fixtures");
                treeNode4.Tag = new GalaxyChartFillingsTreeControl.TreeNodeTag(GalaxyChartFillingsTreeControl.TreeNodeTagType.FixturesRoot);
                foreach (FixtureFillingName fixtureFillingName in galaxyChartFillings.FixtureFillingNames)
                {
                    treeNode4.Nodes.Add(fixtureFillingName.ToString()).Tag = GalaxyChartFillingsTreeControl.TreeNodeTag.TagWith(fixtureFillingName);
                }
                TreeNode treeNode5 = this.treeView.Nodes.Add("Moons");
                treeNode5.Tag = new GalaxyChartFillingsTreeControl.TreeNodeTag(GalaxyChartFillingsTreeControl.TreeNodeTagType.MoonsRoot);
                foreach (MoonFillingName moonFillingName in galaxyChartFillings.MoonFillingNames)
                {
                    treeNode5.Nodes.Add(moonFillingName.ToString()).Tag = GalaxyChartFillingsTreeControl.TreeNodeTag.TagWith(moonFillingName);
                }
            }
            this.treeView.EndUpdate();
            if (restoreUiState)
            {
                treeViewUiState.ApplyTo(this.treeView);
                if (treeViewUiState.SelectedTag != null)
                {
                    this.model.SelectedFillingsComponent = this.TryGetFillingFromTag(treeViewUiState.SelectedTag.Value);
                }
            }
        }


        private object TryGetFillingFromTag(GalaxyChartFillingsTreeControl.TreeNodeTag tag)
        {
            GalaxyChartFillingsSource selectedFillingsSource = this.model.SelectedFillingsSource;
            Solar.Scenarios.GalaxyChartFillings galaxyChartFillings = (selectedFillingsSource != null) ? selectedFillingsSource.Fillings : null;
            if (galaxyChartFillings != null)
            {
                if (tag.Type == GalaxyChartFillingsTreeControl.TreeNodeTagType.GalaxyChartNode)
                {
                    return galaxyChartFillings.FindGalaxyChartNodeFilling(tag.GalaxyChartNodeFillingName);
                }
                if (tag.Type == GalaxyChartFillingsTreeControl.TreeNodeTagType.RandomSkybox)
                {
                    return galaxyChartFillings.FindRandomSkyboxFilling(tag.RandomSkyboxFillingName);
                }
                if (tag.Type == GalaxyChartFillingsTreeControl.TreeNodeTagType.RandomFixture)
                {
                    return galaxyChartFillings.FindRandomFixtureFilling(tag.RandomFixtureFillingName);
                }
                if (tag.Type == GalaxyChartFillingsTreeControl.TreeNodeTagType.Fixture)
                {
                    return galaxyChartFillings.FindFixtureFilling(tag.FixtureFillingName);
                }
                if (tag.Type == GalaxyChartFillingsTreeControl.TreeNodeTagType.Moon)
                {
                    return galaxyChartFillings.FindMoonFilling(tag.MoonFillingName);
                }
            }
            return null;
        }


        private void addItemButton_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = this.treeView.SelectedNode;
            object obj = (selectedNode != null) ? selectedNode.Tag : null;
            if (obj != null)
            {
                GalaxyChartFillingsTreeControl.TreeNodeTag? treeNodeTag = null;
                GalaxyChartFillingsTreeControl.TreeNodeTagType type = ((GalaxyChartFillingsTreeControl.TreeNodeTag)obj).Type;
                if (type == GalaxyChartFillingsTreeControl.TreeNodeTagType.GalaxyChartNode || type == GalaxyChartFillingsTreeControl.TreeNodeTagType.GalaxyChartNodesRoot)
                {
                    GalaxyChartNodeFillingName name = this.model.SelectedFillingsSource.Fillings.AddGalaxyChartNodeFilling();
                    treeNodeTag = new GalaxyChartFillingsTreeControl.TreeNodeTag?(GalaxyChartFillingsTreeControl.TreeNodeTag.TagWith(name));
                }
                else if (type == GalaxyChartFillingsTreeControl.TreeNodeTagType.RandomSkybox || type == GalaxyChartFillingsTreeControl.TreeNodeTagType.RandomSkyboxesRoot)
                {
                    RandomSkyboxFillingName name2 = this.model.SelectedFillingsSource.Fillings.AddRandomSkyboxFilling();
                    treeNodeTag = new GalaxyChartFillingsTreeControl.TreeNodeTag?(GalaxyChartFillingsTreeControl.TreeNodeTag.TagWith(name2));
                }
                else if (type == GalaxyChartFillingsTreeControl.TreeNodeTagType.RandomFixture || type == GalaxyChartFillingsTreeControl.TreeNodeTagType.RandomFixturesRoot)
                {
                    RandomFixtureFillingName name3 = this.model.SelectedFillingsSource.Fillings.AddRandomFixtureFilling(this.model.MakeNewRandomFixtureFillingItem());
                    treeNodeTag = new GalaxyChartFillingsTreeControl.TreeNodeTag?(GalaxyChartFillingsTreeControl.TreeNodeTag.TagWith(name3));
                }
                else if (type == GalaxyChartFillingsTreeControl.TreeNodeTagType.Fixture || type == GalaxyChartFillingsTreeControl.TreeNodeTagType.FixturesRoot)
                {
                    FixtureFillingName name4 = this.model.SelectedFillingsSource.Fillings.AddFixtureFilling();
                    this.model.OnFixtureFillingsChanged();
                    treeNodeTag = new GalaxyChartFillingsTreeControl.TreeNodeTag?(GalaxyChartFillingsTreeControl.TreeNodeTag.TagWith(name4));
                }
                else if (type == GalaxyChartFillingsTreeControl.TreeNodeTagType.Moon || type == GalaxyChartFillingsTreeControl.TreeNodeTagType.MoonsRoot)
                {
                    MoonFillingName name5 = this.model.SelectedFillingsSource.Fillings.AddMoonFilling();
                    this.model.OnFixtureFillingsChanged();
                    treeNodeTag = new GalaxyChartFillingsTreeControl.TreeNodeTag?(GalaxyChartFillingsTreeControl.TreeNodeTag.TagWith(name5));
                }
                if (treeNodeTag != null)
                {
                    this.UpdateTreeView(true);
                    TreeViewUiState<GalaxyChartFillingsTreeControl.TreeNodeTag>.SelectAndExpandToNodeWithTag(this.treeView, treeNodeTag.Value);
                }
            }
        }


        private void removeItemButton_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = this.treeView.SelectedNode;
            object obj = (selectedNode != null) ? selectedNode.Tag : null;
            if (obj != null)
            {
                GalaxyChartFillingsTreeControl.TreeNodeTag treeNodeTag = (GalaxyChartFillingsTreeControl.TreeNodeTag)obj;
                if (treeNodeTag.Type == GalaxyChartFillingsTreeControl.TreeNodeTagType.GalaxyChartNode)
                {
                    this.model.SelectedFillingsSource.Fillings.RemoveGalaxyChartNodeFilling(treeNodeTag.GalaxyChartNodeFillingName);
                }
                else if (treeNodeTag.Type == GalaxyChartFillingsTreeControl.TreeNodeTagType.RandomSkybox)
                {
                    this.model.SelectedFillingsSource.Fillings.RemoveRandomSkyboxFilling(treeNodeTag.RandomSkyboxFillingName);
                }
                else if (treeNodeTag.Type == GalaxyChartFillingsTreeControl.TreeNodeTagType.RandomFixture)
                {
                    this.model.SelectedFillingsSource.Fillings.RemoveRandomFixtureFilling(treeNodeTag.RandomFixtureFillingName);
                }
                else if (treeNodeTag.Type == GalaxyChartFillingsTreeControl.TreeNodeTagType.Fixture)
                {
                    this.model.SelectedFillingsSource.Fillings.RemoveFixtureFilling(treeNodeTag.FixtureFillingName);
                }
                else if (treeNodeTag.Type == GalaxyChartFillingsTreeControl.TreeNodeTagType.Moon)
                {
                    this.model.SelectedFillingsSource.Fillings.RemoveMoonFilling(treeNodeTag.MoonFillingName);
                }
                this.UpdateTreeView(true);
            }
        }


        private void renameItemButton_Click(object sender, EventArgs e)
		{
			TreeNode selectedNode = this.treeView.SelectedNode;
			if (selectedNode != null)
			{
				selectedNode.BeginEdit();
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(GalaxyChartFillingsTreeControl));
			this.treeView = new TreeView();
			this.toolStrip1 = new ToolStrip();
			this.addItemButton = new ToolStripButton();
			this.removeItemButton = new ToolStripButton();
			this.toolStripSeparator1 = new ToolStripSeparator();
			this.toolStripSeparator2 = new ToolStripSeparator();
			this.renameItemButton = new ToolStripButton();
			this.toolStrip1.SuspendLayout();
			base.SuspendLayout();
			this.treeView.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.treeView.HideSelection = false;
			this.treeView.Location = new Point(0, 28);
			this.treeView.Name = "treeView";
			this.treeView.Size = new Size(505, 511);
			this.treeView.TabIndex = 0;
			this.toolStrip1.Items.AddRange(new ToolStripItem[]
			{
				this.addItemButton,
				this.toolStripSeparator1,
				this.removeItemButton,
				this.toolStripSeparator2,
				this.renameItemButton
			});
			this.toolStrip1.Location = new Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new Size(505, 25);
			this.toolStrip1.TabIndex = 1;
			this.toolStrip1.Text = "toolStrip1";
			this.addItemButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
			this.addItemButton.Image = (Image)componentResourceManager.GetObject("addItemButton.Image");
			this.addItemButton.ImageTransparentColor = Color.Magenta;
			this.addItemButton.Name = "addItemButton";
			this.addItemButton.Size = new Size(33, 22);
			this.addItemButton.Text = "Add";
			this.addItemButton.Click += this.addItemButton_Click;
			this.removeItemButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
			this.removeItemButton.Image = (Image)componentResourceManager.GetObject("removeItemButton.Image");
			this.removeItemButton.ImageTransparentColor = Color.Magenta;
			this.removeItemButton.Name = "removeItemButton";
			this.removeItemButton.Size = new Size(54, 22);
			this.removeItemButton.Text = "Remove";
			this.removeItemButton.Click += this.removeItemButton_Click;
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new Size(6, 25);
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new Size(6, 25);
			this.renameItemButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
			this.renameItemButton.Image = (Image)componentResourceManager.GetObject("renameItemButton.Image");
			this.renameItemButton.ImageTransparentColor = Color.Magenta;
			this.renameItemButton.Name = "renameItemButton";
			this.renameItemButton.Size = new Size(54, 22);
			this.renameItemButton.Text = "Rename";
			this.renameItemButton.Click += this.renameItemButton_Click;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.toolStrip1);
			base.Controls.Add(this.treeView);
			base.Name = "GalaxyChartFillingsTreeControl";
			base.Size = new Size(505, 539);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}


		private GalaxyChartFillingsEditorModel model;


		private IContainer components;


		private TreeView treeView;


		private ToolStrip toolStrip1;


		private ToolStripButton addItemButton;


		private ToolStripButton removeItemButton;


		private ToolStripSeparator toolStripSeparator1;


		private ToolStripSeparator toolStripSeparator2;


		private ToolStripButton renameItemButton;


		private enum TreeNodeTagType
		{

			GalaxyChartNodesRoot,

			GalaxyChartNode,

			RandomSkyboxesRoot,

			RandomSkybox,

			RandomFixturesRoot,

			RandomFixture,

			FixturesRoot,

			Fixture,

			MoonsRoot,

			Moon
		}


		private struct TreeNodeTag
		{



			public GalaxyChartFillingsTreeControl.TreeNodeTagType Type { get; set; }




			public GalaxyChartNodeFillingName GalaxyChartNodeFillingName { get; set; }




			public RandomSkyboxFillingName RandomSkyboxFillingName { get; set; }




			public RandomFixtureFillingName RandomFixtureFillingName { get; set; }




			public FixtureFillingName FixtureFillingName { get; set; }




			public MoonFillingName MoonFillingName { get; set; }


			public TreeNodeTag(GalaxyChartFillingsTreeControl.TreeNodeTagType type)
			{
				this.Type = type;
				this.GalaxyChartNodeFillingName = null;
				this.RandomSkyboxFillingName = null;
				this.RandomFixtureFillingName = null;
				this.FixtureFillingName = null;
				this.MoonFillingName = null;
			}


			public static GalaxyChartFillingsTreeControl.TreeNodeTag TagWith(GalaxyChartNodeFillingName name)
			{
				return new GalaxyChartFillingsTreeControl.TreeNodeTag(GalaxyChartFillingsTreeControl.TreeNodeTagType.GalaxyChartNode)
				{
					GalaxyChartNodeFillingName = name
				};
			}


			public static GalaxyChartFillingsTreeControl.TreeNodeTag TagWith(RandomSkyboxFillingName name)
			{
				return new GalaxyChartFillingsTreeControl.TreeNodeTag(GalaxyChartFillingsTreeControl.TreeNodeTagType.RandomSkybox)
				{
					RandomSkyboxFillingName = name
				};
			}


			public static GalaxyChartFillingsTreeControl.TreeNodeTag TagWith(RandomFixtureFillingName name)
			{
				return new GalaxyChartFillingsTreeControl.TreeNodeTag(GalaxyChartFillingsTreeControl.TreeNodeTagType.RandomFixture)
				{
					RandomFixtureFillingName = name
				};
			}


			public static GalaxyChartFillingsTreeControl.TreeNodeTag TagWith(FixtureFillingName name)
			{
				return new GalaxyChartFillingsTreeControl.TreeNodeTag(GalaxyChartFillingsTreeControl.TreeNodeTagType.Fixture)
				{
					FixtureFillingName = name
				};
			}


			public static GalaxyChartFillingsTreeControl.TreeNodeTag TagWith(MoonFillingName name)
			{
				return new GalaxyChartFillingsTreeControl.TreeNodeTag(GalaxyChartFillingsTreeControl.TreeNodeTagType.Moon)
				{
					MoonFillingName = name
				};
			}
		}
	}
}
