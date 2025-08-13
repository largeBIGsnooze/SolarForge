using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SolarForge.Scenes
{

	[TypeConverter(typeof(ExpandableObjectConverter))]
	public class CameraSettings : INotifyPropertyChanged
	{



		public event PropertyChangedEventHandler PropertyChanged;


		public CameraSettings()
		{
			this.ResetToDefault();
		}


		public void ResetToDefault()
		{
			this.nearZ = 1f;
			this.farZ = 2000000f;
			this.fovY = 0.7853982f;
		}




		public float NearZ
		{
			get
			{
				return this.nearZ;
			}
			set
			{
				this.nearZ = value;
				this.OnPropertyChanged("NearZ");
			}
		}




		public float FarZ
		{
			get
			{
				return this.farZ;
			}
			set
			{
				this.farZ = value;
				this.OnPropertyChanged("FarZ");
			}
		}




		public float FovY
		{
			get
			{
				return this.fovY;
			}
			set
			{
				this.fovY = value;
				this.OnPropertyChanged("FovY");
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


		private float nearZ;


		private float farZ;


		private float fovY;
	}
}
