using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Solar.Math;
using Solar.Simulations;

namespace SolarForge.Units
{

	public class UnitCarrierEditorControl : UserControl
	{

		public UnitCarrierEditorControl()
		{
			this.InitializeComponent();
		}



		public UnitModel Model
		{
			set
			{
				this.model = value;
				this.model.UnitDefinitionChanged += this.Model_UnitDefinitionChanged;
			}
		}


		private void Model_UnitDefinitionChanged(UnitDefinition unitDefinition)
		{
			this.SyncHangarPointsListBoxToModel();
			this.addHangarPointButton.Enabled = (this.model.UnitDefinition != null && this.model.UnitDefinition.Carrier != null);
			this.removeHangarPointButton.Enabled = (this.model.UnitDefinition != null && this.model.UnitDefinition.Carrier != null);
		}


		private void SyncHangarPointsListBoxToModel()
		{
			this.hangarPointListBox.Items.Clear();
			if (this.model.UnitDefinition != null && this.model.UnitDefinition.Carrier != null)
			{
				foreach (Basis basis in this.model.UnitDefinition.Carrier.HangarPoints)
				{
					this.hangarPointListBox.Items.Add(basis.Position.ToString());
				}
			}
		}


		private void addHangarPointButton_Click(object sender, EventArgs e)
		{
			if (this.model.SelectedSkinStageMeshPointIndex == -1)
			{
				MessageBox.Show("Select a Mesh Point in the Skin Tab");
				return;
			}
			this.model.UnitDefinition.Carrier.AddHangerPoint(new Basis(this.model.SkinStageMesh.Data.Points[this.model.SelectedSkinStageMeshPointIndex].Position, this.model.SkinStageMesh.Data.Points[this.model.SelectedSkinStageMeshPointIndex].Rotation));
			this.SyncHangarPointsListBoxToModel();
		}


		private void removeHangarPointButton_Click(object sender, EventArgs e)
		{
			if (this.hangarPointListBox.SelectedIndex != -1)
			{
				this.model.UnitDefinition.Carrier.RemoveHangerPointAt(this.hangarPointListBox.SelectedIndex);
				this.SyncHangarPointsListBoxToModel();
			}
		}


		private void syncHangarPointsToMesh(object sender, EventArgs e)
		{
			this.model.SyncHangarPointsToMesh();
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
			this.groupBox1 = new GroupBox();
			this.syncHangarPointsToMeshButton = new Button();
			this.removeHangarPointButton = new Button();
			this.addHangarPointButton = new Button();
			this.hangarPointListBox = new ListBox();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.groupBox1.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.groupBox1.Controls.Add(this.syncHangarPointsToMeshButton);
			this.groupBox1.Controls.Add(this.removeHangarPointButton);
			this.groupBox1.Controls.Add(this.addHangarPointButton);
			this.groupBox1.Controls.Add(this.hangarPointListBox);
			this.groupBox1.Location = new Point(3, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(442, 532);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Hangar Points";
			this.syncHangarPointsToMeshButton.Location = new Point(285, 25);
			this.syncHangarPointsToMeshButton.Margin = new Padding(4, 5, 4, 5);
			this.syncHangarPointsToMeshButton.Name = "syncHangarPointsToMeshButton";
			this.syncHangarPointsToMeshButton.Size = new Size(134, 35);
			this.syncHangarPointsToMeshButton.TabIndex = 3;
			this.syncHangarPointsToMeshButton.Text = "Sync to Mesh";
			this.syncHangarPointsToMeshButton.UseVisualStyleBackColor = true;
			this.syncHangarPointsToMeshButton.Click += this.syncHangarPointsToMesh;
			this.removeHangarPointButton.Location = new Point(144, 25);
			this.removeHangarPointButton.Name = "removeHangarPointButton";
			this.removeHangarPointButton.Size = new Size(132, 37);
			this.removeHangarPointButton.TabIndex = 2;
			this.removeHangarPointButton.Text = "Remove";
			this.removeHangarPointButton.UseVisualStyleBackColor = true;
			this.removeHangarPointButton.Click += this.removeHangarPointButton_Click;
			this.addHangarPointButton.Location = new Point(6, 25);
			this.addHangarPointButton.Name = "addHangarPointButton";
			this.addHangarPointButton.Size = new Size(132, 37);
			this.addHangarPointButton.TabIndex = 1;
			this.addHangarPointButton.Text = "Add";
			this.addHangarPointButton.UseVisualStyleBackColor = true;
			this.addHangarPointButton.Click += this.addHangarPointButton_Click;
			this.hangarPointListBox.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.hangarPointListBox.FormattingEnabled = true;
			this.hangarPointListBox.ItemHeight = 20;
			this.hangarPointListBox.Location = new Point(6, 68);
			this.hangarPointListBox.Name = "hangarPointListBox";
			this.hangarPointListBox.Size = new Size(436, 444);
			this.hangarPointListBox.TabIndex = 0;
			base.AutoScaleDimensions = new SizeF(9f, 20f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.groupBox1);
			base.Name = "UnitCarrierEditorControl";
			base.Size = new Size(462, 538);
			this.groupBox1.ResumeLayout(false);
			base.ResumeLayout(false);
		}


		private UnitModel model;


		private IContainer components;


		private GroupBox groupBox1;


		private Button removeHangarPointButton;


		private Button addHangarPointButton;


		private Button syncHangarPointsToMeshButton;


		private ListBox hangarPointListBox;
	}
}
