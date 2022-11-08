using System;
using System.Globalization;

namespace System.Net
{
	// Token: 0x0200039D RID: 925
	internal class CredentialHostKey
	{
		// Token: 0x06001CE0 RID: 7392 RVA: 0x0006E13D File Offset: 0x0006D13D
		internal CredentialHostKey(string host, int port, string authenticationType)
		{
			this.Host = host;
			this.Port = port;
			this.AuthenticationType = authenticationType;
		}

		// Token: 0x06001CE1 RID: 7393 RVA: 0x0006E15A File Offset: 0x0006D15A
		internal bool Match(string host, int port, string authenticationType)
		{
			return host != null && authenticationType != null && string.Compare(authenticationType, this.AuthenticationType, StringComparison.OrdinalIgnoreCase) == 0 && string.Compare(this.Host, host, StringComparison.OrdinalIgnoreCase) == 0 && port == this.Port;
		}

		// Token: 0x06001CE2 RID: 7394 RVA: 0x0006E194 File Offset: 0x0006D194
		public override int GetHashCode()
		{
			if (!this.m_ComputedHashCode)
			{
				this.m_HashCode = this.AuthenticationType.ToUpperInvariant().GetHashCode() + this.Host.ToUpperInvariant().GetHashCode() + this.Port.GetHashCode();
				this.m_ComputedHashCode = true;
			}
			return this.m_HashCode;
		}

		// Token: 0x06001CE3 RID: 7395 RVA: 0x0006E1EC File Offset: 0x0006D1EC
		public override bool Equals(object comparand)
		{
			CredentialHostKey credentialHostKey = comparand as CredentialHostKey;
			return comparand != null && (string.Compare(this.AuthenticationType, credentialHostKey.AuthenticationType, StringComparison.OrdinalIgnoreCase) == 0 && string.Compare(this.Host, credentialHostKey.Host, StringComparison.OrdinalIgnoreCase) == 0) && this.Port == credentialHostKey.Port;
		}

		// Token: 0x06001CE4 RID: 7396 RVA: 0x0006E240 File Offset: 0x0006D240
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"[",
				this.Host.Length.ToString(NumberFormatInfo.InvariantInfo),
				"]:",
				this.Host,
				":",
				this.Port.ToString(NumberFormatInfo.InvariantInfo),
				":",
				ValidationHelper.ToString(this.AuthenticationType)
			});
		}

		// Token: 0x04001D49 RID: 7497
		internal string Host;

		// Token: 0x04001D4A RID: 7498
		internal string AuthenticationType;

		// Token: 0x04001D4B RID: 7499
		internal int Port;

		// Token: 0x04001D4C RID: 7500
		private int m_HashCode;

		// Token: 0x04001D4D RID: 7501
		private bool m_ComputedHashCode;
	}
}
