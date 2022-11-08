using System;

namespace System.Text
{
	// Token: 0x020003FB RID: 1019
	[Serializable]
	internal sealed class InternalDecoderBestFitFallback : DecoderFallback
	{
		// Token: 0x060029F8 RID: 10744 RVA: 0x000830F0 File Offset: 0x000820F0
		internal InternalDecoderBestFitFallback(Encoding encoding)
		{
			this.encoding = encoding;
			this.bIsMicrosoftBestFitFallback = true;
		}

		// Token: 0x060029F9 RID: 10745 RVA: 0x0008310E File Offset: 0x0008210E
		public override DecoderFallbackBuffer CreateFallbackBuffer()
		{
			return new InternalDecoderBestFitFallbackBuffer(this);
		}

		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x060029FA RID: 10746 RVA: 0x00083116 File Offset: 0x00082116
		public override int MaxCharCount
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060029FB RID: 10747 RVA: 0x0008311C File Offset: 0x0008211C
		public override bool Equals(object value)
		{
			InternalDecoderBestFitFallback internalDecoderBestFitFallback = value as InternalDecoderBestFitFallback;
			return internalDecoderBestFitFallback != null && this.encoding.CodePage == internalDecoderBestFitFallback.encoding.CodePage;
		}

		// Token: 0x060029FC RID: 10748 RVA: 0x0008314D File Offset: 0x0008214D
		public override int GetHashCode()
		{
			return this.encoding.CodePage;
		}

		// Token: 0x0400147F RID: 5247
		internal Encoding encoding;

		// Token: 0x04001480 RID: 5248
		internal char[] arrayBestFit;

		// Token: 0x04001481 RID: 5249
		internal char cReplacement = '?';
	}
}
