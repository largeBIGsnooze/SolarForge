using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Solar.Rendering;

namespace SolarForge.Brushes
{

	public class BrushEditorControl : UserControl
	{

		public BrushEditorControl()
		{
			this.InitializeComponent();
		}



		public BrushModel Model
		{
			set
			{
				this.model = value;
				this.model.BrushChanged += this.Model_BrushChanged;
			}
		}


		private void Model_BrushChanged(Solar.Rendering.Brush brush)
		{
			this.brushPropertyGrid.SelectedObject = brush;
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
			this.brushPropertyGrid = new PropertyGrid();
			base.SuspendLayout();
			this.brushPropertyGrid.Dock = DockStyle.Fill;
			this.brushPropertyGrid.Location = new Point(0, 0);
			this.brushPropertyGrid.Margin = new Padding(4, 5, 4, 5);
			this.brushPropertyGrid.Name = "brushPropertyGrid";
			this.brushPropertyGrid.PropertySort = PropertySort.Alphabetical;
			this.brushPropertyGrid.Size = new Size(498, 635);
			this.brushPropertyGrid.TabIndex = 1;
			this.brushPropertyGrid.ToolbarVisible = false;
			base.AutoScaleDimensions = new SizeF(9f, 20f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.brushPropertyGrid);
			base.Margin = new Padding(4, 5, 4, 5);
			base.Name = "BrushEditorControl";
			base.Size = new Size(498, 635);
			base.ResumeLayout(false);
		}


		private BrushModel model;


		private IContainer components;


		private PropertyGrid brushPropertyGrid;
	}
}
