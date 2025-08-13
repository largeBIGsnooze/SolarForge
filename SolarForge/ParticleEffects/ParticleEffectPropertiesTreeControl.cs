using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Solar.Rendering;
using SolarForge.Utility;

namespace SolarForge.ParticleEffects
{

	public class ParticleEffectPropertiesTreeControl : UserControl
	{

		public ParticleEffectPropertiesTreeControl()
		{
			this.InitializeComponent();
			this.treeView.AfterSelect += delegate(object sender, TreeViewEventArgs e)
			{
				if (this.model != null)
				{
					this.model.SelectedParticleEffectComponent = this.TryGetParticleEffectComponentFromTag((ParticleEffectPropertiesTreeControl.TreeNodeTag)e.Node.Tag);
				}
			};
			this.treeView.NodeMouseClick += this.TreeView_NodeMouseClick;
			this.treeView.DragOver += this.TreeView_DragOver;
			this.treeView.BeforeCollapse += this.TreeView_BeforeCollapse;
			this.treeView.BeforeExpand += this.TreeView_BeforeExpand;
			this.nodesRoot = this.treeView.Nodes.Add("Nodes");
			this.nodesRoot.Tag = new ParticleEffectPropertiesTreeControl.TreeNodeTag(ParticleEffectPropertiesTreeControl.TreeNodeTagType.NodesRoot);
			this.emittersRoot = this.treeView.Nodes.Add("Emitters");
			this.emittersRoot.Tag = new ParticleEffectPropertiesTreeControl.TreeNodeTag(ParticleEffectPropertiesTreeControl.TreeNodeTagType.EmittersRoot);
			this.modifiersRoot = this.treeView.Nodes.Add("Modifiers");
			this.modifiersRoot.Tag = new ParticleEffectPropertiesTreeControl.TreeNodeTag(ParticleEffectPropertiesTreeControl.TreeNodeTagType.ModifiersRoot);
			this.maxScalarRoot = this.treeView.Nodes.Add("MaxScalar");
			this.maxScalarRoot.Tag = new ParticleEffectPropertiesTreeControl.TreeNodeTag(ParticleEffectPropertiesTreeControl.TreeNodeTagType.MaxScalarRoot);
		}


		private void TreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
		{
			if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
			{
				foreach (object obj in e.Node.Nodes)
				{
					TreeNode treeNode = (TreeNode)obj;
					if (!treeNode.IsExpanded)
					{
						treeNode.Expand();
					}
				}
			}
		}


		private void TreeView_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
		{
			if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
			{
				foreach (object obj in e.Node.Nodes)
				{
					TreeNode treeNode = (TreeNode)obj;
					if (treeNode.IsExpanded)
					{
						treeNode.Collapse();
						e.Cancel = true;
					}
				}
			}
		}


		[DllImport("user32.dll")]
		private static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);


		private void TreeView_DragOver(object sender, DragEventArgs e)
		{
			Point point = this.treeView.PointToClient(Cursor.Position);
			if ((float)point.Y + 20f > (float)this.treeView.Height)
			{
				ParticleEffectPropertiesTreeControl.SendMessage(this.treeView.Handle, 277, (IntPtr)1, (IntPtr)0);
				return;
			}
			if ((float)point.Y < (float)this.treeView.Top + 20f)
			{
				ParticleEffectPropertiesTreeControl.SendMessage(this.treeView.Handle, 277, (IntPtr)0, (IntPtr)0);
			}
		}


		private bool IsEmitterSolo(ParticleEmitterId emitterId)
		{
			return this.model.SoloEmitterId != null && this.model.SoloEmitterId.Value.Value == emitterId.Value;
		}


		private void OpenContextMenu(TreeNode node, Point location)
		{
			this.contextMenuNode = node;
			ParticleEffectPropertiesTreeControl.TreeNodeTag treeNodeTag = (ParticleEffectPropertiesTreeControl.TreeNodeTag)node.Tag;
			this.renameMenuItem.Enabled = treeNodeTag.CanRename;
			this.deleteMenuItem.Enabled = treeNodeTag.CanDelete;
			this.copyMenuItem.Enabled = treeNodeTag.CanCopy;
			this.pasteMenuItem.Enabled = (this.copiedTag != null);
			this.soloMenuItem.Enabled = (treeNodeTag.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.Emitter);
			this.soloMenuItem.Checked = (treeNodeTag.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.Emitter && this.IsEmitterSolo(treeNodeTag.EmitterId.Value));
			this.contextMenu.Show(this.treeView.PointToScreen(location));
		}


		private void TreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				this.treeView.SelectedNode = e.Node;
				this.OpenContextMenu(e.Node, e.Location);
			}
		}



		public ParticleEffectModel Model
		{
			set
			{
				this.model = value;
				this.model.ParticleEffectChanged += this.Model_ParticleEffectChanged;
				this.model.ParticleEffectComponentsChanged += this.Model_ParticleEffectComponentsChanged;
				this.model.ParticleEffectPropertyValueChanged += this.Model_ParticleEffectPropertyValueChanged;
			}
		}


		private void Model_ParticleEffectPropertyValueChanged(string label)
		{
			if (label.ToLower() == "name")
			{
				this.UpdateTreeView(true);
				return;
			}
			if (label.ToLower() == "isvisible")
			{
				this.UpdateTreeView(true);
			}
		}


		private void Model_ParticleEffectComponentsChanged(ParticleEffect particleEffect)
		{
			this.UpdateTreeView(true);
		}


		private void Model_ParticleEffectChanged(ParticleEffect particleEffect)
		{
			this.UpdateTreeView(false);
		}


		private void AddExpandedTagsRecursive(TreeNodeCollection nodes, List<ParticleEffectPropertiesTreeControl.TreeNodeTag> tags)
		{
			foreach (TreeNode treeNode in nodes.Cast<TreeNode>())
			{
				if (treeNode.IsExpanded)
				{
					tags.Add((ParticleEffectPropertiesTreeControl.TreeNodeTag)treeNode.Tag);
				}
				this.AddExpandedTagsRecursive(treeNode.Nodes, tags);
			}
		}


		private void UpdateTreeView(bool restoreUiState)
		{
			TreeViewUiState<ParticleEffectPropertiesTreeControl.TreeNodeTag> treeViewUiState = new TreeViewUiState<ParticleEffectPropertiesTreeControl.TreeNodeTag>(this.treeView);
			this.model.SelectedParticleEffectComponent = null;
			this.treeView.BeginUpdate();
			this.nodesRoot.Nodes.Clear();
			this.emittersRoot.Nodes.Clear();
			this.modifiersRoot.Nodes.Clear();
			this.maxScalarRoot.Nodes.Clear();
			ParticleEffectDefinition particleEffectDefinition = this.model.ParticleEffectDefinition;
			if (particleEffectDefinition != null)
			{
				for (int i = 0; i < particleEffectDefinition.Nodes.Count; i++)
				{
					ParticleEmitterNode particleEmitterNode = particleEffectDefinition.Nodes[i];
					ParticleEffectPropertiesTreeControl.TreeNodeTag tag = new ParticleEffectPropertiesTreeControl.TreeNodeTag(ParticleEffectPropertiesTreeControl.TreeNodeTagType.Node);
					tag.NodeId = new ParticleEmitterNodeId?(particleEmitterNode.Id);
					TreeNode treeNode = this.AddParticleEffectComponentNode(this.nodesRoot, particleEffectDefinition.Nodes[i].Name, tag, null);
					int count = particleEmitterNode.AttachedEmitters.Count;
					if (count == 0)
					{
						treeNode.ForeColor = Color.Gray;
					}
					for (int j = 0; j < count; j++)
					{
						ParticleEmitter particleEmitter = particleEmitterNode.AttachedEmitters[j];
						ParticleEffectPropertiesTreeControl.TreeNodeTag tag2 = new ParticleEffectPropertiesTreeControl.TreeNodeTag(ParticleEffectPropertiesTreeControl.TreeNodeTagType.NodeAttachedEmitter);
						tag2.NodeId = new ParticleEmitterNodeId?(particleEmitterNode.Id);
						tag2.EmitterId = new ParticleEmitterId?(particleEmitter.Id);
						this.AddParticleEffectComponentNode(treeNode, particleEmitter.Name, tag2, new ParticleEffectPropertiesTreeControl.TreeNodeStatus?(ParticleEffectPropertiesTreeControl.TreeNodeStatus.AttachedEmitter));
					}
				}
				HashSet<ParticleModifierId> hashSet = new HashSet<ParticleModifierId>();
				for (int k = 0; k < particleEffectDefinition.Emitters.Count; k++)
				{
					ParticleEmitter particleEmitter2 = particleEffectDefinition.Emitters[k];
					ParticleEffectPropertiesTreeControl.TreeNodeTag tag3 = new ParticleEffectPropertiesTreeControl.TreeNodeTag(ParticleEffectPropertiesTreeControl.TreeNodeTagType.Emitter);
					tag3.EmitterId = new ParticleEmitterId?(particleEmitter2.Id);
					ParticleEffectPropertiesTreeControl.TreeNodeStatus? status = null;
					if (this.model.SoloEmitterId != null)
					{
						status = new ParticleEffectPropertiesTreeControl.TreeNodeStatus?((this.model.SoloEmitterId.Value.Value == particleEmitter2.Id.Value) ? ParticleEffectPropertiesTreeControl.TreeNodeStatus.Solo : ParticleEffectPropertiesTreeControl.TreeNodeStatus.NotSolo);
					}
					else if (!particleEmitter2.Properties.IsVisible)
					{
						status = new ParticleEffectPropertiesTreeControl.TreeNodeStatus?(ParticleEffectPropertiesTreeControl.TreeNodeStatus.NotVisible);
					}
					else if (!particleEffectDefinition.EmitterToNodeAttachments.DoesAnyNodeContainEmitter(particleEmitter2))
					{
						status = new ParticleEffectPropertiesTreeControl.TreeNodeStatus?(ParticleEffectPropertiesTreeControl.TreeNodeStatus.NotVisible);
					}
					TreeNode parent = this.AddParticleEffectComponentNode(this.emittersRoot, particleEmitter2.Name, tag3, status);
					int count2 = particleEffectDefinition.Emitters[k].AttachedModifiers.Count;
					for (int l = 0; l < count2; l++)
					{
						ParticleModifier particleModifier = particleEffectDefinition.Emitters[k].AttachedModifiers[l];
						ParticleEffectPropertiesTreeControl.TreeNodeTag tag4 = new ParticleEffectPropertiesTreeControl.TreeNodeTag(ParticleEffectPropertiesTreeControl.TreeNodeTagType.EmitterAttachedModifier);
						tag4.EmitterId = new ParticleEmitterId?(particleEmitter2.Id);
						tag4.ModifierId = new ParticleModifierId?(particleModifier.Id);
						this.AddParticleEffectComponentNode(parent, particleModifier.Name, tag4, new ParticleEffectPropertiesTreeControl.TreeNodeStatus?(ParticleEffectPropertiesTreeControl.TreeNodeStatus.AttachedModifier));
						hashSet.Add(particleModifier.Id);
					}
				}
				for (int m = 0; m < particleEffectDefinition.Modifiers.Count; m++)
				{
					ParticleModifier particleModifier2 = particleEffectDefinition.Modifiers[m];
					ParticleEffectPropertiesTreeControl.TreeNodeTag tag5 = new ParticleEffectPropertiesTreeControl.TreeNodeTag(ParticleEffectPropertiesTreeControl.TreeNodeTagType.Modifier);
					tag5.ModifierId = new ParticleModifierId?(particleModifier2.Id);
					ParticleEffectPropertiesTreeControl.TreeNodeStatus? status2 = null;
					if (!hashSet.Contains(particleModifier2.Id))
					{
						status2 = new ParticleEffectPropertiesTreeControl.TreeNodeStatus?(ParticleEffectPropertiesTreeControl.TreeNodeStatus.NoAttachedEmitters);
					}
					this.AddParticleEffectComponentNode(this.modifiersRoot, particleModifier2.Name, tag5, status2);
				}
			}
			this.treeView.EndUpdate();
			if (restoreUiState)
			{
				treeViewUiState.ApplyTo(this.treeView);
				if (treeViewUiState.SelectedTag != null)
				{
					this.model.SelectedParticleEffectComponent = this.TryGetParticleEffectComponentFromTag(treeViewUiState.SelectedTag.Value);
				}
			}
		}


		private Color GetTreeNodeStatusForeColor(ParticleEffectPropertiesTreeControl.TreeNodeStatus status)
		{
			switch (status)
			{
			case ParticleEffectPropertiesTreeControl.TreeNodeStatus.Solo:
				return Color.DarkRed;
			case ParticleEffectPropertiesTreeControl.TreeNodeStatus.NotSolo:
				return Color.LightGray;
			case ParticleEffectPropertiesTreeControl.TreeNodeStatus.NotVisible:
				return Color.LightGray;
			case ParticleEffectPropertiesTreeControl.TreeNodeStatus.NoAttachedEmitters:
				return Color.Red;
			case ParticleEffectPropertiesTreeControl.TreeNodeStatus.AttachedModifier:
				return Color.DarkSlateGray;
			case ParticleEffectPropertiesTreeControl.TreeNodeStatus.AttachedEmitter:
				return Color.DarkSlateGray;
			default:
				return Color.Black;
			}
		}


		private TreeNode AddParticleEffectComponentNode(TreeNode parent, string name, ParticleEffectPropertiesTreeControl.TreeNodeTag tag, ParticleEffectPropertiesTreeControl.TreeNodeStatus? status = null)
		{
			TreeNode treeNode = parent.Nodes.Add(name);
			if (status != null)
			{
				treeNode.ForeColor = this.GetTreeNodeStatusForeColor(status.Value);
			}
			treeNode.Tag = tag;
			return treeNode;
		}


		private object TryGetParticleEffectComponentFromTag(ParticleEffectPropertiesTreeControl.TreeNodeTag tag)
		{
			if (tag.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.Node)
			{
				return this.model.ParticleEffectDefinition.Nodes.Find(tag.NodeId.Value);
			}
			if (tag.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.NodeAttachedEmitter)
			{
				return this.model.ParticleEffectDefinition.Emitters.Find(tag.EmitterId.Value);
			}
			if (tag.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.Emitter)
			{
				return this.model.ParticleEffectDefinition.Emitters.Find(tag.EmitterId.Value);
			}
			if (tag.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.EmitterAttachedModifier)
			{
				return this.model.ParticleEffectDefinition.Modifiers.Find(tag.ModifierId.Value);
			}
			if (tag.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.Modifier)
			{
				return this.model.ParticleEffectDefinition.Modifiers.Find(tag.ModifierId.Value);
			}
			if (tag.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.MaxScalarRoot)
			{
				return this.model.ParticleEffectDefinition.MaxScalar;
			}
			return null;
		}


		private void TryRemoveParticleEffectComponentFromTag(ParticleEffectPropertiesTreeControl.TreeNodeTag tag)
		{
			if (tag.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.Node)
			{
				this.model.RemoveNode(tag.NodeId.Value);
			}
			if (tag.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.NodeAttachedEmitter)
			{
				this.model.RemoveEmitterToNodeAttachment(tag.EmitterId.Value, tag.NodeId.Value);
			}
			if (tag.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.Emitter)
			{
				this.model.RemoveEmitter(tag.EmitterId.Value);
			}
			if (tag.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.Modifier)
			{
				this.model.RemoveModifier(tag.ModifierId.Value);
			}
			if (tag.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.EmitterAttachedModifier)
			{
				this.model.RemoveModifierToEmitterAttachment(tag.ModifierId.Value, tag.EmitterId.Value);
			}
		}


		private void treeView_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete && this.treeView.SelectedNode != null)
			{
				this.TryRemoveParticleEffectComponentFromTag((ParticleEffectPropertiesTreeControl.TreeNodeTag)this.treeView.SelectedNode.Tag);
				e.Handled = true;
			}
			if (e.KeyCode == Keys.C && e.Control)
			{
				if (this.treeView.SelectedNode != null)
				{
					this.copiedTag = new ParticleEffectPropertiesTreeControl.TreeNodeTag?((ParticleEffectPropertiesTreeControl.TreeNodeTag)this.treeView.SelectedNode.Tag);
				}
				e.Handled = true;
			}
			if (e.KeyCode == Keys.V && e.Control)
			{
				this.TryPasteCopiedTag();
				e.Handled = true;
			}
			if (e.KeyCode == Keys.F2 && this.treeView.SelectedNode != null)
			{
				this.treeView.SelectedNode.BeginEdit();
				e.Handled = true;
			}
		}



		private ParticleEffectPropertiesTreeControl.TreeNodeTag? SelectedTag
		{
			get
			{
				if (this.treeView.SelectedNode != null)
				{
					return new ParticleEffectPropertiesTreeControl.TreeNodeTag?((ParticleEffectPropertiesTreeControl.TreeNodeTag)this.treeView.SelectedNode.Tag);
				}
				return null;
			}
		}


		private void TryPasteCopiedTag()
		{
			if (this.copiedTag != null)
			{
				if (this.copiedTag.Value.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.Node)
				{
					this.model.CopyNode(this.copiedTag.Value.NodeId.Value);
					return;
				}
				if (this.copiedTag.Value.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.Emitter)
				{
					if (this.SelectedTag != null && (this.SelectedTag.Value.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.Node || this.SelectedTag.Value.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.NodeAttachedEmitter))
					{
						this.model.AddEmitterToNodeAttachment(this.copiedTag.Value.EmitterId.Value, this.SelectedTag.Value.NodeId.Value);
						this.treeView.SelectedNode.Expand();
						return;
					}
					this.model.CopyEmitter(this.copiedTag.Value.EmitterId.Value);
					return;
				}
				else if (this.copiedTag.Value.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.Modifier)
				{
					if (this.SelectedTag != null && (this.SelectedTag.Value.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.Emitter || this.SelectedTag.Value.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.EmitterAttachedModifier))
					{
						this.model.AddModifierToEmitterAttachment(this.copiedTag.Value.ModifierId.Value, this.SelectedTag.Value.EmitterId.Value, null);
						this.treeView.SelectedNode.Expand();
						return;
					}
					this.model.CopyModifier(this.copiedTag.Value.ModifierId.Value);
				}
			}
		}


		private void treeView_KeyPress(object sender, KeyPressEventArgs e)
		{
			e.Handled = true;
		}


		private void treeView_ItemDrag(object sender, ItemDragEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				ParticleEffectPropertiesTreeControl.TreeNodeTag treeNodeTag = (ParticleEffectPropertiesTreeControl.TreeNodeTag)((TreeNode)e.Item).Tag;
				if (treeNodeTag.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.Emitter || treeNodeTag.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.NodeAttachedEmitter || treeNodeTag.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.Modifier || treeNodeTag.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.EmitterAttachedModifier)
				{
					base.DoDragDrop(e.Item, DragDropEffects.Link);
				}
			}
		}


		private void treeView_DragEnter(object sender, DragEventArgs e)
		{
			e.Effect = e.AllowedEffect;
		}


		private void treeView_DragOver(object sender, DragEventArgs e)
		{
			TreeNode nodeAt = this.treeView.GetNodeAt(this.treeView.PointToClient(new Point(e.X, e.Y)));
			if (nodeAt != null)
			{
				bool flag = true;
				ParticleEffectPropertiesTreeControl.TreeNodeTag treeNodeTag = (ParticleEffectPropertiesTreeControl.TreeNodeTag)nodeAt.Tag;
				if (((ParticleEffectPropertiesTreeControl.TreeNodeTag)((TreeNode)e.Data.GetData(typeof(TreeNode))).Tag).Type == treeNodeTag.Type)
				{
					flag = false;
				}
				if (flag)
				{
					nodeAt.Expand();
				}
				this.treeView.SelectedNode = nodeAt;
			}
		}


		private void treeView_DragDrop(object sender, DragEventArgs e)
		{
			TreeNode nodeAt = this.treeView.GetNodeAt(this.treeView.PointToClient(new Point(e.X, e.Y)));
			if (nodeAt != null)
			{
				ParticleEffectPropertiesTreeControl.TreeNodeTag treeNodeTag = (ParticleEffectPropertiesTreeControl.TreeNodeTag)nodeAt.Tag;
				ParticleEffectPropertiesTreeControl.TreeNodeTag treeNodeTag2 = (ParticleEffectPropertiesTreeControl.TreeNodeTag)((TreeNode)e.Data.GetData(typeof(TreeNode))).Tag;
				if (treeNodeTag2.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.Emitter)
				{
					if (treeNodeTag.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.Emitter)
					{
						this.model.MoveEmitterBefore(treeNodeTag2.EmitterId.Value, treeNodeTag.EmitterId.Value);
						return;
					}
					if (treeNodeTag.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.Node || treeNodeTag.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.NodeAttachedEmitter)
					{
						this.model.AddEmitterToNodeAttachment(treeNodeTag2.EmitterId.Value, treeNodeTag.NodeId.Value);
						return;
					}
				}
				else if (treeNodeTag2.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.NodeAttachedEmitter)
				{
					if (treeNodeTag.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.Node || treeNodeTag.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.NodeAttachedEmitter)
					{
						this.model.AddEmitterToNodeAttachment(treeNodeTag2.EmitterId.Value, treeNodeTag.NodeId.Value);
						return;
					}
				}
				else if (treeNodeTag2.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.Modifier)
				{
					if (treeNodeTag.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.Modifier)
					{
						this.model.MoveModifierBefore(treeNodeTag2.ModifierId.Value, treeNodeTag.ModifierId.Value);
						return;
					}
					if (treeNodeTag.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.Emitter)
					{
						this.model.AddModifierToEmitterAttachment(treeNodeTag2.ModifierId.Value, treeNodeTag.EmitterId.Value, null);
						return;
					}
					if (treeNodeTag.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.EmitterAttachedModifier)
					{
						this.model.AddModifierToEmitterAttachment(treeNodeTag2.ModifierId.Value, treeNodeTag.EmitterId.Value, new ParticleModifierId?(treeNodeTag.ModifierId.Value));
						return;
					}
				}
				else if (treeNodeTag2.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.EmitterAttachedModifier)
				{
					if (treeNodeTag.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.Emitter)
					{
						this.model.AddModifierToEmitterAttachment(treeNodeTag2.ModifierId.Value, treeNodeTag.EmitterId.Value, null);
						return;
					}
					if (treeNodeTag.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.EmitterAttachedModifier)
					{
						this.model.AddModifierToEmitterAttachment(treeNodeTag2.ModifierId.Value, treeNodeTag.EmitterId.Value, treeNodeTag.ModifierId);
					}
				}
			}
		}


		private void treeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
		{
			if (e.Label != null)
			{
				ParticleEffectPropertiesTreeControl.TreeNodeTag treeNodeTag = (ParticleEffectPropertiesTreeControl.TreeNodeTag)e.Node.Tag;
				if (treeNodeTag.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.Emitter)
				{
					this.model.RenameEmitter(treeNodeTag.EmitterId.Value, e.Label);
					return;
				}
				if (treeNodeTag.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.Modifier)
				{
					this.model.RenameModifier(treeNodeTag.ModifierId.Value, e.Label);
					return;
				}
				if (treeNodeTag.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.Node)
				{
					this.model.RenameNode(treeNodeTag.NodeId.Value, e.Label);
				}
			}
		}


		private void treeView_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
		{
			if (!((ParticleEffectPropertiesTreeControl.TreeNodeTag)e.Node.Tag).CanRename)
			{
				e.CancelEdit = true;
			}
		}


		private void renameMenuItem_Click(object sender, EventArgs e)
		{
			this.contextMenuNode.BeginEdit();
		}


		private void deleteMenuItem_Click(object sender, EventArgs e)
		{
			this.TryRemoveParticleEffectComponentFromTag((ParticleEffectPropertiesTreeControl.TreeNodeTag)this.contextMenuNode.Tag);
		}


		private void copyMenuItem_Click(object sender, EventArgs e)
		{
			this.copiedTag = new ParticleEffectPropertiesTreeControl.TreeNodeTag?((ParticleEffectPropertiesTreeControl.TreeNodeTag)this.contextMenuNode.Tag);
		}


		private void pasteMenuItem_Click(object sender, EventArgs e)
		{
			this.TryPasteCopiedTag();
		}


		private void newNodeMenuItem_Click(object sender, EventArgs e)
		{
			this.model.AddNode();
		}


		private void newEmitterMenuItem_Click(object sender, EventArgs e)
		{
			this.model.AddEmitter();
		}


		private void newModifierMenuItem_Click(object sender, EventArgs e)
		{
			this.model.AddModifier();
		}


		private void soloMenuItem_Click(object sender, EventArgs e)
		{
			ParticleEmitterId value = ((ParticleEffectPropertiesTreeControl.TreeNodeTag)this.contextMenuNode.Tag).EmitterId.Value;
			if (!this.IsEmitterSolo(value))
			{
				this.model.BeginSoloEmitter(value);
				return;
			}
			this.model.EndSoloEmitter();
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
			this.treeView = new TreeView();
			this.contextMenu = new ContextMenuStrip(this.components);
			this.newMenuItem = new ToolStripMenuItem();
			this.newNodeMenuItem = new ToolStripMenuItem();
			this.newEmitterMenuItem = new ToolStripMenuItem();
			this.newModifierMenuItem = new ToolStripMenuItem();
			this.renameMenuItem = new ToolStripMenuItem();
			this.deleteMenuItem = new ToolStripMenuItem();
			this.copyMenuItem = new ToolStripMenuItem();
			this.pasteMenuItem = new ToolStripMenuItem();
			this.soloMenuItem = new ToolStripMenuItem();
			this.contextMenu.SuspendLayout();
			base.SuspendLayout();
			this.treeView.AllowDrop = true;
			this.treeView.Dock = DockStyle.Fill;
			this.treeView.LabelEdit = true;
			this.treeView.Location = new Point(0, 0);
			this.treeView.Margin = new Padding(4, 5, 4, 5);
			this.treeView.Name = "treeView";
			this.treeView.Size = new Size(576, 805);
			this.treeView.TabIndex = 0;
			this.treeView.BeforeLabelEdit += this.treeView_BeforeLabelEdit;
			this.treeView.AfterLabelEdit += this.treeView_AfterLabelEdit;
			this.treeView.ItemDrag += this.treeView_ItemDrag;
			this.treeView.DragDrop += this.treeView_DragDrop;
			this.treeView.DragEnter += this.treeView_DragEnter;
			this.treeView.DragOver += this.treeView_DragOver;
			this.treeView.KeyPress += this.treeView_KeyPress;
			this.treeView.KeyUp += this.treeView_KeyUp;
			this.contextMenu.ImageScalingSize = new Size(24, 24);
			this.contextMenu.Items.AddRange(new ToolStripItem[]
			{
				this.newMenuItem,
				this.renameMenuItem,
				this.deleteMenuItem,
				this.copyMenuItem,
				this.pasteMenuItem,
				this.soloMenuItem
			});
			this.contextMenu.Name = "contextMenu";
			this.contextMenu.Size = new Size(241, 229);
			this.newMenuItem.DropDownItems.AddRange(new ToolStripItem[]
			{
				this.newNodeMenuItem,
				this.newEmitterMenuItem,
				this.newModifierMenuItem
			});
			this.newMenuItem.Name = "newMenuItem";
			this.newMenuItem.Size = new Size(240, 32);
			this.newMenuItem.Text = "New";
			this.newNodeMenuItem.Name = "newNodeMenuItem";
			this.newNodeMenuItem.Size = new Size(270, 34);
			this.newNodeMenuItem.Text = "Node";
			this.newNodeMenuItem.Click += this.newNodeMenuItem_Click;
			this.newEmitterMenuItem.Name = "newEmitterMenuItem";
			this.newEmitterMenuItem.Size = new Size(270, 34);
			this.newEmitterMenuItem.Text = "Emitter";
			this.newEmitterMenuItem.Click += this.newEmitterMenuItem_Click;
			this.newModifierMenuItem.Name = "newModifierMenuItem";
			this.newModifierMenuItem.Size = new Size(270, 34);
			this.newModifierMenuItem.Text = "Modifier";
			this.newModifierMenuItem.Click += this.newModifierMenuItem_Click;
			this.renameMenuItem.Name = "renameMenuItem";
			this.renameMenuItem.ShortcutKeys = Keys.F2;
			this.renameMenuItem.Size = new Size(240, 32);
			this.renameMenuItem.Text = "Rename";
			this.renameMenuItem.Click += this.renameMenuItem_Click;
			this.deleteMenuItem.Name = "deleteMenuItem";
			this.deleteMenuItem.ShortcutKeys = Keys.Delete;
			this.deleteMenuItem.Size = new Size(240, 32);
			this.deleteMenuItem.Text = "Delete";
			this.deleteMenuItem.Click += this.deleteMenuItem_Click;
			this.copyMenuItem.Name = "copyMenuItem";
			this.copyMenuItem.ShortcutKeys = (Keys)131139;
			this.copyMenuItem.Size = new Size(240, 32);
			this.copyMenuItem.Text = "Copy";
			this.copyMenuItem.Click += this.copyMenuItem_Click;
			this.pasteMenuItem.Name = "pasteMenuItem";
			this.pasteMenuItem.ShortcutKeys = (Keys)131158;
			this.pasteMenuItem.Size = new Size(240, 32);
			this.pasteMenuItem.Text = "Paste";
			this.pasteMenuItem.Click += this.pasteMenuItem_Click;
			this.soloMenuItem.Name = "soloMenuItem";
			this.soloMenuItem.Size = new Size(240, 32);
			this.soloMenuItem.Text = "Solo";
			this.soloMenuItem.Click += this.soloMenuItem_Click;
			base.AutoScaleDimensions = new SizeF(9f, 20f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.treeView);
			base.Margin = new Padding(4, 5, 4, 5);
			base.Name = "ParticleEffectPropertiesTreeControl";
			base.Size = new Size(576, 805);
			this.contextMenu.ResumeLayout(false);
			base.ResumeLayout(false);
		}


		private ParticleEffectModel model;


		private TreeNode nodesRoot;


		private TreeNode emittersRoot;


		private TreeNode modifiersRoot;


		private TreeNode maxScalarRoot;


		private ParticleEffectPropertiesTreeControl.TreeNodeTag? copiedTag;


		private TreeNode contextMenuNode;


		private IContainer components;


		private TreeView treeView;


		private ContextMenuStrip contextMenu;


		private ToolStripMenuItem renameMenuItem;


		private ToolStripMenuItem deleteMenuItem;


		private ToolStripMenuItem copyMenuItem;


		private ToolStripMenuItem pasteMenuItem;


		private ToolStripMenuItem newMenuItem;


		private ToolStripMenuItem newNodeMenuItem;


		private ToolStripMenuItem newEmitterMenuItem;


		private ToolStripMenuItem newModifierMenuItem;


		private ToolStripMenuItem soloMenuItem;


		private enum TreeNodeTagType
		{

			NodesRoot,

			Node,

			NodeAttachedEmitter,

			EmittersRoot,

			Emitter,

			EmitterAttachedModifier,

			ModifiersRoot,

			Modifier,

			MaxScalarRoot
		}


		private enum TreeNodeStatus
		{

			Solo,

			NotSolo,

			NotVisible,

			NoAttachedEmitters,

			AttachedModifier,

			AttachedEmitter
		}


		private struct TreeNodeTag
		{



			public ParticleEffectPropertiesTreeControl.TreeNodeTagType Type { get; set; }




			public ParticleEmitterNodeId? NodeId { get; set; }




			public ParticleEmitterId? EmitterId { get; set; }




			public ParticleModifierId? ModifierId { get; set; }




			public ParticleMaxScalarId? MaxScalarId { get; set; }


			public TreeNodeTag(ParticleEffectPropertiesTreeControl.TreeNodeTagType type)
			{
				this.Type = type;
				this.NodeId = null;
				this.EmitterId = null;
				this.ModifierId = null;
				this.MaxScalarId = null;
			}



			public bool CanRename
			{
				get
				{
					return this.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.Emitter || this.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.Modifier || this.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.Node;
				}
			}



			public bool CanDelete
			{
				get
				{
					return this.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.Emitter || this.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.Modifier || this.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.Node || this.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.NodeAttachedEmitter || this.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.EmitterAttachedModifier;
				}
			}



			public bool CanCopy
			{
				get
				{
					return this.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.Emitter || this.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.Modifier || this.Type == ParticleEffectPropertiesTreeControl.TreeNodeTagType.Node;
				}
			}
		}
	}
}
