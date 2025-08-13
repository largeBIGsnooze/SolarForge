using System;
using System.Collections.Generic;
using System.IO;
using Solar;
using Solar.Math;
using Solar.Rendering;
using Solar.Simulations;
using SolarForge.Scenes;
using SolarForge.Utility;

namespace SolarForge.ParticleEffects
{

	public class ParticleEffectModel : SceneModel3d, IDisposable
	{



		public event ParticleEffectModel.ParticleEffectChangedDelegate ParticleEffectChanged;




		public event ParticleEffectModel.ParticleEffectComponentsChangedDelegate ParticleEffectComponentsChanged;




		public event ParticleEffectModel.ParticleEffectPropertyValueChangedDelegate ParticleEffectPropertyValueChanged;




		public event ParticleEffectModel.SelectedParticleEffectComponentChangedDelegate SelectedParticleEffectComponentChanged;




		public event ParticleEffectModel.MeshChangedDelegate MeshChanged;




		public event ParticleEffectModel.MeshPointIndexChangedDelegate MeshPointIndexChanged;




		public event ParticleEffectModel.UndoChangedDelgate UndoChanged;




		public event ParticleEffectModel.HasUnsavedChangesChangedDelegate HasUnsavedChangesChanged;


		public ParticleEffectModel(Engine engine, PostProcessingParams postProcessingParams, CameraSettings cameraSettings, ParticleEffectSettings modelSettings, IJsonBeautifier jsonBeautifier) : base(engine, postProcessingParams, cameraSettings, modelSettings)
		{
			this.settings = modelSettings;
			this.jsonBeautifier = jsonBeautifier;
			this.undoStack = new List<ParticleEffectDefinition>();
		}


		public override void Dispose()
		{
			this.DisposeParticleEffect(false);
			this.DisposeMesh();
		}


		private void DisposeParticleEffect(bool notifyDelegates)
		{
			if (this.particleEffect != null)
			{
				this.particleEffect.Dispose();
				this.particleEffect = null;
				if (notifyDelegates)
				{
					this.OnParticleEffectChanged();
				}
			}
		}


		private void ResetUndo()
		{
			this.undoStack.Clear();
			this.undoPosition = null;
			this.nextUndo = this.particleEffect.Definition.MakeClone();
			ParticleEffectModel.UndoChangedDelgate undoChanged = this.UndoChanged;
			if (undoChanged == null)
			{
				return;
			}
			undoChanged();
		}


		private void AddToUndoStack()
		{
			if (this.undoPosition != null)
			{
				this.undoStack.RemoveRange(this.undoPosition.Value, this.undoStack.Count - this.undoPosition.Value);
				this.undoPosition = null;
			}
			this.undoStack.Add(this.nextUndo);
			this.nextUndo = this.particleEffect.Definition.MakeClone();
			ParticleEffectModel.UndoChangedDelgate undoChanged = this.UndoChanged;
			if (undoChanged != null)
			{
				undoChanged();
			}
			this.HasUnsavedChanges = true;
		}



		public bool CanUndo
		{
			get
			{
				return this.undoStack.Count > 0 && (this.undoPosition == null || this.undoPosition.Value > 0);
			}
		}


		public void TryUndo()
		{
			if (this.CanUndo)
			{
				if (this.undoPosition == null)
				{
					this.undoPosition = new int?(this.undoStack.Count - 1);
					this.undoStack.Add(this.nextUndo);
				}
				else
				{
					this.undoPosition = new int?(this.undoPosition.Value - 1);
				}
				this.SyncToUndoPosition();
			}
		}



		public bool CanRedo
		{
			get
			{
				return this.undoPosition != null && this.undoPosition.Value < this.undoStack.Count - 1;
			}
		}


		public void TryRedo()
		{
			if (this.CanRedo)
			{
				this.undoPosition = new int?(this.undoPosition.Value + 1);
				this.SyncToUndoPosition();
			}
		}


		private void SyncToUndoPosition()
		{
			this.particleEffect.Definition.ApplyCopy(this.undoStack[this.undoPosition.Value]);
			this.nextUndo = this.undoStack[this.undoPosition.Value];
			this.RestartParticleEffect();
			this.OnParticleEffectComponentsChanged();
			ParticleEffectModel.UndoChangedDelgate undoChanged = this.UndoChanged;
			if (undoChanged != null)
			{
				undoChanged();
			}
			this.HasUnsavedChanges = true;
		}


		public void HandleParticleEffectPropertyValueChanged(string label)
		{
			this.AddToUndoStack();
			this.ParticleEffectSystem.HandleParticleEffectPropertyValueChanged(this.SoloEmitterId);
			ParticleEffectModel.ParticleEffectPropertyValueChangedDelegate particleEffectPropertyValueChanged = this.ParticleEffectPropertyValueChanged;
			if (particleEffectPropertyValueChanged == null)
			{
				return;
			}
			particleEffectPropertyValueChanged(label);
		}


		public void HandleParticleModifierTypeChanged()
		{
			this.AddToUndoStack();
		}


		public void HandleParticleEmitterTypeChanged()
		{
			this.AddToUndoStack();
		}


		public void HandleParticleEmitterParticleTypeChanged()
		{
			this.AddToUndoStack();
		}



		public ParticleEffectSystem ParticleEffectSystem
		{
			get
			{
				return base.Engine.ParticleEffectSystem;
			}
		}



		public override string StatusText
		{
			get
			{
				float num = (this.particleEffect.Radius != null) ? this.particleEffect.Radius.Value : -1f;
				return string.Format("Particles: {0} Seconds: {1:.02} Radius: {2:.02}", this.ParticleEffectSystem.AliveParticleCount, this.particleEffectDuration, num);
			}
		}



		public bool HasParticleEffect
		{
			get
			{
				return this.particleEffect != null;
			}
		}



		public ParticleEffectDefinition ParticleEffectDefinition
		{
			get
			{
				if (this.particleEffect != null)
				{
					return this.particleEffect.Definition;
				}
				return null;
			}
		}


		public override void RefreshScene()
		{
			this.RestartParticleEffect();
		}


		protected override void UpdateTick(float timeElapsed)
		{
			this.ParticleEffectSystem.Update(timeElapsed, base.Engine.CurrentRenderFrame);
			this.particleEffectDuration += timeElapsed;
			if (base.IsLooped && !this.particleEffect.IsRunning)
			{
				this.RestartParticleEffect();
			}
		}


		protected override void RenderScene()
		{
			base.Engine.RenderScene(base.TotalTimeElapsed, base.PostProcessingParams, base.ClearColor, base.Skybox, base.Camera, base.Lights, null, this.ParticleEffectSystem, delegate(RenderSceneSkyboxParams p)
			{
				if (base.Skybox != null)
				{
					p.SkyboxRenderer.RenderSkybox(p.Pipeline, base.Camera, base.Skybox);
				}
			}, delegate(RenderSceneMeshesParams p)
			{
				if (this.mesh != null)
				{
					p.MeshRenderer.BeginRendering(UnitMeshShaderType.Basic, p.MeshRenderPassType);
					p.MeshRenderer.RenderBasicMesh(this.mesh, this.meshBasis, base.Skybox, p.MeshRenderPassType);
					p.MeshRenderer.EndRendering();
				}
				this.ParticleEffectSystem.RenderParticleMeshes(p.MeshRenderer, p.Pipeline, base.Camera, base.Skybox, base.TotalTimeElapsed, p.MeshRenderPassType);
			}, delegate(RenderScenePrim3dSimpleParams p)
			{
				this.ParticleEffectSystem.RenderParticleBillboards(p.Prim3dSimple, p.Pipeline, base.Camera, base.Skybox, base.TotalTimeElapsed);
			}, delegate(RenderScenePrim3dComplexParams p)
			{
			}, delegate(RenderScenePrim3dLinesParams p)
			{
				if (this.settings.ShowBasisAxes && this.particleEffect != null)
				{
					Basis basis = this.particleEffect.Basis;
					p.Prim3dLines.BeginRendering();
					p.Prim3dLines.RenderRotationAxes(basis.Position, basis.Rotation, new Float3(1000f, 1000f, 1000f), 2f, 1f);
					p.Prim3dLines.EndRendering();
				}
			}, delegate(RenderScenePrim3dShapesParams p)
			{
			}, delegate(Render2dSceneParams p)
			{
			});
		}


		public void CreateNewParticleEffect()
		{
			this.DisposeParticleEffect(true);
			this.particleEffectName = "";
			this.settings.ClearSoloEmitter(this.particleEffectName);
			this.particleEffect = this.ParticleEffectSystem.CreateParticleEffect();
			this.FillNewParticleEffect(this.particleEffect);
			this.RestartParticleEffect();
			base.ResetCamera();
			this.OnParticleEffectChanged();
			this.OnParticleEffectComponentsChanged();
			this.SyncParticleEffectBasisToMeshPoint();
			this.ResetUndo();
		}


		private void FillNewParticleEffect(ParticleEffect def)
		{
			ParticleEmitter particleEmitter = def.AddEmitter();
			particleEmitter.Name = "SimplePoint";
			ParticleEmitterNode particleEmitterNode = def.AddNode();
			particleEmitterNode.Name = "Root";
			def.AddEmitterToNodeAttachment(particleEmitter.Id, particleEmitterNode.Id);
		}


		private string PathToSafeKey(string path)
		{
			return path.ToLower().Replace('/', '_').Replace('\\', '_');
		}


		public void LoadParticleEffect(string path)
		{
			this.DisposeParticleEffect(true);
			this.particleEffectName = Path.GetFileName(path);
			this.particleEffect = this.ParticleEffectSystem.CreateParticleEffect();
			this.particleEffect.Definition.LoadFromFile(path);
			this.RestartParticleEffect();
			base.ResetCamera();
			this.OnParticleEffectChanged();
			this.OnParticleEffectComponentsChanged();
			if (this.settings.MeshPointReferences.ContainsKey(this.PathToSafeKey(path)))
			{
				ParticleEffectMeshPointReference particleEffectMeshPointReference = this.settings.MeshPointReferences[this.PathToSafeKey(path)];
				this.LoadMesh(particleEffectMeshPointReference.MeshPath);
				this.SetMeshPointIndex(particleEffectMeshPointReference.PointIndex, true);
			}
			else
			{
				this.RemoveMesh();
			}
			this.ResetUndo();
		}



		public Mesh Mesh
		{
			get
			{
				return this.mesh;
			}
		}


		public void LoadMesh(string path)
		{
			this.DisposeMesh();
			this.mesh = new Mesh();
			this.mesh.LoadFromFile(base.Engine.RenderingSystem, path);
			ParticleEffectModel.MeshChangedDelegate meshChanged = this.MeshChanged;
			if (meshChanged != null)
			{
				meshChanged();
			}
			this.SyncParticleEffectBasisToMeshPoint();
		}


		public void RemoveMesh()
		{
			this.DisposeMesh();
			ParticleEffectModel.MeshChangedDelegate meshChanged = this.MeshChanged;
			if (meshChanged != null)
			{
				meshChanged();
			}
			this.meshPointIndex = 0;
			this.SyncParticleEffectBasisToMeshPoint();
		}


		private void DisposeMesh()
		{
			if (this.mesh != null)
			{
				this.mesh.Dispose();
				this.mesh = null;
			}
		}



		public int MeshPointIndex
		{
			get
			{
				return this.meshPointIndex;
			}
		}


		public void SetMeshPointIndex(int value, bool notifyDelegates)
		{
			this.meshPointIndex = value;
			if (notifyDelegates)
			{
				ParticleEffectModel.MeshPointIndexChangedDelegate meshPointIndexChanged = this.MeshPointIndexChanged;
				if (meshPointIndexChanged != null)
				{
					meshPointIndexChanged();
				}
			}
			this.SyncParticleEffectBasisToMeshPoint();
		}


		private void SyncParticleEffectBasisToMeshPoint()
		{
			if (this.particleEffect != null && this.mesh != null)
			{
				if (this.meshPointIndex < this.mesh.Data.Points.Count)
				{
					this.particleEffect.Basis = this.mesh.Data.Points[this.meshPointIndex].Basis;
					return;
				}
				if (this.mesh.Data.Points.Count > 0)
				{
					this.particleEffect.Basis = this.mesh.Data.Points[0].Basis;
				}
			}
		}




		public bool HasUnsavedChanges
		{
			get
			{
				return this.hasUnsavedChanges;
			}
			set
			{
				if (this.hasUnsavedChanges != value)
				{
					this.hasUnsavedChanges = value;
					this.HasUnsavedChangesChanged();
				}
			}
		}


		public void SaveParticleEffect(string path)
		{
			this.particleEffect.Definition.SaveToFile(path);
			this.jsonBeautifier.BeautifyJson(path);
			this.HasUnsavedChanges = false;
			if (this.mesh != null)
			{
				this.settings.MeshPointReferences[this.PathToSafeKey(path)] = new ParticleEffectMeshPointReference
				{
					MeshPath = this.mesh.LoadedPath,
					PointIndex = this.meshPointIndex
				};
			}
		}


		public void AddEmitter()
		{
			this.particleEffect.AddEmitter();
			this.OnParticleEffectComponentsChanged();
			this.AddToUndoStack();
		}


		public void RemoveEmitter(ParticleEmitterId id)
		{
			this.settings.TryRemoveSoloEmitter(this.particleEffectName, id);
			this.particleEffect.RemoveEmitter(id);
			this.OnParticleEffectComponentsChanged();
			this.AddToUndoStack();
		}


		public void CopyEmitter(ParticleEmitterId id)
		{
			this.particleEffect.CopyEmitter(id);
			this.OnParticleEffectComponentsChanged();
			this.AddToUndoStack();
		}



		public ParticleEmitterId? SoloEmitterId
		{
			get
			{
				return this.settings.GetSoloEmitter(this.particleEffectName);
			}
		}


		public void BeginSoloEmitter(ParticleEmitterId id)
		{
			this.settings.SetSoloEmitter(this.particleEffectName, id);
			this.OnParticleEffectComponentsChanged();
			this.RestartParticleEffect();
		}


		public void EndSoloEmitter()
		{
			this.settings.ClearSoloEmitter(this.particleEffectName);
			this.OnParticleEffectComponentsChanged();
			this.RestartParticleEffect();
		}


		public void RenameEmitter(ParticleEmitterId id, string name)
		{
			ParticleEmitter particleEmitter = this.particleEffect.Definition.Emitters.Find(id);
			if (particleEmitter != null)
			{
				particleEmitter.Name = name;
				this.OnParticleEffectComponentsChanged();
				this.AddToUndoStack();
			}
		}


		public void AddModifier()
		{
			this.particleEffect.AddModifier();
			this.OnParticleEffectComponentsChanged();
			this.AddToUndoStack();
		}


		public void RemoveModifier(ParticleModifierId id)
		{
			this.particleEffect.RemoveModifier(id);
			this.OnParticleEffectComponentsChanged();
			this.AddToUndoStack();
		}


		public void CopyModifier(ParticleModifierId id)
		{
			this.particleEffect.CopyModifier(id);
			this.OnParticleEffectComponentsChanged();
			this.AddToUndoStack();
		}


		public void RenameModifier(ParticleModifierId id, string name)
		{
			ParticleModifier particleModifier = this.particleEffect.Definition.Modifiers.Find(id);
			if (particleModifier != null)
			{
				particleModifier.Name = name;
				this.OnParticleEffectComponentsChanged();
				this.AddToUndoStack();
			}
		}


		public void AddNode()
		{
			this.particleEffect.AddNode();
			this.OnParticleEffectComponentsChanged();
			this.AddToUndoStack();
		}


		public void RemoveNode(ParticleEmitterNodeId id)
		{
			this.particleEffect.RemoveNode(id);
			this.OnParticleEffectComponentsChanged();
			this.AddToUndoStack();
		}


		public void CopyNode(ParticleEmitterNodeId id)
		{
			this.particleEffect.CopyNode(id);
			this.OnParticleEffectComponentsChanged();
			this.AddToUndoStack();
		}


		public void RenameNode(ParticleEmitterNodeId id, string name)
		{
			ParticleEmitterNode particleEmitterNode = this.particleEffect.Definition.Nodes.Find(id);
			if (particleEmitterNode != null)
			{
				particleEmitterNode.Name = name;
				this.OnParticleEffectComponentsChanged();
				this.AddToUndoStack();
			}
		}


		public void MoveEmitterBefore(ParticleEmitterId moveEmitterId, ParticleEmitterId beforeEmitterId)
		{
			this.particleEffect.MoveEmitterBefore(moveEmitterId, beforeEmitterId);
			this.OnParticleEffectComponentsChanged();
			this.AddToUndoStack();
		}


		public void MoveModifierBefore(ParticleModifierId moveModifierId, ParticleModifierId beforeModifierId)
		{
			this.particleEffect.MoveModifierBefore(moveModifierId, beforeModifierId);
			this.OnParticleEffectComponentsChanged();
			this.AddToUndoStack();
		}


		public void AddEmitterToNodeAttachment(ParticleEmitterId emitterId, ParticleEmitterNodeId nodeId)
		{
			this.particleEffect.AddEmitterToNodeAttachment(emitterId, nodeId);
			this.OnParticleEffectComponentsChanged();
			this.AddToUndoStack();
		}


		public void RemoveEmitterToNodeAttachment(ParticleEmitterId emitterId, ParticleEmitterNodeId nodeId)
		{
			this.particleEffect.RemoveEmitterToNodeAttachment(emitterId, nodeId);
			this.OnParticleEffectComponentsChanged();
			this.AddToUndoStack();
		}


		public void AddModifierToEmitterAttachment(ParticleModifierId modifierId, ParticleEmitterId emitterId, ParticleModifierId? beforeModifierId)
		{
			this.particleEffect.AddModifierToEmitterAttachment(modifierId, emitterId, beforeModifierId);
			this.OnParticleEffectComponentsChanged();
			this.AddToUndoStack();
		}


		public void RemoveModifierToEmitterAttachment(ParticleModifierId modifierId, ParticleEmitterId emitterId)
		{
			this.particleEffect.RemoveModifierToEmitterAttachment(modifierId, emitterId);
			this.OnParticleEffectComponentsChanged();
			this.AddToUndoStack();
		}


		public void RestartParticleEffect()
		{
			if (this.particleEffect != null)
			{
				this.particleEffect.PrimaryColor = this.settings.PrimaryExternalColor;
				this.particleEffect.SecondaryColor = this.settings.SecondaryExternalColor;
				this.particleEffect.Restart(this.settings.GetSoloEmitter(this.particleEffectName));
				this.particleEffectDuration = 0f;
			}
		}




		public object SelectedParticleEffectComponent
		{
			get
			{
				return this.selectedParticleEffectComponent;
			}
			set
			{
				if (this.selectedParticleEffectComponent != value)
				{
					this.selectedParticleEffectComponent = value;
					ParticleEffectModel.SelectedParticleEffectComponentChangedDelegate selectedParticleEffectComponentChanged = this.SelectedParticleEffectComponentChanged;
					if (selectedParticleEffectComponentChanged == null)
					{
						return;
					}
					selectedParticleEffectComponentChanged(value);
				}
			}
		}


		private void OnParticleEffectChanged()
		{
			ParticleEffectModel.ParticleEffectChangedDelegate particleEffectChanged = this.ParticleEffectChanged;
			if (particleEffectChanged == null)
			{
				return;
			}
			particleEffectChanged(this.particleEffect);
		}


		private void OnParticleEffectComponentsChanged()
		{
			this.SelectedParticleEffectComponent = null;
			ParticleEffectModel.ParticleEffectComponentsChangedDelegate particleEffectComponentsChanged = this.ParticleEffectComponentsChanged;
			if (particleEffectComponentsChanged == null)
			{
				return;
			}
			particleEffectComponentsChanged(this.particleEffect);
		}


		private ParticleEffectSettings settings;


		private IJsonBeautifier jsonBeautifier;


		private string particleEffectName;


		private ParticleEffect particleEffect;


		private object selectedParticleEffectComponent;


		private float particleEffectDuration;


		private Basis meshBasis = new Basis();


		private Mesh mesh;


		private int meshPointIndex;


		private ParticleEffectDefinition nextUndo;


		private List<ParticleEffectDefinition> undoStack;


		private int? undoPosition;


		private bool hasUnsavedChanges;



		public delegate void ParticleEffectChangedDelegate(ParticleEffect particleEffect);



		public delegate void ParticleEffectComponentsChangedDelegate(ParticleEffect particleEffect);



		public delegate void ParticleEffectPropertyValueChangedDelegate(string label);



		public delegate void SelectedParticleEffectComponentChangedDelegate(object selectedComponent);



		public delegate void MeshChangedDelegate();



		public delegate void MeshPointIndexChangedDelegate();



		public delegate void UndoChangedDelgate();



		public delegate void HasUnsavedChangesChangedDelegate();
	}
}
