using System;

namespace System.Text
{
	// Token: 0x02000402 RID: 1026
	public sealed class DecoderReplacementFallbackBuffer : DecoderFallbackBuffer
	{
		// Token: 0x06002A2A RID: 10794 RVA: 0x0008380F File Offset: 0x0008280F
		public DecoderReplacementFallbackBuffer(DecoderReplacementFallback fallback)
		{
			this.strDefault = fallback.DefaultString;
		}

		// Token: 0x06002A2B RID: 10795 RVA: 0x00083831 File Offset: 0x00082831
		public override bool Fallback(byte[] bytesUnknown, int index)
		{
			if (this.fallbackCount >= 1)
			{
				base.ThrowLastBytesRecursive(bytesUnknown);
			}
			if (this.strDefault.Length == 0)
			{
				return false;
			}
			this.fallbackCount = this.strDefault.Length;
			this.fallbackIndex = -1;
			return true;
		}

		// Token: 0x06002A2C RID: 10796 RVA: 0x0008386C File Offset: 0x0008286C
		public override char GetNextChar()
		{
			this.fallbackCount--;
			this.fallbackIndex++;
			if (this.fallbackCount < 0)
			{
				return '\0';
			}
			if (this.fallbackCount == 2147483647)
			{
				this.fallbackCount = -1;
				return '\0';
			}
			return this.strDefault[this.fallbackIndex];
		}

		// Token: 0x06002A2D RID: 10797 RVA: 0x000838C7 File Offset: 0x000828C7
		public override bool MovePrevious()
		{
			if (this.fallbackCount >= -1 && this.fallbackIndex >= 0)
			{
				this.fallbackIndex--;
				this.fallbackCount++;
				return true;
			}
			return false;
		}

		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x06002A2E RID: 10798 RVA: 0x000838FA File Offset: 0x000828FA
		public override int Remaining
		{
			get
			{
				if (this.fallbackCount >= 0)
				{
					return this.fallbackCount;
				}
				return 0;
			}
		}

		// Token: 0x06002A2F RID: 10799 RVA: 0x0008390D File Offset: 0x0008290D
		public override void Reset()
		{
			this.fallbackCount = -1;
			this.fallbackIndex = -1;
			this.byteStart = null;
		}

		// Token: 0x06002A30 RID: 10800 RVA: 0x00083925 File Offset: 0x00082925
		internal unsafe override int InternalFallback(byte[] bytes, byte* pBytes)
		{
			return this.strDefault.Length;
		}

		// Token: 0x0400148C RID: 5260
		private string strDefault;

		// Token: 0x0400148D RID: 5261
		private int fallbackCount = -1;

		// Token: 0x0400148E RID: 5262
		private int fallbackIndex = -1;
	}
}
