using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Solar.Rendering;

namespace SolarForge.ParticleEffects
{

	public class ParticleEmitterEditorControl : UserControl
	{

		public ParticleEmitterEditorControl()
		{
			this.InitializeComponent();
			foreach (object item in Enum.GetValues(typeof(ParticleEmitterType)))
			{
				this.emitterTypeComboBox.Items.Add(item);
			}
			this.emitterTypeComboBox.SelectedIndexChanged += this.EmitterTypeComboBox_SelectedIndexChanged;
			this.emitterPropertyGrid.PropertyValueChanged += delegate(object s, PropertyValueChangedEventArgs e)
			{
				this.model.HandleParticleEffectPropertyValueChanged(e.ChangedItem.Label);
			};
			foreach (object item2 in Enum.GetValues(typeof(ParticleType)))
			{
				this.particleTypeComboBox.Items.Add(item2);
			}
			this.particleTypeComboBox.SelectedIndexChanged += this.ParticleTypeComboBox_SelectedIndexChanged;
			this.particlePropertyGrid.PropertyValueChanged += delegate(object s, PropertyValueChangedEventArgs e)
			{
				this.model.HandleParticleEffectPropertyValueChanged(e.ChangedItem.Label);
			};
		}


		private void EmitterTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.emitter != null)
			{
				this.emitter.Type = (ParticleEmitterType)this.emitterTypeComboBox.SelectedItem;
				this.model.HandleParticleEmitterTypeChanged();
				this.UpdateEmitterPropertyGridSelectedObject();
			}
		}


		private void ParticleTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.emitter != null)
			{
				this.emitter.ParticleType = (ParticleType)this.particleTypeComboBox.SelectedItem;
				this.model.HandleParticleEmitterParticleTypeChanged();
				this.UpdateParticlePropertyGridSelectedObject();
			}
		}



		public ParticleEffectModel Model
		{
			set
			{
				this.model = value;
			}
		}



		public ParticleEmitter ParticleEmitter
		{
			set
			{
				this.emitter = value;
				if (this.emitter != null)
				{
					this.emitterTypeComboBox.SelectedItem = this.emitter.Type;
					this.particleTypeComboBox.SelectedItem = this.emitter.ParticleType;
				}
				this.UpdateEmitterPropertyGridSelectedObject();
				this.UpdateParticlePropertyGridSelectedObject();
			}
		}


		private void UpdateEmitterPropertyGridSelectedObject()
		{
			if (this.emitter == null)
			{
				this.emitterPropertyGrid.SelectedObject = null;
				return;
			}
			this.emitterPropertyGrid.SelectedObject = this.emitter.Properties;
		}


		private void UpdateParticlePropertyGridSelectedObject()
		{
			if (this.emitter == null)
			{
				this.particlePropertyGrid.SelectedObject = null;
				return;
			}
			this.particlePropertyGrid.SelectedObject = this.emitter.ParticleProperties;
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
			this.emitterTypeComboBox = new ComboBox();
			this.emitterTypeLabel = new Label();
			this.emitterPropertyGrid = new PropertyGrid();
			this.particleTypeLabel = new Label();
			this.particleTypeComboBox = new ComboBox();
			this.particlePropertyGrid = new PropertyGrid();
			((ISupportInitialize)this.splitContainer).BeginInit();
			this.splitContainer.Panel1.SuspendLayout();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			base.SuspendLayout();
			this.splitContainer.Dock = DockStyle.Fill;
			this.splitContainer.Location = new Point(0, 0);
			this.splitContainer.Name = "splitContainer";
			this.splitContainer.Orientation = Orientation.Horizontal;
			this.splitContainer.Panel1.Controls.Add(this.emitterPropertyGrid);
			this.splitContainer.Panel1.Controls.Add(this.emitterTypeComboBox);
			this.splitContainer.Panel1.Controls.Add(this.emitterTypeLabel);
			this.splitContainer.Panel2.Controls.Add(this.particlePropertyGrid);
			this.splitContainer.Panel2.Controls.Add(this.particleTypeComboBox);
			this.splitContainer.Panel2.Controls.Add(this.particleTypeLabel);
			this.splitContainer.Size = new Size(1117, 1153);
			this.splitContainer.SplitterDistance = 576;
			this.splitContainer.TabIndex = 6;
			this.emitterTypeComboBox.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.emitterTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			this.emitterTypeComboBox.FormattingEnabled = true;
			this.emitterTypeComboBox.Location = new Point(120, 11);
			this.emitterTypeComboBox.Name = "emitterTypeComboBox";
			this.emitterTypeComboBox.Size = new Size(979, 28);
			this.emitterTypeComboBox.TabIndex = 4;
			this.emitterTypeLabel.AutoSize = true;
			this.emitterTypeLabel.Location = new Point(12, 14);
			this.emitterTypeLabel.Name = "emitterTypeLabel";
			this.emitterTypeLabel.Size = new Size(102, 20);
			this.emitterTypeLabel.TabIndex = 3;
			this.emitterTypeLabel.Text = "Emitter Type:";
			this.emitterPropertyGrid.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.emitterPropertyGrid.Location = new Point(16, 45);
			this.emitterPropertyGrid.Name = "emitterPropertyGrid";
			this.emitterPropertyGrid.Size = new Size(1083, 519);
			this.emitterPropertyGrid.TabIndex = 5;
			this.particleTypeLabel.AutoSize = true;
			this.particleTypeLabel.Location = new Point(12, 10);
			this.particleTypeLabel.Name = "particleTypeLabel";
			this.particleTypeLabel.Size = new Size(103, 20);
			this.particleTypeLabel.TabIndex = 2;
			this.particleTypeLabel.Text = "Particle Type:";
			this.particleTypeComboBox.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.particleTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			this.particleTypeComboBox.FormattingEnabled = true;
			this.particleTypeComboBox.Location = new Point(120, 7);
			this.particleTypeComboBox.Name = "particleTypeComboBox";
			this.particleTypeComboBox.Size = new Size(979, 28);
			this.particleTypeComboBox.TabIndex = 4;
			this.particlePropertyGrid.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.particlePropertyGrid.Location = new Point(16, 41);
			this.particlePropertyGrid.Name = "particlePropertyGrid";
			this.particlePropertyGrid.Size = new Size(1083, 529);
			this.particlePropertyGrid.TabIndex = 6;
			base.AutoScaleDimensions = new SizeF(9f, 20f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.splitContainer);
			base.Name = "ParticleEmitterEditorControl";
			base.Size = new Size(1117, 1153);
			this.splitContainer.Panel1.ResumeLayout(false);
			this.splitContainer.Panel1.PerformLayout();
			this.splitContainer.Panel2.ResumeLayout(false);
			this.splitContainer.Panel2.PerformLayout();
			((ISupportInitialize)this.splitContainer).EndInit();
			this.splitContainer.ResumeLayout(false);
			base.ResumeLayout(false);
		}


		private ParticleEffectModel model;


		private ParticleEmitter emitter;


		private IContainer components;


		private SplitContainer splitContainer;


		private PropertyGrid emitterPropertyGrid;


		private ComboBox emitterTypeComboBox;


		private Label emitterTypeLabel;


		private PropertyGrid particlePropertyGrid;


		private ComboBox particleTypeComboBox;


		private Label particleTypeLabel;
	}
}
