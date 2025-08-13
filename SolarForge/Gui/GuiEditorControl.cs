using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SolarForge.Gui
{

	public class GuiEditorControl : UserControl
	{

		public GuiEditorControl()
		{
			this.InitializeComponent();
			this.componentsListControl = new GuiComponentsListControl();
			this.componentsListControl.Visible = true;
			this.componentsListControl.Dock = DockStyle.Fill;
			this.splitContainer.Panel1.Controls.Add(this.componentsListControl);
			this.propertyGrid.PropertyValueChanged += this.PropertyGrid_PropertyValueChanged;
		}


		private void PropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
		{
			this.model.NotifySelectedComponentPropertyChanged();
		}



		public GuiModel Model
		{
			set
			{
				this.model = value;
				this.model.SelectedComponentsChanged += this.Model_SelectedComponentChanged;
				this.model.SelectedComponentDraggingUpdated += this.Model_SelectedComponentDraggingUpdated;
				this.componentsListControl.Model = value;
			}
		}


		private void Model_SelectedComponentDraggingUpdated()
		{
			this.propertyGrid.Refresh();
		}


		private void Model_SelectedComponentChanged()
		{
			this.propertyGrid.SelectedObject = this.model.SelectedComponents.FirstOrDefault<GuiComponent>();
			this.propertyGrid.ExpandAllGridItems();
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
			this.splitContainer = new SplitContainer();
			this.propertyGrid = new PropertyGrid();
			((ISupportInitialize)this.splitContainer).BeginInit();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			base.SuspendLayout();
			this.splitContainer.Dock = DockStyle.Fill;
			this.splitContainer.Location = new Point(0, 0);
			this.splitContainer.Name = "splitContainer";
			this.splitContainer.Orientation = Orientation.Horizontal;
			this.splitContainer.Panel2.Controls.Add(this.propertyGrid);
			this.splitContainer.Size = new Size(420, 466);
			this.splitContainer.SplitterDistance = 140;
			this.splitContainer.TabIndex = 0;
			this.propertyGrid.Dock = DockStyle.Fill;
			this.propertyGrid.Location = new Point(0, 0);
			this.propertyGrid.Name = "propertyGrid";
			this.propertyGrid.PropertySort = PropertySort.Alphabetical;
			this.propertyGrid.Size = new Size(420, 322);
			this.propertyGrid.TabIndex = 0;
			this.propertyGrid.ToolbarVisible = false;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.splitContainer);
			base.Name = "GuiEditorControl";
			base.Size = new Size(420, 466);
			this.splitContainer.Panel2.ResumeLayout(false);
			((ISupportInitialize)this.splitContainer).EndInit();
			this.splitContainer.ResumeLayout(false);
			base.ResumeLayout(false);
		}


		private GuiModel model;


		private GuiComponentsListControl componentsListControl;


		private IContainer components;


		private SplitContainer splitContainer;


		private PropertyGrid propertyGrid;
	}
}
