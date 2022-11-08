using System;

namespace System.Text
{
	// Token: 0x020003FE RID: 1022
	[Serializable]
	public sealed class DecoderExceptionFallback : DecoderFallback
	{
		// Token: 0x06002A12 RID: 10770 RVA: 0x000835CC File Offset: 0x000825CC
		public override DecoderFallbackBuffer CreateFallbackBuffer()
		{
			return new DecoderExceptionFallbackBuffer();
		}

		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x06002A13 RID: 10771 RVA: 0x000835D3 File Offset: 0x000825D3
		public override int MaxCharCount
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06002A14 RID: 10772 RVA: 0x000835D8 File Offset: 0x000825D8
		public override bool Equals(object value)
		{
			return value is DecoderExceptionFallback;
		}

		// Token: 0x06002A15 RID: 10773 RVA: 0x000835F2 File Offset: 0x000825F2
		public override int GetHashCode()
		{
			return 879;
		}
	}
}
