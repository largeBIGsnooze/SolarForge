using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SolarForge.Utility
{

	internal class TreeViewUiState<Tag> where Tag : struct
	{



		public Tag? SelectedTag { get; set; }




		public List<Tag> ExpandedTags { get; set; }


		public TreeViewUiState(TreeView treeView)
		{
			if (treeView.SelectedNode != null)
			{
				this.SelectedTag = new Tag?((Tag)((object)treeView.SelectedNode.Tag));
			}
			this.ExpandedTags = new List<Tag>();
			this.AddExpandedTagsRecursive(treeView.Nodes);
		}


		public static void SelectAndExpandToNodeWithTag(TreeView treeView, Tag tag)
		{
			if (TreeViewUiState<Tag>.TryGetTreeNodeFromTag(treeView, tag) != null)
			{
				treeView.SelectedNode = TreeViewUiState<Tag>.TryGetTreeNodeFromTag(treeView, tag);
				if (treeView.SelectedNode != null)
				{
					TreeViewUiState<Tag>.RecursiveExpand(treeView.SelectedNode.Parent);
				}
			}
		}


		public void ApplyTo(TreeView treeView)
		{
			if (this.SelectedTag != null)
			{
				TreeViewUiState<Tag>.SelectAndExpandToNodeWithTag(treeView, this.SelectedTag.Value);
			}
			foreach (Tag tag in this.ExpandedTags)
			{
				TreeNode treeNode = TreeViewUiState<Tag>.TryGetTreeNodeFromTag(treeView, tag);
				if (treeNode != null)
				{
					treeNode.Expand();
				}
			}
		}


		private void AddExpandedTagsRecursive(TreeNodeCollection nodes)
		{
			foreach (TreeNode treeNode in nodes.Cast<TreeNode>())
			{
				if (treeNode.IsExpanded)
				{
					this.ExpandedTags.Add((Tag)((object)treeNode.Tag));
				}
				this.AddExpandedTagsRecursive(treeNode.Nodes);
			}
		}


		private static void RecursiveExpand(TreeNode node)
		{
			if (node != null)
			{
				node.Expand();
				if (node.Parent != null)
				{
					TreeViewUiState<Tag>.RecursiveExpand(node.Parent);
				}
			}
		}


		private static TreeNode TryGetTreeNodeFromTag(TreeView treeView, Tag tag)
		{
			return TreeViewUiState<Tag>.TryGetTreeNodeFromTagRecursive(treeView.Nodes, tag);
		}


		private static TreeNode TryGetTreeNodeFromTagRecursive(TreeNodeCollection nodes, Tag tag)
		{
			foreach (TreeNode treeNode in nodes.Cast<TreeNode>())
			{
				Tag tag2 = (Tag)((object)treeNode.Tag);
				if (tag2.Equals(tag))
				{
					return treeNode;
				}
				TreeNode treeNode2 = TreeViewUiState<Tag>.TryGetTreeNodeFromTagRecursive(treeNode.Nodes, tag);
				if (treeNode2 != null)
				{
					return treeNode2;
				}
			}
			return null;
		}
	}
}
