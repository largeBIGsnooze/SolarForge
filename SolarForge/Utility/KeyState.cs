using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SolarForge.Utility
{

	internal class KeyState
	{

		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		public static extern short GetAsyncKeyState(int vKey);


		public static bool IsKeyDown(Keys key)
		{
			return KeyState.GetAsyncKeyState((int)key) < 0;
		}
	}
}
