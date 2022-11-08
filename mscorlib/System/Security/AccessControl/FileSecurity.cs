using System;
using System.IO;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;

namespace System.Security.AccessControl
{
	// Token: 0x020008F7 RID: 2295
	public sealed class FileSecurity : FileSystemSecurity
	{
		// Token: 0x06005346 RID: 21318 RVA: 0x0012C187 File Offset: 0x0012B187
		public FileSecurity() : base(false)
		{
		}

		// Token: 0x06005347 RID: 21319 RVA: 0x0012C190 File Offset: 0x0012B190
		[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
		public FileSecurity(string fileName, AccessControlSections includeSections) : base(false, fileName, includeSections, false)
		{
			string fullPathInternal = Path.GetFullPathInternal(fileName);
			new FileIOPermission(FileIOPermissionAccess.NoAccess, AccessControlActions.View, fullPathInternal).Demand();
		}

		// Token: 0x06005348 RID: 21320 RVA: 0x0012C1BB File Offset: 0x0012B1BB
		[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
		internal FileSecurity(SafeFileHandle handle, string fullPath, AccessControlSections includeSections) : base(false, handle, includeSections, false)
		{
			if (fullPath != null)
			{
				new FileIOPermission(FileIOPermissionAccess.NoAccess, AccessControlActions.View, fullPath).Demand();
				return;
			}
			new FileIOPermission(PermissionState.Unrestricted).Demand();
		}
	}
}
