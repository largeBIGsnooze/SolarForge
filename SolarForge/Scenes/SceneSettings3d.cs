using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using Solar.Math;
using Solar.Rendering;

namespace SolarForge.Scenes
{

	[TypeConverter(typeof(ExpandableObjectConverter))]
	public class SceneSettings3d : INotifyPropertyChanged
	{



		public event PropertyChangedEventHandler PropertyChanged;


		public SceneSettings3d()
		{
			this.ResetToDefault();
		}


		public virtual void ResetToDefault()
		{
			this.ClearColor = Color.Black;
			this.Skybox = new SkyboxName("");
			this.PrimaryLight = new MeshLight
			{
				Type = MeshLightType.PointInfinite,
				Intensity = 1f,
				Color = Color.LightGoldenrodYellow,
				Position = new Float3(100000f, 0f, 0f)
			};
			this.KeyLight = new MeshLight
			{
				Type = MeshLightType.PointInfinite,
				Intensity = 1f,
				Color = Color.LightGoldenrodYellow,
				Position = new Float3(100000f, 0f, 0f)
			};
			this.FillLight = new MeshLight
			{
				Type = MeshLightType.PointInfinite,
				Intensity = 1f,
				Color = Color.DarkGoldenrod,
				Position = new Float3(0f, 0f, 100000f)
			};
			this.RimLight = new MeshLight
			{
				Type = MeshLightType.PointInfinite,
				Intensity = 1f,
				Color = Color.Black,
				Position = new Float3(-10000f, 0f, 0f)
			};
			this.SecondaryLight = new MeshLight
			{
				Type = MeshLightType.PointInfinite,
				Intensity = 1f,
				Color = Color.BlueViolet,
				Position = new Float3(-100000f, 0f, 0f)
			};
			this.CameraDistance = 100f;
			this.CameraYaw = 15f;
			this.CameraPitch = 13f;
			foreach (object obj in TypeDescriptor.GetProperties(this))
			{
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj;
				DefaultValueAttribute defaultValueAttribute = (DefaultValueAttribute)propertyDescriptor.Attributes[typeof(DefaultValueAttribute)];
				if (defaultValueAttribute != null)
				{
					propertyDescriptor.SetValue(this, defaultValueAttribute.Value);
				}
			}
		}




		[Category("Background")]
		public Color ClearColor
		{
			get
			{
				return this.clearColor;
			}
			set
			{
				this.clearColor = value;
				this.OnPropertyChanged("ClearColor");
			}
		}




		[Category("Background")]
		public SkyboxName Skybox
		{
			get
			{
				return this.skybox;
			}
			set
			{
				this.skybox = value;
				this.OnPropertyChanged("Skybox");
			}
		}




		[Category("Lights")]
		public MeshLight PrimaryLight
		{
			get
			{
				return this.primaryLight;
			}
			set
			{
				this.primaryLight = value;
				this.OnPropertyChanged("PrimaryLight");
			}
		}




		[Category("Lights")]
		public MeshLight KeyLight
		{
			get
			{
				return this.keyLight;
			}
			set
			{
				this.keyLight = value;
				this.OnPropertyChanged("KeyLight");
			}
		}




		[Category("Lights")]
		public MeshLight FillLight
		{
			get
			{
				return this.fillLight;
			}
			set
			{
				this.fillLight = value;
				this.OnPropertyChanged("FillLight");
			}
		}




		[Category("Lights")]
		public MeshLight RimLight
		{
			get
			{
				return this.rimLight;
			}
			set
			{
				this.rimLight = value;
				this.OnPropertyChanged("RimLight");
			}
		}




		[Category("Lights")]
		public MeshLight SecondaryLight
		{
			get
			{
				return this.secondaryLight;
			}
			set
			{
				this.secondaryLight = value;
				this.OnPropertyChanged("SecondaryLight");
			}
		}




		[Category("Camera")]
		[DisplayName("Initial Distance")]
		public float CameraDistance
		{
			get
			{
				return this.cameraDistance;
			}
			set
			{
				this.cameraDistance = value;
				this.OnPropertyChanged("CameraDistance");
			}
		}




		[Category("Camera")]
		[DisplayName("Initial Pitch")]
		public float CameraPitch
		{
			get
			{
				return this.cameraPitch;
			}
			set
			{
				this.cameraPitch = value;
				this.OnPropertyChanged("CameraPitch");
			}
		}




		[Category("Camera")]
		[DisplayName("Initial Yaw")]
		public float CameraYaw
		{
			get
			{
				return this.cameraYaw;
			}
			set
			{
				this.cameraYaw = value;
				this.OnPropertyChanged("CameraYaw");
			}
		}


		protected void OnPropertyChanged([CallerMemberName] string name = null)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged == null)
			{
				return;
			}
			propertyChanged(this, new PropertyChangedEventArgs(name));
		}


		private Color clearColor;


		private SkyboxName skybox;


		private MeshLight primaryLight;


		private MeshLight keyLight;


		private MeshLight fillLight;


		private MeshLight rimLight;


		private MeshLight secondaryLight;


		private float cameraDistance;


		private float cameraYaw;


		private float cameraPitch;


		public const string SkyboxPropertyName = "Skybox";
	}
}
