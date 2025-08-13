using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

namespace SolarForge
{

	public class MruStripMenu
	{

		protected MruStripMenu()
		{
		}


		public MruStripMenu(ToolStripMenuItem recentFileMenuItem, MruStripMenu.ClickedHandler clickedHandler) : this(recentFileMenuItem, clickedHandler, null, false, 4)
		{
		}


		public MruStripMenu(ToolStripMenuItem recentFileMenuItem, MruStripMenu.ClickedHandler clickedHandler, int maxEntries) : this(recentFileMenuItem, clickedHandler, null, false, maxEntries)
		{
		}


		public MruStripMenu(ToolStripMenuItem recentFileMenuItem, MruStripMenu.ClickedHandler clickedHandler, string registryKeyName) : this(recentFileMenuItem, clickedHandler, registryKeyName, true, 4)
		{
		}


		public MruStripMenu(ToolStripMenuItem recentFileMenuItem, MruStripMenu.ClickedHandler clickedHandler, string registryKeyName, int maxEntries) : this(recentFileMenuItem, clickedHandler, registryKeyName, true, maxEntries)
		{
		}


		public MruStripMenu(ToolStripMenuItem recentFileMenuItem, MruStripMenu.ClickedHandler clickedHandler, string registryKeyName, bool loadFromRegistry) : this(recentFileMenuItem, clickedHandler, registryKeyName, loadFromRegistry, 4)
		{
		}


		public MruStripMenu(ToolStripMenuItem recentFileMenuItem, MruStripMenu.ClickedHandler clickedHandler, string registryKeyName, bool loadFromRegistry, int maxEntries)
		{
			this.Init(recentFileMenuItem, clickedHandler, registryKeyName, loadFromRegistry, maxEntries);
		}


		protected void Init(ToolStripMenuItem recentFileMenuItem, MruStripMenu.ClickedHandler clickedHandler, string registryKeyName, bool loadFromRegistry, int maxEntries)
		{
			if (recentFileMenuItem == null)
			{
				throw new ArgumentNullException("recentFileMenuItem");
			}
			this.recentFileMenuItem = recentFileMenuItem;
			this.recentFileMenuItem.Checked = false;
			this.recentFileMenuItem.Enabled = false;
			this.maxEntries = maxEntries;
			this.clickedHandler = clickedHandler;
			if (registryKeyName != null)
			{
				this.RegistryKeyName = registryKeyName;
				if (loadFromRegistry)
				{
					this.LoadFromRegistry();
				}
			}
		}


		protected void OnClick(object sender, EventArgs e)
		{
			MruStripMenu.MruMenuItem mruMenuItem = (MruStripMenu.MruMenuItem)sender;
			this.clickedHandler(this.MenuItems.IndexOf(mruMenuItem) - this.StartIndex, mruMenuItem.Filename);
		}



		public virtual ToolStripItemCollection MenuItems
		{
			get
			{
				return this.recentFileMenuItem.DropDownItems;
			}
		}



		public virtual int StartIndex
		{
			get
			{
				return 0;
			}
		}



		public virtual int EndIndex
		{
			get
			{
				return this.numEntries;
			}
		}



		public int NumEntries
		{
			get
			{
				return this.numEntries;
			}
		}




		public int MaxEntries
		{
			get
			{
				return this.maxEntries;
			}
			set
			{
				if (value > 16)
				{
					this.maxEntries = 16;
					return;
				}
				this.maxEntries = ((value < 4) ? 4 : value);
				int index = this.StartIndex + this.maxEntries;
				while (this.numEntries > this.maxEntries)
				{
					this.MenuItems.RemoveAt(index);
					this.numEntries--;
				}
			}
		}




		public int MaxShortenPathLength
		{
			get
			{
				return this.maxShortenPathLength;
			}
			set
			{
				this.maxShortenPathLength = ((value < 16) ? 16 : value);
			}
		}



		public virtual bool IsInline
		{
			get
			{
				return false;
			}
		}


		protected virtual void Enable()
		{
			this.recentFileMenuItem.Enabled = true;
		}


		protected virtual void Disable()
		{
			this.recentFileMenuItem.Enabled = false;
		}


		protected virtual void SetFirstFile(MruStripMenu.MruMenuItem menuItem)
		{
		}


		public void SetFirstFile(int number)
		{
			if (number > 0 && this.numEntries > 1 && number < this.numEntries)
			{
				MruStripMenu.MruMenuItem mruMenuItem = (MruStripMenu.MruMenuItem)this.MenuItems[this.StartIndex + number];
				this.MenuItems.RemoveAt(this.StartIndex + number);
				this.MenuItems.Insert(this.StartIndex, mruMenuItem);
				this.SetFirstFile(mruMenuItem);
				this.FixupPrefixes(0);
			}
		}


		public static string FixupEntryname(int number, string entryname)
		{
			if (number < 9)
			{
				return "&" + (number + 1).ToString() + "  " + entryname;
			}
			if (number == 9)
			{
				return "1&0  " + entryname;
			}
			return (number + 1).ToString() + "  " + entryname;
		}


		protected void FixupPrefixes(int startNumber)
		{
			if (startNumber < 0)
			{
				startNumber = 0;
			}
			if (startNumber < this.maxEntries)
			{
				int i = this.StartIndex + startNumber;
				while (i < this.EndIndex)
				{
					this.MenuItems[i].Text = MruStripMenu.FixupEntryname(startNumber, this.MenuItems[i].Text.Substring((startNumber == 9) ? 5 : 4));
					i++;
					startNumber++;
				}
			}
		}


		public static string ShortenPathname(string pathname, int maxLength)
		{
			if (pathname.Length <= maxLength)
			{
				return pathname;
			}
			string text = Path.GetPathRoot(pathname);
			if (text.Length > 3)
			{
				text += Path.DirectorySeparatorChar.ToString();
			}
			string[] array = pathname.Substring(text.Length).Split(new char[]
			{
				Path.DirectorySeparatorChar,
				Path.AltDirectorySeparatorChar
			});
			int num = array.GetLength(0) - 1;
			if (array.GetLength(0) == 1)
			{
				if (array[0].Length <= 5)
				{
					return pathname;
				}
				if (text.Length + 6 >= maxLength)
				{
					return text + array[0].Substring(0, 3) + "...";
				}
				return pathname.Substring(0, maxLength - 3) + "...";
			}
			else if (text.Length + 4 + array[num].Length > maxLength)
			{
				text += "...\\";
				int num2 = array[num].Length;
				if (num2 < 6)
				{
					return text + array[num];
				}
				if (text.Length + 6 >= maxLength)
				{
					num2 = 3;
				}
				else
				{
					num2 = maxLength - text.Length - 3;
				}
				return text + array[num].Substring(0, num2) + "...";
			}
			else
			{
				if (array.GetLength(0) == 2)
				{
					return text + "...\\" + array[1];
				}
				int num3 = 0;
				int num4 = 0;
				for (int i = 0; i < num; i++)
				{
					if (array[i].Length > num3)
					{
						num4 = i;
						num3 = array[i].Length;
					}
				}
				int j = pathname.Length - num3 + 3;
				int num5 = num4 + 1;
				while (j > maxLength)
				{
					if (num4 > 0)
					{
						j -= array[--num4].Length - 1;
					}
					if (j <= maxLength)
					{
						break;
					}
					if (num5 < num)
					{
						j -= array[++num5].Length - 1;
					}
					if (num4 == 0 && num5 == num)
					{
						break;
					}
				}
				for (int k = 0; k < num4; k++)
				{
					text = text + array[k] + "\\";
				}
				text += "...\\";
				for (int l = num5; l < num; l++)
				{
					text = text + array[l] + "\\";
				}
				return text + array[num];
			}
		}


		public int FindFilenameNumber(string filename)
		{
			if (filename == null)
			{
				throw new ArgumentNullException("filename");
			}
			if (filename.Length == 0)
			{
				throw new ArgumentException("filename");
			}
			if (this.numEntries > 0)
			{
				int num = 0;
				int i = this.StartIndex;
				while (i < this.EndIndex)
				{
					if (string.Compare(((MruStripMenu.MruMenuItem)this.MenuItems[i]).Filename, filename, true) == 0)
					{
						return num;
					}
					i++;
					num++;
				}
			}
			return -1;
		}


		public int FindFilenameMenuIndex(string filename)
		{
			int num = this.FindFilenameNumber(filename);
			if (num >= 0)
			{
				return this.StartIndex + num;
			}
			return -1;
		}


		public int GetMenuIndex(int number)
		{
			if (number < 0 || number >= this.numEntries)
			{
				throw new ArgumentOutOfRangeException("number");
			}
			return this.StartIndex + number;
		}


		public string GetFileAt(int number)
		{
			if (number < 0 || number >= this.numEntries)
			{
				throw new ArgumentOutOfRangeException("number");
			}
			return ((MruStripMenu.MruMenuItem)this.MenuItems[this.StartIndex + number]).Filename;
		}


		public string[] GetFiles()
		{
			string[] array = new string[this.numEntries];
			int num = this.StartIndex;
			int i = 0;
			while (i < array.GetLength(0))
			{
				array[i] = ((MruStripMenu.MruMenuItem)this.MenuItems[num]).Filename;
				i++;
				num++;
			}
			return array;
		}


		public string[] GetFilesFullEntrystring()
		{
			string[] array = new string[this.numEntries];
			int num = this.StartIndex;
			int i = 0;
			while (i < array.GetLength(0))
			{
				array[i] = this.MenuItems[num].Text;
				i++;
				num++;
			}
			return array;
		}


		public void SetFiles(string[] filenames)
		{
			this.RemoveAll();
			for (int i = filenames.GetLength(0) - 1; i >= 0; i--)
			{
				this.AddFile(filenames[i]);
			}
		}


		public void AddFiles(string[] filenames)
		{
			for (int i = filenames.GetLength(0) - 1; i >= 0; i--)
			{
				this.AddFile(filenames[i]);
			}
		}


		public void AddFile(string filename)
		{
			string fullPath = Path.GetFullPath(filename);
			this.AddFile(fullPath, MruStripMenu.ShortenPathname(fullPath, this.MaxShortenPathLength));
		}


		public void AddFile(string filename, string entryname)
		{
			if (filename == null)
			{
				throw new ArgumentNullException("filename");
			}
			if (filename.Length == 0)
			{
				throw new ArgumentException("filename");
			}
			if (this.numEntries > 0)
			{
				int num = this.FindFilenameMenuIndex(filename);
				if (num >= 0)
				{
					this.SetFirstFile(num - this.StartIndex);
					return;
				}
			}
			if (this.numEntries >= this.maxEntries)
			{
				if (this.numEntries > 1)
				{
					MruStripMenu.MruMenuItem mruMenuItem = (MruStripMenu.MruMenuItem)this.MenuItems[this.StartIndex + this.numEntries - 1];
					this.MenuItems.RemoveAt(this.StartIndex + this.numEntries - 1);
					mruMenuItem.Text = MruStripMenu.FixupEntryname(0, entryname);
					mruMenuItem.Filename = filename;
					this.MenuItems.Insert(this.StartIndex, mruMenuItem);
					this.SetFirstFile(mruMenuItem);
					this.FixupPrefixes(1);
				}
				return;
			}
			MruStripMenu.MruMenuItem mruMenuItem2 = new MruStripMenu.MruMenuItem(filename, MruStripMenu.FixupEntryname(0, entryname), new EventHandler(this.OnClick));
			this.MenuItems.Insert(this.StartIndex, mruMenuItem2);
			this.SetFirstFile(mruMenuItem2);
			int num2 = this.numEntries;
			this.numEntries = num2 + 1;
			if (num2 == 0)
			{
				this.Enable();
				return;
			}
			this.FixupPrefixes(1);
		}


		public void RemoveFile(int number)
		{
			if (number >= 0 && number < this.numEntries)
			{
				int num = this.numEntries - 1;
				this.numEntries = num;
				if (num == 0)
				{
					this.Disable();
					return;
				}
				int startIndex = this.StartIndex;
				if (number == 0)
				{
					this.SetFirstFile((MruStripMenu.MruMenuItem)this.MenuItems[startIndex + 1]);
				}
				this.MenuItems.RemoveAt(startIndex + number);
				if (number < this.numEntries)
				{
					this.FixupPrefixes(number);
				}
			}
		}


		public void RemoveFile(string filename)
		{
			if (this.numEntries > 0)
			{
				this.RemoveFile(this.FindFilenameNumber(filename));
			}
		}


		public void RemoveAll()
		{
			if (this.numEntries > 0)
			{
				for (int i = this.EndIndex - 1; i > this.StartIndex; i--)
				{
					this.MenuItems.RemoveAt(i);
				}
				this.Disable();
				this.numEntries = 0;
			}
		}


		public void RenameFile(string oldFilename, string newFilename)
		{
			string fullPath = Path.GetFullPath(newFilename);
			this.RenameFile(Path.GetFullPath(oldFilename), fullPath, MruStripMenu.ShortenPathname(fullPath, this.MaxShortenPathLength));
		}


		public void RenameFile(string oldFilename, string newFilename, string newEntryname)
		{
			if (newFilename == null)
			{
				throw new ArgumentNullException("newFilename");
			}
			if (newFilename.Length == 0)
			{
				throw new ArgumentException("newFilename");
			}
			if (this.numEntries > 0)
			{
				int num = this.FindFilenameMenuIndex(oldFilename);
				if (num >= 0)
				{
					MruStripMenu.MruMenuItem mruMenuItem = (MruStripMenu.MruMenuItem)this.MenuItems[num];
					mruMenuItem.Text = MruStripMenu.FixupEntryname(0, newEntryname);
					mruMenuItem.Filename = newFilename;
					return;
				}
			}
			this.AddFile(newFilename, newEntryname);
		}




		public string RegistryKeyName
		{
			get
			{
				return this.registryKeyName;
			}
			set
			{
				if (this.mruStripMutex != null)
				{
					this.mruStripMutex.Close();
				}
				this.registryKeyName = value.Trim();
				if (this.registryKeyName.Length == 0)
				{
					this.registryKeyName = null;
					this.mruStripMutex = null;
					return;
				}
				string name = this.registryKeyName.Replace('\\', '_').Replace('/', '_') + "Mutex";
				this.mruStripMutex = new Mutex(false, name);
			}
		}


		public void LoadFromRegistry(string keyName)
		{
			this.RegistryKeyName = keyName;
			this.LoadFromRegistry();
		}


		public void LoadFromRegistry()
		{
			if (this.registryKeyName != null)
			{
				this.mruStripMutex.WaitOne();
				this.RemoveAll();
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(this.registryKeyName);
				if (registryKey != null)
				{
					for (int i = this.maxEntries; i > 0; i--)
					{
						string text = (string)registryKey.GetValue("File" + i.ToString());
						if (text != null)
						{
							this.AddFile(text);
						}
					}
					registryKey.Close();
				}
				this.mruStripMutex.ReleaseMutex();
			}
		}


		public void SaveToRegistry(string keyName)
		{
			this.RegistryKeyName = keyName;
			this.SaveToRegistry();
		}


		public void SaveToRegistry()
		{
			if (this.registryKeyName != null)
			{
				this.mruStripMutex.WaitOne();
				RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(this.registryKeyName);
				if (registryKey != null)
				{
					registryKey.SetValue("max", this.maxEntries);
					int i = 1;
					int j = this.StartIndex;
					while (j < this.EndIndex)
					{
						registryKey.SetValue("File" + i.ToString(), ((MruStripMenu.MruMenuItem)this.MenuItems[j]).Filename);
						j++;
						i++;
					}
					while (i <= 16)
					{
						registryKey.DeleteValue("File" + i.ToString(), false);
						i++;
					}
					registryKey.Close();
				}
				this.mruStripMutex.ReleaseMutex();
			}
		}


		private MruStripMenu.ClickedHandler clickedHandler;


		protected ToolStripMenuItem recentFileMenuItem;


		protected string registryKeyName;


		protected int numEntries;


		protected int maxEntries = 4;


		protected int maxShortenPathLength = 96;


		protected Mutex mruStripMutex;


		public class MruMenuItem : ToolStripMenuItem
		{

			public MruMenuItem()
			{
				base.Tag = "";
			}


			public MruMenuItem(string filename, string entryname, EventHandler eventHandler)
			{
				base.Tag = filename;
				this.Text = entryname;
				base.Click += eventHandler;
			}




			public string Filename
			{
				get
				{
					return (string)base.Tag;
				}
				set
				{
					base.Tag = value;
				}
			}
		}



		public delegate void ClickedHandler(int number, string filename);
	}
}
