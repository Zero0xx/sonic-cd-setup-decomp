using System;

namespace System.Xml
{
	// Token: 0x0200001D RID: 29
	internal class ReaderPositionInfo : PositionInfo
	{
		// Token: 0x0600007B RID: 123 RVA: 0x00003C03 File Offset: 0x00002C03
		public ReaderPositionInfo(IXmlLineInfo lineInfo)
		{
			this.lineInfo = lineInfo;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003C12 File Offset: 0x00002C12
		public override bool HasLineInfo()
		{
			return this.lineInfo.HasLineInfo();
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600007D RID: 125 RVA: 0x00003C1F File Offset: 0x00002C1F
		public override int LineNumber
		{
			get
			{
				return this.lineInfo.LineNumber;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00003C2C File Offset: 0x00002C2C
		public override int LinePosition
		{
			get
			{
				return this.lineInfo.LinePosition;
			}
		}

		// Token: 0x0400047E RID: 1150
		private IXmlLineInfo lineInfo;
	}
}
