using System;

namespace System.Xml
{
	// Token: 0x0200000B RID: 11
	internal abstract class IncrementalReadDecoder
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000012 RID: 18
		internal abstract int DecodedCount { get; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000013 RID: 19
		internal abstract bool IsFull { get; }

		// Token: 0x06000014 RID: 20
		internal abstract void SetNextOutputBuffer(Array array, int offset, int len);

		// Token: 0x06000015 RID: 21
		internal abstract int Decode(char[] chars, int startPos, int len);

		// Token: 0x06000016 RID: 22
		internal abstract int Decode(string str, int startPos, int len);

		// Token: 0x06000017 RID: 23
		internal abstract void Reset();
	}
}
