using System;

namespace System.Security.Cryptography
{
	// Token: 0x020008B5 RID: 2229
	internal class DSASignatureDescription : SignatureDescription
	{
		// Token: 0x060050F4 RID: 20724 RVA: 0x0012248D File Offset: 0x0012148D
		public DSASignatureDescription()
		{
			base.KeyAlgorithm = "System.Security.Cryptography.DSACryptoServiceProvider";
			base.DigestAlgorithm = "System.Security.Cryptography.SHA1CryptoServiceProvider";
			base.FormatterAlgorithm = "System.Security.Cryptography.DSASignatureFormatter";
			base.DeformatterAlgorithm = "System.Security.Cryptography.DSASignatureDeformatter";
		}
	}
}
