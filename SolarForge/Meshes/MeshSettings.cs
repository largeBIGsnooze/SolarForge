using System;
using System.ComponentModel;
using System.Drawing;
using Solar.Simulations;
using SolarForge.Scenes;

namespace SolarForge.Meshes
{

	[TypeConverter(typeof(ExpandableObjectConverter))]
	public class MeshSettings : SceneSettings3d
	{



		[Category("Bounding Sphere")]
		[DisplayName("Visible")]
		[DefaultValue(false)]
		public bool ShowBoundingSphere { get; set; }




		[Category("Bounding Sphere")]
		[DisplayName("Color")]
		[DefaultValue(typeof(Color), "Magenta")]
		public Color BoundingSphereColor { get; set; }




		[Category("Bounding Box")]
		[DisplayName("Visible")]
		[DefaultValue(false)]
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




		[Category("Mesh Points")]
		[DisplayName("Visible")]
		public bool ShowMeshPoints { get; set; }




		[Category("Mesh Points")]
		[DisplayName("Line Length")]
		public float MeshPointLineLength { get; set; }




		[Category("Mesh Points")]
		[DisplayName("Selected - Line Length")]
		public float MeshPointLineLengthWhenSelected { get; set; }




		[Category("Mesh Points")]
		[DisplayName("Line Thickness")]
		public float MeshPointLineThickness { get; set; }




		[Category("Mesh Points")]
		[DisplayName("Selected - Line Thickness")]
		public float MeshPointLineThicknessWhenSelected { get; set; }




		[Category("Mesh Points")]
		[DisplayName("Line Alpha")]
		public float MeshPointLineAlpha { get; set; }




		[Category("Mesh Points")]
		[DisplayName("Selected - Line Alpha")]
		public float MeshPointLineAlphaWhenSelected { get; set; }




		[Category("Shader")]
		[DisplayName("Shader")]
		[DefaultValue(UnitMeshShaderType.Basic)]
		public UnitMeshShaderType Shader { get; set; }


		public override void ResetToDefault()
		{
			base.ResetToDefault();
			base.ClearColor = Color.DarkSlateGray;
			this.ShowBoundingSphere = false;
			this.BoundingSphereColor = Color.FromArgb(100, Color.Magenta);
			this.ShowBoundingBox = false;
			this.BoundingBoxColor = Color.FromArgb(100, Color.AliceBlue);
			this.ShowWorldAxes = false;
			this.WorldAxesLineThickness = 3f;
			this.ShowMeshPoints = true;
			this.MeshPointLineLength = 10f;
			this.MeshPointLineLengthWhenSelected = 20f;
			this.MeshPointLineThickness = 1f;
			this.MeshPointLineThicknessWhenSelected = 3f;
			this.MeshPointLineAlpha = 0.5f;
			this.MeshPointLineAlphaWhenSelected = 1f;
			this.Shader = UnitMeshShaderType.Basic;
		}
	}
}
