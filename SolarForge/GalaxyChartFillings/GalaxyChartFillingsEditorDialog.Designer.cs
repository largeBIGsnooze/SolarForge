namespace SolarForge.GalaxyChartFillings
{

	public partial class GalaxyChartFillingsEditorDialog : global::System.Windows.Forms.Form
	{

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
			this.closeButton = new global::System.Windows.Forms.Button();
			this.fillingsSourceComboBox = new global::System.Windows.Forms.ComboBox();
			this.splitContainer = new global::System.Windows.Forms.SplitContainer();
			this.fillingPropertyGrid = new global::System.Windows.Forms.PropertyGrid();
			((global::System.ComponentModel.ISupportInitialize)this.splitContainer).BeginInit();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			base.SuspendLayout();
			this.closeButton.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right);
			this.closeButton.Location = new global::System.Drawing.Point(661, 406);
			this.closeButton.Name = "closeButton";
			this.closeButton.Size = new global::System.Drawing.Size(127, 36);
			this.closeButton.TabIndex = 0;
			this.closeButton.Text = "Close";
			this.closeButton.UseVisualStyleBackColor = true;
			this.closeButton.Click += new global::System.EventHandler(this.closeButton_Click);
			this.fillingsSourceComboBox.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.fillingsSourceComboBox.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.fillingsSourceComboBox.FormattingEnabled = true;
			this.fillingsSourceComboBox.Location = new global::System.Drawing.Point(12, 12);
			this.fillingsSourceComboBox.Name = "fillingsSourceComboBox";
			this.fillingsSourceComboBox.Size = new global::System.Drawing.Size(776, 21);
			this.fillingsSourceComboBox.TabIndex = 1;
			this.splitContainer.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.splitContainer.Location = new global::System.Drawing.Point(12, 39);
			this.splitContainer.Name = "splitContainer";
			this.splitContainer.Panel2.Controls.Add(this.fillingPropertyGrid);
			this.splitContainer.Size = new global::System.Drawing.Size(776, 361);
			this.splitContainer.SplitterDistance = 400;
			this.splitContainer.TabIndex = 4;
			this.fillingPropertyGrid.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.fillingPropertyGrid.Location = new global::System.Drawing.Point(0, 0);
			this.fillingPropertyGrid.Name = "fillingPropertyGrid";
			this.fillingPropertyGrid.PropertySort = global::System.Windows.Forms.PropertySort.Alphabetical;
			this.fillingPropertyGrid.Size = new global::System.Drawing.Size(372, 361);
			this.fillingPropertyGrid.TabIndex = 4;
			this.fillingPropertyGrid.ToolbarVisible = false;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(800, 450);
			base.Controls.Add(this.splitContainer);
			base.Controls.Add(this.fillingsSourceComboBox);
			base.Controls.Add(this.closeButton);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			base.Name = "GalaxyChartFillingsEditorDialog";
			this.Text = "Galaxy Chart Fillings";
			this.splitContainer.Panel2.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.splitContainer).EndInit();
			this.splitContainer.ResumeLayout(false);
			base.ResumeLayout(false);
		}


		private global::System.ComponentModel.IContainer components;


		private global::System.Windows.Forms.Button closeButton;


		private global::System.Windows.Forms.ComboBox fillingsSourceComboBox;


		private global::System.Windows.Forms.SplitContainer splitContainer;


		private global::System.Windows.Forms.PropertyGrid fillingPropertyGrid;
	}
}
