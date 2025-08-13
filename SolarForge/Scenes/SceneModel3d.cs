using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using Solar;
using Solar.Math;
using Solar.Rendering;

namespace SolarForge.Scenes
{

	public abstract class SceneModel3d : SceneModel
	{

		public SceneModel3d(Engine engine, PostProcessingParams postProcessingParams, CameraSettings cameraSettings, SceneSettings3d sceneSettings) : base(engine, postProcessingParams, cameraSettings)
		{
			this.sceneSettings = sceneSettings;
			this.sceneSettings.PropertyChanged += this.SceneSettings_PropertyChanged;
			this.SyncSkyboxToSettings();
			this.ResetCamera();
		}


		private void SceneSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Skybox")
			{
				this.SyncSkyboxToSettings();
				return;
			}
			if (e.PropertyName.Contains("Camera"))
			{
				this.ResetCamera();
			}
		}


		private void SyncSkyboxToSettings()
		{
			if (string.IsNullOrEmpty(this.skyboxPath))
			{
				if (!this.sceneSettings.Skybox.Empty)
				{
					this.skybox = new Skybox(this.sceneSettings.Skybox);
					return;
				}
				this.skybox = null;
			}
		}



		public Color ClearColor
		{
			get
			{
				return this.sceneSettings.ClearColor;
			}
		}


		public void ResetCamera(float? resetCameraDistance)
		{
			this.resetCameraDistance = resetCameraDistance;
			this.ResetCamera();
		}


		public void ResetCamera()
		{
			base.Camera.SetDistanceToTarget(this.resetCameraDistance.GetValueOrDefault(this.sceneSettings.CameraDistance));
			base.Camera.SetRotation(this.sceneSettings.CameraPitch, this.sceneSettings.CameraYaw);
			base.Camera.FocusOnTargetBasis(new Basis());
		}


		public void ResetCameraWithPitchYaw(float pitch, float yaw)
		{
			base.Camera.SetDistanceToTarget(this.resetCameraDistance.GetValueOrDefault(this.sceneSettings.CameraDistance));
			base.Camera.SetRotation(pitch, yaw);
			base.Camera.FocusOnTargetBasis(new Basis());
		}


		public void FocusOnTargetBasis(Basis basis)
		{
			base.Camera.FocusOnTargetBasis(basis);
		}



		public Skybox Skybox
		{
			get
			{
				return this.skybox;
			}
		}



		public List<MeshLight> Lights
		{
			get
			{
				return new List<MeshLight>
				{
					this.sceneSettings.PrimaryLight,
					this.sceneSettings.KeyLight,
					this.sceneSettings.FillLight,
					this.sceneSettings.RimLight,
					this.sceneSettings.SecondaryLight
				};
			}
		}



		public Float3 PrimaryLightPosition
		{
			get
			{
				return this.sceneSettings.PrimaryLight.Position;
			}
		}


		public void LoadSkybox(string path)
		{
			this.skyboxPath = path;
			this.skybox = new Skybox();
			this.skybox.LoadFromFile(path);
		}


		private SceneSettings3d sceneSettings;


		private Skybox skybox;


		private string skyboxPath;


		private float? resetCameraDistance;
	}
}
