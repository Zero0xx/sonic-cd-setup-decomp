using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Util;

namespace System.Security.Permissions
{
	// Token: 0x02000648 RID: 1608
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class PublisherIdentityPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x060039F7 RID: 14839 RVA: 0x000C2A3C File Offset: 0x000C1A3C
		public PublisherIdentityPermissionAttribute(SecurityAction action) : base(action)
		{
			this.m_x509cert = null;
			this.m_certFile = null;
			this.m_signedFile = null;
		}

		// Token: 0x170009CE RID: 2510
		// (get) Token: 0x060039F8 RID: 14840 RVA: 0x000C2A5A File Offset: 0x000C1A5A
		// (set) Token: 0x060039F9 RID: 14841 RVA: 0x000C2A62 File Offset: 0x000C1A62
		public string X509Certificate
		{
			get
			{
				return this.m_x509cert;
			}
			set
			{
				this.m_x509cert = value;
			}
		}

		// Token: 0x170009CF RID: 2511
		// (get) Token: 0x060039FA RID: 14842 RVA: 0x000C2A6B File Offset: 0x000C1A6B
		// (set) Token: 0x060039FB RID: 14843 RVA: 0x000C2A73 File Offset: 0x000C1A73
		public string CertFile
		{
			get
			{
				return this.m_certFile;
			}
			set
			{
				this.m_certFile = value;
			}
		}

		// Token: 0x170009D0 RID: 2512
		// (get) Token: 0x060039FC RID: 14844 RVA: 0x000C2A7C File Offset: 0x000C1A7C
		// (set) Token: 0x060039FD RID: 14845 RVA: 0x000C2A84 File Offset: 0x000C1A84
		public string SignedFile
		{
			get
			{
				return this.m_signedFile;
			}
			set
			{
				this.m_signedFile = value;
			}
		}

		// Token: 0x060039FE RID: 14846 RVA: 0x000C2A90 File Offset: 0x000C1A90
		public override IPermission CreatePermission()
		{
			if (this.m_unrestricted)
			{
				return new PublisherIdentityPermission(PermissionState.Unrestricted);
			}
			if (this.m_x509cert != null)
			{
				return new PublisherIdentityPermission(new X509Certificate(Hex.DecodeHexString(this.m_x509cert)));
			}
			if (this.m_certFile != null)
			{
				return new PublisherIdentityPermission(System.Security.Cryptography.X509Certificates.X509Certificate.CreateFromCertFile(this.m_certFile));
			}
			if (this.m_signedFile != null)
			{
				return new PublisherIdentityPermission(System.Security.Cryptography.X509Certificates.X509Certificate.CreateFromSignedFile(this.m_signedFile));
			}
			return new PublisherIdentityPermission(PermissionState.None);
		}

		// Token: 0x04001E1B RID: 7707
		private string m_x509cert;

		// Token: 0x04001E1C RID: 7708
		private string m_certFile;

		// Token: 0x04001E1D RID: 7709
		private string m_signedFile;
	}
}
