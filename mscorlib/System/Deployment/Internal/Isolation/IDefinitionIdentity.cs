using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000202 RID: 514
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("587bf538-4d90-4a3c-9ef1-58a200a8a9e7")]
	[ComImport]
	internal interface IDefinitionIdentity
	{
		// Token: 0x06001569 RID: 5481
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string GetAttribute([MarshalAs(UnmanagedType.LPWStr)] [In] string Namespace, [MarshalAs(UnmanagedType.LPWStr)] [In] string Name);

		// Token: 0x0600156A RID: 5482
		void SetAttribute([MarshalAs(UnmanagedType.LPWStr)] [In] string Namespace, [MarshalAs(UnmanagedType.LPWStr)] [In] string Name, [MarshalAs(UnmanagedType.LPWStr)] [In] string Value);

		// Token: 0x0600156B RID: 5483
		IEnumIDENTITY_ATTRIBUTE EnumAttributes();

		// Token: 0x0600156C RID: 5484
		IDefinitionIdentity Clone([In] IntPtr cDeltas, [MarshalAs(UnmanagedType.LPArray)] [In] IDENTITY_ATTRIBUTE[] Deltas);
	}
}
