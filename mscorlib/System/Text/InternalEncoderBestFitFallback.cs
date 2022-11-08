using System;

namespace System.Text
{
	// Token: 0x02000405 RID: 1029
	[Serializable]
	internal class InternalEncoderBestFitFallback : EncoderFallback
	{
		// Token: 0x06002A46 RID: 10822 RVA: 0x00083F88 File Offset: 0x00082F88
		internal InternalEncoderBestFitFallback(Encoding encoding)
		{
			this.encoding = encoding;
			this.bIsMicrosoftBestFitFallback = true;
		}

		// Token: 0x06002A47 RID: 10823 RVA: 0x00083F9E File Offset: 0x00082F9E
		public override EncoderFallbackBuffer CreateFallbackBuffer()
		{
			return new InternalEncoderBestFitFallbackBuffer(this);
		}

		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x06002A48 RID: 10824 RVA: 0x00083FA6 File Offset: 0x00082FA6
		public override int MaxCharCount
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06002A49 RID: 10825 RVA: 0x00083FAC File Offset: 0x00082FAC
		public override bool Equals(object value)
		{
			InternalEncoderBestFitFallback internalEncoderBestFitFallback = value as InternalEncoderBestFitFallback;
			return internalEncoderBestFitFallback != null && this.encoding.CodePage == internalEncoderBestFitFallback.encoding.CodePage;
		}

		// Token: 0x06002A4A RID: 10826 RVA: 0x00083FDD File Offset: 0x00082FDD
		public override int GetHashCode()
		{
			return this.encoding.CodePage;
		}

		// Token: 0x04001498 RID: 5272
		internal Encoding encoding;

		// Token: 0x04001499 RID: 5273
		internal char[] arrayBestFit;
	}
}
