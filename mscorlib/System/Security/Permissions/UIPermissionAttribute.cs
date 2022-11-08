using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000643 RID: 1603
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class UIPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x060039DD RID: 14813 RVA: 0x000C284E File Offset: 0x000C184E
		public UIPermissionAttribute(SecurityAction action) : base(action)
		{
		}

		// Token: 0x170009C6 RID: 2502
		// (get) Token: 0x060039DE RID: 14814 RVA: 0x000C2857 File Offset: 0x000C1857
		// (set) Token: 0x060039DF RID: 14815 RVA: 0x000C285F File Offset: 0x000C185F
		public UIPermissionWindow Window
		{
			get
			{
				return this.m_windowFlag;
			}
			set
			{
				this.m_windowFlag = value;
			}
		}

		// Token: 0x170009C7 RID: 2503
		// (get) Token: 0x060039E0 RID: 14816 RVA: 0x000C2868 File Offset: 0x000C1868
		// (set) Token: 0x060039E1 RID: 14817 RVA: 0x000C2870 File Offset: 0x000C1870
		public UIPermissionClipboard Clipboard
		{
			get
			{
				return this.m_clipboardFlag;
			}
			set
			{
				this.m_clipboardFlag = value;
			}
		}

		// Token: 0x060039E2 RID: 14818 RVA: 0x000C2879 File Offset: 0x000C1879
		public override IPermission CreatePermission()
		{
			if (this.m_unrestricted)
			{
				return new UIPermission(PermissionState.Unrestricted);
			}
			return new UIPermission(this.m_windowFlag, this.m_clipboardFlag);
		}

		// Token: 0x04001E13 RID: 7699
		private UIPermissionWindow m_windowFlag;

		// Token: 0x04001E14 RID: 7700
		private UIPermissionClipboard m_clipboardFlag;
	}
}
