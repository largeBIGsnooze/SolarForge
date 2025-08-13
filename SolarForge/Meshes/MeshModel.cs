using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Solar;
using Solar.Math;
using Solar.Rendering;
using Solar.Simulations;
using SolarForge.Scenes;
using SolarForge.Utility;

namespace SolarForge.Meshes
{

	public class MeshModel : SceneModel3d, IDisposable
	{



		public event MeshModel.SelectedMeshInstanceChangedDelegate SelectedMeshInstanceChanged;




		public event MeshModel.SelectedMeshTrianglesFacingChangedDelegate SelectedMeshTrianglesFacingChanged;




		public event MeshModel.SelectedPointChangedDelegate SelectedPointChanged;




		public event MeshModel.SelectedMaterialChangedDelegate SelectedMaterialChanged;




		public event MeshModel.MeshInstancesChangedDelegate MeshInstancesChanged;


		public MeshModel(Engine engine, PostProcessingParams postProcessingParams, CameraSettings cameraSettings, MeshSettings modelSettings, IJsonBeautifier jsonBeautifier) : base(engine, postProcessingParams, cameraSettings, modelSettings)
		{
			this.engine = engine;
			this.settings = modelSettings;
			this.jsonBeautifier = jsonBeautifier;
			this.selectedMeshView = MeshView.Default;
			this.selectedMeshTrianglesView = SolarForge.Meshes.MeshTrianglesView.FacingLoose;
			this.selectedMeshTrianglesFacing = Facing.Front;
			this.meshInstances = new List<MeshInstance>();
		}


		public override void Dispose()
		{
			this.DisposeMeshes(false);
		}


		public void DisposeMeshes(bool notifyDelegates)
		{
			foreach (MeshInstance meshInstance in this.meshInstances)
			{
				meshInstance.Mesh.Dispose();
			}
			this.meshInstances.Clear();
			this.SetSelectedMeshIndex(null, notifyDelegates);
			if (notifyDelegates)
			{
				MeshModel.MeshInstancesChangedDelegate meshInstancesChanged = this.MeshInstancesChanged;
				if (meshInstancesChanged == null)
				{
					return;
				}
				meshInstancesChanged(this.meshInstances);
			}
		}


		public void SaveMaterialChanges()
		{
			if (this.SelectedMeshInstance != null)
			{
				foreach (MeshMaterial meshMaterial in this.SelectedMeshInstance.Mesh.Data.Materials)
				{
					meshMaterial.SaveToFile(meshMaterial.Path);
					this.jsonBeautifier.BeautifyJson(meshMaterial.Path);
				}
			}
		}




		public MeshView SelectedMeshView
		{
			get
			{
				return this.selectedMeshView;
			}
			set
			{
				this.selectedMeshView = value;
			}
		}




		public SolarForge.Meshes.MeshTrianglesView SelectedMeshTrianglesView
		{
			get
			{
				return this.selectedMeshTrianglesView;
			}
			set
			{
				this.selectedMeshTrianglesView = value;
			}
		}




		public Point? SelectedMeshTrianglesGridCoord
		{
			get
			{
				return this.selectedMeshTrianglesGridCoord;
			}
			set
			{
				this.selectedMeshTrianglesGridCoord = value;
			}
		}




		public Facing SelectedMeshTrianglesFacing
		{
			get
			{
				return this.selectedMeshTrianglesFacing;
			}
			set
			{
				this.selectedMeshTrianglesFacing = value;
				MeshModel.SelectedMeshTrianglesFacingChangedDelegate selectedMeshTrianglesFacingChanged = this.SelectedMeshTrianglesFacingChanged;
				if (selectedMeshTrianglesFacingChanged == null)
				{
					return;
				}
				selectedMeshTrianglesFacingChanged();
			}
		}



		public int? SelectedMeshIndex
		{
			get
			{
				return this.selectedMeshIndex;
			}
		}



		public MeshInstance SelectedMeshInstance
		{
			get
			{
				if (this.selectedMeshIndex == null)
				{
					return null;
				}
				return this.meshInstances[this.selectedMeshIndex.Value];
			}
		}


		public void SetSelectedMeshIndex(int? meshIndex, bool notifyDelegates)
		{
			if (meshIndex.GetValueOrDefault() == -1)
			{
				this.selectedMeshIndex = null;
			}
			else
			{
				this.selectedMeshIndex = meshIndex;
			}
			this.SetSelectedPointIndex(null, notifyDelegates);
			this.SetSelectedMaterialIndex(null, notifyDelegates);
			if (notifyDelegates)
			{
				MeshModel.SelectedMeshInstanceChangedDelegate selectedMeshInstanceChanged = this.SelectedMeshInstanceChanged;
				if (selectedMeshInstanceChanged == null)
				{
					return;
				}
				selectedMeshInstanceChanged(this.SelectedMeshInstance);
			}
		}


		public void SetSelectedPointIndex(int? index, bool notifyDelegates)
		{
			this.selectedPointIndex = index;
			if (notifyDelegates)
			{
				MeshModel.SelectedPointChangedDelegate selectedPointChanged = this.SelectedPointChanged;
				if (selectedPointChanged == null)
				{
					return;
				}
				selectedPointChanged();
			}
		}


		public void SetSelectedMaterialIndex(int? index, bool notifyDelegates)
		{
			this.selectedMaterialIndex = index;
			if (notifyDelegates)
			{
				MeshModel.SelectedMaterialChangedDelegate selectedMaterialChanged = this.SelectedMaterialChanged;
				if (selectedMaterialChanged == null)
				{
					return;
				}
				selectedMaterialChanged();
			}
		}



		public MeshSettings Settings
		{
			get
			{
				return this.settings;
			}
		}


		public void FocusOnSelectedMeshPoint()
		{
			if (this.SelectedPoint != null)
			{
				base.FocusOnTargetBasis(new Basis(this.SelectedMeshInstance.Basis.Transform(this.SelectedPoint.Position), this.SelectedMeshInstance.Basis.Transform(this.SelectedPoint.Rotation)));
			}
		}


		public void FocusOnSelectedMeshInstance()
		{
			if (this.SelectedMeshInstance != null)
			{
				base.FocusOnTargetBasis(this.SelectedMeshInstance.Basis);
			}
		}


		public override void HandleMouseClick(Point point, MouseButtons button)
		{
			if (button == MouseButtons.Left && this.SelectedMeshInstance != null)
			{
				Ray3 ray = base.Camera.MakeCursorRayInObjectSpace(this.SelectedMeshInstance.Basis, point);
				List<int> rayIntersectionTriangleIndices = this.SelectedMeshInstance.Mesh.Data.GetRayIntersectionTriangleIndices(ray);
				if (rayIntersectionTriangleIndices.Count == 0)
				{
					this.SelectedMeshInstance.PickedTriangleIndex = null;
					return;
				}
				if (this.SelectedMeshInstance.PickedTriangleIndex != null && rayIntersectionTriangleIndices.Contains(this.SelectedMeshInstance.PickedTriangleIndex.Value))
				{
					int num = rayIntersectionTriangleIndices.IndexOf(this.SelectedMeshInstance.PickedTriangleIndex.Value);
					this.SelectedMeshInstance.PickedTriangleIndex = new int?(rayIntersectionTriangleIndices[(num + 1) % rayIntersectionTriangleIndices.Count]);
					return;
				}
				this.SelectedMeshInstance.PickedTriangleIndex = new int?(rayIntersectionTriangleIndices.First<int>());
			}
		}


		public override void RefreshScene()
		{
		}


		protected override void UpdateTick(float timeElapsed)
		{
		}


		protected override void RenderScene()
		{
			List<MeshShadowBlocker> list = new List<MeshShadowBlocker>();
			foreach (MeshInstance meshInstance in this.meshInstances)
			{
				list.Add(new MeshShadowBlocker(meshInstance.Mesh, meshInstance.Basis));
			}
			this.engine.RenderScene(base.TotalTimeElapsed, base.PostProcessingParams, base.ClearColor, base.Skybox, base.Camera, base.Lights, list, null, delegate(RenderSceneSkyboxParams p)
			{
				if (base.Skybox != null)
				{
					p.SkyboxRenderer.RenderSkybox(p.Pipeline, base.Camera, base.Skybox);
				}
			}, delegate(RenderSceneMeshesParams p)
			{
				p.MeshRenderer.BeginRendering(this.settings.Shader, p.MeshRenderPassType);
				if (this.settings.Shader == UnitMeshShaderType.Basic)
				{
					using (List<MeshInstance>.Enumerator enumerator2 = this.meshInstances.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							MeshInstance meshInstance2 = enumerator2.Current;
							p.MeshRenderer.RenderBasicMesh(meshInstance2.Mesh, meshInstance2.Basis, base.Skybox, p.MeshRenderPassType);
						}
						goto IL_13E;
					}
				}
				if (this.settings.Shader == UnitMeshShaderType.Ship)
				{
					using (List<MeshInstance>.Enumerator enumerator2 = this.meshInstances.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							MeshInstance meshInstance3 = enumerator2.Current;
							p.MeshRenderer.RenderShipMesh(meshInstance3.Mesh, meshInstance3.Basis, base.Skybox, p.MeshRenderPassType);
						}
						goto IL_13E;
					}
				}
				if (this.settings.Shader == UnitMeshShaderType.DebugNormals)
				{
					foreach (MeshInstance meshInstance4 in this.meshInstances)
					{
						p.MeshRenderer.RenderBasicMesh(meshInstance4.Mesh, meshInstance4.Basis, base.Skybox, p.MeshRenderPassType);
					}
				}
				IL_13E:
				p.MeshRenderer.EndRendering();
			}, delegate(RenderScenePrim3dSimpleParams p)
			{
			}, delegate(RenderScenePrim3dComplexParams p)
			{
			}, delegate(RenderScenePrim3dLinesParams p)
			{
				this.RenderSelectedMeshPrim3dLines(p);
			}, delegate(RenderScenePrim3dShapesParams p)
			{
				this.RenderSelectedMeshPrim3dShapes(p);
			}, delegate(Render2dSceneParams p)
			{
			});
		}


		private void RenderSelectedMeshPrim3dShapes(RenderScenePrim3dShapesParams p)
		{
			MeshInstance selectedMeshInstance = this.SelectedMeshInstance;
			if (selectedMeshInstance != null && this.settings.ShowBoundingSphere)
			{
				p.Prim3dShapes.BeginRendering(p.Pipeline, base.Camera);
				p.Prim3dShapes.RenderSphere(new Sphere(selectedMeshInstance.Basis.Transform(selectedMeshInstance.Mesh.Data.BoundingSphere.Center), selectedMeshInstance.Mesh.Data.BoundingSphere.Radius), this.settings.BoundingSphereColor);
				p.Prim3dShapes.EndRendering();
			}
		}


		private void RenderSelectedMeshPrim3dLines(RenderScenePrim3dLinesParams p)
		{
			MeshInstance selectedMeshInstance = this.SelectedMeshInstance;
			if (selectedMeshInstance != null)
			{
				if (this.selectedMeshView == MeshView.Default)
				{
					if (this.settings.ShowWorldAxes)
					{
						float num = selectedMeshInstance.Mesh.Data.BoundingSphere.Radius * 2f;
						p.Prim3dLines.BeginRendering();
						p.Prim3dLines.RenderRotationAxes(selectedMeshInstance.Basis.Position, new Float3x3(), new Float3(num, num, num), this.settings.WorldAxesLineThickness, 1f);
						p.Prim3dLines.EndRendering();
					}
					if (this.settings.ShowMeshPoints)
					{
						p.Prim3dLines.BeginRendering();
						for (int i = 0; i < selectedMeshInstance.Mesh.Data.Points.Count; i++)
						{
							MeshPoint meshPoint = selectedMeshInstance.Mesh.Data.Points[i];
							int num2 = i;
							int? num3 = this.selectedPointIndex;
							bool flag = num2 == num3.GetValueOrDefault() & num3 != null;
							float num4 = flag ? this.settings.MeshPointLineLengthWhenSelected : this.settings.MeshPointLineLength;
							p.Prim3dLines.RenderRotationAxes(selectedMeshInstance.Basis.Transform(meshPoint.Position), selectedMeshInstance.Basis.Transform(meshPoint.Rotation), new Float3(num4, num4, num4), flag ? this.settings.MeshPointLineThicknessWhenSelected : this.settings.MeshPointLineThickness, flag ? this.settings.MeshPointLineAlphaWhenSelected : this.settings.MeshPointLineAlpha);
						}
						p.Prim3dLines.EndRendering();
					}
					if (this.settings.ShowBoundingBox)
					{
						p.Prim3dLines.BeginRendering();
						p.Prim3dLines.RenderLineBox(selectedMeshInstance.Mesh.Data.BoundingBox, selectedMeshInstance.Basis.Position, selectedMeshInstance.Basis.Rotation, this.settings.BoundingBoxColor, 3f);
						p.Prim3dLines.EndRendering();
						return;
					}
				}
				else if (this.selectedMeshView == MeshView.Triangles)
				{
					if (this.selectedMeshTrianglesView == SolarForge.Meshes.MeshTrianglesView.Picked)
					{
						this.RenderPickedTriangleLines(p, selectedMeshInstance);
						return;
					}
					if (this.selectedMeshTrianglesView == SolarForge.Meshes.MeshTrianglesView.All)
					{
						this.RenderAllTriangleLines(p, selectedMeshInstance);
						return;
					}
					if (this.selectedMeshTrianglesView == SolarForge.Meshes.MeshTrianglesView.Fat)
					{
						this.RenderAllFatTriangleLines(p, selectedMeshInstance);
						return;
					}
					if (this.SelectedMeshTrianglesView == SolarForge.Meshes.MeshTrianglesView.FacingLoose) 
					{
						this.RenderFacingLooseTriangleLines(p, selectedMeshInstance);
						return;
					}
					if (this.SelectedMeshTrianglesView == SolarForge.Meshes.MeshTrianglesView.FacingNonOverlapped)
					{
						this.RenderFacingNonOverlappedTriangleLines(p, selectedMeshInstance);
					}
				}
			}
		}


		private void RenderTriangleLines(RenderScenePrim3dLinesParams p, MeshInstance mi, Triangle triangle, Color color)
		{
			Float3 @float = mi.Basis.Transform(triangle.A);
			Float3 float2 = mi.Basis.Transform(triangle.B);
			Float3 float3 = mi.Basis.Transform(triangle.C);
			p.Prim3dLines.RenderLine(@float, float2, color, 1f);
			p.Prim3dLines.RenderLine(float2, float3, color, 1f);
			p.Prim3dLines.RenderLine(float3, @float, color, 1f);
		}


		private void RenderTriangleNormalLine(RenderScenePrim3dLinesParams p, MeshInstance mi, Float3 position, Float3 normal, Color color)
		{
			int num = 10;
			p.Prim3dLines.RenderLine(mi.Basis.Transform(position), mi.Basis.Transform(position + normal.Scale((float)num)), color, 1f);
		}


		private void RenderPickedTriangleLines(RenderScenePrim3dLinesParams p, MeshInstance mi)
		{
			if (mi.PickedTriangleIndex != null)
			{
				p.Prim3dLines.BeginRendering();
				Triangle triangleAt = mi.Mesh.Data.GetTriangleAt(mi.PickedTriangleIndex.Value);
				this.RenderTriangleLines(p, mi, triangleAt, Color.White);
				MeshTriangleProperties trianglePropertiesAt = mi.Mesh.Data.GetTrianglePropertiesAt(mi.PickedTriangleIndex.Value);
				this.RenderTriangleNormalLine(p, mi, trianglePropertiesAt.MiddlePosition, trianglePropertiesAt.AverageNormal, Color.Red);
				this.RenderTriangleNormalLine(p, mi, trianglePropertiesAt.MiddlePosition, trianglePropertiesAt.EdgeNormal, Color.Green);
				this.RenderTriangleNormalLine(p, mi, trianglePropertiesAt.APosition, trianglePropertiesAt.ANormal, Color.Blue);
				this.RenderTriangleNormalLine(p, mi, trianglePropertiesAt.BPosition, trianglePropertiesAt.BNormal, Color.Blue);
				this.RenderTriangleNormalLine(p, mi, trianglePropertiesAt.CPosition, trianglePropertiesAt.CNormal, Color.Blue);
				p.Prim3dLines.EndRendering();
			}
		}


		private void RenderAllTriangleLines(RenderScenePrim3dLinesParams p, MeshInstance mi)
		{
			p.Prim3dLines.BeginRendering();
			for (int i = 0; i < mi.Mesh.Data.TriangleCount; i++)
			{
				this.RenderTriangleLines(p, mi, mi.Mesh.Data.GetTriangleAt(i), Color.White);
			}
			p.Prim3dLines.EndRendering();
		}


		private void RenderAllFatTriangleLines(RenderScenePrim3dLinesParams p, MeshInstance mi)
		{
			p.Prim3dLines.BeginRendering();
			for (int i = 0; i < mi.Mesh.Data.FatTriangleCount; i++)
			{
				this.RenderTriangleLines(p, mi, mi.Mesh.Data.GetFatTriangleAt(i), Color.White);
			}
			p.Prim3dLines.EndRendering();
		}


		private Color GetFacingGridCoordColor(MeshTriangleGrid grid, Point coord)
		{
			Random random = new Random(coord.X + coord.Y * grid.Width);
			return Color.FromArgb(random.Next(255), random.Next(255), random.Next(255));
		}


		private void RenderFacingBoundingBoxAtGridCoord(RenderScenePrim3dLinesParams p, MeshInstance mi, MeshTriangleGrid grid, Point coord)
		{
			Box boundingBoxAt = grid.GetBoundingBoxAt(coord);
			p.Prim3dLines.RenderLineBox(boundingBoxAt, mi.Basis.Position, mi.Basis.Rotation, this.GetFacingGridCoordColor(grid, coord), 1f);
		}


		private void RenderFacingLooseTriangleLinesAtGridCoord(RenderScenePrim3dLinesParams p, MeshInstance mi, MeshTriangleGrid grid, Point coord)
		{
			Color facingGridCoordColor = this.GetFacingGridCoordColor(grid, coord);
			int looseTriangleCountAt = grid.GetLooseTriangleCountAt(coord);
			for (int i = 0; i < looseTriangleCountAt; i++)
			{
				this.RenderTriangleLines(p, mi, grid.GetLooseTriangleAt(coord, i), facingGridCoordColor);
			}
		}


		private void RenderFacingLooseTriangleLines(RenderScenePrim3dLinesParams p, MeshInstance mi)
		{
			p.Prim3dLines.BeginRendering();
			MeshTriangleGrid triangleGrid = mi.Mesh.Data.GetTriangleGrid(this.selectedMeshTrianglesFacing);
			if (this.selectedMeshTrianglesGridCoord != null && triangleGrid.GetNonEmptyTriangleGridCoords().Contains(this.selectedMeshTrianglesGridCoord.Value))
			{
				this.RenderFacingBoundingBoxAtGridCoord(p, mi, triangleGrid, this.selectedMeshTrianglesGridCoord.Value);
				this.RenderFacingLooseTriangleLinesAtGridCoord(p, mi, triangleGrid, this.selectedMeshTrianglesGridCoord.Value);
			}
			else
			{
				foreach (Point coord in triangleGrid.GetNonEmptyTriangleGridCoords())
				{
					this.RenderFacingLooseTriangleLinesAtGridCoord(p, mi, triangleGrid, coord);
				}
			}
			p.Prim3dLines.EndRendering();
		}


		private void RenderFacingNonOverlappedTriangleLinesAtGridCoord(RenderScenePrim3dLinesParams p, MeshInstance mi, MeshTriangleGrid grid, Point coord)
		{
			Color facingGridCoordColor = this.GetFacingGridCoordColor(grid, coord);
			int nonOverlappedTriangleCountAt = grid.GetNonOverlappedTriangleCountAt(coord);
			for (int i = 0; i < nonOverlappedTriangleCountAt; i++)
			{
				this.RenderTriangleLines(p, mi, grid.GetNonOverlappedTriangleAt(coord, i), facingGridCoordColor);
			}
		}


		private void RenderFacingNonOverlappedTriangleLines(RenderScenePrim3dLinesParams p, MeshInstance mi)
		{
			p.Prim3dLines.BeginRendering();
			MeshTriangleGrid triangleGrid = mi.Mesh.Data.GetTriangleGrid(this.selectedMeshTrianglesFacing);
			if (this.selectedMeshTrianglesGridCoord != null && triangleGrid.GetNonEmptyTriangleGridCoords().Contains(this.selectedMeshTrianglesGridCoord.Value))
			{
				this.RenderFacingBoundingBoxAtGridCoord(p, mi, triangleGrid, this.selectedMeshTrianglesGridCoord.Value);
				this.RenderFacingNonOverlappedTriangleLinesAtGridCoord(p, mi, triangleGrid, this.selectedMeshTrianglesGridCoord.Value);
			}
			else
			{
				foreach (Point coord in triangleGrid.GetNonEmptyTriangleGridCoords())
				{
					this.RenderFacingNonOverlappedTriangleLinesAtGridCoord(p, mi, triangleGrid, coord);
				}
			}
			p.Prim3dLines.EndRendering();
		}


		public void LoadMesh(string path, Basis basis)
		{
			MeshInstance meshInstance = new MeshInstance();
			meshInstance.Name = Path.GetFileNameWithoutExtension(path);
			meshInstance.Mesh = new Mesh();
			meshInstance.Mesh.LoadFromFile(base.Engine.RenderingSystem, path);
			meshInstance.Basis = basis;
			if (meshInstance.Basis == null)
			{
				if (this.meshInstances.Count == 0)
				{
					meshInstance.Basis = new Basis();
				}
				else
				{
					MeshInstance meshInstance2 = this.meshInstances[this.meshInstances.Count - 1];
					meshInstance.Basis = new Basis(meshInstance2.Basis);
					float num = meshInstance.Mesh.Data.BoundingSphere.Radius + meshInstance2.Mesh.Data.BoundingSphere.Radius;
					meshInstance.Basis.Position.X += num;
				}
			}
			this.meshInstances.Add(meshInstance);
			if (this.selectedMeshIndex == null)
			{
				this.SetSelectedMeshIndex(new int?(0), true);
			}
			if (this.meshInstances.Count == 1)
			{
				MeshInstance meshInstance3 = this.meshInstances[0];
				base.ResetCamera(new float?((meshInstance3.Mesh.Data.BoundingSphere.Center.Length + meshInstance3.Mesh.Data.BoundingSphere.Radius) * 1.5f));
			}
			MeshModel.MeshInstancesChangedDelegate meshInstancesChanged = this.MeshInstancesChanged;
			if (meshInstancesChanged == null)
			{
				return;
			}
			meshInstancesChanged(this.meshInstances);
		}



		public MeshPoint SelectedPoint
		{
			get
			{
				if (this.selectedPointIndex != null && this.SelectedMeshInstance != null)
				{
					int? num = this.selectedPointIndex;
					int count = this.SelectedMeshInstance.Mesh.Data.Points.Count;
					if (num.GetValueOrDefault() < count & num != null)
					{
						return this.SelectedMeshInstance.Mesh.Data.Points[this.selectedPointIndex.Value];
					}
				}
				return null;
			}
		}



		public MeshMaterial SelectedMaterial
		{
			get
			{
				if (this.selectedMaterialIndex != null && this.SelectedMeshInstance != null)
				{
					int? num = this.selectedMaterialIndex;
					int count = this.SelectedMeshInstance.Mesh.Data.Materials.Count;
					if (num.GetValueOrDefault() < count & num != null)
					{
						return this.SelectedMeshInstance.Mesh.Data.Materials[this.selectedMaterialIndex.Value];
					}
				}
				return null;
			}
		}


		private Engine engine;


		private MeshSettings settings;


		private IJsonBeautifier jsonBeautifier;


		private List<MeshInstance> meshInstances;


		private int? selectedMeshIndex;


		private int? selectedPointIndex;


		private int? selectedMaterialIndex;


		private MeshView selectedMeshView;


		private SolarForge.Meshes.MeshTrianglesView selectedMeshTrianglesView; 


		private Facing selectedMeshTrianglesFacing;


		private Point? selectedMeshTrianglesGridCoord;



		public delegate void SelectedMeshInstanceChangedDelegate(MeshInstance meshInstance);



		public delegate void SelectedMeshTrianglesFacingChangedDelegate();



		public delegate void SelectedPointChangedDelegate();



		public delegate void SelectedMaterialChangedDelegate();



		public delegate void MeshInstancesChangedDelegate(IEnumerable<MeshInstance> meshInstances);
	}
}
