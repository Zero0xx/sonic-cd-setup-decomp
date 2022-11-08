using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading;
using Microsoft.Win32;

namespace System.Text
{
	// Token: 0x020003EA RID: 1002
	[ComVisible(true)]
	[Serializable]
	public abstract class Encoding : ICloneable
	{
		// Token: 0x06002908 RID: 10504 RVA: 0x0007F1AC File Offset: 0x0007E1AC
		protected Encoding() : this(0)
		{
		}

		// Token: 0x06002909 RID: 10505 RVA: 0x0007F1B5 File Offset: 0x0007E1B5
		protected Encoding(int codePage)
		{
			if (codePage < 0)
			{
				throw new ArgumentOutOfRangeException("codePage");
			}
			this.m_codePage = codePage;
			this.SetDefaultFallbacks();
		}

		// Token: 0x0600290A RID: 10506 RVA: 0x0007F1E0 File Offset: 0x0007E1E0
		internal virtual void SetDefaultFallbacks()
		{
			this.encoderFallback = new InternalEncoderBestFitFallback(this);
			this.decoderFallback = new InternalDecoderBestFitFallback(this);
		}

		// Token: 0x0600290B RID: 10507 RVA: 0x0007F1FA File Offset: 0x0007E1FA
		internal void OnDeserializing()
		{
			this.encoderFallback = null;
			this.decoderFallback = null;
			this.m_isReadOnly = true;
		}

		// Token: 0x0600290C RID: 10508 RVA: 0x0007F211 File Offset: 0x0007E211
		internal void OnDeserialized()
		{
			if (this.encoderFallback == null || this.decoderFallback == null)
			{
				this.m_deserializedFromEverett = true;
				this.SetDefaultFallbacks();
			}
			this.dataItem = null;
		}

		// Token: 0x0600290D RID: 10509 RVA: 0x0007F237 File Offset: 0x0007E237
		[OnDeserializing]
		private void OnDeserializing(StreamingContext ctx)
		{
			this.OnDeserializing();
		}

		// Token: 0x0600290E RID: 10510 RVA: 0x0007F23F File Offset: 0x0007E23F
		[OnDeserialized]
		private void OnDeserialized(StreamingContext ctx)
		{
			this.OnDeserialized();
		}

		// Token: 0x0600290F RID: 10511 RVA: 0x0007F247 File Offset: 0x0007E247
		[OnSerializing]
		private void OnSerializing(StreamingContext ctx)
		{
			this.dataItem = null;
		}

		// Token: 0x06002910 RID: 10512 RVA: 0x0007F250 File Offset: 0x0007E250
		internal void DeserializeEncoding(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			this.m_codePage = (int)info.GetValue("m_codePage", typeof(int));
			this.dataItem = null;
			try
			{
				this.m_isReadOnly = (bool)info.GetValue("m_isReadOnly", typeof(bool));
				this.encoderFallback = (EncoderFallback)info.GetValue("encoderFallback", typeof(EncoderFallback));
				this.decoderFallback = (DecoderFallback)info.GetValue("decoderFallback", typeof(DecoderFallback));
			}
			catch (SerializationException)
			{
				this.m_deserializedFromEverett = true;
				this.m_isReadOnly = true;
				this.SetDefaultFallbacks();
			}
		}

		// Token: 0x06002911 RID: 10513 RVA: 0x0007F31C File Offset: 0x0007E31C
		internal void SerializeEncoding(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("m_isReadOnly", this.m_isReadOnly);
			info.AddValue("encoderFallback", this.EncoderFallback);
			info.AddValue("decoderFallback", this.DecoderFallback);
			info.AddValue("m_codePage", this.m_codePage);
			info.AddValue("dataItem", null);
			info.AddValue("Encoding+m_codePage", this.m_codePage);
			info.AddValue("Encoding+dataItem", null);
		}

		// Token: 0x06002912 RID: 10514 RVA: 0x0007F3A4 File Offset: 0x0007E3A4
		public static byte[] Convert(Encoding srcEncoding, Encoding dstEncoding, byte[] bytes)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			return Encoding.Convert(srcEncoding, dstEncoding, bytes, 0, bytes.Length);
		}

		// Token: 0x06002913 RID: 10515 RVA: 0x0007F3C0 File Offset: 0x0007E3C0
		public static byte[] Convert(Encoding srcEncoding, Encoding dstEncoding, byte[] bytes, int index, int count)
		{
			if (srcEncoding == null || dstEncoding == null)
			{
				throw new ArgumentNullException((srcEncoding == null) ? "srcEncoding" : "dstEncoding", Environment.GetResourceString("ArgumentNull_Array"));
			}
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes", Environment.GetResourceString("ArgumentNull_Array"));
			}
			return dstEncoding.GetBytes(srcEncoding.GetChars(bytes, index, count));
		}

		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x06002914 RID: 10516 RVA: 0x0007F41C File Offset: 0x0007E41C
		private static object InternalSyncObject
		{
			get
			{
				if (Encoding.s_InternalSyncObject == null)
				{
					object value = new object();
					Interlocked.CompareExchange(ref Encoding.s_InternalSyncObject, value, null);
				}
				return Encoding.s_InternalSyncObject;
			}
		}

		// Token: 0x06002915 RID: 10517 RVA: 0x0007F448 File Offset: 0x0007E448
		public static Encoding GetEncoding(int codepage)
		{
			if (codepage < 0 || codepage > 65535)
			{
				throw new ArgumentOutOfRangeException("codepage", Environment.GetResourceString("ArgumentOutOfRange_Range", new object[]
				{
					0,
					65535
				}));
			}
			Encoding encoding = null;
			if (Encoding.encodings != null)
			{
				encoding = (Encoding)Encoding.encodings[codepage];
			}
			if (encoding == null)
			{
				lock (Encoding.InternalSyncObject)
				{
					if (Encoding.encodings == null)
					{
						Encoding.encodings = new Hashtable();
					}
					if ((encoding = (Encoding)Encoding.encodings[codepage]) != null)
					{
						return encoding;
					}
					if (codepage <= 1201)
					{
						switch (codepage)
						{
						case 0:
							encoding = Encoding.Default;
							goto IL_188;
						case 1:
						case 2:
						case 3:
							break;
						default:
							if (codepage != 42)
							{
								switch (codepage)
								{
								case 1200:
									encoding = Encoding.Unicode;
									goto IL_188;
								case 1201:
									encoding = Encoding.BigEndianUnicode;
									goto IL_188;
								default:
									goto IL_177;
								}
							}
							break;
						}
						throw new ArgumentException(Environment.GetResourceString("Argument_CodepageNotSupported", new object[]
						{
							codepage
						}), "codepage");
					}
					if (codepage <= 20127)
					{
						if (codepage == 1252)
						{
							encoding = new SBCSCodePageEncoding(codepage);
							goto IL_188;
						}
						if (codepage == 20127)
						{
							encoding = Encoding.ASCII;
							goto IL_188;
						}
					}
					else
					{
						if (codepage == 28591)
						{
							encoding = Encoding.Latin1;
							goto IL_188;
						}
						if (codepage == 65001)
						{
							encoding = Encoding.UTF8;
							goto IL_188;
						}
					}
					IL_177:
					encoding = Encoding.GetEncodingCodePage(codepage);
					if (encoding == null)
					{
						encoding = Encoding.GetEncodingRare(codepage);
					}
					IL_188:
					Encoding.encodings.Add(codepage, encoding);
				}
				return encoding;
			}
			return encoding;
		}

		// Token: 0x06002916 RID: 10518 RVA: 0x0007F618 File Offset: 0x0007E618
		public static Encoding GetEncoding(int codepage, EncoderFallback encoderFallback, DecoderFallback decoderFallback)
		{
			Encoding encoding = Encoding.GetEncoding(codepage);
			Encoding encoding2 = (Encoding)encoding.Clone();
			encoding2.EncoderFallback = encoderFallback;
			encoding2.DecoderFallback = decoderFallback;
			return encoding2;
		}

		// Token: 0x06002917 RID: 10519 RVA: 0x0007F648 File Offset: 0x0007E648
		private static Encoding GetEncodingRare(int codepage)
		{
			if (codepage <= 51932)
			{
				if (codepage <= 12001)
				{
					if (codepage == 10003)
					{
						return new DBCSCodePageEncoding(10003, 20949);
					}
					if (codepage == 10008)
					{
						return new DBCSCodePageEncoding(10008, 20936);
					}
					switch (codepage)
					{
					case 12000:
						return Encoding.UTF32;
					case 12001:
						return new UTF32Encoding(true, true);
					default:
						goto IL_1B4;
					}
				}
				else
				{
					if (codepage == 38598)
					{
						return new SBCSCodePageEncoding(codepage, 28598);
					}
					switch (codepage)
					{
					case 50220:
					case 50221:
					case 50222:
					case 50225:
						break;
					case 50223:
					case 50224:
					case 50226:
					case 50228:
						goto IL_1B4;
					case 50227:
						goto IL_172;
					case 50229:
						throw new NotSupportedException(Environment.GetResourceString("NotSupported_CodePage50229"));
					default:
						if (codepage != 51932)
						{
							goto IL_1B4;
						}
						return new EUCJPEncoding();
					}
				}
			}
			else if (codepage <= 52936)
			{
				if (codepage == 51936)
				{
					goto IL_172;
				}
				if (codepage == 51949)
				{
					return new DBCSCodePageEncoding(codepage, 20949);
				}
				if (codepage != 52936)
				{
					goto IL_1B4;
				}
			}
			else
			{
				if (codepage == 54936)
				{
					return new GB18030Encoding();
				}
				switch (codepage)
				{
				case 57002:
				case 57003:
				case 57004:
				case 57005:
				case 57006:
				case 57007:
				case 57008:
				case 57009:
				case 57010:
				case 57011:
					return new ISCIIEncoding(codepage);
				default:
					if (codepage == 65000)
					{
						return Encoding.UTF7;
					}
					goto IL_1B4;
				}
			}
			return new ISO2022Encoding(codepage);
			IL_172:
			return new DBCSCodePageEncoding(codepage, 936);
			IL_1B4:
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NoCodepageData", new object[]
			{
				codepage
			}));
		}

		// Token: 0x06002918 RID: 10520 RVA: 0x0007F82C File Offset: 0x0007E82C
		private static Encoding GetEncodingCodePage(int CodePage)
		{
			int codePageByteSize = BaseCodePageEncoding.GetCodePageByteSize(CodePage);
			if (codePageByteSize == 1)
			{
				return new SBCSCodePageEncoding(CodePage);
			}
			if (codePageByteSize == 2)
			{
				return new DBCSCodePageEncoding(CodePage);
			}
			return null;
		}

		// Token: 0x06002919 RID: 10521 RVA: 0x0007F857 File Offset: 0x0007E857
		public static Encoding GetEncoding(string name)
		{
			return Encoding.GetEncoding(EncodingTable.GetCodePageFromName(name));
		}

		// Token: 0x0600291A RID: 10522 RVA: 0x0007F864 File Offset: 0x0007E864
		public static Encoding GetEncoding(string name, EncoderFallback encoderFallback, DecoderFallback decoderFallback)
		{
			return Encoding.GetEncoding(EncodingTable.GetCodePageFromName(name), encoderFallback, decoderFallback);
		}

		// Token: 0x0600291B RID: 10523 RVA: 0x0007F873 File Offset: 0x0007E873
		public static EncodingInfo[] GetEncodings()
		{
			return EncodingTable.GetEncodings();
		}

		// Token: 0x0600291C RID: 10524 RVA: 0x0007F87A File Offset: 0x0007E87A
		public virtual byte[] GetPreamble()
		{
			return Encoding.emptyByteArray;
		}

		// Token: 0x0600291D RID: 10525 RVA: 0x0007F884 File Offset: 0x0007E884
		private void GetDataItem()
		{
			if (this.dataItem == null)
			{
				this.dataItem = EncodingTable.GetCodePageDataItem(this.m_codePage);
				if (this.dataItem == null)
				{
					throw new NotSupportedException(Environment.GetResourceString("NotSupported_NoCodepageData", new object[]
					{
						this.m_codePage
					}));
				}
			}
		}

		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x0600291E RID: 10526 RVA: 0x0007F8D8 File Offset: 0x0007E8D8
		public virtual string BodyName
		{
			get
			{
				if (this.dataItem == null)
				{
					this.GetDataItem();
				}
				return this.dataItem.BodyName;
			}
		}

		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x0600291F RID: 10527 RVA: 0x0007F8F3 File Offset: 0x0007E8F3
		public virtual string EncodingName
		{
			get
			{
				return Environment.GetResourceString("Globalization.cp_" + this.m_codePage);
			}
		}

		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x06002920 RID: 10528 RVA: 0x0007F90F File Offset: 0x0007E90F
		public virtual string HeaderName
		{
			get
			{
				if (this.dataItem == null)
				{
					this.GetDataItem();
				}
				return this.dataItem.HeaderName;
			}
		}

		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x06002921 RID: 10529 RVA: 0x0007F92A File Offset: 0x0007E92A
		public virtual string WebName
		{
			get
			{
				if (this.dataItem == null)
				{
					this.GetDataItem();
				}
				return this.dataItem.WebName;
			}
		}

		// Token: 0x170007C0 RID: 1984
		// (get) Token: 0x06002922 RID: 10530 RVA: 0x0007F945 File Offset: 0x0007E945
		public virtual int WindowsCodePage
		{
			get
			{
				if (this.dataItem == null)
				{
					this.GetDataItem();
				}
				return this.dataItem.UIFamilyCodePage;
			}
		}

		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x06002923 RID: 10531 RVA: 0x0007F960 File Offset: 0x0007E960
		public virtual bool IsBrowserDisplay
		{
			get
			{
				if (this.dataItem == null)
				{
					this.GetDataItem();
				}
				return (this.dataItem.Flags & 2U) != 0U;
			}
		}

		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x06002924 RID: 10532 RVA: 0x0007F983 File Offset: 0x0007E983
		public virtual bool IsBrowserSave
		{
			get
			{
				if (this.dataItem == null)
				{
					this.GetDataItem();
				}
				return (this.dataItem.Flags & 512U) != 0U;
			}
		}

		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x06002925 RID: 10533 RVA: 0x0007F9AA File Offset: 0x0007E9AA
		public virtual bool IsMailNewsDisplay
		{
			get
			{
				if (this.dataItem == null)
				{
					this.GetDataItem();
				}
				return (this.dataItem.Flags & 1U) != 0U;
			}
		}

		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x06002926 RID: 10534 RVA: 0x0007F9CD File Offset: 0x0007E9CD
		public virtual bool IsMailNewsSave
		{
			get
			{
				if (this.dataItem == null)
				{
					this.GetDataItem();
				}
				return (this.dataItem.Flags & 256U) != 0U;
			}
		}

		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x06002927 RID: 10535 RVA: 0x0007F9F4 File Offset: 0x0007E9F4
		[ComVisible(false)]
		public virtual bool IsSingleByte
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x06002928 RID: 10536 RVA: 0x0007F9F7 File Offset: 0x0007E9F7
		// (set) Token: 0x06002929 RID: 10537 RVA: 0x0007F9FF File Offset: 0x0007E9FF
		[ComVisible(false)]
		public EncoderFallback EncoderFallback
		{
			get
			{
				return this.encoderFallback;
			}
			set
			{
				if (this.IsReadOnly)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ReadOnly"));
				}
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.encoderFallback = value;
			}
		}

		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x0600292A RID: 10538 RVA: 0x0007FA2E File Offset: 0x0007EA2E
		// (set) Token: 0x0600292B RID: 10539 RVA: 0x0007FA36 File Offset: 0x0007EA36
		[ComVisible(false)]
		public DecoderFallback DecoderFallback
		{
			get
			{
				return this.decoderFallback;
			}
			set
			{
				if (this.IsReadOnly)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ReadOnly"));
				}
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.decoderFallback = value;
			}
		}

		// Token: 0x0600292C RID: 10540 RVA: 0x0007FA68 File Offset: 0x0007EA68
		[ComVisible(false)]
		public virtual object Clone()
		{
			Encoding encoding = (Encoding)base.MemberwiseClone();
			encoding.m_isReadOnly = false;
			return encoding;
		}

		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x0600292D RID: 10541 RVA: 0x0007FA89 File Offset: 0x0007EA89
		[ComVisible(false)]
		public bool IsReadOnly
		{
			get
			{
				return this.m_isReadOnly;
			}
		}

		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x0600292E RID: 10542 RVA: 0x0007FA91 File Offset: 0x0007EA91
		public static Encoding ASCII
		{
			get
			{
				if (Encoding.asciiEncoding == null)
				{
					Encoding.asciiEncoding = new ASCIIEncoding();
				}
				return Encoding.asciiEncoding;
			}
		}

		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x0600292F RID: 10543 RVA: 0x0007FAA9 File Offset: 0x0007EAA9
		private static Encoding Latin1
		{
			get
			{
				if (Encoding.latin1Encoding == null)
				{
					Encoding.latin1Encoding = new Latin1Encoding();
				}
				return Encoding.latin1Encoding;
			}
		}

		// Token: 0x06002930 RID: 10544 RVA: 0x0007FAC1 File Offset: 0x0007EAC1
		public virtual int GetByteCount(char[] chars)
		{
			if (chars == null)
			{
				throw new ArgumentNullException("chars", Environment.GetResourceString("ArgumentNull_Array"));
			}
			return this.GetByteCount(chars, 0, chars.Length);
		}

		// Token: 0x06002931 RID: 10545 RVA: 0x0007FAE8 File Offset: 0x0007EAE8
		public virtual int GetByteCount(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			char[] array = s.ToCharArray();
			return this.GetByteCount(array, 0, array.Length);
		}

		// Token: 0x06002932 RID: 10546
		public abstract int GetByteCount(char[] chars, int index, int count);

		// Token: 0x06002933 RID: 10547 RVA: 0x0007FB18 File Offset: 0x0007EB18
		[CLSCompliant(false)]
		[ComVisible(false)]
		public unsafe virtual int GetByteCount(char* chars, int count)
		{
			if (chars == null)
			{
				throw new ArgumentNullException("chars", Environment.GetResourceString("ArgumentNull_Array"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			char[] array = new char[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = chars[i];
			}
			return this.GetByteCount(array, 0, count);
		}

		// Token: 0x06002934 RID: 10548 RVA: 0x0007FB7E File Offset: 0x0007EB7E
		internal unsafe virtual int GetByteCount(char* chars, int count, EncoderNLS encoder)
		{
			return this.GetByteCount(chars, count);
		}

		// Token: 0x06002935 RID: 10549 RVA: 0x0007FB88 File Offset: 0x0007EB88
		public virtual byte[] GetBytes(char[] chars)
		{
			if (chars == null)
			{
				throw new ArgumentNullException("chars", Environment.GetResourceString("ArgumentNull_Array"));
			}
			return this.GetBytes(chars, 0, chars.Length);
		}

		// Token: 0x06002936 RID: 10550 RVA: 0x0007FBB0 File Offset: 0x0007EBB0
		public virtual byte[] GetBytes(char[] chars, int index, int count)
		{
			byte[] array = new byte[this.GetByteCount(chars, index, count)];
			this.GetBytes(chars, index, count, array, 0);
			return array;
		}

		// Token: 0x06002937 RID: 10551
		public abstract int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex);

		// Token: 0x06002938 RID: 10552 RVA: 0x0007FBDC File Offset: 0x0007EBDC
		public virtual byte[] GetBytes(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s", Environment.GetResourceString("ArgumentNull_String"));
			}
			char[] array = s.ToCharArray();
			return this.GetBytes(array, 0, array.Length);
		}

		// Token: 0x06002939 RID: 10553 RVA: 0x0007FC13 File Offset: 0x0007EC13
		public virtual int GetBytes(string s, int charIndex, int charCount, byte[] bytes, int byteIndex)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			return this.GetBytes(s.ToCharArray(), charIndex, charCount, bytes, byteIndex);
		}

		// Token: 0x0600293A RID: 10554 RVA: 0x0007FC35 File Offset: 0x0007EC35
		internal unsafe virtual int GetBytes(char* chars, int charCount, byte* bytes, int byteCount, EncoderNLS encoder)
		{
			return this.GetBytes(chars, charCount, bytes, byteCount);
		}

		// Token: 0x0600293B RID: 10555 RVA: 0x0007FC44 File Offset: 0x0007EC44
		[CLSCompliant(false)]
		[ComVisible(false)]
		public unsafe virtual int GetBytes(char* chars, int charCount, byte* bytes, int byteCount)
		{
			if (bytes == null || chars == null)
			{
				throw new ArgumentNullException((bytes == null) ? "bytes" : "chars", Environment.GetResourceString("ArgumentNull_Array"));
			}
			if (charCount < 0 || byteCount < 0)
			{
				throw new ArgumentOutOfRangeException((charCount < 0) ? "charCount" : "byteCount", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			char[] array = new char[charCount];
			for (int i = 0; i < charCount; i++)
			{
				array[i] = chars[i];
			}
			byte[] array2 = new byte[byteCount];
			int bytes2 = this.GetBytes(array, 0, charCount, array2, 0);
			if (bytes2 < byteCount)
			{
				byteCount = bytes2;
			}
			for (int i = 0; i < byteCount; i++)
			{
				bytes[i] = array2[i];
			}
			return byteCount;
		}

		// Token: 0x0600293C RID: 10556 RVA: 0x0007FCF4 File Offset: 0x0007ECF4
		public virtual int GetCharCount(byte[] bytes)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes", Environment.GetResourceString("ArgumentNull_Array"));
			}
			return this.GetCharCount(bytes, 0, bytes.Length);
		}

		// Token: 0x0600293D RID: 10557
		public abstract int GetCharCount(byte[] bytes, int index, int count);

		// Token: 0x0600293E RID: 10558 RVA: 0x0007FD1C File Offset: 0x0007ED1C
		[ComVisible(false)]
		[CLSCompliant(false)]
		public unsafe virtual int GetCharCount(byte* bytes, int count)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes", Environment.GetResourceString("ArgumentNull_Array"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			byte[] array = new byte[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = bytes[i];
			}
			return this.GetCharCount(array, 0, count);
		}

		// Token: 0x0600293F RID: 10559 RVA: 0x0007FD7F File Offset: 0x0007ED7F
		internal unsafe virtual int GetCharCount(byte* bytes, int count, DecoderNLS decoder)
		{
			return this.GetCharCount(bytes, count);
		}

		// Token: 0x06002940 RID: 10560 RVA: 0x0007FD89 File Offset: 0x0007ED89
		public virtual char[] GetChars(byte[] bytes)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes", Environment.GetResourceString("ArgumentNull_Array"));
			}
			return this.GetChars(bytes, 0, bytes.Length);
		}

		// Token: 0x06002941 RID: 10561 RVA: 0x0007FDB0 File Offset: 0x0007EDB0
		public virtual char[] GetChars(byte[] bytes, int index, int count)
		{
			char[] array = new char[this.GetCharCount(bytes, index, count)];
			this.GetChars(bytes, index, count, array, 0);
			return array;
		}

		// Token: 0x06002942 RID: 10562
		public abstract int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex);

		// Token: 0x06002943 RID: 10563 RVA: 0x0007FDDC File Offset: 0x0007EDDC
		[CLSCompliant(false)]
		[ComVisible(false)]
		public unsafe virtual int GetChars(byte* bytes, int byteCount, char* chars, int charCount)
		{
			if (chars == null || bytes == null)
			{
				throw new ArgumentNullException((chars == null) ? "chars" : "bytes", Environment.GetResourceString("ArgumentNull_Array"));
			}
			if (byteCount < 0 || charCount < 0)
			{
				throw new ArgumentOutOfRangeException((byteCount < 0) ? "byteCount" : "charCount", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			byte[] array = new byte[byteCount];
			for (int i = 0; i < byteCount; i++)
			{
				array[i] = bytes[i];
			}
			char[] array2 = new char[charCount];
			int chars2 = this.GetChars(array, 0, byteCount, array2, 0);
			if (chars2 < charCount)
			{
				charCount = chars2;
			}
			for (int i = 0; i < charCount; i++)
			{
				chars[i] = array2[i];
			}
			return charCount;
		}

		// Token: 0x06002944 RID: 10564 RVA: 0x0007FE8C File Offset: 0x0007EE8C
		internal unsafe virtual int GetChars(byte* bytes, int byteCount, char* chars, int charCount, DecoderNLS decoder)
		{
			return this.GetChars(bytes, byteCount, chars, charCount);
		}

		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x06002945 RID: 10565 RVA: 0x0007FE99 File Offset: 0x0007EE99
		public virtual int CodePage
		{
			get
			{
				return this.m_codePage;
			}
		}

		// Token: 0x06002946 RID: 10566 RVA: 0x0007FEA1 File Offset: 0x0007EEA1
		[ComVisible(false)]
		public bool IsAlwaysNormalized()
		{
			return this.IsAlwaysNormalized(NormalizationForm.FormC);
		}

		// Token: 0x06002947 RID: 10567 RVA: 0x0007FEAA File Offset: 0x0007EEAA
		[ComVisible(false)]
		public virtual bool IsAlwaysNormalized(NormalizationForm form)
		{
			return false;
		}

		// Token: 0x06002948 RID: 10568 RVA: 0x0007FEAD File Offset: 0x0007EEAD
		public virtual Decoder GetDecoder()
		{
			return new Encoding.DefaultDecoder(this);
		}

		// Token: 0x06002949 RID: 10569 RVA: 0x0007FEB8 File Offset: 0x0007EEB8
		private static Encoding CreateDefaultEncoding()
		{
			int acp = Win32Native.GetACP();
			Encoding result;
			if (acp == 1252)
			{
				result = new SBCSCodePageEncoding(acp);
			}
			else
			{
				result = Encoding.GetEncoding(acp);
			}
			return result;
		}

		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x0600294A RID: 10570 RVA: 0x0007FEE4 File Offset: 0x0007EEE4
		public static Encoding Default
		{
			get
			{
				if (Encoding.defaultEncoding == null)
				{
					Encoding.defaultEncoding = Encoding.CreateDefaultEncoding();
				}
				return Encoding.defaultEncoding;
			}
		}

		// Token: 0x0600294B RID: 10571 RVA: 0x0007FEFC File Offset: 0x0007EEFC
		public virtual Encoder GetEncoder()
		{
			return new Encoding.DefaultEncoder(this);
		}

		// Token: 0x0600294C RID: 10572
		public abstract int GetMaxByteCount(int charCount);

		// Token: 0x0600294D RID: 10573
		public abstract int GetMaxCharCount(int byteCount);

		// Token: 0x0600294E RID: 10574 RVA: 0x0007FF04 File Offset: 0x0007EF04
		public virtual string GetString(byte[] bytes)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes", Environment.GetResourceString("ArgumentNull_Array"));
			}
			return this.GetString(bytes, 0, bytes.Length);
		}

		// Token: 0x0600294F RID: 10575 RVA: 0x0007FF29 File Offset: 0x0007EF29
		public virtual string GetString(byte[] bytes, int index, int count)
		{
			return new string(this.GetChars(bytes, index, count));
		}

		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x06002950 RID: 10576 RVA: 0x0007FF39 File Offset: 0x0007EF39
		public static Encoding Unicode
		{
			get
			{
				if (Encoding.unicodeEncoding == null)
				{
					Encoding.unicodeEncoding = new UnicodeEncoding(false, true);
				}
				return Encoding.unicodeEncoding;
			}
		}

		// Token: 0x170007CE RID: 1998
		// (get) Token: 0x06002951 RID: 10577 RVA: 0x0007FF53 File Offset: 0x0007EF53
		public static Encoding BigEndianUnicode
		{
			get
			{
				if (Encoding.bigEndianUnicode == null)
				{
					Encoding.bigEndianUnicode = new UnicodeEncoding(true, true);
				}
				return Encoding.bigEndianUnicode;
			}
		}

		// Token: 0x170007CF RID: 1999
		// (get) Token: 0x06002952 RID: 10578 RVA: 0x0007FF6D File Offset: 0x0007EF6D
		public static Encoding UTF7
		{
			get
			{
				if (Encoding.utf7Encoding == null)
				{
					Encoding.utf7Encoding = new UTF7Encoding();
				}
				return Encoding.utf7Encoding;
			}
		}

		// Token: 0x170007D0 RID: 2000
		// (get) Token: 0x06002953 RID: 10579 RVA: 0x0007FF85 File Offset: 0x0007EF85
		public static Encoding UTF8
		{
			get
			{
				if (Encoding.utf8Encoding == null)
				{
					Encoding.utf8Encoding = new UTF8Encoding(true);
				}
				return Encoding.utf8Encoding;
			}
		}

		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x06002954 RID: 10580 RVA: 0x0007FF9E File Offset: 0x0007EF9E
		public static Encoding UTF32
		{
			get
			{
				if (Encoding.utf32Encoding == null)
				{
					Encoding.utf32Encoding = new UTF32Encoding(false, true);
				}
				return Encoding.utf32Encoding;
			}
		}

		// Token: 0x06002955 RID: 10581 RVA: 0x0007FFB8 File Offset: 0x0007EFB8
		public override bool Equals(object value)
		{
			Encoding encoding = value as Encoding;
			return encoding != null && (this.m_codePage == encoding.m_codePage && this.EncoderFallback.Equals(encoding.EncoderFallback)) && this.DecoderFallback.Equals(encoding.DecoderFallback);
		}

		// Token: 0x06002956 RID: 10582 RVA: 0x00080005 File Offset: 0x0007F005
		public override int GetHashCode()
		{
			return this.m_codePage + this.EncoderFallback.GetHashCode() + this.DecoderFallback.GetHashCode();
		}

		// Token: 0x06002957 RID: 10583 RVA: 0x00080025 File Offset: 0x0007F025
		internal virtual char[] GetBestFitUnicodeToBytesData()
		{
			return new char[0];
		}

		// Token: 0x06002958 RID: 10584 RVA: 0x0008002D File Offset: 0x0007F02D
		internal virtual char[] GetBestFitBytesToUnicodeData()
		{
			return new char[0];
		}

		// Token: 0x06002959 RID: 10585 RVA: 0x00080038 File Offset: 0x0007F038
		internal void ThrowBytesOverflow()
		{
			throw new ArgumentException(Environment.GetResourceString("Argument_EncodingConversionOverflowBytes", new object[]
			{
				this.EncodingName,
				this.EncoderFallback.GetType()
			}), "bytes");
		}

		// Token: 0x0600295A RID: 10586 RVA: 0x00080078 File Offset: 0x0007F078
		internal void ThrowBytesOverflow(EncoderNLS encoder, bool nothingEncoded)
		{
			if (encoder == null || encoder.m_throwOnOverflow || nothingEncoded)
			{
				if (encoder != null && encoder.InternalHasFallbackBuffer)
				{
					encoder.FallbackBuffer.InternalReset();
				}
				this.ThrowBytesOverflow();
			}
			encoder.ClearMustFlush();
		}

		// Token: 0x0600295B RID: 10587 RVA: 0x000800AC File Offset: 0x0007F0AC
		internal void ThrowCharsOverflow()
		{
			throw new ArgumentException(Environment.GetResourceString("Argument_EncodingConversionOverflowChars", new object[]
			{
				this.EncodingName,
				this.DecoderFallback.GetType()
			}), "chars");
		}

		// Token: 0x0600295C RID: 10588 RVA: 0x000800EC File Offset: 0x0007F0EC
		internal void ThrowCharsOverflow(DecoderNLS decoder, bool nothingDecoded)
		{
			if (decoder == null || decoder.m_throwOnOverflow || nothingDecoded)
			{
				if (decoder != null && decoder.InternalHasFallbackBuffer)
				{
					decoder.FallbackBuffer.InternalReset();
				}
				this.ThrowCharsOverflow();
			}
			decoder.ClearMustFlush();
		}

		// Token: 0x040013F8 RID: 5112
		private const int MIMECONTF_MAILNEWS = 1;

		// Token: 0x040013F9 RID: 5113
		private const int MIMECONTF_BROWSER = 2;

		// Token: 0x040013FA RID: 5114
		private const int MIMECONTF_SAVABLE_MAILNEWS = 256;

		// Token: 0x040013FB RID: 5115
		private const int MIMECONTF_SAVABLE_BROWSER = 512;

		// Token: 0x040013FC RID: 5116
		private const int CodePageDefault = 0;

		// Token: 0x040013FD RID: 5117
		private const int CodePageNoOEM = 1;

		// Token: 0x040013FE RID: 5118
		private const int CodePageNoMac = 2;

		// Token: 0x040013FF RID: 5119
		private const int CodePageNoThread = 3;

		// Token: 0x04001400 RID: 5120
		private const int CodePageNoSymbol = 42;

		// Token: 0x04001401 RID: 5121
		private const int CodePageUnicode = 1200;

		// Token: 0x04001402 RID: 5122
		private const int CodePageBigEndian = 1201;

		// Token: 0x04001403 RID: 5123
		private const int CodePageWindows1252 = 1252;

		// Token: 0x04001404 RID: 5124
		private const int CodePageMacGB2312 = 10008;

		// Token: 0x04001405 RID: 5125
		private const int CodePageGB2312 = 20936;

		// Token: 0x04001406 RID: 5126
		private const int CodePageMacKorean = 10003;

		// Token: 0x04001407 RID: 5127
		private const int CodePageDLLKorean = 20949;

		// Token: 0x04001408 RID: 5128
		private const int ISO2022JP = 50220;

		// Token: 0x04001409 RID: 5129
		private const int ISO2022JPESC = 50221;

		// Token: 0x0400140A RID: 5130
		private const int ISO2022JPSISO = 50222;

		// Token: 0x0400140B RID: 5131
		private const int ISOKorean = 50225;

		// Token: 0x0400140C RID: 5132
		private const int ISOSimplifiedCN = 50227;

		// Token: 0x0400140D RID: 5133
		private const int EUCJP = 51932;

		// Token: 0x0400140E RID: 5134
		private const int ChineseHZ = 52936;

		// Token: 0x0400140F RID: 5135
		private const int DuplicateEUCCN = 51936;

		// Token: 0x04001410 RID: 5136
		private const int EUCCN = 936;

		// Token: 0x04001411 RID: 5137
		private const int EUCKR = 51949;

		// Token: 0x04001412 RID: 5138
		internal const int CodePageASCII = 20127;

		// Token: 0x04001413 RID: 5139
		internal const int ISO_8859_1 = 28591;

		// Token: 0x04001414 RID: 5140
		private const int ISCIIAssemese = 57006;

		// Token: 0x04001415 RID: 5141
		private const int ISCIIBengali = 57003;

		// Token: 0x04001416 RID: 5142
		private const int ISCIIDevanagari = 57002;

		// Token: 0x04001417 RID: 5143
		private const int ISCIIGujarathi = 57010;

		// Token: 0x04001418 RID: 5144
		private const int ISCIIKannada = 57008;

		// Token: 0x04001419 RID: 5145
		private const int ISCIIMalayalam = 57009;

		// Token: 0x0400141A RID: 5146
		private const int ISCIIOriya = 57007;

		// Token: 0x0400141B RID: 5147
		private const int ISCIIPanjabi = 57011;

		// Token: 0x0400141C RID: 5148
		private const int ISCIITamil = 57004;

		// Token: 0x0400141D RID: 5149
		private const int ISCIITelugu = 57005;

		// Token: 0x0400141E RID: 5150
		private const int GB18030 = 54936;

		// Token: 0x0400141F RID: 5151
		private const int ISO_8859_8I = 38598;

		// Token: 0x04001420 RID: 5152
		private const int ISO_8859_8_Visual = 28598;

		// Token: 0x04001421 RID: 5153
		private const int ENC50229 = 50229;

		// Token: 0x04001422 RID: 5154
		private const int CodePageUTF7 = 65000;

		// Token: 0x04001423 RID: 5155
		private const int CodePageUTF8 = 65001;

		// Token: 0x04001424 RID: 5156
		private const int CodePageUTF32 = 12000;

		// Token: 0x04001425 RID: 5157
		private const int CodePageUTF32BE = 12001;

		// Token: 0x04001426 RID: 5158
		private static Encoding defaultEncoding;

		// Token: 0x04001427 RID: 5159
		private static Encoding unicodeEncoding;

		// Token: 0x04001428 RID: 5160
		private static Encoding bigEndianUnicode;

		// Token: 0x04001429 RID: 5161
		private static Encoding utf7Encoding;

		// Token: 0x0400142A RID: 5162
		private static Encoding utf8Encoding;

		// Token: 0x0400142B RID: 5163
		private static Encoding utf32Encoding;

		// Token: 0x0400142C RID: 5164
		private static Encoding asciiEncoding;

		// Token: 0x0400142D RID: 5165
		private static Encoding latin1Encoding;

		// Token: 0x0400142E RID: 5166
		private static Hashtable encodings;

		// Token: 0x0400142F RID: 5167
		internal int m_codePage;

		// Token: 0x04001430 RID: 5168
		internal CodePageDataItem dataItem;

		// Token: 0x04001431 RID: 5169
		[NonSerialized]
		internal bool m_deserializedFromEverett;

		// Token: 0x04001432 RID: 5170
		[OptionalField(VersionAdded = 2)]
		private bool m_isReadOnly = true;

		// Token: 0x04001433 RID: 5171
		[OptionalField(VersionAdded = 2)]
		internal EncoderFallback encoderFallback;

		// Token: 0x04001434 RID: 5172
		[OptionalField(VersionAdded = 2)]
		internal DecoderFallback decoderFallback;

		// Token: 0x04001435 RID: 5173
		internal static readonly byte[] emptyByteArray = new byte[0];

		// Token: 0x04001436 RID: 5174
		private static object s_InternalSyncObject;

		// Token: 0x020003EC RID: 1004
		[Serializable]
		internal class DefaultEncoder : Encoder, ISerializable, IObjectReference
		{
			// Token: 0x0600296B RID: 10603 RVA: 0x00080552 File Offset: 0x0007F552
			public DefaultEncoder(Encoding encoding)
			{
				this.m_encoding = encoding;
			}

			// Token: 0x0600296C RID: 10604 RVA: 0x00080564 File Offset: 0x0007F564
			internal DefaultEncoder(SerializationInfo info, StreamingContext context)
			{
				if (info == null)
				{
					throw new ArgumentNullException("info");
				}
				this.m_encoding = (Encoding)info.GetValue("encoding", typeof(Encoding));
				try
				{
					this.m_fallback = (EncoderFallback)info.GetValue("m_fallback", typeof(EncoderFallback));
					this.charLeftOver = (char)info.GetValue("charLeftOver", typeof(char));
				}
				catch (SerializationException)
				{
				}
			}

			// Token: 0x0600296D RID: 10605 RVA: 0x000805FC File Offset: 0x0007F5FC
			public object GetRealObject(StreamingContext context)
			{
				Encoder encoder = this.m_encoding.GetEncoder();
				if (this.m_fallback != null)
				{
					encoder.m_fallback = this.m_fallback;
				}
				if (this.charLeftOver != '\0')
				{
					EncoderNLS encoderNLS = encoder as EncoderNLS;
					if (encoderNLS != null)
					{
						encoderNLS.charLeftOver = this.charLeftOver;
					}
				}
				return encoder;
			}

			// Token: 0x0600296E RID: 10606 RVA: 0x00080648 File Offset: 0x0007F648
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
			void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
			{
				if (info == null)
				{
					throw new ArgumentNullException("info");
				}
				info.AddValue("encoding", this.m_encoding);
			}

			// Token: 0x0600296F RID: 10607 RVA: 0x00080669 File Offset: 0x0007F669
			public override int GetByteCount(char[] chars, int index, int count, bool flush)
			{
				return this.m_encoding.GetByteCount(chars, index, count);
			}

			// Token: 0x06002970 RID: 10608 RVA: 0x00080679 File Offset: 0x0007F679
			public unsafe override int GetByteCount(char* chars, int count, bool flush)
			{
				return this.m_encoding.GetByteCount(chars, count);
			}

			// Token: 0x06002971 RID: 10609 RVA: 0x00080688 File Offset: 0x0007F688
			public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex, bool flush)
			{
				return this.m_encoding.GetBytes(chars, charIndex, charCount, bytes, byteIndex);
			}

			// Token: 0x06002972 RID: 10610 RVA: 0x0008069C File Offset: 0x0007F69C
			public unsafe override int GetBytes(char* chars, int charCount, byte* bytes, int byteCount, bool flush)
			{
				return this.m_encoding.GetBytes(chars, charCount, bytes, byteCount);
			}

			// Token: 0x04001439 RID: 5177
			private Encoding m_encoding;

			// Token: 0x0400143A RID: 5178
			[NonSerialized]
			internal char charLeftOver;
		}

		// Token: 0x020003EE RID: 1006
		[Serializable]
		internal class DefaultDecoder : Decoder, ISerializable, IObjectReference
		{
			// Token: 0x06002982 RID: 10626 RVA: 0x00080AEE File Offset: 0x0007FAEE
			public DefaultDecoder(Encoding encoding)
			{
				this.m_encoding = encoding;
			}

			// Token: 0x06002983 RID: 10627 RVA: 0x00080B00 File Offset: 0x0007FB00
			internal DefaultDecoder(SerializationInfo info, StreamingContext context)
			{
				if (info == null)
				{
					throw new ArgumentNullException("info");
				}
				this.m_encoding = (Encoding)info.GetValue("encoding", typeof(Encoding));
				try
				{
					this.m_fallback = (DecoderFallback)info.GetValue("m_fallback", typeof(DecoderFallback));
				}
				catch (SerializationException)
				{
					this.m_fallback = null;
				}
			}

			// Token: 0x06002984 RID: 10628 RVA: 0x00080B80 File Offset: 0x0007FB80
			public object GetRealObject(StreamingContext context)
			{
				Decoder decoder = this.m_encoding.GetDecoder();
				if (this.m_fallback != null)
				{
					decoder.m_fallback = this.m_fallback;
				}
				return decoder;
			}

			// Token: 0x06002985 RID: 10629 RVA: 0x00080BAE File Offset: 0x0007FBAE
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
			void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
			{
				if (info == null)
				{
					throw new ArgumentNullException("info");
				}
				info.AddValue("encoding", this.m_encoding);
			}

			// Token: 0x06002986 RID: 10630 RVA: 0x00080BCF File Offset: 0x0007FBCF
			public override int GetCharCount(byte[] bytes, int index, int count)
			{
				return this.GetCharCount(bytes, index, count, false);
			}

			// Token: 0x06002987 RID: 10631 RVA: 0x00080BDB File Offset: 0x0007FBDB
			public override int GetCharCount(byte[] bytes, int index, int count, bool flush)
			{
				return this.m_encoding.GetCharCount(bytes, index, count);
			}

			// Token: 0x06002988 RID: 10632 RVA: 0x00080BEB File Offset: 0x0007FBEB
			public unsafe override int GetCharCount(byte* bytes, int count, bool flush)
			{
				return this.m_encoding.GetCharCount(bytes, count);
			}

			// Token: 0x06002989 RID: 10633 RVA: 0x00080BFA File Offset: 0x0007FBFA
			public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
			{
				return this.GetChars(bytes, byteIndex, byteCount, chars, charIndex, false);
			}

			// Token: 0x0600298A RID: 10634 RVA: 0x00080C0A File Offset: 0x0007FC0A
			public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex, bool flush)
			{
				return this.m_encoding.GetChars(bytes, byteIndex, byteCount, chars, charIndex);
			}

			// Token: 0x0600298B RID: 10635 RVA: 0x00080C1E File Offset: 0x0007FC1E
			public unsafe override int GetChars(byte* bytes, int byteCount, char* chars, int charCount, bool flush)
			{
				return this.m_encoding.GetChars(bytes, byteCount, chars, charCount);
			}

			// Token: 0x0400143D RID: 5181
			private Encoding m_encoding;
		}

		// Token: 0x020003EF RID: 1007
		internal class EncodingCharBuffer
		{
			// Token: 0x0600298C RID: 10636 RVA: 0x00080C30 File Offset: 0x0007FC30
			internal unsafe EncodingCharBuffer(Encoding enc, DecoderNLS decoder, char* charStart, int charCount, byte* byteStart, int byteCount)
			{
				this.enc = enc;
				this.decoder = decoder;
				this.chars = charStart;
				this.charStart = charStart;
				this.charEnd = charStart + charCount;
				this.byteStart = byteStart;
				this.bytes = byteStart;
				this.byteEnd = byteStart + byteCount;
				if (this.decoder == null)
				{
					this.fallbackBuffer = enc.DecoderFallback.CreateFallbackBuffer();
				}
				else
				{
					this.fallbackBuffer = this.decoder.FallbackBuffer;
				}
				this.fallbackBuffer.InternalInitialize(this.bytes, this.charEnd);
			}

			// Token: 0x0600298D RID: 10637 RVA: 0x00080CCC File Offset: 0x0007FCCC
			internal unsafe bool AddChar(char ch, int numBytes)
			{
				if (this.chars != null)
				{
					if (this.chars >= this.charEnd)
					{
						this.bytes -= numBytes;
						this.enc.ThrowCharsOverflow(this.decoder, this.bytes == this.byteStart);
						return false;
					}
					*(this.chars++) = ch;
				}
				this.charCountResult++;
				return true;
			}

			// Token: 0x0600298E RID: 10638 RVA: 0x00080D46 File Offset: 0x0007FD46
			internal bool AddChar(char ch)
			{
				return this.AddChar(ch, 1);
			}

			// Token: 0x0600298F RID: 10639 RVA: 0x00080D50 File Offset: 0x0007FD50
			internal bool AddChar(char ch1, char ch2, int numBytes)
			{
				if (this.chars >= this.charEnd - 1)
				{
					this.bytes -= numBytes;
					this.enc.ThrowCharsOverflow(this.decoder, this.bytes == this.byteStart);
					return false;
				}
				return this.AddChar(ch1, numBytes) && this.AddChar(ch2, numBytes);
			}

			// Token: 0x06002990 RID: 10640 RVA: 0x00080DB4 File Offset: 0x0007FDB4
			internal void AdjustBytes(int count)
			{
				this.bytes += count;
			}

			// Token: 0x170007D8 RID: 2008
			// (get) Token: 0x06002991 RID: 10641 RVA: 0x00080DC4 File Offset: 0x0007FDC4
			internal bool MoreData
			{
				get
				{
					return this.bytes < this.byteEnd;
				}
			}

			// Token: 0x06002992 RID: 10642 RVA: 0x00080DD4 File Offset: 0x0007FDD4
			internal bool EvenMoreData(int count)
			{
				return this.bytes == this.byteEnd - count;
			}

			// Token: 0x06002993 RID: 10643 RVA: 0x00080DEC File Offset: 0x0007FDEC
			internal unsafe byte GetNextByte()
			{
				if (this.bytes >= this.byteEnd)
				{
					return 0;
				}
				return *(this.bytes++);
			}

			// Token: 0x170007D9 RID: 2009
			// (get) Token: 0x06002994 RID: 10644 RVA: 0x00080E1C File Offset: 0x0007FE1C
			internal int BytesUsed
			{
				get
				{
					return (int)((long)(this.bytes - this.byteStart));
				}
			}

			// Token: 0x06002995 RID: 10645 RVA: 0x00080E30 File Offset: 0x0007FE30
			internal bool Fallback(byte fallbackByte)
			{
				byte[] byteBuffer = new byte[]
				{
					fallbackByte
				};
				return this.Fallback(byteBuffer);
			}

			// Token: 0x06002996 RID: 10646 RVA: 0x00080E54 File Offset: 0x0007FE54
			internal bool Fallback(byte byte1, byte byte2)
			{
				byte[] byteBuffer = new byte[]
				{
					byte1,
					byte2
				};
				return this.Fallback(byteBuffer);
			}

			// Token: 0x06002997 RID: 10647 RVA: 0x00080E7C File Offset: 0x0007FE7C
			internal bool Fallback(byte byte1, byte byte2, byte byte3, byte byte4)
			{
				byte[] byteBuffer = new byte[]
				{
					byte1,
					byte2,
					byte3,
					byte4
				};
				return this.Fallback(byteBuffer);
			}

			// Token: 0x06002998 RID: 10648 RVA: 0x00080EAC File Offset: 0x0007FEAC
			internal unsafe bool Fallback(byte[] byteBuffer)
			{
				if (this.chars != null)
				{
					char* ptr = this.chars;
					if (!this.fallbackBuffer.InternalFallback(byteBuffer, this.bytes, ref this.chars))
					{
						this.bytes -= byteBuffer.Length;
						this.fallbackBuffer.InternalReset();
						this.enc.ThrowCharsOverflow(this.decoder, this.chars == this.charStart);
						return false;
					}
					this.charCountResult += (int)((long)(this.chars - ptr));
				}
				else
				{
					this.charCountResult += this.fallbackBuffer.InternalFallback(byteBuffer, this.bytes);
				}
				return true;
			}

			// Token: 0x170007DA RID: 2010
			// (get) Token: 0x06002999 RID: 10649 RVA: 0x00080F5B File Offset: 0x0007FF5B
			internal int Count
			{
				get
				{
					return this.charCountResult;
				}
			}

			// Token: 0x0400143E RID: 5182
			private unsafe char* chars;

			// Token: 0x0400143F RID: 5183
			private unsafe char* charStart;

			// Token: 0x04001440 RID: 5184
			private unsafe char* charEnd;

			// Token: 0x04001441 RID: 5185
			private int charCountResult;

			// Token: 0x04001442 RID: 5186
			private Encoding enc;

			// Token: 0x04001443 RID: 5187
			private DecoderNLS decoder;

			// Token: 0x04001444 RID: 5188
			private unsafe byte* byteStart;

			// Token: 0x04001445 RID: 5189
			private unsafe byte* byteEnd;

			// Token: 0x04001446 RID: 5190
			private unsafe byte* bytes;

			// Token: 0x04001447 RID: 5191
			private DecoderFallbackBuffer fallbackBuffer;
		}

		// Token: 0x020003F0 RID: 1008
		internal class EncodingByteBuffer
		{
			// Token: 0x0600299A RID: 10650 RVA: 0x00080F64 File Offset: 0x0007FF64
			internal unsafe EncodingByteBuffer(Encoding inEncoding, EncoderNLS inEncoder, byte* inByteStart, int inByteCount, char* inCharStart, int inCharCount)
			{
				this.enc = inEncoding;
				this.encoder = inEncoder;
				this.charStart = inCharStart;
				this.chars = inCharStart;
				this.charEnd = inCharStart + inCharCount;
				this.bytes = inByteStart;
				this.byteStart = inByteStart;
				this.byteEnd = inByteStart + inByteCount;
				if (this.encoder == null)
				{
					this.fallbackBuffer = this.enc.EncoderFallback.CreateFallbackBuffer();
				}
				else
				{
					this.fallbackBuffer = this.encoder.FallbackBuffer;
					if (this.encoder.m_throwOnOverflow && this.encoder.InternalHasFallbackBuffer && this.fallbackBuffer.Remaining > 0)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_EncoderFallbackNotEmpty", new object[]
						{
							this.encoder.Encoding.EncodingName,
							this.encoder.Fallback.GetType()
						}));
					}
				}
				this.fallbackBuffer.InternalInitialize(this.chars, this.charEnd, this.encoder, this.bytes != null);
			}

			// Token: 0x0600299B RID: 10651 RVA: 0x0008107C File Offset: 0x0008007C
			internal unsafe bool AddByte(byte b, int moreBytesExpected)
			{
				if (this.bytes != null)
				{
					if (this.bytes >= this.byteEnd - moreBytesExpected)
					{
						this.MovePrevious(true);
						return false;
					}
					*(this.bytes++) = b;
				}
				this.byteCountResult++;
				return true;
			}

			// Token: 0x0600299C RID: 10652 RVA: 0x000810CF File Offset: 0x000800CF
			internal bool AddByte(byte b1)
			{
				return this.AddByte(b1, 0);
			}

			// Token: 0x0600299D RID: 10653 RVA: 0x000810D9 File Offset: 0x000800D9
			internal bool AddByte(byte b1, byte b2)
			{
				return this.AddByte(b1, b2, 0);
			}

			// Token: 0x0600299E RID: 10654 RVA: 0x000810E4 File Offset: 0x000800E4
			internal bool AddByte(byte b1, byte b2, int moreBytesExpected)
			{
				return this.AddByte(b1, 1 + moreBytesExpected) && this.AddByte(b2, moreBytesExpected);
			}

			// Token: 0x0600299F RID: 10655 RVA: 0x000810FC File Offset: 0x000800FC
			internal bool AddByte(byte b1, byte b2, byte b3)
			{
				return this.AddByte(b1, b2, b3, 0);
			}

			// Token: 0x060029A0 RID: 10656 RVA: 0x00081108 File Offset: 0x00080108
			internal bool AddByte(byte b1, byte b2, byte b3, int moreBytesExpected)
			{
				return this.AddByte(b1, 2 + moreBytesExpected) && this.AddByte(b2, 1 + moreBytesExpected) && this.AddByte(b3, moreBytesExpected);
			}

			// Token: 0x060029A1 RID: 10657 RVA: 0x0008112F File Offset: 0x0008012F
			internal bool AddByte(byte b1, byte b2, byte b3, byte b4)
			{
				return this.AddByte(b1, 3) && this.AddByte(b2, 2) && this.AddByte(b3, 1) && this.AddByte(b4, 0);
			}

			// Token: 0x060029A2 RID: 10658 RVA: 0x0008115C File Offset: 0x0008015C
			internal void MovePrevious(bool bThrow)
			{
				if (this.fallbackBuffer.bFallingBack)
				{
					this.fallbackBuffer.MovePrevious();
				}
				else if (this.chars != this.charStart)
				{
					this.chars--;
				}
				if (bThrow)
				{
					this.enc.ThrowBytesOverflow(this.encoder, this.bytes == this.byteStart);
				}
			}

			// Token: 0x060029A3 RID: 10659 RVA: 0x000811C3 File Offset: 0x000801C3
			internal bool Fallback(char charFallback)
			{
				return this.fallbackBuffer.InternalFallback(charFallback, ref this.chars);
			}

			// Token: 0x170007DB RID: 2011
			// (get) Token: 0x060029A4 RID: 10660 RVA: 0x000811D7 File Offset: 0x000801D7
			internal bool MoreData
			{
				get
				{
					return this.fallbackBuffer.Remaining > 0 || this.chars < this.charEnd;
				}
			}

			// Token: 0x060029A5 RID: 10661 RVA: 0x000811F8 File Offset: 0x000801F8
			internal unsafe char GetNextChar()
			{
				char c = this.fallbackBuffer.InternalGetNextChar();
				if (c == '\0' && this.chars < this.charEnd)
				{
					c = *(this.chars++);
				}
				return c;
			}

			// Token: 0x170007DC RID: 2012
			// (get) Token: 0x060029A6 RID: 10662 RVA: 0x00081237 File Offset: 0x00080237
			internal int CharsUsed
			{
				get
				{
					return (int)((long)(this.chars - this.charStart));
				}
			}

			// Token: 0x170007DD RID: 2013
			// (get) Token: 0x060029A7 RID: 10663 RVA: 0x0008124A File Offset: 0x0008024A
			internal int Count
			{
				get
				{
					return this.byteCountResult;
				}
			}

			// Token: 0x04001448 RID: 5192
			private unsafe byte* bytes;

			// Token: 0x04001449 RID: 5193
			private unsafe byte* byteStart;

			// Token: 0x0400144A RID: 5194
			private unsafe byte* byteEnd;

			// Token: 0x0400144B RID: 5195
			private unsafe char* chars;

			// Token: 0x0400144C RID: 5196
			private unsafe char* charStart;

			// Token: 0x0400144D RID: 5197
			private unsafe char* charEnd;

			// Token: 0x0400144E RID: 5198
			private int byteCountResult;

			// Token: 0x0400144F RID: 5199
			private Encoding enc;

			// Token: 0x04001450 RID: 5200
			private EncoderNLS encoder;

			// Token: 0x04001451 RID: 5201
			internal EncoderFallbackBuffer fallbackBuffer;
		}
	}
}
