using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace SolarForge.Scenes
{

	[TypeConverter(typeof(ExpandableObjectConverter))]
	public class SceneSettings2d : INotifyPropertyChanged
	{



		public event PropertyChangedEventHandler PropertyChanged;


		public SceneSettings2d()
		{
			this.ResetToDefault();
		}


		public virtual void ResetToDefault()
		{
			this.ClearColor = Color.Black;
		}




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
	}
}
