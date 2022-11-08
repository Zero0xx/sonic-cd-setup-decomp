using System;
using System.Threading;

namespace System.Text
{
	// Token: 0x020003FA RID: 1018
	[Serializable]
	public abstract class DecoderFallback
	{
		// Token: 0x170007E1 RID: 2017
		// (get) Token: 0x060029F1 RID: 10737 RVA: 0x00083014 File Offset: 0x00082014
		private static object InternalSyncObject
		{
			get
			{
				if (DecoderFallback.s_InternalSyncObject == null)
				{
					object value = new object();
					Interlocked.CompareExchange(ref DecoderFallback.s_InternalSyncObject, value, null);
				}
				return DecoderFallback.s_InternalSyncObject;
			}
		}

		// Token: 0x170007E2 RID: 2018
		// (get) Token: 0x060029F2 RID: 10738 RVA: 0x00083040 File Offset: 0x00082040
		public static DecoderFallback ReplacementFallback
		{
			get
			{
				if (DecoderFallback.replacementFallback == null)
				{
					lock (DecoderFallback.InternalSyncObject)
					{
						if (DecoderFallback.replacementFallback == null)
						{
							DecoderFallback.replacementFallback = new DecoderReplacementFallback();
						}
					}
				}
				return DecoderFallback.replacementFallback;
			}
		}

		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x060029F3 RID: 10739 RVA: 0x00083090 File Offset: 0x00082090
		public static DecoderFallback ExceptionFallback
		{
			get
			{
				if (DecoderFallback.exceptionFallback == null)
				{
					lock (DecoderFallback.InternalSyncObject)
					{
						if (DecoderFallback.exceptionFallback == null)
						{
							DecoderFallback.exceptionFallback = new DecoderExceptionFallback();
						}
					}
				}
				return DecoderFallback.exceptionFallback;
			}
		}

		// Token: 0x060029F4 RID: 10740
		public abstract DecoderFallbackBuffer CreateFallbackBuffer();

		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x060029F5 RID: 10741
		public abstract int MaxCharCount { get; }

		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x060029F6 RID: 10742 RVA: 0x000830E0 File Offset: 0x000820E0
		internal bool IsMicrosoftBestFitFallback
		{
			get
			{
				return this.bIsMicrosoftBestFitFallback;
			}
		}

		// Token: 0x0400147B RID: 5243
		internal bool bIsMicrosoftBestFitFallback;

		// Token: 0x0400147C RID: 5244
		private static DecoderFallback replacementFallback;

		// Token: 0x0400147D RID: 5245
		private static DecoderFallback exceptionFallback;

		// Token: 0x0400147E RID: 5246
		private static object s_InternalSyncObject;
	}
}
