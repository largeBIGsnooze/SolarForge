using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SolarForge.Utility
{

	public class AdvancedFolderBrowserDialog : IDisposable
	{



		public string DirectoryPath { get; set; }


		public void Dispose()
		{
		}


		public DialogResult ShowDialog(IWin32Window owner)
		{
			IntPtr parent = (owner != null) ? owner.Handle : AdvancedFolderBrowserDialog.GetActiveWindow();
			AdvancedFolderBrowserDialog.IFileOpenDialog fileOpenDialog = (AdvancedFolderBrowserDialog.IFileOpenDialog)new AdvancedFolderBrowserDialog.FileOpenDialog();
			DialogResult result;
			try
			{
				if (!string.IsNullOrEmpty(this.DirectoryPath))
				{
					uint num = 0U;
					IntPtr intPtr;
					if (AdvancedFolderBrowserDialog.SHILCreateFromPath(this.DirectoryPath, out intPtr, ref num) == 0)
					{
						AdvancedFolderBrowserDialog.IShellItem shellItem;
						if (AdvancedFolderBrowserDialog.SHCreateShellItem(IntPtr.Zero, IntPtr.Zero, intPtr, out shellItem) == 0)
						{
							fileOpenDialog.SetFolder(shellItem);
						}
						Marshal.FreeCoTaskMem(intPtr);
					}
				}
				fileOpenDialog.SetOptions(AdvancedFolderBrowserDialog.FOS.FOS_FORCEFILESYSTEM | AdvancedFolderBrowserDialog.FOS.FOS_PICKFOLDERS);
				uint num2 = fileOpenDialog.Show(parent);
				if (num2 == 2147943623U)
				{
					result = DialogResult.Cancel;
				}
				else if (num2 != 0U)
				{
					result = DialogResult.Abort;
				}
				else
				{
					AdvancedFolderBrowserDialog.IShellItem shellItem;
					fileOpenDialog.GetResult(out shellItem);
					string directoryPath;
					shellItem.GetDisplayName((AdvancedFolderBrowserDialog.SIGDN)2147844096U, out directoryPath);
					this.DirectoryPath = directoryPath;
					result = DialogResult.OK;
				}
			}
			finally
			{
				Marshal.ReleaseComObject(fileOpenDialog);
			}
			return result;
		}


		[DllImport("shell32.dll")]
		private static extern int SHILCreateFromPath([MarshalAs(UnmanagedType.LPWStr)] string pszPath, out IntPtr ppIdl, ref uint rgflnOut);


		[DllImport("shell32.dll")]
		private static extern int SHCreateShellItem(IntPtr pidlParent, IntPtr psfParent, IntPtr pidl, out AdvancedFolderBrowserDialog.IShellItem ppsi);


		[DllImport("user32.dll")]
		private static extern IntPtr GetActiveWindow();


		private const uint ERROR_CANCELLED = 2147943623U;


		[Guid("DC1C5A9C-E88A-4dde-A5A1-60F82A20AEF7")]
		[ComImport]
		private class FileOpenDialog
		{

		} 


		[Guid("42f85136-db7e-439c-85f1-e4075d135fc8")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		private interface IFileOpenDialog
		{

			[PreserveSig]
			uint Show([In] IntPtr parent);


			void SetFileTypes();


			void SetFileTypeIndex([In] uint iFileType);


			void GetFileTypeIndex(out uint piFileType);


			void Advise();


			void Unadvise();


			void SetOptions([In] AdvancedFolderBrowserDialog.FOS fos);


			void GetOptions(out AdvancedFolderBrowserDialog.FOS pfos);


			void SetDefaultFolder(AdvancedFolderBrowserDialog.IShellItem psi);


			void SetFolder(AdvancedFolderBrowserDialog.IShellItem psi);


			void GetFolder(out AdvancedFolderBrowserDialog.IShellItem ppsi);


			void GetCurrentSelection(out AdvancedFolderBrowserDialog.IShellItem ppsi);


			void SetFileName([MarshalAs(UnmanagedType.LPWStr)] [In] string pszName);


			void GetFileName([MarshalAs(UnmanagedType.LPWStr)] out string pszName);


			void SetTitle([MarshalAs(UnmanagedType.LPWStr)] [In] string pszTitle);


			void SetOkButtonLabel([MarshalAs(UnmanagedType.LPWStr)] [In] string pszText);


			void SetFileNameLabel([MarshalAs(UnmanagedType.LPWStr)] [In] string pszLabel);


			void GetResult(out AdvancedFolderBrowserDialog.IShellItem ppsi);


			void AddPlace(AdvancedFolderBrowserDialog.IShellItem psi, int alignment);


			void SetDefaultExtension([MarshalAs(UnmanagedType.LPWStr)] [In] string pszDefaultExtension);


			void Close(int hr);


			void SetClientGuid();


			void ClearClientData();


			void SetFilter([MarshalAs(UnmanagedType.Interface)] IntPtr pFilter);


			void GetResults([MarshalAs(UnmanagedType.Interface)] out IntPtr ppenum);


			void GetSelectedItems([MarshalAs(UnmanagedType.Interface)] out IntPtr ppsai);
		}


		[Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		private interface IShellItem
		{

			void BindToHandler();


			void GetParent();


			void GetDisplayName([In] AdvancedFolderBrowserDialog.SIGDN sigdnName, [MarshalAs(UnmanagedType.LPWStr)] out string ppszName);


			void GetAttributes();


			void Compare();
		}


		private enum SIGDN : uint
		{

			SIGDN_DESKTOPABSOLUTEEDITING = 2147794944U,

			SIGDN_DESKTOPABSOLUTEPARSING = 2147647488U,

			SIGDN_FILESYSPATH = 2147844096U,

			SIGDN_NORMALDISPLAY = 0U,

			SIGDN_PARENTRELATIVE = 2148007937U,

			SIGDN_PARENTRELATIVEEDITING = 2147684353U,

			SIGDN_PARENTRELATIVEFORADDRESSBAR = 2147991553U,

			SIGDN_PARENTRELATIVEPARSING = 2147581953U,

			SIGDN_URL = 2147909632U
		}


		[Flags]
		private enum FOS
		{

			FOS_ALLNONSTORAGEITEMS = 128,

			FOS_ALLOWMULTISELECT = 512,

			FOS_CREATEPROMPT = 8192,

			FOS_DEFAULTNOMINIMODE = 536870912,

			FOS_DONTADDTORECENT = 33554432,

			FOS_FILEMUSTEXIST = 4096,

			FOS_FORCEFILESYSTEM = 64,

			FOS_FORCESHOWHIDDEN = 268435456,

			FOS_HIDEMRUPLACES = 131072,

			FOS_HIDEPINNEDPLACES = 262144,

			FOS_NOCHANGEDIR = 8,

			FOS_NODEREFERENCELINKS = 1048576,

			FOS_NOREADONLYRETURN = 32768,

			FOS_NOTESTFILECREATE = 65536,

			FOS_NOVALIDATE = 256,

			FOS_OVERWRITEPROMPT = 2,

			FOS_PATHMUSTEXIST = 2048,

			FOS_PICKFOLDERS = 32,

			FOS_SHAREAWARE = 16384,

			FOS_STRICTFILETYPES = 4
		}
	}
}
