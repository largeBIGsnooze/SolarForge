using System;
using System.Collections.Generic;
using System.IO;
using Solar;
using Solar.DeathSequences;
using Solar.Math;
using Solar.Rendering;
using Solar.Simulations;
using SolarForge.Scenes;

namespace SolarForge.DeathSequences
{

	public class DeathSequenceModel : SceneModel3d, IDisposable
	{



		public event DeathSequenceModel.DeathSequenceChangedDelegate DeathSequenceChanged;




		public event DeathSequenceModel.MeshChangedDelegate MeshChanged;


		public DeathSequenceModel(Engine engine, PostProcessingParams postProcessingParams, CameraSettings cameraSettings, DeathSequenceSettings modelSettings) : base(engine, postProcessingParams, cameraSettings, modelSettings)
		{
			this.engine = engine;
			this.settings = modelSettings;
		}


		public override void Dispose()
		{
			this.DisposeParticleEffects();
			this.DisposeMesh();
		}


		private void DisposeParticleEffects()
		{
			foreach (ParticleEffect particleEffect in this.particleEffects)
			{
				particleEffect.Dispose();
			}
			this.particleEffects.Clear();
		}


		public void LoadDeathSequence(string path)
		{
			this.deathSequencePath = path;
			this.deathSequence = new DeathSequence();
			this.deathSequence.LoadFromFile(path);
			DeathSequenceModel.DeathSequenceChangedDelegate deathSequenceChanged = this.DeathSequenceChanged;
			if (deathSequenceChanged != null)
			{
				deathSequenceChanged(this.deathSequence);
			}
			if (this.settings.MeshReferences.ContainsKey(this.PathToSafeKey(path)))
			{
				DeathSequenceMeshReference deathSequenceMeshReference = this.settings.MeshReferences[this.PathToSafeKey(path)];
				this.LoadMesh(deathSequenceMeshReference.MeshPath, false);
			}
		}


		public override void RefreshScene()
		{
			this.DisposeParticleEffects();
			this.deathSequenceTime = 0f;
		}


		protected override void UpdateTick(float timeElapsed)
		{
			this.ParticleEffectSystem.Update(timeElapsed, base.Engine.CurrentRenderFrame);
			if (this.deathSequence != null)
			{
				float num = this.deathSequenceTime;
				this.deathSequenceTime += timeElapsed;
				foreach (DeathSequenceEvent deathSequenceEvent in this.deathSequence.Events)
				{
					if (deathSequenceEvent.Time >= num && deathSequenceEvent.Time < this.deathSequenceTime)
					{
						this.ProcessDeathSequenceEvent(deathSequenceEvent);
					}
				}
			}
		}


		private void ProcessDeathSequenceEvent(DeathSequenceEvent e)
		{
			if (e.ParticleEffects.Count > 0)
			{
				ParticleEffectDefinitionName name = e.ParticleEffects[this.random.Next(e.ParticleEffects.Count)];
				ParticleEffect particleEffect = this.ParticleEffectSystem.CreateParticleEffect(name);
				if (e.Location == DeathSequenceEventLocation.Center)
				{
					particleEffect.Basis = this.meshBasis;
				}
				else if (this.mesh != null)
				{
					particleEffect.Basis = this.mesh.GetRandomMeshSurfacePointUsingFatTriangles(this.random);
				}
				particleEffect.Restart(null);
				this.particleEffects.Add(particleEffect);
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
				if (this.mesh != null && (this.deathSequence == null || this.deathSequenceTime < this.deathSequence.MeshNotVisibleTime))
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
			}, delegate(RenderScenePrim3dShapesParams p)
			{
			}, delegate(Render2dSceneParams p)
			{
			});
		}



		public override string StatusText
		{
			get
			{
				return string.Format("{0} Particles - {1:.02} Seconds", this.ParticleEffectSystem.AliveParticleCount, this.deathSequenceTime);
			}
		}



		public ParticleEffectSystem ParticleEffectSystem
		{
			get
			{
				return base.Engine.ParticleEffectSystem;
			}
		}


		private string PathToSafeKey(string path)
		{
			return Path.GetFileNameWithoutExtension(path).ToLower().Replace('/', '_').Replace('\\', '_');
		}


		public void LoadMesh(string path, bool updateSettings)
		{
			this.DisposeMesh();
			this.mesh = new Mesh();
			this.mesh.LoadFromFile(base.Engine.RenderingSystem, path);
			DeathSequenceModel.MeshChangedDelegate meshChanged = this.MeshChanged;
			if (meshChanged != null)
			{
				meshChanged(this.mesh);
			}
			if (updateSettings)
			{
				this.settings.MeshReferences[this.PathToSafeKey(this.deathSequencePath)] = new DeathSequenceMeshReference
				{
					MeshPath = this.mesh.LoadedPath,
					Velocity = this.meshVelocity
				};
			}
		}


		public void RemoveMesh()
		{
			this.DisposeMesh();
			DeathSequenceModel.MeshChangedDelegate meshChanged = this.MeshChanged;
			if (meshChanged == null)
			{
				return;
			}
			meshChanged(this.mesh);
		}


		private void DisposeMesh()
		{
			if (this.mesh != null)
			{
				this.mesh.Dispose();
				this.mesh = null;
			}
		}


		private Engine engine;


		private DeathSequenceSettings settings;


		private DeathSequence deathSequence;


		public string deathSequencePath;


		private Mesh mesh;


		private float meshVelocity;


		private Basis meshBasis = new Basis();


		private float deathSequenceTime;


		private List<ParticleEffect> particleEffects = new List<ParticleEffect>();


		private Random random = new Random();



		public delegate void DeathSequenceChangedDelegate(DeathSequence deathSequence);



		public delegate void MeshChangedDelegate(Mesh mesh);
	}
}
