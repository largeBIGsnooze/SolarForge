using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Solar.Scenarios;

namespace SolarForge.Scenarios
{

	public class ScenarioViewportActionMenu : UserControl
	{

		public ScenarioViewportActionMenu()
		{
			this.InitializeComponent();
			this.contextMenuStrip.Closed += this.ContextMenuStrip_Closed;
		}


		private void ContextMenuStrip_Closed(object sender, ToolStripDropDownClosedEventArgs e)
		{
			if (this.model != null)
			{
				this.model.ActionTargetGalaxyChartNode = null;
			}
		}



		public ScenarioModel Model
		{
			set
			{
				this.model = value;
			}
		}



		public ScenarioViewportControl ViewportControl
		{
			set
			{
				this.viewportControl = value;
			}
		}


		private ScenarioViewportActionMenu.ActionContext GetActionContext(GalaxyChartNode node, GalaxyChartPhaseLane phaseLane)
		{
			if (node != null)
			{
				if (this.model.SelectedGalaxyChartNode != null && !object.Equals(this.model.SelectedGalaxyChartNode, node))
				{
					return ScenarioViewportActionMenu.ActionContext.NodeToNode;
				}
				return ScenarioViewportActionMenu.ActionContext.Node;
			}
			else
			{
				if (phaseLane != null)
				{
					return ScenarioViewportActionMenu.ActionContext.PhaseLane;
				}
				return ScenarioViewportActionMenu.ActionContext.Empty;
			}
		}


		public void ShowMenu(Control control, Point mouseLocation, GalaxyChartNode nodeAtMouseLocation, GalaxyChartPhaseLane phaseLaneAtMouseLocation)
		{
			this.mouseLocation = mouseLocation;
			this.model.ActionTargetGalaxyChartNode = nodeAtMouseLocation;
			this.actionTargetNode = nodeAtMouseLocation;
			this.actionTargetPhaseLane = phaseLaneAtMouseLocation;
			this.contextMenuStrip.Show(control, mouseLocation);
			ScenarioViewportActionMenu.ActionContext actionContext = this.GetActionContext(nodeAtMouseLocation, phaseLaneAtMouseLocation);
			this.addNodeMenuItem.Visible = (actionContext == ScenarioViewportActionMenu.ActionContext.Empty);
			this.addPhaseLaneMenuItem.Visible = (actionContext == ScenarioViewportActionMenu.ActionContext.NodeToNode);
			this.deletePhaseLaneByNodeConnection.Visible = (actionContext == ScenarioViewportActionMenu.ActionContext.NodeToNode);
			this.deleteNodeMenuItem.Visible = (actionContext == ScenarioViewportActionMenu.ActionContext.Node);
			this.deletePhaseLaneMenuItem.Visible = (actionContext == ScenarioViewportActionMenu.ActionContext.PhaseLane);
			this.addNodeMenuItem.DropDownItems.Clear();
			//List<GalaxyChartNodeFillingName> galaxyChartNodeFillingNames = this.model.Fillings.GetGalaxyChartNodeFillingNames();
			//int val = 100;
			//for (int i = 0; i < Math.Min(galaxyChartNodeFillingNames.Count, val); i++)
			//{
			//	GalaxyChartNodeFillingName galaxyChartNodeFillingName = galaxyChartNodeFillingNames[i];
			//	ToolStripItem toolStripItem = this.addNodeMenuItem.DropDownItems.Add(galaxyChartNodeFillingName.ToString());
			//	toolStripItem.Tag = galaxyChartNodeFillingName;
			//	toolStripItem.Click += this.addNodeToolStripMenuItem_Click;
			//} 
		}


		private void addNodeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			GalaxyChartNodeFillingName fillingName = (GalaxyChartNodeFillingName)((ToolStripItem)sender).Tag;
			this.viewportControl.AddNodeAt(fillingName, this.mouseLocation);
		}


		private void addPhaseLaneToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.model.AddPhaseLane(this.model.SelectedGalaxyChartNode, this.actionTargetNode);
		}


		private void deletePhaseLaneByNodeConnectionToolStripMenuItem_Click(object sender, EventArgs e)
		{
			GalaxyChartNode selectedGalaxyChartNode = this.model.SelectedGalaxyChartNode;
			GalaxyChartNode galaxyChartNode = this.actionTargetNode;
			if (selectedGalaxyChartNode != null && galaxyChartNode != null)
			{
				this.model.FindAndRemovePhaseLaneBetweenNodes(selectedGalaxyChartNode, galaxyChartNode);
			}
		}


		private void deleteNodeMenuItem_Click(object sender, EventArgs e)
		{
			this.model.RemoveNode(this.actionTargetNode);
		}


		private void deletePhaseLaneMenuItem_Click(object sender, EventArgs e)
		{
			this.model.RemovePhaseLane(this.actionTargetPhaseLane);
		}


		private void setNodeParentToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.model.MoveNodeToParent(this.model.SelectedGalaxyChartNode, this.actionTargetNode);
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
			this.components = new Container();
			this.contextMenuStrip = new ContextMenuStrip(this.components);
			this.addNodeMenuItem = new ToolStripMenuItem();
			this.addPhaseLaneMenuItem = new ToolStripMenuItem();
			this.deleteNodeMenuItem = new ToolStripMenuItem();
			this.deletePhaseLaneByNodeConnection = new ToolStripMenuItem();
			this.deletePhaseLaneMenuItem = new ToolStripMenuItem();
			this.setNodeParentToolStripMenuItem = new ToolStripMenuItem();
			this.contextMenuStrip.SuspendLayout();
			base.SuspendLayout();
			this.contextMenuStrip.Items.AddRange(new ToolStripItem[]
			{
				this.addNodeMenuItem,
				this.addPhaseLaneMenuItem,
				this.deleteNodeMenuItem,
				this.deletePhaseLaneByNodeConnection,
				this.deletePhaseLaneMenuItem,
				this.setNodeParentToolStripMenuItem
			});
			this.contextMenuStrip.Name = "contextMenuStrip";
			this.contextMenuStrip.Size = new Size(181, 136);
			this.addNodeMenuItem.Name = "addNodeMenuItem";
			this.addNodeMenuItem.Size = new Size(180, 22);
			this.addNodeMenuItem.Text = "Add Node";
			this.addPhaseLaneMenuItem.Name = "addPhaseLaneMenuItem";
			this.addPhaseLaneMenuItem.Size = new Size(180, 22);
			this.addPhaseLaneMenuItem.Text = "Add Phase Lane";
			this.addPhaseLaneMenuItem.Click += this.addPhaseLaneToolStripMenuItem_Click;
			this.deletePhaseLaneByNodeConnection.Name = "deletePhaseLaneByNodeConnection";
			this.deletePhaseLaneByNodeConnection.Size = new Size(180, 22);
			this.deletePhaseLaneByNodeConnection.Text = "Delete Phase Lane To Node";
			this.deletePhaseLaneByNodeConnection.Click += this.deletePhaseLaneByNodeConnectionToolStripMenuItem_Click;
			this.deleteNodeMenuItem.Name = "deleteNodeMenuItem";
			this.deleteNodeMenuItem.Size = new Size(180, 22);
			this.deleteNodeMenuItem.Text = "Delete Node";
			this.deleteNodeMenuItem.Click += this.deleteNodeMenuItem_Click;
			this.deletePhaseLaneMenuItem.Name = "deletePhaseLaneMenuItem";
			this.deletePhaseLaneMenuItem.Size = new Size(180, 22);
			this.deletePhaseLaneMenuItem.Text = "Delete Phase Lane";
			this.deletePhaseLaneMenuItem.Click += this.deletePhaseLaneMenuItem_Click;
			this.setNodeParentToolStripMenuItem.Name = "setNodeParentToolStripMenuItem";
			this.setNodeParentToolStripMenuItem.Size = new Size(180, 22);
			this.setNodeParentToolStripMenuItem.Text = "Set Node Parent";
			this.setNodeParentToolStripMenuItem.Click += this.setNodeParentToolStripMenuItem_Click;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Name = "ScenarioViewportActionMenu";
			this.contextMenuStrip.ResumeLayout(false);
			base.ResumeLayout(false);
		}


		private ScenarioModel model;


		private ScenarioViewportControl viewportControl;


		private Point mouseLocation;


		private GalaxyChartNode actionTargetNode;


		private GalaxyChartPhaseLane actionTargetPhaseLane;


		private IContainer components;


		private ContextMenuStrip contextMenuStrip;


		private ToolStripMenuItem addNodeMenuItem;


		private ToolStripMenuItem addPhaseLaneMenuItem;


		private ToolStripMenuItem deletePhaseLaneByNodeConnection;


		private ToolStripMenuItem deleteNodeMenuItem;


		private ToolStripMenuItem deletePhaseLaneMenuItem;


		private ToolStripMenuItem setNodeParentToolStripMenuItem;


		private enum ActionContext
		{

			Empty,

			Node,

			NodeToNode,

			PhaseLane
		}
	}
}
