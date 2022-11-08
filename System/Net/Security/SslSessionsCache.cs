using System;
using System.Collections;

namespace System.Net.Security
{
	// Token: 0x0200053B RID: 1339
	internal static class SslSessionsCache
	{
		// Token: 0x060028EB RID: 10475 RVA: 0x000AA200 File Offset: 0x000A9200
		internal static SafeFreeCredentials TryCachedCredential(byte[] thumbPrint, SchProtocols allowedProtocols)
		{
			if (SslSessionsCache.s_CachedCreds.Count == 0)
			{
				return null;
			}
			object key = new SslSessionsCache.SslCredKey(thumbPrint, allowedProtocols);
			SafeCredentialReference safeCredentialReference = SslSessionsCache.s_CachedCreds[key] as SafeCredentialReference;
			if (safeCredentialReference == null || safeCredentialReference.IsClosed || safeCredentialReference._Target.IsInvalid)
			{
				return null;
			}
			return safeCredentialReference._Target;
		}

		// Token: 0x060028EC RID: 10476 RVA: 0x000AA25C File Offset: 0x000A925C
		internal static void CacheCredential(SafeFreeCredentials creds, byte[] thumbPrint, SchProtocols allowedProtocols)
		{
			if (creds.IsInvalid)
			{
				return;
			}
			object key = new SslSessionsCache.SslCredKey(thumbPrint, allowedProtocols);
			SafeCredentialReference safeCredentialReference = SslSessionsCache.s_CachedCreds[key] as SafeCredentialReference;
			if (safeCredentialReference == null || safeCredentialReference.IsClosed || safeCredentialReference._Target.IsInvalid)
			{
				lock (SslSessionsCache.s_CachedCreds)
				{
					safeCredentialReference = (SslSessionsCache.s_CachedCreds[key] as SafeCredentialReference);
					if (safeCredentialReference == null || safeCredentialReference.IsClosed)
					{
						safeCredentialReference = SafeCredentialReference.CreateReference(creds);
						if (safeCredentialReference != null)
						{
							SslSessionsCache.s_CachedCreds[key] = safeCredentialReference;
							if (SslSessionsCache.s_CachedCreds.Count % 32 == 0)
							{
								DictionaryEntry[] array = new DictionaryEntry[SslSessionsCache.s_CachedCreds.Count];
								SslSessionsCache.s_CachedCreds.CopyTo(array, 0);
								for (int i = 0; i < array.Length; i++)
								{
									safeCredentialReference = (array[i].Value as SafeCredentialReference);
									if (safeCredentialReference != null)
									{
										creds = safeCredentialReference._Target;
										safeCredentialReference.Close();
										if (!creds.IsClosed && !creds.IsInvalid && (safeCredentialReference = SafeCredentialReference.CreateReference(creds)) != null)
										{
											SslSessionsCache.s_CachedCreds[array[i].Key] = safeCredentialReference;
										}
										else
										{
											SslSessionsCache.s_CachedCreds.Remove(array[i].Key);
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x040027C4 RID: 10180
		private const int c_CheckExpiredModulo = 32;

		// Token: 0x040027C5 RID: 10181
		private static Hashtable s_CachedCreds = new Hashtable(32);

		// Token: 0x0200053C RID: 1340
		private struct SslCredKey
		{
			// Token: 0x060028EE RID: 10478 RVA: 0x000AA3C4 File Offset: 0x000A93C4
			internal SslCredKey(byte[] thumbPrint, SchProtocols allowedProtocols)
			{
				this._CertThumbPrint = ((thumbPrint == null) ? SslSessionsCache.SslCredKey.s_EmptyArray : thumbPrint);
				this._HashCode = 0;
				if (thumbPrint != null)
				{
					this._HashCode ^= (int)this._CertThumbPrint[0];
					if (1 < this._CertThumbPrint.Length)
					{
						this._HashCode ^= (int)this._CertThumbPrint[1] << 8;
					}
					if (2 < this._CertThumbPrint.Length)
					{
						this._HashCode ^= (int)this._CertThumbPrint[2] << 16;
					}
					if (3 < this._CertThumbPrint.Length)
					{
						this._HashCode ^= (int)this._CertThumbPrint[3] << 24;
					}
				}
				this._AllowedProtocols = allowedProtocols;
				this._HashCode ^= (int)this._AllowedProtocols;
			}

			// Token: 0x060028EF RID: 10479 RVA: 0x000AA483 File Offset: 0x000A9483
			public override int GetHashCode()
			{
				return this._HashCode;
			}

			// Token: 0x060028F0 RID: 10480 RVA: 0x000AA48B File Offset: 0x000A948B
			public static bool operator ==(SslSessionsCache.SslCredKey sslCredKey1, SslSessionsCache.SslCredKey sslCredKey2)
			{
				return sslCredKey1 == sslCredKey2 || (sslCredKey1 != null && sslCredKey2 != null && sslCredKey1.Equals(sslCredKey2));
			}

			// Token: 0x060028F1 RID: 10481 RVA: 0x000AA4C2 File Offset: 0x000A94C2
			public static bool operator !=(SslSessionsCache.SslCredKey sslCredKey1, SslSessionsCache.SslCredKey sslCredKey2)
			{
				return sslCredKey1 != sslCredKey2 && (sslCredKey1 == null || sslCredKey2 == null || !sslCredKey1.Equals(sslCredKey2));
			}

			// Token: 0x060028F2 RID: 10482 RVA: 0x000AA4FC File Offset: 0x000A94FC
			public override bool Equals(object y)
			{
				SslSessionsCache.SslCredKey sslCredKey = (SslSessionsCache.SslCredKey)y;
				if (this._CertThumbPrint.Length != sslCredKey._CertThumbPrint.Length)
				{
					return false;
				}
				if (this._HashCode != sslCredKey._HashCode)
				{
					return false;
				}
				for (int i = 0; i < this._CertThumbPrint.Length; i++)
				{
					if (this._CertThumbPrint[i] != sslCredKey._CertThumbPrint[i])
					{
						return false;
					}
				}
				return true;
			}

			// Token: 0x040027C6 RID: 10182
			private static readonly byte[] s_EmptyArray = new byte[0];

			// Token: 0x040027C7 RID: 10183
			private byte[] _CertThumbPrint;

			// Token: 0x040027C8 RID: 10184
			private SchProtocols _AllowedProtocols;

			// Token: 0x040027C9 RID: 10185
			private int _HashCode;
		}
	}
}
