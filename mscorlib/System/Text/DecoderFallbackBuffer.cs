using System;
using System.Globalization;

namespace System.Text
{
	// Token: 0x020003FC RID: 1020
	public abstract class DecoderFallbackBuffer
	{
		// Token: 0x060029FD RID: 10749
		public abstract bool Fallback(byte[] bytesUnknown, int index);

		// Token: 0x060029FE RID: 10750
		public abstract char GetNextChar();

		// Token: 0x060029FF RID: 10751
		public abstract bool MovePrevious();

		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x06002A00 RID: 10752
		public abstract int Remaining { get; }

		// Token: 0x06002A01 RID: 10753 RVA: 0x0008315A File Offset: 0x0008215A
		public virtual void Reset()
		{
			while (this.GetNextChar() != '\0')
			{
			}
		}

		// Token: 0x06002A02 RID: 10754 RVA: 0x00083164 File Offset: 0x00082164
		internal void InternalReset()
		{
			this.byteStart = null;
			this.Reset();
		}

		// Token: 0x06002A03 RID: 10755 RVA: 0x00083174 File Offset: 0x00082174
		internal unsafe void InternalInitialize(byte* byteStart, char* charEnd)
		{
			this.byteStart = byteStart;
			this.charEnd = charEnd;
		}

		// Token: 0x06002A04 RID: 10756 RVA: 0x00083184 File Offset: 0x00082184
		internal unsafe virtual bool InternalFallback(byte[] bytes, byte* pBytes, ref char* chars)
		{
			if (this.Fallback(bytes, (int)((long)(pBytes - this.byteStart) - (long)bytes.Length)))
			{
				char* ptr = chars;
				bool flag = false;
				char nextChar;
				while ((nextChar = this.GetNextChar()) != '\0')
				{
					if (char.IsSurrogate(nextChar))
					{
						if (char.IsHighSurrogate(nextChar))
						{
							if (flag)
							{
								throw new ArgumentException(Environment.GetResourceString("Argument_InvalidCharSequenceNoIndex"));
							}
							flag = true;
						}
						else
						{
							if (!flag)
							{
								throw new ArgumentException(Environment.GetResourceString("Argument_InvalidCharSequenceNoIndex"));
							}
							flag = false;
						}
					}
					if (ptr >= this.charEnd)
					{
						return false;
					}
					*(ptr++) = nextChar;
				}
				if (flag)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidCharSequenceNoIndex"));
				}
				chars = ptr;
			}
			return true;
		}

		// Token: 0x06002A05 RID: 10757 RVA: 0x00083224 File Offset: 0x00082224
		internal unsafe virtual int InternalFallback(byte[] bytes, byte* pBytes)
		{
			if (!this.Fallback(bytes, (int)((long)(pBytes - this.byteStart) - (long)bytes.Length)))
			{
				return 0;
			}
			int num = 0;
			bool flag = false;
			char nextChar;
			while ((nextChar = this.GetNextChar()) != '\0')
			{
				if (char.IsSurrogate(nextChar))
				{
					if (char.IsHighSurrogate(nextChar))
					{
						if (flag)
						{
							throw new ArgumentException(Environment.GetResourceString("Argument_InvalidCharSequenceNoIndex"));
						}
						flag = true;
					}
					else
					{
						if (!flag)
						{
							throw new ArgumentException(Environment.GetResourceString("Argument_InvalidCharSequenceNoIndex"));
						}
						flag = false;
					}
				}
				num++;
			}
			if (flag)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidCharSequenceNoIndex"));
			}
			return num;
		}

		// Token: 0x06002A06 RID: 10758 RVA: 0x000832B4 File Offset: 0x000822B4
		internal void ThrowLastBytesRecursive(byte[] bytesUnknown)
		{
			StringBuilder stringBuilder = new StringBuilder(bytesUnknown.Length * 3);
			int num = 0;
			while (num < bytesUnknown.Length && num < 20)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" ");
				}
				stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "\\x{0:X2}", new object[]
				{
					bytesUnknown[num]
				}));
				num++;
			}
			if (num == 20)
			{
				stringBuilder.Append(" ...");
			}
			throw new ArgumentException(Environment.GetResourceString("Argument_RecursiveFallbackBytes", new object[]
			{
				stringBuilder.ToString()
			}), "bytesUnknown");
		}

		// Token: 0x04001482 RID: 5250
		internal unsafe byte* byteStart = null;

		// Token: 0x04001483 RID: 5251
		internal unsafe char* charEnd = null;
	}
}
