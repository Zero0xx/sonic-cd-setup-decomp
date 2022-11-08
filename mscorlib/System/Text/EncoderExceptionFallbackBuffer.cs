using System;

namespace System.Text
{
	// Token: 0x02000409 RID: 1033
	public sealed class EncoderExceptionFallbackBuffer : EncoderFallbackBuffer
	{
		// Token: 0x06002A65 RID: 10853 RVA: 0x0008449C File Offset: 0x0008349C
		public override bool Fallback(char charUnknown, int index)
		{
			throw new EncoderFallbackException(Environment.GetResourceString("Argument_InvalidCodePageConversionIndex", new object[]
			{
				(int)charUnknown,
				index
			}), charUnknown, index);
		}

		// Token: 0x06002A66 RID: 10854 RVA: 0x000844D4 File Offset: 0x000834D4
		public override bool Fallback(char charUnknownHigh, char charUnknownLow, int index)
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
			int num = char.ConvertToUtf32(charUnknownHigh, charUnknownLow);
			throw new EncoderFallbackException(Environment.GetResourceString("Argument_InvalidCodePageConversionIndex", new object[]
			{
				num,
				index
			}), charUnknownHigh, charUnknownLow, index);
		}

		// Token: 0x06002A67 RID: 10855 RVA: 0x00084593 File Offset: 0x00083593
		public override char GetNextChar()
		{
			return '\0';
		}

		// Token: 0x06002A68 RID: 10856 RVA: 0x00084596 File Offset: 0x00083596
		public override bool MovePrevious()
		{
			return false;
		}

		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x06002A69 RID: 10857 RVA: 0x00084599 File Offset: 0x00083599
		public override int Remaining
		{
			get
			{
				return 0;
			}
		}
	}
}
