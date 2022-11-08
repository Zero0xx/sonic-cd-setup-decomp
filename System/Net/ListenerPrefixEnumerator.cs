using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Net
{
	// Token: 0x020003D0 RID: 976
	internal class ListenerPrefixEnumerator : IEnumerator<string>, IDisposable, IEnumerator
	{
		// Token: 0x06001ED4 RID: 7892 RVA: 0x0007761A File Offset: 0x0007661A
		internal ListenerPrefixEnumerator(IEnumerator enumerator)
		{
			this.enumerator = enumerator;
		}

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x06001ED5 RID: 7893 RVA: 0x00077629 File Offset: 0x00076629
		public string Current
		{
			get
			{
				return (string)this.enumerator.Current;
			}
		}

		// Token: 0x06001ED6 RID: 7894 RVA: 0x0007763B File Offset: 0x0007663B
		public bool MoveNext()
		{
			return this.enumerator.MoveNext();
		}

		// Token: 0x06001ED7 RID: 7895 RVA: 0x00077648 File Offset: 0x00076648
		public void Dispose()
		{
		}

		// Token: 0x06001ED8 RID: 7896 RVA: 0x0007764A File Offset: 0x0007664A
		void IEnumerator.Reset()
		{
			this.enumerator.Reset();
		}

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x06001ED9 RID: 7897 RVA: 0x00077657 File Offset: 0x00076657
		object IEnumerator.Current
		{
			get
			{
				return this.enumerator.Current;
			}
		}

		// Token: 0x04001E7C RID: 7804
		private IEnumerator enumerator;
	}
}
