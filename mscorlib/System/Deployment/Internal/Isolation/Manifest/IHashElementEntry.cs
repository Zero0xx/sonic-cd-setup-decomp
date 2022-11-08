using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001AB RID: 427
	[Guid("9D46FB70-7B54-4f4f-9331-BA9E87833FF5")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IHashElementEntry
	{
		// Token: 0x1700025B RID: 603
		// (get) Token: 0x0600146C RID: 5228
		HashElementEntry AllData { get; }

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x0600146D RID: 5229
		uint index { get; }

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x0600146E RID: 5230
		byte Transform { get; }

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x0600146F RID: 5231
		object TransformMetadata { [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06001470 RID: 5232
		byte DigestMethod { get; }

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06001471 RID: 5233
		object DigestValue { [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06001472 RID: 5234
		string Xml { [return: MarshalAs(UnmanagedType.LPWStr)] get; }
	}
}
