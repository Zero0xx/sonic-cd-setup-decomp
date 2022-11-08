using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x0200087A RID: 2170
	[ComVisible(true)]
	public abstract class DeriveBytes
	{
		// Token: 0x06004F23 RID: 20259
		public abstract byte[] GetBytes(int cb);

		// Token: 0x06004F24 RID: 20260
		public abstract void Reset();
	}
}
