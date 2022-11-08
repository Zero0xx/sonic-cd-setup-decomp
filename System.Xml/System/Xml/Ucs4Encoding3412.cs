using System;

namespace System.Xml
{
	// Token: 0x02000039 RID: 57
	internal class Ucs4Encoding3412 : Ucs4Encoding
	{
		// Token: 0x0600019D RID: 413 RVA: 0x00007A6D File Offset: 0x00006A6D
		public Ucs4Encoding3412()
		{
			this.ucs4Decoder = new Ucs4Decoder3412();
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600019E RID: 414 RVA: 0x00007A80 File Offset: 0x00006A80
		public override string EncodingName
		{
			get
			{
				return "ucs-4 (order 3412)";
			}
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00007A88 File Offset: 0x00006A88
		public override byte[] GetPreamble()
		{
			byte[] array = new byte[4];
			array[0] = 254;
			array[1] = byte.MaxValue;
			return array;
		}
	}
}
