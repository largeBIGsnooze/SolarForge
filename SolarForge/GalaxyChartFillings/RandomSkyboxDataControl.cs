using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Solar.Rendering;
using Solar.Scenarios;

namespace SolarForge.GalaxyChartFillings
{

	public class RandomSkyboxDataControl : UserControl
	{

		public RandomSkyboxDataControl()
		{
			this.InitializeComponent();
			this.dataGridView.AutoGenerateColumns = false;
			this.dataGridView.CellClick += this.DataGridView_CellClick;
			this.dataGridView.SelectionChanged += this.DataGridView_SelectionChanged;
			this.dataGridItems = new BindingList<RandomSkyboxFillingItem>();
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


		private void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			DataGridView dataGridView = (DataGridView)sender;
			if (e.ColumnIndex >= 0 && e.RowIndex >= 0 && dataGridView.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
			{
				RandomSkyboxFillingItem randomSkyboxFillingItem = ((IEnumerable<RandomSkyboxFillingItem>)this.dataGridView.DataSource).ElementAt(e.RowIndex);
				using (OpenFileDialog openFileDialog = new OpenFileDialog())
				{
					openFileDialog.AddExtension = false;
					openFileDialog.FileName = "";
					openFileDialog.Filter = SkyboxNameEditor.Filter;
					openFileDialog.Multiselect = false;
					openFileDialog.RestoreDirectory = true;
					openFileDialog.Title = SkyboxNameEditor.Title;
					openFileDialog.ValidateNames = true;
					if (openFileDialog.ShowDialog() == DialogResult.OK)
					{
						randomSkyboxFillingItem.SkyboxName = new SkyboxName(openFileDialog.FileName);
					}
				}
			}
		}



		public GalaxyChartFillingsEditorModel Model
		{
			set
			{
				this.model = value;
				this.model.SelectedFillingsSourceChanged += this.Model_SelectedFillingsSourceChanged;
				this.model.SelectedFillingsComponentChanged += this.Model_SelectedFillingsComponentChanged;
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
			base.Visible = (this.model.SelectedRandomSkyboxFilling != null);
			this.UpdateDataSource();
		}


		private void UpdateDataSource()
		{
			this.dataGridItems.Clear();
			RandomSkyboxFilling selectedRandomSkyboxFilling = this.model.SelectedRandomSkyboxFilling;
			IEnumerable<RandomSkyboxFillingItem> enumerable = (selectedRandomSkyboxFilling != null) ? selectedRandomSkyboxFilling.Items : null;
			if (enumerable != null)
			{
				foreach (RandomSkyboxFillingItem item in enumerable)
				{
					this.dataGridItems.Add(item);
				}
			}
		}


		private void addItemButton_Click(object sender, EventArgs e)
		{
			this.model.SelectedRandomSkyboxFilling.AddItem();
			this.UpdateDataSource();
		}


		private void removeItemButton_Click(object sender, EventArgs e)
		{
			if (this.SelectedItemIndex != null)
			{
				this.model.SelectedRandomSkyboxFilling.RemoveItemAt(this.SelectedItemIndex.Value);
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
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(RandomSkyboxDataControl));
			this.dataGridView = new DataGridView();
			this.skyboxNameColumn = new DataGridViewButtonColumn();
			this.probabilityColumn = new DataGridViewTextBoxColumn();
			this.toolStrip1 = new ToolStrip();
			this.addItemButton = new ToolStripButton();
			this.removeItemButton = new ToolStripButton();
			((ISupportInitialize)this.dataGridView).BeginInit();
			this.toolStrip1.SuspendLayout();
			base.SuspendLayout();
			this.dataGridView.AllowUserToResizeRows = false;
			this.dataGridView.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView.Columns.AddRange(new DataGridViewColumn[]
			{
				this.skyboxNameColumn,
				this.probabilityColumn
			});
			this.dataGridView.Location = new Point(0, 28);
			this.dataGridView.MultiSelect = false;
			this.dataGridView.Name = "dataGridView";
			this.dataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dataGridView.Size = new Size(478, 543);
			this.dataGridView.TabIndex = 0;
			this.skyboxNameColumn.DataPropertyName = "SkyboxName";
			dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
			this.skyboxNameColumn.DefaultCellStyle = dataGridViewCellStyle;
			this.skyboxNameColumn.HeaderText = "Skybox";
			this.skyboxNameColumn.Name = "skyboxNameColumn";
			this.skyboxNameColumn.Width = 200;
			this.probabilityColumn.DataPropertyName = "Probability";
			this.probabilityColumn.HeaderText = "Probability";
			this.probabilityColumn.Name = "probabilityColumn";
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
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.toolStrip1);
			base.Controls.Add(this.dataGridView);
			base.Name = "RandomSkyboxDataControl";
			base.Size = new Size(481, 574);
			((ISupportInitialize)this.dataGridView).EndInit();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}


		private GalaxyChartFillingsEditorModel model;


		private BindingList<RandomSkyboxFillingItem> dataGridItems;


		private bool suppressDataErrors;


		private IContainer components;


		private DataGridView dataGridView;


		private DataGridViewButtonColumn skyboxNameColumn;


		private DataGridViewTextBoxColumn probabilityColumn;


		private ToolStrip toolStrip1;


		private ToolStripButton addItemButton;


		private ToolStripButton removeItemButton;
	}
}
