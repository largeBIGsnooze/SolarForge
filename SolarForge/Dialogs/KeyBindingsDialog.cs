using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SolarForge.Dialogs
{

	public partial class KeyBindingsDialog : Form
	{

		public KeyBindingsDialog()
		{
			this.InitializeComponent();
			this.contentTextBox.Text = this.ContentText;
		}


		public string ContentText = string.Concat(new string[]
		{
			"[Reset Camera = F1]",
			Environment.NewLine,
			"[Reset Effect = F5]",
			Environment.NewLine,
			"[Play/Pause = F6]",
			Environment.NewLine,
			"[Advance 1 Frame = F7]",
			Environment.NewLine,
			"[Rotate Camera = Right Click Mouse]",
			Environment.NewLine,
			"[Pan Camera = Left Click Mouse]",
			Environment.NewLine,
			"[Pan Left = A]",
			Environment.NewLine,
			"[Pan Right = D]",
			Environment.NewLine,
			"[Pan Forward = W]",
			Environment.NewLine,
			"[Pan Backward = S]",
			Environment.NewLine,
			"[Pan Up = E]",
			Environment.NewLine,
			"[Pan Down = Q]",
			Environment.NewLine,
			"[Slow Zoom = LSHIFT]",
			Environment.NewLine,
			Environment.NewLine,
			"=====Scenario Editor Specific Keys=====",
			Environment.NewLine,
			Environment.NewLine,
			"[Select Group = Left Click + Mouse Drag Box]",
			Environment.NewLine,
			"[Deselect Group = Escape OR Left Click Empty Space Away From Group]",
			Environment.NewLine,
			"[Translate Selected Group = Drag Node in Selected Group]",
			Environment.NewLine,
			"[Copy Group = Ctrl + C]",
			Environment.NewLine,
			"[Paste Group = Ctrl + V]",
			Environment.NewLine,
			"[Start Rotate Group = R]",
			Environment.NewLine,
			"[End Rotate Group = R]",
			Environment.NewLine,
			"[Undo Previous Action = Ctrl + Z]",
			Environment.NewLine,
			"[Redo Previous Action = Ctrl + Y]",
			Environment.NewLine,
			"[(With Node Selected) Ctrl + Click Node = Add Phase Lane From Selected to Clicked Node]"
		});
	}
}
