using System;
using System.Windows.Forms;

namespace SolarForge
{

	public class MruMenuInline : MruMenu
	{

		public MruMenuInline(MenuItem _recentFileMenuItem, MruMenu.ClickedHandler _clickedHandler)
		{
			this.maxShortenPathLength = 128;
			this.firstMenuItem = _recentFileMenuItem;
			base.Init(_recentFileMenuItem, _clickedHandler, null, false, 4);
		}


		public MruMenuInline(MenuItem _recentFileMenuItem, MruMenu.ClickedHandler _clickedHandler, int _maxEntries)
		{
			this.maxShortenPathLength = 128;
			this.firstMenuItem = _recentFileMenuItem;
			base.Init(_recentFileMenuItem, _clickedHandler, null, false, _maxEntries);
		}


		public MruMenuInline(MenuItem _recentFileMenuItem, MruMenu.ClickedHandler _clickedHandler, string _registryKeyName)
		{
			this.maxShortenPathLength = 128;
			this.firstMenuItem = _recentFileMenuItem;
			base.Init(_recentFileMenuItem, _clickedHandler, _registryKeyName, true, 4);
		}


		public MruMenuInline(MenuItem _recentFileMenuItem, MruMenu.ClickedHandler _clickedHandler, string _registryKeyName, int _maxEntries)
		{
			this.maxShortenPathLength = 128;
			this.firstMenuItem = _recentFileMenuItem;
			base.Init(_recentFileMenuItem, _clickedHandler, _registryKeyName, true, _maxEntries);
		}


		public MruMenuInline(MenuItem _recentFileMenuItem, MruMenu.ClickedHandler _clickedHandler, string _registryKeyName, bool loadFromRegistry)
		{
			this.maxShortenPathLength = 128;
			this.firstMenuItem = _recentFileMenuItem;
			base.Init(_recentFileMenuItem, _clickedHandler, _registryKeyName, loadFromRegistry, 4);
		}


		public MruMenuInline(MenuItem _recentFileMenuItem, MruMenu.ClickedHandler _clickedHandler, string _registryKeyName, bool loadFromRegistry, int _maxEntries)
		{
			this.maxShortenPathLength = 128;
			this.firstMenuItem = _recentFileMenuItem;
			base.Init(_recentFileMenuItem, _clickedHandler, _registryKeyName, loadFromRegistry, _maxEntries);
		}



		public override Menu.MenuItemCollection MenuItems
		{
			get
			{
				return this.firstMenuItem.Parent.MenuItems;
			}
		}



		public override int StartIndex
		{
			get
			{
				return this.firstMenuItem.Index;
			}
		}



		public override int EndIndex
		{
			get
			{
				return this.StartIndex + this.numEntries;
			}
		}


		protected override void Enable()
		{
			this.MenuItems.Remove(this.recentFileMenuItem);
		}


		protected override void SetFirstFile(MenuItem menuItem)
		{
			this.firstMenuItem = menuItem;
		}


		protected override void Disable()
		{
			this.MenuItems.Add(this.firstMenuItem.Index, this.recentFileMenuItem);
			this.MenuItems.Remove(this.firstMenuItem);
			this.firstMenuItem = this.recentFileMenuItem;
		}


		protected MenuItem firstMenuItem;
	}
}
