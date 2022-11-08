using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000510 RID: 1296
	[ComVisible(true)]
	public struct HandleRef
	{
		// Token: 0x060031D9 RID: 12761 RVA: 0x000AA259 File Offset: 0x000A9259
		public HandleRef(object wrapper, IntPtr handle)
		{
			this.m_wrapper = wrapper;
			this.m_handle = handle;
		}

		// Token: 0x170008D9 RID: 2265
		// (get) Token: 0x060031DA RID: 12762 RVA: 0x000AA269 File Offset: 0x000A9269
		public object Wrapper
		{
			get
			{
				return this.m_wrapper;
			}
		}

		// Token: 0x170008DA RID: 2266
		// (get) Token: 0x060031DB RID: 12763 RVA: 0x000AA271 File Offset: 0x000A9271
		public IntPtr Handle
		{
			get
			{
				return this.m_handle;
			}
		}

		// Token: 0x060031DC RID: 12764 RVA: 0x000AA279 File Offset: 0x000A9279
		public static explicit operator IntPtr(HandleRef value)
		{
			return value.m_handle;
		}

		// Token: 0x060031DD RID: 12765 RVA: 0x000AA282 File Offset: 0x000A9282
		public static IntPtr ToIntPtr(HandleRef value)
		{
			return value.m_handle;
		}

		// Token: 0x040019CC RID: 6604
		internal object m_wrapper;

		// Token: 0x040019CD RID: 6605
		internal IntPtr m_handle;
	}
}
