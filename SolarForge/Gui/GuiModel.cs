using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Solar.Gui;
using SolarForge.Utility;

namespace SolarForge.Gui
{

	public class GuiModel
	{



		public event GuiModel.RootComponentChangedDelegate RootComponentChanged;




		public event GuiModel.SelectedComponentsChangedDelegate SelectedComponentsChanged;




		public event GuiModel.SelectedComponentPropertyChangedDelegate SelectedComponentPropertyChanged;




		public event GuiModel.SelectedComponentDraggingUpdatedDelegate SelectedComponentDraggingUpdated;


		public GuiModel(GuiSettings settings, IJsonBeautifier jsonBeautifier)
		{
			this.settings = settings;
			this.jsonBeautifier = jsonBeautifier;
			this.selectedComponents = new List<GuiComponent>();
		}



		public GuiSettings Settings
		{
			get
			{
				return this.settings;
			}
		}


		public void LoadGui(string path)
		{
			using (StreamReader streamReader = new StreamReader(path))
			{
				string json = streamReader.ReadToEnd();
				this.rootObject = JObject.Parse(json);
				this.rootComponent = GuiModel.TryMakeComponentRecursive(this.rootObject, "root", "root", 0);
				GuiModel.RootComponentChangedDelegate rootComponentChanged = this.RootComponentChanged;
				if (rootComponentChanged != null)
				{
					rootComponentChanged();
				}
			}
		}


		public void SaveGui(string path)
		{
			using (StreamWriter streamWriter = new StreamWriter(path))
			{
				using (JsonTextWriter jsonTextWriter = new JsonTextWriter(streamWriter))
				{
					jsonTextWriter.Formatting = Formatting.Indented;
					JToken jtoken = this.rootObject.DeepClone();
					this.MergeComponentChangesRecursive((JObject)jtoken, this.rootComponent);
					jtoken.WriteTo(jsonTextWriter, Array.Empty<JsonConverter>());
					streamWriter.Write('\n');
				}
			}
			this.jsonBeautifier.BeautifyJson(path);
		}


		private void MergeComponentChangesRecursive(JObject obj, GuiComponent component)
		{
			if (!component.IsLayoutPlaceholderOnly)
			{
				obj["layout"] = GuiComponent.NewJObjectFromLayout(component.Layout);
			}
			foreach (KeyValuePair<string, GuiComponent> keyValuePair in component.Children)
			{
				this.MergeComponentChangesRecursive((JObject)obj[keyValuePair.Key], keyValuePair.Value);
			}
		}



		public GuiComponent RootComponent
		{
			get
			{
				return this.rootComponent;
			}
		}



		public IEnumerable<GuiComponent> SelectedComponents
		{
			get
			{
				return this.selectedComponents;
			}
		}


		public bool IsComponentSelected(GuiComponent component)
		{
			return this.selectedComponents.Contains(component);
		}


		public void SetSelectedComponents(IEnumerable<GuiComponent> components)
		{
			this.selectedComponents = components.ToList<GuiComponent>();
			GuiModel.SelectedComponentsChangedDelegate selectedComponentsChanged = this.SelectedComponentsChanged;
			if (selectedComponentsChanged == null)
			{
				return;
			}
			selectedComponentsChanged();
		}


		public void NotifySelectedComponentPropertyChanged()
		{
			GuiModel.SelectedComponentPropertyChangedDelegate selectedComponentPropertyChanged = this.SelectedComponentPropertyChanged;
			if (selectedComponentPropertyChanged == null)
			{
				return;
			}
			selectedComponentPropertyChanged();
		}


		public void NotifySelectedComponentDraggingUpdated()
		{
			GuiModel.SelectedComponentDraggingUpdatedDelegate selectedComponentDraggingUpdated = this.SelectedComponentDraggingUpdated;
			if (selectedComponentDraggingUpdated == null)
			{
				return;
			}
			selectedComponentDraggingUpdated();
		}


		private static GuiComponent TryMakeComponentRecursive(JObject obj, string path, string name, int depth)
		{
			GuiComponent guiComponent = new GuiComponent(path, name, depth);
			JToken jtoken;
			if (obj.TryGetValue("layout", out jtoken))
			{
				JObject jobject = jtoken as JObject;
				if (jobject == null)
				{
					throw new InvalidDataException("'layout' not an object");
				}
				guiComponent.Layout = GuiComponent.NewLayoutFromJObject(jobject);
			}
			else
			{
				guiComponent.IsLayoutPlaceholderOnly = true;
				guiComponent.Layout = new WindowLayout();
				guiComponent.Layout.VerticalAlignment = WindowVerticalAlignment.Top;
				guiComponent.Layout.HorizontalAlignment = WindowHorizontalAlignment.Left;
				guiComponent.Layout.Width = 1000;
				guiComponent.Layout.Height = 1000;
			}
			foreach (KeyValuePair<string, JToken> keyValuePair in obj)
			{
				if (keyValuePair.Key != "layout" && !keyValuePair.Key.Contains("shared_"))
				{
					JObject jobject2 = keyValuePair.Value as JObject;
					if (jobject2 != null && jobject2.ContainsKey("layout"))
					{
						GuiComponent guiComponent2 = GuiModel.TryMakeComponentRecursive(jobject2, path + "/" + keyValuePair.Key, keyValuePair.Key, depth + 1);
						if (guiComponent2 != null)
						{
							guiComponent.Children[keyValuePair.Key] = guiComponent2;
						}
					}
				}
			}
			return guiComponent;
		}


		public void SyncSelection(GuiModel.SyncComponentsAction action)
		{
			try
			{
				switch (action)
				{
				case GuiModel.SyncComponentsAction.AlignLeft:
					this.AlignSelection((GuiComponent x) => x.Layout.Margins.Left, delegate(int v, GuiComponent c)
					{
						c.Layout.Margins.Left = v;
					});
					break;
				case GuiModel.SyncComponentsAction.AlignRight:
					this.AlignSelection((GuiComponent x) => x.Layout.Margins.Right, delegate(int v, GuiComponent c)
					{
						c.Layout.Margins.Right = v;
					});
					break;
				case GuiModel.SyncComponentsAction.AlignTop:
					this.AlignSelection((GuiComponent x) => x.Layout.Margins.Top, delegate(int v, GuiComponent c)
					{
						c.Layout.Margins.Top = v;
					});
					break;
				case GuiModel.SyncComponentsAction.AlignBottom:
					this.AlignSelection((GuiComponent x) => x.Layout.Margins.Bottom, delegate(int v, GuiComponent c)
					{
						c.Layout.Margins.Bottom = v;
					});
					break;
				case GuiModel.SyncComponentsAction.MakeSameWidth:
					this.MakeSelectionSameSizeValue((GuiComponent x) => x.Layout.Width, delegate(int v, GuiComponent c)
					{
						c.Layout.Width = v;
					});
					break;
				case GuiModel.SyncComponentsAction.MakeSameHeight:
					this.MakeSelectionSameSizeValue((GuiComponent x) => x.Layout.Height, delegate(int v, GuiComponent c)
					{
						c.Layout.Height = v;
					});
					break;
				case GuiModel.SyncComponentsAction.MakeSameSize:
					this.MakeSelectionSameSizeValue((GuiComponent x) => x.Layout.Width, delegate(int v, GuiComponent c)
					{
						c.Layout.Width = v;
					});
					this.MakeSelectionSameSizeValue((GuiComponent x) => x.Layout.Height, delegate(int v, GuiComponent c)
					{
						c.Layout.Height = v;
					});
					break;
				case GuiModel.SyncComponentsAction.MakeHorizontalSpacingEqual:
					this.MakeSelectionHorizontalSpacingEqual();
					break;
				case GuiModel.SyncComponentsAction.MakeVerticalSpacingEqual:
					this.MakeSelectionVerticalSpacingEqual();
					break;
				}
			}
			catch (GuiModel.SyncComponentsException ex)
			{
				MessageBox.Show(ex.Message, "SyncSelection Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			GuiModel.SelectedComponentPropertyChangedDelegate selectedComponentPropertyChanged = this.SelectedComponentPropertyChanged;
			if (selectedComponentPropertyChanged == null)
			{
				return;
			}
			selectedComponentPropertyChanged();
		}


		private void AlignSelection(Func<GuiComponent, int> getAlignToValue, Action<int, GuiComponent> alignComponentAction)
		{
			GuiComponent arg = this.selectedComponents.First<GuiComponent>();
			int arg2 = getAlignToValue(arg);
			foreach (GuiComponent arg3 in this.selectedComponents)
			{
				alignComponentAction(arg2, arg3);
			}
		}


		private void MakeSelectionSameSizeValue(Func<GuiComponent, int> getSizeValue, Action<int, GuiComponent> makeSameSizeAction)
		{
			GuiComponent arg = this.selectedComponents.First<GuiComponent>();
			int arg2 = getSizeValue(arg);
			foreach (GuiComponent arg3 in this.selectedComponents)
			{
				makeSameSizeAction(arg2, arg3);
			}
		}


		private void MakeSelectionHorizontalSpacingEqual()
		{
			if (this.selectedComponents.Any((GuiComponent x) => x.Layout.HorizontalAlignment != WindowHorizontalAlignment.Left))
			{
				throw new GuiModel.SyncComponentsException("Only HorizontalAlignment.Left supported.");
			}
			IOrderedEnumerable<GuiComponent> orderedEnumerable = from x in this.selectedComponents
			orderby x.Layout.Margins.Left
			select x;
			if (orderedEnumerable.Count<GuiComponent>() > 2)
			{
				int num = orderedEnumerable.ElementAt(1).Layout.Margins.Left - orderedEnumerable.ElementAt(0).Layout.Margins.Left - orderedEnumerable.ElementAt(0).Layout.Width;
				int num2 = orderedEnumerable.ElementAt(0).Layout.Margins.Left;
				foreach (GuiComponent guiComponent in orderedEnumerable)
				{
					guiComponent.Layout.Margins.Left = num2;
					num2 += guiComponent.Layout.Width;
					num2 += num;
				}
			}
		}


		private void MakeSelectionVerticalSpacingEqual()
		{
			if (this.selectedComponents.Any((GuiComponent x) => x.Layout.VerticalAlignment != WindowVerticalAlignment.Top))
			{
				throw new GuiModel.SyncComponentsException("Only VerticalAlignment.Top supported.");
			}
			IOrderedEnumerable<GuiComponent> orderedEnumerable = from x in this.selectedComponents
			orderby x.Layout.Margins.Top
			select x;
			if (orderedEnumerable.Count<GuiComponent>() > 2)
			{
				int num = orderedEnumerable.ElementAt(1).Layout.Margins.Top - orderedEnumerable.ElementAt(0).Layout.Margins.Top - orderedEnumerable.ElementAt(0).Layout.Height;
				int num2 = orderedEnumerable.ElementAt(0).Layout.Margins.Top;
				foreach (GuiComponent guiComponent in orderedEnumerable)
				{
					guiComponent.Layout.Margins.Top = num2;
					num2 += guiComponent.Layout.Height;
					num2 += num;
				}
			}
		}


		private GuiSettings settings;


		private IJsonBeautifier jsonBeautifier;


		private JObject rootObject;


		private GuiComponent rootComponent;


		private List<GuiComponent> selectedComponents;


		private const string LayoutKey = "layout";



		public delegate void RootComponentChangedDelegate();



		public delegate void SelectedComponentsChangedDelegate();



		public delegate void SelectedComponentPropertyChangedDelegate();



		public delegate void SelectedComponentDraggingUpdatedDelegate();


		public enum SyncComponentsAction
		{

			AlignLeft,

			AlignRight,

			AlignTop,

			AlignBottom,

			MakeSameWidth,

			MakeSameHeight,

			MakeSameSize,

			MakeHorizontalSpacingEqual,

			MakeVerticalSpacingEqual
		}


		public class SyncComponentsException : Exception
		{

			public SyncComponentsException(string message) : base(message)
			{
			}
		}
	}
}
