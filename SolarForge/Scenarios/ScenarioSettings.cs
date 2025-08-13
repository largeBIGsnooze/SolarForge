using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace SolarForge.Scenarios
{

	[TypeConverter(typeof(ExpandableObjectConverter))]
	public class ScenarioSettings : INotifyPropertyChanged
	{



		public event PropertyChangedEventHandler PropertyChanged;




		[Category("Clear Color")]
		[DisplayName("With Nothing")]
		public Color WithNoChartClearColor
		{
			get
			{
				return this.withNoChartClearColor;
			}
			set
			{
				this.withNoChartClearColor = value;
				this.OnPropertyChanged("WithNoChartClearColor");
			}
		}




		[Category("Clear Color")]
		[DisplayName("With Chart Preview")]
		public Color WithPreviewChartClearColor
		{
			get
			{
				return this.withPreviewChartClearColor;
			}
			set
			{
				this.withPreviewChartClearColor = value;
				this.OnPropertyChanged("WithPreviewChartClearColor");
			}
		}




		[Category("Clear Color")]
		[DisplayName("With Chart")]
		public Color WithChartClearColor
		{
			get
			{
				return this.withChartClearColor;
			}
			set
			{
				this.withChartClearColor = value;
				this.OnPropertyChanged("WithChartClearColor");
			}
		}




		[Category("Accessibility Options")]
		[DisplayName("Show Node Helper")]
		public bool ShowNodeHelper
		{
			get
			{
				return this.showNodeHelper;
			}
			set
			{
				this.showNodeHelper = value;
				this.OnPropertyChanged("ShowNodeHelper");
			}
		}




		[Category("Visual Options")]
		[DisplayName("Use Node Graphics")]
		[Description("Use custom graphics for nodes instead of colored dots")]
		public bool UseNodeGraphics
		{
			get
			{
				return this.useNodeGraphics;
			}
			set
			{
				this.useNodeGraphics = value;
				this.OnPropertyChanged("UseNodeGraphics");
			}
		}




		[Category("Visual Options")]
		[DisplayName("Fallback to Colored Dots")]
		[Description("Show colored dots when custom graphics are not available")]
		public bool FallbackToColoredDots
		{
			get
			{
				return this.fallbackToColoredDots;
			}
			set
			{
				this.fallbackToColoredDots = value;
				this.OnPropertyChanged("FallbackToColoredDots");
			}
		}




		[Category("Grid Options")]
		[DisplayName("Show Grid")]
		public bool ShowGrid
		{
			get
			{
				return this.showGrid;
			}
			set
			{
				this.showGrid = value;
				this.OnPropertyChanged("ShowGrid");
			}
		}




		[Category("Grid Options")]
		[DisplayName("Grid Color")]
		public Color GridColor
		{
			get
			{
				return this.gridColor;
			}
			set
			{
				this.gridColor = value;
				this.OnPropertyChanged("GridColor");
			}
		}




		[Category("Grid Options")]
		[DisplayName("Grid Transparency")]
		[Description("Value between 0 (Transparent) and 255 (Opaque)")]
		public int GridAlpha
		{
			get
			{
				return this.gridAlpha;
			}
			set
			{
				if (value < 0 || value > 255)
				{
					return;
				}
				this.gridAlpha = value;
				this.OnPropertyChanged("GridAlpha");
			}
		}




		[Category("Grid Options")]
		[DisplayName("Grid Spacing")]
		public int GridSpacing
		{
			get
			{
				return this.gridSpacing;
			}
			set
			{
				this.gridSpacing = value;
				this.OnPropertyChanged("GridSpacing");
			}
		}


		public ScenarioSettings()
		{
			this.ResetToDefault();
		}


		public void ResetToDefault()
		{
			this.WithNoChartClearColor = Color.GhostWhite;
			this.WithPreviewChartClearColor = Color.DarkSlateGray;
			this.WithChartClearColor = Color.Black;
			this.ShowNodeHelper = false;
			this.UseNodeGraphics = true;
			this.FallbackToColoredDots = true;
			this.ShowGrid = true;
			this.GridColor = Color.FromArgb(120, Color.GhostWhite);
			this.GridSpacing = 100;
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


		private Color withNoChartClearColor;


		private Color withPreviewChartClearColor;


		private Color withChartClearColor;


		private bool showNodeHelper;


		private bool useNodeGraphics = true;


		private bool fallbackToColoredDots = true;


		private bool showGrid = true;


		private Color gridColor = Color.GhostWhite;


		private int gridAlpha = 120;


		private int gridSpacing = 100;
	}
}
