using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using SolarForge.Scenes;

namespace SolarForge
{

	public class SceneToolBar : UserControl
	{

		public SceneToolBar()
		{
			this.InitializeComponent();
			this.refreshButton.Image = this.imageList.Images["refresh-regular-24.png"];
			this.singleStepButton.Image = this.imageList.Images["walk-regular-24.png"];
			this.decreaseTimeScaleButton.Image = this.imageList.Images["rewind-circle-regular-24.png"];
			this.increaseTimeScaleButton.Image = this.imageList.Images["fast-forward-circle-regular-24.png"];
			this.timeScaleComboBox.Items.Add("0.1");
			this.timeScaleComboBox.Items.Add("0.5");
			this.timeScaleComboBox.Items.Add("1");
			this.timeScaleComboBox.Items.Add("1.5");
			this.timeScaleComboBox.Items.Add("2.0");
		}



		public SceneModel Model
		{
			set
			{
				this.model = value;
				this.SyncButtonsToModel();
				this.model.IsPausedChanged += delegate(bool isPaused)
				{
					this.SyncButtonsToModel();
				};
				this.model.IsLoopedChanged += delegate(bool isLooped)
				{
					this.SyncButtonsToModel();
				};
				this.SyncTimeScaleTextToModel();
				this.model.TimeScaleChanged += delegate(double timeScale)
				{
					if (!this.suppressTimeScaleChangedHandler)
					{
						this.SyncTimeScaleTextToModel();
					}
				};
			}
		}


		private void SyncButtonsToModel()
		{
			this.playPauseButton.Image = this.imageList.Images[this.model.IsPaused ? "play-circle-regular-24.png" : "pause-circle-regular-24.png"];
			this.isLoopedButton.Image = this.imageList.Images[this.model.IsLooped ? "infinite-regular-24.png" : "infinite-faded-24.png"];
			this.singleStepButton.Enabled = this.model.IsPaused;
		}


		private void SyncTimeScaleTextToModel()
		{
			this.timeScaleComboBox.Text = this.model.TimeScale.ToString();
		}


		private void refreshButton_Click(object sender, EventArgs e)
		{
			this.model.RefreshScene();
		}


		private void playPauseButton_Click(object sender, EventArgs e)
		{
			this.model.IsPaused = !this.model.IsPaused;
		}


		private double ConstrainTimeScale(double timeScale)
		{
			timeScale = Math.Max(timeScale, 0.01);
			timeScale = Math.Min(timeScale, 100.0);
			return timeScale;
		}


		private double RoundToFirstDecimal(double v)
		{
			return Math.Round(this.model.TimeScale * 10.0) / 10.0;
		}


		private void decreaseTimeScaleButton_Click(object sender, EventArgs e)
		{
			this.model.TimeScale = this.ConstrainTimeScale(this.RoundToFirstDecimal(this.model.TimeScale) - 0.1);
		}


		private void increaseTimeScaleButton_Click(object sender, EventArgs e)
		{
			this.model.TimeScale = this.ConstrainTimeScale(this.RoundToFirstDecimal(this.model.TimeScale) + 0.1);
		}


		private void timeScaleComboBox_TextChanged(object sender, EventArgs e)
		{
			double num;
			if (double.TryParse(this.timeScaleComboBox.Text, out num))
			{
				double num2 = this.ConstrainTimeScale(num);
				if (num2 != num)
				{
					this.model.TimeScale = num2;
					return;
				}
				this.suppressTimeScaleChangedHandler = true;
				this.model.TimeScale = num2;
				this.suppressTimeScaleChangedHandler = false;
			}
		}


		private void isLoopedButton_Click(object sender, EventArgs e)
		{
			this.model.IsLooped = !this.model.IsLooped;
		}


		private void singleStepButton_Click(object sender, EventArgs e)
		{
			this.model.SingleStep((Control.ModifierKeys == Keys.Shift) ? 10 : 1);
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
			this.components = new Container();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(SceneToolBar));
			this.toolStrip = new ToolStrip();
			this.isLoopedButton = new ToolStripButton();
			this.refreshButton = new ToolStripButton();
			this.playPauseButton = new ToolStripButton();
			this.singleStepButton = new ToolStripButton();
			this.toolStripSeparator1 = new ToolStripSeparator();
			this.decreaseTimeScaleButton = new ToolStripButton();
			this.timeScaleComboBox = new ToolStripComboBox();
			this.increaseTimeScaleButton = new ToolStripButton();
			this.imageList = new ImageList(this.components);
			this.toolStrip.SuspendLayout();
			base.SuspendLayout();
			this.toolStrip.Dock = DockStyle.Fill;
			this.toolStrip.ImageScalingSize = new Size(24, 24);
			this.toolStrip.Items.AddRange(new ToolStripItem[]
			{
				this.isLoopedButton,
				this.refreshButton,
				this.playPauseButton,
				this.singleStepButton,
				this.toolStripSeparator1,
				this.decreaseTimeScaleButton,
				this.timeScaleComboBox,
				this.increaseTimeScaleButton
			});
			this.toolStrip.Location = new Point(0, 0);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Padding = new Padding(0, 0, 3, 0);
			this.toolStrip.Size = new Size(954, 62);
			this.toolStrip.TabIndex = 0;
			this.toolStrip.Text = "toolStrip1";
			this.isLoopedButton.AutoSize = false;
			this.isLoopedButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.isLoopedButton.ImageScaling = ToolStripItemImageScaling.None;
			this.isLoopedButton.ImageTransparentColor = Color.Magenta;
			this.isLoopedButton.Name = "isLoopedButton";
			this.isLoopedButton.Size = new Size(37, 37);
			this.isLoopedButton.ToolTipText = "If enabled the effect will auto-restart when finished.";
			this.isLoopedButton.Click += this.isLoopedButton_Click;
			this.refreshButton.AutoSize = false;
			this.refreshButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.refreshButton.ImageScaling = ToolStripItemImageScaling.None;
			this.refreshButton.ImageTransparentColor = Color.Magenta;
			this.refreshButton.Name = "refreshButton";
			this.refreshButton.Size = new Size(40, 40);
			this.refreshButton.Text = "toolStripButton1";
			this.refreshButton.ToolTipText = "Reset effect (F5)";
			this.refreshButton.Click += this.refreshButton_Click;
			this.playPauseButton.AutoSize = false;
			this.playPauseButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.playPauseButton.ImageScaling = ToolStripItemImageScaling.None;
			this.playPauseButton.ImageTransparentColor = Color.Magenta;
			this.playPauseButton.Name = "playPauseButton";
			this.playPauseButton.Size = new Size(37, 37);
			this.playPauseButton.ToolTipText = "Play / Pause Effect (F6)";
			this.playPauseButton.Click += this.playPauseButton_Click;
			this.singleStepButton.AutoSize = false;
			this.singleStepButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.singleStepButton.ImageScaling = ToolStripItemImageScaling.None;
			this.singleStepButton.ImageTransparentColor = Color.Magenta;
			this.singleStepButton.Name = "singleStepButton";
			this.singleStepButton.Size = new Size(37, 37);
			this.singleStepButton.ToolTipText = "Advance 1 frame when paused (F7)";
			this.singleStepButton.Click += this.singleStepButton_Click;
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new Size(6, 62);
			this.decreaseTimeScaleButton.AutoSize = false;
			this.decreaseTimeScaleButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.decreaseTimeScaleButton.ImageScaling = ToolStripItemImageScaling.None;
			this.decreaseTimeScaleButton.ImageTransparentColor = Color.Magenta;
			this.decreaseTimeScaleButton.Name = "decreaseTimeScaleButton";
			this.decreaseTimeScaleButton.Size = new Size(37, 37);
			this.decreaseTimeScaleButton.ToolTipText = "Decrease Playback Speed";
			this.decreaseTimeScaleButton.Click += this.decreaseTimeScaleButton_Click;
			this.timeScaleComboBox.Name = "timeScaleComboBox";
			this.timeScaleComboBox.Size = new Size(110, 62);
			this.timeScaleComboBox.ToolTipText = "Set the Playback Speed Scalar.";
			this.timeScaleComboBox.TextChanged += this.timeScaleComboBox_TextChanged;
			this.increaseTimeScaleButton.AutoSize = false;
			this.increaseTimeScaleButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.increaseTimeScaleButton.ImageScaling = ToolStripItemImageScaling.None;
			this.increaseTimeScaleButton.ImageTransparentColor = Color.Magenta;
			this.increaseTimeScaleButton.Name = "increaseTimeScaleButton";
			this.increaseTimeScaleButton.Size = new Size(37, 37);
			this.increaseTimeScaleButton.ToolTipText = "Increase Playback Speed";
			this.increaseTimeScaleButton.Click += this.increaseTimeScaleButton_Click;
			this.imageList.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("imageList.ImageStream");
			this.imageList.TransparentColor = Color.Transparent;
			this.imageList.Images.SetKeyName(0, "fast-forward-circle-regular-24.png");
			this.imageList.Images.SetKeyName(1, "pause-circle-regular-24.png");
			this.imageList.Images.SetKeyName(2, "play-circle-regular-24.png");
			this.imageList.Images.SetKeyName(3, "refresh-regular-24.png");
			this.imageList.Images.SetKeyName(4, "rewind-circle-regular-24.png");
			this.imageList.Images.SetKeyName(5, "stop-circle-regular-24.png");
			this.imageList.Images.SetKeyName(6, "infinite-faded-24.png");
			this.imageList.Images.SetKeyName(7, "infinite-regular-24.png");
			this.imageList.Images.SetKeyName(8, "walk-regular-24.png");
			base.AutoScaleDimensions = new SizeF(9f, 20f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.toolStrip);
			base.Margin = new Padding(4, 5, 4, 5);
			base.Name = "SceneToolBar";
			base.Size = new Size(954, 62);
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}


		private SceneModel model;


		private bool suppressTimeScaleChangedHandler;


		private IContainer components;


		private ToolStrip toolStrip;


		private ToolStripButton playPauseButton;


		private ToolStripButton refreshButton;


		private ImageList imageList;


		private ToolStripSeparator toolStripSeparator1;


		private ToolStripComboBox timeScaleComboBox;


		private ToolStripButton decreaseTimeScaleButton;


		private ToolStripButton increaseTimeScaleButton;


		private ToolStripButton isLoopedButton;


		private ToolStripButton singleStepButton;


		private class ImageKeys
		{

			public const string Play = "play-circle-regular-24.png";


			public const string Pause = "pause-circle-regular-24.png";


			public const string Stop = "stop-circle-regular-24.png";


			public const string Refresh = "refresh-regular-24.png";


			public const string IncreaseTimeScale = "fast-forward-circle-regular-24.png";


			public const string DecreaseTimeScale = "rewind-circle-regular-24.png";


			public const string LoopedOn = "infinite-regular-24.png";


			public const string LoopedOff = "infinite-faded-24.png";


			public const string SingleStep = "walk-regular-24.png";
		}
	}
}
