using System;

namespace System.Xml
{
	// Token: 0x02000037 RID: 55
	internal class Ucs4Encoding4321 : Ucs4Encoding
	{
		// Token: 0x06000197 RID: 407 RVA: 0x000079ED File Offset: 0x000069ED
		public Ucs4Encoding4321()
		{
			this.ucs4Decoder = new Ucs4Decoder4321();
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000198 RID: 408 RVA: 0x00007A00 File Offset: 0x00006A00
		public override string EncodingName
		{
			get
			{
				return "ucs-4";
			}
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00007A08 File Offset: 0x00006A08
		public override byte[] GetPreamble()
		{
			byte[] array = new byte[4];
			array[0] = byte.MaxValue;
			array[1] = 254;
			return array;
		}
	}
}
