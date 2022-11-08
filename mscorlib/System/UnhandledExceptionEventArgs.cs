using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x02000137 RID: 311
	[ComVisible(true)]
	[Serializable]
	public class UnhandledExceptionEventArgs : EventArgs
	{
		// Token: 0x06001146 RID: 4422 RVA: 0x0002FE5A File Offset: 0x0002EE5A
		public UnhandledExceptionEventArgs(object exception, bool isTerminating)
		{
			this._Exception = exception;
			this._IsTerminating = isTerminating;
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06001147 RID: 4423 RVA: 0x0002FE70 File Offset: 0x0002EE70
		public object ExceptionObject
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return this._Exception;
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06001148 RID: 4424 RVA: 0x0002FE78 File Offset: 0x0002EE78
		public bool IsTerminating
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return this._IsTerminating;
			}
		}

		// Token: 0x040005D9 RID: 1497
		private object _Exception;

		// Token: 0x040005DA RID: 1498
		private bool _IsTerminating;
	}
}
