using System;

namespace System.Xml
{
	// Token: 0x0200000E RID: 14
	internal class XmlRawWriterBase64Encoder : Base64Encoder
	{
		// Token: 0x06000027 RID: 39 RVA: 0x0000277D File Offset: 0x0000177D
		internal XmlRawWriterBase64Encoder(XmlRawWriter rawWriter)
		{
			this.rawWriter = rawWriter;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x0000278C File Offset: 0x0000178C
		internal override void WriteChars(char[] chars, int index, int count)
		{
			this.rawWriter.WriteRaw(chars, index, count);
		}

		// Token: 0x04000450 RID: 1104
		private XmlRawWriter rawWriter;
	}
}
