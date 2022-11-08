using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x020008AB RID: 2219
	[ComVisible(true)]
	public sealed class SHA1CryptoServiceProvider : SHA1
	{
		// Token: 0x0600509B RID: 20635 RVA: 0x0011FE00 File Offset: 0x0011EE00
		public SHA1CryptoServiceProvider()
		{
			SafeHashHandle invalidHandle = SafeHashHandle.InvalidHandle;
			Utils._CreateHash(Utils.StaticProvHandle, 32772, ref invalidHandle);
			this._safeHashHandle = invalidHandle;
		}

		// Token: 0x0600509C RID: 20636 RVA: 0x0011FE31 File Offset: 0x0011EE31
		protected override void Dispose(bool disposing)
		{
			if (this._safeHashHandle != null && !this._safeHashHandle.IsClosed)
			{
				this._safeHashHandle.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600509D RID: 20637 RVA: 0x0011FE5C File Offset: 0x0011EE5C
		public override void Initialize()
		{
			if (this._safeHashHandle != null && !this._safeHashHandle.IsClosed)
			{
				this._safeHashHandle.Dispose();
			}
			SafeHashHandle invalidHandle = SafeHashHandle.InvalidHandle;
			Utils._CreateHash(Utils.StaticProvHandle, 32772, ref invalidHandle);
			this._safeHashHandle = invalidHandle;
		}

		// Token: 0x0600509E RID: 20638 RVA: 0x0011FEA7 File Offset: 0x0011EEA7
		protected override void HashCore(byte[] rgb, int ibStart, int cbSize)
		{
			Utils._HashData(this._safeHashHandle, rgb, ibStart, cbSize);
		}

		// Token: 0x0600509F RID: 20639 RVA: 0x0011FEB7 File Offset: 0x0011EEB7
		protected override byte[] HashFinal()
		{
			return Utils._EndHash(this._safeHashHandle);
		}

		// Token: 0x04002975 RID: 10613
		private SafeHashHandle _safeHashHandle;
	}
}
