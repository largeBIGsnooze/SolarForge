using System;
using System.Collections.Generic;
using System.Drawing;
using Solar;
using Solar.Math;
using Solar.Rendering;
using SolarForge.Scenes;
using SolarForge.Utility;

namespace SolarForge.BeamEffects
{

	public class BeamEffectModel : SceneModel3d
	{



		public event BeamEffectModel.BeamEffectChangedDelegate BeamEffectChanged;




		public event BeamEffectModel.ParticleEffectsChangedDelegate ParticleEffectsChanged;




		public event BeamEffectModel.SelectedParticleEffectChangedDelegate SelectedParticleEffectChanged;


		public BeamEffectModel(Engine engine, PostProcessingParams postProcessingParams, CameraSettings cameraSettings, BeamEffectSettings modelSettings, IJsonBeautifier jsonBeautifier) : base(engine, postProcessingParams, cameraSettings, modelSettings)
		{
			this.engine = engine;
			this.settings = modelSettings;
			this.particleEffects = new List<ParticleEffect>();
			this.jsonBeautifier = jsonBeautifier;
		}



		public BeamEffectSettings Settings
		{
			get
			{
				return this.settings;
			}
		}



		public ParticleEffectSystem ParticleEffectSystem
		{
			get
			{
				return base.Engine.ParticleEffectSystem;
			}
		}


		public override void Dispose()
		{
			this.DisposeParticleEffects();
		}


		private void DisposeParticleEffects()
		{
			foreach (ParticleEffect particleEffect in this.particleEffects)
			{
				particleEffect.Dispose();
			}
			this.particleEffects.Clear();
		}


		private void CreateParticleEffects()
		{
			if (this.beamEffectDefinition != null)
			{
				Random random = new Random();
				Float3 @float = this.settings.EndPointB - this.settings.EndPointA;
				Float3 float2 = @float.NewNormalized();
				if (float2 != null)
				{
					for (int i = 0; i < this.beamEffectDefinition.ParticleEffects.Count; i++)
					{
						BeamEffectParticleEffectDefinition beamEffectParticleEffectDefinition = this.beamEffectDefinition.ParticleEffects[i];
						float length = @float.Length;
						if (length > 0f)
						{
							float num2;
							for (float num = 0f; num < length; num += num2)
							{
								ParticleEffect particleEffect = this.ParticleEffectSystem.CreateParticleEffect(beamEffectParticleEffectDefinition.ParticleEffect);
								particleEffect.Basis = new Basis(this.settings.EndPointA + float2 * num, Float3x3.NewForwardUp(float2, new Float3(0f, 1f, 0f)));
								particleEffect.Restart(null);
								this.particleEffects.Add(particleEffect);
								num2 = beamEffectParticleEffectDefinition.DistanceInterval.MakeRandomValue(random);
								if (num2 <= 0f)
								{
									break;
								}
							}
						}
					}
				}
			}
		}



		public BeamEffectDefinition BeamEffectDefinition
		{
			get
			{
				return this.beamEffectDefinition;
			}
		}




		public BeamEffectParticleEffectDefinition SelectedParticleEffectDefinition
		{
			get
			{
				return this.selectedParticleEffectDefinition;
			}
			set
			{
				this.selectedParticleEffectDefinition = value;
				BeamEffectModel.SelectedParticleEffectChangedDelegate selectedParticleEffectChanged = this.SelectedParticleEffectChanged;
				if (selectedParticleEffectChanged == null)
				{
					return;
				}
				selectedParticleEffectChanged(value);
			}
		}


		public override void RefreshScene()
		{
			this.beamTimeElapsed = 0f;
			this.DisposeParticleEffects();
			this.CreateParticleEffects();
		}


		protected override void UpdateTick(float timeElapsed)
		{
			this.beamTimeElapsed += timeElapsed;
			if (this.beamTimeElapsed >= this.settings.Duration)
			{
				this.DisposeParticleEffects();
			}
			this.ParticleEffectSystem.Update(timeElapsed, base.Engine.CurrentRenderFrame);
		}


		protected override void RenderScene()
		{
			this.engine.RenderScene(base.TotalTimeElapsed, base.PostProcessingParams, base.ClearColor, base.Skybox, base.Camera, base.Lights, null, this.ParticleEffectSystem, delegate(RenderSceneSkyboxParams p)
			{
				if (base.Skybox != null)
				{
					p.SkyboxRenderer.RenderSkybox(p.Pipeline, base.Camera, base.Skybox);
				}
			}, delegate(RenderSceneMeshesParams p)
			{
				this.ParticleEffectSystem.RenderParticleMeshes(p.MeshRenderer, p.Pipeline, base.Camera, base.Skybox, this.beamTimeElapsed, p.MeshRenderPassType);
			}, delegate(RenderScenePrim3dSimpleParams p)
			{
				this.ParticleEffectSystem.RenderParticleBillboards(p.Prim3dSimple, p.Pipeline, base.Camera, base.Skybox, this.beamTimeElapsed);
			}, delegate(RenderScenePrim3dComplexParams p)
			{
				if (this.beamTimeElapsed < this.settings.Duration)
				{
					p.BeamEffectRenderer.BeginRendering(base.Camera);
					p.BeamEffectRenderer.RenderBeamEffect(this.beamEffectDefinition, this.settings.EndPointA, this.settings.EndPointB, this.settings.Duration, this.beamTimeElapsed);
					p.BeamEffectRenderer.EndRendering();
				}
			}, delegate(RenderScenePrim3dLinesParams p)
			{
			}, delegate(RenderScenePrim3dShapesParams p)
			{
			}, delegate(Render2dSceneParams p)
			{
			});
		}


		public void CreateNewBeamEffect()
		{
			this.beamEffectDefinition = new BeamEffectDefinition();
			this.FillNewBeamEffect(this.beamEffectDefinition);
			base.ResetCamera();
			this.RefreshScene();
			this.HandleBeamEffectChanged();
			this.beamEffectDefinition.CreateResources();
		}


		private void FillNewBeamEffect(BeamEffectDefinition beamEffectDefinition)
		{
			beamEffectDefinition.Width = 20f;
			beamEffectDefinition.GlowColor = Color.White;
			beamEffectDefinition.CoreColor = Color.White;
		}


		public void LoadBeamEffect(string path)
		{
			this.beamEffectDefinition = new BeamEffectDefinition();
			this.FillNewBeamEffect(this.beamEffectDefinition);
			this.beamEffectDefinition.LoadFromFile(path);
			base.ResetCamera();
			this.RefreshScene();
			this.HandleBeamEffectChanged();
		}


		public void SaveBeamEffect(string path)
		{
			this.beamEffectDefinition.SaveToFile(path);
			this.jsonBeautifier.BeautifyJson(path);
		}


		private void HandleBeamEffectChanged()
		{
			if (this.BeamEffectChanged != null)
			{
				this.BeamEffectChanged(this.beamEffectDefinition);
			}
			BeamEffectModel.ParticleEffectsChangedDelegate particleEffectsChanged = this.ParticleEffectsChanged;
			if (particleEffectsChanged != null)
			{
				particleEffectsChanged(this.beamEffectDefinition);
			}
			this.SelectedParticleEffectDefinition = null;
		}


		public void AddParticleEffect()
		{
			this.beamEffectDefinition.AddParticleEffect();
			BeamEffectModel.ParticleEffectsChangedDelegate particleEffectsChanged = this.ParticleEffectsChanged;
			if (particleEffectsChanged == null)
			{
				return;
			}
			particleEffectsChanged(this.beamEffectDefinition);
		}


		public void RemoveParticleEffect(BeamEffectParticleEffectDefinition particleEffect)
		{
			if (this.SelectedParticleEffectDefinition == particleEffect)
			{
				this.SelectedParticleEffectDefinition = null;
			}
			this.beamEffectDefinition.RemoveParticleEffect(particleEffect);
			BeamEffectModel.ParticleEffectsChangedDelegate particleEffectsChanged = this.ParticleEffectsChanged;
			if (particleEffectsChanged == null)
			{
				return;
			}
			particleEffectsChanged(this.beamEffectDefinition);
		}


		private Engine engine;


		private BeamEffectSettings settings;


		private IJsonBeautifier jsonBeautifier;


		private BeamEffectDefinition beamEffectDefinition;


		private BeamEffectParticleEffectDefinition selectedParticleEffectDefinition;


		private List<ParticleEffect> particleEffects;


		private float beamTimeElapsed;



		public delegate void BeamEffectChangedDelegate(BeamEffectDefinition beamEffectDefinition);



		public delegate void ParticleEffectsChangedDelegate(BeamEffectDefinition beamEffectDefinition);



		public delegate void SelectedParticleEffectChangedDelegate(BeamEffectParticleEffectDefinition selectedParticleEffect);
	}
}
