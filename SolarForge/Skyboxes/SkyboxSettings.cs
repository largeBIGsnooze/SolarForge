using System;
using System.ComponentModel;
using System.Drawing;
using SolarForge.Scenes;

namespace SolarForge.Skyboxes
{

	[TypeConverter(typeof(ExpandableObjectConverter))]
	public class SkyboxSettings : SceneSettings3d
	{

		public override void ResetToDefault()
		{
			base.ResetToDefault();
			base.ClearColor = Color.DarkSlateGray;
		}
	}
}
