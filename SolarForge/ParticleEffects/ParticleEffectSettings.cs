using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using Solar.Rendering;
using SolarForge.Scenes;

namespace SolarForge.ParticleEffects
{

	[TypeConverter(typeof(ExpandableObjectConverter))]
	public class ParticleEffectSettings : SceneSettings3d
	{



		[Browsable(false)]
		public Dictionary<string, int> SoloEmitters { get; set; } = new Dictionary<string, int>();




		[Browsable(false)]
		public Dictionary<string, ParticleEffectMeshPointReference> MeshPointReferences { get; set; } = new Dictionary<string, ParticleEffectMeshPointReference>();




		[Category("Basis Axes")]
		[DisplayName("Visible")]
		public bool ShowBasisAxes { get; set; }




		[Category("External Colors")]
		[DisplayName("External Primary Color")]
		public Color PrimaryExternalColor { get; set; }




		[Category("External Colors")]
		[DisplayName("External Secondary Color")]
		public Color SecondaryExternalColor { get; set; }


		public ParticleEffectSettings()
		{
			this.ResetToDefault();
		}


		public override void ResetToDefault()
		{
			base.ResetToDefault();
			base.ClearColor = Color.DarkSlateGray;
			this.ShowBasisAxes = false;
			this.PrimaryExternalColor = Color.Pink;
			this.SecondaryExternalColor = Color.Orange;
		}


		public void ClearSoloEmitter(string effectName)
		{
			if (this.SoloEmitters.ContainsKey(effectName))
			{
				this.SoloEmitters.Remove(effectName);
			}
		}


		public bool TryRemoveSoloEmitter(string effectName, ParticleEmitterId emitterId)
		{
			if (this.SoloEmitters.ContainsKey(effectName) && this.SoloEmitters[effectName] == emitterId.Value)
			{
				this.SoloEmitters.Remove(effectName);
				return true;
			}
			return false;
		}


		public void SetSoloEmitter(string effectName, ParticleEmitterId emitterId)
		{
			if (this.SoloEmitters.ContainsKey(effectName))
			{
				this.SoloEmitters[effectName] = emitterId.Value;
				return;
			}
			this.SoloEmitters.Add(effectName, emitterId.Value);
		}


		public ParticleEmitterId? GetSoloEmitter(string effectName)
		{
			int value;
			if (this.SoloEmitters.TryGetValue(effectName, out value))
			{
				return new ParticleEmitterId?(new ParticleEmitterId(value));
			}
			return null;
		}
	}
}
