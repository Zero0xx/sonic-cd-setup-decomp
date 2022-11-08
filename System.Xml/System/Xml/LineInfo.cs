using System;

namespace System.Xml
{
	// Token: 0x0200001F RID: 31
	internal struct LineInfo
	{
		// Token: 0x06000082 RID: 130 RVA: 0x00003C39 File Offset: 0x00002C39
		public LineInfo(int lineNo, int linePos)
		{
			this.lineNo = lineNo;
			this.linePos = linePos;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003C49 File Offset: 0x00002C49
		public void Set(int lineNo, int linePos)
		{
			this.lineNo = lineNo;
			this.linePos = linePos;
		}

		// Token: 0x0400047F RID: 1151
		internal int lineNo;

		// Token: 0x04000480 RID: 1152
		internal int linePos;
	}
}
