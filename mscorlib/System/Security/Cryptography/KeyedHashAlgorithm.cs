using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000883 RID: 2179
	[ComVisible(true)]
	public abstract class KeyedHashAlgorithm : HashAlgorithm
	{
		// Token: 0x06004F71 RID: 20337 RVA: 0x001146A2 File Offset: 0x001136A2
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.KeyValue != null)
				{
					Array.Clear(this.KeyValue, 0, this.KeyValue.Length);
				}
				this.KeyValue = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x17000DD5 RID: 3541
		// (get) Token: 0x06004F72 RID: 20338 RVA: 0x001146D1 File Offset: 0x001136D1
		// (set) Token: 0x06004F73 RID: 20339 RVA: 0x001146E3 File Offset: 0x001136E3
		public virtual byte[] Key
		{
			get
			{
				return (byte[])this.KeyValue.Clone();
			}
			set
			{
				if (this.State != 0)
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_HashKeySet"));
				}
				this.KeyValue = (byte[])value.Clone();
			}
		}

		// Token: 0x06004F74 RID: 20340 RVA: 0x0011470E File Offset: 0x0011370E
		public new static KeyedHashAlgorithm Create()
		{
			return KeyedHashAlgorithm.Create("System.Security.Cryptography.KeyedHashAlgorithm");
		}

		// Token: 0x06004F75 RID: 20341 RVA: 0x0011471A File Offset: 0x0011371A
		public new static KeyedHashAlgorithm Create(string algName)
		{
			return (KeyedHashAlgorithm)CryptoConfig.CreateFromName(algName);
		}

		// Token: 0x040028FE RID: 10494
		protected byte[] KeyValue;
	}
}
