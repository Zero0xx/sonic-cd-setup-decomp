using System;
using System.IO;
using System.Security.Permissions;

namespace System.Security.AccessControl
{
	// Token: 0x020008F8 RID: 2296
	public sealed class DirectorySecurity : FileSystemSecurity
	{
		// Token: 0x06005349 RID: 21321 RVA: 0x0012C1E3 File Offset: 0x0012B1E3
		public DirectorySecurity() : base(true)
		{
		}

		// Token: 0x0600534A RID: 21322 RVA: 0x0012C1EC File Offset: 0x0012B1EC
		[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
		public DirectorySecurity(string name, AccessControlSections includeSections) : base(true, name, includeSections, true)
		{
			string fullPathInternal = Path.GetFullPathInternal(name);
			new FileIOPermission(FileIOPermissionAccess.NoAccess, AccessControlActions.View, fullPathInternal).Demand();
		}
	}
}
