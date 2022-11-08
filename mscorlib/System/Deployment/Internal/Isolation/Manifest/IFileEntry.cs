using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001AE RID: 430
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("A2A55FAD-349B-469b-BF12-ADC33D14A937")]
	[ComImport]
	internal interface IFileEntry
	{
		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06001477 RID: 5239
		FileEntry AllData { get; }

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06001478 RID: 5240
		string Name { [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06001479 RID: 5241
		uint HashAlgorithm { get; }

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x0600147A RID: 5242
		string LoadFrom { [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x0600147B RID: 5243
		string SourcePath { [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x0600147C RID: 5244
		string ImportPath { [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x0600147D RID: 5245
		string SourceName { [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x0600147E RID: 5246
		string Location { [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x0600147F RID: 5247
		object HashValue { [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06001480 RID: 5248
		ulong Size { get; }

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06001481 RID: 5249
		string Group { [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06001482 RID: 5250
		uint Flags { get; }

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06001483 RID: 5251
		IMuiResourceMapEntry MuiMapping { get; }

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06001484 RID: 5252
		uint WritableType { get; }

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06001485 RID: 5253
		ISection HashElements { get; }
	}
}
