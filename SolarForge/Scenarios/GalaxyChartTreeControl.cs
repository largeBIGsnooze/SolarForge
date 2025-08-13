using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Solar.Scenarios;
using SolarForge.Utility;

namespace SolarForge.Scenarios
{

	public class GalaxyChartTreeControl : UserControl
	{

		public GalaxyChartTreeControl()
		{
			this.InitializeComponent();
			this.treeView.AfterSelect += this.TreeView_AfterSelect;
			this.treeView.KeyUp += this.TreeView_KeyUp;
			this.treeView.ItemDrag += this.TreeView_ItemDrag;
			this.treeView.DragEnter += this.TreeView_DragEnter;
			this.treeView.DragOver += this.TreeView_DragOver;
			this.treeView.DragDrop += this.TreeView_DragDrop;
			this.treeView.AllowDrop = true;
		}


		private void TreeView_DragDrop(object sender, DragEventArgs e)
		{
			TreeNode nodeAt = this.treeView.GetNodeAt(this.treeView.PointToClient(new Point(e.X, e.Y)));
			if (nodeAt != null)
			{
				GalaxyChartTreeControl.TreeNodeTag tag = (GalaxyChartTreeControl.TreeNodeTag)nodeAt.Tag;
				GalaxyChartTreeControl.TreeNodeTag tag2 = (GalaxyChartTreeControl.TreeNodeTag)((TreeNode)e.Data.GetData(typeof(TreeNode))).Tag;
				if (tag2.Type == GalaxyChartTreeControl.TreeNodeTagType.Node)
				{
					GalaxyChartNode node = (GalaxyChartNode)this.TryGetGalaxyChartComponentFromTag(tag2);
					if (tag.Type == GalaxyChartTreeControl.TreeNodeTagType.NodesRoot)
					{
						this.model.MoveNodeToRoot(node);
						return;
					}
					if (tag.Type == GalaxyChartTreeControl.TreeNodeTagType.Node)
					{
						GalaxyChartNode parent = (GalaxyChartNode)this.TryGetGalaxyChartComponentFromTag(tag);
						this.model.MoveNodeToParent(node, parent);
					}
				}
			}
		}


		private void TreeView_DragOver(object sender, DragEventArgs e)
		{
			this.treeView.SelectedNode = this.treeView.GetNodeAt(this.treeView.PointToClient(new Point(e.X, e.Y)));
			if (this.treeView.SelectedNode != null)
			{
				this.treeView.SelectedNode.Expand();
			}
		}


		private void TreeView_DragEnter(object sender, DragEventArgs e)
		{
			e.Effect = e.AllowedEffect;
		}


		private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (this.model != null)
			{
				this.model.SelectedGalaxyChartComponent = this.TryGetGalaxyChartComponentFromTag((GalaxyChartTreeControl.TreeNodeTag)e.Node.Tag);
			}
		}


		private void TreeView_ItemDrag(object sender, ItemDragEventArgs e)
		{
			if (e.Button == MouseButtons.Left && ((GalaxyChartTreeControl.TreeNodeTag)((TreeNode)e.Item).Tag).Type == GalaxyChartTreeControl.TreeNodeTagType.Node)
			{
				base.DoDragDrop(e.Item, DragDropEffects.Move);
			}
		}


		private void TreeView_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete && this.treeView.SelectedNode != null)
			{
				object obj = this.TryGetGalaxyChartComponentFromTag((GalaxyChartTreeControl.TreeNodeTag)this.treeView.SelectedNode.Tag);
				GalaxyChartNode galaxyChartNode = obj as GalaxyChartNode;
				if (galaxyChartNode != null)
				{
					this.model.RemoveNode(galaxyChartNode);
				}
				GalaxyChartPhaseLane galaxyChartPhaseLane = obj as GalaxyChartPhaseLane;
				if (galaxyChartPhaseLane != null)
				{
					this.model.RemovePhaseLane(galaxyChartPhaseLane);
				}
				e.Handled = true;
			}
		}



		public ScenarioModel Model
		{
			set
			{
				this.model = value;
				this.model.GalaxyChartChanged += this.ScenarioModel_GalaxyChartChanged;
				this.model.GalaxyChartComponentsChanged += this.Model_GalaxyChartComponentsChanged;
				this.model.SelectedGalaxyChartComponentChanged += this.Model_SelectedGalaxyChartComponentChanged;
			}
		}


		private void Model_SelectedGalaxyChartComponentChanged(object selectedComponent)
		{
			GalaxyChartNode galaxyChartNode = selectedComponent as GalaxyChartNode;
			if (galaxyChartNode != null)
			{
				TreeViewUiState<GalaxyChartTreeControl.TreeNodeTag>.SelectAndExpandToNodeWithTag(this.treeView, new GalaxyChartTreeControl.TreeNodeTag(galaxyChartNode.Id));
			}
			GalaxyChartPhaseLane galaxyChartPhaseLane = selectedComponent as GalaxyChartPhaseLane;
			if (galaxyChartPhaseLane != null)
			{
				TreeViewUiState<GalaxyChartTreeControl.TreeNodeTag>.SelectAndExpandToNodeWithTag(this.treeView, new GalaxyChartTreeControl.TreeNodeTag(galaxyChartPhaseLane.Id));
			}
		}


		private void Model_GalaxyChartComponentsChanged()
		{
			this.UpdateTreeView(true);
		}


		private void ScenarioModel_GalaxyChartChanged(GalaxyChart galaxyChart, bool isUndo)
		{
			this.UpdateTreeView(false);
		}


		private void UpdateTreeView(bool restoreUiState)
		{
			TreeViewUiState<GalaxyChartTreeControl.TreeNodeTag> treeViewUiState = new TreeViewUiState<GalaxyChartTreeControl.TreeNodeTag>(this.treeView);
			this.model.SelectedGalaxyChartComponent = null;
			this.treeView.BeginUpdate();
			this.treeView.Nodes.Clear();
			if (this.model.GalaxyChart != null)
			{
				this.treeView.Nodes.Add("Properties").Tag = new GalaxyChartTreeControl.TreeNodeTag(GalaxyChartTreeControl.TreeNodeTagType.Properties);
				TreeNode treeNode = this.treeView.Nodes.Add("Nodes");
				treeNode.Tag = new GalaxyChartTreeControl.TreeNodeTag(GalaxyChartTreeControl.TreeNodeTagType.NodesRoot);
				this.AddGalaxyChartNodesRecursive(treeNode, this.model.GalaxyChart.RootNodes);
				TreeNode treeNode2 = this.treeView.Nodes.Add("Phase Lanes");
				treeNode2.Tag = new GalaxyChartTreeControl.TreeNodeTag(GalaxyChartTreeControl.TreeNodeTagType.PhaseLanesRoot);
				for (int i = 0; i < this.model.GalaxyChart.PhaseLanes.Count; i++)
				{
					GalaxyChartPhaseLane galaxyChartPhaseLane = this.model.GalaxyChart.PhaseLanes[i];
					treeNode2.Nodes.Add(string.Format("{0}", galaxyChartPhaseLane.Id)).Tag = new GalaxyChartTreeControl.TreeNodeTag(galaxyChartPhaseLane.Id);
				}
			}
			this.treeView.EndUpdate();
			if (restoreUiState)
			{
				treeViewUiState.ApplyTo(this.treeView);
				if (treeViewUiState.SelectedTag != null)
				{
					this.model.SelectedGalaxyChartComponent = this.TryGetGalaxyChartComponentFromTag(treeViewUiState.SelectedTag.Value);
				}
			}
		}


		private void AddGalaxyChartNodesRecursive(TreeNode parentNode, GalaxyChartNodeCollection nodes)
		{
			for (int i = 0; i < nodes.Count; i++)
			{
				GalaxyChartNode galaxyChartNode = nodes[i];
				TreeNode treeNode = parentNode.Nodes.Add(string.Format("{0} : {1}", galaxyChartNode.Id, galaxyChartNode.FillingName));
				treeNode.Tag = new GalaxyChartTreeControl.TreeNodeTag(galaxyChartNode.Id);
				this.AddGalaxyChartNodesRecursive(treeNode, galaxyChartNode.ChildNodes);
			}
		}


		private object TryGetGalaxyChartComponentFromTag(GalaxyChartTreeControl.TreeNodeTag tag)
		{
			if (this.model.GalaxyChart != null)
			{
				if (tag.Type == GalaxyChartTreeControl.TreeNodeTagType.Node)
				{
					return this.model.GalaxyChart.FindNode(tag.NodeId);
				}
				if (tag.Type == GalaxyChartTreeControl.TreeNodeTagType.PhaseLane)
				{
					return this.model.GalaxyChart.FindPhaseLane(tag.PhaseLaneId);
				}
				if (tag.Type == GalaxyChartTreeControl.TreeNodeTagType.Properties)
				{
					return this.model.GalaxyChart;
				}
			}
			return null;
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
			this.treeView = new TreeView();
			base.SuspendLayout();
			this.treeView.Dock = DockStyle.Fill;
			this.treeView.HideSelection = false;
			this.treeView.Location = new Point(0, 0);
			this.treeView.Name = "treeView";
			this.treeView.Size = new Size(316, 313);
			this.treeView.TabIndex = 0;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.treeView);
			base.Name = "GalaxyChartTreeControl";
			base.Size = new Size(316, 313);
			base.ResumeLayout(false);
		}


		private ScenarioModel model;


		private IContainer components;


		private TreeView treeView;


		private enum TreeNodeTagType
		{

			Properties,

			NodesRoot,

			Node,

			PhaseLanesRoot,

			PhaseLane
		}


		private struct TreeNodeTag
		{



			public GalaxyChartTreeControl.TreeNodeTagType Type { get; set; }




			public GalaxyChartNodeId? NodeId { get; set; }




			public GalaxyChartPhaseLaneId? PhaseLaneId { get; set; }


			public TreeNodeTag(GalaxyChartTreeControl.TreeNodeTagType type)
			{
				this.Type = type;
				this.NodeId = null;
				this.PhaseLaneId = null;
			}


			public TreeNodeTag(GalaxyChartNodeId nodeId)
			{
				this.Type = GalaxyChartTreeControl.TreeNodeTagType.Node;
				this.NodeId = new GalaxyChartNodeId?(nodeId);
				this.PhaseLaneId = null;
			}


			public TreeNodeTag(GalaxyChartPhaseLaneId phaseLaneId)
			{
				this.Type = GalaxyChartTreeControl.TreeNodeTagType.PhaseLane;
				this.NodeId = null;
				this.PhaseLaneId = new GalaxyChartPhaseLaneId?(phaseLaneId);
			}
		}
	}
}
