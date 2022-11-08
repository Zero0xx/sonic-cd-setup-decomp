using System;

namespace System
{
	// Token: 0x02000092 RID: 146
	[Serializable]
	public sealed class ConsoleCancelEventArgs : EventArgs
	{
		// Token: 0x060007FA RID: 2042 RVA: 0x0001A22B File Offset: 0x0001922B
		internal ConsoleCancelEventArgs(ConsoleSpecialKey type)
		{
			this._type = type;
			this._cancel = false;
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060007FB RID: 2043 RVA: 0x0001A241 File Offset: 0x00019241
		// (set) Token: 0x060007FC RID: 2044 RVA: 0x0001A249 File Offset: 0x00019249
		public bool Cancel
		{
			get
			{
				return this._cancel;
			}
			set
			{
				if (this._type == ConsoleSpecialKey.ControlBreak && value)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CantCancelCtrlBreak"));
				}
				this._cancel = value;
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060007FD RID: 2045 RVA: 0x0001A26E File Offset: 0x0001926E
		public ConsoleSpecialKey SpecialKey
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x040002D6 RID: 726
		private ConsoleSpecialKey _type;

		// Token: 0x040002D7 RID: 727
		private bool _cancel;
	}
}
