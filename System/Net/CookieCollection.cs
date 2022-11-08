using System;
using System.Collections;
using System.Runtime.Serialization;

namespace System.Net
{
	// Token: 0x0200038F RID: 911
	[Serializable]
	public class CookieCollection : ICollection, IEnumerable
	{
		// Token: 0x06001C72 RID: 7282 RVA: 0x0006BE04 File Offset: 0x0006AE04
		public CookieCollection()
		{
			this.m_IsReadOnly = true;
		}

		// Token: 0x06001C73 RID: 7283 RVA: 0x0006BE29 File Offset: 0x0006AE29
		internal CookieCollection(bool IsReadOnly)
		{
			this.m_IsReadOnly = IsReadOnly;
		}

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x06001C74 RID: 7284 RVA: 0x0006BE4E File Offset: 0x0006AE4E
		public bool IsReadOnly
		{
			get
			{
				return this.m_IsReadOnly;
			}
		}

		// Token: 0x1700058A RID: 1418
		public Cookie this[int index]
		{
			get
			{
				if (index < 0 || index >= this.m_list.Count)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				return (Cookie)this.m_list[index];
			}
		}

		// Token: 0x1700058B RID: 1419
		public Cookie this[string name]
		{
			get
			{
				foreach (object obj in this.m_list)
				{
					Cookie cookie = (Cookie)obj;
					if (string.Compare(cookie.Name, name, StringComparison.OrdinalIgnoreCase) == 0)
					{
						return cookie;
					}
				}
				return null;
			}
		}

		// Token: 0x06001C77 RID: 7287 RVA: 0x0006BEF0 File Offset: 0x0006AEF0
		public void Add(Cookie cookie)
		{
			if (cookie == null)
			{
				throw new ArgumentNullException("cookie");
			}
			this.m_version++;
			int num = this.IndexOf(cookie);
			if (num == -1)
			{
				this.m_list.Add(cookie);
				return;
			}
			this.m_list[num] = cookie;
		}

		// Token: 0x06001C78 RID: 7288 RVA: 0x0006BF40 File Offset: 0x0006AF40
		public void Add(CookieCollection cookies)
		{
			if (cookies == null)
			{
				throw new ArgumentNullException("cookies");
			}
			foreach (object obj in cookies)
			{
				Cookie cookie = (Cookie)obj;
				this.Add(cookie);
			}
		}

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x06001C79 RID: 7289 RVA: 0x0006BFA4 File Offset: 0x0006AFA4
		public int Count
		{
			get
			{
				return this.m_list.Count;
			}
		}

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x06001C7A RID: 7290 RVA: 0x0006BFB1 File Offset: 0x0006AFB1
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x06001C7B RID: 7291 RVA: 0x0006BFB4 File Offset: 0x0006AFB4
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06001C7C RID: 7292 RVA: 0x0006BFB7 File Offset: 0x0006AFB7
		public void CopyTo(Array array, int index)
		{
			this.m_list.CopyTo(array, index);
		}

		// Token: 0x06001C7D RID: 7293 RVA: 0x0006BFC6 File Offset: 0x0006AFC6
		public void CopyTo(Cookie[] array, int index)
		{
			this.m_list.CopyTo(array, index);
		}

		// Token: 0x06001C7E RID: 7294 RVA: 0x0006BFD8 File Offset: 0x0006AFD8
		internal DateTime TimeStamp(CookieCollection.Stamp how)
		{
			switch (how)
			{
			case CookieCollection.Stamp.Set:
				this.m_TimeStamp = DateTime.Now;
				break;
			case CookieCollection.Stamp.SetToUnused:
				this.m_TimeStamp = DateTime.MinValue;
				break;
			case CookieCollection.Stamp.SetToMaxUsed:
				this.m_TimeStamp = DateTime.MaxValue;
				break;
			}
			return this.m_TimeStamp;
		}

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x06001C7F RID: 7295 RVA: 0x0006C02A File Offset: 0x0006B02A
		internal bool IsOtherVersionSeen
		{
			get
			{
				return this.m_has_other_versions;
			}
		}

		// Token: 0x06001C80 RID: 7296 RVA: 0x0006C034 File Offset: 0x0006B034
		internal int InternalAdd(Cookie cookie, bool isStrict)
		{
			int result = 1;
			if (isStrict)
			{
				IComparer comparer = Cookie.GetComparer();
				int num = 0;
				foreach (object obj in this.m_list)
				{
					Cookie cookie2 = (Cookie)obj;
					if (comparer.Compare(cookie, cookie2) == 0)
					{
						result = 0;
						if (cookie2.Variant <= cookie.Variant)
						{
							this.m_list[num] = cookie;
							break;
						}
						break;
					}
					else
					{
						num++;
					}
				}
				if (num == this.m_list.Count)
				{
					this.m_list.Add(cookie);
				}
			}
			else
			{
				this.m_list.Add(cookie);
			}
			if (cookie.Version != 1)
			{
				this.m_has_other_versions = true;
			}
			return result;
		}

		// Token: 0x06001C81 RID: 7297 RVA: 0x0006C108 File Offset: 0x0006B108
		internal int IndexOf(Cookie cookie)
		{
			IComparer comparer = Cookie.GetComparer();
			int num = 0;
			foreach (object obj in this.m_list)
			{
				Cookie y = (Cookie)obj;
				if (comparer.Compare(cookie, y) == 0)
				{
					return num;
				}
				num++;
			}
			return -1;
		}

		// Token: 0x06001C82 RID: 7298 RVA: 0x0006C180 File Offset: 0x0006B180
		internal void RemoveAt(int idx)
		{
			this.m_list.RemoveAt(idx);
		}

		// Token: 0x06001C83 RID: 7299 RVA: 0x0006C18E File Offset: 0x0006B18E
		public IEnumerator GetEnumerator()
		{
			return new CookieCollection.CookieCollectionEnumerator(this);
		}

		// Token: 0x04001D19 RID: 7449
		internal int m_version;

		// Token: 0x04001D1A RID: 7450
		private ArrayList m_list = new ArrayList();

		// Token: 0x04001D1B RID: 7451
		private DateTime m_TimeStamp = DateTime.MinValue;

		// Token: 0x04001D1C RID: 7452
		private bool m_has_other_versions;

		// Token: 0x04001D1D RID: 7453
		[OptionalField]
		private bool m_IsReadOnly;

		// Token: 0x02000390 RID: 912
		internal enum Stamp
		{
			// Token: 0x04001D1F RID: 7455
			Check,
			// Token: 0x04001D20 RID: 7456
			Set,
			// Token: 0x04001D21 RID: 7457
			SetToUnused,
			// Token: 0x04001D22 RID: 7458
			SetToMaxUsed
		}

		// Token: 0x02000391 RID: 913
		private class CookieCollectionEnumerator : IEnumerator
		{
			// Token: 0x06001C84 RID: 7300 RVA: 0x0006C196 File Offset: 0x0006B196
			internal CookieCollectionEnumerator(CookieCollection cookies)
			{
				this.m_cookies = cookies;
				this.m_count = cookies.Count;
				this.m_version = cookies.m_version;
			}

			// Token: 0x17000590 RID: 1424
			// (get) Token: 0x06001C85 RID: 7301 RVA: 0x0006C1C4 File Offset: 0x0006B1C4
			object IEnumerator.Current
			{
				get
				{
					if (this.m_index < 0 || this.m_index >= this.m_count)
					{
						throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumOpCantHappen"));
					}
					if (this.m_version != this.m_cookies.m_version)
					{
						throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumFailedVersion"));
					}
					return this.m_cookies[this.m_index];
				}
			}

			// Token: 0x06001C86 RID: 7302 RVA: 0x0006C22C File Offset: 0x0006B22C
			bool IEnumerator.MoveNext()
			{
				if (this.m_version != this.m_cookies.m_version)
				{
					throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumFailedVersion"));
				}
				if (++this.m_index < this.m_count)
				{
					return true;
				}
				this.m_index = this.m_count;
				return false;
			}

			// Token: 0x06001C87 RID: 7303 RVA: 0x0006C284 File Offset: 0x0006B284
			void IEnumerator.Reset()
			{
				this.m_index = -1;
			}

			// Token: 0x04001D23 RID: 7459
			private CookieCollection m_cookies;

			// Token: 0x04001D24 RID: 7460
			private int m_count;

			// Token: 0x04001D25 RID: 7461
			private int m_index = -1;

			// Token: 0x04001D26 RID: 7462
			private int m_version;
		}
	}
}
