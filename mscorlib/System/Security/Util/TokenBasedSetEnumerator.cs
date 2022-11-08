using System;

namespace System.Security.Util
{
	// Token: 0x02000489 RID: 1161
	internal struct TokenBasedSetEnumerator
	{
		// Token: 0x06002E1E RID: 11806 RVA: 0x0009AD27 File Offset: 0x00099D27
		public bool MoveNext()
		{
			return this._tb != null && this._tb.MoveNext(ref this);
		}

		// Token: 0x06002E1F RID: 11807 RVA: 0x0009AD3F File Offset: 0x00099D3F
		public void Reset()
		{
			this.Index = -1;
			this.Current = null;
		}

		// Token: 0x06002E20 RID: 11808 RVA: 0x0009AD4F File Offset: 0x00099D4F
		public TokenBasedSetEnumerator(TokenBasedSet tb)
		{
			this.Index = -1;
			this.Current = null;
			this._tb = tb;
		}

		// Token: 0x040017B7 RID: 6071
		public object Current;

		// Token: 0x040017B8 RID: 6072
		public int Index;

		// Token: 0x040017B9 RID: 6073
		private TokenBasedSet _tb;
	}
}
