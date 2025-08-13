using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Solar.Scenarios;

namespace SolarForge.Scenarios
{

	public class GalaxyChartGeneratorControl : UserControl
	{

		public GalaxyChartGeneratorControl()
		{
			this.InitializeComponent();
			this.random = new Random();
			this.MakeNewRandomSeed();
			this.previewPlayerCountComboBox.Text = "10";
			this.previewPlayerCountComboBox.Items.Add("1");
			this.previewPlayerCountComboBox.Items.Add("2");
			this.previewPlayerCountComboBox.Items.Add("4");
			this.previewPlayerCountComboBox.Items.Add("6");
			this.previewPlayerCountComboBox.Items.Add("8");
			this.previewPlayerCountComboBox.Items.Add("10");
			this.treeControl = new GalaxyChartGeneratorParamsTreeControl();
			this.treeControl.Dock = DockStyle.Fill;
			this.splitContainer.Panel1.Controls.Add(this.treeControl);
			this.previewGalaxyChartEditorControl = new GalaxyChartEditorControl();
			this.previewGalaxyChartEditorControl.Visible = true;
			this.previewGalaxyChartEditorControl.Dock = DockStyle.Fill;
			this.previewGalaxyChartEditorPanel.Controls.Add(this.previewGalaxyChartEditorControl);
			this.bottomButton.Click += this.BakeButtonActive;
			this.propertyGrid.PropertyValueChanged += delegate(object s, PropertyValueChangedEventArgs e)
			{
				if (e.ChangedItem.Label.Contains("FillingName"))
				{
					this.treeControl.HandleAnyFillingNameChanged();
				}
			};
		}



		public ScenarioModel Model
		{
			set
			{
				this.model = value;
				this.model.ScenarioChanged += this.Model_ScenarioChanged;
				this.model.SelectedGalaxyChartGeneratorParamsComponentChanged += this.ScenarioModel_SelectedGalaxyChartGeneratorParamsComponentChanged;
				this.model.ScenarioChanged += this.OnCanBakeGalaxyChartPreviewChanged;
				this.treeControl.Model = value;
				this.previewGalaxyChartEditorControl.Model = value;
				this.UpdateBottomButton();
			}
		}


		private void OnCanBakeGalaxyChartPreviewChanged(Scenario scenario, bool isUndo)
		{
			this.UpdateBottomButton();
		}


		private void UpdateBottomButton()
		{
			if (this.model != null && this.model.CanBakeGalaxyChartPreview)
			{
				this.bottomButton.Text = "Unbaked Preview";
				this.bottomButton.Font = new Font(this.bottomButton.Font.FontFamily, 16f);
				this.bottomButton.ForeColor = Color.Red;
				return;
			}
			this.bottomButton.Text = "Baked";
			this.bottomButton.Font = new Font(this.bottomButton.Font.FontFamily, 16f);
			this.bottomButton.ForeColor = Color.Black;
		}


		private void BakeButtonActive(object sender, EventArgs e)
		{
			if (this.model != null && this.model.CanBakeGalaxyChartPreview)
			{
				this.model.BakeGalaxyChartPreview();
			}
		}


		private void Model_ScenarioChanged(Scenario scenario, bool isUndo)
		{
			if (this.model.CanGenerateGalaxyChartPreview)
			{
				this.GeneratePreview(isUndo);
			}
		}


		private void MakeNewRandomSeed()
		{
			this.previewRandomSeedTextBox.Text = ((uint)this.random.Next()).ToString();
		}


		private void ScenarioModel_SelectedGalaxyChartGeneratorParamsComponentChanged(object selectedComponent)
		{
			this.propertyGrid.SelectedObject = selectedComponent;
		}


		private static uint ParseUint(string text)
		{
			uint result = 0U;
			uint.TryParse(text, out result);
			return result;
		}


		private static ulong ParseUlong(string text)
		{
			ulong result = 0UL;
			ulong.TryParse(text, out result);
			return result;
		}



		private ulong PreviewRandomSeed
		{
			get
			{
				return GalaxyChartGeneratorControl.ParseUlong(this.previewRandomSeedTextBox.Text);
			}
		}



		private uint PreviewPlayerCount
		{
			get
			{
				return GalaxyChartGeneratorControl.ParseUint(this.previewPlayerCountComboBox.Text);
			}
		}


		private void GeneratePreview(bool isUndo)
		{
			this.model.GenerateGalaxyChartPreview(this.PreviewRandomSeed, this.PreviewPlayerCount, isUndo);
		}


		private void generatePreviewGalaxyChartButton_Click(object sender, EventArgs e)
		{
			this.GeneratePreview(false);
		}


		private void newPreviewRandomSeedButton_Click(object sender, EventArgs e)
		{
			this.MakeNewRandomSeed();
		}


		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}


		private void InitializeComponent()
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(GalaxyChartGeneratorControl));
			this.tabControl = new TabControl();
			this.generatorParamsTabPage = new TabPage();
			this.splitContainer = new SplitContainer();
			this.propertyGrid = new PropertyGrid();
			this.previewGalaxyChartTabPage = new TabPage();
			this.toolStrip1 = new ToolStrip();
			this.generatePreviewGalaxyChartButton = new ToolStripButton();
			this.toolStripSeparator1 = new ToolStripSeparator();
			this.newPreviewRandomSeedButton = new ToolStripButton();
			this.previewGalaxyChartEditorPanel = new Panel();
			this.toolStripLabel1 = new ToolStripLabel();
			this.previewRandomSeedTextBox = new ToolStripTextBox();
			this.toolStripSeparator2 = new ToolStripSeparator();
			this.toolStripLabel2 = new ToolStripLabel();
			this.previewPlayerCountComboBox = new ToolStripComboBox();
			this.previewGalaxyChartEditorPanel = new Panel();
			this.bottomButton = new Button();
			this.tabControl.SuspendLayout();
			this.generatorParamsTabPage.SuspendLayout();
			((ISupportInitialize)this.splitContainer).BeginInit();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			this.previewGalaxyChartTabPage.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			base.SuspendLayout();
			this.tabControl.Controls.Add(this.generatorParamsTabPage);
			this.tabControl.Controls.Add(this.previewGalaxyChartTabPage);
			this.tabControl.Dock = DockStyle.Fill;
			this.tabControl.Location = new Point(0, 0);
			this.tabControl.Margin = new Padding(4, 4, 4, 4);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new Size(839, 788);
			this.tabControl.TabIndex = 2;
			this.generatorParamsTabPage.Controls.Add(this.splitContainer);
			this.generatorParamsTabPage.Location = new Point(4, 25);
			this.generatorParamsTabPage.Margin = new Padding(4, 4, 4, 4);
			this.generatorParamsTabPage.Name = "generatorParamsTabPage";
			this.generatorParamsTabPage.Padding = new Padding(4, 4, 4, 4);
			this.generatorParamsTabPage.Size = new Size(831, 759);
			this.generatorParamsTabPage.TabIndex = 0;
			this.generatorParamsTabPage.Text = "Generator Params";
			this.generatorParamsTabPage.UseVisualStyleBackColor = true;
			this.splitContainer.Dock = DockStyle.Fill;
			this.splitContainer.Location = new Point(3, 3);
			this.splitContainer.Name = "splitContainer";
			this.splitContainer.Orientation = Orientation.Horizontal;
			this.splitContainer.Panel2.Controls.Add(this.propertyGrid);
			this.splitContainer.Size = new Size(823, 751);
			this.splitContainer.SplitterDistance = 233;
			this.splitContainer.SplitterWidth = 5;
			this.splitContainer.TabIndex = 4;
			this.propertyGrid.Dock = DockStyle.Fill;
			this.propertyGrid.Location = new Point(0, 0);
			this.propertyGrid.Margin = new Padding(4, 4, 4, 4);
			this.propertyGrid.Name = "propertyGrid";
			this.propertyGrid.Size = new Size(823, 513);
			this.propertyGrid.TabIndex = 0;
			this.previewGalaxyChartTabPage.Controls.Add(this.toolStrip1);
			this.previewGalaxyChartTabPage.Controls.Add(this.previewGalaxyChartEditorPanel);
			this.previewGalaxyChartTabPage.Location = new Point(4, 25);
			this.previewGalaxyChartTabPage.Margin = new Padding(4, 4, 4, 4);
			this.previewGalaxyChartTabPage.Name = "previewGalaxyChartTabPage";
			this.previewGalaxyChartTabPage.Padding = new Padding(4, 4, 4, 4);
			this.previewGalaxyChartTabPage.Size = new Size(831, 777);
			this.previewGalaxyChartTabPage.TabIndex = 1;
			this.previewGalaxyChartTabPage.Text = "Preview Galaxy Chart";
			this.previewGalaxyChartTabPage.UseVisualStyleBackColor = true;
			this.toolStrip1.ImageScalingSize = new Size(20, 20);
			this.toolStrip1.Items.AddRange(new ToolStripItem[]
			{
				this.generatePreviewGalaxyChartButton,
				this.toolStripSeparator1,
				this.newPreviewRandomSeedButton,
				this.toolStripLabel1,
				this.previewRandomSeedTextBox,
				this.toolStripSeparator2,
				this.toolStripLabel2,
				this.previewPlayerCountComboBox
			});
			this.toolStrip1.Location = new Point(4, 4);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new Size(823, 28);
			this.toolStrip1.TabIndex = 1;
			this.toolStrip1.Text = "toolStrip1";
			this.generatePreviewGalaxyChartButton.Image = (Image)componentResourceManager.GetObject("generatePreviewGalaxyChartButton.Image");
			this.generatePreviewGalaxyChartButton.ImageTransparentColor = Color.Magenta;
			this.generatePreviewGalaxyChartButton.Name = "generatePreviewGalaxyChartButton";
			this.generatePreviewGalaxyChartButton.Size = new Size(93, 25);
			this.generatePreviewGalaxyChartButton.Text = "Generate";
			this.generatePreviewGalaxyChartButton.Click += this.generatePreviewGalaxyChartButton_Click;
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new Size(6, 25);
			this.previewRandomSeedTextBox.Font = new Font("Segoe UI", 9f);
			this.previewRandomSeedTextBox.Name = "previewRandomSeedTextBox";
			this.previewRandomSeedTextBox.Size = new Size(100, 25);
			this.previewRandomSeedTextBox.ToolTipText = "Random seeed to use for generation";
			this.newPreviewRandomSeedButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.newPreviewRandomSeedButton.Image = (Image)componentResourceManager.GetObject("newPreviewRandomSeedButton.Image");
			this.newPreviewRandomSeedButton.ImageTransparentColor = Color.Magenta;
			this.newPreviewRandomSeedButton.Name = "newPreviewRandomSeedButton";
			this.newPreviewRandomSeedButton.Size = new Size(23, 22);
			this.newPreviewRandomSeedButton.Text = "toolStripButton2";
			this.newPreviewRandomSeedButton.Click += this.newPreviewRandomSeedButton_Click;
			this.previewGalaxyChartEditorPanel.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.previewGalaxyChartEditorPanel.Location = new Point(0, 31);
			this.previewGalaxyChartEditorPanel.Name = "previewGalaxyChartEditorPanel";
			this.previewGalaxyChartEditorPanel.Size = new Size(615, 598);
			this.previewGalaxyChartEditorPanel.TabIndex = 0;
			this.toolStripLabel1.Name = "toolStripLabel1";
			this.toolStripLabel1.Size = new Size(83, 22);
			this.toolStripLabel1.Text = "Random Seed:";
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new Size(6, 25);
			this.toolStripLabel2.Name = "toolStripLabel2";
			this.toolStripLabel2.Size = new Size(58, 25);
			this.toolStripLabel2.Text = "Players:";
			this.previewPlayerCountComboBox.Name = "previewPlayerCountComboBox";
			this.previewPlayerCountComboBox.Size = new Size(99, 28);
			this.previewGalaxyChartEditorPanel.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.previewGalaxyChartEditorPanel.Location = new Point(0, 38);
			this.previewGalaxyChartEditorPanel.Margin = new Padding(4, 4, 4, 4);
			this.previewGalaxyChartEditorPanel.Name = "previewGalaxyChartEditorPanel";
			this.previewGalaxyChartEditorPanel.Size = new Size(820, 736);
			this.previewGalaxyChartEditorPanel.TabIndex = 0;
			this.bottomButton.Dock = DockStyle.Bottom;
			this.bottomButton.Size = new Size(406, 50);
			this.bottomButton.Enabled = true;
			this.bottomButton.TabIndex = 1;
			base.AutoScaleDimensions = new SizeF(8f, 16f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.tabControl);
			base.Controls.Add(this.bottomButton);
			base.Margin = new Padding(4, 4, 4, 4);
			base.Name = "GalaxyChartGeneratorControl";
			base.Size = new Size(839, 819);
			this.tabControl.ResumeLayout(false);
			this.generatorParamsTabPage.ResumeLayout(false);
			this.splitContainer.Panel2.ResumeLayout(false);
			((ISupportInitialize)this.splitContainer).EndInit();
			this.splitContainer.ResumeLayout(false);
			this.previewGalaxyChartTabPage.ResumeLayout(false);
			this.previewGalaxyChartTabPage.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			base.ResumeLayout(false);
		}


		private ScenarioModel model;


		private GalaxyChartGeneratorParamsTreeControl treeControl;


		private GalaxyChartEditorControl previewGalaxyChartEditorControl;


		private Random random;


		private IContainer components;


		private TabControl tabControl;


		private TabPage generatorParamsTabPage;


		private SplitContainer splitContainer;


		private PropertyGrid propertyGrid;


		private TabPage previewGalaxyChartTabPage;


		private Panel previewGalaxyChartEditorPanel;


		private ToolStrip toolStrip1;


		private ToolStripButton generatePreviewGalaxyChartButton;


		private ToolStripSeparator toolStripSeparator1;


		private ToolStripTextBox previewRandomSeedTextBox;


		private ToolStripButton newPreviewRandomSeedButton;


		private ToolStripLabel toolStripLabel1;


		private ToolStripSeparator toolStripSeparator2;


		private ToolStripLabel toolStripLabel2;


		private ToolStripComboBox previewPlayerCountComboBox;


		public Button bottomButton;
	}
}
