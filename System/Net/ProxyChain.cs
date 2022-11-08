using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Net
{
	// Token: 0x02000503 RID: 1283
	internal abstract class ProxyChain : IEnumerable<Uri>, IEnumerable, IDisposable
	{
		// Token: 0x060027DE RID: 10206 RVA: 0x000A49B7 File Offset: 0x000A39B7
		protected ProxyChain(Uri destination)
		{
			this.m_Destination = destination;
		}

		// Token: 0x060027DF RID: 10207 RVA: 0x000A49D4 File Offset: 0x000A39D4
		public IEnumerator<Uri> GetEnumerator()
		{
			ProxyChain.ProxyEnumerator proxyEnumerator = new ProxyChain.ProxyEnumerator(this);
			if (this.m_MainEnumerator == null)
			{
				this.m_MainEnumerator = proxyEnumerator;
			}
			return proxyEnumerator;
		}

		// Token: 0x060027E0 RID: 10208 RVA: 0x000A49F8 File Offset: 0x000A39F8
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060027E1 RID: 10209 RVA: 0x000A4A00 File Offset: 0x000A3A00
		public virtual void Dispose()
		{
		}

		// Token: 0x1700083A RID: 2106
		// (get) Token: 0x060027E2 RID: 10210 RVA: 0x000A4A02 File Offset: 0x000A3A02
		internal IEnumerator<Uri> Enumerator
		{
			get
			{
				if (this.m_MainEnumerator != null)
				{
					return this.m_MainEnumerator;
				}
				return this.GetEnumerator();
			}
		}

		// Token: 0x1700083B RID: 2107
		// (get) Token: 0x060027E3 RID: 10211 RVA: 0x000A4A19 File Offset: 0x000A3A19
		internal Uri Destination
		{
			get
			{
				return this.m_Destination;
			}
		}

		// Token: 0x060027E4 RID: 10212 RVA: 0x000A4A21 File Offset: 0x000A3A21
		internal virtual void Abort()
		{
		}

		// Token: 0x060027E5 RID: 10213 RVA: 0x000A4A23 File Offset: 0x000A3A23
		internal bool HttpAbort(HttpWebRequest request, WebException webException)
		{
			this.Abort();
			return true;
		}

		// Token: 0x1700083C RID: 2108
		// (get) Token: 0x060027E6 RID: 10214 RVA: 0x000A4A2C File Offset: 0x000A3A2C
		internal HttpAbortDelegate HttpAbortDelegate
		{
			get
			{
				if (this.m_HttpAbortDelegate == null)
				{
					this.m_HttpAbortDelegate = new HttpAbortDelegate(this.HttpAbort);
				}
				return this.m_HttpAbortDelegate;
			}
		}

		// Token: 0x060027E7 RID: 10215
		protected abstract bool GetNextProxy(out Uri proxy);

		// Token: 0x04002736 RID: 10038
		private List<Uri> m_Cache = new List<Uri>();

		// Token: 0x04002737 RID: 10039
		private bool m_CacheComplete;

		// Token: 0x04002738 RID: 10040
		private ProxyChain.ProxyEnumerator m_MainEnumerator;

		// Token: 0x04002739 RID: 10041
		private Uri m_Destination;

		// Token: 0x0400273A RID: 10042
		private HttpAbortDelegate m_HttpAbortDelegate;

		// Token: 0x02000504 RID: 1284
		private class ProxyEnumerator : IEnumerator<Uri>, IDisposable, IEnumerator
		{
			// Token: 0x060027E8 RID: 10216 RVA: 0x000A4A4E File Offset: 0x000A3A4E
			internal ProxyEnumerator(ProxyChain chain)
			{
				this.m_Chain = chain;
			}

			// Token: 0x1700083D RID: 2109
			// (get) Token: 0x060027E9 RID: 10217 RVA: 0x000A4A64 File Offset: 0x000A3A64
			public Uri Current
			{
				get
				{
					if (this.m_Finished || this.m_CurrentIndex < 0)
					{
						throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumOpCantHappen"));
					}
					return this.m_Chain.m_Cache[this.m_CurrentIndex];
				}
			}

			// Token: 0x1700083E RID: 2110
			// (get) Token: 0x060027EA RID: 10218 RVA: 0x000A4A9D File Offset: 0x000A3A9D
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x060027EB RID: 10219 RVA: 0x000A4AA8 File Offset: 0x000A3AA8
			public bool MoveNext()
			{
				if (this.m_Finished)
				{
					return false;
				}
				checked
				{
					this.m_CurrentIndex++;
					if (this.m_Chain.m_Cache.Count > this.m_CurrentIndex)
					{
						return true;
					}
					if (this.m_Chain.m_CacheComplete)
					{
						this.m_Finished = true;
						return false;
					}
					bool result;
					lock (this.m_Chain.m_Cache)
					{
						if (this.m_Chain.m_Cache.Count > this.m_CurrentIndex)
						{
							result = true;
						}
						else if (this.m_Chain.m_CacheComplete)
						{
							this.m_Finished = true;
							result = false;
						}
						else
						{
							Uri uri;
							while (this.m_Chain.GetNextProxy(out uri))
							{
								if (uri == null)
								{
									if (this.m_TriedDirect)
									{
										continue;
									}
									this.m_TriedDirect = true;
								}
								this.m_Chain.m_Cache.Add(uri);
								return true;
							}
							this.m_Finished = true;
							this.m_Chain.m_CacheComplete = true;
							result = false;
						}
					}
					return result;
				}
			}

			// Token: 0x060027EC RID: 10220 RVA: 0x000A4BB0 File Offset: 0x000A3BB0
			public void Reset()
			{
				this.m_Finished = false;
				this.m_CurrentIndex = -1;
			}

			// Token: 0x060027ED RID: 10221 RVA: 0x000A4BC0 File Offset: 0x000A3BC0
			public void Dispose()
			{
			}

			// Token: 0x0400273B RID: 10043
			private ProxyChain m_Chain;

			// Token: 0x0400273C RID: 10044
			private bool m_Finished;

			// Token: 0x0400273D RID: 10045
			private int m_CurrentIndex = -1;

			// Token: 0x0400273E RID: 10046
			private bool m_TriedDirect;
		}
	}
}
