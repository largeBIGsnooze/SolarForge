using System;
using System.ComponentModel;
using System.Drawing;
using Solar.Rendering;
using SolarForge.Scenes;

namespace SolarForge.Brushes
{

	[TypeConverter(typeof(ExpandableObjectConverter))]
	public class BrushSettings : SceneSettings2d
	{



		[Category("Brush")]
		[DisplayName("State")]
		public BrushState BrushState { get; set; }




		[Category("Brush")]
		[DisplayName("Render Style")]
		public BrushRenderStyle BrushRenderStyle { get; set; }




		[Category("Brush")]
		[DisplayName("Area")]
		public Rectangle BrushArea { get; set; }




		[Category("Brush")]
		[DisplayName("Color")]
		public Color BrushColor { get; set; }


		public override void ResetToDefault()
		{
			base.ResetToDefault();
			base.ClearColor = Color.DarkSlateGray;
			this.BrushState = BrushState.Normal;
			this.BrushRenderStyle = BrushRenderStyle.Centered;
			this.BrushArea = new Rectangle(0, 0, 200, 200);
			this.BrushColor = Color.White;
		}
	}
}
