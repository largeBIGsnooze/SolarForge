using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Solar.Math;
using Solar.Scenarios;
using SolarForge.Utility;

namespace SolarForge.Scenarios
{

	public class ScenarioViewportControl : UserControl
	{

		public ScenarioViewportControl()
		{
			this.InitializeComponent();
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
			FontFamily family = new FontFamily(GenericFontFamilies.Monospace);
			this.playerIndexFont = new Font(family, 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.actionMenu = new ScenarioViewportActionMenu();
			this.nodeGraphicsManager = new NodeGraphicsManager(null);
			Task.Factory.StartNew(new Action(this.RunPanTask), TaskCreationOptions.LongRunning);
			base.LostFocus += this.ScenarioViewportControl_LostFocus;
			base.GotFocus += this.ScenarioViewportControl_GotFocus;
		}


		private void ScenarioViewportControl_GotFocus(object sender, EventArgs e)
		{
			this.canPan = true;
		}


		private void ScenarioViewportControl_LostFocus(object sender, EventArgs e)
		{
			this.canPan = false;
		}


		private double GetNextPanSpeed(double currentSpeed, int? state, double timeElapsed)
		{
			double num;
			if (state != null)
			{
				num = currentSpeed + (double)state.Value * 5.0 * timeElapsed;
				num = Math.Max(-1.0, num);
				num = Math.Min(1.0, num);
			}
			else if (Math.Abs(currentSpeed) < 0.001)
			{
				num = 0.0;
			}
			else
			{
				double num2 = Math.Abs(currentSpeed) * 10.0 * timeElapsed;
				if (currentSpeed > 0.0)
				{
					num = Math.Max(currentSpeed - num2, 0.0);
				}
				else
				{
					num = Math.Min(currentSpeed + num2, 0.0);
				}
			}
			return num;
		}


		private void RunPanTask()
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			for (;;)
			{
				double totalSeconds = stopwatch.Elapsed.TotalSeconds;
				stopwatch.Reset();
				stopwatch.Start();
				int? state = null;
				int? state2 = null;
				if (this.canPan)
				{
					try
					{
						Form activeForm = Form.ActiveForm;
						if (activeForm != null && Application.OpenForms.Count > 0 && activeForm.Equals(Application.OpenForms[0]))
						{
							if (KeyState.IsKeyDown(Keys.Left))
							{
								state = new int?(-1);
							}
							else if (KeyState.IsKeyDown(Keys.Right))
							{
								state = new int?(1);
							}
							if (KeyState.IsKeyDown(Keys.Up))
							{
								state2 = new int?(1);
							}
							else if (KeyState.IsKeyDown(Keys.Down))
							{
								state2 = new int?(-1);
							}
						}
					}
					catch (ArgumentOutOfRangeException)
					{
					}
				}
				this.panXSpeed = this.GetNextPanSpeed(this.panXSpeed, state, totalSeconds);
				this.panYSpeed = this.GetNextPanSpeed(this.panYSpeed, state2, totalSeconds);
				if (this.panXSpeed != 0.0 || this.panYSpeed != 0.0)
				{
					this.anchorPosition.X += (float)(this.panXSpeed * totalSeconds * (500.0 / (double)this.zoomLevel));
					this.anchorPosition.Y += (float)(this.panYSpeed * totalSeconds * (500.0 / (double)this.zoomLevel));
					base.Invalidate();
				}
			}
		}



		public ScenarioModel Model
		{
			set
			{
				this.model = value;
				this.model.Settings.PropertyChanged += this.Settings_PropertyChanged;
				this.model.GalaxyChartChanged += this.Model_GalaxyChartChanged;
				this.model.SelectedGalaxyChartComponentChanged += this.Model_SelectedGalaxyChartComponentChanged;
				this.model.ActionTargetGalaxyChartNodeChanged += this.Model_ActionTargetGalaxyChartNodeChanged;
				this.model.AnyGalaxyChartComponentPropertyChanged += this.Model_AnyGalaxyChartComponentPropertyChanged;
				this.actionMenu.Model = value;
				this.actionMenu.ViewportControl = this;
			}
		}


		private void Model_ActionTargetGalaxyChartNodeChanged(GalaxyChartNode node)
		{
			base.Invalidate();
		}


		private void Model_AnyGalaxyChartComponentPropertyChanged()
		{
			base.Invalidate();
		}


		private void Model_SelectedGalaxyChartComponentChanged(object selectedComponent)
		{
			base.Invalidate();
		}


		private void Model_GalaxyChartChanged(GalaxyChart galaxyChart, bool isUndo)
		{
			this.EndDragging();
			if (!isUndo)
			{
				this.ResetAnchorPositionAndZoomLevel();
			}
			base.Invalidate();
		}


		private void ResetAnchorPositionAndZoomLevel()
		{
			if (this.model.GalaxyChart != null)
			{
				FloatRect floatRect = this.model.GalaxyChart.CalculateBoundingArea();
				float val = ((double)floatRect.Size.Width > 0.0) ? ((float)base.ClientSize.Width / floatRect.Size.Width) : ((float)base.ClientSize.Width);
				float val2 = ((double)floatRect.Size.Height > 0.0) ? ((float)base.ClientSize.Height / floatRect.Size.Height) : ((float)base.ClientSize.Height);
				this.zoomLevel = Math.Min(val, val2) * 0.9f;
				this.anchorPosition = new FloatPoint(floatRect.TopLeft);
				this.anchorPosition.X -= Math.Max(0f, ((float)base.ClientSize.Width / this.zoomLevel - floatRect.Size.Width) / 2f);
				this.anchorPosition.Y -= Math.Max(0f, ((float)base.ClientSize.Height / this.zoomLevel - floatRect.Size.Height) / 2f);
			}
		}


		private void Settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.Invalidate();
		}


		public void AddNodeAt(GalaxyChartNodeFillingName fillingName, Point mouseLocation)
		{
			FloatPoint position = this.ConvertViewportLocationToGalaxyChartPosition(mouseLocation);
			GalaxyChartNode selectedGalaxyChartNode = this.model.SelectedGalaxyChartNode;
			this.model.AddNode(fillingName, position, selectedGalaxyChartNode);
		}


		protected override void OnPaint(PaintEventArgs pe)
		{
			pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			this.PaintBackdrop(pe);
			this.PaintGrid(pe);
			this.PaintPhaseLanes(pe);
			this.PaintNodesRelationships(pe);
			this.PaintNodes(pe);
			this.PaintBoundingBox(pe);
			base.OnPaint(pe);
		}


		private void PaintBackdrop(PaintEventArgs pe)
		{
			using (SolidBrush solidBrush = new SolidBrush(Color.Black))
			{
				pe.Graphics.FillRegion(solidBrush, new Region(base.ClientRectangle));
			}
		}


		private void PaintGrid(PaintEventArgs pe)
		{
			if (this.model.Settings.ShowGrid)
			{
				using (Pen pen = new Pen(Color.FromArgb(this.model.Settings.GridAlpha, this.model.Settings.GridColor), 0.5f))
				{
					float num = (float)this.model.Settings.GridSpacing;
					FloatPoint floatPoint = this.ConvertViewportLocationToGalaxyChartPosition(new Point(0, 0));
					FloatPoint floatPoint2 = this.ConvertViewportLocationToGalaxyChartPosition(new Point(base.ClientSize.Width, base.ClientSize.Height));
					float num2 = (float)(Math.Floor((double)(floatPoint.X / num)) * (double)num);
					float num3 = (float)(Math.Floor((double)(floatPoint2.Y / num)) * (double)num);
					for (float num4 = num2; num4 <= floatPoint2.X; num4 += num)
					{
						PointF pointF = this.ConvertGalaxyChartPositionToViewportLocation(new FloatPoint(num4, floatPoint.Y));
						PointF pointF2 = this.ConvertGalaxyChartPositionToViewportLocation(new FloatPoint(num4, floatPoint2.Y));
						pe.Graphics.DrawLine(pen, pointF.X, 0f, pointF2.X, (float)base.ClientSize.Height);
					}
					for (float num5 = num3; num5 <= floatPoint.Y; num5 += num)
					{
						PointF pointF3 = this.ConvertGalaxyChartPositionToViewportLocation(new FloatPoint(floatPoint.X, num5));
						PointF pointF4 = this.ConvertGalaxyChartPositionToViewportLocation(new FloatPoint(floatPoint2.X, num5));
						pe.Graphics.DrawLine(pen, 0f, pointF3.Y, (float)base.ClientSize.Width, pointF4.Y);
					}
				}
			}
		}


		private void PaintNodes(PaintEventArgs pe)
		{
			if (this.model.GalaxyChart != null)
			{
				this.PaintNodesRecursive(pe, this.model.GalaxyChart, this.model.GalaxyChart.RootNodes);
			}
		}


		private void PaintNodesRecursive(PaintEventArgs pe, GalaxyChart galaxy_chart, GalaxyChartNodeCollection nodes)
		{
			for (int i = 0; i < nodes.Count; i++)
			{
				GalaxyChartNode galaxyChartNode = nodes[i];
				this.PaintNode(pe, galaxy_chart, galaxyChartNode);
				this.PaintNodesRecursive(pe, galaxy_chart, galaxyChartNode.ChildNodes);
			}
		}


		private void PaintNodesRelationships(PaintEventArgs pe)
		{
			if (this.model.GalaxyChart != null)
			{
				this.PaintNodesRelationshipsRecursive(pe, this.model.GalaxyChart, this.model.GalaxyChart.RootNodes);
			}
		}


		private void PaintNodesRelationshipsRecursive(PaintEventArgs pe, GalaxyChart galaxy_chart, GalaxyChartNodeCollection nodes)
		{
			for (int i = 0; i < nodes.Count; i++)
			{
				GalaxyChartNode galaxyChartNode = nodes[i];
				this.PaintNodeRelationships(pe, galaxy_chart, galaxyChartNode);
				this.PaintNodesRelationshipsRecursive(pe, galaxy_chart, galaxyChartNode.ChildNodes);
			}
		}


		private void PaintBoundingBox(PaintEventArgs pe)
		{
			Color color = ColorTranslator.FromHtml("#ffe755");
			if (this.boundingBox != null)
			{
				Rectangle rect = new Rectangle(Math.Min(this.boundingBox.InitialMouseLocation.X, this.boundingBox.FinalMouseLocation.X), Math.Min(this.boundingBox.InitialMouseLocation.Y, this.boundingBox.FinalMouseLocation.Y), Math.Abs(this.boundingBox.InitialMouseLocation.X - this.boundingBox.FinalMouseLocation.X), Math.Abs(this.boundingBox.InitialMouseLocation.Y - this.boundingBox.FinalMouseLocation.Y));
				using (Pen pen = new Pen(color))
				{
					pe.Graphics.DrawRectangle(pen, rect);
				}
			}
		}


		private void BeginDraggingGalaxyChartNode(GalaxyChartNode node, Point mouseLocation)
		{
			this.dragState = new ScenarioViewportControl.DragState();
			this.dragState.Node = node;
			this.dragState.InitialMouseLocation = mouseLocation;
		}


		private void BeginCreatingBoundingBox(Point mouseLocation)
		{
			this.boundingBox = new ScenarioViewportControl.BoundingBox();
			this.boundingBox.InitialMouseLocation = mouseLocation;
		}


		private void CreateBoundingBox(Point mouseLocation)
		{
			if (this.boundingBox != null)
			{
				this.boundingBox.FinalMouseLocation = mouseLocation;
				this.CheckNewIntersections();
				base.Invalidate();
			}
		}


		private void CheckNewIntersections()
		{
			if (this.model.GalaxyChart != null)
			{
				Rectangle rectangle = new Rectangle(Math.Min(this.boundingBox.InitialMouseLocation.X, this.boundingBox.FinalMouseLocation.X), Math.Min(this.boundingBox.InitialMouseLocation.Y, this.boundingBox.FinalMouseLocation.Y), Math.Abs(this.boundingBox.InitialMouseLocation.X - this.boundingBox.FinalMouseLocation.X), Math.Abs(this.boundingBox.InitialMouseLocation.Y - this.boundingBox.FinalMouseLocation.Y));
				List<GalaxyChartNode> list = new List<GalaxyChartNode>();
				foreach (GalaxyChartNode galaxyChartNode in this.model.GalaxyChart.GetAllNodes())
				{
					PointF pointF = this.ConvertGalaxyChartPositionToViewportLocation(galaxyChartNode.Position);
					Point pt = new Point((int)pointF.X, (int)pointF.Y);
					if (rectangle.Contains(pt))
					{
						list.Add(galaxyChartNode);
					}
				}
				this.boundingBox.SelectedNodes = list.ToArray();
			}
		}


		private void FocusOnSelectedComponent()
		{
			if (this.model.SelectedGalaxyChartNode != null)
			{
				this.FocusOnPosition(this.model.SelectedGalaxyChartNode.Position);
				return;
			}
			if (this.model.SelectedGalaxyChartPhaseLane != null)
			{
				FloatPoint position = this.model.SelectedGalaxyChartPhaseLane.NodeA.Position;
				this.FocusOnPosition(position);
			}
		}


		private void FocusOnPosition(FloatPoint position)
		{
			FloatPoint b = this.ConvertViewportLocationToGalaxyChartPosition(this.ClientCenter);
			FloatPoint b2 = FloatPoint.Subtract(position, b);
			this.anchorPosition = FloatPoint.Add(this.anchorPosition, b2);
			base.Invalidate();
		}


		protected override bool IsInputKey(Keys keyData)
		{
			return keyData == Keys.Left || keyData == Keys.Right || keyData == Keys.Up || keyData == Keys.Down || base.IsInputKey(keyData);
		}


		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData != (Keys)131139)
			{
				switch (keyData)
				{
				case (Keys)131158:
					if (this.copyGroup != null)
					{
						this.PasteSelectedGroup(Cursor.Position);
						return true;
					}
					break;
				case (Keys)131161:
					this.model.TryRedo();
					return true;
				case (Keys)131162:
					this.model.TryUndo();
					return true;
				}
				return base.ProcessCmdKey(ref msg, keyData);
			}
			this.CopySelectedGroup();
			return true;
		}


		protected override void OnKeyUp(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
			{
				if (this.selectedGroup != null && this.selectedGroup.SelectedGroupOfNodes.Length != 0)
				{
					this.DeleteSelectedNodes();
					return;
				}
				this.model.TryDeleteSelectedGalaxyChartComponent();
				return;
			}
			else
			{
				if (e.KeyCode == Keys.Space)
				{
					this.FocusOnSelectedComponent();
					return;
				}
				if (e.KeyCode == Keys.Escape)
				{
					if (this.selectedGroup != null && this.selectedGroup.SelectedGroupOfNodes.Length != 0)
					{
						this.selectedGroup.SelectedGroupOfNodes = new GalaxyChartNode[0];
						return;
					}
				}
				else if (e.KeyCode == Keys.R)
				{
					if (this.selectedGroup != null && this.selectedGroup.SelectedGroupOfNodes.Length != 0)
					{
						if (!this.selectedGroup.IsRotating)
						{
							this.StartRotateSelectedGroup(Cursor.Position);
							return;
						}
						this.EndRotateSelectedGroup();
						return;
					}
				}
				else
				{
					base.OnKeyUp(e);
				}
				return;
			}
		}


		private void DeleteSelectedNodes()
		{
			foreach (GalaxyChartNode node in this.selectedGroup.SelectedGroupOfNodes)
			{
				this.model.RemoveNode(node);
			}
			this.selectedGroup.SelectedGroupOfNodes = new GalaxyChartNode[0];
			if (this.copyGroup != null && this.copyGroup.CopiedNodes.Length != 0)
			{
				for (int j = 0; j < this.copyGroup.CopiedNodes.Length; j++)
				{
					Trace.WriteLine(this.copyGroup.CopiedNodes[j].Id);
				}
			}
		}


		private void StartRotateSelectedGroup(Point cursorLocation)
		{
			if (this.selectedGroup != null && this.selectedGroup.SelectedGroupOfNodes.Length != 0)
			{
				this.selectedGroup.IsRotating = true;
				int num = int.MaxValue;
				int num2 = int.MaxValue;
				int num3 = int.MinValue;
				int num4 = int.MinValue;
				foreach (GalaxyChartNode galaxyChartNode in this.selectedGroup.SelectedGroupOfNodes)
				{
					PointF pointF = this.ConvertGalaxyChartPositionToViewportLocation(galaxyChartNode.Position);
					num = Math.Min(num, (int)pointF.X);
					num2 = Math.Min(num2, (int)pointF.Y);
					num3 = Math.Max(num3, (int)pointF.X);
					num4 = Math.Max(num4, (int)pointF.Y);
				}
				this.selectedGroup.RotationCenter = new PointF((float)(num + num3) / 2f, (float)(num2 + num4) / 2f);
				this.selectedGroup.InitialMouseLocation = cursorLocation;
				base.Invalidate();
			}
		}


		private void RotateSelectedGroup(MouseEventArgs e)
		{
			if (this.selectedGroup.IsRotating)
			{
				PointF rotationCenter = this.selectedGroup.RotationCenter;
				Point initialMouseLocation = this.selectedGroup.InitialMouseLocation;
				Point location = e.Location;
				double num = Math.Atan2((double)((float)initialMouseLocation.Y - rotationCenter.Y), (double)((float)initialMouseLocation.X - rotationCenter.X));
				double num2 = Math.Atan2((double)((float)location.Y - rotationCenter.Y), (double)((float)location.X - rotationCenter.X)) - num;
				foreach (GalaxyChartNode galaxyChartNode in this.selectedGroup.SelectedGroupOfNodes)
				{
					PointF pointF = this.ConvertGalaxyChartPositionToViewportLocation(galaxyChartNode.Position);
					float num3 = pointF.X - rotationCenter.X;
					float num4 = pointF.Y - rotationCenter.Y;
					double num5 = (double)rotationCenter.X + ((double)num3 * Math.Cos(num2) - (double)num4 * Math.Sin(num2));
					double num6 = (double)rotationCenter.Y + ((double)num3 * Math.Sin(num2) + (double)num4 * Math.Cos(num2));
					FloatPoint position = this.ConvertViewportLocationToGalaxyChartPosition(new PointF((float)num5, (float)num6));
					galaxyChartNode.Position = position;
				}
				this.selectedGroup.InitialMouseLocation = location;
				this.Refresh();
			}
		}


		private void EndRotateSelectedGroup()
		{
			if (this.selectedGroup.IsRotating)
			{
				this.selectedGroup.IsRotating = false;
				base.Invalidate();
			}
		}


		private void PasteSelectedGroup(Point cursorLocation)
		{
			Point viewportLocation = base.PointToClient(cursorLocation);
			if (this.copyGroup.CopiedNodes.Length != 0)
			{
				Dictionary<int, GalaxyChartNode> dictionary = new Dictionary<int, GalaxyChartNode>();
				FloatPoint a = this.ConvertViewportLocationToGalaxyChartPosition(viewportLocation);
				foreach (GalaxyChartNodeCopy galaxyChartNodeCopy in this.copyGroup.CopiedNodes)
				{
					FloatPoint b = FloatPoint.Subtract(galaxyChartNodeCopy.Position, this.copyGroup.CopiedNodes[0].Position);
					GalaxyChartNode parentNode = this.model.GalaxyChart.FindParentNode(galaxyChartNodeCopy.Id);
					GalaxyChartNode galaxyChartNode = this.model.GalaxyChart.AddNode(galaxyChartNodeCopy, parentNode);
					galaxyChartNode.Position = FloatPoint.Add(a, b);
					dictionary[galaxyChartNodeCopy.Id.Value] = galaxyChartNode;
				}
				foreach (KeyValuePair<int, GalaxyChartNode> keyValuePair in dictionary)
				{
					if (dictionary.ContainsKey(keyValuePair.Value.OriginalParentId.Value))
					{
						keyValuePair.Value.OriginalParentId = dictionary[keyValuePair.Value.OriginalParentId.Value].Id;
					}
				}
				if (this.copyGroup.ConnectedLanes != null && this.copyGroup.ConnectedLanes.Count > 0)
				{
					foreach (ScenarioViewportControl.PhaseLaneData phaseLaneData in this.copyGroup.ConnectedLanes)
					{
						GalaxyChartNode galaxyChartNode2;
						GalaxyChartNode galaxyChartNode3;
						if (dictionary.TryGetValue(phaseLaneData.NodeAId.Value, out galaxyChartNode2) && dictionary.TryGetValue(phaseLaneData.NodeBId.Value, out galaxyChartNode3) && galaxyChartNode2 != null && galaxyChartNode3 != null)
						{
							this.model.AddPhaseLane(galaxyChartNode2, galaxyChartNode3);
						}
					}
				}
			}
		}


		private void CopySelectedGroup()
		{
			if (this.selectedGroup != null && this.selectedGroup.SelectedGroupOfNodes.Length != 0)
			{
				this.copyGroup = new ScenarioViewportControl.CopyGroup();
				List<GalaxyChartNodeCopy> list = new List<GalaxyChartNodeCopy>();
				GalaxyChartNode[] selectedGroupOfNodes = this.selectedGroup.SelectedGroupOfNodes;
				for (int i = 0; i < selectedGroupOfNodes.Length; i++)
				{
					GalaxyChartNodeCopy item = new GalaxyChartNodeCopy(selectedGroupOfNodes[i]);
					list.Add(item);
				}
				this.copyGroup.CopiedNodes = list.ToArray();
				List<ScenarioViewportControl.PhaseLaneData> list2 = new List<ScenarioViewportControl.PhaseLaneData>();
				for (int j = 0; j < this.model.GalaxyChart.PhaseLanes.Count; j++)
				{
					GalaxyChartPhaseLane galaxyChartPhaseLane = this.model.GalaxyChart.PhaseLanes[j];
					if (galaxyChartPhaseLane != null)
					{
						GalaxyChartNodeId nodeAId = galaxyChartPhaseLane.NodeAId;
						GalaxyChartNodeId nodeBId = galaxyChartPhaseLane.NodeBId;
						if (nodeAId.HasValue() && nodeBId.HasValue() && this.selectedGroup.SelectedGroupOfNodes.Any((GalaxyChartNode node) => node.Id.Value == nodeAId.Value) && this.selectedGroup.SelectedGroupOfNodes.Any((GalaxyChartNode node) => node.Id.Value == nodeBId.Value))
						{
							list2.Add(new ScenarioViewportControl.PhaseLaneData
							{
								NodeAId = nodeAId,
								NodeBId = nodeBId
							});
						}
					}
				}
				this.copyGroup.ConnectedLanes = list2;
				return;
			}
			if (this.model.SelectedGalaxyChartNode != null)
			{
				this.copyGroup = new ScenarioViewportControl.CopyGroup();
				this.copyGroup.CopiedNodes = new GalaxyChartNodeCopy[]
				{
					new GalaxyChartNodeCopy(this.model.SelectedGalaxyChartNode)
				};
			}
		}


		private void MoveChildNodesRecursive(GalaxyChartNodeCollection nodes, FloatPoint delta)
		{
			for (int i = 0; i < nodes.Count; i++)
			{
				GalaxyChartNode galaxyChartNode = nodes[i];
				FloatPoint position = new FloatPoint(galaxyChartNode.Position.X + delta.X, galaxyChartNode.Position.Y + delta.Y);
				galaxyChartNode.Position = position;
				this.MoveChildNodesRecursive(galaxyChartNode.ChildNodes, delta);
			}
		}


		private void EndDragging()
		{
			this.dragState = null;
		}


		private void EndBoundingBox()
		{
			if (this.boundingBox != null && this.boundingBox.SelectedNodes != null && this.boundingBox.SelectedNodes.Length != 0)
			{
				this.selectedGroup = new ScenarioViewportControl.SelectedGroup();
				this.selectedGroup.SelectedGroupOfNodes = new GalaxyChartNode[this.boundingBox.SelectedNodes.Length];
				this.selectedGroup.NodeRelativePositions = new Point[this.boundingBox.SelectedNodes.Length];
				PointF pointF = this.ConvertGalaxyChartPositionToViewportLocation(this.boundingBox.SelectedNodes[0].Position);
				int num = (int)pointF.X;
				int num2 = (int)pointF.Y;
				int val = (int)pointF.X;
				int val2 = (int)pointF.Y;
				for (int i = 0; i < this.boundingBox.SelectedNodes.Length; i++)
				{
					this.selectedGroup.SelectedGroupOfNodes[i] = this.boundingBox.SelectedNodes[i];
					PointF pointF2 = this.ConvertGalaxyChartPositionToViewportLocation(this.boundingBox.SelectedNodes[i].Position);
					Point point = new Point((int)pointF2.X, (int)pointF2.Y);
					num = Math.Min(num, point.X);
					num2 = Math.Min(num2, point.Y);
					val = Math.Max(val, point.X);
					val2 = Math.Max(val2, point.Y);
				}
				for (int j = 0; j < this.boundingBox.SelectedNodes.Length; j++)
				{
					PointF pointF3 = this.ConvertGalaxyChartPositionToViewportLocation(this.boundingBox.SelectedNodes[j].Position);
					Point point2 = new Point((int)pointF3.X, (int)pointF3.Y);
					this.selectedGroup.NodeRelativePositions[j] = new Point(point2.X - num, point2.Y - num2);
				}
			}
			else if (this.selectedGroup != null)
			{
				this.selectedGroup.SelectedGroupOfNodes = new GalaxyChartNode[0];
				this.selectedGroup.NodeRelativePositions = new Point[0];
			}
			this.boundingBox = null;
		}


		private void DragSelectedGroup(MouseEventArgs e)
		{
			FloatPoint floatPoint = new FloatPoint((float)(this.dragState.InitialMouseLocation.X - e.Location.X) / this.zoomLevel, (float)(e.Location.Y - this.dragState.InitialMouseLocation.Y) / this.zoomLevel);
			foreach (GalaxyChartNode galaxyChartNode in this.selectedGroup.SelectedGroupOfNodes)
			{
				galaxyChartNode.Position = new FloatPoint(galaxyChartNode.Position.X - floatPoint.X, galaxyChartNode.Position.Y - floatPoint.Y);
			}
			this.dragState.InitialMouseLocation = e.Location;
			this.Refresh();
		}


		private RectangleF MakeRect(PointF center, int size)
		{
			return new RectangleF(center - new SizeF((float)size / 2f, (float)size / 2f), new SizeF((float)size, (float)size));
		}


		private bool IsStarNode(string fillingName)
		{
			if (string.IsNullOrEmpty(fillingName))
			{
				return false;
			}
			string text = fillingName.ToLowerInvariant();
			return text.Contains("_star") || text == "sta";
		}


		private void PaintNode(PaintEventArgs pe, GalaxyChart galaxy_chart, GalaxyChartNode node)
		{
			PointF pointF = this.ConvertGalaxyChartPositionToViewportLocation(node.Position);
			if (node.ParentDepth == 0)
			{
				SolidBrush brush = new SolidBrush(this.RootNodeDotUnderlayColor);
				RectangleF rect = this.MakeRect(pointF, 32);
				pe.Graphics.FillEllipse(brush, rect);
			}
			int num = Math.Max(10, 25 - node.ParentDepth * 5);
			GalaxyChartNodeFillingName fillingName = node.FillingName;
			string text = (fillingName != null) ? fillingName.ToString() : null;
			bool flag = this.model.Settings.UseNodeGraphics && !string.IsNullOrEmpty(text) && this.nodeGraphicsManager.HasCustomGraphic(text);
			if (!string.IsNullOrEmpty(text) && this.model.Settings.UseNodeGraphics)
			{
				this.nodeGraphicsManager.GetDebugInfo(text);
			}
			if (flag)
			{
				int size;
				if (this.IsStarNode(text))
				{
					size = Math.Max(48, num * 3);
				}
				else
				{
					size = Math.Max(28, (int)((double)num * 1.5));
				}
				Image nodeGraphic = this.nodeGraphicsManager.GetNodeGraphic(text, size);
				if (nodeGraphic != null)
				{
					RectangleF rect2 = this.MakeRect(pointF, size);
					CompositingMode compositingMode = pe.Graphics.CompositingMode;
					CompositingQuality compositingQuality = pe.Graphics.CompositingQuality;
					InterpolationMode interpolationMode = pe.Graphics.InterpolationMode;
					pe.Graphics.CompositingMode = CompositingMode.SourceOver;
					pe.Graphics.CompositingQuality = CompositingQuality.HighQuality;
					pe.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
					pe.Graphics.DrawImage(nodeGraphic, rect2);
					pe.Graphics.CompositingMode = compositingMode;
					pe.Graphics.CompositingQuality = compositingQuality;
					pe.Graphics.InterpolationMode = interpolationMode;
				}
				else if (this.model.Settings.FallbackToColoredDots)
				{
					SolidBrush solidBrush = new SolidBrush(this.model.GetGalaxyChartNodeColor(node));
					RectangleF rect3 = this.MakeRect(pointF, num);
					pe.Graphics.FillEllipse(solidBrush, rect3);
					solidBrush.Dispose();
				}
			}
			else if (this.model.Settings.FallbackToColoredDots || !this.model.Settings.UseNodeGraphics)
			{
				SolidBrush solidBrush2 = new SolidBrush(this.model.GetGalaxyChartNodeColor(node));
				RectangleF rect4 = this.MakeRect(pointF, num);
				pe.Graphics.FillEllipse(solidBrush2, rect4);
				solidBrush2.Dispose();
			}
			if (this.model.Settings.ShowNodeHelper)
			{
				this.DrawFillingNameText(pe, node, pointF, num);
			}
			if (node.OwnerPlayerIndex != null)
			{
				Color playerIndexColor = this.model.GetPlayerIndexColor(node.OwnerPlayerIndex.Value);
				Pen pen = new Pen(playerIndexColor, 2f);
				pen.DashStyle = DashStyle.Dash;
				RectangleF rect5 = this.MakeRect(pointF, num + 4);
				pe.Graphics.DrawEllipse(pen, rect5);
				GalaxyChartNodeFilling filling = node.Filling;
				if (((filling != null) ? new bool?(filling.IsPrimaryFixturePlayerHomePlanet) : null).GetValueOrDefault(false))
				{
					Pen pen2 = new Pen(playerIndexColor, 2f);
					pen2.DashStyle = DashStyle.Solid;
					RectangleF rect6 = this.MakeRect(pointF, num + 12);
					pe.Graphics.DrawEllipse(pen2, rect6);
				}
				TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
				Rectangle bounds = new Rectangle((int)rect5.Left, (int)rect5.Top - (int)rect5.Height, (int)rect5.Width, (int)rect5.Height);
				TextRenderer.DrawText(pe.Graphics, string.Format("{0}", node.OwnerPlayerIndex.Value), this.playerIndexFont, bounds, playerIndexColor, flags);
			}
			if (node.IsNPC)
			{
				Color ghostWhite = Color.GhostWhite;
				Pen pen3 = new Pen(ghostWhite, 2f);
				pen3.DashStyle = DashStyle.Dash;
				RectangleF rect7 = this.MakeRect(pointF, num + 10);
				pe.Graphics.DrawEllipse(pen3, rect7);
				TextFormatFlags flags2 = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
				Rectangle bounds2 = new Rectangle((int)rect7.Left, (int)rect7.Top - (int)rect7.Height, (int)rect7.Width, (int)rect7.Height);
				TextRenderer.DrawText(pe.Graphics, "NPC", this.playerIndexFont, bounds2, ghostWhite, flags2);
			}
			if (this.selectedGroup != null && this.selectedGroup.SelectedGroupOfNodes != null && this.selectedGroup.SelectedGroupOfNodes.Contains(node))
			{
				Pen pen4 = new Pen(this.SelectionRingColor, 3f);
				pe.Graphics.DrawEllipse(pen4, this.MakeRect(pointF, 42));
			}
			if (object.Equals(this.model.SelectedGalaxyChartComponent, node))
			{
				Pen pen5 = new Pen(this.SelectionRingColor, 3f);
				pe.Graphics.DrawEllipse(pen5, this.MakeRect(pointF, 32));
			}
			if (object.Equals(this.model.ActionTargetGalaxyChartNode, node))
			{
				Pen pen6 = new Pen(this.ActionTargetRingColor, 3f);
				pe.Graphics.DrawEllipse(pen6, this.MakeRect(pointF, 32));
			}
		}


		private void DrawFillingNameText(PaintEventArgs pe, GalaxyChartNode node, PointF nodeCenter, int dotSize)
		{
			Font font = new Font(new FontFamily(GenericFontFamilies.Monospace), 11f, FontStyle.Bold, GraphicsUnit.Pixel);
			string editorVisualTag = node.Filling.EditorVisualTag;
			Color editorColor = node.Filling.EditorColor;
			TextFormatFlags flags = TextFormatFlags.HorizontalCenter;
			Rectangle bounds = new Rectangle((int)nodeCenter.X - 35, (int)nodeCenter.Y + dotSize / 2, 75, 20);
			TextRenderer.DrawText(pe.Graphics, editorVisualTag, font, bounds, editorColor, flags);
		}


		private void PaintRelationship(PaintEventArgs pe, PointF pointA, PointF pointB, bool isSelected, Color lineColor, Color selectedLineColor, int offsetSign)
		{
			int num = 1;
			Color color = lineColor;
			if (isSelected)
			{
				num = 3;
				color = selectedLineColor;
			}
			Pen pen = new Pen(color, (float)num);
			int num2 = 25 * offsetSign;
			int num3 = -Math.Sign((pointA.Y - pointB.Y) / Math.Max(pointA.X - pointB.X, 0.01f));
			PointF pointF = new PointF((pointA.X + pointB.X) / 2f, (pointA.Y + pointB.Y) / 2f);
			PointF pointF2 = new PointF(pointF.X + (float)(num2 * num3), pointF.Y + (float)num2);
			PointF pt = pointF2;
			pe.Graphics.DrawBezier(pen, pointA, pointF2, pt, pointB);
		}


		private void PaintNodeRelationships(PaintEventArgs pe, GalaxyChart galaxy_chart, GalaxyChartNode node)
		{
			PointF pointF = this.ConvertGalaxyChartPositionToViewportLocation(node.Position);
			if (node.OriginalParentId.HasValue())
			{
				GalaxyChartNode galaxyChartNode = galaxy_chart.FindNode(node.OriginalParentId);
				if (galaxyChartNode != null)
				{
					PointF pointF2 = this.ConvertGalaxyChartPositionToViewportLocation(galaxyChartNode.Position);
					PointF pointA = pointF;
					PointF pointB = pointF2;
					bool isSelected = object.Equals(this.model.SelectedGalaxyChartComponent, node);
					this.PaintRelationship(pe, pointA, pointB, isSelected, Color.LightGreen, Color.LimeGreen, 1);
				}
			}
			GalaxyChartNodeId parentId = node.ParentId;
			if (parentId.HasValue())
			{
				GalaxyChartNode galaxyChartNode2 = galaxy_chart.FindNode(parentId);
				if (galaxyChartNode2 != null && galaxyChartNode2.ParentId.HasValue())
				{
					PointF pointF3 = this.ConvertGalaxyChartPositionToViewportLocation(galaxyChartNode2.Position);
					PointF pointA2 = pointF;
					PointF pointB2 = pointF3;
					bool isSelected2 = object.Equals(this.model.SelectedGalaxyChartComponent, node);
					this.PaintRelationship(pe, pointA2, pointB2, isSelected2, Color.DeepSkyBlue, Color.RoyalBlue, -1);
				}
			}
		}


		private void PaintPhaseLanes(PaintEventArgs pe)
		{
			if (this.model.GalaxyChart != null)
			{
				for (int i = 0; i < this.model.GalaxyChart.PhaseLanes.Count; i++)
				{
					this.PaintPhaseLane(pe, this.model.GalaxyChart.PhaseLanes[i]);
				}
			}
		}


		private void PaintPhaseLane(PaintEventArgs pe, GalaxyChartPhaseLane phaseLane)
		{
			PointF pt = this.ConvertGalaxyChartPositionToViewportLocation(phaseLane.NodeA.Position);
			PointF pt2 = this.ConvertGalaxyChartPositionToViewportLocation(phaseLane.NodeB.Position);
			int num = 2;
			Color color = Color.LightGray;
			if (phaseLane.Type == GalaxyChartPhaseLaneType.Star)
			{
				color = Color.Orange;
			}
			else if (phaseLane.Type == GalaxyChartPhaseLaneType.Wormhole)
			{
				color = Color.MediumPurple;
			}
			if (object.Equals(this.model.SelectedGalaxyChartComponent, phaseLane))
			{
				num = 4;
				color = Color.White;
			}
			Pen pen = new Pen(color, (float)num);
			pe.Graphics.DrawLine(pen, pt, pt2);
		}



		private Point ClientCenter
		{
			get
			{
				return new Point(base.ClientRectangle.Left + base.ClientRectangle.Width / 2, base.ClientRectangle.Top + base.ClientRectangle.Height / 2);
			}
		}


		protected override void OnMouseWheel(MouseEventArgs e)
		{
			if (this.model.GalaxyChart != null)
			{
				if (e.Delta > 0)
				{
					FloatPoint a = this.ConvertViewportLocationToGalaxyChartPosition(e.Location);
					this.Zoom(e.Delta);
					FloatPoint b = this.ConvertViewportLocationToGalaxyChartPosition(e.Location);
					FloatPoint b2 = FloatPoint.Subtract(a, b);
					this.anchorPosition = FloatPoint.Add(this.anchorPosition, b2);
				}
				else
				{
					FloatPoint a2 = this.ConvertViewportLocationToGalaxyChartPosition(this.ClientCenter);
					this.Zoom(e.Delta);
					FloatPoint b3 = this.ConvertViewportLocationToGalaxyChartPosition(this.ClientCenter);
					FloatPoint b4 = FloatPoint.Subtract(a2, b3);
					this.anchorPosition = FloatPoint.Add(this.anchorPosition, b4);
				}
				this.Refresh();
			}
		}


		private void Zoom(int delta)
		{
			for (int i = 0; i < Math.Abs(delta / 30); i++)
			{
				this.zoomLevel *= ((delta < 0) ? 0.95f : 1.0526316f);
				this.zoomLevel = Math.Max(0.01f, this.zoomLevel);
				this.zoomLevel = Math.Min(100f, this.zoomLevel);
			}
		}


		protected override void OnAutoSizeChanged(EventArgs e)
		{
			base.Invalidate();
			base.OnAutoSizeChanged(e);
		}


		private static Point ConvertPointFToPoint(PointF pf)
		{
			return new Point((int)pf.X, (int)pf.Y);
		}


		private GalaxyChartPhaseLane FindGalaxyChartPhaseLaneAtMouseLocation(Point location)
		{
			if (this.model.GalaxyChart != null)
			{
				for (int i = 0; i < this.model.GalaxyChart.PhaseLanes.Count; i++)
				{
					GalaxyChartPhaseLane galaxyChartPhaseLane = this.model.GalaxyChart.PhaseLanes[i];
					PointF pf = this.ConvertGalaxyChartPositionToViewportLocation(galaxyChartPhaseLane.NodeA.Position);
					PointF pf2 = this.ConvertGalaxyChartPositionToViewportLocation(galaxyChartPhaseLane.NodeB.Position);
					if (this.DoesLineContainPoint(ScenarioViewportControl.ConvertPointFToPoint(pf), ScenarioViewportControl.ConvertPointFToPoint(pf2), location))
					{
						return galaxyChartPhaseLane;
					}
				}
			}
			return null;
		}


		private Point GetPointOnLine(Point endPointA, Point endPointB, float percentage)
		{
			return new Point(endPointA.X + (int)((float)(endPointB.X - endPointA.X) * percentage), endPointA.Y + (int)((float)(endPointB.Y - endPointA.Y) * percentage));
		}


		private bool DoesLineContainPoint(Point endPointA, Point endPointB, Point point)
		{
			bool result = false;
			int num = Math.Max(Math.Abs(endPointA.X - endPointB.X), Math.Abs(endPointA.Y - endPointB.Y));
			if (num > 0)
			{
				int num2 = 6;
				float num3 = (float)num2 / (float)num;
				for (float num4 = 0f; num4 <= 1f; num4 += num3)
				{
					Point pointOnLine = this.GetPointOnLine(endPointA, endPointB, num4);
					Rectangle rectangle = new Rectangle(pointOnLine - new Size(num2 / 2, num2 / 2), new Size(num2, num2));
					if (rectangle.Contains(point))
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}


		private static double GetDistance(double x1, double y1, double x2, double y2)
		{
			return Math.Sqrt(Math.Pow(x2 - x1, 2.0) + Math.Pow(y2 - y1, 2.0));
		}


		private GalaxyChartNode FindGalaxyChartNodeAtMouseLocation(Point location)
		{
			double? num = null;
			GalaxyChartNode result = null;
			if (this.model.GalaxyChart != null)
			{
				foreach (GalaxyChartNode galaxyChartNode in this.model.GalaxyChart.GetAllNodes())
				{
					PointF center = this.ConvertGalaxyChartPositionToViewportLocation(galaxyChartNode.Position);
					if (this.MakeRect(center, 32).Contains(location))
					{
						double distance = ScenarioViewportControl.GetDistance((double)location.X, (double)location.Y, (double)center.X, (double)center.Y);
						if (num == null || distance < num.Value)
						{
							result = galaxyChartNode;
							num = new double?(distance);
						}
					}
				}
			}
			return result;
		}


		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				GalaxyChartNode galaxyChartNode = this.FindGalaxyChartNodeAtMouseLocation(e.Location);
				if (Control.ModifierKeys == Keys.Control)
				{
					if (this.model.SelectedGalaxyChartNode != null && galaxyChartNode != null && this.model.SelectedGalaxyChartNode != galaxyChartNode)
					{
						this.model.AddPhaseLane(this.model.SelectedGalaxyChartNode, galaxyChartNode);
						this.model.SelectedGalaxyChartComponent = galaxyChartNode;
						return;
					}
				}
				else if (this.selectedGroup != null && this.selectedGroup.SelectedGroupOfNodes.Length != 0)
				{
					if (this.selectedGroup.SelectedGroupOfNodes.Contains(galaxyChartNode))
					{
						this.BeginDraggingSelectedNodes(e.Location);
						return;
					}
					this.BeginDraggingGalaxyChartNode(galaxyChartNode, e.Location);
					return;
				}
				else
				{
					if (galaxyChartNode != null)
					{
						this.BeginDraggingGalaxyChartNode(galaxyChartNode, e.Location);
						return;
					}
					this.BeginCreatingBoundingBox(base.PointToClient(Cursor.Position));
				}
			}
		}


		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				if (this.dragState != null)
				{
					if (!this.dragState.IsDraggingActive)
					{
						if (this.dragState.Node != null)
						{
							this.model.SelectedGalaxyChartComponent = this.dragState.Node;
						}
						else
						{
							GalaxyChartPhaseLane galaxyChartPhaseLane = this.FindGalaxyChartPhaseLaneAtMouseLocation(e.Location);
							GalaxyChartNode galaxyChartNode = this.FindGalaxyChartNodeAtMouseLocation(e.Location);
							if (this.selectedGroup != null && this.selectedGroup.SelectedGroupOfNodes.Length != 0)
							{
								if (galaxyChartNode == null || !this.selectedGroup.SelectedGroupOfNodes.Contains(galaxyChartNode))
								{
									this.selectedGroup.SelectedGroupOfNodes = new GalaxyChartNode[0];
									this.model.SelectedGalaxyChartComponent = null;
									base.Invalidate();
								}
								else
								{
									this.model.SelectedGalaxyChartComponent = galaxyChartNode;
								}
							}
							else if (galaxyChartPhaseLane != null)
							{
								this.model.SelectedGalaxyChartComponent = galaxyChartPhaseLane;
							}
							else if (galaxyChartNode != null)
							{
								this.model.SelectedGalaxyChartComponent = galaxyChartNode;
							}
							else
							{
								this.model.SelectedGalaxyChartComponent = null;
							}
						}
					}
				}
				else
				{
					this.model.SelectedGalaxyChartComponent = this.FindGalaxyChartPhaseLaneAtMouseLocation(e.Location);
				}
				this.EndDragging();
				if (this.boundingBox != null)
				{
					this.EndBoundingBox();
				}
				this.model.AddToUndoStack();
				return;
			}
			if (e.Button == MouseButtons.Right)
			{
				this.actionMenu.ShowMenu(this, e.Location, this.FindGalaxyChartNodeAtMouseLocation(e.Location), this.FindGalaxyChartPhaseLaneAtMouseLocation(e.Location));
			}
		}


		private void BeginDraggingSelectedNodes(Point mouseLocation)
		{
			this.dragState = new ScenarioViewportControl.DragState
			{
				Node = null,
				InitialMouseLocation = mouseLocation,
				IsDraggingActive = false
			};
		}


		protected override void OnMouseClick(MouseEventArgs e)
		{
			base.Focus();
			if (this.model.SelectedGalaxyChartComponent == null && this.FindGalaxyChartNodeAtMouseLocation(e.Location) == null)
			{
				base.Invalidate();
			}
			base.OnMouseClick(e);
		}


		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (this.selectedGroup != null && this.selectedGroup.IsRotating)
			{
				this.RotateSelectedGroup(e);
			}
			if (this.dragState != null)
			{
				if (!this.dragState.IsDraggingActive)
				{
					this.dragState.IsDraggingActive = (Math.Abs(this.dragState.InitialMouseLocation.X - e.Location.X) + Math.Abs(this.dragState.InitialMouseLocation.Y - e.Location.Y) > 2);
				}
				if (this.dragState.IsDraggingActive)
				{
					if (this.selectedGroup != null && this.selectedGroup.SelectedGroupOfNodes != null && this.selectedGroup.SelectedGroupOfNodes.Length != 0)
					{
						this.DragSelectedGroup(e);
					}
					else if (this.dragState.Node != null)
					{
						FloatPoint floatPoint = this.ConvertViewportLocationToGalaxyChartPosition(e.Location);
						FloatPoint delta = new FloatPoint(floatPoint.X - this.dragState.Node.Position.X, floatPoint.Y - this.dragState.Node.Position.Y);
						this.dragState.Node.Position = floatPoint;
						if (Control.ModifierKeys == Keys.Shift)
						{
							this.MoveChildNodesRecursive(this.dragState.Node.ChildNodes, delta);
						}
						this.dragState.InitialMouseLocation = e.Location;
						this.Refresh();
					}
				}
			}
			if (this.boundingBox != null)
			{
				this.CreateBoundingBox(e.Location);
			}
		}


		private PointF ConvertGalaxyChartPositionToViewportLocation(FloatPoint galaxyChartPosition)
		{
			FloatPoint floatPoint = FloatPoint.Subtract(galaxyChartPosition, this.anchorPosition);
			PointF pointF = new PointF(floatPoint.X * this.zoomLevel, floatPoint.Y * this.zoomLevel);
			return new PointF(pointF.X, (float)base.ClientSize.Height - pointF.Y);
		}


		private FloatPoint ConvertViewportLocationToGalaxyChartPosition(PointF viewportLocation)
		{
			PointF pointF = new PointF(viewportLocation.X, (float)base.ClientSize.Height - viewportLocation.Y);
			FloatPoint b = new FloatPoint(pointF.X / this.zoomLevel, pointF.Y / this.zoomLevel);
			return FloatPoint.Add(this.anchorPosition, b);
		}


		private FloatPoint ConvertViewportLocationToGalaxyChartPosition(Point viewportLocation)
		{
			return this.ConvertViewportLocationToGalaxyChartPosition(new PointF((float)viewportLocation.X, (float)viewportLocation.Y));
		}


		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			if (disposing)
			{
				NodeGraphicsManager nodeGraphicsManager = this.nodeGraphicsManager;
				if (nodeGraphicsManager != null)
				{
					nodeGraphicsManager.Dispose();
				}
				Font font = this.playerIndexFont;
				if (font != null)
				{
					font.Dispose();
				}
			}
			base.Dispose(disposing);
		}


		private void InitializeComponent()
		{
			base.SuspendLayout();
			base.Name = "GalaxyChartCanvas";
			base.Size = new Size(544, 498);
			base.ResumeLayout(false);
		}


		private Color RootNodeDotUnderlayColor = Color.LightSlateGray;


		private const int RootNodeDotUnderlaySize = 32;


		private const int BaseDotSize = 25;


		private const int MinDotSize = 10;


		private const int ParentDepthDotShrinkage = 5;


		private const int OwnershipRingWidth = 2;


		private Color ActionTargetRingColor = Color.YellowGreen;


		private Color SelectionRingColor = Color.White;


		private const int SelectionRingSize = 32;


		private const int SelectionPenWidth = 3;


		private const float MinZoomLevel = 0.01f;


		private const float MaxZoomLevel = 100f;


		private const double PanAcceleration = 5.0;


		private const double PanDragRate = 10.0;


		private const double PanDistanceAtMaxSpeed = 500.0;


		private ScenarioModel model;


		private ScenarioViewportControl.DragState dragState;


		private ScenarioViewportControl.BoundingBox boundingBox;


		private ScenarioViewportControl.SelectedGroup selectedGroup;


		private ScenarioViewportControl.CopyGroup copyGroup;


		private Font playerIndexFont;


		private ScenarioViewportActionMenu actionMenu;


		private NodeGraphicsManager nodeGraphicsManager;


		private FloatPoint anchorPosition = new FloatPoint();


		private float zoomLevel = 1f;


		private bool canPan = true;


		private double panXSpeed;


		private double panYSpeed;


		private IContainer components;


		private class DragState
		{



			public GalaxyChartNode Node { get; set; }




			public Point InitialMouseLocation { get; set; }




			public bool IsDraggingActive { get; set; }
		}


		private class BoundingBox
		{



			public Point InitialMouseLocation { get; set; }




			public Point FinalMouseLocation { get; set; }




			public GalaxyChartNode[] SelectedNodes { get; set; }
		}


		private class SelectedGroup
		{



			public Point[] NodeRelativePositions { get; set; }




			public PointF RotationCenter { get; set; }




			public Point InitialMouseLocation { get; set; }


			public GalaxyChartNode[] SelectedGroupOfNodes;


			public bool IsRotating;
		}


		public class PhaseLaneData
		{



			public GalaxyChartNodeId NodeAId { get; set; }




			public GalaxyChartNodeId NodeBId { get; set; }
		}


		public class CopyGroup
		{



			public int[] CopiedNodeIds { get; set; }




			public List<ScenarioViewportControl.PhaseLaneData> ConnectedLanes { get; set; }




			public Point[] NodeRelativePositions { get; set; }




			public Dictionary<int, GalaxyChartNodeId?> ParentNodeIds { get; set; }




			public Dictionary<int, GalaxyChartNodeFillingName> FillingNames { get; set; }




			public GalaxyChartNodeCopy[] CopiedNodes { get; set; }
		}
	}
}
