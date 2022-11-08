using System;
using System.Collections;
using System.Collections.Specialized;
using System.Security.Permissions;

namespace System.Net
{
	// Token: 0x0200053A RID: 1338
	internal class SpnDictionary : StringDictionary
	{
		// Token: 0x060028D9 RID: 10457 RVA: 0x000A9F6F File Offset: 0x000A8F6F
		internal SpnDictionary()
		{
		}

		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x060028DA RID: 10458 RVA: 0x000A9F87 File Offset: 0x000A8F87
		public override int Count
		{
			get
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
				return this.m_SyncTable.Count;
			}
		}

		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x060028DB RID: 10459 RVA: 0x000A9F9E File Offset: 0x000A8F9E
		public override bool IsSynchronized
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060028DC RID: 10460 RVA: 0x000A9FA4 File Offset: 0x000A8FA4
		internal string InternalGet(string canonicalKey)
		{
			int num = 0;
			string text = null;
			lock (this.m_SyncTable.SyncRoot)
			{
				foreach (object obj in this.m_SyncTable.Keys)
				{
					string text2 = (string)obj;
					if (text2 != null && text2.Length > num && string.Compare(text2, 0, canonicalKey, 0, text2.Length, StringComparison.OrdinalIgnoreCase) == 0)
					{
						num = text2.Length;
						text = text2;
					}
				}
			}
			if (text == null)
			{
				return null;
			}
			return (string)this.m_SyncTable[text];
		}

		// Token: 0x060028DD RID: 10461 RVA: 0x000AA070 File Offset: 0x000A9070
		internal void InternalSet(string canonicalKey, string spn)
		{
			this.m_SyncTable[canonicalKey] = spn;
		}

		// Token: 0x1700085B RID: 2139
		public override string this[string key]
		{
			get
			{
				key = SpnDictionary.GetCanonicalKey(key);
				return this.InternalGet(key);
			}
			set
			{
				key = SpnDictionary.GetCanonicalKey(key);
				this.InternalSet(key, value);
			}
		}

		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x060028E0 RID: 10464 RVA: 0x000AA0A2 File Offset: 0x000A90A2
		public override ICollection Keys
		{
			get
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
				return this.m_SyncTable.Keys;
			}
		}

		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x060028E1 RID: 10465 RVA: 0x000AA0B9 File Offset: 0x000A90B9
		public override object SyncRoot
		{
			[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
			get
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
				return this.m_SyncTable;
			}
		}

		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x060028E2 RID: 10466 RVA: 0x000AA0CB File Offset: 0x000A90CB
		public override ICollection Values
		{
			get
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
				return this.m_SyncTable.Values;
			}
		}

		// Token: 0x060028E3 RID: 10467 RVA: 0x000AA0E2 File Offset: 0x000A90E2
		public override void Add(string key, string value)
		{
			key = SpnDictionary.GetCanonicalKey(key);
			this.m_SyncTable.Add(key, value);
		}

		// Token: 0x060028E4 RID: 10468 RVA: 0x000AA0F9 File Offset: 0x000A90F9
		public override void Clear()
		{
			ExceptionHelper.WebPermissionUnrestricted.Demand();
			this.m_SyncTable.Clear();
		}

		// Token: 0x060028E5 RID: 10469 RVA: 0x000AA110 File Offset: 0x000A9110
		public override bool ContainsKey(string key)
		{
			key = SpnDictionary.GetCanonicalKey(key);
			return this.m_SyncTable.ContainsKey(key);
		}

		// Token: 0x060028E6 RID: 10470 RVA: 0x000AA126 File Offset: 0x000A9126
		public override bool ContainsValue(string value)
		{
			ExceptionHelper.WebPermissionUnrestricted.Demand();
			return this.m_SyncTable.ContainsValue(value);
		}

		// Token: 0x060028E7 RID: 10471 RVA: 0x000AA13E File Offset: 0x000A913E
		public override void CopyTo(Array array, int index)
		{
			ExceptionHelper.WebPermissionUnrestricted.Demand();
			this.m_SyncTable.CopyTo(array, index);
		}

		// Token: 0x060028E8 RID: 10472 RVA: 0x000AA157 File Offset: 0x000A9157
		public override IEnumerator GetEnumerator()
		{
			ExceptionHelper.WebPermissionUnrestricted.Demand();
			return this.m_SyncTable.GetEnumerator();
		}

		// Token: 0x060028E9 RID: 10473 RVA: 0x000AA16E File Offset: 0x000A916E
		public override void Remove(string key)
		{
			key = SpnDictionary.GetCanonicalKey(key);
			this.m_SyncTable.Remove(key);
		}

		// Token: 0x060028EA RID: 10474 RVA: 0x000AA184 File Offset: 0x000A9184
		private static string GetCanonicalKey(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			try
			{
				Uri uri = new Uri(key);
				key = uri.GetParts(UriComponents.Scheme | UriComponents.Host | UriComponents.Port | UriComponents.Path, UriFormat.SafeUnescaped);
				new WebPermission(NetworkAccess.Connect, new Uri(key)).Demand();
			}
			catch (UriFormatException innerException)
			{
				throw new ArgumentException(SR.GetString("net_mustbeuri", new object[]
				{
					"key"
				}), "key", innerException);
			}
			return key;
		}

		// Token: 0x040027C3 RID: 10179
		private Hashtable m_SyncTable = Hashtable.Synchronized(new Hashtable());
	}
}
