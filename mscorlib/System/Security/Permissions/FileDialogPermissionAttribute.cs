using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x0200063C RID: 1596
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class FileDialogPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x0600396F RID: 14703 RVA: 0x000C1F5A File Offset: 0x000C0F5A
		public FileDialogPermissionAttribute(SecurityAction action) : base(action)
		{
		}

		// Token: 0x17000996 RID: 2454
		// (get) Token: 0x06003970 RID: 14704 RVA: 0x000C1F63 File Offset: 0x000C0F63
		// (set) Token: 0x06003971 RID: 14705 RVA: 0x000C1F73 File Offset: 0x000C0F73
		public bool Open
		{
			get
			{
				return (this.m_access & FileDialogPermissionAccess.Open) != FileDialogPermissionAccess.None;
			}
			set
			{
				this.m_access = (value ? (this.m_access | FileDialogPermissionAccess.Open) : (this.m_access & ~FileDialogPermissionAccess.Open));
			}
		}

		// Token: 0x17000997 RID: 2455
		// (get) Token: 0x06003972 RID: 14706 RVA: 0x000C1F91 File Offset: 0x000C0F91
		// (set) Token: 0x06003973 RID: 14707 RVA: 0x000C1FA1 File Offset: 0x000C0FA1
		public bool Save
		{
			get
			{
				return (this.m_access & FileDialogPermissionAccess.Save) != FileDialogPermissionAccess.None;
			}
			set
			{
				this.m_access = (value ? (this.m_access | FileDialogPermissionAccess.Save) : (this.m_access & ~FileDialogPermissionAccess.Save));
			}
		}

		// Token: 0x06003974 RID: 14708 RVA: 0x000C1FBF File Offset: 0x000C0FBF
		public override IPermission CreatePermission()
		{
			if (this.m_unrestricted)
			{
				return new FileDialogPermission(PermissionState.Unrestricted);
			}
			return new FileDialogPermission(this.m_access);
		}

		// Token: 0x04001DFA RID: 7674
		private FileDialogPermissionAccess m_access;
	}
}
