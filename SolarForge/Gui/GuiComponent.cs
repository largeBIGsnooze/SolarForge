using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using Solar.Gui;

namespace SolarForge.Gui
{

	public class GuiComponent
	{


		public string Path { get; }



		public string Name { get; }




		[Browsable(false)]
		public int Depth { get; private set; }




		public WindowLayout Layout { get; set; }




		[Browsable(false)]
		public bool IsLayoutPlaceholderOnly { get; set; }




		[Browsable(false)]
		public Dictionary<string, GuiComponent> Children { get; set; } = new Dictionary<string, GuiComponent>();


		public GuiComponent(string path, string name, int depth)
		{
			this.Path = path;
			this.Name = name;
			this.Depth = depth;
		}


		public override string ToString()
		{
			return this.Name;
		}



		[Browsable(false)]
		public string NameInListBox
		{
			get
			{
				return new string(' ', this.Depth * 2) + this.Name;
			}
		}



		private static Dictionary<WindowVerticalAlignment, string> VerticalAlignmentNames
		{
			get
			{
				return new Dictionary<WindowVerticalAlignment, string>
				{
					{
						WindowVerticalAlignment.Center,
						"center"
					},
					{
						WindowVerticalAlignment.Top,
						"top"
					},
					{
						WindowVerticalAlignment.Bottom,
						"bottom"
					},
					{
						WindowVerticalAlignment.Stretch,
						"stretch"
					}
				};
			}
		}



		private static Dictionary<WindowHorizontalAlignment, string> HorizontalAlignmentNames
		{
			get
			{
				return new Dictionary<WindowHorizontalAlignment, string>
				{
					{
						WindowHorizontalAlignment.Center,
						"center"
					},
					{
						WindowHorizontalAlignment.Left,
						"left"
					},
					{
						WindowHorizontalAlignment.Right,
						"right"
					},
					{
						WindowHorizontalAlignment.Stretch,
						"stretch"
					}
				};
			}
		}


		public static WindowLayout NewLayoutFromJObject(JObject obj)
		{
			WindowLayout windowLayout = new WindowLayout();
			using (IEnumerator<KeyValuePair<string, JToken>> enumerator = obj.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, JToken> kvp = enumerator.Current;
					if (kvp.Key == "vertical_alignment")
					{
						windowLayout.VerticalAlignment = GuiComponent.VerticalAlignmentNames.First((KeyValuePair<WindowVerticalAlignment, string> x) => x.Value == kvp.Value.Value<string>()).Key;
					}
					else if (kvp.Key == "horizontal_alignment")
					{
						windowLayout.HorizontalAlignment = GuiComponent.HorizontalAlignmentNames.First((KeyValuePair<WindowHorizontalAlignment, string> x) => x.Value == kvp.Value.Value<string>()).Key;
					}
					else if (kvp.Key == "width")
					{
						windowLayout.Width = kvp.Value.Value<int>();
					}
					else if (kvp.Key == "height")
					{
						windowLayout.Height = kvp.Value.Value<int>();
					}
					else if (kvp.Key == "margins")
					{
						windowLayout.Margins = GuiComponent.NewMarginsFromJObject(kvp.Value.Value<JObject>());
					}
					else
					{
						if (!(kvp.Key == "area"))
						{
							throw new InvalidDataException("Unexpected key in layout : " + kvp.Key);
						}
						GuiComponent.Area area = GuiComponent.Area.FromJObject(kvp.Value.Value<JObject>());
						windowLayout.VerticalAlignment = WindowVerticalAlignment.Top;
						windowLayout.HorizontalAlignment = WindowHorizontalAlignment.Left;
						windowLayout.Margins.Left = area.Left;
						windowLayout.Margins.Top = area.Top;
						windowLayout.Width = area.Width;
						windowLayout.Height = area.Height;
					}
				}
			}
			return windowLayout;
		}


		public static WindowMargins NewMarginsFromJObject(JObject obj)
		{
			WindowMargins windowMargins = new WindowMargins();
			foreach (KeyValuePair<string, JToken> keyValuePair in obj)
			{
				if (keyValuePair.Key == "left")
				{
					windowMargins.Left = keyValuePair.Value.Value<int>();
				}
				else if (keyValuePair.Key == "right")
				{
					windowMargins.Right = keyValuePair.Value.Value<int>();
				}
				else if (keyValuePair.Key == "top")
				{
					windowMargins.Top = keyValuePair.Value.Value<int>();
				}
				else
				{
					if (!(keyValuePair.Key == "bottom"))
					{
						throw new InvalidDataException("Unexpected key in margins : " + keyValuePair.Key);
					}
					windowMargins.Bottom = keyValuePair.Value.Value<int>();
				}
			}
			return windowMargins;
		}


		public static JObject NewJObjectFromLayout(WindowLayout layout)
		{
			JObject jobject = new JObject();
			jobject["vertical_alignment"] = GuiComponent.VerticalAlignmentNames[layout.VerticalAlignment];
			jobject["horizontal_alignment"] = GuiComponent.HorizontalAlignmentNames[layout.HorizontalAlignment];
			if (layout.Width != 0)
			{
				jobject["width"] = layout.Width;
			}
			if (layout.Height != 0)
			{
				jobject["height"] = layout.Height;
			}
			if (layout.Margins.Left != 0 || layout.Margins.Right != 0 || layout.Margins.Top != 0 || layout.Margins.Bottom != 0)
			{
				jobject["margins"] = GuiComponent.NewJObjectFromMargins(layout.Margins);
			}
			return jobject;
		}


		public static JObject NewJObjectFromMargins(WindowMargins margins)
		{
			JObject jobject = new JObject();
			if (margins.Left != 0)
			{
				jobject["left"] = margins.Left;
			}
			if (margins.Right != 0)
			{
				jobject["right"] = margins.Right;
			}
			if (margins.Top != 0)
			{
				jobject["top"] = margins.Top;
			}
			if (margins.Bottom != 0)
			{
				jobject["bottom"] = margins.Bottom;
			}
			return jobject;
		}


		private class Area
		{



			public int Width { get; set; }




			public int Height { get; set; }




			public int Left { get; set; }




			public int Top { get; set; }


			public static GuiComponent.Area FromJObject(JObject obj)
			{
				GuiComponent.Area area = new GuiComponent.Area();
				foreach (KeyValuePair<string, JToken> keyValuePair in obj)
				{
					if (keyValuePair.Key == "top_left")
					{
						JArray jarray = keyValuePair.Value.Value<JArray>();
						area.Left = jarray[0].Value<int>();
						area.Top = jarray[1].Value<int>();
					}
					else
					{
						if (!(keyValuePair.Key == "size"))
						{
							throw new InvalidDataException("Unexpected key in area : " + keyValuePair.Key);
						}
						JArray jarray2 = keyValuePair.Value.Value<JArray>();
						area.Width = jarray2[0].Value<int>();
						area.Height = jarray2[1].Value<int>();
					}
				}
				return area;
			}
		}
	}
}
