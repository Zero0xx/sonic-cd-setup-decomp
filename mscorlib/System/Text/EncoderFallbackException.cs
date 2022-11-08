using System;
using System.Runtime.Serialization;

namespace System.Text
{
	// Token: 0x0200040A RID: 1034
	[Serializable]
	public sealed class EncoderFallbackException : ArgumentException
	{
		// Token: 0x06002A6B RID: 10859 RVA: 0x000845A4 File Offset: 0x000835A4
		public EncoderFallbackException() : base(Environment.GetResourceString("Arg_ArgumentException"))
		{
			base.SetErrorCode(-2147024809);
		}

		// Token: 0x06002A6C RID: 10860 RVA: 0x000845C1 File Offset: 0x000835C1
		public EncoderFallbackException(string message) : base(message)
		{
			base.SetErrorCode(-2147024809);
		}

		// Token: 0x06002A6D RID: 10861 RVA: 0x000845D5 File Offset: 0x000835D5
		public EncoderFallbackException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2147024809);
		}

		// Token: 0x06002A6E RID: 10862 RVA: 0x000845EA File Offset: 0x000835EA
		internal EncoderFallbackException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06002A6F RID: 10863 RVA: 0x000845F4 File Offset: 0x000835F4
		internal EncoderFallbackException(string message, char charUnknown, int index) : base(message)
		{
			this.charUnknown = charUnknown;
			this.index = index;
		}

		// Token: 0x06002A70 RID: 10864 RVA: 0x0008460C File Offset: 0x0008360C
		internal EncoderFallbackException(string message, char charUnknownHigh, char charUnknownLow, int index) : base(message)
		{
			if (!char.IsHighSurrogate(charUnknownHigh))
			{
				throw new ArgumentOutOfRangeException("charUnknownHigh", Environment.GetResourceString("ArgumentOutOfRange_Range", new object[]
				{
					55296,
					56319
				}));
			}
			if (!char.IsLowSurrogate(charUnknownLow))
			{
				throw new ArgumentOutOfRangeException("CharUnknownLow", Environment.GetResourceString("ArgumentOutOfRange_Range", new object[]
				{
					56320,
					57343
				}));
			}
			this.charUnknownHigh = charUnknownHigh;
			this.charUnknownLow = charUnknownLow;
			this.index = index;
		}

		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x06002A71 RID: 10865 RVA: 0x000846B4 File Offset: 0x000836B4
		public char CharUnknown
		{
			get
			{
				return this.charUnknown;
			}
		}

		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x06002A72 RID: 10866 RVA: 0x000846BC File Offset: 0x000836BC
		public char CharUnknownHigh
		{
			get
			{
				return this.charUnknownHigh;
			}
		}

		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x06002A73 RID: 10867 RVA: 0x000846C4 File Offset: 0x000836C4
		public char CharUnknownLow
		{
			get
			{
				return this.charUnknownLow;
			}
		}

		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x06002A74 RID: 10868 RVA: 0x000846CC File Offset: 0x000836CC
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x06002A75 RID: 10869 RVA: 0x000846D4 File Offset: 0x000836D4
		public bool IsUnknownSurrogate()
		{
			return this.charUnknownHigh != '\0';
		}

		// Token: 0x040014A7 RID: 5287
		private char charUnknown;

		// Token: 0x040014A8 RID: 5288
		private char charUnknownHigh;

		// Token: 0x040014A9 RID: 5289
		private char charUnknownLow;

		// Token: 0x040014AA RID: 5290
		private int index;
	}
}
