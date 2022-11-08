using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x0200077C RID: 1916
	internal class MainWindowFinder
	{
		// Token: 0x06003B28 RID: 15144 RVA: 0x000FBC24 File Offset: 0x000FAC24
		public IntPtr FindMainWindow(int processId)
		{
			this.bestHandle = (IntPtr)0;
			this.processId = processId;
			NativeMethods.EnumThreadWindowsCallback enumThreadWindowsCallback = new NativeMethods.EnumThreadWindowsCallback(this.EnumWindowsCallback);
			NativeMethods.EnumWindows(enumThreadWindowsCallback, IntPtr.Zero);
			GC.KeepAlive(enumThreadWindowsCallback);
			return this.bestHandle;
		}

		// Token: 0x06003B29 RID: 15145 RVA: 0x000FBC69 File Offset: 0x000FAC69
		private bool IsMainWindow(IntPtr handle)
		{
			return !(NativeMethods.GetWindow(new HandleRef(this, handle), 4) != (IntPtr)0) && NativeMethods.IsWindowVisible(new HandleRef(this, handle));
		}

		// Token: 0x06003B2A RID: 15146 RVA: 0x000FBC98 File Offset: 0x000FAC98
		private bool EnumWindowsCallback(IntPtr handle, IntPtr extraParameter)
		{
			int num;
			NativeMethods.GetWindowThreadProcessId(new HandleRef(this, handle), out num);
			if (num == this.processId && this.IsMainWindow(handle))
			{
				this.bestHandle = handle;
				return false;
			}
			return true;
		}

		// Token: 0x040033CE RID: 13262
		private IntPtr bestHandle;

		// Token: 0x040033CF RID: 13263
		private int processId;
	}
}
