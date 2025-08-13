using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Solar;
using Solar.Math;
using Solar.Scenarios;
using Solar.Utility;
using SolarForge.Utility;

namespace SolarForge.Scenarios
{

	public class ScenarioModel
	{



		public event ScenarioModel.ScenarioChangedDelegate ScenarioChanged;




		public event ScenarioModel.GalaxyChartChangedDelegate GalaxyChartChanged;




		public event ScenarioModel.GalaxyChartComponentsChangedDelegate GalaxyChartComponentsChanged;




		public event ScenarioModel.SelectedGalaxyChartComponentChangedDelegate SelectedGalaxyChartComponentChanged;




		public event ScenarioModel.SelectedGalaxyChartGeneratorParamsComponentChangedDelegate SelectedGalaxyChartGeneratorParamsComponentChanged;




		public event ScenarioModel.ActionTargetGalaxyChartNodeChangedDelegate ActionTargetGalaxyChartNodeChanged;




		public event ScenarioModel.AnyGalaxyChartComponentPropertyChangedDelegate AnyGalaxyChartComponentPropertyChanged;


		public ScenarioModel(Engine engine, ScenarioSettings settings, IJsonBeautifier jsonBeautifier)
		{
			this.engine = engine;
			this.settings = settings;
			this.jsonBeautifier = jsonBeautifier;
			this.undoStack = new List<Scenario>();
		}



		public Scenario Scenario
		{
			get
			{
				return this.scenario;
			}
		}



		public ScenarioSettings Settings
		{
			get
			{
				return this.settings;
			}
		}



		public GalaxyChartGeneratorParams GalaxyChartGeneratorParams
		{
			get
			{
				if (this.scenario != null)
				{
					return this.scenario.GalaxyChartGeneratorParams;
				}
				return null;
			}
		}



		public bool IsGalaxyChartPreview
		{
			get
			{
				return this.GalaxyChart == this.previewGalaxyChart;
			}
		}



		public GalaxyChart GalaxyChart
		{
			get
			{
				if (this.scenario == null)
				{
					return null;
				}
				if (this.scenario.GalaxyChartGeneratorParams != null)
				{
					return this.previewGalaxyChart;
				}
				return this.scenario.GalaxyChart;
			}
		}


		public Color GetPlayerIndexColor(uint playerIndex)
		{
			Random random = new Random((int)playerIndex);
			return Color.FromArgb(random.Next(0, 256), random.Next(0, 256), random.Next(0, 256));
		}



		public bool CanGenerateGalaxyChartPreview
		{
			get
			{
				return this.GalaxyChartGeneratorParams != null;
			}
		}


		public void GenerateGalaxyChartPreview(ulong randomSeed, uint playerCount, bool isUndo)
		{
			if (!this.CanGenerateGalaxyChartPreview)
			{
				throw new InvalidOperationException("!CanMakeGalaxyChartPreview");
			}
			//this.previewGalaxyChart = this.GalaxyChartGeneratorParams.GenerateGalaxyChart(this.fillings, randomSeed, playerCount);
			this.OnGalaxyChartChanged(isUndo); 
		}



		public bool CanBakeGalaxyChartPreview
		{
			get
			{
				return this.previewGalaxyChart != null;
			}
		}


		public void BakeGalaxyChartPreview()
		{
			if (!this.CanBakeGalaxyChartPreview)
			{
				throw new InvalidOperationException("!CanBakeGalaxyChartPreview");
			}
			this.scenario.GalaxyChart = this.previewGalaxyChart;
			this.scenario.GalaxyChart.Skybox = this.scenario.GalaxyChartGeneratorParams.Skybox;
			this.scenario.GalaxyChartGeneratorParams = null;
			this.previewGalaxyChart = null;
			this.OnScenarioChanged(false);
		}



		public bool CanGenerateSimulation
		{
			get
			{
				return this.GalaxyChart != null;
			}
		}


		public void GenerateSimulation()
		{
			if (!this.CanGenerateSimulation)
			{
				throw new InvalidOperationException("!CanGenerateSimulation");
			}
		}


		private void OnGalaxyChartChanged(bool isUndo)
		{
			this.SelectedGalaxyChartComponent = null;
			this.ActionTargetGalaxyChartNode = null;
			ScenarioModel.GalaxyChartChangedDelegate galaxyChartChanged = this.GalaxyChartChanged;
			if (galaxyChartChanged == null)
			{
				return;
			}
			galaxyChartChanged(this.GalaxyChart, isUndo);
		}


		public void LoadScenarioFromFile(string path)
		{
			this.scenario = new Scenario();
			this.scenario.LoadFromFile(path);
			this.ResetUndo();
			this.OnScenarioChanged(false);
		}


		public void LoadScenarioFromFolder(string path)
		{
			this.scenario = new Scenario();
			this.scenario.LoadFromFolder(path);
			this.ResetUndo();
			this.OnScenarioChanged(false);
		}


		public void SaveScenarioToFile(string path)
		{
			if (this.ValidateGalaxyModel())
			{
				this.scenario.SaveToFile(path);
				this.HasUnsavedChanges = false;
			}
		}


		public void SaveScenarioToFolder(string path)
		{
			if (this.ValidateGalaxyModel())
			{
				this.scenario.SaveToFolder(path);
				this.HasUnsavedChanges = false;
			}
		}


		public bool ValidateGalaxyModel()
		{
			List<GalaxyChartNode> allNodes = this.GalaxyChart.GetAllNodes();
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			Dictionary<uint?, bool> dictionary = new Dictionary<uint?, bool>();
			new List<ScenarioViewportControl.PhaseLaneData>();
			for (int i = 0; i < allNodes.Count; i++)
			{
				for (int j = i + 1; j < allNodes.Count; j++)
				{
					GalaxyChartNodeId id = allNodes[i].Id;
					if (id.Equals(allNodes[j].Id))
					{
						string str = "Node ID: ";
						id = allNodes[i].Id;
						string item = str + id.Value.ToString();
						if (!list.Contains(item))
						{
							list.Add(item);
						}
					}
				}
			}
			for (int k = 0; k < this.GalaxyChart.PhaseLanes.Count; k++)
			{
				GalaxyChartPhaseLane galaxyChartPhaseLane = this.GalaxyChart.PhaseLanes[k];
				if (galaxyChartPhaseLane != null)
				{
					for (int l = k + 1; l < this.GalaxyChart.PhaseLanes.Count; l++)
					{
						GalaxyChartPhaseLane galaxyChartPhaseLane2 = this.GalaxyChart.PhaseLanes[l];
						if (galaxyChartPhaseLane2 != null)
						{
							GalaxyChartPhaseLaneId id2 = galaxyChartPhaseLane.Id;
							if (id2.Equals(galaxyChartPhaseLane2.Id))
							{
								string str2 = "Phase Lane ID: ";
								id2 = galaxyChartPhaseLane.Id;
								string item2 = str2 + id2.Value.ToString();
								if (!list2.Contains(item2))
								{
									list2.Add(item2);
								}
							}
						}
					}
				}
			}
			if (list.Count > 0)
			{
				string caption = "Save Error: GalaxyChart Invalid.";
				MessageBox.Show("Duplicate nodes with identical IDs found:\n" + string.Join("\n", list) + "\nFix errors in GalaxyChart to prevent save corruption.", caption);
				return false;
			}
			if (list2.Count > 0)
			{
				string caption2 = "Save Error: GalaxyChart Invalid.";
				MessageBox.Show("Duplicate phase lanes with identical IDs found:\n" + string.Join("\n", list2) + "\nFix errors in GalaxyChart to prevent save corruption.", caption2);
				return false;
			}
			for (int m = 0; m < allNodes.Count; m++)
			{
				if (allNodes[m].OwnerPlayerIndex != null)
				{
					uint? ownerPlayerIndex = allNodes[m].OwnerPlayerIndex;
					if (!dictionary.ContainsKey(ownerPlayerIndex))
					{
						dictionary[ownerPlayerIndex] = false;
					}
					if (allNodes[m].Filling.IsPrimaryFixturePlayerHomePlanet)
					{
						dictionary[ownerPlayerIndex] = true;
					}
				}
			}
			foreach (KeyValuePair<uint?, bool> keyValuePair in dictionary)
			{
				if (!keyValuePair.Value)
				{
					string caption3 = "Save Error: Missing Home Planet.";
					MessageBox.Show(string.Concat(new string[]
					{
						string.Format("Player with OwnerPlayerIndex {0} has no node with a NodeFilling with IsPrimaryFixturePlayerHomePlanet set to True.", keyValuePair.Key),
						Environment.NewLine,
						Environment.NewLine,
						"You may edit existing NodeFillings, or add your own, under Scenario->Edit Fillings.",
						Environment.NewLine,
						Environment.NewLine,
						"(Note: For now, you will have to create a node, save and quit SolarForge, and restart it in order to see a new NodeFilling in the dropdown."
					}), caption3);
					return false;
				}
			}
			if (this.GalaxyChart.Skybox == null || string.IsNullOrWhiteSpace(this.GalaxyChart.Skybox.ToString()))
			{
				string caption4 = "Skybox Error";
				MessageBox.Show("No Skybox set in map. Please set under Properties in the Tree View, and try saving again.", caption4);
				return false;
			}
			return true;
		}


		public void AddNewGalaxyChartGeneratorSolarSystem()
		{
			GalaxyChartGeneratorSolarSystemParams galaxyChartGeneratorSolarSystemParams = this.scenario.GalaxyChartGeneratorParams.AddSolarSystem();
			galaxyChartGeneratorSolarSystemParams.StarFillingName = new GalaxyChartNodeFillingName("random_star");
			galaxyChartGeneratorSolarSystemParams.MaxPlayerCount = 10;
			galaxyChartGeneratorSolarSystemParams.Radius = 1000f;
			this.AddNewGalaxyChartGeneratorPlanetRange(galaxyChartGeneratorSolarSystemParams);
		}


		public void AddNewGalaxyChartGeneratorPlanetRange(GalaxyChartGeneratorSolarSystemParams solarSystem)
		{
			GalaxyChartGeneratorPlanetRangeParams galaxyChartGeneratorPlanetRangeParams = solarSystem.AddPlanetRange();
			galaxyChartGeneratorPlanetRangeParams.FillingName = new GalaxyChartNodeFillingName("random_planet_weighted");
			galaxyChartGeneratorPlanetRangeParams.Count = new IntRange(10);
			galaxyChartGeneratorPlanetRangeParams.SolarSystemRadiusRange = new FloatRange(0.1f, 1f);
		}


		public void CreateNewScenario()
		{
			this.scenario = new Scenario();
			this.scenario.ScenarioInfo.Name = ":New Scenario";
			this.scenario.ScenarioInfo.Description = ":An empty scenario";
			this.scenario.ScenarioInfo.PlayerCount = 10;
			this.scenario.GalaxyChartGeneratorParams = new GalaxyChartGeneratorParams();
			this.scenario.GalaxyChartGeneratorParams.PlayerHomePlanet.FillingName = new GalaxyChartNodeFillingName("player_home_planet");
			this.scenario.GalaxyChartGeneratorParams.PlayerHomePlanet.SolarSystemRadiusRange = new FloatRange(0.4f, 0.8f);
			this.AddNewGalaxyChartGeneratorSolarSystem();
			this.ResetUndo();
			this.OnScenarioChanged(false);
		}



        public Solar.Scenarios.GalaxyChartFillings Fillings
        {
            get 
            {
                return this.fillings;
            } 
        }



        public Engine Engine
		{
			get
			{
				return this.engine;
			}
		}


		private void RefreshFillings()
		{
			if (this.scenario.GalaxyChartFillings == null)
			{
				throw new InvalidOperationException("scenario.GalaxyChartFillings must always exist");
			}
            this.fillings = new Solar.Scenarios.GalaxyChartFillings(); 
            this.fillings.AddFillings(this.engine.GameUniforms.GalaxyChartFillings);
            this.fillings.AddFillings(this.scenario.GalaxyChartFillings);
            GalaxyChartNode.FillingProvider = this.fillings;
            GalaxyChartNodeFillingNameConverter.NameProvider = this.fillings;
            RandomSkyboxFillingNameConverter.NameProvider = this.fillings;
            RandomFixtureFillingNameConverter.NameProvider = this.fillings;
            FixtureFillingNameConverter.NameProvider = this.fillings; 
            NpcFillingNameConverter.NameProvider = this.fillings;
            ArtifactFillingNameConverter.NameProvider = this.fillings;
        }


		private void OnScenarioChanged(bool isUndo)
		{
			this.previewGalaxyChart = null;
			this.OnGalaxyChartChanged(isUndo);
			this.RefreshFillings();
			ScenarioModel.ScenarioChangedDelegate scenarioChanged = this.ScenarioChanged;
			if (scenarioChanged == null)
			{
				return;
			}
			scenarioChanged(this.scenario, isUndo);
		}



		public object SelectedGalaxyChartGeneratorParamsComponent
		{
			set
			{
				this.selectedGalaxyChartGeneratorParamsComponent = value;
				this.OnSelectedGalaxyChartGeneratorParamsComponentChanged();
			}
		}


		private void OnSelectedGalaxyChartGeneratorParamsComponentChanged()
		{
			if (this.SelectedGalaxyChartGeneratorParamsComponentChanged != null)
			{
				this.SelectedGalaxyChartGeneratorParamsComponentChanged(this.selectedGalaxyChartGeneratorParamsComponent);
			}
		}


		public Color GetGalaxyChartNodeColor(GalaxyChartNode node)
		{
			if (node.Filling != null)
			{
				return node.Filling.EditorColor;
			}
			return Color.Magenta;
		}




		public object SelectedGalaxyChartComponent
		{
			get
			{
				return this.selectedGalaxyChartComponent;
			}
			set
			{
				this.selectedGalaxyChartComponent = value;
				this.OnSelectedGalaxyChartComponentChanged();
			}
		}




		public GalaxyChartNode ActionTargetGalaxyChartNode
		{
			get
			{
				return this.actionTargetGalaxyChartNode;
			}
			set
			{
				this.actionTargetGalaxyChartNode = value;
				ScenarioModel.ActionTargetGalaxyChartNodeChangedDelegate actionTargetGalaxyChartNodeChanged = this.ActionTargetGalaxyChartNodeChanged;
				if (actionTargetGalaxyChartNodeChanged == null)
				{
					return;
				}
				actionTargetGalaxyChartNodeChanged(value);
			}
		}



		public GalaxyChartNode SelectedGalaxyChartNode
		{
			get
			{
				return this.SelectedGalaxyChartComponent as GalaxyChartNode;
			}
		}



		public GalaxyChartPhaseLane SelectedGalaxyChartPhaseLane
		{
			get
			{
				return this.SelectedGalaxyChartComponent as GalaxyChartPhaseLane;
			}
		}


		public void AddNode(GalaxyChartNodeFillingName fillingName, FloatPoint position, GalaxyChartNode parentNode)
		{
			GalaxyChartNode nodeA = this.GalaxyChart.AddNode(fillingName, position, parentNode);
			if (parentNode != null && parentNode.ParentDepth > 0)
			{
				this.GalaxyChart.AddPhaseLane(nodeA, parentNode);
			}
			this.AddToUndoStack();
			this.OnGalaxyChartComponentsChanged();
		}


		public void AddPhaseLane(GalaxyChartNode nodeA, GalaxyChartNode nodeB)
		{
			this.GalaxyChart.AddPhaseLane(nodeA, nodeB);
			this.AddToUndoStack();
			this.OnGalaxyChartComponentsChanged();
		}


		public void RemovePhaseLane(GalaxyChartPhaseLane phaseLane)
		{
			this.GalaxyChart.RemovePhaseLane(phaseLane);
			this.AddToUndoStack();
			this.OnGalaxyChartComponentRemoved(phaseLane);
		}


		public void RemoveNode(GalaxyChartNode node)
		{
			this.GalaxyChart.RemoveNode(node);
			this.AddToUndoStack();
			this.OnGalaxyChartComponentRemoved(node);
		}


		public void MoveNodeToRoot(GalaxyChartNode node)
		{
			this.GalaxyChart.MoveNodeToRoot(node);
			this.AddToUndoStack();
			this.OnGalaxyChartComponentsChanged();
		}


		public void MoveNodeToParent(GalaxyChartNode node, GalaxyChartNode parent)
		{
			this.GalaxyChart.MoveNodeToParent(node, parent);
			this.AddToUndoStack();
			this.OnGalaxyChartComponentsChanged();
		}


		public void TryDeleteSelectedGalaxyChartComponent()
		{
			if (this.SelectedGalaxyChartComponent != null)
			{
				GalaxyChartNode galaxyChartNode = this.SelectedGalaxyChartComponent as GalaxyChartNode;
				if (galaxyChartNode != null)
				{
					this.GalaxyChart.RemoveNode(galaxyChartNode);
				}
				else
				{
					GalaxyChartPhaseLane galaxyChartPhaseLane = this.SelectedGalaxyChartComponent as GalaxyChartPhaseLane;
					if (galaxyChartPhaseLane != null)
					{
						this.GalaxyChart.RemovePhaseLane(galaxyChartPhaseLane);
					}
				}
				this.SelectedGalaxyChartComponent = null;
			}
			this.AddToUndoStack();
			this.OnGalaxyChartComponentsChanged();
		}


		public GalaxyChartPhaseLane FindAndRemovePhaseLaneBetweenNodes(GalaxyChartNode nodeA, GalaxyChartNode nodeB)
		{
			if (this.GalaxyChart == null)
			{
				return null;
			}
			GalaxyChartPhaseLane galaxyChartPhaseLane = null;
			for (int i = 0; i < this.GalaxyChart.PhaseLanes.Count; i++)
			{
				GalaxyChartPhaseLane galaxyChartPhaseLane2 = this.GalaxyChart.PhaseLanes[i];
				if ((object.Equals(nodeA.Id, galaxyChartPhaseLane2.NodeAId) && object.Equals(nodeB.Id, galaxyChartPhaseLane2.NodeBId)) || (object.Equals(nodeA.Id, galaxyChartPhaseLane2.NodeBId) && object.Equals(nodeB.Id, galaxyChartPhaseLane2.NodeAId)))
				{
					galaxyChartPhaseLane = galaxyChartPhaseLane2;
					break;
				}
			}
			if (galaxyChartPhaseLane != null)
			{
				this.RemovePhaseLane(galaxyChartPhaseLane);
			}
			return galaxyChartPhaseLane;
		}


		private void OnGalaxyChartComponentRemoved(object component)
		{
			if (this.SelectedGalaxyChartComponent == component)
			{
				this.SelectedGalaxyChartComponent = null;
			}
			this.OnGalaxyChartComponentsChanged();
		}


		private void OnGalaxyChartComponentsChanged()
		{
			ScenarioModel.GalaxyChartComponentsChangedDelegate galaxyChartComponentsChanged = this.GalaxyChartComponentsChanged;
			if (galaxyChartComponentsChanged == null)
			{
				return;
			}
			galaxyChartComponentsChanged();
		}


		private void OnSelectedGalaxyChartComponentChanged()
		{
			ScenarioModel.SelectedGalaxyChartComponentChangedDelegate selectedGalaxyChartComponentChanged = this.SelectedGalaxyChartComponentChanged;
			if (selectedGalaxyChartComponentChanged == null)
			{
				return;
			}
			selectedGalaxyChartComponentChanged(this.selectedGalaxyChartComponent);
		}


		public void OnSelectedGalaxyChartComponentPropertyChanged(PropertyValueChangedEventArgs e)
		{
			if (this.GalaxyChart != null)
			{
				this.OnAnyGalaxyChartComponentPropertyChanged();
			}
		}


		private void OnAnyGalaxyChartComponentPropertyChanged()
		{
			ScenarioModel.AnyGalaxyChartComponentPropertyChangedDelegate anyGalaxyChartComponentPropertyChanged = this.AnyGalaxyChartComponentPropertyChanged;
			if (anyGalaxyChartComponentPropertyChanged == null)
			{
				return;
			}
			anyGalaxyChartComponentPropertyChanged();
		}


		private void ResetUndo()
		{
			this.undoStack.Clear();
			this.undoPosition = null;
			this.nextUndo = this.scenario.MakeClone();
			Action undoChanged = this.UndoChanged;
			if (undoChanged == null)
			{
				return;
			}
			undoChanged();
		}


		public void AddToUndoStack()
		{
			if (this.undoPosition != null)
			{
				this.undoStack.RemoveRange(this.undoPosition.Value, this.undoStack.Count - this.undoPosition.Value);
				this.undoPosition = null;
			}
			this.undoStack.Add(this.nextUndo);
			this.nextUndo = this.scenario.MakeClone();
			Action undoChanged = this.UndoChanged;
			if (undoChanged == null)
			{
				return;
			}
			undoChanged();
		}



		public bool CanUndo
		{
			get
			{
				return this.undoStack.Count > 0 && (this.undoPosition == null || this.undoPosition.Value > 0);
			}
		}


		public void TryUndo()
		{
			if (this.CanUndo)
			{
				if (this.undoPosition == null)
				{
					this.undoPosition = new int?(this.undoStack.Count - 1);
					this.undoStack.Add(this.nextUndo);
				}
				else
				{
					this.undoPosition = new int?(this.undoPosition.Value - 1);
				}
				this.SyncToUndoPosition();
			}
		}



		public bool CanRedo
		{
			get
			{
				return this.undoPosition != null && this.undoPosition.Value < this.undoStack.Count - 1;
			}
		}


		public void TryRedo()
		{
			if (this.CanRedo)
			{
				this.undoPosition = new int?(this.undoPosition.Value + 1);
				this.SyncToUndoPosition();
			}
		}


		private void SyncToUndoPosition()
		{
			this.scenario.ApplyCopy(this.undoStack[this.undoPosition.Value]);
			this.nextUndo = this.undoStack[this.undoPosition.Value];
			this.OnScenarioChanged(true);
			Action undoChanged = this.UndoChanged;
			if (undoChanged == null)
			{
				return;
			}
			undoChanged();
		}




		public event Action UndoChanged;




		public bool HasUnsavedChanges { get; private set; }


		private Engine engine;


		private ScenarioSettings settings;


		private IJsonBeautifier jsonBeautifier;


		private Scenario scenario;


		private GalaxyChart previewGalaxyChart;


		private Solar.Scenarios.GalaxyChartFillings fillings;  


		private object selectedGalaxyChartGeneratorParamsComponent;


		private object selectedGalaxyChartComponent;


		private GalaxyChartNode actionTargetGalaxyChartNode;


		private List<Scenario> undoStack;


		private int? undoPosition;


		private Scenario nextUndo;



		public delegate void ScenarioChangedDelegate(Scenario scenario, bool isUndo);



		public delegate void GalaxyChartChangedDelegate(GalaxyChart galaxyChart, bool isUndo);



		public delegate void GalaxyChartComponentsChangedDelegate();



		public delegate void SelectedGalaxyChartComponentChangedDelegate(object selectedComponent);



		public delegate void SelectedGalaxyChartGeneratorParamsComponentChangedDelegate(object selectedComponent);



		public delegate void ActionTargetGalaxyChartNodeChangedDelegate(GalaxyChartNode node);



		public delegate void AnyGalaxyChartComponentPropertyChangedDelegate();
	}
}
