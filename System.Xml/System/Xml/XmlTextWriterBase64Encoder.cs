using System;

namespace System.Xml
{
	// Token: 0x0200000F RID: 15
	internal class XmlTextWriterBase64Encoder : Base64Encoder
	{
		// Token: 0x06000029 RID: 41 RVA: 0x0000279C File Offset: 0x0000179C
		internal XmlTextWriterBase64Encoder(XmlTextEncoder xmlTextEncoder)
		{
			this.xmlTextEncoder = xmlTextEncoder;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000027AB File Offset: 0x000017AB
		internal override void WriteChars(char[] chars, int index, int count)
		{
			this.xmlTextEncoder.WriteRaw(chars, index, count);
		}

		// Token: 0x04000451 RID: 1105
		private XmlTextEncoder xmlTextEncoder;
	}
}
