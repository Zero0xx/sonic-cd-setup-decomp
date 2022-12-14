using System;
using System.Runtime.CompilerServices;

namespace System.Text
{
	// Token: 0x02000426 RID: 1062
	[Serializable]
	public sealed class UTF32Encoding : Encoding
	{
		// Token: 0x06002B67 RID: 11111 RVA: 0x0008EA9C File Offset: 0x0008DA9C
		public UTF32Encoding() : this(false, true, false)
		{
		}

		// Token: 0x06002B68 RID: 11112 RVA: 0x0008EAA7 File Offset: 0x0008DAA7
		public UTF32Encoding(bool bigEndian, bool byteOrderMark) : this(bigEndian, byteOrderMark, false)
		{
		}

		// Token: 0x06002B69 RID: 11113 RVA: 0x0008EAB2 File Offset: 0x0008DAB2
		public UTF32Encoding(bool bigEndian, bool byteOrderMark, bool throwOnInvalidCharacters) : base(bigEndian ? 12001 : 12000)
		{
			this.bigEndian = bigEndian;
			this.emitUTF32ByteOrderMark = byteOrderMark;
			this.isThrowException = throwOnInvalidCharacters;
			if (this.isThrowException)
			{
				this.SetDefaultFallbacks();
			}
		}

		// Token: 0x06002B6A RID: 11114 RVA: 0x0008EAEC File Offset: 0x0008DAEC
		internal override void SetDefaultFallbacks()
		{
			if (this.isThrowException)
			{
				this.encoderFallback = EncoderFallback.ExceptionFallback;
				this.decoderFallback = DecoderFallback.ExceptionFallback;
				return;
			}
			this.encoderFallback = new EncoderReplacementFallback("�");
			this.decoderFallback = new DecoderReplacementFallback("�");
		}

		// Token: 0x06002B6B RID: 11115 RVA: 0x0008EB38 File Offset: 0x0008DB38
		public unsafe override int GetByteCount(char[] chars, int index, int count)
		{
			if (chars == null)
			{
				throw new ArgumentNullException("chars", Environment.GetResourceString("ArgumentNull_Array"));
			}
			if (index < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (chars.Length - index < count)
			{
				throw new ArgumentOutOfRangeException("chars", Environment.GetResourceString("ArgumentOutOfRange_IndexCountBuffer"));
			}
			if (chars.Length == 0)
			{
				return 0;
			}
			fixed (char* ptr = chars)
			{
				return this.GetByteCount(ptr + index, count, null);
			}
		}

		// Token: 0x06002B6C RID: 11116 RVA: 0x0008EBD4 File Offset: 0x0008DBD4
		public unsafe override int GetByteCount(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			IntPtr intPtr2;
			IntPtr intPtr = intPtr2 = s;
			if (intPtr != 0)
			{
				intPtr2 = (IntPtr)((int)intPtr + RuntimeHelpers.OffsetToStringData);
			}
			char* chars = intPtr2;
			return this.GetByteCount(chars, s.Length, null);
		}

		// Token: 0x06002B6D RID: 11117 RVA: 0x0008EC0F File Offset: 0x0008DC0F
		[CLSCompliant(false)]
		public unsafe override int GetByteCount(char* chars, int count)
		{
			if (chars == null)
			{
				throw new ArgumentNullException("chars", Environment.GetResourceString("ArgumentNull_Array"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			return this.GetByteCount(chars, count, null);
		}

		// Token: 0x06002B6E RID: 11118 RVA: 0x0008EC50 File Offset: 0x0008DC50
		public unsafe override int GetBytes(string s, int charIndex, int charCount, byte[] bytes, int byteIndex)
		{
			if (s == null || bytes == null)
			{
				throw new ArgumentNullException((s == null) ? "s" : "bytes", Environment.GetResourceString("ArgumentNull_Array"));
			}
			if (charIndex < 0 || charCount < 0)
			{
				throw new ArgumentOutOfRangeException((charIndex < 0) ? "charIndex" : "charCount", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (s.Length - charIndex < charCount)
			{
				throw new ArgumentOutOfRangeException("s", Environment.GetResourceString("ArgumentOutOfRange_IndexCount"));
			}
			if (byteIndex < 0 || byteIndex > bytes.Length)
			{
				throw new ArgumentOutOfRangeException("byteIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			int byteCount = bytes.Length - byteIndex;
			if (bytes.Length == 0)
			{
				bytes = new byte[1];
			}
			IntPtr intPtr2;
			IntPtr intPtr = intPtr2 = s;
			if (intPtr != 0)
			{
				intPtr2 = (IntPtr)((int)intPtr + RuntimeHelpers.OffsetToStringData);
			}
			char* ptr = intPtr2;
			fixed (byte* ptr2 = bytes)
			{
				return this.GetBytes(ptr + charIndex, charCount, ptr2 + byteIndex, byteCount, null);
			}
		}

		// Token: 0x06002B6F RID: 11119 RVA: 0x0008ED48 File Offset: 0x0008DD48
		public unsafe override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
		{
			if (chars == null || bytes == null)
			{
				throw new ArgumentNullException((chars == null) ? "chars" : "bytes", Environment.GetResourceString("ArgumentNull_Array"));
			}
			if (charIndex < 0 || charCount < 0)
			{
				throw new ArgumentOutOfRangeException((charIndex < 0) ? "charIndex" : "charCount", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (chars.Length - charIndex < charCount)
			{
				throw new ArgumentOutOfRangeException("chars", Environment.GetResourceString("ArgumentOutOfRange_IndexCountBuffer"));
			}
			if (byteIndex < 0 || byteIndex > bytes.Length)
			{
				throw new ArgumentOutOfRangeException("byteIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			if (chars.Length == 0)
			{
				return 0;
			}
			int byteCount = bytes.Length - byteIndex;
			if (bytes.Length == 0)
			{
				bytes = new byte[1];
			}
			fixed (char* ptr = chars)
			{
				fixed (byte* ptr2 = bytes)
				{
					return this.GetBytes(ptr + charIndex, charCount, ptr2 + byteIndex, byteCount, null);
				}
			}
		}

		// Token: 0x06002B70 RID: 11120 RVA: 0x0008EE50 File Offset: 0x0008DE50
		[CLSCompliant(false)]
		public unsafe override int GetBytes(char* chars, int charCount, byte* bytes, int byteCount)
		{
			if (bytes == null || chars == null)
			{
				throw new ArgumentNullException((bytes == null) ? "bytes" : "chars", Environment.GetResourceString("ArgumentNull_Array"));
			}
			if (charCount < 0 || byteCount < 0)
			{
				throw new ArgumentOutOfRangeException((charCount < 0) ? "charCount" : "byteCount", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			return this.GetBytes(chars, charCount, bytes, byteCount, null);
		}

		// Token: 0x06002B71 RID: 11121 RVA: 0x0008EEC0 File Offset: 0x0008DEC0
		public unsafe override int GetCharCount(byte[] bytes, int index, int count)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes", Environment.GetResourceString("ArgumentNull_Array"));
			}
			if (index < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (bytes.Length - index < count)
			{
				throw new ArgumentOutOfRangeException("bytes", Environment.GetResourceString("ArgumentOutOfRange_IndexCountBuffer"));
			}
			if (bytes.Length == 0)
			{
				return 0;
			}
			fixed (byte* ptr = bytes)
			{
				return this.GetCharCount(ptr + index, count, null);
			}
		}

		// Token: 0x06002B72 RID: 11122 RVA: 0x0008EF58 File Offset: 0x0008DF58
		[CLSCompliant(false)]
		public unsafe override int GetCharCount(byte* bytes, int count)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes", Environment.GetResourceString("ArgumentNull_Array"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			return this.GetCharCount(bytes, count, null);
		}

		// Token: 0x06002B73 RID: 11123 RVA: 0x0008EF98 File Offset: 0x0008DF98
		public unsafe override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
		{
			if (bytes == null || chars == null)
			{
				throw new ArgumentNullException((bytes == null) ? "bytes" : "chars", Environment.GetResourceString("ArgumentNull_Array"));
			}
			if (byteIndex < 0 || byteCount < 0)
			{
				throw new ArgumentOutOfRangeException((byteIndex < 0) ? "byteIndex" : "byteCount", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (bytes.Length - byteIndex < byteCount)
			{
				throw new ArgumentOutOfRangeException("bytes", Environment.GetResourceString("ArgumentOutOfRange_IndexCountBuffer"));
			}
			if (charIndex < 0 || charIndex > chars.Length)
			{
				throw new ArgumentOutOfRangeException("charIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			if (bytes.Length == 0)
			{
				return 0;
			}
			int charCount = chars.Length - charIndex;
			if (chars.Length == 0)
			{
				chars = new char[1];
			}
			fixed (byte* ptr = bytes)
			{
				fixed (char* ptr2 = chars)
				{
					return this.GetChars(ptr + byteIndex, byteCount, ptr2 + charIndex, charCount, null);
				}
			}
		}

		// Token: 0x06002B74 RID: 11124 RVA: 0x0008F0A0 File Offset: 0x0008E0A0
		[CLSCompliant(false)]
		public unsafe override int GetChars(byte* bytes, int byteCount, char* chars, int charCount)
		{
			if (bytes == null || chars == null)
			{
				throw new ArgumentNullException((bytes == null) ? "bytes" : "chars", Environment.GetResourceString("ArgumentNull_Array"));
			}
			if (charCount < 0 || byteCount < 0)
			{
				throw new ArgumentOutOfRangeException((charCount < 0) ? "charCount" : "byteCount", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			return this.GetChars(bytes, byteCount, chars, charCount, null);
		}

		// Token: 0x06002B75 RID: 11125 RVA: 0x0008F110 File Offset: 0x0008E110
		public unsafe override string GetString(byte[] bytes, int index, int count)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes", Environment.GetResourceString("ArgumentNull_Array"));
			}
			if (index < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (bytes.Length - index < count)
			{
				throw new ArgumentOutOfRangeException("bytes", Environment.GetResourceString("ArgumentOutOfRange_IndexCountBuffer"));
			}
			if (bytes.Length == 0)
			{
				return string.Empty;
			}
			fixed (byte* ptr = bytes)
			{
				return string.CreateStringFromEncoding(ptr + index, count, this);
			}
		}

		// Token: 0x06002B76 RID: 11126 RVA: 0x0008F1AC File Offset: 0x0008E1AC
		internal unsafe override int GetByteCount(char* chars, int count, EncoderNLS encoder)
		{
			char* ptr = chars + count;
			char* charStart = chars;
			int num = 0;
			char c = '\0';
			EncoderFallbackBuffer encoderFallbackBuffer;
			if (encoder != null)
			{
				c = encoder.charLeftOver;
				encoderFallbackBuffer = encoder.FallbackBuffer;
				if (encoderFallbackBuffer.Remaining > 0)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_EncoderFallbackNotEmpty", new object[]
					{
						this.EncodingName,
						encoder.Fallback.GetType()
					}));
				}
			}
			else
			{
				encoderFallbackBuffer = this.encoderFallback.CreateFallbackBuffer();
			}
			encoderFallbackBuffer.InternalInitialize(charStart, ptr, encoder, false);
			for (;;)
			{
				char c2;
				if ((c2 = encoderFallbackBuffer.InternalGetNextChar()) == '\0' && chars >= ptr)
				{
					if ((encoder != null && !encoder.MustFlush) || c <= '\0')
					{
						break;
					}
					encoderFallbackBuffer.InternalFallback(c, ref chars);
					c = '\0';
				}
				else
				{
					if (c2 == '\0')
					{
						c2 = *chars;
						chars++;
					}
					if (c != '\0')
					{
						if (char.IsLowSurrogate(c2))
						{
							c = '\0';
							num += 4;
						}
						else
						{
							chars--;
							encoderFallbackBuffer.InternalFallback(c, ref chars);
							c = '\0';
						}
					}
					else if (char.IsHighSurrogate(c2))
					{
						c = c2;
					}
					else if (char.IsLowSurrogate(c2))
					{
						encoderFallbackBuffer.InternalFallback(c2, ref chars);
					}
					else
					{
						num += 4;
					}
				}
			}
			if (num < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_GetByteCountOverflow"));
			}
			return num;
		}

		// Token: 0x06002B77 RID: 11127 RVA: 0x0008F2DC File Offset: 0x0008E2DC
		internal unsafe override int GetBytes(char* chars, int charCount, byte* bytes, int byteCount, EncoderNLS encoder)
		{
			char* ptr = chars;
			char* ptr2 = chars + charCount;
			byte* ptr3 = bytes;
			byte* ptr4 = bytes + byteCount;
			char c = '\0';
			EncoderFallbackBuffer encoderFallbackBuffer;
			if (encoder != null)
			{
				c = encoder.charLeftOver;
				encoderFallbackBuffer = encoder.FallbackBuffer;
				if (encoder.m_throwOnOverflow && encoderFallbackBuffer.Remaining > 0)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_EncoderFallbackNotEmpty", new object[]
					{
						this.EncodingName,
						encoder.Fallback.GetType()
					}));
				}
			}
			else
			{
				encoderFallbackBuffer = this.encoderFallback.CreateFallbackBuffer();
			}
			encoderFallbackBuffer.InternalInitialize(ptr, ptr2, encoder, true);
			for (;;)
			{
				char c2;
				if ((c2 = encoderFallbackBuffer.InternalGetNextChar()) != '\0' || chars < ptr2)
				{
					if (c2 == '\0')
					{
						c2 = *chars;
						chars++;
					}
					if (c != '\0')
					{
						if (!char.IsLowSurrogate(c2))
						{
							chars--;
							encoderFallbackBuffer.InternalFallback(c, ref chars);
							c = '\0';
							continue;
						}
						uint surrogate = this.GetSurrogate(c, c2);
						c = '\0';
						if (bytes + 3 >= ptr4)
						{
							if (encoderFallbackBuffer.bFallingBack)
							{
								encoderFallbackBuffer.MovePrevious();
								encoderFallbackBuffer.MovePrevious();
							}
							else
							{
								chars -= 2;
							}
							base.ThrowBytesOverflow(encoder, bytes == ptr3);
							c = '\0';
						}
						else
						{
							if (this.bigEndian)
							{
								*(bytes++) = 0;
								*(bytes++) = (byte)(surrogate >> 16);
								*(bytes++) = (byte)(surrogate >> 8);
								*(bytes++) = (byte)surrogate;
								continue;
							}
							*(bytes++) = (byte)surrogate;
							*(bytes++) = (byte)(surrogate >> 8);
							*(bytes++) = (byte)(surrogate >> 16);
							*(bytes++) = 0;
							continue;
						}
					}
					else
					{
						if (char.IsHighSurrogate(c2))
						{
							c = c2;
							continue;
						}
						if (char.IsLowSurrogate(c2))
						{
							encoderFallbackBuffer.InternalFallback(c2, ref chars);
							continue;
						}
						if (bytes + 3 >= ptr4)
						{
							if (encoderFallbackBuffer.bFallingBack)
							{
								encoderFallbackBuffer.MovePrevious();
							}
							else
							{
								chars--;
							}
							base.ThrowBytesOverflow(encoder, bytes == ptr3);
						}
						else
						{
							if (this.bigEndian)
							{
								*(bytes++) = 0;
								*(bytes++) = 0;
								*(bytes++) = (byte)(c2 >> 8);
								*(bytes++) = (byte)c2;
								continue;
							}
							*(bytes++) = (byte)c2;
							*(bytes++) = (byte)(c2 >> 8);
							*(bytes++) = 0;
							*(bytes++) = 0;
							continue;
						}
					}
				}
				if ((encoder != null && !encoder.MustFlush) || c <= '\0')
				{
					break;
				}
				encoderFallbackBuffer.InternalFallback(c, ref chars);
				c = '\0';
			}
			if (encoder != null)
			{
				encoder.charLeftOver = c;
				encoder.m_charsUsed = (int)((long)(chars - ptr));
			}
			return (int)((long)(bytes - ptr3));
		}

		// Token: 0x06002B78 RID: 11128 RVA: 0x0008F584 File Offset: 0x0008E584
		internal unsafe override int GetCharCount(byte* bytes, int count, DecoderNLS baseDecoder)
		{
			UTF32Encoding.UTF32Decoder utf32Decoder = (UTF32Encoding.UTF32Decoder)baseDecoder;
			int num = 0;
			byte* ptr = bytes + count;
			byte* byteStart = bytes;
			int i = 0;
			uint num2 = 0U;
			DecoderFallbackBuffer decoderFallbackBuffer;
			if (utf32Decoder != null)
			{
				i = utf32Decoder.readByteCount;
				num2 = (uint)utf32Decoder.iChar;
				decoderFallbackBuffer = utf32Decoder.FallbackBuffer;
			}
			else
			{
				decoderFallbackBuffer = this.decoderFallback.CreateFallbackBuffer();
			}
			decoderFallbackBuffer.InternalInitialize(byteStart, null);
			while (bytes < ptr && num >= 0)
			{
				if (this.bigEndian)
				{
					num2 <<= 8;
					num2 += (uint)(*(bytes++));
				}
				else
				{
					num2 >>= 8;
					num2 += (uint)((uint)(*(bytes++)) << 24);
				}
				i++;
				if (i >= 4)
				{
					i = 0;
					if (num2 > 1114111U || (num2 >= 55296U && num2 <= 57343U))
					{
						byte[] bytes2;
						if (this.bigEndian)
						{
							bytes2 = new byte[]
							{
								(byte)(num2 >> 24),
								(byte)(num2 >> 16),
								(byte)(num2 >> 8),
								(byte)num2
							};
						}
						else
						{
							bytes2 = new byte[]
							{
								(byte)num2,
								(byte)(num2 >> 8),
								(byte)(num2 >> 16),
								(byte)(num2 >> 24)
							};
						}
						num += decoderFallbackBuffer.InternalFallback(bytes2, bytes);
						num2 = 0U;
					}
					else
					{
						if (num2 >= 65536U)
						{
							num++;
						}
						num++;
						num2 = 0U;
					}
				}
			}
			if (i > 0 && (utf32Decoder == null || utf32Decoder.MustFlush))
			{
				byte[] array = new byte[i];
				if (this.bigEndian)
				{
					while (i > 0)
					{
						array[--i] = (byte)num2;
						num2 >>= 8;
					}
				}
				else
				{
					while (i > 0)
					{
						array[--i] = (byte)(num2 >> 24);
						num2 <<= 8;
					}
				}
				num += decoderFallbackBuffer.InternalFallback(array, bytes);
			}
			if (num < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_GetByteCountOverflow"));
			}
			return num;
		}

		// Token: 0x06002B79 RID: 11129 RVA: 0x0008F760 File Offset: 0x0008E760
		internal unsafe override int GetChars(byte* bytes, int byteCount, char* chars, int charCount, DecoderNLS baseDecoder)
		{
			UTF32Encoding.UTF32Decoder utf32Decoder = (UTF32Encoding.UTF32Decoder)baseDecoder;
			char* ptr = chars;
			char* ptr2 = chars + charCount;
			byte* ptr3 = bytes;
			byte* ptr4 = bytes + byteCount;
			int num = 0;
			uint num2 = 0U;
			DecoderFallbackBuffer decoderFallbackBuffer;
			if (utf32Decoder != null)
			{
				num = utf32Decoder.readByteCount;
				num2 = (uint)utf32Decoder.iChar;
				decoderFallbackBuffer = baseDecoder.FallbackBuffer;
			}
			else
			{
				decoderFallbackBuffer = this.decoderFallback.CreateFallbackBuffer();
			}
			decoderFallbackBuffer.InternalInitialize(bytes, chars + charCount);
			while (bytes < ptr4)
			{
				if (this.bigEndian)
				{
					num2 <<= 8;
					num2 += (uint)(*(bytes++));
				}
				else
				{
					num2 >>= 8;
					num2 += (uint)((uint)(*(bytes++)) << 24);
				}
				num++;
				if (num >= 4)
				{
					num = 0;
					if (num2 > 1114111U || (num2 >= 55296U && num2 <= 57343U))
					{
						byte[] bytes2;
						if (this.bigEndian)
						{
							bytes2 = new byte[]
							{
								(byte)(num2 >> 24),
								(byte)(num2 >> 16),
								(byte)(num2 >> 8),
								(byte)num2
							};
						}
						else
						{
							bytes2 = new byte[]
							{
								(byte)num2,
								(byte)(num2 >> 8),
								(byte)(num2 >> 16),
								(byte)(num2 >> 24)
							};
						}
						if (!decoderFallbackBuffer.InternalFallback(bytes2, bytes, ref chars))
						{
							bytes -= 4;
							num2 = 0U;
							decoderFallbackBuffer.InternalReset();
							base.ThrowCharsOverflow(utf32Decoder, chars == ptr);
							break;
						}
						num2 = 0U;
					}
					else
					{
						if (num2 >= 65536U)
						{
							if (chars >= ptr2 - 1)
							{
								bytes -= 4;
								num2 = 0U;
								base.ThrowCharsOverflow(utf32Decoder, chars == ptr);
								break;
							}
							*(chars++) = this.GetHighSurrogate(num2);
							num2 = (uint)this.GetLowSurrogate(num2);
						}
						else if (chars >= ptr2)
						{
							bytes -= 4;
							num2 = 0U;
							base.ThrowCharsOverflow(utf32Decoder, chars == ptr);
							break;
						}
						*(chars++) = (char)num2;
						num2 = 0U;
					}
				}
			}
			if (num > 0 && (utf32Decoder == null || utf32Decoder.MustFlush))
			{
				byte[] array = new byte[num];
				int i = num;
				if (this.bigEndian)
				{
					while (i > 0)
					{
						array[--i] = (byte)num2;
						num2 >>= 8;
					}
				}
				else
				{
					while (i > 0)
					{
						array[--i] = (byte)(num2 >> 24);
						num2 <<= 8;
					}
				}
				if (!decoderFallbackBuffer.InternalFallback(array, bytes, ref chars))
				{
					decoderFallbackBuffer.InternalReset();
					base.ThrowCharsOverflow(utf32Decoder, chars == ptr);
				}
				else
				{
					num = 0;
					num2 = 0U;
				}
			}
			if (utf32Decoder != null)
			{
				utf32Decoder.iChar = (int)num2;
				utf32Decoder.readByteCount = num;
				utf32Decoder.m_bytesUsed = (int)((long)(bytes - ptr3));
			}
			return (int)((long)(chars - ptr));
		}

		// Token: 0x06002B7A RID: 11130 RVA: 0x0008F9EA File Offset: 0x0008E9EA
		private uint GetSurrogate(char cHigh, char cLow)
		{
			return (uint)((cHigh - '\ud800') * 'Ѐ' + (cLow - '\udc00')) + 65536U;
		}

		// Token: 0x06002B7B RID: 11131 RVA: 0x0008FA07 File Offset: 0x0008EA07
		private char GetHighSurrogate(uint iChar)
		{
			return (char)((iChar - 65536U) / 1024U + 55296U);
		}

		// Token: 0x06002B7C RID: 11132 RVA: 0x0008FA1D File Offset: 0x0008EA1D
		private char GetLowSurrogate(uint iChar)
		{
			return (char)((iChar - 65536U) % 1024U + 56320U);
		}

		// Token: 0x06002B7D RID: 11133 RVA: 0x0008FA33 File Offset: 0x0008EA33
		public override Decoder GetDecoder()
		{
			return new UTF32Encoding.UTF32Decoder(this);
		}

		// Token: 0x06002B7E RID: 11134 RVA: 0x0008FA3B File Offset: 0x0008EA3B
		public override Encoder GetEncoder()
		{
			return new EncoderNLS(this);
		}

		// Token: 0x06002B7F RID: 11135 RVA: 0x0008FA44 File Offset: 0x0008EA44
		public override int GetMaxByteCount(int charCount)
		{
			if (charCount < 0)
			{
				throw new ArgumentOutOfRangeException("charCount", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			long num = (long)charCount + 1L;
			if (base.EncoderFallback.MaxCharCount > 1)
			{
				num *= (long)base.EncoderFallback.MaxCharCount;
			}
			num *= 4L;
			if (num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("charCount", Environment.GetResourceString("ArgumentOutOfRange_GetByteCountOverflow"));
			}
			return (int)num;
		}

		// Token: 0x06002B80 RID: 11136 RVA: 0x0008FAB4 File Offset: 0x0008EAB4
		public override int GetMaxCharCount(int byteCount)
		{
			if (byteCount < 0)
			{
				throw new ArgumentOutOfRangeException("byteCount", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			int num = byteCount / 2 + 2;
			if (base.DecoderFallback.MaxCharCount > 2)
			{
				num *= base.DecoderFallback.MaxCharCount;
				num /= 2;
			}
			if (num > 2147483647)
			{
				throw new ArgumentOutOfRangeException("byteCount", Environment.GetResourceString("ArgumentOutOfRange_GetCharCountOverflow"));
			}
			return num;
		}

		// Token: 0x06002B81 RID: 11137 RVA: 0x0008FB20 File Offset: 0x0008EB20
		public override byte[] GetPreamble()
		{
			if (!this.emitUTF32ByteOrderMark)
			{
				return Encoding.emptyByteArray;
			}
			if (this.bigEndian)
			{
				return new byte[]
				{
					0,
					0,
					254,
					byte.MaxValue
				};
			}
			byte[] array = new byte[4];
			array[0] = byte.MaxValue;
			array[1] = 254;
			return array;
		}

		// Token: 0x06002B82 RID: 11138 RVA: 0x0008FB74 File Offset: 0x0008EB74
		public override bool Equals(object value)
		{
			UTF32Encoding utf32Encoding = value as UTF32Encoding;
			return utf32Encoding != null && (this.emitUTF32ByteOrderMark == utf32Encoding.emitUTF32ByteOrderMark && this.bigEndian == utf32Encoding.bigEndian && base.EncoderFallback.Equals(utf32Encoding.EncoderFallback)) && base.DecoderFallback.Equals(utf32Encoding.DecoderFallback);
		}

		// Token: 0x06002B83 RID: 11139 RVA: 0x0008FBCF File Offset: 0x0008EBCF
		public override int GetHashCode()
		{
			return base.EncoderFallback.GetHashCode() + base.DecoderFallback.GetHashCode() + this.CodePage + (this.emitUTF32ByteOrderMark ? 4 : 0) + (this.bigEndian ? 8 : 0);
		}

		// Token: 0x04001531 RID: 5425
		private bool emitUTF32ByteOrderMark;

		// Token: 0x04001532 RID: 5426
		private bool isThrowException;

		// Token: 0x04001533 RID: 5427
		private bool bigEndian;

		// Token: 0x02000427 RID: 1063
		[Serializable]
		internal class UTF32Decoder : DecoderNLS
		{
			// Token: 0x06002B84 RID: 11140 RVA: 0x0008FC09 File Offset: 0x0008EC09
			public UTF32Decoder(UTF32Encoding encoding) : base(encoding)
			{
			}

			// Token: 0x06002B85 RID: 11141 RVA: 0x0008FC12 File Offset: 0x0008EC12
			public override void Reset()
			{
				this.iChar = 0;
				this.readByteCount = 0;
				if (this.m_fallbackBuffer != null)
				{
					this.m_fallbackBuffer.Reset();
				}
			}

			// Token: 0x17000816 RID: 2070
			// (get) Token: 0x06002B86 RID: 11142 RVA: 0x0008FC35 File Offset: 0x0008EC35
			internal override bool HasState
			{
				get
				{
					return this.readByteCount != 0;
				}
			}

			// Token: 0x04001534 RID: 5428
			internal int iChar;

			// Token: 0x04001535 RID: 5429
			internal int readByteCount;
		}
	}
}
