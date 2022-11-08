using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace System.Net
{
	// Token: 0x020004D4 RID: 1236
	internal class HttpDigestChallenge
	{
		// Token: 0x06002672 RID: 9842 RVA: 0x0009C9FC File Offset: 0x0009B9FC
		internal void SetFromRequest(HttpWebRequest httpWebRequest)
		{
			this.HostName = httpWebRequest.ChallengedUri.Host;
			this.Method = httpWebRequest.CurrentMethod.Name;
			this.Uri = httpWebRequest.Address.AbsolutePath;
			this.ChallengedUri = httpWebRequest.ChallengedUri;
		}

		// Token: 0x06002673 RID: 9843 RVA: 0x0009CA48 File Offset: 0x0009BA48
		internal HttpDigestChallenge CopyAndIncrementNonce()
		{
			HttpDigestChallenge httpDigestChallenge = null;
			lock (this)
			{
				httpDigestChallenge = (base.MemberwiseClone() as HttpDigestChallenge);
				this.NonceCount++;
			}
			httpDigestChallenge.MD5provider = new MD5CryptoServiceProvider();
			return httpDigestChallenge;
		}

		// Token: 0x06002674 RID: 9844 RVA: 0x0009CAA0 File Offset: 0x0009BAA0
		public bool defineAttribute(string name, string value)
		{
			name = name.Trim().ToLower(CultureInfo.InvariantCulture);
			if (name.Equals("algorithm"))
			{
				this.Algorithm = value;
			}
			else if (name.Equals("cnonce"))
			{
				this.ClientNonce = value;
			}
			else if (name.Equals("nc"))
			{
				this.NonceCount = int.Parse(value, NumberFormatInfo.InvariantInfo);
			}
			else if (name.Equals("nonce"))
			{
				this.Nonce = value;
			}
			else if (name.Equals("opaque"))
			{
				this.Opaque = value;
			}
			else if (name.Equals("qop"))
			{
				this.QualityOfProtection = value;
				this.QopPresent = (this.QualityOfProtection != null && this.QualityOfProtection.Length > 0);
			}
			else if (name.Equals("realm"))
			{
				this.Realm = value;
			}
			else if (name.Equals("domain"))
			{
				this.Domain = value;
			}
			else if (!name.Equals("response"))
			{
				if (name.Equals("stale"))
				{
					this.Stale = value.ToLower(CultureInfo.InvariantCulture).Equals("true");
				}
				else if (name.Equals("uri"))
				{
					this.Uri = value;
				}
				else if (name.Equals("charset"))
				{
					this.Charset = value;
				}
				else if (!name.Equals("cipher") && !name.Equals("username"))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002675 RID: 9845 RVA: 0x0009CC30 File Offset: 0x0009BC30
		internal string ToBlob()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(HttpDigest.pair("realm", this.Realm, true));
			if (this.Algorithm != null)
			{
				stringBuilder.Append(",");
				stringBuilder.Append(HttpDigest.pair("algorithm", this.Algorithm, true));
			}
			if (this.Charset != null)
			{
				stringBuilder.Append(",");
				stringBuilder.Append(HttpDigest.pair("charset", this.Charset, false));
			}
			if (this.Nonce != null)
			{
				stringBuilder.Append(",");
				stringBuilder.Append(HttpDigest.pair("nonce", this.Nonce, true));
			}
			if (this.Uri != null)
			{
				stringBuilder.Append(",");
				stringBuilder.Append(HttpDigest.pair("uri", this.Uri, true));
			}
			if (this.ClientNonce != null)
			{
				stringBuilder.Append(",");
				stringBuilder.Append(HttpDigest.pair("cnonce", this.ClientNonce, true));
			}
			if (this.NonceCount > 0)
			{
				stringBuilder.Append(",");
				stringBuilder.Append(HttpDigest.pair("nc", this.NonceCount.ToString("x8", NumberFormatInfo.InvariantInfo), true));
			}
			if (this.QualityOfProtection != null)
			{
				stringBuilder.Append(",");
				stringBuilder.Append(HttpDigest.pair("qop", this.QualityOfProtection, true));
			}
			if (this.Opaque != null)
			{
				stringBuilder.Append(",");
				stringBuilder.Append(HttpDigest.pair("opaque", this.Opaque, true));
			}
			if (this.Domain != null)
			{
				stringBuilder.Append(",");
				stringBuilder.Append(HttpDigest.pair("domain", this.Domain, true));
			}
			if (this.Stale)
			{
				stringBuilder.Append(",");
				stringBuilder.Append(HttpDigest.pair("stale", "true", true));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040025EB RID: 9707
		internal string HostName;

		// Token: 0x040025EC RID: 9708
		internal string Realm;

		// Token: 0x040025ED RID: 9709
		internal Uri ChallengedUri;

		// Token: 0x040025EE RID: 9710
		internal string Uri;

		// Token: 0x040025EF RID: 9711
		internal string Nonce;

		// Token: 0x040025F0 RID: 9712
		internal string Opaque;

		// Token: 0x040025F1 RID: 9713
		internal bool Stale;

		// Token: 0x040025F2 RID: 9714
		internal string Algorithm;

		// Token: 0x040025F3 RID: 9715
		internal string Method;

		// Token: 0x040025F4 RID: 9716
		internal string Domain;

		// Token: 0x040025F5 RID: 9717
		internal string QualityOfProtection;

		// Token: 0x040025F6 RID: 9718
		internal string ClientNonce;

		// Token: 0x040025F7 RID: 9719
		internal int NonceCount;

		// Token: 0x040025F8 RID: 9720
		internal string Charset;

		// Token: 0x040025F9 RID: 9721
		internal string ServiceName;

		// Token: 0x040025FA RID: 9722
		internal string ChannelBinding;

		// Token: 0x040025FB RID: 9723
		internal bool UTF8Charset;

		// Token: 0x040025FC RID: 9724
		internal bool QopPresent;

		// Token: 0x040025FD RID: 9725
		internal MD5CryptoServiceProvider MD5provider = new MD5CryptoServiceProvider();
	}
}
