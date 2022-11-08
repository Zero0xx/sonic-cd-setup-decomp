using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Net
{
	// Token: 0x020003D1 RID: 977
	public class HttpListenerPrefixCollection : ICollection<string>, IEnumerable<string>, IEnumerable
	{
		// Token: 0x06001EDA RID: 7898 RVA: 0x00077664 File Offset: 0x00076664
		internal HttpListenerPrefixCollection(HttpListener listener)
		{
			this.m_HttpListener = listener;
		}

		// Token: 0x06001EDB RID: 7899 RVA: 0x00077674 File Offset: 0x00076674
		public void CopyTo(Array array, int offset)
		{
			this.m_HttpListener.CheckDisposed();
			if (this.Count > array.Length)
			{
				throw new ArgumentOutOfRangeException("array", SR.GetString("net_array_too_small"));
			}
			if (offset + this.Count > array.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			int num = 0;
			foreach (object obj in this.m_HttpListener.m_UriPrefixes.Keys)
			{
				string value = (string)obj;
				array.SetValue(value, offset + num++);
			}
		}

		// Token: 0x06001EDC RID: 7900 RVA: 0x0007772C File Offset: 0x0007672C
		public void CopyTo(string[] array, int offset)
		{
			this.m_HttpListener.CheckDisposed();
			if (this.Count > array.Length)
			{
				throw new ArgumentOutOfRangeException("array", SR.GetString("net_array_too_small"));
			}
			if (offset + this.Count > array.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			int num = 0;
			foreach (object obj in this.m_HttpListener.m_UriPrefixes.Keys)
			{
				string text = (string)obj;
				array[offset + num++] = text;
			}
		}

		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x06001EDD RID: 7901 RVA: 0x000777D8 File Offset: 0x000767D8
		public int Count
		{
			get
			{
				return this.m_HttpListener.m_UriPrefixes.Count;
			}
		}

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x06001EDE RID: 7902 RVA: 0x000777EA File Offset: 0x000767EA
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x06001EDF RID: 7903 RVA: 0x000777ED File Offset: 0x000767ED
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001EE0 RID: 7904 RVA: 0x000777F0 File Offset: 0x000767F0
		public void Add(string uriPrefix)
		{
			this.m_HttpListener.AddPrefix(uriPrefix);
		}

		// Token: 0x06001EE1 RID: 7905 RVA: 0x000777FE File Offset: 0x000767FE
		public bool Contains(string uriPrefix)
		{
			return this.m_HttpListener.m_UriPrefixes.Contains(uriPrefix);
		}

		// Token: 0x06001EE2 RID: 7906 RVA: 0x00077811 File Offset: 0x00076811
		IEnumerator IEnumerable.GetEnumerator()
		{
			return null;
		}

		// Token: 0x06001EE3 RID: 7907 RVA: 0x00077814 File Offset: 0x00076814
		public IEnumerator<string> GetEnumerator()
		{
			return new ListenerPrefixEnumerator(this.m_HttpListener.m_UriPrefixes.Keys.GetEnumerator());
		}

		// Token: 0x06001EE4 RID: 7908 RVA: 0x00077830 File Offset: 0x00076830
		public bool Remove(string uriPrefix)
		{
			return this.m_HttpListener.RemovePrefix(uriPrefix);
		}

		// Token: 0x06001EE5 RID: 7909 RVA: 0x0007783E File Offset: 0x0007683E
		public void Clear()
		{
			this.m_HttpListener.RemoveAll(true);
		}

		// Token: 0x04001E7D RID: 7805
		private HttpListener m_HttpListener;
	}
}
