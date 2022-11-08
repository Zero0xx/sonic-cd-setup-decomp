using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace System.Net
{
	// Token: 0x02000404 RID: 1028
	internal struct SecureCredential
	{
		// Token: 0x060020AB RID: 8363 RVA: 0x00080C60 File Offset: 0x0007FC60
		public SecureCredential(int version, X509Certificate certificate, SecureCredential.Flags flags, SchProtocols protocols)
		{
			this.rootStore = (this.phMappers = (this.palgSupportedAlgs = (this.certContextArray = IntPtr.Zero)));
			this.cCreds = (this.cMappers = (this.cSupportedAlgs = 0));
			this.dwMinimumCipherStrength = (this.dwMaximumCipherStrength = 0);
			this.dwSessionLifespan = (this.reserved = 0);
			this.version = version;
			this.dwFlags = flags;
			this.grbitEnabledProtocols = protocols;
			if (certificate != null)
			{
				this.certContextArray = certificate.Handle;
				this.cCreds = 1;
			}
		}

		// Token: 0x060020AC RID: 8364 RVA: 0x00080CFE File Offset: 0x0007FCFE
		[Conditional("TRAVE")]
		internal void DebugDump()
		{
		}

		// Token: 0x0400206F RID: 8303
		public const int CurrentVersion = 4;

		// Token: 0x04002070 RID: 8304
		public int version;

		// Token: 0x04002071 RID: 8305
		public int cCreds;

		// Token: 0x04002072 RID: 8306
		public IntPtr certContextArray;

		// Token: 0x04002073 RID: 8307
		private readonly IntPtr rootStore;

		// Token: 0x04002074 RID: 8308
		public int cMappers;

		// Token: 0x04002075 RID: 8309
		private readonly IntPtr phMappers;

		// Token: 0x04002076 RID: 8310
		public int cSupportedAlgs;

		// Token: 0x04002077 RID: 8311
		private readonly IntPtr palgSupportedAlgs;

		// Token: 0x04002078 RID: 8312
		public SchProtocols grbitEnabledProtocols;

		// Token: 0x04002079 RID: 8313
		public int dwMinimumCipherStrength;

		// Token: 0x0400207A RID: 8314
		public int dwMaximumCipherStrength;

		// Token: 0x0400207B RID: 8315
		public int dwSessionLifespan;

		// Token: 0x0400207C RID: 8316
		public SecureCredential.Flags dwFlags;

		// Token: 0x0400207D RID: 8317
		public int reserved;

		// Token: 0x02000405 RID: 1029
		[Flags]
		public enum Flags
		{
			// Token: 0x0400207F RID: 8319
			Zero = 0,
			// Token: 0x04002080 RID: 8320
			NoSystemMapper = 2,
			// Token: 0x04002081 RID: 8321
			NoNameCheck = 4,
			// Token: 0x04002082 RID: 8322
			ValidateManual = 8,
			// Token: 0x04002083 RID: 8323
			NoDefaultCred = 16,
			// Token: 0x04002084 RID: 8324
			ValidateAuto = 32,
			// Token: 0x04002085 RID: 8325
			SendAuxRecord = 2097152,
			// Token: 0x04002086 RID: 8326
			UseStrongCrypto = 4194304
		}
	}
}
