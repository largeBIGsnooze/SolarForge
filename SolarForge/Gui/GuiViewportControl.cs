using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using Solar.Gui;

namespace SolarForge.Gui
{

	public class GuiViewportControl : ScrollableControl
	{

		public GuiViewportControl()
		{
			this.InitializeComponent();
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
			this.AutoScroll = true;
			base.AutoScrollMinSize = this.RootWindowSize;
		}



		public GuiModel Model
		{
			set
			{
				this.model = value;
				this.model.SelectedComponentsChanged += this.Model_SelectedComponentsChanged;
				this.model.SelectedComponentPropertyChanged += this.Model_SelectedComponentPropertyChanged;
			}
		}


		private void Model_SelectedComponentPropertyChanged()
		{
			this.StopDragging();
			base.Invalidate();
		}


		private void Model_SelectedComponentsChanged()
		{
			this.StopDragging();
			base.Invalidate();
		}


		protected override void OnPaint(PaintEventArgs pe)
		{
			pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			this.PaintBackdrop(pe);
			this.PaintComponents(pe);
			base.OnPaint(pe);
		}


		protected override void OnSizeChanged(EventArgs e)
		{
			base.Invalidate();
			base.OnSizeChanged(e);
		}


		protected override void OnScroll(ScrollEventArgs se)
		{
			this.Refresh();
			base.OnScroll(se);
		}


		protected override void OnAutoSizeChanged(EventArgs e)
		{
			base.Invalidate();
			base.OnAutoSizeChanged(e);
		}


		private GuiViewportControl.DragNode TryGetSelectedComponentDragNodeAtMouseLocation(Point mouseLocation)
		{
			foreach (GuiViewportControl.ComponentArea componentArea in this.GetComponentAreas())
			{
				if (this.model.IsComponentSelected(componentArea.Component))
				{
					//IEnumerable<GuiViewportControl.DragNodeArea> dragNodeAreas = this.GetDragNodeAreas(componentArea);
					//Func<GuiViewportControl.DragNodeArea, bool> predicate;
					if (model.IsComponentSelected(componentArea.Component))
					{ 
						var dragNodeArea = this.GetDragNodeAreas(componentArea).FirstOrDefault(X => X.Area.Contains(mouseLocation));
						if (dragNodeArea != null)
						{
							return dragNodeArea.DragNode;
						} 
					}
				}
			}
			return null;
		}



		private bool IsDragging
		{
			get
			{
				return this.dragState != null;
			}
		}


		private void StartDragging(GuiViewportControl.DragNode dragNode, Point mouseLocation)
		{
			this.dragState = new GuiViewportControl.DragState(dragNode, mouseLocation);
			base.Invalidate();
		}


		private void StopDragging()
		{
			this.dragState = null;
			base.Invalidate();
		}


		private void MoveLeftEdge(GuiComponent component, int deltaX)
		{
			switch (component.Layout.HorizontalAlignment)
			{
			case WindowHorizontalAlignment.Center:
			{
				int width = Math.Max(0, component.Layout.Width - deltaX);
				component.Layout.Width = width;
				return;
			}
			case WindowHorizontalAlignment.Left:
			{
				int num = Math.Max(0, component.Layout.Margins.Left + deltaX);
				int num2 = component.Layout.Width + (component.Layout.Margins.Left - num);
				if (num2 < 0)
				{
					num += num2;
					num2 = 0;
				}
				component.Layout.Width = num2;
				component.Layout.Margins.Left = num;
				return;
			}
			case WindowHorizontalAlignment.Right:
			{
				int width2 = Math.Max(0, component.Layout.Width - deltaX);
				component.Layout.Width = width2;
				return;
			}
			case WindowHorizontalAlignment.Stretch:
			{
				int left = Math.Max(0, component.Layout.Margins.Left + deltaX);
				component.Layout.Margins.Left = left;
				return;
			}
			default:
				return;
			}
		}


		private void MoveRightEdge(GuiComponent component, int deltaX)
		{
			switch (component.Layout.HorizontalAlignment)
			{
			case WindowHorizontalAlignment.Center:
			{
				int width = Math.Max(0, component.Layout.Width + deltaX);
				component.Layout.Width = width;
				return;
			}
			case WindowHorizontalAlignment.Left:
			{
				int width2 = Math.Max(0, component.Layout.Width + deltaX);
				component.Layout.Width = width2;
				return;
			}
			case WindowHorizontalAlignment.Right:
			{
				int num = Math.Max(0, component.Layout.Margins.Right - deltaX);
				int num2 = component.Layout.Width + (component.Layout.Margins.Right - num);
				if (num2 < 0)
				{
					num += num2;
					num2 = 0;
				}
				component.Layout.Width = num2;
				component.Layout.Margins.Right = num;
				return;
			}
			case WindowHorizontalAlignment.Stretch:
			{
				int right = Math.Max(0, component.Layout.Margins.Right - deltaX);
				component.Layout.Margins.Right = right;
				return;
			}
			default:
				return;
			}
		}


		private void MoveTopEdge(GuiComponent component, int deltaY)
		{
			switch (component.Layout.VerticalAlignment)
			{
			case WindowVerticalAlignment.Center:
			{
				int height = Math.Max(0, component.Layout.Height - deltaY);
				component.Layout.Height = height;
				return;
			}
			case WindowVerticalAlignment.Top:
			{
				int num = Math.Max(0, component.Layout.Margins.Top + deltaY);
				int num2 = component.Layout.Height + (component.Layout.Margins.Top - num);
				if (num < 0)
				{
					num += num2;
					num2 = 0;
				}
				component.Layout.Height = num2;
				component.Layout.Margins.Top = num;
				return;
			}
			case WindowVerticalAlignment.Bottom:
			{
				int height2 = Math.Max(0, component.Layout.Height - deltaY);
				component.Layout.Height = height2;
				return;
			}
			case WindowVerticalAlignment.Stretch:
			{
				int top = Math.Max(0, component.Layout.Margins.Top + deltaY);
				component.Layout.Margins.Top = top;
				return;
			}
			default:
				return;
			}
		}


		private void MoveBottomEdge(GuiComponent component, int deltaY)
		{
			switch (component.Layout.VerticalAlignment)
			{
			case WindowVerticalAlignment.Center:
			{
				int height = Math.Max(0, component.Layout.Height + deltaY);
				component.Layout.Height = height;
				return;
			}
			case WindowVerticalAlignment.Top:
			{
				int height2 = Math.Max(0, component.Layout.Height + deltaY);
				component.Layout.Height = height2;
				return;
			}
			case WindowVerticalAlignment.Bottom:
			{
				int num = Math.Max(0, component.Layout.Margins.Bottom - deltaY);
				int num2 = component.Layout.Height + (component.Layout.Margins.Bottom - num);
				if (num2 < 0)
				{
					num += num2;
					num2 = 0;
				}
				component.Layout.Height = num2;
				component.Layout.Margins.Bottom = num;
				return;
			}
			case WindowVerticalAlignment.Stretch:
			{
				int bottom = Math.Max(0, component.Layout.Margins.Bottom - deltaY);
				component.Layout.Margins.Bottom = bottom;
				return;
			}
			default:
				return;
			}
		}


		private void MoveComponent(GuiComponent component, int deltaX, int deltaY)
		{
			switch (component.Layout.HorizontalAlignment)
			{
			case WindowHorizontalAlignment.Left:
			{
				int left = Math.Max(0, component.Layout.Margins.Left + deltaX);
				component.Layout.Margins.Left = left;
				break;
			}
			case WindowHorizontalAlignment.Right:
			{
				int right = Math.Max(0, component.Layout.Margins.Right - deltaX);
				component.Layout.Margins.Right = right;
				break;
			}
			}
			switch (component.Layout.VerticalAlignment)
			{
			case WindowVerticalAlignment.Center:
			case WindowVerticalAlignment.Stretch:
				break;
			case WindowVerticalAlignment.Top:
			{
				int top = Math.Max(0, component.Layout.Margins.Top + deltaY);
				component.Layout.Margins.Top = top;
				return;
			}
			case WindowVerticalAlignment.Bottom:
			{
				int bottom = Math.Max(0, component.Layout.Margins.Bottom - deltaY);
				component.Layout.Margins.Bottom = bottom;
				break;
			}
			default:
				return;
			}
		}


		private void UpdateDragging(Point mouseLocation)
		{
			int deltaX = mouseLocation.X - this.dragState.LastMouseLocation.X;
			int deltaY = mouseLocation.Y - this.dragState.LastMouseLocation.Y;
			foreach (GuiComponent component in this.model.SelectedComponents)
			{
				switch (this.dragState.DragNode.Anchor)
				{
				case GuiViewportControl.DragNodeAnchor.Center:
					this.MoveComponent(component, deltaX, deltaY);
					break;
				case GuiViewportControl.DragNodeAnchor.TopLeft:
					this.MoveLeftEdge(component, deltaX);
					this.MoveTopEdge(component, deltaY);
					break;
				case GuiViewportControl.DragNodeAnchor.Top:
					this.MoveTopEdge(component, deltaY);
					break;
				case GuiViewportControl.DragNodeAnchor.TopRight:
					this.MoveRightEdge(component, deltaX);
					this.MoveTopEdge(component, deltaY);
					break;
				case GuiViewportControl.DragNodeAnchor.Right:
					this.MoveRightEdge(component, deltaX);
					break;
				case GuiViewportControl.DragNodeAnchor.BottomRight:
					this.MoveRightEdge(component, deltaX);
					this.MoveBottomEdge(component, deltaY);
					break;
				case GuiViewportControl.DragNodeAnchor.Bottom:
					this.MoveBottomEdge(component, deltaY);
					break;
				case GuiViewportControl.DragNodeAnchor.BottomLeft:
					this.MoveLeftEdge(component, deltaX);
					this.MoveBottomEdge(component, deltaY);
					break;
				case GuiViewportControl.DragNodeAnchor.Left:
					this.MoveLeftEdge(component, deltaX);
					break;
				}
			}
			this.dragState.LastMouseLocation = mouseLocation;
			this.model.NotifySelectedComponentDraggingUpdated();
			this.Refresh();
		}


		protected override void OnMouseMove(MouseEventArgs e)
		{
			GuiViewportControl.DragNode rhs = null;
			if (!this.IsDragging)
			{
				rhs = this.TryGetSelectedComponentDragNodeAtMouseLocation(e.Location);
			}
			else
			{
				this.UpdateDragging(e.Location);
			}
			if (this.lastHoveredDragNode != rhs)
			{
				this.lastHoveredDragNode = rhs;
				this.Refresh();
			}
			base.OnMouseMove(e);
		}


		protected override void OnMouseDown(MouseEventArgs e)
		{
			GuiViewportControl.DragNode dragNode = this.TryGetSelectedComponentDragNodeAtMouseLocation(e.Location);
			if (dragNode != null)
			{
				this.StartDragging(dragNode, e.Location);
			}
			else
			{
				List<GuiViewportControl.ComponentArea> componentAreas = this.GetComponentAreas();
				componentAreas.Reverse();
				foreach (GuiViewportControl.ComponentArea componentArea in componentAreas)
				{
					if (componentArea.Area.Contains(e.Location))
					{
						if (Control.ModifierKeys.HasFlag(Keys.Shift))
						{
							List<GuiComponent> list = this.model.SelectedComponents.ToList<GuiComponent>();
							if (list.Contains(componentArea.Component))
							{
								list.Remove(componentArea.Component);
							}
							else
							{
								list.Add(componentArea.Component);
							}
							this.model.SetSelectedComponents(list);
							break;
						}
						this.model.SetSelectedComponents(new GuiComponent[]
						{
							componentArea.Component
						});
						break;
					}
				}
			}
			base.OnMouseDown(e);
		}


		protected override void OnMouseUp(MouseEventArgs e)
		{
			this.StopDragging();
			base.OnMouseUp(e);
		}


		private void PaintBackdrop(PaintEventArgs pe)
		{
			using (SolidBrush solidBrush = new SolidBrush(this.model.Settings.ClearColor))
			{
				pe.Graphics.FillRegion(solidBrush, new Region(base.ClientRectangle));
			}
		}


		private List<GuiViewportControl.ComponentArea> GetComponentAreas()
		{
			List<GuiViewportControl.ComponentArea> list = new List<GuiViewportControl.ComponentArea>();
			if (this.model.RootComponent != null)
			{
				this.GetComponentAreasRecursive(list, this.model.RootComponent, this.DisplayRectangle);
			}
			return list;
		}


		private void GetComponentAreasRecursive(List<GuiViewportControl.ComponentArea> componentAreas, GuiComponent component, Rectangle parentArea)
		{
			Rectangle area = component.Layout.GetArea(parentArea);
			componentAreas.Add(new GuiViewportControl.ComponentArea
			{
				Component = component,
				Area = area
			});
			foreach (KeyValuePair<string, GuiComponent> keyValuePair in component.Children)
			{
				this.GetComponentAreasRecursive(componentAreas, keyValuePair.Value, area);
			}
		}


		private Rectangle GetDragNodeLocationArea(int x, int y)
		{
			return new Rectangle(x - this.DragNodeSize / 2, y - this.DragNodeSize / 2, this.DragNodeSize, this.DragNodeSize);
		}


		private List<GuiViewportControl.DragNodeArea> GetDragNodeAreas(GuiViewportControl.ComponentArea componentArea)
		{
			GuiComponent component = componentArea.Component;
			Rectangle area = componentArea.Area;
			return new List<GuiViewportControl.DragNodeArea>
			{
				new GuiViewportControl.DragNodeArea(component, GuiViewportControl.DragNodeAnchor.Center, this.GetDragNodeLocationArea(area.Left + area.Width / 2, area.Top + area.Height / 2)),
				new GuiViewportControl.DragNodeArea(component, GuiViewportControl.DragNodeAnchor.TopLeft, this.GetDragNodeLocationArea(area.Left, area.Top)),
				new GuiViewportControl.DragNodeArea(component, GuiViewportControl.DragNodeAnchor.Top, this.GetDragNodeLocationArea(area.Left + area.Width / 2, area.Top)),
				new GuiViewportControl.DragNodeArea(component, GuiViewportControl.DragNodeAnchor.TopRight, this.GetDragNodeLocationArea(area.Right, area.Top)),
				new GuiViewportControl.DragNodeArea(component, GuiViewportControl.DragNodeAnchor.Right, this.GetDragNodeLocationArea(area.Right, area.Top + area.Height / 2)),
				new GuiViewportControl.DragNodeArea(component, GuiViewportControl.DragNodeAnchor.BottomRight, this.GetDragNodeLocationArea(area.Right, area.Bottom)),
				new GuiViewportControl.DragNodeArea(component, GuiViewportControl.DragNodeAnchor.Bottom, this.GetDragNodeLocationArea(area.Left + area.Width / 2, area.Bottom)),
				new GuiViewportControl.DragNodeArea(component, GuiViewportControl.DragNodeAnchor.BottomLeft, this.GetDragNodeLocationArea(area.Left, area.Bottom)),
				new GuiViewportControl.DragNodeArea(component, GuiViewportControl.DragNodeAnchor.Left, this.GetDragNodeLocationArea(area.Left, area.Top + area.Height / 2))
			};
		}


		private void PaintComponents(PaintEventArgs pe)
		{
			List<GuiViewportControl.ComponentArea> list = new List<GuiViewportControl.ComponentArea>();
			foreach (GuiViewportControl.ComponentArea componentArea in this.GetComponentAreas())
			{
				if (!this.model.IsComponentSelected(componentArea.Component))
				{
					this.PaintComponent(pe, componentArea.Component, componentArea.Area);
				}
				else
				{
					list.Add(componentArea);
				}
			}
			foreach (GuiViewportControl.ComponentArea componentArea2 in list)
			{
				this.PaintComponent(pe, componentArea2.Component, componentArea2.Area);
				foreach (GuiViewportControl.DragNodeArea dragNodeArea in this.GetDragNodeAreas(componentArea2))
				{
					this.PaintDragNode(pe, dragNodeArea);
				}
			}
		}


		private void PaintComponent(PaintEventArgs pe, GuiComponent component, Rectangle area)
		{
			bool flag = this.model.IsComponentSelected(component);
			Color color = flag ? this.SelectedComponentBorderColor : this.ComponentBorderColor;
			float width = flag ? this.SelectedComponentBorderWidth : this.ComponentBorderWidth;
			Pen pen = new Pen(color, width);
			pe.Graphics.DrawRectangle(pen, area);
		}


		private void PaintDragNode(PaintEventArgs pe, GuiViewportControl.DragNodeArea dragNodeArea)
		{
			if (this.dragState != null && this.dragState.DragNode == dragNodeArea.DragNode)
			{
				SolidBrush brush = new SolidBrush(this.DraggingDragNodeFillColor);
				pe.Graphics.FillRectangle(brush, dragNodeArea.Area);
				Pen pen = new Pen(this.DraggingDragNodeBorderColor, 1f);
				pe.Graphics.DrawRectangle(pen, dragNodeArea.Area);
				return;
			}
			if (this.lastHoveredDragNode == dragNodeArea.DragNode)
			{
				SolidBrush brush2 = new SolidBrush(this.HoveredDragNodeFillColor);
				pe.Graphics.FillRectangle(brush2, dragNodeArea.Area);
				Pen pen2 = new Pen(this.HoveredDragNodeBorderColor, 1f);
				pe.Graphics.DrawRectangle(pen2, dragNodeArea.Area);
				return;
			}
			Pen pen3 = new Pen(this.DragNodeBorderColor, 1f);
			pen3.DashStyle = DashStyle.Dash;
			pe.Graphics.DrawRectangle(pen3, dragNodeArea.Area);
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
			this.components = new Container();
		}


		private Size RootWindowSize = new Size(1600, 1200);


		private Color DraggingDragNodeBorderColor = Color.Black;


		private Color DraggingDragNodeFillColor = Color.Green;


		private Color HoveredDragNodeBorderColor = Color.Black;


		private Color HoveredDragNodeFillColor = Color.Yellow;


		private Color DragNodeBorderColor = Color.Gray;


		private Color ComponentBorderColor = Color.Black;


		private float ComponentBorderWidth = 1f;


		private Color SelectedComponentBorderColor = Color.Goldenrod;


		private float SelectedComponentBorderWidth = 2f;


		private int DragNodeSize = 10;


		private GuiModel model;


		private GuiViewportControl.DragNode lastHoveredDragNode;


		private GuiViewportControl.DragState dragState;


		private IContainer components;


		private enum DragNodeAnchor
		{

			Center,

			TopLeft,

			Top,

			TopRight,

			Right,

			BottomRight,

			Bottom,

			BottomLeft,

			Left
		}


		private class DragNode
		{



			public GuiComponent Component { get; set; }




			public GuiViewportControl.DragNodeAnchor Anchor { get; set; }


			public DragNode(GuiComponent component, GuiViewportControl.DragNodeAnchor anchor)
			{
				this.Component = component;
				this.Anchor = anchor;
			}


			public override bool Equals(object obj)
			{
				return obj != null && base.GetType().Equals(obj.GetType()) && this.Component == ((GuiViewportControl.DragNode)obj).Component && this.Anchor == ((GuiViewportControl.DragNode)obj).Anchor;
			}


			public static bool operator ==(GuiViewportControl.DragNode lhs, GuiViewportControl.DragNode rhs)
			{
				if (lhs == null)
				{
					return rhs == null;
				}
				return lhs.Equals(rhs);
			}


			public static bool operator !=(GuiViewportControl.DragNode lhs, GuiViewportControl.DragNode rhs)
			{
				return !(lhs == rhs);
			}


			public override int GetHashCode()
			{
				return this.Component.GetHashCode() ^ this.Anchor.GetHashCode();
			}
		}


		private class DragNodeArea
		{



			public GuiViewportControl.DragNode DragNode { get; set; }




			public Rectangle Area { get; set; }


			public DragNodeArea(GuiComponent component, GuiViewportControl.DragNodeAnchor anchor, Rectangle area)
			{
				this.DragNode = new GuiViewportControl.DragNode(component, anchor);
				this.Area = area;
			}
		}


		private class ComponentArea
		{



			public GuiComponent Component { get; set; }




			public Rectangle Area { get; set; }
		}


		private class DragState
		{



			public GuiViewportControl.DragNode DragNode { get; set; }




			public Point BeginMouseLocation { get; set; }




			public Point LastMouseLocation { get; set; }


			public DragState(GuiViewportControl.DragNode dragNode, Point beginMouseLocation)
			{
				this.DragNode = dragNode;
				this.BeginMouseLocation = beginMouseLocation;
				this.LastMouseLocation = beginMouseLocation;
			}
		}
	}
}
