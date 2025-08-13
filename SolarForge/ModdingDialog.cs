using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Solar.Modding;

namespace SolarForge
{

	public partial class ModdingDialog : Form
	{

		public ModdingDialog()
		{
			this.InitializeComponent();
			this.availableModsListBox.SelectedValueChanged += this.AvailableModsListBox_SelectedValueChanged;
			this.enabledModsListBox.SelectedValueChanged += this.EnabledModsListBox_SelectedValueChanged;
		}



		private Mod SelectedAvailableMod
		{
			get
			{
				return (Mod)this.availableModsListBox.SelectedItem;
			}
		}



		private Mod SelectedEnabledMod
		{
			get
			{
				return (Mod)this.enabledModsListBox.SelectedItem;
			}
		}


		private void EnabledModsListBox_SelectedValueChanged(object sender, EventArgs e)
		{
			this.SyncUiStateToSelection();
		}


		private void AvailableModsListBox_SelectedValueChanged(object sender, EventArgs e)
		{
			this.SyncUiStateToSelection();
		}



		public ModdingSystem ModdingSystem
		{
			set
			{
				this.moddingSystem = value;
				this.SyncContentsToModdingSystem();
			}
		}


		private static void SyncListBoxToMods(ListBox listBox, IEnumerable<Mod> mods)
		{
			Mod objB = (Mod)listBox.SelectedItem;
			listBox.Items.Clear();
			foreach (Mod item in mods)
			{
				listBox.Items.Add(item);
			}
			foreach (Mod mod in listBox.Items.Cast<Mod>())
			{
				if (object.Equals(mod, objB))
				{
					listBox.SelectedItem = mod;
					break;
				}
			}
		}


		private void SyncContentsToModdingSystem()
		{
			this.applyChangesButton.Enabled = this.moddingSystem.HasChangesToApply;
			this.modsPathTextBox.Text = this.moddingSystem.RootPath;
			ModdingDialog.SyncListBoxToMods(this.availableModsListBox, this.moddingSystem.DiscoveredMods);
			ModdingDialog.SyncListBoxToMods(this.enabledModsListBox, this.moddingSystem.EnabledMods);
			this.SyncUiStateToSelection();
		}


		private void SyncUiStateToSelection()
		{
			this.availableModPropertyGrid.SelectedObject = this.SelectedAvailableMod;
			this.enabledModPropertyGrid.SelectedObject = this.SelectedEnabledMod;
			this.enableModButton.Enabled = this.moddingSystem.CanEnableMod(this.SelectedAvailableMod);
			this.disableModButton.Enabled = this.moddingSystem.CanDisableMod(this.SelectedEnabledMod);
			this.moveEnabledModUpButton.Enabled = this.moddingSystem.CanMoveEnabledModUp(this.SelectedEnabledMod);
			this.moveEnabledModDownButton.Enabled = this.moddingSystem.CanMoveEnabledModDown(this.SelectedEnabledMod);
		}


		private void closeButton_Click(object sender, EventArgs e)
		{
			base.Close();
		}


		protected override void OnKeyUp(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				base.Close();
				return;
			}
			base.OnKeyUp(e);
		}


		private void browseRootFolderButton_Click(object sender, EventArgs e)
		{
			Process.Start("explorer.exe", this.moddingSystem.RootPath);
		}


		private void enableModButton_Click(object sender, EventArgs e)
		{
			this.moddingSystem.EnableMod(this.SelectedAvailableMod);
			this.SyncContentsToModdingSystem();
		}


		private void disableModButton_Click(object sender, EventArgs e)
		{
			this.moddingSystem.DisableMod(this.SelectedEnabledMod);
			this.SyncContentsToModdingSystem();
		}


		private void moveEnabledModDownButton_Click(object sender, EventArgs e)
		{
			this.moddingSystem.MoveEnabledModDown(this.SelectedEnabledMod);
			this.SyncContentsToModdingSystem();
		}


		private void moveEnabledModUpButton_Click(object sender, EventArgs e)
		{
			this.moddingSystem.MoveEnabledModUp(this.SelectedEnabledMod);
			this.SyncContentsToModdingSystem();
		}


		private void applyChangesButton_Click(object sender, EventArgs e)
		{
			this.moddingSystem.ApplyChanges();
		}


		private ModdingSystem moddingSystem;
	}
}
