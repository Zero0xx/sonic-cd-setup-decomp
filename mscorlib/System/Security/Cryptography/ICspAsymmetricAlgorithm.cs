using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x0200087E RID: 2174
	[ComVisible(true)]
	public interface ICspAsymmetricAlgorithm
	{
		// Token: 0x17000DC7 RID: 3527
		// (get) Token: 0x06004F30 RID: 20272
		CspKeyContainerInfo CspKeyContainerInfo { get; }

		// Token: 0x06004F31 RID: 20273
		byte[] ExportCspBlob(bool includePrivateParameters);

		// Token: 0x06004F32 RID: 20274
		void ImportCspBlob(byte[] rawData);
	}
}
