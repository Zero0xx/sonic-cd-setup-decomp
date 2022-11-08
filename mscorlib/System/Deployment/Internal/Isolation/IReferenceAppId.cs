using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000207 RID: 519
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("054f0bef-9e45-4363-8f5a-2f8e142d9a3b")]
	[ComImport]
	internal interface IReferenceAppId
	{
		// Token: 0x06001580 RID: 5504
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string get_SubscriptionId();

		// Token: 0x06001581 RID: 5505
		void put_SubscriptionId([MarshalAs(UnmanagedType.LPWStr)] [In] string Subscription);

		// Token: 0x06001582 RID: 5506
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string get_Codebase();

		// Token: 0x06001583 RID: 5507
		void put_Codebase([MarshalAs(UnmanagedType.LPWStr)] [In] string CodeBase);

		// Token: 0x06001584 RID: 5508
		IEnumReferenceIdentity EnumAppPath();
	}
}
