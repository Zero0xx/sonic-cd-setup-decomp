using System;

namespace System.Xml
{
	// Token: 0x02000038 RID: 56
	internal class Ucs4Encoding2143 : Ucs4Encoding
	{
		// Token: 0x0600019A RID: 410 RVA: 0x00007A2D File Offset: 0x00006A2D
		public Ucs4Encoding2143()
		{
			this.ucs4Decoder = new Ucs4Decoder2143();
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600019B RID: 411 RVA: 0x00007A40 File Offset: 0x00006A40
		public override string EncodingName
		{
			get
			{
				return "ucs-4 (order 2143)";
			}
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00007A48 File Offset: 0x00006A48
		public override byte[] GetPreamble()
		{
			return new byte[]
			{
				0,
				0,
				byte.MaxValue,
				254
			};
		}
	}
}
