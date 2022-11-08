using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Net
{
	// Token: 0x02000399 RID: 921
	public class CredentialCache : ICredentials, ICredentialsByHost, IEnumerable
	{
		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x06001CB8 RID: 7352 RVA: 0x0006D6E5 File Offset: 0x0006C6E5
		internal bool IsDefaultInCache
		{
			get
			{
				return this.m_NumbDefaultCredInCache != 0;
			}
		}

		// Token: 0x06001CBA RID: 7354 RVA: 0x0006D714 File Offset: 0x0006C714
		public void Add(Uri uriPrefix, string authType, NetworkCredential cred)
		{
			if (uriPrefix == null)
			{
				throw new ArgumentNullException("uriPrefix");
			}
			if (authType == null)
			{
				throw new ArgumentNullException("authType");
			}
			if (cred is SystemNetworkCredential && string.Compare(authType, "NTLM", StringComparison.OrdinalIgnoreCase) != 0 && (!DigestClient.WDigestAvailable || string.Compare(authType, "Digest", StringComparison.OrdinalIgnoreCase) != 0) && string.Compare(authType, "Kerberos", StringComparison.OrdinalIgnoreCase) != 0 && string.Compare(authType, "Negotiate", StringComparison.OrdinalIgnoreCase) != 0)
			{
				throw new ArgumentException(SR.GetString("net_nodefaultcreds", new object[]
				{
					authType
				}), "authType");
			}
			this.m_version++;
			CredentialKey key = new CredentialKey(uriPrefix, authType);
			this.cache.Add(key, cred);
			if (cred is SystemNetworkCredential)
			{
				this.m_NumbDefaultCredInCache++;
			}
		}

		// Token: 0x06001CBB RID: 7355 RVA: 0x0006D7E4 File Offset: 0x0006C7E4
		public void Add(string host, int port, string authenticationType, NetworkCredential credential)
		{
			if (host == null)
			{
				throw new ArgumentNullException("host");
			}
			if (authenticationType == null)
			{
				throw new ArgumentNullException("authenticationType");
			}
			if (host.Length == 0)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[]
				{
					"host"
				}));
			}
			if (port < 0)
			{
				throw new ArgumentOutOfRangeException("port");
			}
			if (credential is SystemNetworkCredential && string.Compare(authenticationType, "NTLM", StringComparison.OrdinalIgnoreCase) != 0 && (!DigestClient.WDigestAvailable || string.Compare(authenticationType, "Digest", StringComparison.OrdinalIgnoreCase) != 0) && string.Compare(authenticationType, "Kerberos", StringComparison.OrdinalIgnoreCase) != 0 && string.Compare(authenticationType, "Negotiate", StringComparison.OrdinalIgnoreCase) != 0)
			{
				throw new ArgumentException(SR.GetString("net_nodefaultcreds", new object[]
				{
					authenticationType
				}), "authenticationType");
			}
			this.m_version++;
			CredentialHostKey key = new CredentialHostKey(host, port, authenticationType);
			this.cacheForHosts.Add(key, credential);
			if (credential is SystemNetworkCredential)
			{
				this.m_NumbDefaultCredInCache++;
			}
		}

		// Token: 0x06001CBC RID: 7356 RVA: 0x0006D8EC File Offset: 0x0006C8EC
		public void Remove(Uri uriPrefix, string authType)
		{
			if (uriPrefix == null || authType == null)
			{
				return;
			}
			this.m_version++;
			CredentialKey key = new CredentialKey(uriPrefix, authType);
			if (this.cache[key] is SystemNetworkCredential)
			{
				this.m_NumbDefaultCredInCache--;
			}
			this.cache.Remove(key);
		}

		// Token: 0x06001CBD RID: 7357 RVA: 0x0006D94C File Offset: 0x0006C94C
		public void Remove(string host, int port, string authenticationType)
		{
			if (host == null || authenticationType == null)
			{
				return;
			}
			if (port < 0)
			{
				return;
			}
			this.m_version++;
			CredentialHostKey key = new CredentialHostKey(host, port, authenticationType);
			if (this.cacheForHosts[key] is SystemNetworkCredential)
			{
				this.m_NumbDefaultCredInCache--;
			}
			this.cacheForHosts.Remove(key);
		}

		// Token: 0x06001CBE RID: 7358 RVA: 0x0006D9AC File Offset: 0x0006C9AC
		public NetworkCredential GetCredential(Uri uriPrefix, string authType)
		{
			if (uriPrefix == null)
			{
				throw new ArgumentNullException("uriPrefix");
			}
			if (authType == null)
			{
				throw new ArgumentNullException("authType");
			}
			int num = -1;
			NetworkCredential result = null;
			IDictionaryEnumerator enumerator = this.cache.GetEnumerator();
			while (enumerator.MoveNext())
			{
				CredentialKey credentialKey = (CredentialKey)enumerator.Key;
				if (credentialKey.Match(uriPrefix, authType))
				{
					int uriPrefixLength = credentialKey.UriPrefixLength;
					if (uriPrefixLength > num)
					{
						num = uriPrefixLength;
						result = (NetworkCredential)enumerator.Value;
					}
				}
			}
			return result;
		}

		// Token: 0x06001CBF RID: 7359 RVA: 0x0006DA28 File Offset: 0x0006CA28
		public NetworkCredential GetCredential(string host, int port, string authenticationType)
		{
			if (host == null)
			{
				throw new ArgumentNullException("host");
			}
			if (authenticationType == null)
			{
				throw new ArgumentNullException("authenticationType");
			}
			if (host.Length == 0)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[]
				{
					"host"
				}));
			}
			if (port < 0)
			{
				throw new ArgumentOutOfRangeException("port");
			}
			NetworkCredential result = null;
			IDictionaryEnumerator enumerator = this.cacheForHosts.GetEnumerator();
			while (enumerator.MoveNext())
			{
				CredentialHostKey credentialHostKey = (CredentialHostKey)enumerator.Key;
				if (credentialHostKey.Match(host, port, authenticationType))
				{
					result = (NetworkCredential)enumerator.Value;
				}
			}
			return result;
		}

		// Token: 0x06001CC0 RID: 7360 RVA: 0x0006DAC4 File Offset: 0x0006CAC4
		public IEnumerator GetEnumerator()
		{
			return new CredentialCache.CredentialEnumerator(this, this.cache, this.cacheForHosts, this.m_version);
		}

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x06001CC1 RID: 7361 RVA: 0x0006DADE File Offset: 0x0006CADE
		public static ICredentials DefaultCredentials
		{
			get
			{
				new EnvironmentPermission(EnvironmentPermissionAccess.Read, "USERNAME").Demand();
				return SystemNetworkCredential.defaultCredential;
			}
		}

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x06001CC2 RID: 7362 RVA: 0x0006DAF5 File Offset: 0x0006CAF5
		public static NetworkCredential DefaultNetworkCredentials
		{
			get
			{
				new EnvironmentPermission(EnvironmentPermissionAccess.Read, "USERNAME").Demand();
				return SystemNetworkCredential.defaultCredential;
			}
		}

		// Token: 0x04001D35 RID: 7477
		private Hashtable cache = new Hashtable();

		// Token: 0x04001D36 RID: 7478
		private Hashtable cacheForHosts = new Hashtable();

		// Token: 0x04001D37 RID: 7479
		internal int m_version;

		// Token: 0x04001D38 RID: 7480
		private int m_NumbDefaultCredInCache;

		// Token: 0x0200039A RID: 922
		private class CredentialEnumerator : IEnumerator
		{
			// Token: 0x06001CC3 RID: 7363 RVA: 0x0006DB0C File Offset: 0x0006CB0C
			internal CredentialEnumerator(CredentialCache cache, Hashtable table, Hashtable hostTable, int version)
			{
				this.m_cache = cache;
				this.m_array = new ICredentials[table.Count + hostTable.Count];
				table.Values.CopyTo(this.m_array, 0);
				hostTable.Values.CopyTo(this.m_array, table.Count);
				this.m_version = version;
			}

			// Token: 0x1700059D RID: 1437
			// (get) Token: 0x06001CC4 RID: 7364 RVA: 0x0006DB78 File Offset: 0x0006CB78
			object IEnumerator.Current
			{
				get
				{
					if (this.m_index < 0 || this.m_index >= this.m_array.Length)
					{
						throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumOpCantHappen"));
					}
					if (this.m_version != this.m_cache.m_version)
					{
						throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumFailedVersion"));
					}
					return this.m_array[this.m_index];
				}
			}

			// Token: 0x06001CC5 RID: 7365 RVA: 0x0006DBE0 File Offset: 0x0006CBE0
			bool IEnumerator.MoveNext()
			{
				if (this.m_version != this.m_cache.m_version)
				{
					throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumFailedVersion"));
				}
				if (++this.m_index < this.m_array.Length)
				{
					return true;
				}
				this.m_index = this.m_array.Length;
				return false;
			}

			// Token: 0x06001CC6 RID: 7366 RVA: 0x0006DC3C File Offset: 0x0006CC3C
			void IEnumerator.Reset()
			{
				this.m_index = -1;
			}

			// Token: 0x04001D39 RID: 7481
			private CredentialCache m_cache;

			// Token: 0x04001D3A RID: 7482
			private ICredentials[] m_array;

			// Token: 0x04001D3B RID: 7483
			private int m_index = -1;

			// Token: 0x04001D3C RID: 7484
			private int m_version;
		}
	}
}
