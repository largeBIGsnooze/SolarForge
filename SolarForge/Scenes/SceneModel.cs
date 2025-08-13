using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Solar;
using Solar.Rendering;
using Solar.Time;
using SolarForge.Utility;

namespace SolarForge.Scenes
{

	public abstract class SceneModel
	{

		public abstract void RefreshScene();


		protected abstract void UpdateTick(float timeElapsed);


		protected abstract void RenderScene();




		public event SceneModel.IsPausedChangedDelegate IsPausedChanged;




		public event SceneModel.IsLoopedChangedDelegate IsLoopedChanged;




		public event SceneModel.TimeScaleChangedDelegate TimeScaleChanged;


		public SceneModel(Engine engine, PostProcessingParams postProcessingParams, CameraSettings cameraSettings)
		{
			this.engine = engine;
			this.postProcessingParams = postProcessingParams;
			this.camera = new Camera();
			this.cameraSettings = cameraSettings;
			this.cameraSettings.PropertyChanged += this.CameraSettings_PropertyChanged;
			this.SyncCameraToSettings();
			this.timeTicker = new FixedTimeTicker(10);
			this.totalTimeElapsed = 0f;
			this.timeScale = 1.0;
			this.isPaused = false;
			this.isLooped = false;
			this.singleSteps = 0;
			this.frameTimer = new Stopwatch();
		}


		private void CameraSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.SyncCameraToSettings();
		}


		private void SyncCameraToSettings()
		{
			this.camera.SetFovNearFarZ(this.cameraSettings.FovY, this.cameraSettings.NearZ, this.cameraSettings.FarZ);
		}


		public virtual void Dispose()
		{
		}


		public void SetViewportSize(Size viewportSize)
		{
			this.camera.SetViewportSize(viewportSize);
		}



		public virtual string StatusText
		{
			get
			{
				return "";
			}
		}



		public Engine Engine
		{
			get
			{
				return this.engine;
			}
		}



		public PostProcessingParams PostProcessingParams
		{
			get
			{
				return this.postProcessingParams;
			}
		}



		public Camera Camera
		{
			get
			{
				return this.camera;
			}
		}


		public void Update()
		{
			int num = this.timeTicker.Update(10, (float)this.TimeScale);
			if (this.IsPaused)
			{
				num = Math.Min(this.singleSteps, num);
				this.singleSteps -= num;
			}
			for (int i = 0; i < num; i++)
			{
				this.UpdateTick(this.timeTicker.SecondsPerTick);
				this.totalTimeElapsed += this.timeTicker.SecondsPerTick;
			}
			this.UpdateInternal();
		}


		protected virtual void UpdateInternal()
		{
		}


		public void OnVisible()
		{
			this.frameTimer.Restart();
		}


		public void UpdateAndRenderScene(bool canPan)
		{
			int? panX = null;
			int? panY = null;
			int? panZ = null;
			if (canPan)
			{
				try
				{
					Form activeForm = Form.ActiveForm;
					if (activeForm != null && Application.OpenForms.Count > 0 && activeForm.Equals(Application.OpenForms[0]))
					{
						if (KeyState.IsKeyDown(Keys.A))
						{
							panX = new int?(-1);
						}
						else if (KeyState.IsKeyDown(Keys.D))
						{
							panX = new int?(1);
						}
						if (KeyState.IsKeyDown(Keys.E))
						{
							panY = new int?(1);
						}
						else if (KeyState.IsKeyDown(Keys.Q))
						{
							panY = new int?(-1);
						}
						if (KeyState.IsKeyDown(Keys.W))
						{
							panZ = new int?(1);
						}
						else if (KeyState.IsKeyDown(Keys.S))
						{
							panZ = new int?(-1);
						}
					}
				}
				catch (ArgumentOutOfRangeException)
				{
				}
			}
			this.camera.Update(this.IsSlowZoomEnabled, panX, panY, panZ, (float)this.frameTimer.Elapsed.TotalSeconds);
			this.frameTimer.Restart();
			this.RenderScene();
		}


		public void HandleMouseWheel(MouseEventArgs e)
		{
			this.camera.Zoom(e.Delta > 0, this.IsSlowZoomEnabled);
		}



		public bool IsSlowZoomEnabled
		{
			get
			{
				return KeyState.IsKeyDown(Keys.LShiftKey);
			}
		}


		public void HandleMouseMove(int deltaX, int deltaY, MouseButtons buttons)
		{
			if (buttons == MouseButtons.Left)
			{
				this.camera.PanWithMouse(deltaX, deltaY);
				return;
			}
			if (buttons == MouseButtons.Right)
			{
				this.camera.RotateWithMouse(deltaX, deltaY);
			}
		}


		public virtual void HandleMouseClick(Point point, MouseButtons button)
		{
		}



		public float TotalTimeElapsed
		{
			get
			{
				return this.totalTimeElapsed;
			}
		}




		public bool IsPaused
		{
			get
			{
				return this.isPaused;
			}
			set
			{
				this.isPaused = value;
				SceneModel.IsPausedChangedDelegate isPausedChanged = this.IsPausedChanged;
				if (isPausedChanged == null)
				{
					return;
				}
				isPausedChanged(value);
			}
		}




		public bool IsLooped
		{
			get
			{
				return this.isLooped;
			}
			set
			{
				this.isLooped = value;
				SceneModel.IsLoopedChangedDelegate isLoopedChanged = this.IsLoopedChanged;
				if (isLoopedChanged == null)
				{
					return;
				}
				isLoopedChanged(value);
			}
		}




		public double TimeScale
		{
			get
			{
				return this.timeScale;
			}
			set
			{
				this.timeScale = value;
				SceneModel.TimeScaleChangedDelegate timeScaleChanged = this.TimeScaleChanged;
				if (timeScaleChanged == null)
				{
					return;
				}
				timeScaleChanged(value);
			}
		}


		public void SingleStep(int steps)
		{
			this.singleSteps += steps;
		}


		public void ResetTime()
		{
			this.totalTimeElapsed = 0f;
			this.RefreshScene();
		}


		public void ToggleIsPaused()
		{
			this.IsPaused = !this.IsPaused;
		}


		public const int ShiftSingleSteps = 10;


		public const int DefaultTickIntervalInMs = 10;


		private Engine engine;


		private PostProcessingParams postProcessingParams;


		private Camera camera;


		private CameraSettings cameraSettings;


		private FixedTimeTicker timeTicker;


		private float totalTimeElapsed;


		private bool isPaused;


		private bool isLooped;


		private double timeScale;


		private int singleSteps;


		private Stopwatch frameTimer;



		public delegate void IsPausedChangedDelegate(bool isPaused);



		public delegate void IsLoopedChangedDelegate(bool isLooped);



		public delegate void TimeScaleChangedDelegate(double timeScale);
	}
}
