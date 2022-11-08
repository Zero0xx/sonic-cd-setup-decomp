using System;

namespace System.Text
{
	// Token: 0x02000408 RID: 1032
	[Serializable]
	public sealed class EncoderExceptionFallback : EncoderFallback
	{
		// Token: 0x06002A61 RID: 10849 RVA: 0x0008446B File Offset: 0x0008346B
		public override EncoderFallbackBuffer CreateFallbackBuffer()
		{
			return new EncoderExceptionFallbackBuffer();
		}

		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x06002A62 RID: 10850 RVA: 0x00084472 File Offset: 0x00083472
		public override int MaxCharCount
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06002A63 RID: 10851 RVA: 0x00084478 File Offset: 0x00083478
		public override bool Equals(object value)
		{
			return value is EncoderExceptionFallback;
		}

		// Token: 0x06002A64 RID: 10852 RVA: 0x00084492 File Offset: 0x00083492
		public override int GetHashCode()
		{
			return 654;
		}
	}
}
