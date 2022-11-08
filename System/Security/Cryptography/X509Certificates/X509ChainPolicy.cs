using System;
using System.Globalization;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000337 RID: 823
	public sealed class X509ChainPolicy
	{
		// Token: 0x060019EE RID: 6638 RVA: 0x0005A553 File Offset: 0x00059553
		public X509ChainPolicy()
		{
			this.Reset();
		}

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x060019EF RID: 6639 RVA: 0x0005A561 File Offset: 0x00059561
		public OidCollection ApplicationPolicy
		{
			get
			{
				return this.m_applicationPolicy;
			}
		}

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x060019F0 RID: 6640 RVA: 0x0005A569 File Offset: 0x00059569
		public OidCollection CertificatePolicy
		{
			get
			{
				return this.m_certificatePolicy;
			}
		}

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x060019F1 RID: 6641 RVA: 0x0005A571 File Offset: 0x00059571
		// (set) Token: 0x060019F2 RID: 6642 RVA: 0x0005A57C File Offset: 0x0005957C
		public X509RevocationMode RevocationMode
		{
			get
			{
				return this.m_revocationMode;
			}
			set
			{
				if (value < X509RevocationMode.NoCheck || value > X509RevocationMode.Offline)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("Arg_EnumIllegalVal"), new object[]
					{
						"value"
					}));
				}
				this.m_revocationMode = value;
			}
		}

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x060019F3 RID: 6643 RVA: 0x0005A5C2 File Offset: 0x000595C2
		// (set) Token: 0x060019F4 RID: 6644 RVA: 0x0005A5CC File Offset: 0x000595CC
		public X509RevocationFlag RevocationFlag
		{
			get
			{
				return this.m_revocationFlag;
			}
			set
			{
				if (value < X509RevocationFlag.EndCertificateOnly || value > X509RevocationFlag.ExcludeRoot)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("Arg_EnumIllegalVal"), new object[]
					{
						"value"
					}));
				}
				this.m_revocationFlag = value;
			}
		}

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x060019F5 RID: 6645 RVA: 0x0005A612 File Offset: 0x00059612
		// (set) Token: 0x060019F6 RID: 6646 RVA: 0x0005A61C File Offset: 0x0005961C
		public X509VerificationFlags VerificationFlags
		{
			get
			{
				return this.m_verificationFlags;
			}
			set
			{
				if (value < X509VerificationFlags.NoFlag || value > X509VerificationFlags.AllFlags)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("Arg_EnumIllegalVal"), new object[]
					{
						"value"
					}));
				}
				this.m_verificationFlags = value;
			}
		}

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x060019F7 RID: 6647 RVA: 0x0005A666 File Offset: 0x00059666
		// (set) Token: 0x060019F8 RID: 6648 RVA: 0x0005A66E File Offset: 0x0005966E
		public DateTime VerificationTime
		{
			get
			{
				return this.m_verificationTime;
			}
			set
			{
				this.m_verificationTime = value;
			}
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x060019F9 RID: 6649 RVA: 0x0005A677 File Offset: 0x00059677
		// (set) Token: 0x060019FA RID: 6650 RVA: 0x0005A67F File Offset: 0x0005967F
		public TimeSpan UrlRetrievalTimeout
		{
			get
			{
				return this.m_timeout;
			}
			set
			{
				this.m_timeout = value;
			}
		}

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x060019FB RID: 6651 RVA: 0x0005A688 File Offset: 0x00059688
		public X509Certificate2Collection ExtraStore
		{
			get
			{
				return this.m_extraStore;
			}
		}

		// Token: 0x060019FC RID: 6652 RVA: 0x0005A690 File Offset: 0x00059690
		public void Reset()
		{
			this.m_applicationPolicy = new OidCollection();
			this.m_certificatePolicy = new OidCollection();
			this.m_revocationMode = X509RevocationMode.Online;
			this.m_revocationFlag = X509RevocationFlag.ExcludeRoot;
			this.m_verificationFlags = X509VerificationFlags.NoFlag;
			this.m_verificationTime = DateTime.Now;
			this.m_timeout = new TimeSpan(0, 0, 0);
			this.m_extraStore = new X509Certificate2Collection();
		}

		// Token: 0x04001AFC RID: 6908
		private OidCollection m_applicationPolicy;

		// Token: 0x04001AFD RID: 6909
		private OidCollection m_certificatePolicy;

		// Token: 0x04001AFE RID: 6910
		private X509RevocationMode m_revocationMode;

		// Token: 0x04001AFF RID: 6911
		private X509RevocationFlag m_revocationFlag;

		// Token: 0x04001B00 RID: 6912
		private DateTime m_verificationTime;

		// Token: 0x04001B01 RID: 6913
		private TimeSpan m_timeout;

		// Token: 0x04001B02 RID: 6914
		private X509Certificate2Collection m_extraStore;

		// Token: 0x04001B03 RID: 6915
		private X509VerificationFlags m_verificationFlags;
	}
}
