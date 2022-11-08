using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000881 RID: 2177
	[ComVisible(true)]
	public class DSASignatureFormatter : AsymmetricSignatureFormatter
	{
		// Token: 0x06004F57 RID: 20311 RVA: 0x0011421A File Offset: 0x0011321A
		public DSASignatureFormatter()
		{
			this._oid = CryptoConfig.MapNameToOID("SHA1");
		}

		// Token: 0x06004F58 RID: 20312 RVA: 0x00114232 File Offset: 0x00113232
		public DSASignatureFormatter(AsymmetricAlgorithm key) : this()
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._dsaKey = (DSA)key;
		}

		// Token: 0x06004F59 RID: 20313 RVA: 0x00114254 File Offset: 0x00113254
		public override void SetKey(AsymmetricAlgorithm key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._dsaKey = (DSA)key;
		}

		// Token: 0x06004F5A RID: 20314 RVA: 0x00114270 File Offset: 0x00113270
		public override void SetHashAlgorithm(string strName)
		{
			if (CryptoConfig.MapNameToOID(strName) != this._oid)
			{
				throw new CryptographicUnexpectedOperationException(Environment.GetResourceString("Cryptography_InvalidOperation"));
			}
		}

		// Token: 0x06004F5B RID: 20315 RVA: 0x00114298 File Offset: 0x00113298
		public override byte[] CreateSignature(byte[] rgbHash)
		{
			if (this._oid == null)
			{
				throw new CryptographicUnexpectedOperationException(Environment.GetResourceString("Cryptography_MissingOID"));
			}
			if (this._dsaKey == null)
			{
				throw new CryptographicUnexpectedOperationException(Environment.GetResourceString("Cryptography_MissingKey"));
			}
			if (rgbHash == null)
			{
				throw new ArgumentNullException("rgbHash");
			}
			return this._dsaKey.CreateSignature(rgbHash);
		}

		// Token: 0x040028F8 RID: 10488
		private DSA _dsaKey;

		// Token: 0x040028F9 RID: 10489
		private string _oid;
	}
}
