using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000201 RID: 513
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("6eaf5ace-7917-4f3c-b129-e046a9704766")]
	[ComImport]
	internal interface IReferenceIdentity
	{
		// Token: 0x06001565 RID: 5477
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string GetAttribute([MarshalAs(UnmanagedType.LPWStr)] [In] string Namespace, [MarshalAs(UnmanagedType.LPWStr)] [In] string Name);

		// Token: 0x06001566 RID: 5478
		void SetAttribute([MarshalAs(UnmanagedType.LPWStr)] [In] string Namespace, [MarshalAs(UnmanagedType.LPWStr)] [In] string Name, [MarshalAs(UnmanagedType.LPWStr)] [In] string Value);

		// Token: 0x06001567 RID: 5479
		IEnumIDENTITY_ATTRIBUTE EnumAttributes();

		// Token: 0x06001568 RID: 5480
		IReferenceIdentity Clone([In] IntPtr cDeltas, [MarshalAs(UnmanagedType.LPArray)] [In] IDENTITY_ATTRIBUTE[] Deltas);
	}
}
