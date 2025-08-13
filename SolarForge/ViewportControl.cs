using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SolarForge
{

	public class ViewportControl : UserControl
	{



		public ProgramModel ProgramModel { get; set; }



		public ViewportControl.HandleWndProcDelegate HandleWndProc
		{
			set
			{
				this.handleWndProc = value;
			}
		}


		public ViewportControl()
		{
			this.InitializeComponent();
		}


		protected override void WndProc(ref Message m)
		{
			bool flag = false;
			if (this.handleWndProc != null)
			{
				flag = this.handleWndProc(ref m);
			}
			if (!flag)
			{
				base.WndProc(ref m);
			}
		}


		protected override void OnMouseWheel(MouseEventArgs e)
		{
			this.ProgramModel.HandleMouseWheel(e);
		}


		protected override void OnMouseMove(MouseEventArgs e)
		{
			Point point = this.lastMouseLocation;
			int deltaX = e.Location.X - this.lastMouseLocation.X;
			int deltaY = e.Location.Y - this.lastMouseLocation.Y;
			this.ProgramModel.HandleMouseMove(deltaX, deltaY, Control.MouseButtons);
			this.lastMouseLocation = e.Location;
		}


		protected override void OnMouseClick(MouseEventArgs e)
		{
			this.ProgramModel.HandleMouseClick(e.Location, e.Button);
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
			base.SuspendLayout();
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(800, 450);
			base.Name = "ViewportForm";
			this.Text = "Viewport";
			base.ResumeLayout(false);
		}


		private ViewportControl.HandleWndProcDelegate handleWndProc;


		private Point lastMouseLocation;


		private IContainer components;



		public delegate bool HandleWndProcDelegate(ref Message m);
	}
}
