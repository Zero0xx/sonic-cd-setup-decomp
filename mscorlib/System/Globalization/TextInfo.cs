using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;

namespace System.Globalization
{
	// Token: 0x020003CA RID: 970
	[ComVisible(true)]
	[Serializable]
	public class TextInfo : ICloneable, IDeserializationCallback
	{
		// Token: 0x06002774 RID: 10100 RVA: 0x00076590 File Offset: 0x00075590
		unsafe static TextInfo()
		{
			byte* globalizationResourceBytePtr = GlobalizationAssembly.GetGlobalizationResourceBytePtr(typeof(TextInfo).Assembly, "l_intl.nlp");
			Thread.MemoryBarrier();
			TextInfo.m_pDataTable = globalizationResourceBytePtr;
			TextInfo.TextInfoDataHeader* pDataTable = (TextInfo.TextInfoDataHeader*)TextInfo.m_pDataTable;
			TextInfo.m_exceptionCount = (int)pDataTable->exceptionCount;
			TextInfo.m_exceptionTable = (TextInfo.ExceptionTableItem*)(&pDataTable->exceptionLangId);
			TextInfo.m_exceptionNativeTextInfo = new long[TextInfo.m_exceptionCount];
			TextInfo.m_pDefaultCasingTable = TextInfo.AllocateDefaultCasingTable(TextInfo.m_pDataTable);
		}

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x06002775 RID: 10101 RVA: 0x00076600 File Offset: 0x00075600
		private static object InternalSyncObject
		{
			get
			{
				if (TextInfo.s_InternalSyncObject == null)
				{
					object value = new object();
					Interlocked.CompareExchange(ref TextInfo.s_InternalSyncObject, value, null);
				}
				return TextInfo.s_InternalSyncObject;
			}
		}

		// Token: 0x06002776 RID: 10102 RVA: 0x0007662C File Offset: 0x0007562C
		internal TextInfo(CultureTableRecord table)
		{
			this.m_cultureTableRecord = table;
			this.m_textInfoID = (int)this.m_cultureTableRecord.ITEXTINFO;
			if (table.IsSynthetic)
			{
				this.m_pNativeTextInfo = TextInfo.InvariantNativeTextInfo;
				return;
			}
			this.m_pNativeTextInfo = TextInfo.GetNativeTextInfo(this.m_textInfoID);
		}

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x06002777 RID: 10103 RVA: 0x0007667C File Offset: 0x0007567C
		internal unsafe static void* InvariantNativeTextInfo
		{
			get
			{
				if (TextInfo.m_pInvariantNativeTextInfo == null)
				{
					lock (TextInfo.InternalSyncObject)
					{
						if (TextInfo.m_pInvariantNativeTextInfo == null)
						{
							TextInfo.m_pInvariantNativeTextInfo = TextInfo.GetNativeTextInfo(127);
						}
					}
				}
				return TextInfo.m_pInvariantNativeTextInfo;
			}
		}

		// Token: 0x06002778 RID: 10104 RVA: 0x000766D4 File Offset: 0x000756D4
		[OnDeserializing]
		private void OnDeserializing(StreamingContext ctx)
		{
			this.m_cultureTableRecord = null;
			this.m_win32LangID = 0;
		}

		// Token: 0x06002779 RID: 10105 RVA: 0x000766E4 File Offset: 0x000756E4
		private void OnDeserialized()
		{
			if (this.m_cultureTableRecord == null)
			{
				if (this.m_win32LangID == 0)
				{
					this.m_win32LangID = CultureTableRecord.IdFromEverettDataItem(this.m_nDataItem);
				}
				if (this.customCultureName != null)
				{
					this.m_cultureTableRecord = CultureTableRecord.GetCultureTableRecord(this.customCultureName, this.m_useUserOverride);
				}
				else
				{
					this.m_cultureTableRecord = CultureTableRecord.GetCultureTableRecord(this.m_win32LangID, this.m_useUserOverride);
				}
				this.m_textInfoID = (int)this.m_cultureTableRecord.ITEXTINFO;
				if (this.m_cultureTableRecord.IsSynthetic)
				{
					this.m_pNativeTextInfo = TextInfo.InvariantNativeTextInfo;
					return;
				}
				this.m_pNativeTextInfo = TextInfo.GetNativeTextInfo(this.m_textInfoID);
			}
		}

		// Token: 0x0600277A RID: 10106 RVA: 0x00076788 File Offset: 0x00075788
		[OnDeserialized]
		private void OnDeserialized(StreamingContext ctx)
		{
			this.OnDeserialized();
		}

		// Token: 0x0600277B RID: 10107 RVA: 0x00076790 File Offset: 0x00075790
		[OnSerializing]
		private void OnSerializing(StreamingContext ctx)
		{
			this.m_nDataItem = this.m_cultureTableRecord.EverettDataItem();
			this.m_useUserOverride = this.m_cultureTableRecord.UseUserOverride;
			if (CultureTableRecord.IsCustomCultureId(this.m_cultureTableRecord.CultureID))
			{
				this.customCultureName = this.m_cultureTableRecord.SNAME;
				this.m_win32LangID = this.m_textInfoID;
				return;
			}
			this.customCultureName = null;
			this.m_win32LangID = this.m_cultureTableRecord.CultureID;
		}

		// Token: 0x0600277C RID: 10108 RVA: 0x00076808 File Offset: 0x00075808
		internal unsafe static void* GetNativeTextInfo(int cultureID)
		{
			if (cultureID != 127 || Environment.OSVersion.Platform != PlatformID.Win32NT)
			{
				void* result = TextInfo.m_pDefaultCasingTable;
				for (int i = 0; i < TextInfo.m_exceptionCount; i++)
				{
					if ((int)TextInfo.m_exceptionTable[i].langID == cultureID)
					{
						if (TextInfo.m_exceptionNativeTextInfo[i] == 0L)
						{
							lock (TextInfo.InternalSyncObject)
							{
								if (TextInfo.m_pExceptionFile == null)
								{
									TextInfo.m_pExceptionFile = GlobalizationAssembly.GetGlobalizationResourceBytePtr(typeof(TextInfo).Assembly, "l_except.nlp");
								}
								long num = TextInfo.InternalAllocateCasingTable(TextInfo.m_pExceptionFile, (int)TextInfo.m_exceptionTable[i].exceptIndex);
								Thread.MemoryBarrier();
								TextInfo.m_exceptionNativeTextInfo[i] = num;
							}
						}
						result = TextInfo.m_exceptionNativeTextInfo[i];
						break;
					}
				}
				return result;
			}
			void* ptr = TextInfo.nativeGetInvariantTextInfo();
			if (ptr != null)
			{
				return ptr;
			}
			throw new TypeInitializationException(typeof(TextInfo).ToString(), null);
		}

		// Token: 0x0600277D RID: 10109 RVA: 0x00076914 File Offset: 0x00075914
		internal static int CompareOrdinalIgnoreCase(string str1, string str2)
		{
			return TextInfo.nativeCompareOrdinalIgnoreCase(TextInfo.InvariantNativeTextInfo, str1, str2);
		}

		// Token: 0x0600277E RID: 10110 RVA: 0x00076922 File Offset: 0x00075922
		internal static int CompareOrdinalIgnoreCaseEx(string strA, int indexA, string strB, int indexB, int length)
		{
			return TextInfo.nativeCompareOrdinalIgnoreCaseEx(TextInfo.InvariantNativeTextInfo, strA, indexA, strB, indexB, length);
		}

		// Token: 0x0600277F RID: 10111 RVA: 0x00076934 File Offset: 0x00075934
		internal static int GetHashCodeOrdinalIgnoreCase(string s)
		{
			return TextInfo.nativeGetHashCodeOrdinalIgnoreCase(TextInfo.InvariantNativeTextInfo, s);
		}

		// Token: 0x06002780 RID: 10112 RVA: 0x00076941 File Offset: 0x00075941
		internal static int IndexOfStringOrdinalIgnoreCase(string source, string value, int startIndex, int count)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return TextInfo.nativeIndexOfStringOrdinalIgnoreCase(TextInfo.InvariantNativeTextInfo, source, value, startIndex, count);
		}

		// Token: 0x06002781 RID: 10113 RVA: 0x0007695F File Offset: 0x0007595F
		internal static int LastIndexOfStringOrdinalIgnoreCase(string source, string value, int startIndex, int count)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return TextInfo.nativeLastIndexOfStringOrdinalIgnoreCase(TextInfo.InvariantNativeTextInfo, source, value, startIndex, count);
		}

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x06002782 RID: 10114 RVA: 0x0007697D File Offset: 0x0007597D
		public virtual int ANSICodePage
		{
			get
			{
				return (int)this.m_cultureTableRecord.IDEFAULTANSICODEPAGE;
			}
		}

		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x06002783 RID: 10115 RVA: 0x0007698A File Offset: 0x0007598A
		public virtual int OEMCodePage
		{
			get
			{
				return (int)this.m_cultureTableRecord.IDEFAULTOEMCODEPAGE;
			}
		}

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x06002784 RID: 10116 RVA: 0x00076997 File Offset: 0x00075997
		public virtual int MacCodePage
		{
			get
			{
				return (int)this.m_cultureTableRecord.IDEFAULTMACCODEPAGE;
			}
		}

		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x06002785 RID: 10117 RVA: 0x000769A4 File Offset: 0x000759A4
		public virtual int EBCDICCodePage
		{
			get
			{
				return (int)this.m_cultureTableRecord.IDEFAULTEBCDICCODEPAGE;
			}
		}

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x06002786 RID: 10118 RVA: 0x000769B1 File Offset: 0x000759B1
		[ComVisible(false)]
		public int LCID
		{
			get
			{
				return this.m_textInfoID;
			}
		}

		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x06002787 RID: 10119 RVA: 0x000769B9 File Offset: 0x000759B9
		[ComVisible(false)]
		public string CultureName
		{
			get
			{
				if (this.m_name == null)
				{
					this.m_name = CultureInfo.GetCultureInfo(this.m_textInfoID).Name;
				}
				return this.m_name;
			}
		}

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x06002788 RID: 10120 RVA: 0x000769DF File Offset: 0x000759DF
		[ComVisible(false)]
		public bool IsReadOnly
		{
			get
			{
				return this.m_isReadOnly;
			}
		}

		// Token: 0x06002789 RID: 10121 RVA: 0x000769E8 File Offset: 0x000759E8
		[ComVisible(false)]
		public virtual object Clone()
		{
			object obj = base.MemberwiseClone();
			((TextInfo)obj).SetReadOnlyState(false);
			return obj;
		}

		// Token: 0x0600278A RID: 10122 RVA: 0x00076A0C File Offset: 0x00075A0C
		[ComVisible(false)]
		public static TextInfo ReadOnly(TextInfo textInfo)
		{
			if (textInfo == null)
			{
				throw new ArgumentNullException("textInfo");
			}
			if (textInfo.IsReadOnly)
			{
				return textInfo;
			}
			TextInfo textInfo2 = (TextInfo)textInfo.MemberwiseClone();
			textInfo2.SetReadOnlyState(true);
			return textInfo2;
		}

		// Token: 0x0600278B RID: 10123 RVA: 0x00076A45 File Offset: 0x00075A45
		private void VerifyWritable()
		{
			if (this.m_isReadOnly)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ReadOnly"));
			}
		}

		// Token: 0x0600278C RID: 10124 RVA: 0x00076A5F File Offset: 0x00075A5F
		internal void SetReadOnlyState(bool readOnly)
		{
			this.m_isReadOnly = readOnly;
		}

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x0600278D RID: 10125 RVA: 0x00076A68 File Offset: 0x00075A68
		// (set) Token: 0x0600278E RID: 10126 RVA: 0x00076A89 File Offset: 0x00075A89
		public virtual string ListSeparator
		{
			get
			{
				if (this.m_listSeparator == null)
				{
					this.m_listSeparator = this.m_cultureTableRecord.SLIST;
				}
				return this.m_listSeparator;
			}
			[ComVisible(false)]
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("value", Environment.GetResourceString("ArgumentNull_String"));
				}
				this.m_listSeparator = value;
			}
		}

		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x0600278F RID: 10127 RVA: 0x00076AB0 File Offset: 0x00075AB0
		internal TextInfo CasingTextInfo
		{
			get
			{
				if (this.m_casingTextInfo == null)
				{
					if (this.ANSICodePage == 1254)
					{
						this.m_casingTextInfo = CultureInfo.GetCultureInfo("tr-TR").TextInfo;
					}
					else
					{
						this.m_casingTextInfo = CultureInfo.GetCultureInfo("en-US").TextInfo;
					}
				}
				return this.m_casingTextInfo;
			}
		}

		// Token: 0x06002790 RID: 10128 RVA: 0x00076B04 File Offset: 0x00075B04
		public virtual char ToLower(char c)
		{
			if (this.m_cultureTableRecord.IsSynthetic)
			{
				return this.CasingTextInfo.ToLower(c);
			}
			return TextInfo.nativeChangeCaseChar(this.m_textInfoID, this.m_pNativeTextInfo, c, false);
		}

		// Token: 0x06002791 RID: 10129 RVA: 0x00076B33 File Offset: 0x00075B33
		public virtual string ToLower(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			if (this.m_cultureTableRecord.IsSynthetic)
			{
				return this.CasingTextInfo.ToLower(str);
			}
			return TextInfo.nativeChangeCaseString(this.m_textInfoID, this.m_pNativeTextInfo, str, false);
		}

		// Token: 0x06002792 RID: 10130 RVA: 0x00076B70 File Offset: 0x00075B70
		public virtual char ToUpper(char c)
		{
			if (this.m_cultureTableRecord.IsSynthetic)
			{
				return this.CasingTextInfo.ToUpper(c);
			}
			return TextInfo.nativeChangeCaseChar(this.m_textInfoID, this.m_pNativeTextInfo, c, true);
		}

		// Token: 0x06002793 RID: 10131 RVA: 0x00076B9F File Offset: 0x00075B9F
		public virtual string ToUpper(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			if (this.m_cultureTableRecord.IsSynthetic)
			{
				return this.CasingTextInfo.ToUpper(str);
			}
			return TextInfo.nativeChangeCaseString(this.m_textInfoID, this.m_pNativeTextInfo, str, true);
		}

		// Token: 0x06002794 RID: 10132 RVA: 0x00076BDC File Offset: 0x00075BDC
		public override bool Equals(object obj)
		{
			TextInfo textInfo = obj as TextInfo;
			return textInfo != null && this.CultureName.Equals(textInfo.CultureName);
		}

		// Token: 0x06002795 RID: 10133 RVA: 0x00076C06 File Offset: 0x00075C06
		public override int GetHashCode()
		{
			return this.CultureName.GetHashCode();
		}

		// Token: 0x06002796 RID: 10134 RVA: 0x00076C13 File Offset: 0x00075C13
		public override string ToString()
		{
			return "TextInfo - " + this.m_textInfoID;
		}

		// Token: 0x06002797 RID: 10135 RVA: 0x00076C2A File Offset: 0x00075C2A
		private bool IsWordSeparator(UnicodeCategory category)
		{
			return (536672256 & 1 << (int)category) != 0;
		}

		// Token: 0x06002798 RID: 10136 RVA: 0x00076C40 File Offset: 0x00075C40
		public string ToTitleCase(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			if (this.m_cultureTableRecord.IsSynthetic)
			{
				if (this.ANSICodePage == 1254)
				{
					return CultureInfo.GetCultureInfo("tr-TR").TextInfo.ToTitleCase(str);
				}
				return CultureInfo.GetCultureInfo("en-US").TextInfo.ToTitleCase(str);
			}
			else
			{
				int length = str.Length;
				if (length == 0)
				{
					return str;
				}
				StringBuilder stringBuilder = new StringBuilder();
				string text = null;
				for (int i = 0; i < length; i++)
				{
					int num;
					UnicodeCategory unicodeCategory = CharUnicodeInfo.InternalGetUnicodeCategory(str, i, out num);
					if (char.CheckLetter(unicodeCategory))
					{
						if (num == 1)
						{
							stringBuilder.Append(TextInfo.nativeGetTitleCaseChar(this.m_pNativeTextInfo, str[i]));
						}
						else
						{
							char value;
							char value2;
							this.ChangeCaseSurrogate(str[i], str[i + 1], out value, out value2, true);
							stringBuilder.Append(value);
							stringBuilder.Append(value2);
						}
						i += num;
						int num2 = i;
						bool flag = unicodeCategory == UnicodeCategory.LowercaseLetter;
						while (i < length)
						{
							unicodeCategory = CharUnicodeInfo.InternalGetUnicodeCategory(str, i, out num);
							if (this.IsLetterCategory(unicodeCategory))
							{
								if (unicodeCategory == UnicodeCategory.LowercaseLetter)
								{
									flag = true;
								}
								i += num;
							}
							else if (str[i] == '\'')
							{
								i++;
								if (flag)
								{
									if (text == null)
									{
										text = this.ToLower(str);
									}
									stringBuilder.Append(text, num2, i - num2);
								}
								else
								{
									stringBuilder.Append(str, num2, i - num2);
								}
								num2 = i;
								flag = true;
							}
							else
							{
								if (this.IsWordSeparator(unicodeCategory))
								{
									break;
								}
								i += num;
							}
						}
						int num3 = i - num2;
						if (num3 > 0)
						{
							if (flag)
							{
								if (text == null)
								{
									text = this.ToLower(str);
								}
								stringBuilder.Append(text, num2, num3);
							}
							else
							{
								stringBuilder.Append(str, num2, num3);
							}
						}
						if (i < length)
						{
							if (num == 1)
							{
								stringBuilder.Append(str[i]);
							}
							else
							{
								stringBuilder.Append(str[i++]);
								stringBuilder.Append(str[i]);
							}
						}
					}
					else if (num == 1)
					{
						stringBuilder.Append(str[i]);
					}
					else
					{
						stringBuilder.Append(str[i++]);
						stringBuilder.Append(str[i]);
					}
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x06002799 RID: 10137 RVA: 0x00076E61 File Offset: 0x00075E61
		[ComVisible(false)]
		public bool IsRightToLeft
		{
			get
			{
				return (this.m_cultureTableRecord.ILINEORIENTATIONS & 32768) != 0;
			}
		}

		// Token: 0x0600279A RID: 10138 RVA: 0x00076E7A File Offset: 0x00075E7A
		private bool IsLetterCategory(UnicodeCategory uc)
		{
			return uc == UnicodeCategory.UppercaseLetter || uc == UnicodeCategory.LowercaseLetter || uc == UnicodeCategory.TitlecaseLetter || uc == UnicodeCategory.ModifierLetter || uc == UnicodeCategory.OtherLetter;
		}

		// Token: 0x0600279B RID: 10139 RVA: 0x00076E91 File Offset: 0x00075E91
		void IDeserializationCallback.OnDeserialization(object sender)
		{
			this.OnDeserialized();
		}

		// Token: 0x0600279C RID: 10140 RVA: 0x00076E9C File Offset: 0x00075E9C
		internal int GetCaseInsensitiveHashCode(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			if (this.m_pNativeTextInfo == null)
			{
				this.OnDeserialized();
			}
			int textInfoID = this.m_textInfoID;
			if (textInfoID == 1055 || textInfoID == 1068)
			{
				str = TextInfo.nativeChangeCaseString(this.m_textInfoID, this.m_pNativeTextInfo, str, true);
			}
			return TextInfo.nativeGetCaseInsHash(str, this.m_pNativeTextInfo);
		}

		// Token: 0x0600279D RID: 10141 RVA: 0x00076F00 File Offset: 0x00075F00
		internal unsafe void ChangeCaseSurrogate(char highSurrogate, char lowSurrogate, out char resultHighSurrogate, out char resultLowSurrogate, bool isToUpper)
		{
			fixed (char* ptr = &resultHighSurrogate, ptr2 = &resultLowSurrogate)
			{
				TextInfo.nativeChangeCaseSurrogate(this.m_pNativeTextInfo, highSurrogate, lowSurrogate, ptr, ptr2, isToUpper);
			}
		}

		// Token: 0x0600279E RID: 10142
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void* AllocateDefaultCasingTable(byte* ptr);

		// Token: 0x0600279F RID: 10143
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void* nativeGetInvariantTextInfo();

		// Token: 0x060027A0 RID: 10144
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void* InternalAllocateCasingTable(byte* ptr, int exceptionIndex);

		// Token: 0x060027A1 RID: 10145
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern int nativeGetCaseInsHash(string str, void* pNativeTextInfo);

		// Token: 0x060027A2 RID: 10146
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern char nativeGetTitleCaseChar(void* pNativeTextInfo, char ch);

		// Token: 0x060027A3 RID: 10147
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern char nativeChangeCaseChar(int win32LangID, void* pNativeTextInfo, char ch, bool isToUpper);

		// Token: 0x060027A4 RID: 10148
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern string nativeChangeCaseString(int win32LangID, void* pNativeTextInfo, string str, bool isToUpper);

		// Token: 0x060027A5 RID: 10149
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern void nativeChangeCaseSurrogate(void* pNativeTextInfo, char highSurrogate, char lowSurrogate, char* resultHighSurrogate, char* resultLowSurrogate, bool isToUpper);

		// Token: 0x060027A6 RID: 10150
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern int nativeCompareOrdinalIgnoreCase(void* pNativeTextInfo, string str1, string str2);

		// Token: 0x060027A7 RID: 10151
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern int nativeCompareOrdinalIgnoreCaseEx(void* pNativeTextInfo, string strA, int indexA, string strB, int indexB, int length);

		// Token: 0x060027A8 RID: 10152
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern int nativeGetHashCodeOrdinalIgnoreCase(void* pNativeTextInfo, string s);

		// Token: 0x060027A9 RID: 10153
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern int nativeIndexOfStringOrdinalIgnoreCase(void* pNativeTextInfo, string str, string value, int startIndex, int count);

		// Token: 0x060027AA RID: 10154
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern int nativeLastIndexOfStringOrdinalIgnoreCase(void* pNativeTextInfo, string str, string value, int startIndex, int count);

		// Token: 0x060027AB RID: 10155
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern int nativeIndexOfCharOrdinalIgnoreCase(void* pNativeTextInfo, string str, char value, int startIndex, int count);

		// Token: 0x060027AC RID: 10156
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern int nativeLastIndexOfCharOrdinalIgnoreCase(void* pNativeTextInfo, string str, char value, int startIndex, int count);

		// Token: 0x040011FA RID: 4602
		private const string CASING_FILE_NAME = "l_intl.nlp";

		// Token: 0x040011FB RID: 4603
		private const string CASING_EXCEPTIONS_FILE_NAME = "l_except.nlp";

		// Token: 0x040011FC RID: 4604
		private const int wordSeparatorMask = 536672256;

		// Token: 0x040011FD RID: 4605
		internal const int TurkishAnsiCodepage = 1254;

		// Token: 0x040011FE RID: 4606
		[OptionalField(VersionAdded = 2)]
		private string m_listSeparator;

		// Token: 0x040011FF RID: 4607
		[OptionalField(VersionAdded = 2)]
		private bool m_isReadOnly;

		// Token: 0x04001200 RID: 4608
		[NonSerialized]
		private int m_textInfoID;

		// Token: 0x04001201 RID: 4609
		[NonSerialized]
		private string m_name;

		// Token: 0x04001202 RID: 4610
		[NonSerialized]
		private CultureTableRecord m_cultureTableRecord;

		// Token: 0x04001203 RID: 4611
		[NonSerialized]
		private TextInfo m_casingTextInfo;

		// Token: 0x04001204 RID: 4612
		[NonSerialized]
		private unsafe void* m_pNativeTextInfo;

		// Token: 0x04001205 RID: 4613
		private unsafe static void* m_pInvariantNativeTextInfo;

		// Token: 0x04001206 RID: 4614
		private unsafe static void* m_pDefaultCasingTable;

		// Token: 0x04001207 RID: 4615
		private unsafe static byte* m_pDataTable;

		// Token: 0x04001208 RID: 4616
		private static int m_exceptionCount;

		// Token: 0x04001209 RID: 4617
		private unsafe static TextInfo.ExceptionTableItem* m_exceptionTable;

		// Token: 0x0400120A RID: 4618
		private unsafe static byte* m_pExceptionFile;

		// Token: 0x0400120B RID: 4619
		private static long[] m_exceptionNativeTextInfo;

		// Token: 0x0400120C RID: 4620
		private static object s_InternalSyncObject;

		// Token: 0x0400120D RID: 4621
		[OptionalField(VersionAdded = 2)]
		private string customCultureName;

		// Token: 0x0400120E RID: 4622
		internal int m_nDataItem;

		// Token: 0x0400120F RID: 4623
		internal bool m_useUserOverride;

		// Token: 0x04001210 RID: 4624
		internal int m_win32LangID;

		// Token: 0x020003CB RID: 971
		[StructLayout(LayoutKind.Explicit)]
		internal struct TextInfoDataHeader
		{
			// Token: 0x04001211 RID: 4625
			[FieldOffset(0)]
			internal char TableName;

			// Token: 0x04001212 RID: 4626
			[FieldOffset(32)]
			internal ushort version;

			// Token: 0x04001213 RID: 4627
			[FieldOffset(40)]
			internal uint OffsetToUpperCasingTable;

			// Token: 0x04001214 RID: 4628
			[FieldOffset(44)]
			internal uint OffsetToLowerCasingTable;

			// Token: 0x04001215 RID: 4629
			[FieldOffset(48)]
			internal uint OffsetToTitleCaseTable;

			// Token: 0x04001216 RID: 4630
			[FieldOffset(52)]
			internal uint PlaneOffset;

			// Token: 0x04001217 RID: 4631
			[FieldOffset(180)]
			internal ushort exceptionCount;

			// Token: 0x04001218 RID: 4632
			[FieldOffset(182)]
			internal ushort exceptionLangId;
		}

		// Token: 0x020003CC RID: 972
		[StructLayout(LayoutKind.Sequential, Pack = 2)]
		internal struct ExceptionTableItem
		{
			// Token: 0x04001219 RID: 4633
			internal ushort langID;

			// Token: 0x0400121A RID: 4634
			internal ushort exceptIndex;
		}
	}
}
