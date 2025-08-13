using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Solar.Scenarios;

namespace SolarForge.GalaxyChartFillings
{

	public class RandomFixtureDataControl : UserControl
	{

		public RandomFixtureDataControl()
		{
			this.InitializeComponent();
			this.dataGridView.AutoGenerateColumns = false;
			this.dataGridView.SelectionChanged += this.DataGridView_SelectionChanged;
			this.dataGridItems = new BindingList<RandomFixtureFillingItem>();
			this.dataGridItems.AllowNew = false;
			this.dataGridItems.AllowRemove = false;
			this.dataGridView.DataSource = this.dataGridItems;
			this.dataGridView.DataError += this.DataGridView_DataError;
		}


		private void DataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
			if (!this.suppressDataErrors)
			{
				MessageBox.Show("Data Error : " + e.Exception.Message, "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				this.suppressDataErrors = true;
			}
			e.Cancel = true;
		}



		private int? SelectedItemIndex
		{
			get
			{
				if (this.dataGridView.SelectedRows.Count > 0 && this.dataGridView.SelectedRows[0].Index >= 0)
				{
					return new int?(this.dataGridView.SelectedRows[0].Index);
				}
				return null;
			}
		}


		private void DataGridView_SelectionChanged(object sender, EventArgs e)
		{
			this.removeItemButton.Enabled = (this.SelectedItemIndex != null);
		}



        public GalaxyChartFillingsEditorModel Model
        {
            set
            {
                this.model = value;
                this.model.SelectedFillingsSourceChanged += this.Model_SelectedFillingsSourceChanged;
                this.model.SelectedFillingsComponentChanged += this.Model_SelectedFillingsComponentChanged;
                this.model.FixtureFillingsChanged += this.Model_FixtureFillingsChanged;
                this.RefreshFixtureColumnItems();
            } 
        }


        private void Model_FixtureFillingsChanged()
        {
            this.RefreshFixtureColumnItems();
        }


        private void RefreshFixtureColumnItems()
        {
            this.fixtureColumn.Items.Clear();
            foreach (GalaxyChartFillingsSource galaxyChartFillingsSource in this.model.FillingsSources)
            {
                foreach (FixtureFillingName item in galaxyChartFillingsSource.Fillings.FixtureFillingNames)
                {
                    this.fixtureColumn.Items.Add(item);
                }
            }
        }


        private void Model_SelectedFillingsSourceChanged()
		{
			DataGridView dataGridView = this.dataGridView;
			GalaxyChartFillingsSource selectedFillingsSource = this.model.SelectedFillingsSource;
			dataGridView.ReadOnly = ((selectedFillingsSource != null) ? new bool?(selectedFillingsSource.ReadOnly) : null).GetValueOrDefault();
		}


		private void Model_SelectedFillingsComponentChanged()
		{
			this.suppressDataErrors = false;
			base.Visible = (this.model.SelectedRandomFixtureFilling != null);
			this.UpdateDataSource();
		}


		private void UpdateDataSource()
		{
			this.dataGridItems.Clear();
			RandomFixtureFilling selectedRandomFixtureFilling = this.model.SelectedRandomFixtureFilling;
			IEnumerable<RandomFixtureFillingItem> enumerable = (selectedRandomFixtureFilling != null) ? selectedRandomFixtureFilling.Items : null;
			if (enumerable != null)
			{
				foreach (RandomFixtureFillingItem item in enumerable)
				{
					this.dataGridItems.Add(item);
				}
			}
		}


		private void addItemButton_Click(object sender, EventArgs e)
		{
			this.model.SelectedRandomFixtureFilling.AddItem(this.model.MakeNewRandomFixtureFillingItem());
			this.UpdateDataSource();
		}


		private void removeItemButton_Click(object sender, EventArgs e)
		{
			if (this.SelectedItemIndex != null)
			{
				this.model.SelectedRandomFixtureFilling.RemoveItemAt(this.SelectedItemIndex.Value);
				this.UpdateDataSource();
			}
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(RandomFixtureDataControl));
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			this.dataGridView = new DataGridView();
			this.toolStrip1 = new ToolStrip();
			this.addItemButton = new ToolStripButton();
			this.removeItemButton = new ToolStripButton();
			this.fixtureColumn = new DataGridViewComboBoxColumn();
			this.probabilityColumn = new DataGridViewTextBoxColumn();
			((ISupportInitialize)this.dataGridView).BeginInit();
			this.toolStrip1.SuspendLayout();
			base.SuspendLayout();
			this.dataGridView.AllowUserToResizeRows = false;
			this.dataGridView.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView.Columns.AddRange(new DataGridViewColumn[]
			{
				this.fixtureColumn,
				this.probabilityColumn
			});
			this.dataGridView.Location = new Point(0, 28);
			this.dataGridView.MultiSelect = false;
			this.dataGridView.Name = "dataGridView";
			this.dataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dataGridView.Size = new Size(478, 543);
			this.dataGridView.TabIndex = 0;
			this.toolStrip1.Items.AddRange(new ToolStripItem[]
			{
				this.addItemButton,
				this.removeItemButton
			});
			this.toolStrip1.Location = new Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new Size(481, 25);
			this.toolStrip1.TabIndex = 1;
			this.toolStrip1.Text = "toolStrip1";
			this.addItemButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
			this.addItemButton.Image = (Image)componentResourceManager.GetObject("addItemButton.Image");
			this.addItemButton.ImageTransparentColor = Color.Magenta;
			this.addItemButton.Name = "addItemButton";
			this.addItemButton.Size = new Size(33, 22);
			this.addItemButton.Text = "Add";
			this.addItemButton.Click += this.addItemButton_Click;
			this.removeItemButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
			this.removeItemButton.Image = (Image)componentResourceManager.GetObject("removeItemButton.Image");
			this.removeItemButton.ImageTransparentColor = Color.Magenta;
			this.removeItemButton.Name = "removeItemButton";
			this.removeItemButton.Size = new Size(54, 22);
			this.removeItemButton.Text = "Remove";
			this.removeItemButton.Click += this.removeItemButton_Click;
			this.fixtureColumn.DataPropertyName = "Name";
			dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
			this.fixtureColumn.DefaultCellStyle = dataGridViewCellStyle;
			this.fixtureColumn.HeaderText = "Fixture";
			this.fixtureColumn.Name = "fixtureColumn";
			this.fixtureColumn.Resizable = DataGridViewTriState.True;
			this.fixtureColumn.Width = 200;
			this.probabilityColumn.DataPropertyName = "Probability";
			this.probabilityColumn.HeaderText = "Probability";
			this.probabilityColumn.Name = "probabilityColumn";
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.toolStrip1);
			base.Controls.Add(this.dataGridView);
			base.Name = "RandomFixtureDataControl";
			base.Size = new Size(481, 574);
			((ISupportInitialize)this.dataGridView).EndInit();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}


		private GalaxyChartFillingsEditorModel model;


		private BindingList<RandomFixtureFillingItem> dataGridItems;


		private bool suppressDataErrors;


		private IContainer components;


		private DataGridView dataGridView;


		private ToolStrip toolStrip1;


		private ToolStripButton addItemButton;


		private ToolStripButton removeItemButton;


		private DataGridViewComboBoxColumn fixtureColumn;


		private DataGridViewTextBoxColumn probabilityColumn;
	}
}
