using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Solar.Simulations;

namespace SolarForge.Units
{

	public class UnitWeaponsEditorControl : UserControl
	{

		public UnitWeaponsEditorControl()
		{
			this.InitializeComponent();
			this.weaponInstanceListBox.DisplayMember = "Weapon";
			this.weaponInstancePropertyGrid.PropertyValueChanged += delegate(object s, PropertyValueChangedEventArgs e)
			{
				if (e.ChangedItem.Label == "Weapon")
				{
					this.RefreshWeaponInstanceListBoxDataSource(true);
				}
			};
		}



		public UnitModel Model
		{
			set
			{
				this.model = value;
				this.model.UnitDefinitionChanged += this.Model_UnitDefinitionChanged;
				this.model.SelectedWeaponInstanceChanged += this.Model_SelectedWeaponInstanceChanged;
			}
		}


		private void Model_SelectedWeaponInstanceChanged(WeaponInstanceDefinition weaponInstanceDefinition)
		{
			this.weaponInstancePropertyGrid.SelectedObject = weaponInstanceDefinition;
			this.weaponPropertyGrid.SelectedObject = this.model.TryGetWeaponDefinition(weaponInstanceDefinition);
			this.SyncWeaponToMesh.Enabled = (weaponInstanceDefinition != null);
		}


		private void RefreshWeaponInstanceListBoxDataSource(bool preserveSelection)
		{
			int selectedIndex = -1;
			if (preserveSelection)
			{
				selectedIndex = this.weaponInstanceListBox.SelectedIndex;
			}
			List<WeaponInstanceDefinition> dataSource = null;
			if (this.model.UnitDefinition != null && this.model.UnitDefinition.Weapons != null)
			{
				dataSource = this.model.UnitDefinition.Weapons.WeaponInstances;
			}
			this.weaponInstanceListBox.DataSource = null;
			this.weaponInstanceListBox.DataSource = dataSource;
			this.weaponInstanceListBox.DisplayMember = "Weapon";
			if (preserveSelection)
			{
				this.weaponInstanceListBox.SelectedIndex = selectedIndex;
			}
		}


		private void Model_UnitDefinitionChanged(UnitDefinition unitDefinition)
		{
			this.RefreshWeaponInstanceListBoxDataSource(false);
			this.SyncModelSelectedWeaponInstanceIndexToControl();
		}


		private void weaponInstanceListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.SyncModelSelectedWeaponInstanceIndexToControl();
		}


		private void SyncModelSelectedWeaponInstanceIndexToControl()
		{
			int selectedIndex = this.weaponInstanceListBox.SelectedIndex;
			this.model.SelectedWeaponInstanceIndex = ((selectedIndex != -1) ? new int?(selectedIndex) : null);
		}


		private void SyncWeaponToMeshButton_Click(object sender, EventArgs e)
		{
			this.model.SyncSelectedWeaponToMesh();
		}


		private void SyncAllWeaponsToMeshButton_Click(object sender, EventArgs e)
		{
			this.model.SyncAllWeaponsToMesh();
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
			this.weaponInstanceListBox = new ListBox();
			this.weaponInstancePropertyGrid = new PropertyGrid();
			this.weaponPropertyGrid = new PropertyGrid();
			this.SyncWeaponToMesh = new Button();
			this.SyncAllWeaponsToMesh = new Button();
			base.SuspendLayout();
			this.weaponInstanceListBox.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.weaponInstanceListBox.FormattingEnabled = true;
			this.weaponInstanceListBox.Location = new Point(7, 8);
			this.weaponInstanceListBox.Name = "weaponInstanceListBox";
			this.weaponInstanceListBox.Size = new Size(361, 186);
			this.weaponInstanceListBox.TabIndex = 0;
			this.weaponInstanceListBox.SelectedIndexChanged += this.weaponInstanceListBox_SelectedIndexChanged;
			this.weaponInstancePropertyGrid.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.weaponInstancePropertyGrid.Location = new Point(7, 272);
			this.weaponInstancePropertyGrid.Name = "weaponInstancePropertyGrid";
			this.weaponInstancePropertyGrid.Size = new Size(361, 233);
			this.weaponInstancePropertyGrid.TabIndex = 1;
			this.weaponPropertyGrid.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.weaponPropertyGrid.Location = new Point(7, 511);
			this.weaponPropertyGrid.Name = "weaponPropertyGrid";
			this.weaponPropertyGrid.Size = new Size(361, 282);
			this.weaponPropertyGrid.TabIndex = 2;
			this.SyncWeaponToMesh.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.SyncWeaponToMesh.Location = new Point(7, 200);
			this.SyncWeaponToMesh.Name = "SyncWeaponToMesh";
			this.SyncWeaponToMesh.Size = new Size(361, 23);
			this.SyncWeaponToMesh.TabIndex = 3;
			this.SyncWeaponToMesh.Text = "Sync Weapon To Mesh";
			this.SyncWeaponToMesh.UseVisualStyleBackColor = true;
			this.SyncWeaponToMesh.Click += this.SyncWeaponToMeshButton_Click;
			this.SyncAllWeaponsToMesh.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.SyncAllWeaponsToMesh.Location = new Point(7, 229);
			this.SyncAllWeaponsToMesh.Name = "SyncAllWeaponsToMesh";
			this.SyncAllWeaponsToMesh.Size = new Size(361, 23);
			this.SyncAllWeaponsToMesh.TabIndex = 4;
			this.SyncAllWeaponsToMesh.Text = "Sync All Weapons To Mesh";
			this.SyncAllWeaponsToMesh.UseVisualStyleBackColor = true;
			this.SyncAllWeaponsToMesh.Click += this.SyncAllWeaponsToMeshButton_Click;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.SyncAllWeaponsToMesh);
			base.Controls.Add(this.SyncWeaponToMesh);
			base.Controls.Add(this.weaponPropertyGrid);
			base.Controls.Add(this.weaponInstancePropertyGrid);
			base.Controls.Add(this.weaponInstanceListBox);
			base.Name = "UnitWeaponsEditorControl";
			base.Size = new Size(380, 796);
			base.ResumeLayout(false);
		}


		private UnitModel model;


		private IContainer components;


		private ListBox weaponInstanceListBox;


		private PropertyGrid weaponInstancePropertyGrid;


		private PropertyGrid weaponPropertyGrid;


		private Button SyncWeaponToMesh;


		private Button SyncAllWeaponsToMesh;
	}
}
