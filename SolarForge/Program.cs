using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Windows.Forms;
using Solar;
using Solar.Simulations;

namespace SolarForge
{

	internal static class Program
	{

		[STAThread]
		private static void Main()
		{
			try
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				using (ProgramModel model = new ProgramModel())
				{
					MainForm mainForm = new MainForm();
					mainForm.Show();
					string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Program.ResolveAppUserFolder());
					model.Settings.Load(text);
					model.Settings.Fixup();
					ProgramOutputHandler programOutputHandler = new ProgramOutputHandler();
					programOutputHandler.BeginErrorSuppression();
					model.Engine.Setup(mainForm.ViewportControl.Handle, model.Settings.RootDataFolderPath, programOutputHandler);
					List<string> list = programOutputHandler.EndErrorSuppression();
					if (list.Count > 0)
					{
						string text2 = Path.Combine(text, "setupErrors.log");
						File.WriteAllText(text2, string.Join("\n", list));
						if (list.Count <= 10)
						{
							using (List<string>.Enumerator enumerator = list.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									string text3 = enumerator.Current;
									MessageBox.Show(text3, "Setup Errors", MessageBoxButtons.OK, MessageBoxIcon.Hand);
								}
								goto IL_16C;
							}
						}
						MessageBox.Show(list.ElementAt(0), "Setup Errors", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						MessageBox.Show(string.Format("Found errors while setting up the core engine. This is most likely due to either your Sins2 Folder being set incorrectly (fix in settings). Or SolarForge is old and needs to be updated. See {0} for errors", text2), "Setup Errors", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					}
					IL_16C:
					Program.BindNameProvidersToEngine(model.Engine);
					model.CreateModels();
					mainForm.ProgramModel = model;
					mainForm.ViewportControl.HandleWndProc = delegate(ref Message m)
					{
						return model.Engine.HandleWndProc(m.HWnd, m.Msg, m.WParam, m.LParam);
					};
					model.SetViewportSize(mainForm.ViewportControl.ClientSize);
					mainForm.ViewportControl.ClientSizeChanged += delegate(object sender, EventArgs e)
					{
						model.SetViewportSize(mainForm.ViewportControl.ClientSize);
					};
					model.Update();
					while (mainForm.Created)
					{
						Application.DoEvents();
						model.Update();
						if (mainForm.WindowState == FormWindowState.Minimized)
						{
							Thread.Sleep(100);
						}
						else
						{
							bool canPan = false;
							if (!mainForm.ViewportControl.IsDisposed)
							{
								canPan = mainForm.ViewportControl.RectangleToScreen(mainForm.ViewportControl.ClientRectangle).Contains(Cursor.Position);
							}
							model.UpdateAndRenderScene(list.Count > 0, canPan);
						}
						mainForm.EditorStatusText = model.EditorStatusText;
					}
					model.Settings.Save(text);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + "\n\n" + ex.StackTrace, ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}


		private static void BindNameProvidersToEngine(Engine engine)
		{
			WeaponDefinitionNameConverter.NameProvider = engine.EntityDefinitionFactories;
			UnitDefinitionNameConverter.NameProvider = engine.EntityDefinitionFactories;
			UnitSkinDefinitionNameConverter.NameProvider = engine.EntityDefinitionFactories;
			GravityWellFixtureNameConverter.NameProvider = engine.EntityDefinitionFactories;
		}


		private static string ResolveAppUserFolder()
		{
			if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), ".solarforge")))
			{ 
				try  
				{
                    return JsonSerializer.Deserialize<Program.ProgramConfig>(File.ReadAllText(".solarforge"), new JsonSerializerOptions()).UserFolder;
				}
				catch 
				{
				}
			}
			return Application.ProductName;
		}


		private class ProgramConfig
		{



			public string UserFolder { get; set; }
		}
	}
}
