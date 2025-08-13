using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using SolarForge.Scenes;

namespace SolarForge.DeathSequences
{

	[TypeConverter(typeof(ExpandableObjectConverter))]
	public class DeathSequenceSettings : SceneSettings3d
	{



		[Browsable(false)]
		public Dictionary<string, DeathSequenceMeshReference> MeshReferences { get; set; } = new Dictionary<string, DeathSequenceMeshReference>();


		public override void ResetToDefault()
		{
			base.ResetToDefault();
			base.ClearColor = Color.DarkSlateGray;
		}
	}
}
