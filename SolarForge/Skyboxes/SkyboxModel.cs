using System;
using System.Drawing;
using Solar;
using Solar.Rendering;
using SolarForge.Scenes;

namespace SolarForge.Skyboxes
{

	public class SkyboxModel : SceneModel3d, IDisposable
	{

		public SkyboxModel(Engine engine, PostProcessingParams postProcessingParams, CameraSettings cameraSettings, SkyboxSettings modelSettings) : base(engine, postProcessingParams, cameraSettings, modelSettings)
		{
		}


		public override void RefreshScene()
		{
		}


		protected override void UpdateTick(float timeElapsed)
		{
		}


		protected override void RenderScene()
		{
			base.Engine.RenderScene(base.TotalTimeElapsed, base.PostProcessingParams, Color.Black, base.Skybox, base.Camera, base.Lights, null, null, delegate(RenderSceneSkyboxParams p)
			{
				if (base.Skybox != null)
				{
					p.SkyboxRenderer.RenderSkybox(p.Pipeline, base.Camera, base.Skybox);
				}
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
			});
		}
	}
}
