using System;
using System.Security.Permissions;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200051F RID: 1311
	[ComVisible(true)]
	[Serializable]
	public sealed class DispatchWrapper
	{
		// Token: 0x060032D9 RID: 13017 RVA: 0x000ABB68 File Offset: 0x000AAB68
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public DispatchWrapper(object obj)
		{
			if (obj != null)
			{
				IntPtr idispatchForObject = Marshal.GetIDispatchForObject(obj);
				Marshal.Release(idispatchForObject);
			}
			this.m_WrappedObject = obj;
		}

		// Token: 0x170008DF RID: 2271
		// (get) Token: 0x060032DA RID: 13018 RVA: 0x000ABB93 File Offset: 0x000AAB93
		public object WrappedObject
		{
			get
			{
				return this.m_WrappedObject;
			}
		}

		// Token: 0x040019FB RID: 6651
		private object m_WrappedObject;
	}
}
