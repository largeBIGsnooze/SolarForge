using System;
using System.ComponentModel;
using System.Drawing;

namespace SolarForge.Gui
{

	[TypeConverter(typeof(ExpandableObjectConverter))]
	public class GuiSettings
	{



		public Color ClearColor { get; set; }


		public GuiSettings()
		{
			this.ResetToDefault();
		}


		public void ResetToDefault()
		{
			this.ClearColor = Color.White;
		}
	}
}
