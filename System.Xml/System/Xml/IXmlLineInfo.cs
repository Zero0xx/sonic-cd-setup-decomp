using System;

namespace System.Xml
{
	// Token: 0x0200001B RID: 27
	public interface IXmlLineInfo
	{
		// Token: 0x06000073 RID: 115
		bool HasLineInfo();

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000074 RID: 116
		int LineNumber { get; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000075 RID: 117
		int LinePosition { get; }
	}
}
