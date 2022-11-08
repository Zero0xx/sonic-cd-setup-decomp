using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x020003FC RID: 1020
	internal struct SSL_EXTRA_CERT_CHAIN_POLICY_PARA
	{
		// Token: 0x060020A6 RID: 8358 RVA: 0x00080B9E File Offset: 0x0007FB9E
		internal SSL_EXTRA_CERT_CHAIN_POLICY_PARA(bool amIServer)
		{
			this.u.cbStruct = SSL_EXTRA_CERT_CHAIN_POLICY_PARA.StructSize;
			this.u.cbSize = SSL_EXTRA_CERT_CHAIN_POLICY_PARA.StructSize;
			this.dwAuthType = (amIServer ? 1 : 2);
			this.fdwChecks = 0U;
			this.pwszServerName = null;
		}

		// Token: 0x04002053 RID: 8275
		internal SSL_EXTRA_CERT_CHAIN_POLICY_PARA.U u;

		// Token: 0x04002054 RID: 8276
		internal int dwAuthType;

		// Token: 0x04002055 RID: 8277
		internal uint fdwChecks;

		// Token: 0x04002056 RID: 8278
		internal unsafe char* pwszServerName;

		// Token: 0x04002057 RID: 8279
		private static readonly uint StructSize = (uint)Marshal.SizeOf(typeof(SSL_EXTRA_CERT_CHAIN_POLICY_PARA));

		// Token: 0x020003FD RID: 1021
		[StructLayout(LayoutKind.Explicit)]
		internal struct U
		{
			// Token: 0x04002058 RID: 8280
			[FieldOffset(0)]
			internal uint cbStruct;

			// Token: 0x04002059 RID: 8281
			[FieldOffset(0)]
			internal uint cbSize;
		}
	}
}
