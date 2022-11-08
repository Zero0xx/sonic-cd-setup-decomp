using System;

namespace System.Xml
{
	// Token: 0x02000036 RID: 54
	internal class Ucs4Encoding1234 : Ucs4Encoding
	{
		// Token: 0x06000194 RID: 404 RVA: 0x000079AB File Offset: 0x000069AB
		public Ucs4Encoding1234()
		{
			this.ucs4Decoder = new Ucs4Decoder1234();
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000195 RID: 405 RVA: 0x000079BE File Offset: 0x000069BE
		public override string EncodingName
		{
			get
			{
				return "ucs-4 (Bigendian)";
			}
		}

		// Token: 0x06000196 RID: 406 RVA: 0x000079C8 File Offset: 0x000069C8
		public override byte[] GetPreamble()
		{
			return new byte[]
			{
				0,
				0,
				254,
				byte.MaxValue
			};
		}
	}
}
