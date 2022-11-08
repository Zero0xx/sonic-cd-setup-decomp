using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000880 RID: 2176
	[ComVisible(true)]
	public class DSASignatureDeformatter : AsymmetricSignatureDeformatter
	{
		// Token: 0x06004F52 RID: 20306 RVA: 0x00114151 File Offset: 0x00113151
		public DSASignatureDeformatter()
		{
			this._oid = CryptoConfig.MapNameToOID("SHA1");
		}

		// Token: 0x06004F53 RID: 20307 RVA: 0x00114169 File Offset: 0x00113169
		public DSASignatureDeformatter(AsymmetricAlgorithm key) : this()
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._dsaKey = (DSA)key;
		}

		// Token: 0x06004F54 RID: 20308 RVA: 0x0011418B File Offset: 0x0011318B
		public override void SetKey(AsymmetricAlgorithm key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._dsaKey = (DSA)key;
		}

		// Token: 0x06004F55 RID: 20309 RVA: 0x001141A7 File Offset: 0x001131A7
		public override void SetHashAlgorithm(string strName)
		{
			if (CryptoConfig.MapNameToOID(strName) != this._oid)
			{
				throw new CryptographicUnexpectedOperationException(Environment.GetResourceString("Cryptography_InvalidOperation"));
			}
		}

		// Token: 0x06004F56 RID: 20310 RVA: 0x001141CC File Offset: 0x001131CC
		public override bool VerifySignature(byte[] rgbHash, byte[] rgbSignature)
		{
			if (this._dsaKey == null)
			{
				throw new CryptographicUnexpectedOperationException(Environment.GetResourceString("Cryptography_MissingKey"));
			}
			if (rgbHash == null)
			{
				throw new ArgumentNullException("rgbHash");
			}
			if (rgbSignature == null)
			{
				throw new ArgumentNullException("rgbSignature");
			}
			return this._dsaKey.VerifySignature(rgbHash, rgbSignature);
		}

		// Token: 0x040028F6 RID: 10486
		private DSA _dsaKey;

		// Token: 0x040028F7 RID: 10487
		private string _oid;
	}
}
