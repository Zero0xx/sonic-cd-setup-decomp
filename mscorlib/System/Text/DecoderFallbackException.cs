using System;
using System.Runtime.Serialization;

namespace System.Text
{
	// Token: 0x02000400 RID: 1024
	[Serializable]
	public sealed class DecoderFallbackException : ArgumentException
	{
		// Token: 0x06002A1C RID: 10780 RVA: 0x000836AF File Offset: 0x000826AF
		public DecoderFallbackException() : base(Environment.GetResourceString("Arg_ArgumentException"))
		{
			base.SetErrorCode(-2147024809);
		}

		// Token: 0x06002A1D RID: 10781 RVA: 0x000836CC File Offset: 0x000826CC
		public DecoderFallbackException(string message) : base(message)
		{
			base.SetErrorCode(-2147024809);
		}

		// Token: 0x06002A1E RID: 10782 RVA: 0x000836E0 File Offset: 0x000826E0
		public DecoderFallbackException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2147024809);
		}

		// Token: 0x06002A1F RID: 10783 RVA: 0x000836F5 File Offset: 0x000826F5
		internal DecoderFallbackException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06002A20 RID: 10784 RVA: 0x000836FF File Offset: 0x000826FF
		public DecoderFallbackException(string message, byte[] bytesUnknown, int index) : base(message)
		{
			this.bytesUnknown = bytesUnknown;
			this.index = index;
		}

		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x06002A21 RID: 10785 RVA: 0x00083716 File Offset: 0x00082716
		public byte[] BytesUnknown
		{
			get
			{
				return this.bytesUnknown;
			}
		}

		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x06002A22 RID: 10786 RVA: 0x0008371E File Offset: 0x0008271E
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x04001489 RID: 5257
		private byte[] bytesUnknown;

		// Token: 0x0400148A RID: 5258
		private int index;
	}
}
