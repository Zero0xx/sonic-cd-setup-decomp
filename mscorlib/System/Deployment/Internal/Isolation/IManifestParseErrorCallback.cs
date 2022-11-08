using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000241 RID: 577
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("ace1b703-1aac-4956-ab87-90cac8b93ce6")]
	[ComImport]
	internal interface IManifestParseErrorCallback
	{
		// Token: 0x0600160B RID: 5643
		void OnError([In] uint StartLine, [In] uint nStartColumn, [In] uint cCharacterCount, [In] int hr, [MarshalAs(UnmanagedType.LPWStr)] [In] string ErrorStatusHostFile, [In] uint ParameterCount, [MarshalAs(UnmanagedType.LPArray)] [In] string[] Parameters);
	}
}
