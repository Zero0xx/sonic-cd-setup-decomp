using System;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x0200004C RID: 76
	[ComVisible(true)]
	public class ResolveEventArgs : EventArgs
	{
		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000425 RID: 1061 RVA: 0x00010A07 File Offset: 0x0000FA07
		public string Name
		{
			get
			{
				return this._Name;
			}
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x00010A0F File Offset: 0x0000FA0F
		public ResolveEventArgs(string name)
		{
			this._Name = name;
		}

		// Token: 0x0400018E RID: 398
		private string _Name;
	}
}
