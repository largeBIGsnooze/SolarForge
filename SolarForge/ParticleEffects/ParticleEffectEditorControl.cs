using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Solar.Rendering;

namespace SolarForge.ParticleEffects
{

	public class ParticleEffectEditorControl : UserControl
	{

		public ParticleEffectEditorControl()
		{
			this.InitializeComponent();
			this.treeControl = new ParticleEffectPropertiesTreeControl();
			this.treeControl.Dock = DockStyle.Fill;
			this.splitContainer.Panel1.Controls.Add(this.treeControl);
			this.emitterEditorControl = new ParticleEmitterEditorControl();
			this.emitterEditorControl.Dock = DockStyle.Fill;
			this.emitterEditorControl.Visible = false;
			this.splitContainer.Panel2.Controls.Add(this.emitterEditorControl);
			this.modifierEditorControl = new ParticleModifierEditorControl();
			this.modifierEditorControl.Dock = DockStyle.Fill;
			this.modifierEditorControl.Visible = false;
			this.splitContainer.Panel2.Controls.Add(this.modifierEditorControl);
			this.nodePropertyGrid = new PropertyGrid();
			this.nodePropertyGrid.Dock = DockStyle.Fill;
			this.nodePropertyGrid.Visible = false;
			this.nodePropertyGrid.PropertyValueChanged += delegate(object s, PropertyValueChangedEventArgs e)
			{
				this.model.HandleParticleEffectPropertyValueChanged(e.ChangedItem.Label);
			};
			this.splitContainer.Panel2.Controls.Add(this.nodePropertyGrid);
			this.maxScalarPropertyGrid = new PropertyGrid();
			this.maxScalarPropertyGrid.Dock = DockStyle.Fill;
			this.maxScalarPropertyGrid.Visible = false;
			this.maxScalarPropertyGrid.PropertyValueChanged += delegate(object s, PropertyValueChangedEventArgs e)
			{
				this.model.HandleParticleEffectPropertyValueChanged(e.ChangedItem.Label);
			};
			this.splitContainer.Panel2.Controls.Add(this.maxScalarPropertyGrid);
		}



		public ParticleEffectModel Model
		{
			set
			{
				this.model = value;
				this.model.SelectedParticleEffectComponentChanged += this.Model_SelectedParticleEffectComponentChanged;
				this.model.MeshChanged += this.Model_MeshChanged;
				this.model.MeshPointIndexChanged += this.Model_MeshPointIndexChanged;
				this.treeControl.Model = value;
				this.emitterEditorControl.Model = value;
				this.modifierEditorControl.Model = value;
			}
		}


		private void Model_MeshChanged()
		{
			this.meshPointComboBox.Items.Clear();
			if (this.model.Mesh == null)
			{
				this.meshTextBox.Text = "";
				return;
			}
			this.meshTextBox.Text = Path.GetFileNameWithoutExtension(this.model.Mesh.LoadedPath);
			foreach (MeshPoint meshPoint in this.model.Mesh.Data.Points)
			{
				this.meshPointComboBox.Items.Add(meshPoint.Name);
			}
		}


		private void Model_MeshPointIndexChanged()
		{
			if (this.model.MeshPointIndex < this.meshPointComboBox.Items.Count)
			{
				this.meshPointComboBox.SelectedIndex = this.model.MeshPointIndex;
			}
		}


		private void Model_SelectedParticleEffectComponentChanged(object selectedComponent)
		{
			this.emitterEditorControl.Visible = false;
			this.modifierEditorControl.Visible = false;
			this.nodePropertyGrid.Visible = false;
			this.maxScalarPropertyGrid.Visible = false;
			this.nodePropertyGrid.SelectedObject = null;
			if (selectedComponent is ParticleModifier)
			{
				this.modifierEditorControl.ParticleModifier = (ParticleModifier)selectedComponent;
				this.modifierEditorControl.Visible = true;
				return;
			}
			if (selectedComponent is ParticleEmitter)
			{
				this.emitterEditorControl.ParticleEmitter = (ParticleEmitter)selectedComponent;
				this.emitterEditorControl.Visible = true;
				return;
			}
			if (selectedComponent is ParticleEmitterNode)
			{
				this.nodePropertyGrid.SelectedObject = selectedComponent;
				this.nodePropertyGrid.Visible = true;
				return;
			}
			if (selectedComponent is ParticleMaxScalarNode)
			{
				this.maxScalarPropertyGrid.SelectedObject = selectedComponent;
				this.maxScalarPropertyGrid.Visible = true;
			}
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
					this.model.LoadMesh(openFileDialog.FileName);
				}
			}
		}


		private void removeMeshButton_Click(object sender, EventArgs e)
		{
			this.model.RemoveMesh();
		}


		private void meshPointComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.model.SetMeshPointIndex(this.meshPointComboBox.SelectedIndex, false);
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
			this.splitContainer = new SplitContainer();
			this.meshLabel = new Label();
			this.meshPointLabel = new Label();
			this.meshTextBox = new TextBox();
			this.loadMeshButton = new Button();
			this.removeMeshButton = new Button();
			this.meshPointComboBox = new ComboBox();
			((ISupportInitialize)this.splitContainer).BeginInit();
			this.splitContainer.SuspendLayout();
			base.SuspendLayout();
			this.splitContainer.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.splitContainer.Location = new Point(0, 0);
			this.splitContainer.Margin = new Padding(4, 5, 4, 5);
			this.splitContainer.Name = "splitContainer";
			this.splitContainer.Size = new Size(820, 713);
			this.splitContainer.SplitterDistance = 264;
			this.splitContainer.SplitterWidth = 6;
			this.splitContainer.TabIndex = 0;
			this.meshLabel.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.meshLabel.AutoSize = true;
			this.meshLabel.Location = new Point(20, 740);
			this.meshLabel.Name = "meshLabel";
			this.meshLabel.Size = new Size(52, 20);
			this.meshLabel.TabIndex = 1;
			this.meshLabel.Text = "Mesh:";
			this.meshPointLabel.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.meshPointLabel.AutoSize = true;
			this.meshPointLabel.Location = new Point(20, 775);
			this.meshPointLabel.Name = "meshPointLabel";
			this.meshPointLabel.Size = new Size(92, 20);
			this.meshPointLabel.TabIndex = 2;
			this.meshPointLabel.Text = "Mesh Point:";
			this.meshTextBox.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.meshTextBox.Location = new Point(129, 740);
			this.meshTextBox.Name = "meshTextBox";
			this.meshTextBox.ReadOnly = true;
			this.meshTextBox.Size = new Size(593, 26);
			this.meshTextBox.TabIndex = 3;
			this.loadMeshButton.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
			this.loadMeshButton.Location = new Point(728, 734);
			this.loadMeshButton.Name = "loadMeshButton";
			this.loadMeshButton.Size = new Size(37, 38);
			this.loadMeshButton.TabIndex = 4;
			this.loadMeshButton.Text = "...";
			this.loadMeshButton.UseVisualStyleBackColor = true;
			this.loadMeshButton.Click += this.loadMeshButton_Click;
			this.removeMeshButton.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
			this.removeMeshButton.Location = new Point(771, 734);
			this.removeMeshButton.Name = "removeMeshButton";
			this.removeMeshButton.Size = new Size(36, 38);
			this.removeMeshButton.TabIndex = 5;
			this.removeMeshButton.Text = "X";
			this.removeMeshButton.UseVisualStyleBackColor = true;
			this.removeMeshButton.Click += this.removeMeshButton_Click;
			this.meshPointComboBox.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.meshPointComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			this.meshPointComboBox.FormattingEnabled = true;
			this.meshPointComboBox.Location = new Point(129, 775);
			this.meshPointComboBox.Name = "meshPointComboBox";
			this.meshPointComboBox.Size = new Size(593, 28);
			this.meshPointComboBox.TabIndex = 6;
			this.meshPointComboBox.SelectedIndexChanged += this.meshPointComboBox_SelectedIndexChanged;
			base.AutoScaleDimensions = new SizeF(9f, 20f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.meshPointComboBox);
			base.Controls.Add(this.removeMeshButton);
			base.Controls.Add(this.loadMeshButton);
			base.Controls.Add(this.meshTextBox);
			base.Controls.Add(this.meshPointLabel);
			base.Controls.Add(this.meshLabel);
			base.Controls.Add(this.splitContainer);
			base.Margin = new Padding(4, 5, 4, 5);
			base.Name = "ParticleEffectEditorControl";
			base.Size = new Size(820, 825);
			((ISupportInitialize)this.splitContainer).EndInit();
			this.splitContainer.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}


		private ParticleEffectModel model;


		private ParticleEffectPropertiesTreeControl treeControl;


		private ParticleEmitterEditorControl emitterEditorControl;


		private ParticleModifierEditorControl modifierEditorControl;


		private PropertyGrid nodePropertyGrid;


		private PropertyGrid maxScalarPropertyGrid;


		private IContainer components;


		private SplitContainer splitContainer;


		private Label meshLabel;


		private Label meshPointLabel;


		private TextBox meshTextBox;


		private Button loadMeshButton;


		private Button removeMeshButton;


		private ComboBox meshPointComboBox;
	}
}
