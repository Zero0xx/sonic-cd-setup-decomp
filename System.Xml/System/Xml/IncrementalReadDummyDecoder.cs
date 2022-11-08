using System;

namespace System.Xml
{
	// Token: 0x02000059 RID: 89
	internal class IncrementalReadDummyDecoder : IncrementalReadDecoder
	{
		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600033B RID: 827 RVA: 0x00010DFA File Offset: 0x0000FDFA
		internal override int DecodedCount
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x0600033C RID: 828 RVA: 0x00010DFD File Offset: 0x0000FDFD
		internal override bool IsFull
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600033D RID: 829 RVA: 0x00010E00 File Offset: 0x0000FE00
		internal override void SetNextOutputBuffer(Array array, int offset, int len)
		{
		}

		// Token: 0x0600033E RID: 830 RVA: 0x00010E02 File Offset: 0x0000FE02
		internal override int Decode(char[] chars, int startPos, int len)
		{
			return len;
		}

		// Token: 0x0600033F RID: 831 RVA: 0x00010E05 File Offset: 0x0000FE05
		internal override int Decode(string str, int startPos, int len)
		{
			return len;
		}

		// Token: 0x06000340 RID: 832 RVA: 0x00010E08 File Offset: 0x0000FE08
		internal override void Reset()
		{
		}
	}
}
