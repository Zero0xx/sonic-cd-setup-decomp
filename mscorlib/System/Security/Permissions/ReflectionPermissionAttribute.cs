using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000640 RID: 1600
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class ReflectionPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x060039A1 RID: 14753 RVA: 0x000C2317 File Offset: 0x000C1317
		public ReflectionPermissionAttribute(SecurityAction action) : base(action)
		{
		}

		// Token: 0x170009AB RID: 2475
		// (get) Token: 0x060039A2 RID: 14754 RVA: 0x000C2320 File Offset: 0x000C1320
		// (set) Token: 0x060039A3 RID: 14755 RVA: 0x000C2328 File Offset: 0x000C1328
		public ReflectionPermissionFlag Flags
		{
			get
			{
				return this.m_flag;
			}
			set
			{
				this.m_flag = value;
			}
		}

		// Token: 0x170009AC RID: 2476
		// (get) Token: 0x060039A4 RID: 14756 RVA: 0x000C2331 File Offset: 0x000C1331
		// (set) Token: 0x060039A5 RID: 14757 RVA: 0x000C2341 File Offset: 0x000C1341
		[Obsolete("This API has been deprecated. http://go.microsoft.com/fwlink/?linkid=14202")]
		public bool TypeInformation
		{
			get
			{
				return (this.m_flag & ReflectionPermissionFlag.TypeInformation) != ReflectionPermissionFlag.NoFlags;
			}
			set
			{
				this.m_flag = (value ? (this.m_flag | ReflectionPermissionFlag.TypeInformation) : (this.m_flag & ~ReflectionPermissionFlag.TypeInformation));
			}
		}

		// Token: 0x170009AD RID: 2477
		// (get) Token: 0x060039A6 RID: 14758 RVA: 0x000C235F File Offset: 0x000C135F
		// (set) Token: 0x060039A7 RID: 14759 RVA: 0x000C236F File Offset: 0x000C136F
		public bool MemberAccess
		{
			get
			{
				return (this.m_flag & ReflectionPermissionFlag.MemberAccess) != ReflectionPermissionFlag.NoFlags;
			}
			set
			{
				this.m_flag = (value ? (this.m_flag | ReflectionPermissionFlag.MemberAccess) : (this.m_flag & ~ReflectionPermissionFlag.MemberAccess));
			}
		}

		// Token: 0x170009AE RID: 2478
		// (get) Token: 0x060039A8 RID: 14760 RVA: 0x000C238D File Offset: 0x000C138D
		// (set) Token: 0x060039A9 RID: 14761 RVA: 0x000C239D File Offset: 0x000C139D
		public bool ReflectionEmit
		{
			get
			{
				return (this.m_flag & ReflectionPermissionFlag.ReflectionEmit) != ReflectionPermissionFlag.NoFlags;
			}
			set
			{
				this.m_flag = (value ? (this.m_flag | ReflectionPermissionFlag.ReflectionEmit) : (this.m_flag & ~ReflectionPermissionFlag.ReflectionEmit));
			}
		}

		// Token: 0x170009AF RID: 2479
		// (get) Token: 0x060039AA RID: 14762 RVA: 0x000C23BB File Offset: 0x000C13BB
		// (set) Token: 0x060039AB RID: 14763 RVA: 0x000C23CB File Offset: 0x000C13CB
		public bool RestrictedMemberAccess
		{
			get
			{
				return (this.m_flag & ReflectionPermissionFlag.RestrictedMemberAccess) != ReflectionPermissionFlag.NoFlags;
			}
			set
			{
				this.m_flag = (value ? (this.m_flag | ReflectionPermissionFlag.RestrictedMemberAccess) : (this.m_flag & ~ReflectionPermissionFlag.RestrictedMemberAccess));
			}
		}

		// Token: 0x060039AC RID: 14764 RVA: 0x000C23E9 File Offset: 0x000C13E9
		public override IPermission CreatePermission()
		{
			if (this.m_unrestricted)
			{
				return new ReflectionPermission(PermissionState.Unrestricted);
			}
			return new ReflectionPermission(this.m_flag);
		}

		// Token: 0x04001E0C RID: 7692
		private ReflectionPermissionFlag m_flag;
	}
}
