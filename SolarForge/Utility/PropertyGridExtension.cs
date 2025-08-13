using System;
using System.Windows.Forms;

namespace SolarForge.Utility
{

	public static class PropertyGridExtension
	{

		public static GridItem TryGetRootGridItem(this PropertyGrid propertyGrid)
		{
			GridItem gridItem = propertyGrid.SelectedGridItem;
			while (gridItem != null && gridItem.Parent != null)
			{
				gridItem = gridItem.Parent;
			}
			return gridItem;
		}


		public static void ExpandGridItem(this PropertyGrid propertyGrid, GridItemType gridItemType, string label)
		{
			GridItem gridItem = propertyGrid.TryGetRootGridItem();
			if (gridItem != null)
			{
				PropertyGridExtension.ExpandGridItemRecursive(gridItem, gridItemType, label);
			}
		}


		private static void ExpandGridItemRecursive(GridItem item, GridItemType gridItemType, string label)
		{
			if (item.GridItemType == gridItemType && item.Label == label)
			{
				item.Expanded = true;
				return;
			}
			foreach (object obj in item.GridItems)
			{
				PropertyGridExtension.ExpandGridItemRecursive((GridItem)obj, gridItemType, label);
			}
		}
	}
}
