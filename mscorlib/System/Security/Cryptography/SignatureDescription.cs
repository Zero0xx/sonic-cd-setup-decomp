using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x020008B3 RID: 2227
	[ComVisible(true)]
	public class SignatureDescription
	{
		// Token: 0x060050E5 RID: 20709 RVA: 0x00122311 File Offset: 0x00121311
		public SignatureDescription()
		{
		}

		// Token: 0x060050E6 RID: 20710 RVA: 0x0012231C File Offset: 0x0012131C
		public SignatureDescription(SecurityElement el)
		{
			if (el == null)
			{
				throw new ArgumentNullException("el");
			}
			this._strKey = el.SearchForTextOfTag("Key");
			this._strDigest = el.SearchForTextOfTag("Digest");
			this._strFormatter = el.SearchForTextOfTag("Formatter");
			this._strDeformatter = el.SearchForTextOfTag("Deformatter");
		}

		// Token: 0x17000E13 RID: 3603
		// (get) Token: 0x060050E7 RID: 20711 RVA: 0x00122381 File Offset: 0x00121381
		// (set) Token: 0x060050E8 RID: 20712 RVA: 0x00122389 File Offset: 0x00121389
		public string KeyAlgorithm
		{
			get
			{
				return this._strKey;
			}
			set
			{
				this._strKey = value;
			}
		}

		// Token: 0x17000E14 RID: 3604
		// (get) Token: 0x060050E9 RID: 20713 RVA: 0x00122392 File Offset: 0x00121392
		// (set) Token: 0x060050EA RID: 20714 RVA: 0x0012239A File Offset: 0x0012139A
		public string DigestAlgorithm
		{
			get
			{
				return this._strDigest;
			}
			set
			{
				this._strDigest = value;
			}
		}

		// Token: 0x17000E15 RID: 3605
		// (get) Token: 0x060050EB RID: 20715 RVA: 0x001223A3 File Offset: 0x001213A3
		// (set) Token: 0x060050EC RID: 20716 RVA: 0x001223AB File Offset: 0x001213AB
		public string FormatterAlgorithm
		{
			get
			{
				return this._strFormatter;
			}
			set
			{
				this._strFormatter = value;
			}
		}

		// Token: 0x17000E16 RID: 3606
		// (get) Token: 0x060050ED RID: 20717 RVA: 0x001223B4 File Offset: 0x001213B4
		// (set) Token: 0x060050EE RID: 20718 RVA: 0x001223BC File Offset: 0x001213BC
		public string DeformatterAlgorithm
		{
			get
			{
				return this._strDeformatter;
			}
			set
			{
				this._strDeformatter = value;
			}
		}

		// Token: 0x060050EF RID: 20719 RVA: 0x001223C8 File Offset: 0x001213C8
		public virtual AsymmetricSignatureDeformatter CreateDeformatter(AsymmetricAlgorithm key)
		{
			AsymmetricSignatureDeformatter asymmetricSignatureDeformatter = (AsymmetricSignatureDeformatter)CryptoConfig.CreateFromName(this._strDeformatter);
			asymmetricSignatureDeformatter.SetKey(key);
			return asymmetricSignatureDeformatter;
		}

		// Token: 0x060050F0 RID: 20720 RVA: 0x001223F0 File Offset: 0x001213F0
		public virtual AsymmetricSignatureFormatter CreateFormatter(AsymmetricAlgorithm key)
		{
			AsymmetricSignatureFormatter asymmetricSignatureFormatter = (AsymmetricSignatureFormatter)CryptoConfig.CreateFromName(this._strFormatter);
			asymmetricSignatureFormatter.SetKey(key);
			return asymmetricSignatureFormatter;
		}

		// Token: 0x060050F1 RID: 20721 RVA: 0x00122416 File Offset: 0x00121416
		public virtual HashAlgorithm CreateDigest()
		{
			return (HashAlgorithm)CryptoConfig.CreateFromName(this._strDigest);
		}

		// Token: 0x04002989 RID: 10633
		private string _strKey;

		// Token: 0x0400298A RID: 10634
		private string _strDigest;

		// Token: 0x0400298B RID: 10635
		private string _strFormatter;

		// Token: 0x0400298C RID: 10636
		private string _strDeformatter;
	}
}
