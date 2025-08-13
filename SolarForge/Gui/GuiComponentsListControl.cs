using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SolarForge.Gui
{

	public class GuiComponentsListControl : UserControl
	{

		public GuiComponentsListControl()
		{
			this.InitializeComponent();
			this.listBox.DisplayMember = "NameInListBox";
			this.listBox.SelectedIndexChanged += this.ListBox_SelectedIndexChanged;
		}


		private void ListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!this.ignoreListBoxSelectedIndexChanged)
			{
				List<GuiComponent> list = new List<GuiComponent>();
				foreach (object obj in this.listBox.SelectedItems)
				{
					list.Add((GuiComponent)obj);
				}
				this.ignoreModelSelectedComponentsChanged = true;
				this.model.SetSelectedComponents(list);
				this.ignoreModelSelectedComponentsChanged = false;
			}
		}



		public GuiModel Model
		{
			set
			{
				this.model = value;
				this.model.RootComponentChanged += this.Model_RootComponentChanged;
				this.model.SelectedComponentsChanged += this.Model_SelectedComponentsChanged;
			}
		}


		private void Model_RootComponentChanged()
		{
			this.SyncListBoxItemsToModel();
		}


		private void SyncListBoxItemsToModel()
		{
			this.listBox.Items.Clear();
			this.AddComponentItemsRecursive(this.model.RootComponent);
		}


		private void AddComponentItemsRecursive(GuiComponent component)
		{
			this.listBox.Items.Add(component);
			foreach (GuiComponent component2 in component.Children.Values)
			{
				this.AddComponentItemsRecursive(component2);
			}
		}


		private void Model_SelectedComponentsChanged()
		{
			if (!this.ignoreModelSelectedComponentsChanged)
			{
				this.ignoreListBoxSelectedIndexChanged = true;
				for (int i = 0; i < this.listBox.Items.Count; i++)
				{
					this.listBox.SetSelected(i, this.model.IsComponentSelected((GuiComponent)this.listBox.Items[i]));
				}
				this.ignoreListBoxSelectedIndexChanged = false;
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
            this.listBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listBox
            // 
            this.listBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox.FormattingEnabled = true;
            this.listBox.Location = new System.Drawing.Point(0, 0);
            this.listBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.listBox.Name = "listBox";
            this.listBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox.Size = new System.Drawing.Size(323, 365);
            this.listBox.TabIndex = 0;
            this.listBox.SelectedIndexChanged += new System.EventHandler(this.listBox_SelectedIndexChanged_1);
            // 
            // GuiComponentsListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listBox);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "GuiComponentsListControl";
            this.Size = new System.Drawing.Size(323, 365);
            this.ResumeLayout(false);

		}


		private GuiModel model;


		private bool ignoreModelSelectedComponentsChanged;


		private bool ignoreListBoxSelectedIndexChanged;


		private IContainer components;


		private ListBox listBox;

        private void listBox_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }
    }
}
