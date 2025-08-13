using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Solar;
using SolarForge.BeamEffects;
using SolarForge.Brushes;
using SolarForge.DeathSequences;
using SolarForge.GalaxyChartFillings;
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

	public class ProgramModel : IDisposable, IJsonBeautifier
	{



		public event ProgramModel.CurrentEditorTypeChangedDelegate CurrentEditorTypeChanged;




		public event ProgramModel.CurrentFileOrFolderPathChangedDelegate CurrentFileOrFolderPathChanged;




		public event ProgramModel.HasUnsavedChangesChangedDelegate HasUnsavedChangesChanged;




		public Engine Engine { get; private set; }




		public ProgramSettings Settings { get; private set; }




		public BeamEffectModel BeamEffectModel { get; private set; }




		public BrushModel BrushModel { get; private set; }




		public UnitModel UnitModel { get; private set; }




		public GuiModel GuiModel { get; private set; }




		public MeshModel MeshModel { get; private set; }




		public ParticleEffectModel ParticleEffectModel { get; private set; }




		public ScenarioModel ScenarioModel { get; private set; }




		public SkyboxModel SkyboxModel { get; private set; }




		public DeathSequenceModel DeathSequenceModel { get; private set; }


		public ProgramModel()
		{
			this.Engine = new Engine();
			this.Settings = new ProgramSettings(this.Engine);
		}


		public void CreateModels()
		{
			this.BeamEffectModel = new BeamEffectModel(this.Engine, this.Settings.PostProcessingParams, this.Settings.CameraSettings, this.Settings.BeamEffectSettings, this);
			this.BrushModel = new BrushModel(this.Engine, this.Settings.PostProcessingParams, this.Settings.CameraSettings, this.Settings.BrushSettings, this);
			this.UnitModel = new UnitModel(this.Engine, this.Settings.PostProcessingParams, this.Settings.CameraSettings, this.Settings.UnitSettings, this.Settings, this);
			this.GuiModel = new GuiModel(this.Settings.GuiSettings, this);
			this.MeshModel = new MeshModel(this.Engine, this.Settings.PostProcessingParams, this.Settings.CameraSettings, this.Settings.MeshSettings, this);
			this.ParticleEffectModel = new ParticleEffectModel(this.Engine, this.Settings.PostProcessingParams, this.Settings.CameraSettings, this.Settings.ParticleEffectSettings, this);
			this.ParticleEffectModel.HasUnsavedChangesChanged += delegate()
			{
				this.HasUnsavedChangesChanged();
			};
			this.ScenarioModel = new ScenarioModel(this.Engine, this.Settings.ScenarioSettings, this);
			this.SkyboxModel = new SkyboxModel(this.Engine, this.Settings.PostProcessingParams, this.Settings.CameraSettings, this.Settings.SkyboxSettings);
			this.DeathSequenceModel = new DeathSequenceModel(this.Engine, this.Settings.PostProcessingParams, this.Settings.CameraSettings, this.Settings.DeathSequenceSettings);
			this._sceneModels = new List<SceneModel>
			{
				this.BeamEffectModel,
				this.BrushModel,
				this.UnitModel,
				this.MeshModel,
				this.ParticleEffectModel,
				this.SkyboxModel,
				this.DeathSequenceModel
			};
		}


		public GalaxyChartFillingsEditorModel NewGalaxyChartFillingsEditorModel()
		{
			GalaxyChartFillingsEditorModel galaxyChartFillingsEditorModel = new GalaxyChartFillingsEditorModel();
			List<GalaxyChartFillingsSource> list = new List<GalaxyChartFillingsSource>();
			if (this.ScenarioModel.Scenario != null)
			{
				list.Add(new GalaxyChartFillingsSource
				{
					Name = "Scenario",
					Fillings = this.ScenarioModel.Scenario.GalaxyChartFillings,
					ReadOnly = false 
				});
			}
			list.Add(new GalaxyChartFillingsSource
			{
				Name = "Uniforms", 
				Fillings = this.Engine.GameUniforms.GalaxyChartFillings,
				ReadOnly = false
			});
			galaxyChartFillingsEditorModel.FillingsSources = list;
			return galaxyChartFillingsEditorModel;
		}


		public void Dispose()
		{
			if (this._sceneModels != null)
			{
				foreach (SceneModel sceneModel in this._sceneModels)
				{
					sceneModel.Dispose();
				}
			}
			this.Engine.Teardown();
		}


		public void SetViewportSize(Size viewportSize)
		{
			foreach (SceneModel sceneModel in this._sceneModels)
			{
				sceneModel.SetViewportSize(viewportSize);
			}
		}


		public void Update()
		{
			this.Engine.Pump();
			SceneModel currentSceneModel = this.CurrentSceneModel;
			if (currentSceneModel != null)
			{
				currentSceneModel.Update();
			}
		}


		public void UpdateAndRenderScene(bool hasSetupErrors, bool canPan)
		{
			SceneModel currentSceneModel = this.CurrentSceneModel;
			if (currentSceneModel != null)
			{
				currentSceneModel.UpdateAndRenderScene(canPan);
				return;
			}
			if (!hasSetupErrors)
			{
				this.RenderEmptyScene();
			}
		}



		public string EditorStatusText
		{
			get
			{
				SceneModel currentSceneModel = this.CurrentSceneModel;
				if (currentSceneModel != null)
				{
					return currentSceneModel.StatusText;
				}
				return "";
			}
		}


		private void RenderEmptyScene()
		{
			this.Engine.RenderEmptyScene(this.Settings.PostProcessingParams, this.Settings.EmptySceneClearColor);
		}


		public void HandleMouseWheel(MouseEventArgs e)
		{
			SceneModel currentSceneModel = this.CurrentSceneModel;
			if (currentSceneModel != null)
			{
				currentSceneModel.HandleMouseWheel(e);
			}
		}


		public void HandleMouseMove(int deltaX, int deltaY, MouseButtons buttons)
		{
			SceneModel currentSceneModel = this.CurrentSceneModel;
			if (currentSceneModel != null)
			{
				currentSceneModel.HandleMouseMove(deltaX, deltaY, buttons);
			}
		}


		public void HandleMouseClick(Point point, MouseButtons button)
		{
			SceneModel currentSceneModel = this.CurrentSceneModel;
			if (currentSceneModel != null)
			{
				currentSceneModel.HandleMouseClick(point, button);
			}
		}


		private string GetFileTypeFilter(FileType fileType)
		{
			string fileTypeName = FileTypeHelpers.GetFileTypeName(fileType);
			string text = FileTypeHelpers.MakeFileTypeExtensionsListString(FileTypeHelpers.GetFileTypeExtensions(fileType));
			return string.Concat(new string[]
			{
				fileTypeName,
				" (",
				text,
				")|",
				text
			});
		}


		public string BrowseToAndLoadFile()
		{
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.AddExtension = true;
				openFileDialog.FileName = "";
				IEnumerable<FileType> enumerable = Enum.GetValues(typeof(FileType)).Cast<FileType>();
				List<string> list = new List<string>();
				List<string> list2 = new List<string>();
				foreach (FileType fileType in enumerable)
				{
					list2.AddRange(FileTypeHelpers.GetFileTypeExtensions(fileType));
				}
				string text = FileTypeHelpers.MakeFileTypeExtensionsListString(list2);
				list.Add("All Sins2 files (" + text + ")|" + text);
				list.Add("All files (*.*)|*.*");
				foreach (FileType fileType2 in enumerable)
				{
					string text2 = FileTypeHelpers.MakeFileTypeExtensionsListString(FileTypeHelpers.GetFileTypeExtensions(fileType2));
					list.Add(string.Format("{0} files ({1})|{2}", fileType2, text2, text2));
				}
				openFileDialog.Filter = string.Join("|", list);
				openFileDialog.Multiselect = false;
				openFileDialog.RestoreDirectory = true;
				openFileDialog.Title = "Open File";
				openFileDialog.ValidateNames = true;
				if (openFileDialog.ShowDialog() == DialogResult.OK && this.LoadFile(openFileDialog.FileName, true))
				{
					return openFileDialog.FileName;
				}
			}
			return null;
		}


		public void SaveScenarioToFolder(string path)
		{
			this.ScenarioModel.SaveScenarioToFolder(path);
		}


		public void SaveScenarioToFolderAs()
		{
			using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
			{
				this.TrySetFolderBrowserDialogSelectedPathToRootDataSource(folderBrowserDialog);
				folderBrowserDialog.ShowNewFolderButton = true;
				if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
				{
					this.ScenarioModel.SaveScenarioToFolder(folderBrowserDialog.SelectedPath);
					this.CurrentFileOrFolderPath = folderBrowserDialog.SelectedPath;
				}
			}
		}


		private void TrySetFolderBrowserDialogSelectedPathToRootDataSource(FolderBrowserDialog dialog)
		{
			string text = Path.Combine(this.Settings.RootDataSourceSaveFolderPath, "data", "scenarios");
			if (Directory.Exists(text))
			{
				dialog.SelectedPath = text;
			}
		}


		public string BrowseToAndLoadScenarioFolder()
		{
			using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
			{
				this.TrySetFolderBrowserDialogSelectedPathToRootDataSource(folderBrowserDialog);
				if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
				{
					this.LoadScenarioFolder(folderBrowserDialog.SelectedPath);
					return folderBrowserDialog.SelectedPath;
				}
			}
			return null;
		}


		public string RunExternalExe(string filename, string arguments = null)
		{
			string text = "'" + filename + (string.IsNullOrEmpty(arguments) ? string.Empty : (" " + arguments)) + "'";
			Trace.WriteLine("Running External Exe : " + text);
			Process process = new Process();
			process.StartInfo.FileName = filename;
			if (!string.IsNullOrEmpty(arguments))
			{
				process.StartInfo.Arguments = arguments;
			}
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.RedirectStandardError = true;
			process.StartInfo.RedirectStandardOutput = true;
			StringBuilder stdOutput = new StringBuilder();
			process.OutputDataReceived += delegate(object sender, DataReceivedEventArgs args)
			{
				stdOutput.AppendLine(args.Data);
			};
			string value = null;
			try
			{
				process.Start();
				process.BeginOutputReadLine();
				value = process.StandardError.ReadToEnd();
				process.WaitForExit();
			}
			catch (Exception ex)
			{
				throw new Exception("OS error while executing " + text + ": " + ex.Message, ex);
			}
			if (process.ExitCode == 0)
			{
				return stdOutput.ToString();
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(value))
			{
				stringBuilder.AppendLine(value);
			}
			if (stdOutput.Length != 0)
			{
				stringBuilder.AppendLine("Std output:");
				stringBuilder.AppendLine(stdOutput.ToString());
			}
			string[] array = new string[5];
			array[0] = text;
			array[1] = " finished with exit code = ";
			array[2] = process.ExitCode.ToString();
			array[3] = ": ";
			int num = 4;
			StringBuilder stringBuilder2 = stringBuilder;
			array[num] = ((stringBuilder2 != null) ? stringBuilder2.ToString() : null);
			throw new Exception(string.Concat(array));
		}


		public string BuildMesh(string path)
		{
			string text = this.Settings.ResolveMeshBuilderExePath();
			if (!string.IsNullOrEmpty(text))
			{
				string text2 = Path.Combine(this.Settings.ResolveMeshBuilderOutFolderPath(), Path.GetFileNameWithoutExtension(path));
				Directory.CreateDirectory(text2);
				this.RunExternalExe(text, string.Concat(new string[]
				{
					"--input_path ",
					path,
					" --output_folder_path ",
					text2,
					" --mesh_output_format json --fill_triangle_facing_grid"
				}));
				return Path.Combine(text2, Path.GetFileNameWithoutExtension(path) + ".mesh_json");
			}
			return null;
		}


		public void LoadScenarioFolder(string path)
		{
			this.CurrentEditorType = new EditorType?(EditorType.Scenario);
			this.CurrentFileOrFolderPath = path;
			this.ScenarioModel.LoadScenarioFromFolder(path);
		}



		public SceneModel CurrentSceneModel
		{
			get
			{
				EditorType? currentEditorType = this.CurrentEditorType;
				if (currentEditorType != null)
				{
					switch (currentEditorType.GetValueOrDefault())
					{
					case EditorType.ParticleEffect:
						return this.ParticleEffectModel;
					case EditorType.BeamEffect:
						return this.BeamEffectModel;
					case EditorType.Mesh:
						return this.MeshModel;
					case EditorType.Skybox:
						return this.SkyboxModel;
					case EditorType.Brush:
						return this.BrushModel;
					case EditorType.Unit:
						return this.UnitModel;
					case EditorType.DeathSequence:
						return this.DeathSequenceModel;
					}
				}
				return null;
			}
		}



		public SceneModel3d CurrentSceneModel3d
		{
			get
			{
				return this.CurrentSceneModel as SceneModel3d;
			}
		}



		public bool HasUnsavedChanges
		{
			get
			{
				EditorType? currentEditorType = this.CurrentEditorType;
				EditorType editorType = EditorType.ParticleEffect;
				return (currentEditorType.GetValueOrDefault() == editorType & currentEditorType != null) && this.ParticleEffectModel.HasUnsavedChanges;
			}
		}


		public bool LoadFile(string path, bool showLoadFailErrorMessage = true)
		{
			bool result = true;
			try
			{
				FileType? fileType = this.TryGetFileTypeFromPathExtension(path);
				if (fileType != null)
				{
					if (fileType.Value == FileType.Gltf || fileType.Value == FileType.Sins1Mesh)
					{
						string text = this.BuildMesh(path);
						if (text != null)
						{
							this.CurrentEditorType = new EditorType?(EditorType.Mesh);
							this.CurrentFileOrFolderPath = text;
							this.MeshModel.LoadMesh(text, null);
						}
					}
					else
					{
						this.CurrentFileOrFolderPath = path;
						if (fileType.Value == FileType.ParticleEffect)
						{
							this.CurrentEditorType = new EditorType?(EditorType.ParticleEffect);
							this.ParticleEffectModel.LoadParticleEffect(path);
						}
						else if (fileType.Value == FileType.BeamEffect)
						{
							this.CurrentEditorType = new EditorType?(EditorType.BeamEffect);
							this.BeamEffectModel.LoadBeamEffect(path);
						}
						else if (fileType.Value == FileType.Mesh)
						{
							this.CurrentEditorType = new EditorType?(EditorType.Mesh);
							this.MeshModel.LoadMesh(path, null);
						}
						else if (fileType.Value == FileType.Skybox)
						{
							this.CurrentEditorType = new EditorType?(EditorType.Skybox);
							this.SkyboxModel.LoadSkybox(path);
						}
						else if (fileType.Value == FileType.Scenario)
						{
							this.CurrentEditorType = new EditorType?(EditorType.Scenario);
							this.ScenarioModel.LoadScenarioFromFile(path);
						}
						else if (fileType.Value == FileType.Gui)
						{
							this.CurrentEditorType = new EditorType?(EditorType.Gui);
							this.GuiModel.LoadGui(path);
						}
						else if (fileType.Value == FileType.Brush)
						{
							this.CurrentEditorType = new EditorType?(EditorType.Brush);
							this.BrushModel.LoadBrush(path);
						}
						else if (fileType.Value == FileType.Unit)
						{
							this.CurrentEditorType = new EditorType?(EditorType.Unit);
							this.UnitModel.LoadEntity(path);
						}
						else
						{
							if (fileType.Value != FileType.DeathSequence)
							{
								throw new NotImplementedException();
							}
							this.CurrentEditorType = new EditorType?(EditorType.DeathSequence);
							this.DeathSequenceModel.LoadDeathSequence(path);
						}
					}
				}
				else
				{
					MessageBox.Show("Unknown file type '" + path + "'");
				}
			}
			catch (Exception ex)
			{
				if (showLoadFailErrorMessage)
				{
					MessageBox.Show("Failed to load '" + path + "' : " + ex.Message, "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				else
				{
					Trace.WriteLine("Failed to load '" + path + "' : " + ex.Message);
				}
				result = false;
			}
			return result;
		}


		public void CreateNewParticleEffect()
		{
			this.ParticleEffectModel.CreateNewParticleEffect();
			this.CurrentFileOrFolderPath = null;
			this.CurrentEditorType = new EditorType?(EditorType.ParticleEffect);
		}


		public void CreateNewBeamEffect()
		{
			this.BeamEffectModel.CreateNewBeamEffect();
			this.CurrentFileOrFolderPath = null;
			this.CurrentEditorType = new EditorType?(EditorType.BeamEffect);
		}


		public void CreateNewScenario()
		{
			this.ScenarioModel.CreateNewScenario();
			this.CurrentFileOrFolderPath = null;
			this.CurrentEditorType = new EditorType?(EditorType.Scenario);
		}


		public void CreateNewBrush()
		{
			this.BrushModel.CreateNewBrush();
			this.CurrentFileOrFolderPath = null;
			this.CurrentEditorType = new EditorType?(EditorType.Brush);
		}


		public void Save()
		{
			if (this.CurrentFileOrFolderPath == null)
			{
				this.SaveFileAs();
				return;
			}
			if (Directory.Exists(this.CurrentFileOrFolderPath))
			{
				this.SaveScenarioToFolder(this.CurrentFileOrFolderPath);
				return;
			}
			this.SaveFileToPath(this.CurrentFileOrFolderPath);
		}



		public bool CanSave
		{
			get
			{
				return this.CurrentEditorType != null && (this.CurrentEditorType.Value == EditorType.ParticleEffect || this.CurrentEditorType.Value == EditorType.BeamEffect || this.CurrentEditorType.Value == EditorType.Scenario || this.CurrentEditorType.Value == EditorType.Gui || this.CurrentEditorType.Value == EditorType.Brush || this.CurrentEditorType.Value == EditorType.Unit);
			}
		}


		public void SaveFileAs()
		{
			if (this.CurrentEditorType != null)
			{
				FileType fileTypeFromEditorType = FileTypeHelpers.GetFileTypeFromEditorType(this.CurrentEditorType.Value);
				using (SaveFileDialog saveFileDialog = new SaveFileDialog())
				{
					saveFileDialog.AddExtension = true;
					if (this.CurrentFileOrFolderPath != null && File.Exists(this.CurrentFileOrFolderPath))
					{
						saveFileDialog.InitialDirectory = Path.GetDirectoryName(this.CurrentFileOrFolderPath);
						saveFileDialog.FileName = Path.GetFileName(this.CurrentFileOrFolderPath);
					}
					saveFileDialog.Filter = this.GetFileTypeFilter(fileTypeFromEditorType);
					saveFileDialog.OverwritePrompt = true;
					saveFileDialog.RestoreDirectory = true;
					saveFileDialog.ValidateNames = true;
					saveFileDialog.Title = "Save " + FileTypeHelpers.GetFileTypeName(fileTypeFromEditorType);
					if (saveFileDialog.ShowDialog() == DialogResult.OK)
					{
						this.SaveFileToPath(saveFileDialog.FileName);
					}
				}
			}
		}


		private void SaveFileToPath(string path)
		{
			if (this.CurrentEditorType != null)
			{
				if (this.CurrentEditorType.Value == EditorType.ParticleEffect)
				{
					this.ParticleEffectModel.SaveParticleEffect(path);
				}
				else if (this.CurrentEditorType.Value == EditorType.BeamEffect)
				{
					this.BeamEffectModel.SaveBeamEffect(path);
				}
				else if (this.CurrentEditorType.Value == EditorType.Scenario)
				{
					this.ScenarioModel.SaveScenarioToFile(path);
				}
				else if (this.CurrentEditorType.Value == EditorType.Gui)
				{
					this.GuiModel.SaveGui(path);
				}
				else if (this.CurrentEditorType.Value == EditorType.Brush)
				{
					this.BrushModel.SaveBrush(path);
				}
				else
				{
					if (this.CurrentEditorType.Value != EditorType.Unit)
					{
						throw new NotImplementedException();
					}
					this.UnitModel.SaveEntity(path);
				}
				this.CurrentFileOrFolderPath = path;
				return;
			}
			MessageBox.Show("Nothing to save to '" + path + "'");
		}




		public string CurrentFileOrFolderPath
		{
			get
			{
				return this._currentFileOrFolderPath;
			}
			set
			{
				this._currentFileOrFolderPath = value;
				if (this.CurrentFileOrFolderPathChanged != null)
				{
					this.CurrentFileOrFolderPathChanged(value);
				}
			}
		}




		public EditorType? CurrentEditorType
		{
			get
			{
				return this._currentEditorType;
			}
			set
			{
				this._currentEditorType = value;
				if (this.CurrentEditorTypeChanged != null)
				{
					this.CurrentEditorTypeChanged(value);
				}
				SceneModel currentSceneModel = this.CurrentSceneModel;
				if (currentSceneModel != null)
				{
					currentSceneModel.OnVisible();
				}
			}
		}


		public FileType? TryGetFileTypeFromPathExtension(string path)
		{
			if (path != null)
			{
				string value = Path.GetExtension(path).ToLower();
				foreach (FileType fileType in Enum.GetValues(typeof(FileType)).Cast<FileType>())
				{
					if (FileTypeHelpers.GetFileTypeExtensions(fileType).Contains(value))
					{
						return new FileType?(fileType);
					}
				}
			}
			return null;
		}


		public void BeautifyJson(string path)
		{
			string str = "run-beautify.py";
			Process process = new Process();
			process.StartInfo = new ProcessStartInfo
			{
				FileName = "py",
				Arguments = str + " " + path,
				UseShellExecute = false,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				CreateNoWindow = true
			};
			process.Start();
			process.StandardOutput.ReadToEnd();
			string str2 = process.StandardError.ReadToEnd();
			process.WaitForExit();
			if (process.ExitCode != 0)
			{
				MessageBox.Show("BeautifyJson returned non-zero exit code\n\n" + str2, "BeautifyJson Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}


		private List<SceneModel> _sceneModels;


		private EditorType? _currentEditorType;


		private string _currentFileOrFolderPath;



		public delegate void CurrentEditorTypeChangedDelegate(EditorType? fileType);



		public delegate void CurrentFileOrFolderPathChangedDelegate(string path);



		public delegate void HasUnsavedChangesChangedDelegate();
	}
}
