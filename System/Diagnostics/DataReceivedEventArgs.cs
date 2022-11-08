using System;

namespace System.Diagnostics
{
	// Token: 0x02000745 RID: 1861
	public class DataReceivedEventArgs : EventArgs
	{
		// Token: 0x060038C4 RID: 14532 RVA: 0x000EF989 File Offset: 0x000EE989
		internal DataReceivedEventArgs(string data)
		{
			this._data = data;
		}

		// Token: 0x17000D27 RID: 3367
		// (get) Token: 0x060038C5 RID: 14533 RVA: 0x000EF998 File Offset: 0x000EE998
		public string Data
		{
			get
			{
				return this._data;
			}
		}

		// Token: 0x04003261 RID: 12897
		internal string _data;
	}
}
