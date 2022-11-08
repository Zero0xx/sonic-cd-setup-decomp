using System;
using System.Threading;

namespace System.Text
{
	// Token: 0x02000404 RID: 1028
	[Serializable]
	public abstract class EncoderFallback
	{
		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x06002A40 RID: 10816 RVA: 0x00083EB4 File Offset: 0x00082EB4
		private static object InternalSyncObject
		{
			get
			{
				if (EncoderFallback.s_InternalSyncObject == null)
				{
					object value = new object();
					Interlocked.CompareExchange(ref EncoderFallback.s_InternalSyncObject, value, null);
				}
				return EncoderFallback.s_InternalSyncObject;
			}
		}

		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x06002A41 RID: 10817 RVA: 0x00083EE0 File Offset: 0x00082EE0
		public static EncoderFallback ReplacementFallback
		{
			get
			{
				if (EncoderFallback.replacementFallback == null)
				{
					lock (EncoderFallback.InternalSyncObject)
					{
						if (EncoderFallback.replacementFallback == null)
						{
							EncoderFallback.replacementFallback = new EncoderReplacementFallback();
						}
					}
				}
				return EncoderFallback.replacementFallback;
			}
		}

		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x06002A42 RID: 10818 RVA: 0x00083F30 File Offset: 0x00082F30
		public static EncoderFallback ExceptionFallback
		{
			get
			{
				if (EncoderFallback.exceptionFallback == null)
				{
					lock (EncoderFallback.InternalSyncObject)
					{
						if (EncoderFallback.exceptionFallback == null)
						{
							EncoderFallback.exceptionFallback = new EncoderExceptionFallback();
						}
					}
				}
				return EncoderFallback.exceptionFallback;
			}
		}

		// Token: 0x06002A43 RID: 10819
		public abstract EncoderFallbackBuffer CreateFallbackBuffer();

		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x06002A44 RID: 10820
		public abstract int MaxCharCount { get; }

		// Token: 0x04001494 RID: 5268
		internal bool bIsMicrosoftBestFitFallback;

		// Token: 0x04001495 RID: 5269
		private static EncoderFallback replacementFallback;

		// Token: 0x04001496 RID: 5270
		private static EncoderFallback exceptionFallback;

		// Token: 0x04001497 RID: 5271
		private static object s_InternalSyncObject;
	}
}
