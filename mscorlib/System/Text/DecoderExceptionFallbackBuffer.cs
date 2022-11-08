using System;
using System.Globalization;

namespace System.Text
{
	// Token: 0x020003FF RID: 1023
	public sealed class DecoderExceptionFallbackBuffer : DecoderFallbackBuffer
	{
		// Token: 0x06002A16 RID: 10774 RVA: 0x000835F9 File Offset: 0x000825F9
		public override bool Fallback(byte[] bytesUnknown, int index)
		{
			this.Throw(bytesUnknown, index);
			return true;
		}

		// Token: 0x06002A17 RID: 10775 RVA: 0x00083604 File Offset: 0x00082604
		public override char GetNextChar()
		{
			return '\0';
		}

		// Token: 0x06002A18 RID: 10776 RVA: 0x00083607 File Offset: 0x00082607
		public override bool MovePrevious()
		{
			return false;
		}

		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x06002A19 RID: 10777 RVA: 0x0008360A File Offset: 0x0008260A
		public override int Remaining
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06002A1A RID: 10778 RVA: 0x00083610 File Offset: 0x00082610
		private void Throw(byte[] bytesUnknown, int index)
		{
			StringBuilder stringBuilder = new StringBuilder(bytesUnknown.Length * 3);
			int num = 0;
			while (num < bytesUnknown.Length && num < 20)
			{
				stringBuilder.Append("[");
				stringBuilder.Append(bytesUnknown[num].ToString("X2", CultureInfo.InvariantCulture));
				stringBuilder.Append("]");
				num++;
			}
			if (num == 20)
			{
				stringBuilder.Append(" ...");
			}
			throw new DecoderFallbackException(Environment.GetResourceString("Argument_InvalidCodePageBytesIndex", new object[]
			{
				stringBuilder,
				index
			}), bytesUnknown, index);
		}
	}
}
