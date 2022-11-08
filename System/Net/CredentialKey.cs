using System;
using System.Globalization;

namespace System.Net
{
	// Token: 0x0200039E RID: 926
	internal class CredentialKey
	{
		// Token: 0x06001CE5 RID: 7397 RVA: 0x0006E2BF File Offset: 0x0006D2BF
		internal CredentialKey(Uri uriPrefix, string authenticationType)
		{
			this.UriPrefix = uriPrefix;
			this.UriPrefixLength = this.UriPrefix.ToString().Length;
			this.AuthenticationType = authenticationType;
		}

		// Token: 0x06001CE6 RID: 7398 RVA: 0x0006E2F2 File Offset: 0x0006D2F2
		internal bool Match(Uri uri, string authenticationType)
		{
			return !(uri == null) && authenticationType != null && string.Compare(authenticationType, this.AuthenticationType, StringComparison.OrdinalIgnoreCase) == 0 && this.IsPrefix(uri, this.UriPrefix);
		}

		// Token: 0x06001CE7 RID: 7399 RVA: 0x0006E320 File Offset: 0x0006D320
		internal bool IsPrefix(Uri uri, Uri prefixUri)
		{
			if (prefixUri.Scheme != uri.Scheme || prefixUri.Host != uri.Host || prefixUri.Port != uri.Port)
			{
				return false;
			}
			int num = prefixUri.AbsolutePath.LastIndexOf('/');
			return num <= uri.AbsolutePath.LastIndexOf('/') && string.Compare(uri.AbsolutePath, 0, prefixUri.AbsolutePath, 0, num, StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x06001CE8 RID: 7400 RVA: 0x0006E39B File Offset: 0x0006D39B
		public override int GetHashCode()
		{
			if (!this.m_ComputedHashCode)
			{
				this.m_HashCode = this.AuthenticationType.ToUpperInvariant().GetHashCode() + this.UriPrefixLength + this.UriPrefix.GetHashCode();
				this.m_ComputedHashCode = true;
			}
			return this.m_HashCode;
		}

		// Token: 0x06001CE9 RID: 7401 RVA: 0x0006E3DC File Offset: 0x0006D3DC
		public override bool Equals(object comparand)
		{
			CredentialKey credentialKey = comparand as CredentialKey;
			return comparand != null && string.Compare(this.AuthenticationType, credentialKey.AuthenticationType, StringComparison.OrdinalIgnoreCase) == 0 && this.UriPrefix.Equals(credentialKey.UriPrefix);
		}

		// Token: 0x06001CEA RID: 7402 RVA: 0x0006E420 File Offset: 0x0006D420
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"[",
				this.UriPrefixLength.ToString(NumberFormatInfo.InvariantInfo),
				"]:",
				ValidationHelper.ToString(this.UriPrefix),
				":",
				ValidationHelper.ToString(this.AuthenticationType)
			});
		}

		// Token: 0x04001D4E RID: 7502
		internal Uri UriPrefix;

		// Token: 0x04001D4F RID: 7503
		internal int UriPrefixLength = -1;

		// Token: 0x04001D50 RID: 7504
		internal string AuthenticationType;

		// Token: 0x04001D51 RID: 7505
		private int m_HashCode;

		// Token: 0x04001D52 RID: 7506
		private bool m_ComputedHashCode;
	}
}
