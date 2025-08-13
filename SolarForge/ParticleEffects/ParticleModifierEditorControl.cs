using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Solar.Rendering;

namespace SolarForge.ParticleEffects
{

	public class ParticleModifierEditorControl : UserControl
	{

		public ParticleModifierEditorControl()
		{
			this.InitializeComponent();
			foreach (object item in Enum.GetValues(typeof(ParticleModifierType)))
			{
				this.modifierTypeComboBox.Items.Add(item);
			}
			this.modifierTypeComboBox.SelectedIndexChanged += this.ModifierTypeComboBox_SelectedIndexChanged;
			this.propertyGrid.PropertyValueChanged += delegate(object s, PropertyValueChangedEventArgs e)
			{
				this.model.HandleParticleEffectPropertyValueChanged(e.ChangedItem.Label);
			};
		}


		private void ModifierTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.modifier != null)
			{
				this.modifier.Type = (ParticleModifierType)this.modifierTypeComboBox.SelectedItem;
				this.model.HandleParticleModifierTypeChanged();
				this.UpdatePropertyGridSelectedObject();
			}
		}



		public ParticleEffectModel Model
		{
			set
			{
				this.model = value;
			}
		}



		public ParticleModifier ParticleModifier
		{
			set
			{
				this.modifier = value;
				if (this.modifier != null)
				{
					this.modifierTypeComboBox.SelectedItem = this.modifier.Type;
				}
				this.UpdatePropertyGridSelectedObject();
			}
		}


		private void UpdatePropertyGridSelectedObject()
		{
			if (this.modifier == null)
			{
				this.propertyGrid.SelectedObject = null;
				return;
			}
			this.propertyGrid.SelectedObject = this.modifier.Properties;
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
			this.propertyGrid = new PropertyGrid();
			this.modifierTypeLabel = new Label();
			this.modifierTypeComboBox = new ComboBox();
			base.SuspendLayout();
			this.propertyGrid.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.propertyGrid.Location = new Point(7, 59);
			this.propertyGrid.Name = "propertyGrid";
			this.propertyGrid.Size = new Size(524, 717);
			this.propertyGrid.TabIndex = 0;
			this.modifierTypeLabel.AutoSize = true;
			this.modifierTypeLabel.Location = new Point(3, 20);
			this.modifierTypeLabel.Name = "modifierTypeLabel";
			this.modifierTypeLabel.Size = new Size(103, 20);
			this.modifierTypeLabel.TabIndex = 1;
			this.modifierTypeLabel.Text = "Modifier Type";
			this.modifierTypeComboBox.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.modifierTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			this.modifierTypeComboBox.FormattingEnabled = true;
			this.modifierTypeComboBox.Location = new Point(112, 17);
			this.modifierTypeComboBox.Name = "modifierTypeComboBox";
			this.modifierTypeComboBox.Size = new Size(419, 28);
			this.modifierTypeComboBox.TabIndex = 2;
			base.AutoScaleDimensions = new SizeF(9f, 20f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.modifierTypeComboBox);
			base.Controls.Add(this.modifierTypeLabel);
			base.Controls.Add(this.propertyGrid);
			base.Name = "ParticleModifierEditorControl";
			base.Size = new Size(547, 776);
			base.ResumeLayout(false);
			base.PerformLayout();
		}


		private ParticleEffectModel model;


		private ParticleModifier modifier;


		private IContainer components;


		private PropertyGrid propertyGrid;


		private Label modifierTypeLabel;


		private ComboBox modifierTypeComboBox;
	}
}
