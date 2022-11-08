using System;
using System.Globalization;
using System.Text;

namespace System.Xml
{
	// Token: 0x0200004D RID: 77
	internal class CharEntityEncoderFallbackBuffer : EncoderFallbackBuffer
	{
		// Token: 0x06000215 RID: 533 RVA: 0x000092B1 File Offset: 0x000082B1
		internal CharEntityEncoderFallbackBuffer(CharEntityEncoderFallback parent)
		{
			this.parent = parent;
		}

		// Token: 0x06000216 RID: 534 RVA: 0x000092D4 File Offset: 0x000082D4
		public override bool Fallback(char charUnknown, int index)
		{
			if (this.charEntityIndex >= 0)
			{
				new EncoderExceptionFallbackBuffer().Fallback(charUnknown, index);
			}
			if (this.parent.CanReplaceAt(index))
			{
				this.charEntity = string.Format(CultureInfo.InvariantCulture, "&#x{0:X};", new object[]
				{
					(int)charUnknown
				});
				this.charEntityIndex = 0;
				return true;
			}
			EncoderFallbackBuffer encoderFallbackBuffer = new EncoderExceptionFallback().CreateFallbackBuffer();
			encoderFallbackBuffer.Fallback(charUnknown, index);
			return false;
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000934C File Offset: 0x0000834C
		public override bool Fallback(char charUnknownHigh, char charUnknownLow, int index)
		{
			if (!char.IsSurrogatePair(charUnknownHigh, charUnknownLow))
			{
				throw XmlConvert.CreateInvalidSurrogatePairException(charUnknownHigh, charUnknownLow);
			}
			if (this.charEntityIndex >= 0)
			{
				new EncoderExceptionFallbackBuffer().Fallback(charUnknownHigh, charUnknownLow, index);
			}
			if (this.parent.CanReplaceAt(index))
			{
				this.charEntity = string.Format(CultureInfo.InvariantCulture, "&#x{0:X};", new object[]
				{
					char.ConvertToUtf32(charUnknownHigh, charUnknownLow)
				});
				this.charEntityIndex = 0;
				return true;
			}
			EncoderFallbackBuffer encoderFallbackBuffer = new EncoderExceptionFallback().CreateFallbackBuffer();
			encoderFallbackBuffer.Fallback(charUnknownHigh, charUnknownLow, index);
			return false;
		}

		// Token: 0x06000218 RID: 536 RVA: 0x000093DC File Offset: 0x000083DC
		public override char GetNextChar()
		{
			if (this.charEntityIndex == -1)
			{
				return '\0';
			}
			char result = this.charEntity[this.charEntityIndex++];
			if (this.charEntityIndex == this.charEntity.Length)
			{
				this.charEntityIndex = -1;
			}
			return result;
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000942C File Offset: 0x0000842C
		public override bool MovePrevious()
		{
			if (this.charEntityIndex == -1)
			{
				return false;
			}
			if (this.charEntityIndex > 0)
			{
				this.charEntityIndex--;
				return true;
			}
			return false;
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600021A RID: 538 RVA: 0x00009453 File Offset: 0x00008453
		public override int Remaining
		{
			get
			{
				if (this.charEntityIndex == -1)
				{
					return 0;
				}
				return this.charEntity.Length - this.charEntityIndex;
			}
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00009472 File Offset: 0x00008472
		public override void Reset()
		{
			this.charEntityIndex = -1;
		}

		// Token: 0x04000519 RID: 1305
		private CharEntityEncoderFallback parent;

		// Token: 0x0400051A RID: 1306
		private string charEntity = string.Empty;

		// Token: 0x0400051B RID: 1307
		private int charEntityIndex = -1;
	}
}
