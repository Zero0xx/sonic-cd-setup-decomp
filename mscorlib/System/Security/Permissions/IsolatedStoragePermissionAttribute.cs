using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000649 RID: 1609
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public abstract class IsolatedStoragePermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x060039FF RID: 14847 RVA: 0x000C2B02 File Offset: 0x000C1B02
		protected IsolatedStoragePermissionAttribute(SecurityAction action) : base(action)
		{
		}

		// Token: 0x170009D1 RID: 2513
		// (get) Token: 0x06003A01 RID: 14849 RVA: 0x000C2B14 File Offset: 0x000C1B14
		// (set) Token: 0x06003A00 RID: 14848 RVA: 0x000C2B0B File Offset: 0x000C1B0B
		public long UserQuota
		{
			get
			{
				return this.m_userQuota;
			}
			set
			{
				this.m_userQuota = value;
			}
		}

		// Token: 0x170009D2 RID: 2514
		// (get) Token: 0x06003A03 RID: 14851 RVA: 0x000C2B25 File Offset: 0x000C1B25
		// (set) Token: 0x06003A02 RID: 14850 RVA: 0x000C2B1C File Offset: 0x000C1B1C
		public IsolatedStorageContainment UsageAllowed
		{
			get
			{
				return this.m_allowed;
			}
			set
			{
				this.m_allowed = value;
			}
		}

		// Token: 0x04001E1E RID: 7710
		internal long m_userQuota;

		// Token: 0x04001E1F RID: 7711
		internal IsolatedStorageContainment m_allowed;
	}
}
