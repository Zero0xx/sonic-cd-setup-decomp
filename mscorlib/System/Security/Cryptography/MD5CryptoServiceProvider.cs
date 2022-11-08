using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000890 RID: 2192
	[ComVisible(true)]
	public sealed class MD5CryptoServiceProvider : MD5
	{
		// Token: 0x06004FC3 RID: 20419 RVA: 0x001156F4 File Offset: 0x001146F4
		public MD5CryptoServiceProvider()
		{
			if (Utils.FipsAlgorithmPolicy == 1)
			{
				throw new InvalidOperationException(Environment.GetResourceString("Cryptography_NonCompliantFIPSAlgorithm"));
			}
			SafeHashHandle invalidHandle = SafeHashHandle.InvalidHandle;
			Utils._CreateHash(Utils.StaticProvHandle, 32771, ref invalidHandle);
			this._safeHashHandle = invalidHandle;
		}

		// Token: 0x06004FC4 RID: 20420 RVA: 0x0011573D File Offset: 0x0011473D
		protected override void Dispose(bool disposing)
		{
			if (this._safeHashHandle != null && !this._safeHashHandle.IsClosed)
			{
				this._safeHashHandle.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06004FC5 RID: 20421 RVA: 0x00115768 File Offset: 0x00114768
		public override void Initialize()
		{
			if (this._safeHashHandle != null && !this._safeHashHandle.IsClosed)
			{
				this._safeHashHandle.Dispose();
			}
			SafeHashHandle invalidHandle = SafeHashHandle.InvalidHandle;
			Utils._CreateHash(Utils.StaticProvHandle, 32771, ref invalidHandle);
			this._safeHashHandle = invalidHandle;
		}

		// Token: 0x06004FC6 RID: 20422 RVA: 0x001157B3 File Offset: 0x001147B3
		protected override void HashCore(byte[] rgb, int ibStart, int cbSize)
		{
			Utils._HashData(this._safeHashHandle, rgb, ibStart, cbSize);
		}

		// Token: 0x06004FC7 RID: 20423 RVA: 0x001157C3 File Offset: 0x001147C3
		protected override byte[] HashFinal()
		{
			return Utils._EndHash(this._safeHashHandle);
		}

		// Token: 0x04002917 RID: 10519
		private SafeHashHandle _safeHashHandle;
	}
}
