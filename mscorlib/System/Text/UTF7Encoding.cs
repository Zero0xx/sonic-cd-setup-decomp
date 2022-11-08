using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Text
{
	// Token: 0x0200041E RID: 1054
	[ComVisible(true)]
	[Serializable]
	public class UTF7Encoding : Encoding
	{
		// Token: 0x06002B0C RID: 11020 RVA: 0x0008BB4A File Offset: 0x0008AB4A
		public UTF7Encoding() : this(false)
		{
		}

		// Token: 0x06002B0D RID: 11021 RVA: 0x0008BB53 File Offset: 0x0008AB53
		public UTF7Encoding(bool allowOptionals) : base(65000)
		{
			this.m_allowOptionals = allowOptionals;
			this.MakeTables();
		}

		// Token: 0x06002B0E RID: 11022 RVA: 0x0008BB70 File Offset: 0x0008AB70
		private void MakeTables()
		{
			this.base64Bytes = new byte[64];
			for (int i = 0; i < 64; i++)
			{
				this.base64Bytes[i] = (byte)"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/"[i];
			}
			this.base64Values = new sbyte[128];
			for (int j = 0; j < 128; j++)
			{
				this.base64Values[j] = -1;
			}
			for (int k = 0; k < 64; k++)
			{
				this.base64Values[(int)this.base64Bytes[k]] = (sbyte)k;
			}
			this.directEncode = new bool[128];
			int length = "\t\n\r '(),-./0123456789:?ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".Length;
			for (int l = 0; l < length; l++)
			{
				this.directEncode[(int)"\t\n\r '(),-./0123456789:?ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"[l]] = true;
			}
			if (this.m_allowOptionals)
			{
				length = "!\"#$%&*;<=>@[]^_`{|}".Length;
				for (int m = 0; m < length; m++)
				{
					this.directEncode[(int)"!\"#$%&*;<=>@[]^_`{|}"[m]] = true;
				}
			}
		}

		// Token: 0x06002B0F RID: 11023 RVA: 0x0008BC68 File Offset: 0x0008AC68
		internal override void SetDefaultFallbacks()
		{
			this.encoderFallback = new EncoderReplacementFallback(string.Empty);
			this.decoderFallback = new UTF7Encoding.DecoderUTF7Fallback();
		}

		// Token: 0x06002B10 RID: 11024 RVA: 0x0008BC85 File Offset: 0x0008AC85
		[OnDeserializing]
		private void OnDeserializing(StreamingContext ctx)
		{
			base.OnDeserializing();
		}

		// Token: 0x06002B11 RID: 11025 RVA: 0x0008BC8D File Offset: 0x0008AC8D
		[OnDeserialized]
		private void OnDeserialized(StreamingContext ctx)
		{
			base.OnDeserialized();
			if (this.m_deserializedFromEverett)
			{
				this.m_allowOptionals = this.directEncode[(int)"!\"#$%&*;<=>@[]^_`{|}"[0]];
			}
			this.MakeTables();
		}

		// Token: 0x06002B12 RID: 11026 RVA: 0x0008BCBC File Offset: 0x0008ACBC
		[ComVisible(false)]
		public override bool Equals(object value)
		{
			UTF7Encoding utf7Encoding = value as UTF7Encoding;
			return utf7Encoding != null && (this.m_allowOptionals == utf7Encoding.m_allowOptionals && base.EncoderFallback.Equals(utf7Encoding.EncoderFallback)) && base.DecoderFallback.Equals(utf7Encoding.DecoderFallback);
		}

		// Token: 0x06002B13 RID: 11027 RVA: 0x0008BD09 File Offset: 0x0008AD09
		[ComVisible(false)]
		public override int GetHashCode()
		{
			return this.CodePage + base.EncoderFallback.GetHashCode() + base.DecoderFallback.GetHashCode();
		}

		// Token: 0x06002B14 RID: 11028 RVA: 0x0008BD2C File Offset: 0x0008AD2C
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

		// Token: 0x06002B15 RID: 11029 RVA: 0x0008BDC8 File Offset: 0x0008ADC8
		[ComVisible(false)]
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

		// Token: 0x06002B16 RID: 11030 RVA: 0x0008BE03 File Offset: 0x0008AE03
		[CLSCompliant(false)]
		[ComVisible(false)]
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

		// Token: 0x06002B17 RID: 11031 RVA: 0x0008BE44 File Offset: 0x0008AE44
		[ComVisible(false)]
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

		// Token: 0x06002B18 RID: 11032 RVA: 0x0008BF3C File Offset: 0x0008AF3C
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

		// Token: 0x06002B19 RID: 11033 RVA: 0x0008C044 File Offset: 0x0008B044
		[CLSCompliant(false)]
		[ComVisible(false)]
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

		// Token: 0x06002B1A RID: 11034 RVA: 0x0008C0B4 File Offset: 0x0008B0B4
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

		// Token: 0x06002B1B RID: 11035 RVA: 0x0008C14C File Offset: 0x0008B14C
		[CLSCompliant(false)]
		[ComVisible(false)]
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

		// Token: 0x06002B1C RID: 11036 RVA: 0x0008C18C File Offset: 0x0008B18C
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

		// Token: 0x06002B1D RID: 11037 RVA: 0x0008C294 File Offset: 0x0008B294
		[CLSCompliant(false)]
		[ComVisible(false)]
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

		// Token: 0x06002B1E RID: 11038 RVA: 0x0008C304 File Offset: 0x0008B304
		[ComVisible(false)]
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

		// Token: 0x06002B1F RID: 11039 RVA: 0x0008C39F File Offset: 0x0008B39F
		internal unsafe override int GetByteCount(char* chars, int count, EncoderNLS baseEncoder)
		{
			return this.GetBytes(chars, count, null, 0, baseEncoder);
		}

		// Token: 0x06002B20 RID: 11040 RVA: 0x0008C3B0 File Offset: 0x0008B3B0
		internal unsafe override int GetBytes(char* chars, int charCount, byte* bytes, int byteCount, EncoderNLS baseEncoder)
		{
			UTF7Encoding.Encoder encoder = (UTF7Encoding.Encoder)baseEncoder;
			int num = 0;
			int i = -1;
			Encoding.EncodingByteBuffer encodingByteBuffer = new Encoding.EncodingByteBuffer(this, encoder, bytes, byteCount, chars, charCount);
			if (encoder != null)
			{
				num = encoder.bits;
				i = encoder.bitCount;
				while (i >= 6)
				{
					i -= 6;
					if (!encodingByteBuffer.AddByte(this.base64Bytes[num >> i & 63]))
					{
						base.ThrowBytesOverflow(encoder, encodingByteBuffer.Count == 0);
					}
				}
			}
			while (encodingByteBuffer.MoreData)
			{
				char nextChar = encodingByteBuffer.GetNextChar();
				if (nextChar < '\u0080' && this.directEncode[(int)nextChar])
				{
					if (i >= 0)
					{
						if (i > 0)
						{
							if (!encodingByteBuffer.AddByte(this.base64Bytes[num << 6 - i & 63]))
							{
								break;
							}
							i = 0;
						}
						if (!encodingByteBuffer.AddByte(45))
						{
							break;
						}
						i = -1;
					}
					if (!encodingByteBuffer.AddByte((byte)nextChar))
					{
						break;
					}
				}
				else if (i < 0 && nextChar == '+')
				{
					if (!encodingByteBuffer.AddByte(43, 45))
					{
						break;
					}
				}
				else
				{
					if (i < 0)
					{
						if (!encodingByteBuffer.AddByte(43))
						{
							break;
						}
						i = 0;
					}
					num = (num << 16 | (int)nextChar);
					i += 16;
					while (i >= 6)
					{
						i -= 6;
						if (!encodingByteBuffer.AddByte(this.base64Bytes[num >> i & 63]))
						{
							i += 6;
							nextChar = encodingByteBuffer.GetNextChar();
							break;
						}
					}
					if (i >= 6)
					{
						break;
					}
				}
			}
			if (i >= 0 && (encoder == null || encoder.MustFlush))
			{
				if (i > 0 && encodingByteBuffer.AddByte(this.base64Bytes[num << 6 - i & 63]))
				{
					i = 0;
				}
				if (encodingByteBuffer.AddByte(45))
				{
					num = 0;
					i = -1;
				}
				else
				{
					encodingByteBuffer.GetNextChar();
				}
			}
			if (bytes != null && encoder != null)
			{
				encoder.bits = num;
				encoder.bitCount = i;
				encoder.m_charsUsed = encodingByteBuffer.CharsUsed;
			}
			return encodingByteBuffer.Count;
		}

		// Token: 0x06002B21 RID: 11041 RVA: 0x0008C562 File Offset: 0x0008B562
		internal unsafe override int GetCharCount(byte* bytes, int count, DecoderNLS baseDecoder)
		{
			return this.GetChars(bytes, count, null, 0, baseDecoder);
		}

		// Token: 0x06002B22 RID: 11042 RVA: 0x0008C570 File Offset: 0x0008B570
		internal unsafe override int GetChars(byte* bytes, int byteCount, char* chars, int charCount, DecoderNLS baseDecoder)
		{
			UTF7Encoding.Decoder decoder = (UTF7Encoding.Decoder)baseDecoder;
			Encoding.EncodingCharBuffer encodingCharBuffer = new Encoding.EncodingCharBuffer(this, decoder, chars, charCount, bytes, byteCount);
			int num = 0;
			int num2 = -1;
			bool flag = false;
			if (decoder != null)
			{
				num = decoder.bits;
				num2 = decoder.bitCount;
				flag = decoder.firstByte;
			}
			if (num2 >= 16)
			{
				if (!encodingCharBuffer.AddChar((char)(num >> num2 - 16 & 65535)))
				{
					base.ThrowCharsOverflow(decoder, true);
				}
				num2 -= 16;
			}
			while (encodingCharBuffer.MoreData)
			{
				byte nextByte = encodingCharBuffer.GetNextByte();
				int num3;
				if (num2 >= 0)
				{
					sbyte b;
					if (nextByte < 128 && (b = this.base64Values[(int)nextByte]) >= 0)
					{
						flag = false;
						num = (num << 6 | (int)((byte)b));
						num2 += 6;
						if (num2 < 16)
						{
							continue;
						}
						num3 = (num >> num2 - 16 & 65535);
						num2 -= 16;
					}
					else
					{
						num2 = -1;
						if (nextByte != 45)
						{
							if (!encodingCharBuffer.Fallback(nextByte))
							{
								break;
							}
							continue;
						}
						else
						{
							if (!flag)
							{
								continue;
							}
							num3 = 43;
						}
					}
				}
				else
				{
					if (nextByte == 43)
					{
						num2 = 0;
						flag = true;
						continue;
					}
					if (nextByte >= 128)
					{
						if (!encodingCharBuffer.Fallback(nextByte))
						{
							break;
						}
						continue;
					}
					else
					{
						num3 = (int)nextByte;
					}
				}
				if (num3 >= 0 && !encodingCharBuffer.AddChar((char)num3))
				{
					if (num2 >= 0)
					{
						encodingCharBuffer.AdjustBytes(1);
						num2 += 16;
						break;
					}
					break;
				}
			}
			if (chars != null && decoder != null)
			{
				if (decoder.MustFlush)
				{
					decoder.bits = 0;
					decoder.bitCount = -1;
					decoder.firstByte = false;
				}
				else
				{
					decoder.bits = num;
					decoder.bitCount = num2;
					decoder.firstByte = flag;
				}
				decoder.m_bytesUsed = encodingCharBuffer.BytesUsed;
			}
			return encodingCharBuffer.Count;
		}

		// Token: 0x06002B23 RID: 11043 RVA: 0x0008C6F4 File Offset: 0x0008B6F4
		public override System.Text.Decoder GetDecoder()
		{
			return new UTF7Encoding.Decoder(this);
		}

		// Token: 0x06002B24 RID: 11044 RVA: 0x0008C6FC File Offset: 0x0008B6FC
		public override System.Text.Encoder GetEncoder()
		{
			return new UTF7Encoding.Encoder(this);
		}

		// Token: 0x06002B25 RID: 11045 RVA: 0x0008C704 File Offset: 0x0008B704
		public override int GetMaxByteCount(int charCount)
		{
			if (charCount < 0)
			{
				throw new ArgumentOutOfRangeException("charCount", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			long num = (long)charCount * 3L + 2L;
			if (num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("charCount", Environment.GetResourceString("ArgumentOutOfRange_GetByteCountOverflow"));
			}
			return (int)num;
		}

		// Token: 0x06002B26 RID: 11046 RVA: 0x0008C754 File Offset: 0x0008B754
		public override int GetMaxCharCount(int byteCount)
		{
			if (byteCount < 0)
			{
				throw new ArgumentOutOfRangeException("byteCount", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			int num = byteCount;
			if (num == 0)
			{
				num = 1;
			}
			return num;
		}

		// Token: 0x04001519 RID: 5401
		private const string base64Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

		// Token: 0x0400151A RID: 5402
		private const string directChars = "\t\n\r '(),-./0123456789:?ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

		// Token: 0x0400151B RID: 5403
		private const string optionalChars = "!\"#$%&*;<=>@[]^_`{|}";

		// Token: 0x0400151C RID: 5404
		private const int UTF7_CODEPAGE = 65000;

		// Token: 0x0400151D RID: 5405
		private byte[] base64Bytes;

		// Token: 0x0400151E RID: 5406
		private sbyte[] base64Values;

		// Token: 0x0400151F RID: 5407
		private bool[] directEncode;

		// Token: 0x04001520 RID: 5408
		[OptionalField(VersionAdded = 2)]
		private bool m_allowOptionals;

		// Token: 0x0200041F RID: 1055
		[Serializable]
		private class Decoder : DecoderNLS, ISerializable
		{
			// Token: 0x06002B27 RID: 11047 RVA: 0x0008C782 File Offset: 0x0008B782
			public Decoder(UTF7Encoding encoding) : base(encoding)
			{
			}

			// Token: 0x06002B28 RID: 11048 RVA: 0x0008C78C File Offset: 0x0008B78C
			internal Decoder(SerializationInfo info, StreamingContext context)
			{
				if (info == null)
				{
					throw new ArgumentNullException("info");
				}
				this.bits = (int)info.GetValue("bits", typeof(int));
				this.bitCount = (int)info.GetValue("bitCount", typeof(int));
				this.firstByte = (bool)info.GetValue("firstByte", typeof(bool));
				this.m_encoding = (Encoding)info.GetValue("encoding", typeof(Encoding));
			}

			// Token: 0x06002B29 RID: 11049 RVA: 0x0008C830 File Offset: 0x0008B830
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
			void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
			{
				if (info == null)
				{
					throw new ArgumentNullException("info");
				}
				info.AddValue("encoding", this.m_encoding);
				info.AddValue("bits", this.bits);
				info.AddValue("bitCount", this.bitCount);
				info.AddValue("firstByte", this.firstByte);
			}

			// Token: 0x06002B2A RID: 11050 RVA: 0x0008C88F File Offset: 0x0008B88F
			public override void Reset()
			{
				this.bits = 0;
				this.bitCount = -1;
				this.firstByte = false;
				if (this.m_fallbackBuffer != null)
				{
					this.m_fallbackBuffer.Reset();
				}
			}

			// Token: 0x17000810 RID: 2064
			// (get) Token: 0x06002B2B RID: 11051 RVA: 0x0008C8B9 File Offset: 0x0008B8B9
			internal override bool HasState
			{
				get
				{
					return this.bitCount != -1;
				}
			}

			// Token: 0x04001521 RID: 5409
			internal int bits;

			// Token: 0x04001522 RID: 5410
			internal int bitCount;

			// Token: 0x04001523 RID: 5411
			internal bool firstByte;
		}

		// Token: 0x02000420 RID: 1056
		[Serializable]
		private class Encoder : EncoderNLS, ISerializable
		{
			// Token: 0x06002B2C RID: 11052 RVA: 0x0008C8C7 File Offset: 0x0008B8C7
			public Encoder(UTF7Encoding encoding) : base(encoding)
			{
			}

			// Token: 0x06002B2D RID: 11053 RVA: 0x0008C8D0 File Offset: 0x0008B8D0
			internal Encoder(SerializationInfo info, StreamingContext context)
			{
				if (info == null)
				{
					throw new ArgumentNullException("info");
				}
				this.bits = (int)info.GetValue("bits", typeof(int));
				this.bitCount = (int)info.GetValue("bitCount", typeof(int));
				this.m_encoding = (Encoding)info.GetValue("encoding", typeof(Encoding));
			}

			// Token: 0x06002B2E RID: 11054 RVA: 0x0008C954 File Offset: 0x0008B954
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
			void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
			{
				if (info == null)
				{
					throw new ArgumentNullException("info");
				}
				info.AddValue("encoding", this.m_encoding);
				info.AddValue("bits", this.bits);
				info.AddValue("bitCount", this.bitCount);
			}

			// Token: 0x06002B2F RID: 11055 RVA: 0x0008C9A2 File Offset: 0x0008B9A2
			public override void Reset()
			{
				this.bitCount = -1;
				this.bits = 0;
				if (this.m_fallbackBuffer != null)
				{
					this.m_fallbackBuffer.Reset();
				}
			}

			// Token: 0x17000811 RID: 2065
			// (get) Token: 0x06002B30 RID: 11056 RVA: 0x0008C9C5 File Offset: 0x0008B9C5
			internal override bool HasState
			{
				get
				{
					return this.bits != 0 || this.bitCount != -1;
				}
			}

			// Token: 0x04001524 RID: 5412
			internal int bits;

			// Token: 0x04001525 RID: 5413
			internal int bitCount;
		}

		// Token: 0x02000421 RID: 1057
		[Serializable]
		internal sealed class DecoderUTF7Fallback : DecoderFallback
		{
			// Token: 0x06002B32 RID: 11058 RVA: 0x0008C9E5 File Offset: 0x0008B9E5
			public override DecoderFallbackBuffer CreateFallbackBuffer()
			{
				return new UTF7Encoding.DecoderUTF7FallbackBuffer(this);
			}

			// Token: 0x17000812 RID: 2066
			// (get) Token: 0x06002B33 RID: 11059 RVA: 0x0008C9ED File Offset: 0x0008B9ED
			public override int MaxCharCount
			{
				get
				{
					return 1;
				}
			}

			// Token: 0x06002B34 RID: 11060 RVA: 0x0008C9F0 File Offset: 0x0008B9F0
			public override bool Equals(object value)
			{
				return value is UTF7Encoding.DecoderUTF7Fallback;
			}

			// Token: 0x06002B35 RID: 11061 RVA: 0x0008CA0A File Offset: 0x0008BA0A
			public override int GetHashCode()
			{
				return 984;
			}
		}

		// Token: 0x02000422 RID: 1058
		internal sealed class DecoderUTF7FallbackBuffer : DecoderFallbackBuffer
		{
			// Token: 0x06002B36 RID: 11062 RVA: 0x0008CA11 File Offset: 0x0008BA11
			public DecoderUTF7FallbackBuffer(UTF7Encoding.DecoderUTF7Fallback fallback)
			{
			}

			// Token: 0x06002B37 RID: 11063 RVA: 0x0008CA20 File Offset: 0x0008BA20
			public override bool Fallback(byte[] bytesUnknown, int index)
			{
				this.cFallback = (char)bytesUnknown[0];
				this.iCount = (this.iSize = 1);
				return true;
			}

			// Token: 0x06002B38 RID: 11064 RVA: 0x0008CA48 File Offset: 0x0008BA48
			public override char GetNextChar()
			{
				if (this.iCount-- > 0)
				{
					return this.cFallback;
				}
				return '\0';
			}

			// Token: 0x06002B39 RID: 11065 RVA: 0x0008CA71 File Offset: 0x0008BA71
			public override bool MovePrevious()
			{
				if (this.iCount >= 0)
				{
					this.iCount++;
				}
				return this.iCount >= 0 && this.iCount <= this.iSize;
			}

			// Token: 0x17000813 RID: 2067
			// (get) Token: 0x06002B3A RID: 11066 RVA: 0x0008CAA6 File Offset: 0x0008BAA6
			public override int Remaining
			{
				get
				{
					if (this.iCount <= 0)
					{
						return 0;
					}
					return this.iCount;
				}
			}

			// Token: 0x06002B3B RID: 11067 RVA: 0x0008CAB9 File Offset: 0x0008BAB9
			public override void Reset()
			{
				this.iCount = -1;
				this.byteStart = null;
			}

			// Token: 0x06002B3C RID: 11068 RVA: 0x0008CACA File Offset: 0x0008BACA
			internal unsafe override int InternalFallback(byte[] bytes, byte* pBytes)
			{
				if (bytes[0] != 0)
				{
					return 1;
				}
				return 0;
			}

			// Token: 0x04001526 RID: 5414
			private char cFallback;

			// Token: 0x04001527 RID: 5415
			private int iCount = -1;

			// Token: 0x04001528 RID: 5416
			private int iSize;
		}
	}
}
