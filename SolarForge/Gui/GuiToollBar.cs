using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SolarForge.Gui
{

	public class GuiToollBar : UserControl
	{

		public GuiToollBar()
		{
			this.InitializeComponent();
			this.alignLeftButton.Enabled = false;
			this.alignLeftButton.Image = this.imageList.Images["align-left.png"];
			this.alignRightButton.Enabled = false;
			this.alignRightButton.Image = this.imageList.Images["align-right.png"];
			this.alignTopButton.Enabled = false;
			this.alignTopButton.Image = this.imageList.Images["align-top.png"];
			this.alignBottomButton.Enabled = false;
			this.alignBottomButton.Image = this.imageList.Images["align-left.png"];
			this.makeSameWidthButton.Enabled = false;
			this.makeSameWidthButton.Image = this.imageList.Images["make-same-width.png"];
			this.makeSameHeightButton.Enabled = false;
			this.makeSameHeightButton.Image = this.imageList.Images["make-same-height.png"];
			this.makeSameSizeButton.Enabled = false;
			this.makeSameSizeButton.Image = this.imageList.Images["make-same-size.png"];
			this.makeHorizontalSpacingEqualButton.Enabled = false;
			this.makeHorizontalSpacingEqualButton.Image = this.imageList.Images["make-horizontal-spacing-equal.png"];
			this.makeVerticalSpacingEqualButton.Enabled = false;
			this.makeVerticalSpacingEqualButton.Image = this.imageList.Images["make-vertical-spacing-equal.png"];
		}



		public GuiModel Model
		{
			set
			{
				this.model = value;
				this.model.SelectedComponentsChanged += this.Model_SelectedComponentsChanged;
			}
		}


		private void Model_SelectedComponentsChanged()
		{
			bool enabled = this.model.SelectedComponents.Count<GuiComponent>() >= 2;
			this.alignLeftButton.Enabled = enabled;
			this.alignRightButton.Enabled = enabled;
			this.alignTopButton.Enabled = enabled;
			this.alignBottomButton.Enabled = enabled;
			this.makeSameWidthButton.Enabled = enabled;
			this.makeSameHeightButton.Enabled = enabled;
			this.makeSameSizeButton.Enabled = enabled;
			this.makeHorizontalSpacingEqualButton.Enabled = enabled;
			this.makeVerticalSpacingEqualButton.Enabled = enabled;
		}


		private void alignLeftButton_Click(object sender, EventArgs e)
		{
			this.model.SyncSelection(GuiModel.SyncComponentsAction.AlignLeft);
		}


		private void alignTopButton_Click(object sender, EventArgs e)
		{
			this.model.SyncSelection(GuiModel.SyncComponentsAction.AlignTop);
		}


		private void alignBottomButton_Click(object sender, EventArgs e)
		{
			this.model.SyncSelection(GuiModel.SyncComponentsAction.AlignBottom);
		}


		private void alignRightButton_Click(object sender, EventArgs e)
		{
			this.model.SyncSelection(GuiModel.SyncComponentsAction.AlignRight);
		}


		private void makeSameWidthButton_Click(object sender, EventArgs e)
		{
			this.model.SyncSelection(GuiModel.SyncComponentsAction.MakeSameWidth);
		}


		private void makeSameHeightButton_Click(object sender, EventArgs e)
		{
			this.model.SyncSelection(GuiModel.SyncComponentsAction.MakeSameHeight);
		}


		private void makeSameSizeButton_Click(object sender, EventArgs e)
		{
			this.model.SyncSelection(GuiModel.SyncComponentsAction.MakeSameSize);
		}


		private void makeHorizontalSpacingEqualButton_Click(object sender, EventArgs e)
		{
			this.model.SyncSelection(GuiModel.SyncComponentsAction.MakeHorizontalSpacingEqual);
		}


		private void makeVerticalSpacingEqualButton_Click(object sender, EventArgs e)
		{
			this.model.SyncSelection(GuiModel.SyncComponentsAction.MakeVerticalSpacingEqual);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(GuiToollBar));
			this.toolStrip = new ToolStrip();
			this.alignBottomButton = new ToolStripButton();
			this.imageList = new ImageList(this.components);
			this.alignTopButton = new ToolStripButton();
			this.alignRightButton = new ToolStripButton();
			this.alignLeftButton = new ToolStripButton();
			this.toolStripSeparator1 = new ToolStripSeparator();
			this.makeSameWidthButton = new ToolStripButton();
			this.makeSameSizeButton = new ToolStripButton();
			this.makeSameHeightButton = new ToolStripButton();
			this.toolStripSeparator2 = new ToolStripSeparator();
			this.makeVerticalSpacingEqualButton = new ToolStripButton();
			this.makeHorizontalSpacingEqualButton = new ToolStripButton();
			this.toolStrip.SuspendLayout();
			base.SuspendLayout();
			this.toolStrip.Dock = DockStyle.Fill;
			this.toolStrip.ImageScalingSize = new Size(24, 24);
			this.toolStrip.Items.AddRange(new ToolStripItem[]
			{
				this.alignLeftButton,
				this.alignRightButton,
				this.alignTopButton,
				this.alignBottomButton,
				this.toolStripSeparator2,
				this.makeSameWidthButton,
				this.makeSameHeightButton,
				this.makeSameSizeButton,
				this.toolStripSeparator1,
				this.makeHorizontalSpacingEqualButton,
				this.makeVerticalSpacingEqualButton
			});
			this.toolStrip.Location = new Point(0, 0);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Size = new Size(745, 62);
			this.toolStrip.TabIndex = 0;
			this.toolStrip.Text = "toolStrip1";
			this.alignBottomButton.AutoSize = false;
			this.alignBottomButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.alignBottomButton.ImageTransparentColor = Color.Magenta;
			this.alignBottomButton.Name = "alignBottomButton";
			this.alignBottomButton.Size = new Size(34, 34);
			this.alignBottomButton.ToolTipText = "Align Bottom";
			this.alignBottomButton.Click += this.alignBottomButton_Click;
			this.imageList.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("imageList.ImageStream");
			this.imageList.TransparentColor = Color.Transparent;
			this.imageList.Images.SetKeyName(0, "align-left.png");
			this.imageList.Images.SetKeyName(1, "align-right.png");
			this.imageList.Images.SetKeyName(2, "align-bottom.png");
			this.imageList.Images.SetKeyName(3, "align-top.png");
			this.imageList.Images.SetKeyName(4, "make-same-height.png");
			this.imageList.Images.SetKeyName(5, "make-same-size.png");
			this.imageList.Images.SetKeyName(6, "make-same-width.png");
			this.imageList.Images.SetKeyName(7, "make-horizontal-spacing-equal.png");
			this.imageList.Images.SetKeyName(8, "make-vertical-spacing-equal.png");
			this.alignTopButton.AutoSize = false;
			this.alignTopButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.alignTopButton.ImageTransparentColor = Color.Magenta;
			this.alignTopButton.Name = "alignTopButton";
			this.alignTopButton.Size = new Size(34, 34);
			this.alignTopButton.ToolTipText = "Align Top";
			this.alignTopButton.Click += this.alignTopButton_Click;
			this.alignRightButton.AutoSize = false;
			this.alignRightButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.alignRightButton.ImageTransparentColor = Color.Magenta;
			this.alignRightButton.Name = "alignRightButton";
			this.alignRightButton.Size = new Size(34, 34);
			this.alignRightButton.ToolTipText = "Align Right";
			this.alignRightButton.Click += this.alignRightButton_Click;
			this.alignLeftButton.AutoSize = false;
			this.alignLeftButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.alignLeftButton.ImageTransparentColor = Color.Magenta;
			this.alignLeftButton.Name = "alignLeftButton";
			this.alignLeftButton.Size = new Size(34, 34);
			this.alignLeftButton.ToolTipText = "Align Left";
			this.alignLeftButton.Click += this.alignLeftButton_Click;
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new Size(6, 62);
			this.makeSameWidthButton.AutoSize = false;
			this.makeSameWidthButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.makeSameWidthButton.ImageTransparentColor = Color.Magenta;
			this.makeSameWidthButton.Name = "makeSameWidthButton";
			this.makeSameWidthButton.Size = new Size(34, 34);
			this.makeSameWidthButton.Click += this.makeSameWidthButton_Click;
			this.makeSameSizeButton.AutoSize = false;
			this.makeSameSizeButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.makeSameSizeButton.ImageTransparentColor = Color.Magenta;
			this.makeSameSizeButton.Name = "makeSameSizeButton";
			this.makeSameSizeButton.Size = new Size(34, 34);
			this.makeSameSizeButton.Click += this.makeSameSizeButton_Click;
			this.makeSameHeightButton.AutoSize = false;
			this.makeSameHeightButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.makeSameHeightButton.ImageTransparentColor = Color.Magenta;
			this.makeSameHeightButton.Name = "makeSameHeightButton";
			this.makeSameHeightButton.Size = new Size(34, 34);
			this.makeSameHeightButton.Click += this.makeSameHeightButton_Click;
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new Size(6, 62);
			this.makeVerticalSpacingEqualButton.AutoSize = false;
			this.makeVerticalSpacingEqualButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.makeVerticalSpacingEqualButton.ImageTransparentColor = Color.Magenta;
			this.makeVerticalSpacingEqualButton.Name = "makeVerticalSpacingEqualButton";
			this.makeVerticalSpacingEqualButton.Size = new Size(34, 34);
			this.makeVerticalSpacingEqualButton.ToolTipText = "Make Vertical Spacing Equal";
			this.makeVerticalSpacingEqualButton.Click += this.makeVerticalSpacingEqualButton_Click;
			this.makeHorizontalSpacingEqualButton.AutoSize = false;
			this.makeHorizontalSpacingEqualButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.makeHorizontalSpacingEqualButton.ImageTransparentColor = Color.Magenta;
			this.makeHorizontalSpacingEqualButton.Name = "makeHorizontalSpacingEqualButton";
			this.makeHorizontalSpacingEqualButton.Size = new Size(34, 34);
			this.makeHorizontalSpacingEqualButton.ToolTipText = "Make Horizontal Spacing Equal";
			this.makeHorizontalSpacingEqualButton.Click += this.makeHorizontalSpacingEqualButton_Click;
			base.AutoScaleDimensions = new SizeF(9f, 20f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.toolStrip);
			base.Name = "GuiToollBar";
			base.Size = new Size(745, 62);
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}


		private GuiModel model;


		private IContainer components;


		private ToolStrip toolStrip;


		private ToolStripButton alignBottomButton;


		private ImageList imageList;


		private ToolStripButton alignLeftButton;


		private ToolStripButton alignRightButton;


		private ToolStripButton alignTopButton;


		private ToolStripSeparator toolStripSeparator1;


		private ToolStripButton makeSameWidthButton;


		private ToolStripButton makeSameHeightButton;


		private ToolStripButton makeSameSizeButton;


		private ToolStripSeparator toolStripSeparator2;


		private ToolStripButton makeHorizontalSpacingEqualButton;


		private ToolStripButton makeVerticalSpacingEqualButton;


		private class ImageKeys
		{

			public const string AlignLeft = "align-left.png";


			public const string AlignRight = "align-right.png";


			public const string AlignTop = "align-top.png";


			public const string AlignBottom = "align-bottom.png";


			public const string MakeSameWidth = "make-same-width.png";


			public const string MakeSameHeight = "make-same-height.png";


			public const string MakeSameSize = "make-same-size.png";


			public const string MakeHorizontalSpacingEqual = "make-horizontal-spacing-equal.png";


			public const string MakeVerticalSpacingEqual = "make-vertical-spacing-equal.png";
		}
	}
}
