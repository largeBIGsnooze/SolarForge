using System;
using System.ComponentModel;
using System.Drawing;
using Solar;
using Solar.Rendering;

namespace SolarForge.Scenes
{

	public abstract class SceneModel2d : SceneModel
	{

		public SceneModel2d(Engine engine, PostProcessingParams postProcessingParams, CameraSettings cameraSettings, SceneSettings2d sceneSettings) : base(engine, postProcessingParams, cameraSettings)
		{
			this.sceneSettings = sceneSettings;
			this.sceneSettings.PropertyChanged += this.SceneSettings_PropertyChanged;
		}


		private void SceneSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
		}



		public Color ClearColor
		{
			get
			{
				return this.sceneSettings.ClearColor;
			}
		}


		private SceneSettings2d sceneSettings;
	}
}
