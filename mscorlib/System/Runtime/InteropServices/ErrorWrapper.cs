using System;
using System.Security.Permissions;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000520 RID: 1312
	[ComVisible(true)]
	[Serializable]
	public sealed class ErrorWrapper
	{
		// Token: 0x060032DB RID: 13019 RVA: 0x000ABB9B File Offset: 0x000AAB9B
		public ErrorWrapper(int errorCode)
		{
			this.m_ErrorCode = errorCode;
		}

		// Token: 0x060032DC RID: 13020 RVA: 0x000ABBAA File Offset: 0x000AABAA
		public ErrorWrapper(object errorCode)
		{
			if (!(errorCode is int))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeInt32"), "errorCode");
			}
			this.m_ErrorCode = (int)errorCode;
		}

		// Token: 0x060032DD RID: 13021 RVA: 0x000ABBDB File Offset: 0x000AABDB
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public ErrorWrapper(Exception e)
		{
			this.m_ErrorCode = Marshal.GetHRForException(e);
		}

		// Token: 0x170008E0 RID: 2272
		// (get) Token: 0x060032DE RID: 13022 RVA: 0x000ABBEF File Offset: 0x000AABEF
		public int ErrorCode
		{
			get
			{
				return this.m_ErrorCode;
			}
		}

		// Token: 0x040019FC RID: 6652
		private int m_ErrorCode;
	}
}
