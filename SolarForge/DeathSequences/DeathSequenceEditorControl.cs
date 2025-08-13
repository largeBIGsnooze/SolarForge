using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Solar.DeathSequences;
using Solar.Rendering;

namespace SolarForge.DeathSequences
{

	public class DeathSequenceEditorControl : UserControl
	{



		public DeathSequenceModel Model
		{
			get
			{
				return this.model;
			}
			set
			{
				this.model = value;
				this.model.DeathSequenceChanged += this.Model_DeathSequenceChanged;
				this.model.MeshChanged += this.Model_MeshChanged;
			}
		}


		private void Model_MeshChanged(Mesh mesh)
		{
			this.meshTextBox.Text = ((mesh != null) ? Path.GetFileNameWithoutExtension(mesh.LoadedPath) : "");
		}


		private void Model_DeathSequenceChanged(DeathSequence deathSequence)
		{
			this.deathSequencePropertyGrid.SelectedObject = deathSequence;
			this.deathSequenceEventListBox.Items.Clear();
			foreach (DeathSequenceEvent item in deathSequence.Events)
			{
				this.deathSequenceEventListBox.Items.Add(item);
			}
		}


		public DeathSequenceEditorControl()
		{
			this.InitializeComponent();
			this.deathSequencePropertyGrid.ToolbarVisible = false;
			this.deathSequencePropertyGrid.HelpVisible = false;
			this.deathSequencePropertyGrid.PropertySort = PropertySort.Alphabetical;
		}


		private void loadMeshButton_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.AddExtension = true;
				openFileDialog.FileName = "";
				openFileDialog.Multiselect = false;
				openFileDialog.RestoreDirectory = true;
				openFileDialog.Title = "Load Mesh";
				openFileDialog.ValidateNames = true;
				string text = FileTypeHelpers.MakeFileTypeExtensionsListString(FileTypeHelpers.GetFileTypeExtensions(FileType.Mesh));
				openFileDialog.Filter = "Mesh files (" + text + ")|" + text;
				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					this.model.LoadMesh(openFileDialog.FileName, true);
				}
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
            this.deathSequencePropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.deathSequenceEventListBox = new System.Windows.Forms.ListBox();
            this.addDeathSequenceEventButton = new System.Windows.Forms.Button();
            this.deathSequenceEventPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.removeDeathSequenceEventButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.meshTextBox = new System.Windows.Forms.TextBox();
            this.loadMeshButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // deathSequencePropertyGrid
            // 
            this.deathSequencePropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.deathSequencePropertyGrid.Location = new System.Drawing.Point(2, 2);
            this.deathSequencePropertyGrid.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.deathSequencePropertyGrid.Name = "deathSequencePropertyGrid";
            this.deathSequencePropertyGrid.Size = new System.Drawing.Size(303, 124);
            this.deathSequencePropertyGrid.TabIndex = 0;
            // 
            // deathSequenceEventListBox
            // 
            this.deathSequenceEventListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.deathSequenceEventListBox.FormattingEnabled = true;
            this.deathSequenceEventListBox.Location = new System.Drawing.Point(2, 169);
            this.deathSequenceEventListBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.deathSequenceEventListBox.Name = "deathSequenceEventListBox";
            this.deathSequenceEventListBox.Size = new System.Drawing.Size(304, 173);
            this.deathSequenceEventListBox.TabIndex = 1;
            // 
            // addDeathSequenceEventButton
            // 
            this.addDeathSequenceEventButton.Location = new System.Drawing.Point(2, 142);
            this.addDeathSequenceEventButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.addDeathSequenceEventButton.Name = "addDeathSequenceEventButton";
            this.addDeathSequenceEventButton.Size = new System.Drawing.Size(93, 23);
            this.addDeathSequenceEventButton.TabIndex = 2;
            this.addDeathSequenceEventButton.Text = "Add Event";
            this.addDeathSequenceEventButton.UseVisualStyleBackColor = true;
            this.addDeathSequenceEventButton.Click += new System.EventHandler(this.addDeathSequenceEventButton_Click);
            // 
            // deathSequenceEventPropertyGrid
            // 
            this.deathSequenceEventPropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.deathSequenceEventPropertyGrid.Location = new System.Drawing.Point(2, 344);
            this.deathSequenceEventPropertyGrid.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.deathSequenceEventPropertyGrid.Name = "deathSequenceEventPropertyGrid";
            this.deathSequenceEventPropertyGrid.Size = new System.Drawing.Size(303, 281);
            this.deathSequenceEventPropertyGrid.TabIndex = 3;
            // 
            // removeDeathSequenceEventButton
            // 
            this.removeDeathSequenceEventButton.Location = new System.Drawing.Point(99, 142);
            this.removeDeathSequenceEventButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.removeDeathSequenceEventButton.Name = "removeDeathSequenceEventButton";
            this.removeDeathSequenceEventButton.Size = new System.Drawing.Size(93, 23);
            this.removeDeathSequenceEventButton.TabIndex = 4;
            this.removeDeathSequenceEventButton.Text = "Remove Event";
            this.removeDeathSequenceEventButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 637);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Mesh:";
            // 
            // meshTextBox
            // 
            this.meshTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.meshTextBox.Location = new System.Drawing.Point(41, 637);
            this.meshTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.meshTextBox.Name = "meshTextBox";
            this.meshTextBox.ReadOnly = true;
            this.meshTextBox.Size = new System.Drawing.Size(235, 20);
            this.meshTextBox.TabIndex = 6;
            // 
            // loadMeshButton
            // 
            this.loadMeshButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.loadMeshButton.Location = new System.Drawing.Point(279, 634);
            this.loadMeshButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.loadMeshButton.Name = "loadMeshButton";
            this.loadMeshButton.Size = new System.Drawing.Size(25, 23);
            this.loadMeshButton.TabIndex = 7;
            this.loadMeshButton.Text = "...";
            this.loadMeshButton.UseVisualStyleBackColor = true;
            // 
            // DeathSequenceEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.loadMeshButton);
            this.Controls.Add(this.meshTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.removeDeathSequenceEventButton);
            this.Controls.Add(this.deathSequenceEventPropertyGrid);
            this.Controls.Add(this.addDeathSequenceEventButton);
            this.Controls.Add(this.deathSequenceEventListBox);
            this.Controls.Add(this.deathSequencePropertyGrid);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "DeathSequenceEditorControl";
            this.Size = new System.Drawing.Size(307, 660);
            this.ResumeLayout(false);
            this.PerformLayout();

		}


		private DeathSequenceModel model;


		private IContainer components;


		private PropertyGrid deathSequencePropertyGrid;


		private ListBox deathSequenceEventListBox;


		private Button addDeathSequenceEventButton;


		private PropertyGrid deathSequenceEventPropertyGrid;


		private Button removeDeathSequenceEventButton;


		private Label label1;


		private TextBox meshTextBox;


		private Button loadMeshButton;

        private void addDeathSequenceEventButton_Click(object sender, EventArgs e)
        {

        }
    }
}
