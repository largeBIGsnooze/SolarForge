using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

namespace SolarForge.Utility
{

	public class AdvancedFolderNameEditor : UITypeEditor
	{

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}


		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			using (AdvancedFolderBrowserDialog advancedFolderBrowserDialog = new AdvancedFolderBrowserDialog())
			{
				if (value != null)
				{
					advancedFolderBrowserDialog.DirectoryPath = string.Format("{0}", value);
				}
				if (advancedFolderBrowserDialog.ShowDialog(null) == DialogResult.OK)
				{
					return advancedFolderBrowserDialog.DirectoryPath;
				}
			}
			return value;
		}
	}
}
