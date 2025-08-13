using System;
using Solar;
using Solar.Rendering;
using SolarForge.Scenes;
using SolarForge.Utility;

namespace SolarForge.Brushes
{

	public class BrushModel : SceneModel2d
	{



		public event BrushModel.BrushChangedDelegate BrushChanged;


		public BrushModel(Engine engine, PostProcessingParams postProcessingParams, CameraSettings cameraSettings, BrushSettings modelSettings, IJsonBeautifier jsonBeautifier) : base(engine, postProcessingParams, cameraSettings, modelSettings)
		{
			this.settings = modelSettings;
			this.jsonBeautifier = jsonBeautifier;
		}



		public BrushSettings Settings
		{
			get
			{
				return this.settings;
			}
		}


		public void CreateNewBrush()
		{
			this.brush = new Brush();
			BrushModel.BrushChangedDelegate brushChanged = this.BrushChanged;
			if (brushChanged == null)
			{
				return;
			}
			brushChanged(this.brush);
		}


		public void LoadBrush(string path)
		{
			this.brush = new Brush();
			this.brush.LoadFromFile(path);
			BrushModel.BrushChangedDelegate brushChanged = this.BrushChanged;
			if (brushChanged == null)
			{
				return;
			}
			brushChanged(this.brush);
		}


		public void SaveBrush(string path)
		{
			this.brush.SaveToFile(path);
			this.jsonBeautifier.BeautifyJson(path);
		}


		public override void RefreshScene()
		{
		}


		protected override void UpdateTick(float timeElapsed)
		{
		}


		protected override void RenderScene()
		{
			base.Engine.RenderScene(base.TotalTimeElapsed, base.PostProcessingParams, base.ClearColor, null, base.Camera, null, null, null, delegate(RenderSceneSkyboxParams p)
			{
			}, delegate(RenderSceneMeshesParams p)
			{
			}, delegate(RenderScenePrim3dSimpleParams p)
			{
			}, delegate(RenderScenePrim3dComplexParams p)
			{
			}, delegate(RenderScenePrim3dLinesParams p)
			{
			}, delegate(RenderScenePrim3dShapesParams p)
			{
			}, delegate(Render2dSceneParams p)
			{
				if (this.brush != null)
				{
					p.BrushRenderer.BeginRendering(p.Pipeline, false);
					p.BrushRenderer.RenderBrush(this.brush, this.settings.BrushRenderStyle, this.settings.BrushState, this.settings.BrushArea, this.settings.BrushColor);
					p.BrushRenderer.EndRendering();
				}
			});
		}


		private Brush brush;


		private BrushSettings settings;


		private IJsonBeautifier jsonBeautifier;



		public delegate void BrushChangedDelegate(Brush brush);
	}
}
