using System;

namespace System.Xml
{
	// Token: 0x0200001C RID: 28
	internal class PositionInfo : IXmlLineInfo
	{
		// Token: 0x06000076 RID: 118 RVA: 0x00003BCE File Offset: 0x00002BCE
		public virtual bool HasLineInfo()
		{
			return false;
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00003BD1 File Offset: 0x00002BD1
		public virtual int LineNumber
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00003BD4 File Offset: 0x00002BD4
		public virtual int LinePosition
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00003BD8 File Offset: 0x00002BD8
		public static PositionInfo GetPositionInfo(object o)
		{
			IXmlLineInfo xmlLineInfo = o as IXmlLineInfo;
			if (xmlLineInfo != null)
			{
				return new ReaderPositionInfo(xmlLineInfo);
			}
			return new PositionInfo();
		}
	}
}
