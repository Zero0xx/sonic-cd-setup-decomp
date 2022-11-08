using System;

namespace System.Text
{
	// Token: 0x02000406 RID: 1030
	public abstract class EncoderFallbackBuffer
	{
		// Token: 0x06002A4B RID: 10827
		public abstract bool Fallback(char charUnknown, int index);

		// Token: 0x06002A4C RID: 10828
		public abstract bool Fallback(char charUnknownHigh, char charUnknownLow, int index);

		// Token: 0x06002A4D RID: 10829
		public abstract char GetNextChar();

		// Token: 0x06002A4E RID: 10830
		public abstract bool MovePrevious();

		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x06002A4F RID: 10831
		public abstract int Remaining { get; }

		// Token: 0x06002A50 RID: 10832 RVA: 0x00083FEA File Offset: 0x00082FEA
		public virtual void Reset()
		{
			while (this.GetNextChar() != '\0')
			{
			}
		}

		// Token: 0x06002A51 RID: 10833 RVA: 0x00083FF4 File Offset: 0x00082FF4
		internal void InternalReset()
		{
			this.charStart = null;
			this.bFallingBack = false;
			this.iRecursionCount = 0;
			this.Reset();
		}

		// Token: 0x06002A52 RID: 10834 RVA: 0x00084012 File Offset: 0x00083012
		internal unsafe void InternalInitialize(char* charStart, char* charEnd, EncoderNLS encoder, bool setEncoder)
		{
			this.charStart = charStart;
			this.charEnd = charEnd;
			this.encoder = encoder;
			this.setEncoder = setEncoder;
			this.bUsedEncoder = false;
			this.bFallingBack = false;
			this.iRecursionCount = 0;
		}

		// Token: 0x06002A53 RID: 10835 RVA: 0x00084048 File Offset: 0x00083048
		internal char InternalGetNextChar()
		{
			char nextChar = this.GetNextChar();
			this.bFallingBack = (nextChar != '\0');
			if (nextChar == '\0')
			{
				this.iRecursionCount = 0;
			}
			return nextChar;
		}

		// Token: 0x06002A54 RID: 10836 RVA: 0x00084074 File Offset: 0x00083074
		internal unsafe virtual bool InternalFallback(char ch, ref char* chars)
		{
			int index = (chars - this.charStart) / 2 - 1;
			if (char.IsHighSurrogate(ch))
			{
				if (chars >= this.charEnd)
				{
					if (this.encoder != null && !this.encoder.MustFlush)
					{
						if (this.setEncoder)
						{
							this.bUsedEncoder = true;
							this.encoder.charLeftOver = ch;
						}
						this.bFallingBack = false;
						return false;
					}
				}
				else
				{
					char c = (char)(*chars);
					if (char.IsLowSurrogate(c))
					{
						if (this.bFallingBack && this.iRecursionCount++ > 250)
						{
							this.ThrowLastCharRecursive(char.ConvertToUtf32(ch, c));
						}
						chars += (IntPtr)2;
						this.bFallingBack = this.Fallback(ch, c, index);
						return this.bFallingBack;
					}
				}
			}
			if (this.bFallingBack && this.iRecursionCount++ > 250)
			{
				this.ThrowLastCharRecursive((int)ch);
			}
			this.bFallingBack = this.Fallback(ch, index);
			return this.bFallingBack;
		}

		// Token: 0x06002A55 RID: 10837 RVA: 0x00084174 File Offset: 0x00083174
		internal void ThrowLastCharRecursive(int charRecursive)
		{
			throw new ArgumentException(Environment.GetResourceString("Argument_RecursiveFallback", new object[]
			{
				charRecursive
			}), "chars");
		}

		// Token: 0x0400149A RID: 5274
		private const int iMaxRecursion = 250;

		// Token: 0x0400149B RID: 5275
		internal unsafe char* charStart = null;

		// Token: 0x0400149C RID: 5276
		internal unsafe char* charEnd;

		// Token: 0x0400149D RID: 5277
		internal EncoderNLS encoder;

		// Token: 0x0400149E RID: 5278
		internal bool setEncoder;

		// Token: 0x0400149F RID: 5279
		internal bool bUsedEncoder;

		// Token: 0x040014A0 RID: 5280
		internal bool bFallingBack;

		// Token: 0x040014A1 RID: 5281
		internal int iRecursionCount;
	}
}
