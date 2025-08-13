using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Solar;
using Solar.Rendering;
using Solar.Settings;
using SolarForge.BeamEffects;
using SolarForge.Brushes;
using SolarForge.DeathSequences;
using SolarForge.Gui;
using SolarForge.Meshes;
using SolarForge.ParticleEffects;
using SolarForge.Scenarios;
using SolarForge.Scenes;
using SolarForge.Skyboxes;
using SolarForge.Units;
using SolarForge.Utility;

namespace SolarForge
{

	public class ProgramSettings : IDataSourcePathResolver
	{



		public string AppUserPath { get; private set; }




		[Browsable(false)]
		public PostProcessingParams PostProcessingParams { get; private set; }




		[Browsable(false)]
		public CameraSettings CameraSettings { get; private set; }




		[Browsable(false)]
		public BeamEffectSettings BeamEffectSettings { get; private set; }




		[Browsable(false)]
		public BrushSettings BrushSettings { get; private set; }




		[Browsable(false)]
		public UnitSettings UnitSettings { get; private set; }




		[Browsable(false)]
		public GuiSettings GuiSettings { get; private set; }




		[Browsable(false)]
		public MeshSettings MeshSettings { get; private set; }




		[Browsable(false)]
		public ParticleEffectSettings ParticleEffectSettings { get; private set; }




		[Browsable(false)]
		public ScenarioSettings ScenarioSettings { get; private set; }




		[Browsable(false)]
		public SkyboxSettings SkyboxSettings { get; private set; }




		[Browsable(false)]
		public DeathSequenceSettings DeathSequenceSettings { get; private set; }



		[Browsable(false)]
		public string LoadedValuesFilePath
		{
			get
			{
				return this.engine.SettingRepository.LoadedValuesFilePath;
			}
		}




		[Category("Core")]
		[DisplayName("Sins2 Folder")]
		[Description("Folder where the sins2.exe and built data is located (ex. c:\\sins2_out\\sins2). Changing this will require restart of application.")]
		[Editor(typeof(AdvancedFolderNameEditor), typeof(UITypeEditor))]
		public string RootDataFolderPath
		{
			get
			{
				return this.rootDataFolderPathSetting.Value;
			}
			set
			{
				this.rootDataFolderPathSetting.Value = value;
			}
		}




		[Category("Core")]
		[DisplayName("Sins2 Working Data Folder")]
		[Description("Folder where the unbuilt data is located (ex. c:\\sins2_data). This is where data changes are saved to. This folder will be searched for the equivalent filename to save to.")]
		[Editor(typeof(AdvancedFolderNameEditor), typeof(UITypeEditor))]
		public string RootDataSourceSaveFolderPath
		{
			get
			{
				return this.rootDataSourceSaveFolderPathSetting.Value;
			}
			set
			{
				this.rootDataSourceSaveFolderPathSetting.Value = value;
			}
		}




		[Category("Core")]
		[DisplayName("MeshBuilder Exe")]
		[Description("Path to MeshBuilder.exe (ex. c:\\sins2_out\\tools\\MeshBuilder\\MeshBuilder.exe). Used to view GLTF and sins1 meshes.")]
		[Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
		public string MeshBuilderExePath
		{
			get
			{
				return this.meshBuilderExePathSetting.Value;
			}
			set
			{
				this.meshBuilderExePathSetting.Value = value;
			}
		}




		[Browsable(false)]
		public Color EmptySceneClearColor
		{
			get
			{
				return this.emptySceneClearColorSetting.Value;
			}
			set
			{
				this.emptySceneClearColorSetting.Value = value;
			}
		}


		public ProgramSettings(Engine engine)
		{
			this.engine = engine;
			engine.SettingRepository.AddSetting(this.rootDataFolderPathSetting);
			engine.SettingRepository.AddSetting(this.rootDataSourceSaveFolderPathSetting);
			engine.SettingRepository.AddSetting(this.meshBuilderExePathSetting);
			engine.SettingRepository.AddSetting(this.emptySceneClearColorSetting);
		}


		private string GetSettingsFilePath<T>(string folderPath)
		{
			string path = typeof(T).Name + ".json";
			return Path.Combine(folderPath, path);
		}



		private JsonSerializerOptions SettingsSerializationOptions
		{
			get
			{
				return new JsonSerializerOptions
				{
					IgnoreReadOnlyFields = true,
					IgnoreReadOnlyProperties = true,
					WriteIndented = true,
					Converters =
					{
						new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, true),
						new ColorJsonConverter(),
						new SkyboxNameJsonConverter()
					}
				};
			}
		}


		private T LoadSettings<T>(string folderPath) where T : new()
		{
			string settingsFilePath = this.GetSettingsFilePath<T>(folderPath);
			if (File.Exists(settingsFilePath))
			{
				try
				{
					return JsonSerializer.Deserialize<T>(File.ReadAllText(settingsFilePath), this.SettingsSerializationOptions);
				}
				catch (Exception ex)
				{
					MessageBox.Show("Failed to read '" + settingsFilePath + "'. " + ex.Message, "Load Settings Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			return Activator.CreateInstance<T>();
		}


		private void SaveSettings<T>(string folderPath, T settings)
		{
			string settingsFilePath = this.GetSettingsFilePath<T>(folderPath);
			string contents = JsonSerializer.Serialize<T>(settings, this.SettingsSerializationOptions);
			File.WriteAllText(settingsFilePath, contents);
		}


		public void Load(string appUserPath)
		{
			this.AppUserPath = appUserPath;
			this.engine.SettingRepository.Load(appUserPath);
			string directoryName = Path.GetDirectoryName(this.LoadedValuesFilePath);
			this.PostProcessingParams = this.LoadSettings<PostProcessingParams>(directoryName);
			this.CameraSettings = this.LoadSettings<CameraSettings>(directoryName);
			this.BeamEffectSettings = this.LoadSettings<BeamEffectSettings>(directoryName);
			this.BrushSettings = this.LoadSettings<BrushSettings>(directoryName);
			this.UnitSettings = this.LoadSettings<UnitSettings>(directoryName);
			this.GuiSettings = this.LoadSettings<GuiSettings>(directoryName);
			this.MeshSettings = this.LoadSettings<MeshSettings>(directoryName);
			this.ParticleEffectSettings = this.LoadSettings<ParticleEffectSettings>(directoryName);
			this.ScenarioSettings = this.LoadSettings<ScenarioSettings>(directoryName);
			this.SkyboxSettings = this.LoadSettings<SkyboxSettings>(directoryName);
			this.DeathSequenceSettings = this.LoadSettings<DeathSequenceSettings>(directoryName);
		}


		public void Save(string appUserPath)
		{
			this.engine.SettingRepository.Save(appUserPath);
			string directoryName = Path.GetDirectoryName(this.LoadedValuesFilePath);
			this.SaveSettings<PostProcessingParams>(directoryName, this.PostProcessingParams);
			this.SaveSettings<CameraSettings>(directoryName, this.CameraSettings);
			this.SaveSettings<BeamEffectSettings>(directoryName, this.BeamEffectSettings);
			this.SaveSettings<BrushSettings>(directoryName, this.BrushSettings);
			this.SaveSettings<UnitSettings>(directoryName, this.UnitSettings);
			this.SaveSettings<GuiSettings>(directoryName, this.GuiSettings);
			this.SaveSettings<MeshSettings>(directoryName, this.MeshSettings);
			this.SaveSettings<ParticleEffectSettings>(directoryName, this.ParticleEffectSettings);
			this.SaveSettings<ScenarioSettings>(directoryName, this.ScenarioSettings);
			this.SaveSettings<SkyboxSettings>(directoryName, this.SkyboxSettings);
			this.SaveSettings<DeathSequenceSettings>(directoryName, this.DeathSequenceSettings);
		}


		public void Reset()
		{
			string rootDataFolderPath = this.RootDataFolderPath;
			this.engine.SettingRepository.ResetSettings();
			this.RootDataFolderPath = rootDataFolderPath;
			this.PostProcessingParams.ResetToDefault();
			this.CameraSettings.ResetToDefault();
			this.BeamEffectSettings.ResetToDefault();
			this.BrushSettings.ResetToDefault();
			this.UnitSettings.ResetToDefault();
			this.GuiSettings.ResetToDefault();
			this.MeshSettings.ResetToDefault();
			this.ParticleEffectSettings.ResetToDefault();
			this.ScenarioSettings.ResetToDefault();
			this.SkyboxSettings.ResetToDefault();
			this.DeathSequenceSettings.ResetToDefault();
		}


		public static bool IsValidRootDataFolderPath(string path)
		{
			return File.Exists(Path.Combine(path, "localized_text/en.localized_text"));
		}


		public void Fixup()
		{
			this.FixupRootDataFolder();
			this.FixupRootDataSourceSaveFolder();
			this.FixupMeshBuilderExePath();
		}


		private string MakeRelativeToExePath(string relativePath)
		{
			return Path.GetFullPath(relativePath);
		}


		private void FixupRootDataFolder()
		{
			string text = "It should be the same folder the sins2.exe is located and contains sub-folders for all final game data.";
			if (string.IsNullOrEmpty(this.RootDataFolderPath))
			{
				string text2 = this.MakeRelativeToExePath("../../sins2");
				if (ProgramSettings.IsValidRootDataFolderPath(text2))
				{
					this.RootDataFolderPath = text2;
				}
				else
				{
					MessageBox.Show("Sins2 Folder has not been set.\n\nSolarForge requires this folder to load shaders, textures, etc. " + text, "No Sins2 Folder", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					using (AdvancedFolderBrowserDialog advancedFolderBrowserDialog = new AdvancedFolderBrowserDialog())
					{
						if (advancedFolderBrowserDialog.ShowDialog(null) == DialogResult.OK)
						{
							this.RootDataFolderPath = advancedFolderBrowserDialog.DirectoryPath;
						}
					}
				}
			}
			if (!ProgramSettings.IsValidRootDataFolderPath(this.RootDataFolderPath))
			{
				MessageBox.Show(this.RootDataFolderPath + " is not a valid Sins2 Folder.\n\n" + text + ".\n\nSolarForge will not be able to load shaders or other resources. If shaders are not loaded this tool will error and exit when rendering! Fix in Settings...", "Invalid Sins2 Folder", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}


		private void FixupRootDataSourceSaveFolder()
		{
			if (string.IsNullOrEmpty(this.RootDataSourceSaveFolderPath))
			{
				string text = this.MakeRelativeToExePath("../../../sins2_data");
				if (Directory.Exists(text))
				{
					this.RootDataSourceSaveFolderPath = text;
				}
			}
		}


		private void FixupMeshBuilderExePath()
		{
			if (string.IsNullOrEmpty(this.MeshBuilderExePath))
			{
				string text = this.MakeRelativeToExePath("../MeshBuilder/MeshBuilder.exe");
				if (File.Exists(text))
				{
					this.MeshBuilderExePath = text;
				}
			}
		}


		public string ResolveDataSourceSavePath(string path)
		{
			if (this.RootDataSourceSaveFolderPath.Length == 0 || path.Contains(this.RootDataSourceSaveFolderPath))
			{
				return path;
			}
			string fileName = Path.GetFileName(path);
			string text = Directory.GetFiles(this.RootDataSourceSaveFolderPath, fileName, SearchOption.AllDirectories).FirstOrDefault<string>();
			if (text == null)
			{
				throw new InvalidOperationException(fileName + " not found in Data Source Save Path : " + this.RootDataSourceSaveFolderPath);
			}
			return text;
		}


		private string ResolveToolExePath(string relativeExePath, Func<string> getExePath, Action<string> setExePath)
		{
			if (string.IsNullOrEmpty(getExePath()))
			{
				string fullPath = Path.GetFullPath(Path.Combine(Path.GetFullPath(Path.Combine(this.RootDataFolderPath, "../tools")), relativeExePath));
				if (File.Exists(fullPath))
				{
					setExePath(fullPath);
				}
			}
			if (!File.Exists(getExePath()))
			{
				string fileName = Path.GetFileName(relativeExePath);
				MessageBox.Show(fileName + " not found!", "{exeName} not found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				OpenFileDialog openFileDialog = new OpenFileDialog
				{
					CheckPathExists = true,
					Title = "Find " + fileName,
					Filter = fileName + "|" + fileName
				};
				if (openFileDialog.ShowDialog() != DialogResult.OK)
				{
					return "";
				}
				setExePath(openFileDialog.FileName);
			}
			return getExePath();
		}


		public string ResolveMeshBuilderExePath()
		{
			return this.ResolveToolExePath("MeshBuilder/MeshBuilder.exe", () => this.MeshBuilderExePath, delegate(string value)
			{
				this.MeshBuilderExePath = value;
			});
		}


		public string ResolveMeshBuilderOutFolderPath()
		{
			return Path.Combine(this.AppUserPath, "Meshes");
		}


		private Engine engine;


		private StringSetting rootDataFolderPathSetting = new StringSetting("RootDataFolderPath", "");


		private StringSetting rootDataSourceSaveFolderPathSetting = new StringSetting("RootDataSourceSaveFolderPath", "");


		private StringSetting meshBuilderExePathSetting = new StringSetting("MeshBuilderExePath", "");


		private ColorSetting emptySceneClearColorSetting = new ColorSetting("EmptyScene.ClearColor", Color.AliceBlue);
	}
}
