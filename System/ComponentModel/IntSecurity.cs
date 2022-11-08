using System;
using System.IO;
using System.Security;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020000FD RID: 253
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	internal static class IntSecurity
	{
		// Token: 0x0600081D RID: 2077 RVA: 0x0001C12C File Offset: 0x0001B12C
		public static string UnsafeGetFullPath(string fileName)
		{
			string result = fileName;
			new FileIOPermission(PermissionState.None)
			{
				AllFiles = FileIOPermissionAccess.PathDiscovery
			}.Assert();
			try
			{
				result = Path.GetFullPath(fileName);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return result;
		}

		// Token: 0x04000982 RID: 2434
		public static readonly CodeAccessPermission UnmanagedCode = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);

		// Token: 0x04000983 RID: 2435
		public static readonly CodeAccessPermission FullReflection = new ReflectionPermission(PermissionState.Unrestricted);
	}
}
