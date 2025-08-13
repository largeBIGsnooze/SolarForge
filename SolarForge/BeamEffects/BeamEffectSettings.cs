using System;
using System.ComponentModel;
using System.Drawing;
using Solar.Math;
using SolarForge.Scenes;

namespace SolarForge.BeamEffects
{

	[TypeConverter(typeof(ExpandableObjectConverter))]
	public class BeamEffectSettings : SceneSettings3d
	{



		[Category("Beam Effect")]
		[DisplayName("Begin Point")]
		public Float3 EndPointA
		{
			get
			{
				return this.endPointA;
			}
			set
			{
				this.endPointA = value;
				base.OnPropertyChanged("EndPointA");
			}
		}




		[Category("Beam Effect")]
		[DisplayName("End Point")]
		public Float3 EndPointB
		{
			get
			{
				return this.endPointB;
			}
			set
			{
				this.endPointB = value;
				base.OnPropertyChanged("EndPointB");
			}
		}




		[Category("Beam Effect")]
		[DisplayName("Duration")]
		public float Duration
		{
			get
			{
				return this.duration;
			}
			set
			{
				this.duration = value;
				base.OnPropertyChanged("Duration");
			}
		}


		public override void ResetToDefault()
		{
			base.ResetToDefault();
			base.ClearColor = Color.DarkSlateGray;
			this.EndPointA = new Float3(-100f, 0f, 0f);
			this.EndPointB = new Float3(100f, 0f, 0f);
			this.Duration = 10f;
		}


		private Float3 endPointA;


		private Float3 endPointB;


		private float duration;
	}
}
