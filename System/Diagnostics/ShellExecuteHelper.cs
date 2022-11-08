using System;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x0200077B RID: 1915
	internal class ShellExecuteHelper
	{
		// Token: 0x06003B24 RID: 15140 RVA: 0x000FBB8B File Offset: 0x000FAB8B
		public ShellExecuteHelper(NativeMethods.ShellExecuteInfo executeInfo)
		{
			this._executeInfo = executeInfo;
		}

		// Token: 0x06003B25 RID: 15141 RVA: 0x000FBB9C File Offset: 0x000FAB9C
		public void ShellExecuteFunction()
		{
			if (!(this._succeeded = NativeMethods.ShellExecuteEx(this._executeInfo)))
			{
				this._errorCode = Marshal.GetLastWin32Error();
			}
		}

		// Token: 0x06003B26 RID: 15142 RVA: 0x000FBBCC File Offset: 0x000FABCC
		public bool ShellExecuteOnSTAThread()
		{
			if (Thread.CurrentThread.GetApartmentState() != ApartmentState.STA)
			{
				ThreadStart start = new ThreadStart(this.ShellExecuteFunction);
				Thread thread = new Thread(start);
				thread.SetApartmentState(ApartmentState.STA);
				thread.Start();
				thread.Join();
			}
			else
			{
				this.ShellExecuteFunction();
			}
			return this._succeeded;
		}

		// Token: 0x17000DDD RID: 3549
		// (get) Token: 0x06003B27 RID: 15143 RVA: 0x000FBC1A File Offset: 0x000FAC1A
		public int ErrorCode
		{
			get
			{
				return this._errorCode;
			}
		}

		// Token: 0x040033CB RID: 13259
		private NativeMethods.ShellExecuteInfo _executeInfo;

		// Token: 0x040033CC RID: 13260
		private int _errorCode;

		// Token: 0x040033CD RID: 13261
		private bool _succeeded;
	}
}
