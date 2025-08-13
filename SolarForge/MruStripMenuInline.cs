using System;
using System.Windows.Forms;

namespace SolarForge
{

	public class MruStripMenuInline : MruStripMenu
	{

		public MruStripMenuInline(ToolStripMenuItem owningMenu, ToolStripMenuItem recentFileMenuItem, MruStripMenu.ClickedHandler clickedHandler) : this(owningMenu, recentFileMenuItem, clickedHandler, null, false, 4)
		{
		}


		public MruStripMenuInline(ToolStripMenuItem owningMenu, ToolStripMenuItem recentFileMenuItem, MruStripMenu.ClickedHandler clickedHandler, int maxEntries) : this(owningMenu, recentFileMenuItem, clickedHandler, null, false, maxEntries)
		{
		}


		public MruStripMenuInline(ToolStripMenuItem owningMenu, ToolStripMenuItem recentFileMenuItem, MruStripMenu.ClickedHandler clickedHandler, string registryKeyName) : this(owningMenu, recentFileMenuItem, clickedHandler, registryKeyName, true, 4)
		{
		}


		public MruStripMenuInline(ToolStripMenuItem owningMenu, ToolStripMenuItem recentFileMenuItem, MruStripMenu.ClickedHandler clickedHandler, string registryKeyName, int maxEntries) : this(owningMenu, recentFileMenuItem, clickedHandler, registryKeyName, true, maxEntries)
		{
		}


		public MruStripMenuInline(ToolStripMenuItem owningMenu, ToolStripMenuItem recentFileMenuItem, MruStripMenu.ClickedHandler clickedHandler, string registryKeyName, bool loadFromRegistry) : this(owningMenu, recentFileMenuItem, clickedHandler, registryKeyName, loadFromRegistry, 4)
		{
		}


		public MruStripMenuInline(ToolStripMenuItem owningMenu, ToolStripMenuItem recentFileMenuItem, MruStripMenu.ClickedHandler clickedHandler, string registryKeyName, bool loadFromRegistry, int maxEntries)
		{
			this.maxShortenPathLength = 48;
			this.owningMenu = owningMenu;
			this.firstMenuItem = recentFileMenuItem;
			base.Init(recentFileMenuItem, clickedHandler, registryKeyName, loadFromRegistry, maxEntries);
		}



		public override ToolStripItemCollection MenuItems
		{
			get
			{
				return this.owningMenu.DropDownItems;
			}
		}



		public override int StartIndex
		{
			get
			{
				return this.MenuItems.IndexOf(this.firstMenuItem);
			}
		}



		public override int EndIndex
		{
			get
			{
				return this.StartIndex + this.numEntries;
			}
		}



		public override bool IsInline
		{
			get
			{
				return true;
			}
		}


		protected override void Enable()
		{
			this.MenuItems.Remove(this.recentFileMenuItem);
		}


		protected override void SetFirstFile(MruStripMenu.MruMenuItem menuItem)
		{
			this.firstMenuItem = menuItem;
		}


		protected override void Disable()
		{
			int index = this.MenuItems.IndexOf(this.firstMenuItem);
			this.MenuItems.RemoveAt(index);
			this.MenuItems.Insert(index, this.recentFileMenuItem);
			this.firstMenuItem = this.recentFileMenuItem;
		}


		protected ToolStripMenuItem owningMenu;


		protected ToolStripMenuItem firstMenuItem;
	}
}
