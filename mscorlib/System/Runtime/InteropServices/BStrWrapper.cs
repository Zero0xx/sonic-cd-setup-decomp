using System;
using System.Security.Permissions;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200051C RID: 1308
	[ComVisible(true)]
	[Serializable]
	public sealed class BStrWrapper
	{
		// Token: 0x060032D4 RID: 13012 RVA: 0x000ABB08 File Offset: 0x000AAB08
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public BStrWrapper(string value)
		{
			this.m_WrappedObject = value;
		}

		// Token: 0x170008DD RID: 2269
		// (get) Token: 0x060032D5 RID: 13013 RVA: 0x000ABB17 File Offset: 0x000AAB17
		public string WrappedObject
		{
			get
			{
				return this.m_WrappedObject;
			}
		}

		// Token: 0x040019F5 RID: 6645
		private string m_WrappedObject;
	}
}
