using System;
using System.Security.Permissions;

namespace System.Configuration
{
	// Token: 0x0200071B RID: 1819
	internal static class TypeUtil
	{
		// Token: 0x0600379F RID: 14239 RVA: 0x000EBBD8 File Offset: 0x000EABD8
		[ReflectionPermission(SecurityAction.Assert, Flags = (ReflectionPermissionFlag.TypeInformation | ReflectionPermissionFlag.MemberAccess))]
		internal static object CreateInstanceWithReflectionPermission(string typeString)
		{
			Type type = Type.GetType(typeString, true);
			return Activator.CreateInstance(type, true);
		}
	}
}
