using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Solar;
using Solar.Math;
using Solar.Rendering;
using Solar.Simulations;
using SolarForge.Scenes;
using SolarForge.Utility;

namespace SolarForge.Units
{

	public class UnitModel : SceneModel3d
	{



		public event UnitModel.UnitDefinitionChangedDelegate UnitDefinitionChanged;




		public event UnitModel.UnitSkinDefinitionsChangedDelegate UnitSkinDefinitionsChanged;




		public event UnitModel.SelectedUnitSkinDefinitionChangedDelegate SelectedUnitSkinDefinitionChanged;




		public event UnitModel.SelectedUnitSkinStageDefinitionChangedDelegate SelectedUnitSkinStageDefinitionChanged;




		public event UnitModel.SelectedWeaponInstanceChangedDelegate SelectedWeaponInstanceChanged;




		public event UnitModel.SelectedSpawnCustomDebrisDefinitionChangedDelegate SelectedSpawnCustomDebrisDefinitionChanged;




		public event UnitModel.SkinStageMeshChangedDelegate SkinStageMeshChanged;


		public UnitModel(Engine engine, PostProcessingParams postProcessingParams, CameraSettings cameraSettings, UnitSettings modelSettings, IDataSourcePathResolver dataSourcePathResolver, IJsonBeautifier jsonBeautifier) : base(engine, postProcessingParams, cameraSettings, modelSettings)
		{
			this.settings = modelSettings;
			this.dataSourcePathResolver = dataSourcePathResolver;
			this.jsonBeautifier = jsonBeautifier;
			this.skinStageMeshBasis = new Basis();
			this.unitSkinDefinitions = new List<UnitSkinDefinition>();
			this.weaponDefinitions = new Dictionary<WeaponDefinitionName, WeaponDefinition>();
			this.selectedSkinStageMeshPointIndex = -1;
			this.selectedUnitView = UnitView.Skin;
			this.skinStageMeshTables = new List<SkinStageMeshTable>();
		}


		public override void Dispose()
		{
			this.DisposeMeshes(false);
		}


		private void DisposeMeshes(bool notifyDelegates)
		{
			if (this.enable_changes_skinStageMeshes)
			{
				foreach (SkinStageMeshTable skinStageMeshTable in this.skinStageMeshTables)
				{
					Mesh baseMesh = skinStageMeshTable.BaseMesh;
					if (baseMesh != null)
					{
						baseMesh.Dispose();
					}
					if (skinStageMeshTable.WeaponNameToTurretMeshesMap != null)
					{
						foreach (TurretMeshes turretMeshes in skinStageMeshTable.WeaponNameToTurretMeshesMap.Values)
						{
							Mesh gimbalMesh = turretMeshes.GimbalMesh;
							if (gimbalMesh != null)
							{
								gimbalMesh.Dispose();
							}
							Mesh biaxialBaseMesh = turretMeshes.BiaxialBaseMesh;
							if (biaxialBaseMesh != null)
							{
								biaxialBaseMesh.Dispose();
							}
							Mesh biaxialBarrelMesh = turretMeshes.BiaxialBarrelMesh;
							if (biaxialBarrelMesh != null)
							{
								biaxialBarrelMesh.Dispose();
							}
						}
					}
					if (skinStageMeshTable.ChildMeshes != null)
					{
						foreach (Mesh mesh in skinStageMeshTable.ChildMeshes)
						{
							if (mesh != null)
							{
								mesh.Dispose();
							}
						}
					}
				}
				this.skinStageMeshTables.Clear();
			}
			this.selectedSkinStageMeshPointIndex = -1;
			if (notifyDelegates)
			{
				UnitModel.SkinStageMeshChangedDelegate skinStageMeshChanged = this.SkinStageMeshChanged;
				if (skinStageMeshChanged == null)
				{
					return;
				}
				skinStageMeshChanged(null);
			}
		}



		public UnitSettings Settings
		{
			get
			{
				return this.settings;
			}
		}



		public EntityDefinitionFactories EntityDefinitionFactories
		{
			get
			{
				return base.Engine.EntityDefinitionFactories;
			}
		}


		public void LoadEntity(string path)
		{
			this.enable_changes_skinStageMeshes = false;
			this.SelectedWeaponInstanceIndex = null;
			this.SelectedUnitSkinDefinition = null;
			this.SelectedSpawnCustomDebrisDefinitionIndex = null;
			this.unitDefinition = this.EntityDefinitionFactories.LoadUnitDefinitionFromFile(path);
			UnitModel.UnitDefinitionChangedDelegate unitDefinitionChanged = this.UnitDefinitionChanged;
			if (unitDefinitionChanged != null)
			{
				unitDefinitionChanged(this.unitDefinition);
			}
			this.unitSkinDefinitions.Clear();
			this.weaponDefinitions.Clear();
			if (this.UnitDefinition != null)
			{
				this.unitSkinDefinitions = this.UnitDefinition.SkinNames.Select(new Func<UnitSkinDefinitionName, UnitSkinDefinition>(this.LoadUnitSkinDefinition)).ToList<UnitSkinDefinition>();
				if (this.UnitDefinition.Weapons != null)
				{
					foreach (WeaponInstanceDefinition weaponInstanceDefinition in this.UnitDefinition.Weapons.WeaponInstances)
					{
						if (!this.weaponDefinitions.ContainsKey(weaponInstanceDefinition.Weapon))
						{
							this.weaponDefinitions.Add(weaponInstanceDefinition.Weapon, this.LoadWeaponDefinition(weaponInstanceDefinition.Weapon));
						}
					}
				}
			}
			UnitModel.UnitSkinDefinitionsChangedDelegate unitSkinDefinitionsChanged = this.UnitSkinDefinitionsChanged;
			if (unitSkinDefinitionsChanged != null)
			{
				unitSkinDefinitionsChanged(this.unitSkinDefinitions);
			}
			this.SelectedWeaponInstanceChanged(this.SelectedWeaponInstance);
			this.enable_changes_skinStageMeshes = true;
			this.RefreshMeshes(true);
		}


		private UnitSkinDefinition LoadUnitSkinDefinition(UnitSkinDefinitionName name)
		{
			UnitSkinDefinition unitSkinDefinition = this.EntityDefinitionFactories.LoadUnitSkinDefinitionByName(name);
			if (unitSkinDefinition == null)
			{
				throw new InvalidOperationException(string.Format("Entity not found or not of type UnitSkin : {0}", name));
			}
			return unitSkinDefinition;
		}


		private WeaponDefinition LoadWeaponDefinition(WeaponDefinitionName name)
		{
			WeaponDefinition weaponDefinition = this.EntityDefinitionFactories.LoadWeaponDefinitionByName(name);
			if (weaponDefinition == null)
			{
				throw new InvalidOperationException(string.Format("Entity not found or not of type Weapon : {0}", name));
			}
			return weaponDefinition;
		}


		public void SaveEntity(string path)
		{
			try
			{
				if (this.unitDefinition != null)
				{
					string path2 = this.dataSourcePathResolver.ResolveDataSourceSavePath(path);
					this.EntityDefinitionFactories.SaveEntityDefinitionToFile(this.unitDefinition, path2);
					this.jsonBeautifier.BeautifyJson(path2);
				}
				if (this.SelectedUnitSkinDefinition != null)
				{
					string path3 = this.dataSourcePathResolver.ResolveDataSourceSavePath(this.SelectedUnitSkinDefinition.LoadedPath);
					this.EntityDefinitionFactories.SaveEntityDefinitionToFile(this.SelectedUnitSkinDefinition, path3);
					this.jsonBeautifier.BeautifyJson(path3);
				}
				foreach (WeaponDefinition weaponDefinition in this.weaponDefinitions.Values)
				{
					string path4 = this.dataSourcePathResolver.ResolveDataSourceSavePath(weaponDefinition.LoadedPath);
					this.EntityDefinitionFactories.SaveEntityDefinitionToFile(weaponDefinition, path4);
					this.jsonBeautifier.BeautifyJson(path4);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message ?? "", ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}



		public UnitDefinition UnitDefinition
		{
			get
			{
				return this.unitDefinition;
			}
		}


		private SkinStageMeshTable TryGetSkinStageMeshTable(int? skinStageIndex)
		{
			if (skinStageIndex != null && skinStageIndex.Value < this.skinStageMeshTables.Count)
			{
				return this.skinStageMeshTables[skinStageIndex.Value];
			}
			return null;
		}



		public Mesh SkinStageMesh
		{
			get
			{
				SkinStageMeshTable skinStageMeshTable = this.TryGetSkinStageMeshTable(this.selectedUnitSkinStageIndex);
				if (skinStageMeshTable == null)
				{
					return null;
				}
				return skinStageMeshTable.BaseMesh;
			}
		}




		public UnitSkinDefinition SelectedUnitSkinDefinition
		{
			get
			{
				return this.selectedUnitSkinDefinition;
			}
			set
			{
				if (this.selectedUnitSkinDefinition != value)
				{
					this.SelectedUnitSkinStageIndex = null;
					this.selectedUnitSkinDefinition = value;
					UnitModel.SelectedUnitSkinDefinitionChangedDelegate selectedUnitSkinDefinitionChanged = this.SelectedUnitSkinDefinitionChanged;
					if (selectedUnitSkinDefinitionChanged == null)
					{
						return;
					}
					selectedUnitSkinDefinitionChanged(value);
				}
			}
		}



		public UnitSkinStageDefinition SelectedUnitSkinStageDefinition
		{
			get
			{
				if (this.selectedUnitSkinStageIndex != null)
				{
					return this.SelectedUnitSkinDefinition.SkinStages[this.selectedUnitSkinStageIndex.Value];
				}
				return null;
			}
		}




		public int? SelectedUnitSkinStageIndex
		{
			get
			{
				return this.selectedUnitSkinStageIndex;
			}
			set
			{
				int? num = this.selectedUnitSkinStageIndex;
				int? num2 = value;
				if (!(num.GetValueOrDefault() == num2.GetValueOrDefault() & num != null == (num2 != null)))
				{
					this.selectedUnitSkinStageIndex = value;
					UnitModel.SelectedUnitSkinStageDefinitionChangedDelegate selectedUnitSkinStageDefinitionChanged = this.SelectedUnitSkinStageDefinitionChanged;
					if (selectedUnitSkinStageDefinitionChanged != null)
					{
						selectedUnitSkinStageDefinitionChanged(this.SelectedUnitSkinStageDefinition);
					}
					this.RefreshMeshes(false);
				}
			}
		}




		public UnitView SelectedUnitView
		{
			get
			{
				return this.selectedUnitView;
			}
			set
			{
				this.selectedUnitView = value;
			}
		}



		public WeaponInstanceDefinition SelectedWeaponInstance
		{
			get
			{
				if (this.selectedWeaponInstanceIndex != null)
				{
					return this.UnitDefinition.Weapons.WeaponInstances[this.selectedWeaponInstanceIndex.Value];
				}
				return null;
			}
		}



		public SpawnCustomDebrisDefinition SelectedSpawnCustomDebrisDefinition
		{
			get
			{
				if (this.selectedSpawnCustomDebrisDefinitionIndex != null)
				{
					return this.UnitDefinition.SpawnDebris.CustomDebrisList[this.selectedSpawnCustomDebrisDefinitionIndex.Value];
				}
				return null;
			}
		}


		public WeaponDefinition TryGetWeaponDefinition(WeaponInstanceDefinition weaponInstance)
		{
			WeaponDefinition result = null;
			if (weaponInstance != null)
			{
				this.weaponDefinitions.TryGetValue(weaponInstance.Weapon, out result);
			}
			return result;
		}




		public int SelectedSkinStageMeshPointIndex
		{
			get
			{
				return this.selectedSkinStageMeshPointIndex;
			}
			set
			{
				this.selectedSkinStageMeshPointIndex = value;
			}
		}




		public int? SelectedWeaponInstanceIndex
		{
			get
			{
				return this.selectedWeaponInstanceIndex;
			}
			set
			{
				this.selectedWeaponInstanceIndex = value;
				UnitModel.SelectedWeaponInstanceChangedDelegate selectedWeaponInstanceChanged = this.SelectedWeaponInstanceChanged;
				if (selectedWeaponInstanceChanged == null)
				{
					return;
				}
				selectedWeaponInstanceChanged(this.SelectedWeaponInstance);
			}
		}




		public int? SelectedSpawnCustomDebrisDefinitionIndex
		{
			get
			{
				return this.selectedSpawnCustomDebrisDefinitionIndex;
			}
			set
			{
				this.selectedSpawnCustomDebrisDefinitionIndex = value;
				UnitModel.SelectedSpawnCustomDebrisDefinitionChangedDelegate selectedSpawnCustomDebrisDefinitionChanged = this.SelectedSpawnCustomDebrisDefinitionChanged;
				if (selectedSpawnCustomDebrisDefinitionChanged != null)
				{
					selectedSpawnCustomDebrisDefinitionChanged(this.SelectedSpawnCustomDebrisDefinition);
				}
				this.selectedSpawnCustomDebrisMesh = null;
				if (this.SelectedSpawnCustomDebrisDefinition != null)
				{
					UnitDefinition unitDefinition = this.EntityDefinitionFactories.LoadUnitDefinitionByName(this.SelectedSpawnCustomDebrisDefinition.Unit);
					if (unitDefinition.SkinNames.Count > 0)
					{
						UnitSkinDefinition unitSkinDefinition = this.EntityDefinitionFactories.LoadUnitSkinDefinitionByName(unitDefinition.SkinNames[0]);
						if (unitSkinDefinition.SkinStages.Count > 0 && unitSkinDefinition.SkinStages[0].UnitMesh != null)
						{
							this.selectedSpawnCustomDebrisMesh = this.TryLoadMesh(unitSkinDefinition.SkinStages[0].UnitMesh.MeshName);
						}
					}
				}
			}
		}


		private Mesh TryLoadMesh(MeshName meshName)
		{
			Mesh mesh = null;
			if (meshName != null)
			{
				string text = base.Engine.MeshFactory.ResolveFilePath(meshName);
				if (text.Length > 0)
				{
					mesh = new Mesh();
					mesh.LoadFromFile(base.Engine.RenderingSystem, text);
				}
			}
			return mesh;
		}


		private Mesh TryLoadChildMeshOfUnitSkinStage(string childMeshAlias, UnitSkinDefinition skinDefinition, in UnitSkinStageDefinition skinStageDefinition)
		{
			Mesh mesh = null;
			if (skinStageDefinition != null && !string.IsNullOrEmpty(childMeshAlias))
			{
				UnitMeshDefinition unitMeshDefinition = skinStageDefinition.TryFindChildMesh(childMeshAlias);
				if (unitMeshDefinition == null)
				{
					MessageBox.Show("Child Mesh not found in Skin.\nskin = " + skinDefinition.ToString() + "\nchildMeshAlias = " + childMeshAlias, "ChildMeshAlias not found", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				else
				{
					mesh = this.TryLoadMesh(unitMeshDefinition.MeshName);
					if (mesh == null)
					{
						MessageBox.Show(string.Format("Child mesh of skin not loaded.\nskin = {0}\nchildMeshAlias = {1}\nmeshName = {2}", skinDefinition.ToString(), childMeshAlias, unitMeshDefinition.MeshName), "ChildMesh not loaded", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					}
				}
			}
			return mesh;
		}


		private SkinStageMeshTable CreateSkinStageMeshTable(UnitSkinStageDefinition skinStage, int skinStageIndex)
		{
			SkinStageMeshTable skinStageMeshTable = new SkinStageMeshTable();
			if (skinStage.UnitMesh != null)
			{
				skinStageMeshTable.BaseMesh = this.TryLoadMesh(skinStage.UnitMesh.MeshName);
			}
			else if (skinStage.PlanetMesh != null)
			{
				skinStageMeshTable.BaseMesh = this.TryLoadMesh(skinStage.PlanetMesh.MeshName);
			}
			if (this.UnitDefinition.Weapons != null)
			{
				skinStageMeshTable.WeaponNameToTurretMeshesMap = new Dictionary<WeaponDefinitionName, TurretMeshes>();
				foreach (WeaponInstanceDefinition weaponInstanceDefinition in this.UnitDefinition.Weapons.WeaponInstances)
				{
					if (weaponInstanceDefinition.MinRequiredStageIndex == skinStageIndex && !skinStageMeshTable.WeaponNameToTurretMeshesMap.ContainsKey(weaponInstanceDefinition.Weapon))
					{
						WeaponDefinition weaponDefinition = this.TryGetWeaponDefinition(weaponInstanceDefinition);
						if (weaponDefinition != null)
						{
							TurretMeshes turretMeshes = new TurretMeshes();
							turretMeshes.GimbalMesh = this.TryLoadChildMeshOfUnitSkinStage(weaponDefinition.GimbalMeshAlias, this.SelectedUnitSkinDefinition, skinStage);
							turretMeshes.BiaxialBaseMesh = this.TryLoadChildMeshOfUnitSkinStage(weaponDefinition.BiaxialBaseMeshAlias, this.SelectedUnitSkinDefinition, skinStage);
							turretMeshes.BiaxialBarrelMesh = this.TryLoadChildMeshOfUnitSkinStage(weaponDefinition.BiaxialBarrelMeshAlias, this.SelectedUnitSkinDefinition, skinStage);
							skinStageMeshTable.WeaponNameToTurretMeshesMap.Add(weaponInstanceDefinition.Weapon, turretMeshes);
						}
					}
				}
			}
			skinStageMeshTable.ChildMeshes = new List<Mesh>();
			foreach (UnitChildMeshDefinition unitChildMeshDefinition in this.unitDefinition.ChildMeshes)
			{
				skinStageMeshTable.ChildMeshes.Add(this.TryLoadChildMeshOfUnitSkinStage(unitChildMeshDefinition.MeshAlias, this.SelectedUnitSkinDefinition, skinStage));
			}
			return skinStageMeshTable;
		}


		private void RefreshMeshes(bool resetCamera)
		{
			this.DisposeMeshes(true);
			if (this.enable_changes_skinStageMeshes && this.SelectedUnitSkinDefinition != null)
			{
				for (int i = 0; i < this.SelectedUnitSkinDefinition.SkinStages.Count; i++)
				{
					this.skinStageMeshTables.Add(this.CreateSkinStageMeshTable(this.SelectedUnitSkinDefinition.SkinStages[i], i));
				}
			}
			if (this.SelectedUnitSkinStageDefinition != null && (this.SelectedUnitSkinStageDefinition.UnitMesh != null || this.SelectedUnitSkinStageDefinition.PlanetMesh != null))
			{
				if (this.SkinStageMesh != null && resetCamera)
				{
					base.ResetCamera(new float?((this.SkinStageMesh.Data.BoundingSphere.Center.Length + this.SkinStageMesh.Data.BoundingSphere.Radius) * 1.5f));
				}
				this.selectedSkinStageMeshPointIndex = -1;
				UnitModel.SkinStageMeshChangedDelegate skinStageMeshChanged = this.SkinStageMeshChanged;
				if (skinStageMeshChanged == null)
				{
					return;
				}
				skinStageMeshChanged(this.SkinStageMesh);
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
			base.Engine.RenderScene(base.TotalTimeElapsed, base.PostProcessingParams, base.ClearColor, base.Skybox, base.Camera, base.Lights, null, null, delegate(RenderSceneSkyboxParams p)
			{
				if (base.Skybox != null)
				{
					p.SkyboxRenderer.RenderSkybox(p.Pipeline, base.Camera, base.Skybox);
				}
			}, delegate(RenderSceneMeshesParams p)
			{
				if (this.SkinStageMesh != null)
				{
					if (this.SelectedUnitSkinStageDefinition.UnitMesh != null)
					{
						if (this.SelectedUnitSkinStageDefinition.UnitMesh.Shader == UnitMeshShaderType.Basic)
						{
							p.MeshRenderer.BeginRendering(UnitMeshShaderType.Basic, p.MeshRenderPassType);
							p.MeshRenderer.RenderBasicMesh(this.SkinStageMesh, this.skinStageMeshBasis, base.Skybox, p.MeshRenderPassType);
							p.MeshRenderer.EndRendering();
						}
						else if (this.SelectedUnitSkinStageDefinition.UnitMesh.Shader == UnitMeshShaderType.Ship)
						{
							p.MeshRenderer.BeginRendering(UnitMeshShaderType.Ship, p.MeshRenderPassType);
							p.MeshRenderer.RenderShipMesh(this.SkinStageMesh, this.skinStageMeshBasis, base.Skybox, p.MeshRenderPassType);
							p.MeshRenderer.EndRendering();
						}
						else if (this.SelectedUnitSkinStageDefinition.UnitMesh.Shader == UnitMeshShaderType.DebugNormals)
						{
							p.MeshRenderer.BeginRendering(UnitMeshShaderType.DebugNormals, p.MeshRenderPassType);
							p.MeshRenderer.RenderShipMesh(this.SkinStageMesh, this.skinStageMeshBasis, base.Skybox, p.MeshRenderPassType);
							p.MeshRenderer.EndRendering();
						}
						this.RenderWeaponMeshes(p);
						if (this.Settings.ShowAllChildMeshes)
						{
							this.RenderChildMeshes(p);
						}
						if (this.selectedUnitView == UnitView.Debris && this.selectedSpawnCustomDebrisMesh != null)
						{
							p.MeshRenderer.BeginRendering(UnitMeshShaderType.Basic, p.MeshRenderPassType);
							p.MeshRenderer.RenderBasicMesh(this.selectedSpawnCustomDebrisMesh, this.SelectedSpawnCustomDebrisDefinition.Basis, base.Skybox, p.MeshRenderPassType);
							p.MeshRenderer.EndRendering();
							return;
						}
					}
					else if (this.SelectedUnitSkinStageDefinition.PlanetMesh != null)
					{
						p.MeshRenderer.BeginRendering(UnitMeshShaderType.Basic, p.MeshRenderPassType);
						p.MeshRenderer.RenderBasicMesh(this.SkinStageMesh, this.skinStageMeshBasis, base.Skybox, p.MeshRenderPassType);
						p.MeshRenderer.EndRendering();
					}
				}
			}, delegate(RenderScenePrim3dSimpleParams p)
			{
			}, delegate(RenderScenePrim3dComplexParams p)
			{
			}, delegate(RenderScenePrim3dLinesParams p)
			{
				if (this.SkinStageMesh != null)
				{
					if (this.settings.ShowWorldAxes)
					{
						float num = this.SkinStageMesh.Data.BoundingSphere.Radius * 2f;
						p.Prim3dLines.BeginRendering();
						p.Prim3dLines.RenderRotationAxes(new Float3(), new Float3x3(), new Float3(num, num, num), this.settings.WorldAxesLineThickness, 1f);
						p.Prim3dLines.EndRendering();
					}
					if (this.settings.ShowBoundingBox)
					{
						p.Prim3dLines.BeginRendering();
						p.Prim3dLines.RenderLineBox(this.SkinStageMesh.Data.BoundingBox, new Float3(), new Float3x3(), this.settings.BoundingBoxColor, 3f);
						p.Prim3dLines.EndRendering();
					}
					if (this.selectedUnitView == UnitView.Skin)
					{
						this.RenderMeshPointsOverlay(p);
						return;
					}
					if (this.selectedUnitView == UnitView.Weapons)
					{
						this.RenderWeaponMeshPointsOverlay(p);
						this.RenderSelectedWeapon3dOverlay_Prim3dLines(p);
						return;
					}
					if (this.selectedUnitView == UnitView.Carrier)
					{
						this.RenderHangarPoints3dOverlay(p);
						return;
					}
					if (this.selectedUnitView == UnitView.UnitFactory)
					{
						this.RenderUnitFactoryBuildPoint3dOverlay(p);
						return;
					}
					if (this.selectedUnitView == UnitView.Spatial)
					{
						this.RenderSpatialBoundingBox(p);
						return;
					}
					if (this.selectedUnitView == UnitView.Debris)
					{
						this.RenderSelectedSpawnCustomDebrisDefinition(p);
					}
				}
			}, delegate(RenderScenePrim3dShapesParams p)
			{
				if (this.SkinStageMesh != null)
				{
					if (this.settings.ShowBoundingSphere)
					{
						p.Prim3dShapes.BeginRendering(p.Pipeline, base.Camera);
						p.Prim3dShapes.RenderSphere(new Sphere(this.SkinStageMesh.Data.BoundingSphere.Center, this.SkinStageMesh.Data.BoundingSphere.Radius), this.settings.BoundingSphereColor);
						p.Prim3dShapes.EndRendering();
					}
					if (this.selectedUnitView != UnitView.Skin)
					{
						if (this.selectedUnitView == UnitView.Weapons)
						{
							this.RenderSelectedWeapon3dOverlay_Prim3dShapes(p);
							return;
						}
						if (this.selectedUnitView != UnitView.Carrier && this.selectedUnitView == UnitView.Spatial)
						{
							this.RenderSpatialBoundingSphere(p);
							this.RenderSpatialStructureBuildRadiusSphere(p);
						}
					}
				}
			}, delegate(Render2dSceneParams p)
			{
			});
		}


		private List<int> GetWeaponMeshPointIndices(WeaponInstanceDefinition weaponInstance, int? skinStageIndex)
		{
			List<int> result = new List<int>();
			if (this.TryGetWeaponDefinition(weaponInstance) != null)
			{
				SkinStageMeshTable skinStageMeshTable = this.TryGetSkinStageMeshTable(skinStageIndex);
				if (skinStageMeshTable != null && skinStageMeshTable.BaseMesh != null)
				{
					result = skinStageMeshTable.BaseMesh.GetMeshPointIndicesByName(weaponInstance.MeshPoint);
				}
				else if (this.showInvalidSkinStageIndexWarning)
				{
					MessageBox.Show(string.Format("Invalid SkinStageIndex in GetWeaponMeshPointIndices.\nweapon = {0}\nskin = {1}\nskinStageIndex = {2}", weaponInstance.Weapon.ToString(), this.selectedUnitSkinDefinition.ToString(), skinStageIndex), "Invalid SkinStageIndex", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
					this.showInvalidSkinStageIndexWarning = false;
				}
			}
			return result;
		}


		private List<MeshPoint> GetMeshPoints(MeshPointType meshPointType)
		{
			List<MeshPoint> list = new List<MeshPoint>();
			if (this.SkinStageMesh != null)
			{
				using (List<int>.Enumerator enumerator = this.SkinStageMesh.GetMeshPointIndicesByType(meshPointType).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						int index = enumerator.Current;
						list.Add(this.SkinStageMesh.Data.Points[index]);
					}
					return list;
				}
			}
			if (this.showInvalidSkinStageIndexWarning)
			{
				MessageBox.Show(string.Format("No Selected SkinStage for GetMeshPoints({0})", meshPointType), "Invalid SkinStageIndex", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
				this.showInvalidSkinStageIndexWarning = false;
			}
			return list;
		}


		private void RenderMeshPointsOverlay(RenderScenePrim3dLinesParams p)
		{
			p.Prim3dLines.BeginRendering();
			for (int i = 0; i < this.SkinStageMesh.Data.Points.Count; i++)
			{
				MeshPoint meshPoint = this.SkinStageMesh.Data.Points[i];
				bool flag = i == this.SelectedSkinStageMeshPointIndex;
				float meshPointAxisLength = this.GetMeshPointAxisLength(flag);
				float lineThickness = flag ? 3f : 1f;
				float lineAlpha = flag ? 1f : 0.5f;
				p.Prim3dLines.RenderRotationAxes(meshPoint.Position, meshPoint.Rotation, new Float3(meshPointAxisLength, meshPointAxisLength, meshPointAxisLength), lineThickness, lineAlpha);
			}
			p.Prim3dLines.EndRendering();
		}


		private float GetMeshPointAxisLength(bool selected)
		{
			float lengthOfLongestExtent = this.SkinStageMesh.Data.BoundingBox.GetLengthOfLongestExtent();
			float num = selected ? 2f : 1f;
			return MathFunctions.Lerp(25f, 50f, 100f, 1000f, lengthOfLongestExtent) * num;
		}


		private float GetWeaponAxisLength(WeaponInstanceDefinition weaponInstance, bool selected)
		{
			float result = this.GetMeshPointAxisLength(selected);
			WeaponDefinition weaponDefinition = this.TryGetWeaponDefinition(weaponInstance);
			if (weaponDefinition != null && (weaponDefinition.YawSpeedInDegrees <= 0f || weaponDefinition.PitchSpeedInDegrees <= 0f))
			{
				result = this.SkinStageMesh.Data.BoundingBox.GetLengthOfLongestExtent();
			}
			return result;
		}


		private void RenderWeaponMeshPointsOverlay(RenderScenePrim3dLinesParams p)
		{
			if (this.UnitDefinition != null && this.UnitDefinition.Weapons != null)
			{
				for (int i = 0; i < this.UnitDefinition.Weapons.WeaponInstances.Count; i++)
				{
					int num = i;
					int? num2 = this.SelectedWeaponInstanceIndex;
					bool flag = num == num2.GetValueOrDefault() & num2 != null;
					if (flag || this.settings.ShowAllWeaponPoints)
					{
						WeaponInstanceDefinition weaponInstance = this.UnitDefinition.Weapons.WeaponInstances[i];
						foreach (int index in this.GetWeaponMeshPointIndices(weaponInstance, this.selectedUnitSkinStageIndex))
						{
							MeshPoint meshPoint = this.SkinStageMesh.Data.Points[index];
							float weaponAxisLength = this.GetWeaponAxisLength(weaponInstance, flag);
							float lineThickness = flag ? 3f : 1f;
							float lineAlpha = flag ? 1f : 0.5f;
							p.Prim3dLines.BeginRendering();
							p.Prim3dLines.RenderRotationAxes(meshPoint.Position, meshPoint.Rotation, new Float3(weaponAxisLength, weaponAxisLength, weaponAxisLength), lineThickness, lineAlpha);
							p.Prim3dLines.EndRendering();
						}
					}
				}
			}
		}


		private void RenderSelectedSpawnCustomDebrisDefinition(RenderScenePrim3dLinesParams p)
		{
			SpawnCustomDebrisDefinition selectedSpawnCustomDebrisDefinition = this.SelectedSpawnCustomDebrisDefinition;
			if (selectedSpawnCustomDebrisDefinition != null)
			{
				float meshPointAxisLength = this.GetMeshPointAxisLength(true);
				float lineThickness = 3f;
				float lineAlpha = 1f;
				p.Prim3dLines.BeginRendering();
				p.Prim3dLines.RenderRotationAxes(selectedSpawnCustomDebrisDefinition.Basis.Position, selectedSpawnCustomDebrisDefinition.Basis.Rotation, new Float3(meshPointAxisLength, meshPointAxisLength, meshPointAxisLength), lineThickness, lineAlpha);
				p.Prim3dLines.EndRendering();
			}
		}


		private void RenderSelectedWeapon3dOverlay_Prim3dLines(RenderScenePrim3dLinesParams p)
		{
			if (this.UnitDefinition != null && this.UnitDefinition.Weapons != null)
			{
				for (int i = 0; i < this.UnitDefinition.Weapons.WeaponInstances.Count; i++)
				{
					int num = i;
					int? num2 = this.SelectedWeaponInstanceIndex;
					bool flag = num == num2.GetValueOrDefault() & num2 != null;
					if (flag || this.settings.ShowAllWeaponPoints)
					{
						WeaponInstanceDefinition weaponInstanceDefinition = this.UnitDefinition.Weapons.WeaponInstances[i];
						float weaponAxisLength = this.GetWeaponAxisLength(weaponInstanceDefinition, flag);
						float lineThickness = flag ? 3f : 1f;
						float lineAlpha = flag ? 1f : 0.5f;
						float num3 = flag ? 3f : 1f;
						int alpha = flag ? 255 : 191;
						float arcRadius = weaponAxisLength;
						Color color = Color.FromArgb(alpha, Color.BlueViolet);
						Color color2 = Color.FromArgb(alpha, Color.GreenYellow);
						this.RenderWeaponArc(p, weaponInstanceDefinition, weaponInstanceDefinition.YawArc, Float3x3.NewForwardUp(weaponInstanceDefinition.Rotation.Forward, weaponInstanceDefinition.Rotation.Up), color, color, num3, arcRadius);
						this.RenderWeaponArc(p, weaponInstanceDefinition, weaponInstanceDefinition.PitchArc, Float3x3.NewForwardUp(weaponInstanceDefinition.Rotation.Forward, weaponInstanceDefinition.Rotation.Right), color2, color2, num3, arcRadius);
						WeaponDefinition weaponDefinition = this.TryGetWeaponDefinition(weaponInstanceDefinition);
						if (weaponDefinition != null)
						{
							Color color3 = Color.FromArgb(alpha, Color.Orange);
							Color color4 = Color.FromArgb(alpha, Color.HotPink);
							float arcThickness = num3 + 2f;
							if (weaponDefinition.YawFiringToleranceArc.MaxAngle > 0f && weaponDefinition.YawFiringToleranceArc.MinAngle < 0f)
							{
								this.RenderWeaponArc(p, weaponInstanceDefinition, weaponDefinition.YawFiringToleranceArc, Float3x3.NewForwardUp(weaponInstanceDefinition.Rotation.Forward, weaponInstanceDefinition.Rotation.Up), color3, color3, arcThickness, arcRadius);
							}
							if (weaponDefinition.PitchFiringToleranceArc.MaxAngle > 0f && weaponDefinition.PitchFiringToleranceArc.MinAngle < 0f)
							{
								this.RenderWeaponArc(p, weaponInstanceDefinition, weaponDefinition.PitchFiringToleranceArc, Float3x3.NewForwardUp(weaponInstanceDefinition.Rotation.Forward, weaponInstanceDefinition.Rotation.Right), color4, color4, arcThickness, arcRadius);
							}
						}
						p.Prim3dLines.BeginRendering();
						p.Prim3dLines.RenderRotationAxes(weaponInstanceDefinition.WeaponPosition, weaponInstanceDefinition.Rotation, new Float3(weaponAxisLength, weaponAxisLength, weaponAxisLength), lineThickness, lineAlpha);
						p.Prim3dLines.EndRendering();
					}
				}
			}
		}


		private void RenderSelectedWeapon3dOverlay_Prim3dShapes(RenderScenePrim3dShapesParams p)
		{
			if (this.UnitDefinition != null && this.UnitDefinition.Weapons != null)
			{
				p.Prim3dShapes.BeginRendering(p.Pipeline, base.Camera);
				for (int i = 0; i < this.UnitDefinition.Weapons.WeaponInstances.Count; i++)
				{
					int num = i;
					int? num2 = this.SelectedWeaponInstanceIndex;
					bool flag = num == num2.GetValueOrDefault() & num2 != null;
					if (flag || this.settings.ShowAllWeaponPoints)
					{
						WeaponInstanceDefinition weaponInstance = this.UnitDefinition.Weapons.WeaponInstances[i];
						this.RenderSelectedWeaponInstance3dOverlay_Prim3dShapes(p, weaponInstance, flag);
					}
				}
				p.Prim3dShapes.EndRendering();
			}
		}


		private void RenderSelectedWeaponInstance3dOverlay_Prim3dShapes(RenderScenePrim3dShapesParams p, WeaponInstanceDefinition weaponInstance, bool isSelected)
		{
			float num = this.SkinStageMesh.Data.BoundingSphere.Radius / 100f;
			float radius = (isSelected ? 0.5f : 0.25f) * num;
			int alpha = isSelected ? 191 : 63;
			Color color = Color.FromArgb(alpha, Color.White);
			Color color2 = Color.FromArgb(alpha, Color.Blue);
			Color color3 = Color.FromArgb(alpha, Color.Green);
			Color color4 = Color.FromArgb(alpha, Color.Red);
			p.Prim3dShapes.RenderSphere(new Sphere(weaponInstance.WeaponPosition, radius), color);
			List<int> weaponMeshPointIndices = this.GetWeaponMeshPointIndices(weaponInstance, this.selectedUnitSkinStageIndex);
			if (weaponMeshPointIndices.Count<int>() > 0 && this.SkinStageMesh != null)
			{
				WeaponDefinition weaponDefinition = this.TryGetWeaponDefinition(weaponInstance);
				if (weaponDefinition != null)
				{
					if (weaponDefinition.TurretType == WeaponTurretType.None)
					{
						for (int i = 0; i < weaponMeshPointIndices.Count<int>(); i++)
						{
							MeshPoint meshPoint = this.SkinStageMesh.Data.Points[weaponMeshPointIndices[i]];
							Basis basis = weaponInstance.MakeNonTurretMuzzleBasis(this.skinStageMeshBasis, meshPoint.Position, 0f, 0f);
							p.Prim3dShapes.RenderSphere(new Sphere(basis.Position, radius), color4);
						}
					}
					else
					{
						TurretMeshes turretMeshes = this.TryGetTurretMeshes(weaponInstance, this.selectedUnitSkinStageIndex);
						if (turretMeshes != null)
						{
							if (weaponMeshPointIndices.Count<int>() > 1 && this.showMultipleMeshPointsWarning)
							{
								MessageBox.Show("Multiple mesh points found for turreted weapon. Only the first will be used. weapon = " + weaponDefinition.ToString() + "\ninstance mesh_point = " + weaponMeshPointIndices[0].ToString(), "Turret with multiple mesh points", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
								this.showMultipleMeshPointsWarning = false;
							}
							MeshPoint meshPoint2 = this.SkinStageMesh.Data.Points[weaponMeshPointIndices[0]];
							if (weaponDefinition.TurretType == WeaponTurretType.Gimbal)
							{
								Basis basis2 = weaponInstance.MakeGimbalTurretBaseBasis(this.skinStageMeshBasis, meshPoint2.Position, 0f, 0f);
								if (basis2 != null)
								{
									p.Prim3dShapes.RenderSphere(new Sphere(basis2.Position, radius), color2);
									if (turretMeshes.GimbalMesh != null)
									{
										MeshPoint meshPoint3 = turretMeshes.GimbalMesh.Data.TryFindFirstMeshPointOfType(MeshPointType.TurretMuzzle);
										if (meshPoint3 != null)
										{
											Basis basis3 = weaponInstance.MakeGimbalTurretMuzzleBasis(this.skinStageMeshBasis, basis2, meshPoint3.Position);
											p.Prim3dShapes.RenderSphere(new Sphere(basis3.Position, radius), color4);
										}
									}
								}
							}
							else if (weaponDefinition.TurretType == WeaponTurretType.Biaxial)
							{
								Basis basis4 = weaponInstance.MakeBiaxialTurretBaseBasis(this.skinStageMeshBasis, meshPoint2.Position, 0f, 0f);
								p.Prim3dShapes.RenderSphere(new Sphere(basis4.Position, radius), color2);
								if (turretMeshes.BiaxialBaseMesh != null)
								{
									MeshPoint meshPoint4 = turretMeshes.BiaxialBaseMesh.Data.TryFindFirstMeshPointOfType(MeshPointType.Child);
									if (meshPoint4 != null)
									{
										Basis basis5 = weaponInstance.MakeBiaxialTurretBarrelBasis(this.skinStageMeshBasis, basis4, meshPoint4.Position, 0f, 0f);
										p.Prim3dShapes.RenderSphere(new Sphere(basis5.Position, radius), color3);
										if (turretMeshes.BiaxialBarrelMesh != null)
										{
											foreach (MeshPoint meshPoint5 in turretMeshes.BiaxialBarrelMesh.Data.GetAllMeshPointsOfType(MeshPointType.TurretMuzzle))
											{
												Basis basis6 = weaponInstance.MakeBiaxialTurretMuzzleBasis(this.skinStageMeshBasis, basis5, meshPoint5.Position);
												p.Prim3dShapes.RenderSphere(new Sphere(basis6.Position, radius), color4);
											}
										}
									}
								}
							}
						}
					}
				}
			}
			if (this.settings.ShowAllWeaponPoints && isSelected)
			{
				float weaponAxisLength = this.GetWeaponAxisLength(weaponInstance, isSelected);
				Color color5 = Color.FromArgb(12, Color.White);
				p.Prim3dShapes.RenderSphere(new Sphere(weaponInstance.WeaponPosition, weaponAxisLength), color5);
			}
		}


		private TurretMeshes TryGetTurretMeshes(WeaponInstanceDefinition weaponInstance, int? skinStageIndex)
		{
			TurretMeshes result = null;
			SkinStageMeshTable skinStageMeshTable = this.TryGetSkinStageMeshTable(skinStageIndex);
			if (skinStageMeshTable != null)
			{
				skinStageMeshTable.WeaponNameToTurretMeshesMap.TryGetValue(weaponInstance.Weapon, out result);
			}
			else if (this.showInvalidSkinStageIndexWarning)
			{
				MessageBox.Show(string.Format("Invalid SkinStageIndex in TryGetTurretMeshes.\nweapon = {0}\nskin = {1}\nskinStageIndex = {2}", weaponInstance.Weapon.ToString(), this.selectedUnitSkinDefinition.ToString(), skinStageIndex), "Invalid SkinStageIndex", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
				this.showInvalidSkinStageIndexWarning = false;
			}
			return result;
		}


		private void TryRenderWeaponInstanceMesh(RenderSceneMeshesParams p, Mesh mesh, Basis basis)
		{
			if (mesh != null)
			{
				if (this.SelectedUnitSkinStageDefinition.UnitMesh.Shader == UnitMeshShaderType.Basic)
				{
					p.MeshRenderer.BeginRendering(UnitMeshShaderType.Basic, p.MeshRenderPassType);
					p.MeshRenderer.RenderBasicMesh(mesh, basis, base.Skybox, p.MeshRenderPassType);
					p.MeshRenderer.EndRendering();
					return;
				}
				if (this.SelectedUnitSkinStageDefinition.UnitMesh.Shader == UnitMeshShaderType.Ship)
				{
					p.MeshRenderer.BeginRendering(UnitMeshShaderType.Ship, p.MeshRenderPassType);
					p.MeshRenderer.RenderShipMesh(mesh, basis, base.Skybox, p.MeshRenderPassType);
					p.MeshRenderer.EndRendering();
				}
			}
		}


		private void RenderWeaponInstanceMeshes(RenderSceneMeshesParams p, WeaponInstanceDefinition weaponInstance, WeaponDefinition weapon, TurretMeshes turretMeshes, MeshPoint meshPoint)
		{
			if (weapon.TurretType == WeaponTurretType.Gimbal)
			{
				Basis basis = weaponInstance.MakeGimbalTurretBaseBasis(this.skinStageMeshBasis, meshPoint.Position, 0f, 0f);
				if (basis != null)
				{
					this.TryRenderWeaponInstanceMesh(p, turretMeshes.GimbalMesh, basis);
					return;
				}
			}
			else if (weapon.TurretType == WeaponTurretType.Biaxial)
			{
				Basis basis2 = weaponInstance.MakeBiaxialTurretBaseBasis(this.skinStageMeshBasis, meshPoint.Position, 0f, 0f);
				if (basis2 != null)
				{
					this.TryRenderWeaponInstanceMesh(p, turretMeshes.BiaxialBaseMesh, basis2);
					if (turretMeshes.BiaxialBaseMesh != null)
					{
						MeshPoint meshPoint2 = turretMeshes.BiaxialBaseMesh.Data.TryFindFirstMeshPointOfType(MeshPointType.Child);
						if (meshPoint2 != null)
						{
							Basis basis3 = weaponInstance.MakeBiaxialTurretBarrelBasis(this.skinStageMeshBasis, basis2, meshPoint2.Position, 0f, 0f);
							this.TryRenderWeaponInstanceMesh(p, turretMeshes.BiaxialBarrelMesh, basis3);
						}
					}
				}
			}
		}


		private void RenderWeaponMeshes(RenderSceneMeshesParams p)
		{
			if (this.UnitDefinition != null && this.UnitDefinition.Weapons != null)
			{
				foreach (WeaponInstanceDefinition weaponInstance in this.UnitDefinition.Weapons.WeaponInstances)
				{
					WeaponDefinition weaponDefinition = this.TryGetWeaponDefinition(weaponInstance);
					if (weaponDefinition != null)
					{
						TurretMeshes turretMeshes = this.TryGetTurretMeshes(weaponInstance, this.selectedUnitSkinStageIndex);
						if (turretMeshes != null)
						{
							List<int> weaponMeshPointIndices = this.GetWeaponMeshPointIndices(weaponInstance, this.selectedUnitSkinStageIndex);
							if (weaponMeshPointIndices.Count<int>() > 0 && this.SkinStageMesh != null)
							{
								MeshPoint meshPoint = this.SkinStageMesh.Data.Points[weaponMeshPointIndices[0]];
								this.RenderWeaponInstanceMeshes(p, weaponInstance, weaponDefinition, turretMeshes, meshPoint);
							}
						}
					}
				}
			}
		}


		private void RenderChildMeshes(RenderSceneMeshesParams p)
		{
			if (this.UnitDefinition != null && this.SkinStageMesh != null)
			{
				for (int i = 0; i < this.UnitDefinition.ChildMeshes.Count; i++)
				{
					UnitChildMeshDefinition unitChildMeshDefinition = this.UnitDefinition.ChildMeshes[i];
					List<int> meshPointIndicesByName = this.SkinStageMesh.GetMeshPointIndicesByName(unitChildMeshDefinition.MeshPoint);
					if (meshPointIndicesByName.Count<int>() > 0)
					{
						MeshPoint meshPoint = this.SkinStageMesh.Data.Points[meshPointIndicesByName[0]];
						SkinStageMeshTable skinStageMeshTable = this.TryGetSkinStageMeshTable(this.SelectedUnitSkinStageIndex);
						if (skinStageMeshTable != null)
						{
							Mesh mesh = skinStageMeshTable.ChildMeshes[i];
							if (mesh != null)
							{
								if (this.SelectedUnitSkinStageDefinition.UnitMesh.Shader == UnitMeshShaderType.Basic)
								{
									p.MeshRenderer.BeginRendering(UnitMeshShaderType.Basic, p.MeshRenderPassType);
									p.MeshRenderer.RenderBasicMesh(mesh, meshPoint.Basis, base.Skybox, p.MeshRenderPassType);
									p.MeshRenderer.EndRendering();
								}
								else if (this.SelectedUnitSkinStageDefinition.UnitMesh.Shader == UnitMeshShaderType.Ship)
								{
									p.MeshRenderer.BeginRendering(UnitMeshShaderType.Ship, p.MeshRenderPassType);
									p.MeshRenderer.RenderShipMesh(mesh, meshPoint.Basis, base.Skybox, p.MeshRenderPassType);
									p.MeshRenderer.EndRendering();
								}
							}
						}
					}
				}
			}
		}


		public void SyncSelectedWeaponToMesh()
		{
			this.SyncWeaponToMesh(this.SelectedWeaponInstance);
			UnitModel.SelectedWeaponInstanceChangedDelegate selectedWeaponInstanceChanged = this.SelectedWeaponInstanceChanged;
			if (selectedWeaponInstanceChanged == null)
			{
				return;
			}
			selectedWeaponInstanceChanged(this.SelectedWeaponInstance);
		}


		public void SyncAllWeaponsToMesh()
		{
			if (this.UnitDefinition != null && this.UnitDefinition.Weapons != null)
			{
				foreach (WeaponInstanceDefinition weaponInstance in this.UnitDefinition.Weapons.WeaponInstances)
				{
					this.SyncWeaponToMesh(weaponInstance);
				}
				UnitModel.SelectedWeaponInstanceChangedDelegate selectedWeaponInstanceChanged = this.SelectedWeaponInstanceChanged;
				if (selectedWeaponInstanceChanged == null)
				{
					return;
				}
				selectedWeaponInstanceChanged(this.SelectedWeaponInstance);
			}
		}


		public void SyncWeaponToMesh(WeaponInstanceDefinition weaponInstance)
		{
			if (weaponInstance != null)
			{
				int minRequiredStageIndex = weaponInstance.MinRequiredStageIndex;
				SkinStageMeshTable skinStageMeshTable = this.TryGetSkinStageMeshTable(new int?(minRequiredStageIndex));
				if (skinStageMeshTable == null || skinStageMeshTable.BaseMesh == null)
				{
					MessageBox.Show(string.Format("Invalid minRequiredStageIndex : {0}", minRequiredStageIndex), "Invalid minRequiredStageIndex", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
					return;
				}
				List<int> weaponMeshPointIndices = this.GetWeaponMeshPointIndices(weaponInstance, new int?(minRequiredStageIndex));
				if (weaponMeshPointIndices.Count == 0)
				{
					if (this.showMissingWeaponMeshPointsWarning)
					{
						MessageBox.Show(string.Format("No Mesh Points found for Weapon Instance.\nweapon = {0}\nskin = {1}\nskinStageIndex = {2}", weaponInstance.Weapon.ToString(), this.selectedUnitSkinDefinition.ToString(), minRequiredStageIndex), "Missing Weapon Mesh Points", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
						this.showMissingWeaponMeshPointsWarning = false;
					}
					return;
				}
				Float3 a = new Float3(0f, 0f, 0f);
				for (int i = 0; i < weaponMeshPointIndices.Count<int>(); i++)
				{
					a += skinStageMeshTable.BaseMesh.Data.Points[weaponMeshPointIndices[i]].Position;
				}
				weaponInstance.WeaponPosition = a / (float)weaponMeshPointIndices.Count<int>();
				if (weaponMeshPointIndices.Count > 0)
				{
					MeshPoint meshPoint = skinStageMeshTable.BaseMesh.Data.Points[weaponMeshPointIndices[0]];
					weaponInstance.Forward = meshPoint.Basis.Rotation.Forward;
					weaponInstance.Up = meshPoint.Basis.Rotation.Up;
				}
				weaponInstance.ClearNonTurretMuzzlePositions();
				WeaponDefinition weaponDefinition = this.TryGetWeaponDefinition(weaponInstance);
				if (weaponDefinition != null)
				{
					weaponDefinition.ClearMuzzlePositions();
					if (weaponDefinition.TurretType == WeaponTurretType.None)
					{
						for (int j = 0; j < weaponMeshPointIndices.Count<int>(); j++)
						{
							MeshPoint meshPoint2 = skinStageMeshTable.BaseMesh.Data.Points[weaponMeshPointIndices[j]];
							weaponInstance.AddNonTurretMuzzlePosition(meshPoint2.Position);
						}
						return;
					}
					TurretMeshes turretMeshes = this.TryGetTurretMeshes(weaponInstance, new int?(minRequiredStageIndex));
					if (turretMeshes != null)
					{
						if (weaponMeshPointIndices.Count<int>() > 1 && this.showMultipleMeshPointsWarning)
						{
							MessageBox.Show(string.Concat(new string[]
							{
								"Multiple mesh points found for turreted weapon. Only the first will be used.\nweapon = ",
								weaponDefinition.ToString(),
								"\nskin = ",
								this.selectedUnitSkinDefinition.ToString(),
								"\ninstance mesh_point = ",
								weaponMeshPointIndices[0].ToString()
							}), "Turret with multiple mesh points", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
							this.showMultipleMeshPointsWarning = false;
						}
						if (weaponDefinition.TurretType == WeaponTurretType.Gimbal)
						{
							if (turretMeshes.GimbalMesh != null)
							{
								MeshPoint meshPoint3 = turretMeshes.GimbalMesh.Data.TryFindFirstMeshPointOfType(MeshPointType.TurretMuzzle);
								if (meshPoint3 != null)
								{
									weaponDefinition.AddMuzzlePosition(meshPoint3.Position);
									return;
								}
							}
							else if (this.showMissingTurretMeshWarning)
							{
								MessageBox.Show(string.Concat(new string[]
								{
									"Missing Gimbal mesh for turreted weapon.\nweapon = ",
									weaponDefinition.ToString(),
									"\nskin = ",
									this.selectedUnitSkinDefinition.ToString(),
									"\ninstance mesh_point = ",
									weaponMeshPointIndices[0].ToString()
								}), "Turret with missing Gimbal Mesh", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
								this.showMissingTurretMeshWarning = false;
								return;
							}
						}
						else if (weaponDefinition.TurretType == WeaponTurretType.Biaxial)
						{
							if (turretMeshes.BiaxialBaseMesh != null && turretMeshes.BiaxialBarrelMesh != null)
							{
								MeshPoint meshPoint4 = turretMeshes.BiaxialBaseMesh.Data.TryFindFirstMeshPointOfType(MeshPointType.Child);
								if (meshPoint4 == null)
								{
									return;
								}
								weaponDefinition.TurretBarrelPosition = meshPoint4.Position;
								using (List<MeshPoint>.Enumerator enumerator = turretMeshes.BiaxialBarrelMesh.Data.GetAllMeshPointsOfType(MeshPointType.TurretMuzzle).GetEnumerator())
								{
									while (enumerator.MoveNext())
									{
										MeshPoint meshPoint5 = enumerator.Current;
										weaponDefinition.AddMuzzlePosition(meshPoint5.Position);
									}
									return;
								}
							}
							if (this.showMissingTurretMeshWarning)
							{
								MessageBox.Show(string.Concat(new string[]
								{
									"Missing Biaxial mesh for turreted weapon.\nweapon = ",
									weaponDefinition.ToString(),
									"\nskin = ",
									this.selectedUnitSkinDefinition.ToString(),
									"\ninstance mesh_point = ",
									weaponMeshPointIndices[0].ToString()
								}), "Turret with missing Biaxial Mesh", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
								this.showMissingTurretMeshWarning = false;
							}
						}
					}
				}
			}
		}


		public void SyncSpatialPropertiesToMesh()
		{
			if (this.UnitDefinition != null && this.SkinStageMesh != null)
			{
				this.UnitDefinition.Spatial.BoundingBox = this.SkinStageMesh.Data.BoundingBox;
				this.UnitDefinition.Spatial.Radius = this.SkinStageMesh.Data.BoundingSphere.Radius;
				if (this.UnitDefinition.Planet != null || this.UnitDefinition.TargetFilterUnitType == UnitTargetFilterUnitType.Star)
				{
					this.UnitDefinition.Spatial.LineOfSightRadius = this.UnitDefinition.Spatial.Radius;
					this.UnitDefinition.Spatial.SurfaceRadius = this.UnitDefinition.Spatial.Radius;
					this.UnitDefinition.Spatial.Radius *= 1.5f;
				}
				if (this.UnitDefinition.IsStructure)
				{
					this.UnitDefinition.Spatial.StructureBuildRadius = new float?(this.UnitDefinition.Spatial.BoundingBox.GetBoundingRadiusXZ());
				}
				UnitModel.UnitDefinitionChangedDelegate unitDefinitionChanged = this.UnitDefinitionChanged;
				if (unitDefinitionChanged == null)
				{
					return;
				}
				unitDefinitionChanged(this.unitDefinition);
			}
		}


		public void SyncHangarPointsToMesh()
		{
			if (this.UnitDefinition != null && this.SkinStageMesh != null)
			{
				List<MeshPoint> meshPoints = this.GetMeshPoints(MeshPointType.Hangar);
				if (meshPoints.Count<MeshPoint>() > 0)
				{
					this.UnitDefinition.Carrier.RemoveAllHangerPoints();
					foreach (MeshPoint meshPoint in meshPoints)
					{
						this.UnitDefinition.Carrier.AddHangerPoint(new Basis(meshPoint.Position, meshPoint.Rotation));
					}
					UnitModel.UnitDefinitionChangedDelegate unitDefinitionChanged = this.UnitDefinitionChanged;
					if (unitDefinitionChanged == null)
					{
						return;
					}
					unitDefinitionChanged(this.unitDefinition);
				}
			}
		}


		public void SyncUnitFactoryBuildPointToMesh()
		{
			if (this.UnitDefinition != null && this.UnitDefinition.UnitFactory != null && this.SkinStageMesh != null)
			{
				List<MeshPoint> meshPoints = this.GetMeshPoints(MeshPointType.ShipBuild);
				if (meshPoints.Count<MeshPoint>() == 0)
				{
					meshPoints = this.GetMeshPoints(MeshPointType.Center);
				}
				if (meshPoints.Count<MeshPoint>() == 0)
				{
					MessageBox.Show("No MeshPoints (ship_build,center) found for Unit Factory Build Point : " + this.SkinStageMesh.LoadedPath, "No Mesh Points", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				this.UnitDefinition.UnitFactory.BaseBuildPoint = new Basis(meshPoints[0].Position, meshPoints[0].Rotation);
				UnitModel.UnitDefinitionChangedDelegate unitDefinitionChanged = this.UnitDefinitionChanged;
				if (unitDefinitionChanged == null)
				{
					return;
				}
				unitDefinitionChanged(this.unitDefinition);
			}
		}


		private void RenderSpatialBoundingBox(RenderScenePrim3dLinesParams p)
		{
			Color color = Color.FromArgb(255, Color.Red);
			int num = 2;
			p.Prim3dLines.BeginRendering();
			p.Prim3dLines.RenderLineBox(this.UnitDefinition.Spatial.BoundingBox, new Float3(0f, 0f, 0f), new Float3x3(), color, (float)num);
			p.Prim3dLines.EndRendering();
		}


		private void RenderSpatialBoundingSphere(RenderScenePrim3dShapesParams p)
		{
			Color color = Color.FromArgb(64, Color.White);
			p.Prim3dShapes.BeginRendering(p.Pipeline, base.Camera);
			p.Prim3dShapes.RenderSphere(new Sphere(new Float3(0f, 0f, 0f), this.UnitDefinition.Spatial.Radius), color);
			p.Prim3dShapes.EndRendering();
		}


		private void RenderSpatialStructureBuildRadiusSphere(RenderScenePrim3dShapesParams p)
		{
			if (this.UnitDefinition.Spatial.StructureBuildRadius != null)
			{
				Color color = Color.FromArgb(64, Color.Purple);
				p.Prim3dShapes.BeginRendering(p.Pipeline, base.Camera);
				p.Prim3dShapes.RenderSphere(new Sphere(new Float3(0f, 0f, 0f), this.UnitDefinition.Spatial.StructureBuildRadius.Value), color);
				p.Prim3dShapes.EndRendering();
			}
		}


		private void RenderHangarPoints3dOverlay(RenderScenePrim3dLinesParams p)
		{
			if (this.UnitDefinition != null && this.UnitDefinition.Carrier != null)
			{
				foreach (Basis basis in this.UnitDefinition.Carrier.HangarPoints)
				{
					float meshPointAxisLength = this.GetMeshPointAxisLength(false);
					float lineThickness = 2f;
					float lineAlpha = 1f;
					p.Prim3dLines.BeginRendering();
					p.Prim3dLines.RenderRotationAxes(basis.Position, basis.Rotation, new Float3(meshPointAxisLength, meshPointAxisLength, meshPointAxisLength), lineThickness, lineAlpha);
					p.Prim3dLines.EndRendering();
				}
			}
		}


		private void RenderWeaponArc(RenderScenePrim3dLinesParams p, WeaponInstanceDefinition weapon, WeaponArcDefinition arc, Float3x3 rotation, Color beginColor, Color endColor, float arcThickness, float arcRadius)
		{
			p.Prim3dLines.BeginRendering();
			p.Prim3dLines.RenderLineArc(weapon.WeaponPosition, rotation, arcRadius, beginColor, endColor, arcThickness, 100, arc.MinAngle, arc.MaxAngle, null);
			p.Prim3dLines.EndRendering();
		}


		private void RenderUnitFactoryBuildPoint3dOverlay(RenderScenePrim3dLinesParams p)
		{
			if (this.UnitDefinition != null && this.UnitDefinition.UnitFactory != null)
			{
				float meshPointAxisLength = this.GetMeshPointAxisLength(false);
				float lineThickness = 2f;
				float lineAlpha = 1f;
				p.Prim3dLines.BeginRendering();
				p.Prim3dLines.RenderRotationAxes(this.UnitDefinition.UnitFactory.BaseBuildPoint.Position, this.UnitDefinition.UnitFactory.BaseBuildPoint.Rotation, new Float3(meshPointAxisLength, meshPointAxisLength, meshPointAxisLength), lineThickness, lineAlpha);
				p.Prim3dLines.EndRendering();
			}
		}


		private UnitSettings settings;


		private IDataSourcePathResolver dataSourcePathResolver;


		private IJsonBeautifier jsonBeautifier;


		private List<SkinStageMeshTable> skinStageMeshTables;


		private int selectedSkinStageMeshPointIndex;


		private Basis skinStageMeshBasis;


		private UnitDefinition unitDefinition;


		private List<UnitSkinDefinition> unitSkinDefinitions;


		private UnitSkinDefinition selectedUnitSkinDefinition;


		private int? selectedUnitSkinStageIndex;


		private int? selectedWeaponInstanceIndex;


		private int? selectedSpawnCustomDebrisDefinitionIndex;


		private Mesh selectedSpawnCustomDebrisMesh;


		private Dictionary<WeaponDefinitionName, WeaponDefinition> weaponDefinitions;


		private UnitView selectedUnitView;


		private bool showMultipleMeshPointsWarning = true;


		private bool showInvalidSkinStageIndexWarning = true;


		private bool showMissingTurretMeshWarning = true;


		private bool showMissingWeaponMeshPointsWarning = true;


		private bool enable_changes_skinStageMeshes = true;



		public delegate void UnitDefinitionChangedDelegate(UnitDefinition unitDefinitino);



		public delegate void UnitSkinDefinitionsChangedDelegate(IEnumerable<UnitSkinDefinition> unitSkinDefinitions);



		public delegate void SelectedUnitSkinDefinitionChangedDelegate(UnitSkinDefinition selectedUnitSkinDefinition);



		public delegate void SelectedUnitSkinStageDefinitionChangedDelegate(UnitSkinStageDefinition selectedUnitSkinStageDefinition);



		public delegate void SelectedWeaponInstanceChangedDelegate(WeaponInstanceDefinition weaponInstanceDefinition);



		public delegate void SelectedSpawnCustomDebrisDefinitionChangedDelegate(SpawnCustomDebrisDefinition definition);



		public delegate void SkinStageMeshChangedDelegate(Mesh skinStageMesh);
	}
}
