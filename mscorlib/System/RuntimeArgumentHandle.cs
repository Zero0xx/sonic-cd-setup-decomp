using System;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x02000108 RID: 264
	[ComVisible(true)]
	public struct RuntimeArgumentHandle
	{
		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000EDE RID: 3806 RVA: 0x0002C4FF File Offset: 0x0002B4FF
		internal IntPtr Value
		{
			get
			{
				return this.m_ptr;
			}
		}

		// Token: 0x04000534 RID: 1332
		private IntPtr m_ptr;
	}
}
