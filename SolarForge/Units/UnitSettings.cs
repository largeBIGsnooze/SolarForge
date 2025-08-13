using System;
using System.ComponentModel;
using System.Drawing;
using SolarForge.Scenes;

namespace SolarForge.Units
{

	[TypeConverter(typeof(ExpandableObjectConverter))]
	public class UnitSettings : SceneSettings3d
	{



		[Category("Child Meshes")]
		[DisplayName("All Visible")]
		public bool ShowAllChildMeshes { get; set; }




		[Category("Weapon Points")]
		[DisplayName("All Visible")]
		public bool ShowAllWeaponPoints { get; set; }




		[Category("Bounding Sphere")]
		[DisplayName("Visible")]
		public bool ShowBoundingSphere { get; set; }




		[Category("Bounding Sphere")]
		[DisplayName("Color")]
		public Color BoundingSphereColor { get; set; }




		[Category("Bounding Box")]
		[DisplayName("Visible")]
		public bool ShowBoundingBox { get; set; }




		[Category("Bounding Box")]
		[DisplayName("Color")]
		public Color BoundingBoxColor { get; set; }




		[Category("World Axes")]
		[DisplayName("Visible")]
		public bool ShowWorldAxes { get; set; }




		[Category("World Axes")]
		[DisplayName("Line Thickness")]
		public float WorldAxesLineThickness { get; set; }


		public override void ResetToDefault()
		{
			base.ResetToDefault();
			base.ClearColor = Color.DarkSlateGray;
			this.ShowAllWeaponPoints = false;
			this.ShowBoundingSphere = false;
			this.BoundingSphereColor = Color.FromArgb(100, Color.Magenta);
			this.ShowBoundingBox = false;
			this.BoundingBoxColor = Color.FromArgb(100, Color.AliceBlue);
			this.ShowWorldAxes = false;
			this.WorldAxesLineThickness = 3f;
		}
	}
}
