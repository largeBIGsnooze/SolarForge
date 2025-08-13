using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SolarForge.Units
{

	public class UnitEditorControl : UserControl
	{

		public UnitEditorControl()
		{
			this.InitializeComponent();
			this.tabControl.SelectedIndexChanged += this.TabControl_SelectedIndexChanged;
			this.skinsEditorControl = new UnitSkinsEditorControl();
			this.skinsEditorControl.Dock = DockStyle.Fill;
			this.skinsTabPage.Controls.Add(this.skinsEditorControl);
			this.skinsTabPage.Tag = UnitView.Skin;
			this.weaponsEditorControl = new UnitWeaponsEditorControl();
			this.weaponsEditorControl.Dock = DockStyle.Fill;
			this.weaponsTabPage.Controls.Add(this.weaponsEditorControl);
			this.weaponsTabPage.Tag = UnitView.Weapons;
			this.carrierEditorControl = new UnitCarrierEditorControl();
			this.carrierEditorControl.Dock = DockStyle.Fill;
			this.carrierTabPage.Controls.Add(this.carrierEditorControl);
			this.carrierTabPage.Tag = UnitView.Carrier;
			this.unitFactoryEditorControl = new UnitFactoryEditorControl();
			this.unitFactoryEditorControl.Dock = DockStyle.Fill;
			this.unitFactoryTabPage.Controls.Add(this.unitFactoryEditorControl);
			this.unitFactoryTabPage.Tag = UnitView.UnitFactory;
			this.spatialEditorControl = new UnitSpatialEditorControl();
			this.spatialEditorControl.Dock = DockStyle.Fill;
			this.spatialTabPage.Controls.Add(this.spatialEditorControl);
			this.spatialTabPage.Tag = UnitView.Spatial;
			this.debrisEditorControl = new UnitDebrisEditorControl();
			this.debrisEditorControl.Dock = DockStyle.Fill;
			this.debrisTabPage.Controls.Add(this.debrisEditorControl);
			this.debrisTabPage.Tag = UnitView.Debris;
		}


		private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.SyncModelToSelectedTab();
		}


		private void SyncModelToSelectedTab()
		{
			this.model.SelectedUnitView = (UnitView)this.tabControl.SelectedTab.Tag;
		}



		public UnitModel Model
		{
			set
			{
				this.model = value;
				this.skinsEditorControl.Model = value;
				this.weaponsEditorControl.Model = value;
				this.carrierEditorControl.Model = value;
				this.unitFactoryEditorControl.Model = value;
				this.spatialEditorControl.Model = value;
				this.debrisEditorControl.Model = value;
				this.SyncModelToSelectedTab();
			}
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
			this.spatialTabPage = new TabPage();
			this.unitFactoryTabPage = new TabPage();
			this.carrierTabPage = new TabPage();
			this.weaponsTabPage = new TabPage();
			this.skinsTabPage = new TabPage();
			this.tabControl = new TabControl();
			this.debrisTabPage = new TabPage();
			this.tabControl.SuspendLayout();
			base.SuspendLayout();
			this.spatialTabPage.Location = new Point(4, 29);
			this.spatialTabPage.Name = "spatialTabPage";
			this.spatialTabPage.Size = new Size(830, 1027);
			this.spatialTabPage.TabIndex = 3;
			this.spatialTabPage.Text = "Spatial";
			this.spatialTabPage.UseVisualStyleBackColor = true;
			this.unitFactoryTabPage.Location = new Point(4, 29);
			this.unitFactoryTabPage.Name = "unitFactoryTabPage";
			this.unitFactoryTabPage.Size = new Size(830, 1027);
			this.unitFactoryTabPage.TabIndex = 4;
			this.unitFactoryTabPage.Text = "Factory";
			this.unitFactoryTabPage.UseVisualStyleBackColor = true;
			this.carrierTabPage.Location = new Point(4, 29);
			this.carrierTabPage.Name = "carrierTabPage";
			this.carrierTabPage.Size = new Size(830, 1027);
			this.carrierTabPage.TabIndex = 2;
			this.carrierTabPage.Text = "Carrier";
			this.carrierTabPage.UseVisualStyleBackColor = true;
			this.weaponsTabPage.Location = new Point(4, 29);
			this.weaponsTabPage.Margin = new Padding(4, 5, 4, 5);
			this.weaponsTabPage.Name = "weaponsTabPage";
			this.weaponsTabPage.Padding = new Padding(4, 5, 4, 5);
			this.weaponsTabPage.Size = new Size(830, 1027);
			this.weaponsTabPage.TabIndex = 1;
			this.weaponsTabPage.Text = "Weapons";
			this.weaponsTabPage.UseVisualStyleBackColor = true;
			this.skinsTabPage.Location = new Point(4, 29);
			this.skinsTabPage.Margin = new Padding(4, 5, 4, 5);
			this.skinsTabPage.Name = "skinsTabPage";
			this.skinsTabPage.Padding = new Padding(4, 5, 4, 5);
			this.skinsTabPage.Size = new Size(830, 1027);
			this.skinsTabPage.TabIndex = 0;
			this.skinsTabPage.Text = "Skins";
			this.skinsTabPage.UseVisualStyleBackColor = true;
			this.tabControl.Controls.Add(this.skinsTabPage);
			this.tabControl.Controls.Add(this.weaponsTabPage);
			this.tabControl.Controls.Add(this.carrierTabPage);
			this.tabControl.Controls.Add(this.unitFactoryTabPage);
			this.tabControl.Controls.Add(this.spatialTabPage);
			this.tabControl.Controls.Add(this.debrisTabPage);
			this.tabControl.Dock = DockStyle.Fill;
			this.tabControl.Location = new Point(0, 0);
			this.tabControl.Margin = new Padding(4, 5, 4, 5);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new Size(838, 1060);
			this.tabControl.TabIndex = 0;
			this.debrisTabPage.Location = new Point(4, 29);
			this.debrisTabPage.Name = "debrisTabPage";
			this.debrisTabPage.Size = new Size(830, 1027);
			this.debrisTabPage.TabIndex = 5;
			this.debrisTabPage.Text = "Debris";
			this.debrisTabPage.UseVisualStyleBackColor = true;
			base.AutoScaleDimensions = new SizeF(9f, 20f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.tabControl);
			base.Margin = new Padding(4, 5, 4, 5);
			base.Name = "UnitEditorControl";
			base.Size = new Size(838, 1060);
			this.tabControl.ResumeLayout(false);
			base.ResumeLayout(false);
		}


		private UnitModel model;


		private UnitSkinsEditorControl skinsEditorControl;


		private UnitWeaponsEditorControl weaponsEditorControl;


		private UnitCarrierEditorControl carrierEditorControl;


		private UnitFactoryEditorControl unitFactoryEditorControl;


		private UnitSpatialEditorControl spatialEditorControl;


		private UnitDebrisEditorControl debrisEditorControl;


		private IContainer components;


		private TabPage spatialTabPage;


		private TabPage unitFactoryTabPage;


		private TabPage carrierTabPage;


		private TabPage weaponsTabPage;


		private TabPage skinsTabPage;


		private TabControl tabControl;


		private TabPage debrisTabPage;
	}
}
