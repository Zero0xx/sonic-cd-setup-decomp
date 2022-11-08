using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Threading;

namespace System.Text
{
	// Token: 0x0200041A RID: 1050
	[Serializable]
	internal class SBCSCodePageEncoding : BaseCodePageEncoding, ISerializable
	{
		// Token: 0x06002ADB RID: 10971 RVA: 0x00089168 File Offset: 0x00088168
		public SBCSCodePageEncoding(int codePage) : this(codePage, codePage)
		{
		}

		// Token: 0x06002ADC RID: 10972 RVA: 0x00089172 File Offset: 0x00088172
		internal SBCSCodePageEncoding(int codePage, int dataCodePage)
		{
			this.mapBytesToUnicode = null;
			this.mapUnicodeToBytes = null;
			this.mapCodePageCached = null;
			base..ctor(codePage, dataCodePage);
		}

		// Token: 0x06002ADD RID: 10973 RVA: 0x00089194 File Offset: 0x00088194
		internal SBCSCodePageEncoding(SerializationInfo info, StreamingContext context)
		{
			this.mapBytesToUnicode = null;
			this.mapUnicodeToBytes = null;
			this.mapCodePageCached = null;
			base..ctor(0);
			throw new ArgumentNullException("this");
		}

		// Token: 0x06002ADE RID: 10974 RVA: 0x000891C0 File Offset: 0x000881C0
		protected unsafe override void LoadManagedCodePage()
		{
			if (this.pCodePage->ByteCount != 1)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_NoCodepageData", new object[]
				{
					this.CodePage
				}));
			}
			this.byteUnknown = (byte)this.pCodePage->ByteReplace;
			this.charUnknown = this.pCodePage->UnicodeReplace;
			byte* sharedMemory = base.GetSharedMemory(66052 + this.iExtraBytes);
			this.mapBytesToUnicode = (char*)sharedMemory;
			this.mapUnicodeToBytes = sharedMemory + 512;
			this.mapCodePageCached = (int*)(sharedMemory + 512 + 65536 + this.iExtraBytes);
			if (*this.mapCodePageCached == 0)
			{
				char* ptr = (char*)(&this.pCodePage->FirstDataWord);
				for (int i = 0; i < 256; i++)
				{
					if (ptr[i] != '\0' || i == 0)
					{
						this.mapBytesToUnicode[i] = ptr[i];
						if (ptr[i] != '�')
						{
							this.mapUnicodeToBytes[ptr[i]] = (byte)i;
						}
					}
					else
					{
						this.mapBytesToUnicode[i] = '�';
					}
				}
				*this.mapCodePageCached = this.dataTableCodePage;
				return;
			}
			if (*this.mapCodePageCached != this.dataTableCodePage)
			{
				throw new OutOfMemoryException(Environment.GetResourceString("Arg_OutOfMemoryException"));
			}
		}

		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x06002ADF RID: 10975 RVA: 0x0008930C File Offset: 0x0008830C
		private static object InternalSyncObject
		{
			get
			{
				if (SBCSCodePageEncoding.s_InternalSyncObject == null)
				{
					object value = new object();
					Interlocked.CompareExchange(ref SBCSCodePageEncoding.s_InternalSyncObject, value, null);
				}
				return SBCSCodePageEncoding.s_InternalSyncObject;
			}
		}

		// Token: 0x06002AE0 RID: 10976 RVA: 0x00089338 File Offset: 0x00088338
		protected unsafe override void ReadBestFitTable()
		{
			lock (SBCSCodePageEncoding.InternalSyncObject)
			{
				if (this.arrayUnicodeBestFit == null)
				{
					byte* ptr = (byte*)(&this.pCodePage->FirstDataWord);
					ptr += 512;
					char[] array = new char[256];
					for (int i = 0; i < 256; i++)
					{
						array[i] = this.mapBytesToUnicode[i];
					}
					ushort num;
					while ((num = *(ushort*)ptr) != 0)
					{
						ptr += 2;
						array[(int)num] = (char)(*(ushort*)ptr);
						ptr += 2;
					}
					this.arrayByteBestFit = array;
					ptr += 2;
					byte* ptr2 = ptr;
					int num2 = 0;
					int j = (int)(*(ushort*)ptr);
					ptr += 2;
					while (j < 65536)
					{
						byte b = *ptr;
						ptr++;
						if (b == 1)
						{
							j = (int)(*(ushort*)ptr);
							ptr += 2;
						}
						else if (b < 32 && b > 0 && b != 30)
						{
							j += (int)b;
						}
						else
						{
							if (b > 0)
							{
								num2++;
							}
							j++;
						}
					}
					array = new char[num2 * 2];
					ptr = ptr2;
					j = (int)(*(ushort*)ptr);
					ptr += 2;
					num2 = 0;
					while (j < 65536)
					{
						byte b2 = *ptr;
						ptr++;
						if (b2 == 1)
						{
							j = (int)(*(ushort*)ptr);
							ptr += 2;
						}
						else if (b2 < 32 && b2 > 0 && b2 != 30)
						{
							j += (int)b2;
						}
						else
						{
							if (b2 == 30)
							{
								b2 = *ptr;
								ptr++;
							}
							if (b2 > 0)
							{
								array[num2++] = (char)j;
								array[num2++] = this.mapBytesToUnicode[b2];
							}
							j++;
						}
					}
					this.arrayUnicodeBestFit = array;
				}
			}
		}

		// Token: 0x06002AE1 RID: 10977 RVA: 0x000894E0 File Offset: 0x000884E0
		internal unsafe override int GetByteCount(char* chars, int count, EncoderNLS encoder)
		{
			base.CheckMemorySection();
			char c = '\0';
			EncoderReplacementFallback encoderReplacementFallback;
			if (encoder != null)
			{
				c = encoder.charLeftOver;
				encoderReplacementFallback = (encoder.Fallback as EncoderReplacementFallback);
			}
			else
			{
				encoderReplacementFallback = (base.EncoderFallback as EncoderReplacementFallback);
			}
			if (encoderReplacementFallback != null && encoderReplacementFallback.MaxCharCount == 1)
			{
				if (c > '\0')
				{
					count++;
				}
				return count;
			}
			EncoderFallbackBuffer encoderFallbackBuffer = null;
			int num = 0;
			char* ptr = chars + count;
			if (c > '\0')
			{
				encoderFallbackBuffer = encoder.FallbackBuffer;
				encoderFallbackBuffer.InternalInitialize(chars, ptr, encoder, false);
				encoderFallbackBuffer.InternalFallback(c, ref chars);
			}
			char c2;
			while ((c2 = ((encoderFallbackBuffer == null) ? '\0' : encoderFallbackBuffer.InternalGetNextChar())) != '\0' || chars < ptr)
			{
				if (c2 == '\0')
				{
					c2 = *chars;
					chars++;
				}
				if (this.mapUnicodeToBytes[c2] == 0 && c2 != '\0')
				{
					if (encoderFallbackBuffer == null)
					{
						if (encoder == null)
						{
							encoderFallbackBuffer = this.encoderFallback.CreateFallbackBuffer();
						}
						else
						{
							encoderFallbackBuffer = encoder.FallbackBuffer;
						}
						encoderFallbackBuffer.InternalInitialize(ptr - count, ptr, encoder, false);
					}
					encoderFallbackBuffer.InternalFallback(c2, ref chars);
				}
				else
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06002AE2 RID: 10978 RVA: 0x000895D8 File Offset: 0x000885D8
		internal unsafe override int GetBytes(char* chars, int charCount, byte* bytes, int byteCount, EncoderNLS encoder)
		{
			base.CheckMemorySection();
			char c = '\0';
			EncoderReplacementFallback encoderReplacementFallback;
			if (encoder != null)
			{
				c = encoder.charLeftOver;
				encoderReplacementFallback = (encoder.Fallback as EncoderReplacementFallback);
			}
			else
			{
				encoderReplacementFallback = (base.EncoderFallback as EncoderReplacementFallback);
			}
			char* ptr = chars + charCount;
			byte* ptr2 = bytes;
			char* ptr3 = chars;
			if (encoderReplacementFallback != null && encoderReplacementFallback.MaxCharCount == 1)
			{
				byte b = this.mapUnicodeToBytes[encoderReplacementFallback.DefaultString[0]];
				if (b != 0)
				{
					if (c > '\0')
					{
						if (byteCount == 0)
						{
							base.ThrowBytesOverflow(encoder, true);
						}
						*(bytes++) = b;
						byteCount--;
					}
					if (byteCount < charCount)
					{
						base.ThrowBytesOverflow(encoder, byteCount < 1);
						ptr = chars + byteCount;
					}
					while (chars < ptr)
					{
						char c2 = *chars;
						chars++;
						byte b2 = this.mapUnicodeToBytes[c2];
						if (b2 == 0 && c2 != '\0')
						{
							*bytes = b;
						}
						else
						{
							*bytes = b2;
						}
						bytes++;
					}
					if (encoder != null)
					{
						encoder.charLeftOver = '\0';
						encoder.m_charsUsed = (int)((long)(chars - ptr3));
					}
					return (int)((long)(bytes - ptr2));
				}
			}
			EncoderFallbackBuffer encoderFallbackBuffer = null;
			byte* ptr4 = bytes + byteCount;
			if (c > '\0')
			{
				encoderFallbackBuffer = encoder.FallbackBuffer;
				encoderFallbackBuffer.InternalInitialize(chars, ptr, encoder, true);
				encoderFallbackBuffer.InternalFallback(c, ref chars);
				if ((long)encoderFallbackBuffer.Remaining > (long)(ptr4 - bytes))
				{
					base.ThrowBytesOverflow(encoder, true);
				}
			}
			char c3;
			while ((c3 = ((encoderFallbackBuffer == null) ? '\0' : encoderFallbackBuffer.InternalGetNextChar())) != '\0' || chars < ptr)
			{
				if (c3 == '\0')
				{
					c3 = *chars;
					chars++;
				}
				byte b3 = this.mapUnicodeToBytes[c3];
				if (b3 == 0 && c3 != '\0')
				{
					if (encoderFallbackBuffer == null)
					{
						if (encoder == null)
						{
							encoderFallbackBuffer = this.encoderFallback.CreateFallbackBuffer();
						}
						else
						{
							encoderFallbackBuffer = encoder.FallbackBuffer;
						}
						encoderFallbackBuffer.InternalInitialize(ptr - charCount, ptr, encoder, true);
					}
					encoderFallbackBuffer.InternalFallback(c3, ref chars);
					if ((long)encoderFallbackBuffer.Remaining > (long)(ptr4 - bytes))
					{
						chars--;
						encoderFallbackBuffer.InternalReset();
						base.ThrowBytesOverflow(encoder, chars == ptr3);
						break;
					}
				}
				else
				{
					if (bytes >= ptr4)
					{
						if (encoderFallbackBuffer == null || !encoderFallbackBuffer.bFallingBack)
						{
							chars--;
						}
						base.ThrowBytesOverflow(encoder, chars == ptr3);
						break;
					}
					*bytes = b3;
					bytes++;
				}
			}
			if (encoder != null)
			{
				if (encoderFallbackBuffer != null && !encoderFallbackBuffer.bUsedEncoder)
				{
					encoder.charLeftOver = '\0';
				}
				encoder.m_charsUsed = (int)((long)(chars - ptr3));
			}
			return (int)((long)(bytes - ptr2));
		}

		// Token: 0x06002AE3 RID: 10979 RVA: 0x00089840 File Offset: 0x00088840
		internal unsafe override int GetCharCount(byte* bytes, int count, DecoderNLS decoder)
		{
			base.CheckMemorySection();
			DecoderReplacementFallback decoderReplacementFallback;
			bool isMicrosoftBestFitFallback;
			if (decoder == null)
			{
				decoderReplacementFallback = (base.DecoderFallback as DecoderReplacementFallback);
				isMicrosoftBestFitFallback = base.DecoderFallback.IsMicrosoftBestFitFallback;
			}
			else
			{
				decoderReplacementFallback = (decoder.Fallback as DecoderReplacementFallback);
				isMicrosoftBestFitFallback = decoder.Fallback.IsMicrosoftBestFitFallback;
			}
			if (isMicrosoftBestFitFallback || (decoderReplacementFallback != null && decoderReplacementFallback.MaxCharCount == 1))
			{
				return count;
			}
			DecoderFallbackBuffer decoderFallbackBuffer = null;
			int num = count;
			byte[] array = new byte[1];
			byte* ptr = bytes + count;
			while (bytes < ptr)
			{
				char c = this.mapBytesToUnicode[*bytes];
				bytes++;
				if (c == '�')
				{
					if (decoderFallbackBuffer == null)
					{
						if (decoder == null)
						{
							decoderFallbackBuffer = base.DecoderFallback.CreateFallbackBuffer();
						}
						else
						{
							decoderFallbackBuffer = decoder.FallbackBuffer;
						}
						decoderFallbackBuffer.InternalInitialize(ptr - count, null);
					}
					array[0] = *(bytes - 1);
					num--;
					num += decoderFallbackBuffer.InternalFallback(array, bytes);
				}
			}
			return num;
		}

		// Token: 0x06002AE4 RID: 10980 RVA: 0x00089914 File Offset: 0x00088914
		internal unsafe override int GetChars(byte* bytes, int byteCount, char* chars, int charCount, DecoderNLS decoder)
		{
			base.CheckMemorySection();
			byte* ptr = bytes + byteCount;
			byte* ptr2 = bytes;
			char* ptr3 = chars;
			DecoderReplacementFallback decoderReplacementFallback;
			bool isMicrosoftBestFitFallback;
			if (decoder == null)
			{
				decoderReplacementFallback = (base.DecoderFallback as DecoderReplacementFallback);
				isMicrosoftBestFitFallback = base.DecoderFallback.IsMicrosoftBestFitFallback;
			}
			else
			{
				decoderReplacementFallback = (decoder.Fallback as DecoderReplacementFallback);
				isMicrosoftBestFitFallback = decoder.Fallback.IsMicrosoftBestFitFallback;
			}
			if (isMicrosoftBestFitFallback || (decoderReplacementFallback != null && decoderReplacementFallback.MaxCharCount == 1))
			{
				char c;
				if (decoderReplacementFallback == null)
				{
					c = '?';
				}
				else
				{
					c = decoderReplacementFallback.DefaultString[0];
				}
				if (charCount < byteCount)
				{
					base.ThrowCharsOverflow(decoder, charCount < 1);
					ptr = bytes + charCount;
				}
				while (bytes < ptr)
				{
					char c2;
					if (isMicrosoftBestFitFallback)
					{
						if (this.arrayByteBestFit == null)
						{
							this.ReadBestFitTable();
						}
						c2 = this.arrayByteBestFit[(int)(*bytes)];
					}
					else
					{
						c2 = this.mapBytesToUnicode[*bytes];
					}
					bytes++;
					if (c2 == '�')
					{
						*chars = c;
					}
					else
					{
						*chars = c2;
					}
					chars++;
				}
				if (decoder != null)
				{
					decoder.m_bytesUsed = (int)((long)(bytes - ptr2));
				}
				return (int)((long)(chars - ptr3));
			}
			DecoderFallbackBuffer decoderFallbackBuffer = null;
			byte[] array = new byte[1];
			char* ptr4 = chars + charCount;
			while (bytes < ptr)
			{
				char c3 = this.mapBytesToUnicode[*bytes];
				bytes++;
				if (c3 == '�')
				{
					if (decoderFallbackBuffer == null)
					{
						if (decoder == null)
						{
							decoderFallbackBuffer = base.DecoderFallback.CreateFallbackBuffer();
						}
						else
						{
							decoderFallbackBuffer = decoder.FallbackBuffer;
						}
						decoderFallbackBuffer.InternalInitialize(ptr - byteCount, ptr4);
					}
					array[0] = *(bytes - 1);
					if (!decoderFallbackBuffer.InternalFallback(array, bytes, ref chars))
					{
						bytes--;
						decoderFallbackBuffer.InternalReset();
						base.ThrowCharsOverflow(decoder, bytes == ptr2);
						break;
					}
				}
				else
				{
					if (chars >= ptr4)
					{
						bytes--;
						base.ThrowCharsOverflow(decoder, bytes == ptr2);
						break;
					}
					*chars = c3;
					chars++;
				}
			}
			if (decoder != null)
			{
				decoder.m_bytesUsed = (int)((long)(bytes - ptr2));
			}
			return (int)((long)(chars - ptr3));
		}

		// Token: 0x06002AE5 RID: 10981 RVA: 0x00089AFC File Offset: 0x00088AFC
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
			if (num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("charCount", Environment.GetResourceString("ArgumentOutOfRange_GetByteCountOverflow"));
			}
			return (int)num;
		}

		// Token: 0x06002AE6 RID: 10982 RVA: 0x00089B68 File Offset: 0x00088B68
		public override int GetMaxCharCount(int byteCount)
		{
			if (byteCount < 0)
			{
				throw new ArgumentOutOfRangeException("byteCount", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			long num = (long)byteCount;
			if (base.DecoderFallback.MaxCharCount > 1)
			{
				num *= (long)base.DecoderFallback.MaxCharCount;
			}
			if (num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("byteCount", Environment.GetResourceString("ArgumentOutOfRange_GetCharCountOverflow"));
			}
			return (int)num;
		}

		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x06002AE7 RID: 10983 RVA: 0x00089BCE File Offset: 0x00088BCE
		public override bool IsSingleByte
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002AE8 RID: 10984 RVA: 0x00089BD4 File Offset: 0x00088BD4
		[ComVisible(false)]
		public override bool IsAlwaysNormalized(NormalizationForm form)
		{
			if (form == NormalizationForm.FormC)
			{
				int codePage = this.CodePage;
				if (codePage <= 10007)
				{
					if (codePage <= 775)
					{
						if (codePage <= 500)
						{
							if (codePage != 37 && codePage != 437 && codePage != 500)
							{
								return false;
							}
						}
						else if (codePage != 720 && codePage != 737 && codePage != 775)
						{
							return false;
						}
					}
					else if (codePage <= 1047)
					{
						switch (codePage)
						{
						case 850:
						case 852:
						case 855:
						case 858:
						case 860:
						case 861:
						case 862:
						case 863:
						case 865:
						case 866:
						case 869:
						case 870:
							break;
						case 851:
						case 853:
						case 854:
						case 856:
						case 857:
						case 859:
						case 864:
						case 867:
						case 868:
							return false;
						default:
							if (codePage != 1026 && codePage != 1047)
							{
								return false;
							}
							break;
						}
					}
					else
					{
						switch (codePage)
						{
						case 1140:
						case 1141:
						case 1142:
						case 1143:
						case 1144:
						case 1145:
						case 1146:
						case 1147:
						case 1148:
						case 1149:
							break;
						default:
							switch (codePage)
							{
							case 1250:
							case 1251:
							case 1252:
							case 1254:
							case 1256:
								break;
							case 1253:
							case 1255:
								return false;
							default:
								if (codePage != 10007)
								{
									return false;
								}
								break;
							}
							break;
						}
					}
				}
				else if (codePage <= 20866)
				{
					if (codePage <= 20273)
					{
						if (codePage != 10017 && codePage != 10029 && codePage != 20273)
						{
							return false;
						}
					}
					else
					{
						switch (codePage)
						{
						case 20277:
						case 20278:
						case 20280:
						case 20284:
						case 20285:
							break;
						case 20279:
						case 20281:
						case 20282:
						case 20283:
							return false;
						default:
							if (codePage != 20297 && codePage != 20866)
							{
								return false;
							}
							break;
						}
					}
				}
				else if (codePage <= 20924)
				{
					if (codePage != 20871 && codePage != 20880 && codePage != 20924)
					{
						return false;
					}
				}
				else if (codePage <= 21866)
				{
					if (codePage != 21025 && codePage != 21866)
					{
						return false;
					}
				}
				else
				{
					switch (codePage)
					{
					case 28591:
					case 28592:
					case 28594:
					case 28595:
					case 28599:
						break;
					case 28593:
					case 28596:
					case 28597:
					case 28598:
						return false;
					default:
						switch (codePage)
						{
						case 28603:
						case 28605:
							break;
						case 28604:
							return false;
						default:
							return false;
						}
						break;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x0400150A RID: 5386
		private const char UNKNOWN_CHAR = '�';

		// Token: 0x0400150B RID: 5387
		[NonSerialized]
		private unsafe char* mapBytesToUnicode;

		// Token: 0x0400150C RID: 5388
		[NonSerialized]
		private unsafe byte* mapUnicodeToBytes;

		// Token: 0x0400150D RID: 5389
		[NonSerialized]
		private unsafe int* mapCodePageCached;

		// Token: 0x0400150E RID: 5390
		[NonSerialized]
		private char[] arrayByteBestFit;

		// Token: 0x0400150F RID: 5391
		[NonSerialized]
		private byte byteUnknown;

		// Token: 0x04001510 RID: 5392
		[NonSerialized]
		private char charUnknown;

		// Token: 0x04001511 RID: 5393
		private static object s_InternalSyncObject;
	}
}
