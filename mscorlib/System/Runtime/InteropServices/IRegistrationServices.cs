using System;
using System.Reflection;
using System.Security.Permissions;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000526 RID: 1318
	[Guid("CCBD682C-73A5-4568-B8B0-C7007E11ABA2")]
	[ComVisible(true)]
	public interface IRegistrationServices
	{
		// Token: 0x060032E7 RID: 13031
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		bool RegisterAssembly(Assembly assembly, AssemblyRegistrationFlags flags);

		// Token: 0x060032E8 RID: 13032
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		bool UnregisterAssembly(Assembly assembly);

		// Token: 0x060032E9 RID: 13033
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		Type[] GetRegistrableTypesInAssembly(Assembly assembly);

		// Token: 0x060032EA RID: 13034
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		string GetProgIdForType(Type type);

		// Token: 0x060032EB RID: 13035
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		void RegisterTypeForComClients(Type type, ref Guid g);

		// Token: 0x060032EC RID: 13036
		Guid GetManagedCategoryGuid();

		// Token: 0x060032ED RID: 13037
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		bool TypeRequiresRegistration(Type type);

		// Token: 0x060032EE RID: 13038
		bool TypeRepresentsComType(Type type);
	}
}
