using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x0200032F RID: 815
	public struct X509ChainStatus
	{
		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x060019C7 RID: 6599 RVA: 0x00059777 File Offset: 0x00058777
		// (set) Token: 0x060019C8 RID: 6600 RVA: 0x0005977F File Offset: 0x0005877F
		public X509ChainStatusFlags Status
		{
			get
			{
				return this.m_status;
			}
			set
			{
				this.m_status = value;
			}
		}

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x060019C9 RID: 6601 RVA: 0x00059788 File Offset: 0x00058788
		// (set) Token: 0x060019CA RID: 6602 RVA: 0x0005979E File Offset: 0x0005879E
		public string StatusInformation
		{
			get
			{
				if (this.m_statusInformation == null)
				{
					return string.Empty;
				}
				return this.m_statusInformation;
			}
			set
			{
				this.m_statusInformation = value;
			}
		}

		// Token: 0x04001AD6 RID: 6870
		private X509ChainStatusFlags m_status;

		// Token: 0x04001AD7 RID: 6871
		private string m_statusInformation;
	}
}
