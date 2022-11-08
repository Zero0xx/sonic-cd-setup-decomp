using System;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Threading;
using Microsoft.Win32;

namespace System.Globalization
{
	// Token: 0x02000393 RID: 915
	[ComVisible(true)]
	[Serializable]
	public class CompareInfo : IDeserializationCallback
	{
		// Token: 0x060023E8 RID: 9192
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern byte[] nativeCreateSortKey(void* pSortingFile, string pString, int dwFlags, int win32LCID);

		// Token: 0x060023E9 RID: 9193
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern int nativeGetGlobalizedHashCode(void* pSortingFile, string pString, int dwFlags, int win32LCID);

		// Token: 0x060023EA RID: 9194
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern bool nativeIsSortable(void* pSortingFile, string pString);

		// Token: 0x060023EB RID: 9195
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int nativeCompareString(int lcid, string string1, int offset1, int length1, string string2, int offset2, int length2, int flags);

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x060023EC RID: 9196 RVA: 0x0005BDD4 File Offset: 0x0005ADD4
		private static object InternalSyncObject
		{
			get
			{
				if (CompareInfo.s_InternalSyncObject == null)
				{
					object value = new object();
					Interlocked.CompareExchange(ref CompareInfo.s_InternalSyncObject, value, null);
				}
				return CompareInfo.s_InternalSyncObject;
			}
		}

		// Token: 0x060023ED RID: 9197 RVA: 0x0005BE00 File Offset: 0x0005AE00
		public static CompareInfo GetCompareInfo(int culture, Assembly assembly)
		{
			return CompareInfo.GetCompareInfoWithPrefixedLcid(culture, assembly, 0);
		}

		// Token: 0x060023EE RID: 9198 RVA: 0x0005BE0C File Offset: 0x0005AE0C
		private static CompareInfo GetCompareInfoWithPrefixedLcid(int cultureKey, Assembly assembly, int prefix)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			int cultureId = cultureKey & ~prefix;
			if (CultureTableRecord.IsCustomCultureId(cultureId))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_CustomCultureCannotBePassedByNumber", new object[]
				{
					"culture"
				}));
			}
			if (assembly != typeof(object).Module.Assembly)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_OnlyMscorlib"));
			}
			GlobalizationAssembly globalizationAssembly = GlobalizationAssembly.GetGlobalizationAssembly(assembly);
			object obj = globalizationAssembly.compareInfoCache[cultureKey];
			if (obj == null)
			{
				lock (CompareInfo.InternalSyncObject)
				{
					if ((obj = globalizationAssembly.compareInfoCache[cultureKey]) == null)
					{
						obj = new CompareInfo(globalizationAssembly, cultureId);
						Thread.MemoryBarrier();
						globalizationAssembly.compareInfoCache[cultureKey] = obj;
					}
				}
			}
			return (CompareInfo)obj;
		}

		// Token: 0x060023EF RID: 9199 RVA: 0x0005BEF8 File Offset: 0x0005AEF8
		private static CompareInfo GetCompareInfoByName(string name, Assembly assembly)
		{
			CultureInfo cultureInfo = CultureInfo.GetCultureInfo(name);
			if (cultureInfo.IsNeutralCulture && !CultureTableRecord.IsCustomCultureId(cultureInfo.cultureID))
			{
				if (cultureInfo.cultureID == 31748)
				{
					cultureInfo = CultureInfo.GetCultureInfo(3076);
				}
				else
				{
					cultureInfo = CultureInfo.GetCultureInfo(cultureInfo.CompareInfoId);
				}
			}
			int num = cultureInfo.CompareInfoId;
			if (cultureInfo.Name == "zh-CHS" || cultureInfo.Name == "zh-CHT")
			{
				num |= int.MinValue;
			}
			CompareInfo compareInfoWithPrefixedLcid;
			if (assembly != null)
			{
				compareInfoWithPrefixedLcid = CompareInfo.GetCompareInfoWithPrefixedLcid(num, assembly, int.MinValue);
			}
			else
			{
				compareInfoWithPrefixedLcid = CompareInfo.GetCompareInfoWithPrefixedLcid(num, int.MinValue);
			}
			compareInfoWithPrefixedLcid.m_name = cultureInfo.SortName;
			return compareInfoWithPrefixedLcid;
		}

		// Token: 0x060023F0 RID: 9200 RVA: 0x0005BFA8 File Offset: 0x0005AFA8
		public static CompareInfo GetCompareInfo(string name, Assembly assembly)
		{
			if (name == null || assembly == null)
			{
				throw new ArgumentNullException((name == null) ? "name" : "assembly");
			}
			if (assembly != typeof(object).Module.Assembly)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_OnlyMscorlib"));
			}
			return CompareInfo.GetCompareInfoByName(name, assembly);
		}

		// Token: 0x060023F1 RID: 9201 RVA: 0x0005BFFE File Offset: 0x0005AFFE
		public static CompareInfo GetCompareInfo(int culture)
		{
			return CompareInfo.GetCompareInfoWithPrefixedLcid(culture, 0);
		}

		// Token: 0x060023F2 RID: 9202 RVA: 0x0005C008 File Offset: 0x0005B008
		internal static CompareInfo GetCompareInfoWithPrefixedLcid(int cultureKey, int prefix)
		{
			int cultureId = cultureKey & ~prefix;
			if (CultureTableRecord.IsCustomCultureId(cultureId))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_CustomCultureCannotBePassedByNumber", new object[]
				{
					"culture"
				}));
			}
			object obj = GlobalizationAssembly.DefaultInstance.compareInfoCache[cultureKey];
			if (obj == null)
			{
				lock (CompareInfo.InternalSyncObject)
				{
					if ((obj = GlobalizationAssembly.DefaultInstance.compareInfoCache[cultureKey]) == null)
					{
						obj = new CompareInfo(GlobalizationAssembly.DefaultInstance, cultureId);
						Thread.MemoryBarrier();
						GlobalizationAssembly.DefaultInstance.compareInfoCache[cultureKey] = obj;
					}
				}
			}
			return (CompareInfo)obj;
		}

		// Token: 0x060023F3 RID: 9203 RVA: 0x0005C0C8 File Offset: 0x0005B0C8
		public static CompareInfo GetCompareInfo(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return CompareInfo.GetCompareInfoByName(name, null);
		}

		// Token: 0x060023F4 RID: 9204 RVA: 0x0005C0DF File Offset: 0x0005B0DF
		[ComVisible(false)]
		public static bool IsSortable(char ch)
		{
			return CompareInfo.IsSortable(ch.ToString());
		}

		// Token: 0x060023F5 RID: 9205 RVA: 0x0005C0ED File Offset: 0x0005B0ED
		[ComVisible(false)]
		public static bool IsSortable(string text)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			return text.Length != 0 && CompareInfo.nativeIsSortable(CultureInfo.InvariantCulture.CompareInfo.m_pSortingTable, text);
		}

		// Token: 0x060023F6 RID: 9206 RVA: 0x0005C11C File Offset: 0x0005B11C
		[OnDeserializing]
		private void OnDeserializing(StreamingContext ctx)
		{
			this.culture = -1;
			this.m_sortingLCID = -1;
		}

		// Token: 0x060023F7 RID: 9207 RVA: 0x0005C12C File Offset: 0x0005B12C
		private void OnDeserialized()
		{
			if (this.m_sortingLCID <= 0)
			{
				this.m_sortingLCID = this.GetSortingLCID(this.culture);
			}
			if (this.m_pSortingTable == null && !this.IsSynthetic)
			{
				this.m_pSortingTable = CompareInfo.InitializeCompareInfo(GlobalizationAssembly.DefaultInstance.pNativeGlobalizationAssembly, this.m_sortingLCID);
			}
		}

		// Token: 0x060023F8 RID: 9208 RVA: 0x0005C181 File Offset: 0x0005B181
		[OnDeserialized]
		private void OnDeserialized(StreamingContext ctx)
		{
			this.OnDeserialized();
		}

		// Token: 0x060023F9 RID: 9209 RVA: 0x0005C189 File Offset: 0x0005B189
		[OnSerializing]
		private void OnSerializing(StreamingContext ctx)
		{
			this.win32LCID = this.m_sortingLCID;
		}

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x060023FA RID: 9210 RVA: 0x0005C197 File Offset: 0x0005B197
		[ComVisible(false)]
		public virtual string Name
		{
			get
			{
				if (this.m_name == null)
				{
					this.m_name = CultureInfo.GetCultureInfo(this.culture).SortName;
				}
				return this.m_name;
			}
		}

		// Token: 0x060023FB RID: 9211 RVA: 0x0005C1BD File Offset: 0x0005B1BD
		internal void SetName(string name)
		{
			this.m_name = name;
		}

		// Token: 0x060023FC RID: 9212 RVA: 0x0005C1C8 File Offset: 0x0005B1C8
		internal static void ClearDefaultAssemblyCache()
		{
			lock (CompareInfo.InternalSyncObject)
			{
				GlobalizationAssembly.DefaultInstance.compareInfoCache = new Hashtable(4);
			}
		}

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x060023FD RID: 9213 RVA: 0x0005C20C File Offset: 0x0005B20C
		internal CultureTableRecord CultureTableRecord
		{
			get
			{
				if (this.m_cultureTableRecord == null)
				{
					this.m_cultureTableRecord = CultureInfo.GetCultureInfo(this.m_sortingLCID).m_cultureTableRecord;
				}
				return this.m_cultureTableRecord;
			}
		}

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x060023FE RID: 9214 RVA: 0x0005C232 File Offset: 0x0005B232
		private bool IsSynthetic
		{
			get
			{
				return this.CultureTableRecord.IsSynthetic;
			}
		}

		// Token: 0x060023FF RID: 9215 RVA: 0x0005C240 File Offset: 0x0005B240
		internal CompareInfo(GlobalizationAssembly ga, int culture)
		{
			if (culture < 0)
			{
				throw new ArgumentOutOfRangeException("culture", Environment.GetResourceString("ArgumentOutOfRange_NeedPosNum"));
			}
			this.m_sortingLCID = this.GetSortingLCID(culture);
			if (!this.IsSynthetic)
			{
				this.m_pSortingTable = CompareInfo.InitializeCompareInfo(GlobalizationAssembly.DefaultInstance.pNativeGlobalizationAssembly, this.m_sortingLCID);
			}
			this.culture = culture;
		}

		// Token: 0x06002400 RID: 9216 RVA: 0x0005C2A4 File Offset: 0x0005B2A4
		internal int GetSortingLCID(int culture)
		{
			CultureInfo cultureInfo = CultureInfo.GetCultureInfo(culture);
			if (cultureInfo.m_cultureTableRecord.IsSynthetic)
			{
				return culture;
			}
			int num = cultureInfo.CompareInfoId;
			int sortID = CultureInfo.GetSortID(culture);
			if (sortID != 0)
			{
				if (!cultureInfo.m_cultureTableRecord.IsValidSortID(sortID))
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_CultureNotSupported"), new object[]
					{
						culture
					}), "culture");
				}
				num |= sortID << 16;
			}
			return num;
		}

		// Token: 0x06002401 RID: 9217 RVA: 0x0005C320 File Offset: 0x0005B320
		internal static int GetNativeCompareFlags(CompareOptions options)
		{
			int num = 0;
			if ((options & CompareOptions.IgnoreCase) != CompareOptions.None)
			{
				num |= 1;
			}
			if ((options & CompareOptions.IgnoreKanaType) != CompareOptions.None)
			{
				num |= 65536;
			}
			if ((options & CompareOptions.IgnoreNonSpace) != CompareOptions.None)
			{
				num |= 2;
			}
			if ((options & CompareOptions.IgnoreSymbols) != CompareOptions.None)
			{
				num |= 4;
			}
			if ((options & CompareOptions.IgnoreWidth) != CompareOptions.None)
			{
				num |= 131072;
			}
			if ((options & CompareOptions.StringSort) != CompareOptions.None)
			{
				num |= 4096;
			}
			return num;
		}

		// Token: 0x06002402 RID: 9218 RVA: 0x0005C377 File Offset: 0x0005B377
		public virtual int Compare(string string1, string string2)
		{
			return this.Compare(string1, string2, CompareOptions.None);
		}

		// Token: 0x06002403 RID: 9219 RVA: 0x0005C384 File Offset: 0x0005B384
		public virtual int Compare(string string1, string string2, CompareOptions options)
		{
			if (options == CompareOptions.OrdinalIgnoreCase)
			{
				return string.Compare(string1, string2, StringComparison.OrdinalIgnoreCase);
			}
			if ((options & CompareOptions.Ordinal) != CompareOptions.None)
			{
				if (options != CompareOptions.Ordinal)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_CompareOptionOrdinal"), "options");
				}
				if (string1 == null)
				{
					if (string2 == null)
					{
						return 0;
					}
					return -1;
				}
				else
				{
					if (string2 == null)
					{
						return 1;
					}
					return string.nativeCompareOrdinal(string1, string2, false);
				}
			}
			else
			{
				if ((options & ~(CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreSymbols | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth | CompareOptions.StringSort)) != CompareOptions.None)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidFlag"), "options");
				}
				if (string1 == null)
				{
					if (string2 == null)
					{
						return 0;
					}
					return -1;
				}
				else
				{
					if (string2 == null)
					{
						return 1;
					}
					if (!this.IsSynthetic)
					{
						return CompareInfo.Compare(this.m_pSortingTable, this.m_sortingLCID, string1, string2, options);
					}
					if (options == CompareOptions.Ordinal)
					{
						return CompareInfo.Compare(CultureInfo.InvariantCulture.CompareInfo.m_pSortingTable, this.m_sortingLCID, string1, string2, options);
					}
					return CompareInfo.nativeCompareString(this.m_sortingLCID, string1, 0, string1.Length, string2, 0, string2.Length, CompareInfo.GetNativeCompareFlags(options));
				}
			}
		}

		// Token: 0x06002404 RID: 9220
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern int Compare(void* pSortingTable, int sortingLCID, string string1, string string2, CompareOptions options);

		// Token: 0x06002405 RID: 9221 RVA: 0x0005C471 File Offset: 0x0005B471
		public virtual int Compare(string string1, int offset1, int length1, string string2, int offset2, int length2)
		{
			return this.Compare(string1, offset1, length1, string2, offset2, length2, CompareOptions.None);
		}

		// Token: 0x06002406 RID: 9222 RVA: 0x0005C483 File Offset: 0x0005B483
		public virtual int Compare(string string1, int offset1, string string2, int offset2, CompareOptions options)
		{
			return this.Compare(string1, offset1, (string1 == null) ? 0 : (string1.Length - offset1), string2, offset2, (string2 == null) ? 0 : (string2.Length - offset2), options);
		}

		// Token: 0x06002407 RID: 9223 RVA: 0x0005C4AF File Offset: 0x0005B4AF
		public virtual int Compare(string string1, int offset1, string string2, int offset2)
		{
			return this.Compare(string1, offset1, string2, offset2, CompareOptions.None);
		}

		// Token: 0x06002408 RID: 9224 RVA: 0x0005C4C0 File Offset: 0x0005B4C0
		public virtual int Compare(string string1, int offset1, int length1, string string2, int offset2, int length2, CompareOptions options)
		{
			if (options == CompareOptions.OrdinalIgnoreCase)
			{
				int num = string.Compare(string1, offset1, string2, offset2, (length1 < length2) ? length1 : length2, StringComparison.OrdinalIgnoreCase);
				if (length1 == length2 || num != 0)
				{
					return num;
				}
				if (length1 <= length2)
				{
					return -1;
				}
				return 1;
			}
			else
			{
				if (length1 < 0 || length2 < 0)
				{
					throw new ArgumentOutOfRangeException((length1 < 0) ? "length1" : "length2", Environment.GetResourceString("ArgumentOutOfRange_NeedPosNum"));
				}
				if (offset1 < 0 || offset2 < 0)
				{
					throw new ArgumentOutOfRangeException((offset1 < 0) ? "offset1" : "offset2", Environment.GetResourceString("ArgumentOutOfRange_NeedPosNum"));
				}
				if (offset1 > ((string1 == null) ? 0 : string1.Length) - length1)
				{
					throw new ArgumentOutOfRangeException("string1", Environment.GetResourceString("ArgumentOutOfRange_OffsetLength"));
				}
				if (offset2 > ((string2 == null) ? 0 : string2.Length) - length2)
				{
					throw new ArgumentOutOfRangeException("string2", Environment.GetResourceString("ArgumentOutOfRange_OffsetLength"));
				}
				if ((options & CompareOptions.Ordinal) != CompareOptions.None)
				{
					if (options != CompareOptions.Ordinal)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_CompareOptionOrdinal"), "options");
					}
					if (string1 == null)
					{
						if (string2 == null)
						{
							return 0;
						}
						return -1;
					}
					else
					{
						if (string2 == null)
						{
							return 1;
						}
						int num2 = string.nativeCompareOrdinalEx(string1, offset1, string2, offset2, (length1 < length2) ? length1 : length2);
						if (length1 == length2 || num2 != 0)
						{
							return num2;
						}
						if (length1 <= length2)
						{
							return -1;
						}
						return 1;
					}
				}
				else
				{
					if ((options & ~(CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreSymbols | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth | CompareOptions.StringSort)) != CompareOptions.None)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_InvalidFlag"), "options");
					}
					if (string1 == null)
					{
						if (string2 == null)
						{
							return 0;
						}
						return -1;
					}
					else
					{
						if (string2 == null)
						{
							return 1;
						}
						if (!this.IsSynthetic)
						{
							return CompareInfo.CompareRegion(this.m_pSortingTable, this.m_sortingLCID, string1, offset1, length1, string2, offset2, length2, options);
						}
						if (options == CompareOptions.Ordinal)
						{
							return CompareInfo.CompareRegion(CultureInfo.InvariantCulture.CompareInfo.m_pSortingTable, this.m_sortingLCID, string1, offset1, length1, string2, offset2, length2, options);
						}
						return CompareInfo.nativeCompareString(this.m_sortingLCID, string1, offset1, length1, string2, offset2, length2, CompareInfo.GetNativeCompareFlags(options));
					}
				}
			}
		}

		// Token: 0x06002409 RID: 9225
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern int CompareRegion(void* pSortingTable, int sortingLCID, string string1, int offset1, int length1, string string2, int offset2, int length2, CompareOptions options);

		// Token: 0x0600240A RID: 9226 RVA: 0x0005C6A4 File Offset: 0x0005B6A4
		private unsafe static int FindNLSStringWrap(int lcid, int flags, string src, int start, int cchSrc, string value, int cchValue)
		{
			int result = -1;
			fixed (char* ptr = src)
			{
				fixed (char* lpStringValue = value)
				{
					if (1 == CompareInfo.fFindNLSStringSupported)
					{
						result = Win32Native.FindNLSString(lcid, flags, ptr + start, cchSrc, lpStringValue, cchValue, IntPtr.Zero);
					}
					else
					{
						try
						{
							result = Win32Native.FindNLSString(lcid, flags, ptr + start, cchSrc, lpStringValue, cchValue, IntPtr.Zero);
							CompareInfo.fFindNLSStringSupported = 1;
						}
						catch (EntryPointNotFoundException)
						{
							result = (CompareInfo.fFindNLSStringSupported = -2);
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600240B RID: 9227 RVA: 0x0005C738 File Offset: 0x0005B738
		private bool SyntheticIsPrefix(string source, int start, int length, string prefix, int nativeCompareFlags)
		{
			if (CompareInfo.fFindNLSStringSupported >= 0)
			{
				int num = CompareInfo.FindNLSStringWrap(this.m_sortingLCID, nativeCompareFlags | 1048576, source, start, length, prefix, prefix.Length);
				if (num >= -1)
				{
					return num != -1;
				}
			}
			for (int i = 1; i <= length; i++)
			{
				if (CompareInfo.nativeCompareString(this.m_sortingLCID, prefix, 0, prefix.Length, source, start, i, nativeCompareFlags) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600240C RID: 9228 RVA: 0x0005C7A4 File Offset: 0x0005B7A4
		private int SyntheticIsSuffix(string source, int end, int length, string suffix, int nativeCompareFlags)
		{
			if (CompareInfo.fFindNLSStringSupported >= 0)
			{
				int num = CompareInfo.FindNLSStringWrap(this.m_sortingLCID, nativeCompareFlags | 2097152, source, 0, length, suffix, suffix.Length);
				if (num >= -1)
				{
					return num;
				}
			}
			for (int i = 0; i < length; i++)
			{
				if (CompareInfo.nativeCompareString(this.m_sortingLCID, suffix, 0, suffix.Length, source, end - i, i + 1, nativeCompareFlags) == 0)
				{
					return end - i;
				}
			}
			return -1;
		}

		// Token: 0x0600240D RID: 9229 RVA: 0x0005C810 File Offset: 0x0005B810
		private int SyntheticIndexOf(string source, string value, int start, int length, int nativeCompareFlags)
		{
			if (CompareInfo.fFindNLSStringSupported >= 0)
			{
				int num = CompareInfo.FindNLSStringWrap(this.m_sortingLCID, nativeCompareFlags | 4194304, source, start, length, value, value.Length);
				if (num > -1)
				{
					return num + start;
				}
				if (num == -1)
				{
					return num;
				}
			}
			for (int i = 0; i < length; i++)
			{
				if (this.SyntheticIsPrefix(source, start + i, length - i, value, nativeCompareFlags))
				{
					return start + i;
				}
			}
			return -1;
		}

		// Token: 0x0600240E RID: 9230 RVA: 0x0005C878 File Offset: 0x0005B878
		private int SyntheticLastIndexOf(string source, string value, int startIndex, int length, int nativeCompareFlags)
		{
			if (CompareInfo.fFindNLSStringSupported >= 0)
			{
				int num = CompareInfo.FindNLSStringWrap(this.m_sortingLCID, nativeCompareFlags | 8388608, source, startIndex - length + 1, length, value, value.Length);
				if (num > -1)
				{
					return num + (startIndex - length + 1);
				}
				if (num == -1)
				{
					return num;
				}
			}
			for (int i = 0; i < length; i++)
			{
				int num2 = this.SyntheticIsSuffix(source, startIndex - i, length - i, value, nativeCompareFlags);
				if (num2 >= 0)
				{
					return num2;
				}
			}
			return -1;
		}

		// Token: 0x0600240F RID: 9231 RVA: 0x0005C8EC File Offset: 0x0005B8EC
		public virtual bool IsPrefix(string source, string prefix, CompareOptions options)
		{
			if (source == null || prefix == null)
			{
				throw new ArgumentNullException((source == null) ? "source" : "prefix", Environment.GetResourceString("ArgumentNull_String"));
			}
			if (prefix.Length == 0)
			{
				return true;
			}
			if (options == CompareOptions.OrdinalIgnoreCase)
			{
				return source.StartsWith(prefix, StringComparison.OrdinalIgnoreCase);
			}
			if ((options & ~(CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreSymbols | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth)) != CompareOptions.None && options != CompareOptions.Ordinal)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidFlag"), "options");
			}
			if (!this.IsSynthetic)
			{
				return CompareInfo.nativeIsPrefix(this.m_pSortingTable, this.m_sortingLCID, source, prefix, options);
			}
			if (options == CompareOptions.Ordinal)
			{
				return CompareInfo.nativeIsPrefix(CultureInfo.InvariantCulture.CompareInfo.m_pSortingTable, this.m_sortingLCID, source, prefix, options);
			}
			return this.SyntheticIsPrefix(source, 0, source.Length, prefix, CompareInfo.GetNativeCompareFlags(options));
		}

		// Token: 0x06002410 RID: 9232
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern bool nativeIsPrefix(void* pSortingTable, int sortingLCID, string source, string prefix, CompareOptions options);

		// Token: 0x06002411 RID: 9233 RVA: 0x0005C9B6 File Offset: 0x0005B9B6
		public virtual bool IsPrefix(string source, string prefix)
		{
			return this.IsPrefix(source, prefix, CompareOptions.None);
		}

		// Token: 0x06002412 RID: 9234 RVA: 0x0005C9C4 File Offset: 0x0005B9C4
		public virtual bool IsSuffix(string source, string suffix, CompareOptions options)
		{
			if (source == null || suffix == null)
			{
				throw new ArgumentNullException((source == null) ? "source" : "suffix", Environment.GetResourceString("ArgumentNull_String"));
			}
			if (suffix.Length == 0)
			{
				return true;
			}
			if (options == CompareOptions.OrdinalIgnoreCase)
			{
				return source.EndsWith(suffix, StringComparison.OrdinalIgnoreCase);
			}
			if ((options & ~(CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreSymbols | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth)) != CompareOptions.None && options != CompareOptions.Ordinal)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidFlag"), "options");
			}
			if (!this.IsSynthetic)
			{
				return CompareInfo.nativeIsSuffix(this.m_pSortingTable, this.m_sortingLCID, source, suffix, options);
			}
			if (options == CompareOptions.Ordinal)
			{
				return CompareInfo.nativeIsSuffix(CultureInfo.InvariantCulture.CompareInfo.m_pSortingTable, this.m_sortingLCID, source, suffix, options);
			}
			return this.SyntheticIsSuffix(source, source.Length - 1, source.Length, suffix, CompareInfo.GetNativeCompareFlags(options)) >= 0;
		}

		// Token: 0x06002413 RID: 9235 RVA: 0x0005CA9B File Offset: 0x0005BA9B
		public virtual bool IsSuffix(string source, string suffix)
		{
			return this.IsSuffix(source, suffix, CompareOptions.None);
		}

		// Token: 0x06002414 RID: 9236
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern bool nativeIsSuffix(void* pSortingTable, int sortingLCID, string source, string prefix, CompareOptions options);

		// Token: 0x06002415 RID: 9237 RVA: 0x0005CAA6 File Offset: 0x0005BAA6
		public virtual int IndexOf(string source, char value)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return this.IndexOf(source, value, 0, source.Length, CompareOptions.None);
		}

		// Token: 0x06002416 RID: 9238 RVA: 0x0005CAC6 File Offset: 0x0005BAC6
		public virtual int IndexOf(string source, string value)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return this.IndexOf(source, value, 0, source.Length, CompareOptions.None);
		}

		// Token: 0x06002417 RID: 9239 RVA: 0x0005CAE6 File Offset: 0x0005BAE6
		public virtual int IndexOf(string source, char value, CompareOptions options)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return this.IndexOf(source, value, 0, source.Length, options);
		}

		// Token: 0x06002418 RID: 9240 RVA: 0x0005CB06 File Offset: 0x0005BB06
		public virtual int IndexOf(string source, string value, CompareOptions options)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return this.IndexOf(source, value, 0, source.Length, options);
		}

		// Token: 0x06002419 RID: 9241 RVA: 0x0005CB26 File Offset: 0x0005BB26
		public virtual int IndexOf(string source, char value, int startIndex)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return this.IndexOf(source, value, startIndex, source.Length - startIndex, CompareOptions.None);
		}

		// Token: 0x0600241A RID: 9242 RVA: 0x0005CB48 File Offset: 0x0005BB48
		public virtual int IndexOf(string source, string value, int startIndex)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return this.IndexOf(source, value, startIndex, source.Length - startIndex, CompareOptions.None);
		}

		// Token: 0x0600241B RID: 9243 RVA: 0x0005CB6A File Offset: 0x0005BB6A
		public virtual int IndexOf(string source, char value, int startIndex, CompareOptions options)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return this.IndexOf(source, value, startIndex, source.Length - startIndex, options);
		}

		// Token: 0x0600241C RID: 9244 RVA: 0x0005CB8D File Offset: 0x0005BB8D
		public virtual int IndexOf(string source, string value, int startIndex, CompareOptions options)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return this.IndexOf(source, value, startIndex, source.Length - startIndex, options);
		}

		// Token: 0x0600241D RID: 9245 RVA: 0x0005CBB0 File Offset: 0x0005BBB0
		public virtual int IndexOf(string source, char value, int startIndex, int count)
		{
			return this.IndexOf(source, value, startIndex, count, CompareOptions.None);
		}

		// Token: 0x0600241E RID: 9246 RVA: 0x0005CBBE File Offset: 0x0005BBBE
		public virtual int IndexOf(string source, string value, int startIndex, int count)
		{
			return this.IndexOf(source, value, startIndex, count, CompareOptions.None);
		}

		// Token: 0x0600241F RID: 9247 RVA: 0x0005CBCC File Offset: 0x0005BBCC
		public virtual int IndexOf(string source, char value, int startIndex, int count, CompareOptions options)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (startIndex < 0 || startIndex > source.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			if (count < 0 || startIndex > source.Length - count)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_Count"));
			}
			if (options == CompareOptions.OrdinalIgnoreCase)
			{
				return TextInfo.nativeIndexOfCharOrdinalIgnoreCase(TextInfo.InvariantNativeTextInfo, source, value, startIndex, count);
			}
			if ((options & ~(CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreSymbols | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth)) != CompareOptions.None && options != CompareOptions.Ordinal)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidFlag"), "options");
			}
			if (!this.IsSynthetic)
			{
				return CompareInfo.IndexOfChar(this.m_pSortingTable, this.m_sortingLCID, source, value, startIndex, count, (int)options);
			}
			if (options == CompareOptions.Ordinal)
			{
				return CompareInfo.IndexOfChar(CultureInfo.InvariantCulture.CompareInfo.m_pSortingTable, this.m_sortingLCID, source, value, startIndex, count, (int)options);
			}
			return this.SyntheticIndexOf(source, new string(value, 1), startIndex, count, CompareInfo.GetNativeCompareFlags(options));
		}

		// Token: 0x06002420 RID: 9248 RVA: 0x0005CCD4 File Offset: 0x0005BCD4
		public virtual int IndexOf(string source, string value, int startIndex, int count, CompareOptions options)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (startIndex > source.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			if (source.Length == 0)
			{
				if (value.Length == 0)
				{
					return 0;
				}
				return -1;
			}
			else
			{
				if (startIndex < 0)
				{
					throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				if (count < 0 || startIndex > source.Length - count)
				{
					throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_Count"));
				}
				if (options == CompareOptions.OrdinalIgnoreCase)
				{
					return TextInfo.IndexOfStringOrdinalIgnoreCase(source, value, startIndex, count);
				}
				if ((options & ~(CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreSymbols | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth)) != CompareOptions.None && options != CompareOptions.Ordinal)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidFlag"), "options");
				}
				if (!this.IsSynthetic)
				{
					return CompareInfo.IndexOfString(this.m_pSortingTable, this.m_sortingLCID, source, value, startIndex, count, (int)options);
				}
				if (options == CompareOptions.Ordinal)
				{
					return CompareInfo.IndexOfString(CultureInfo.InvariantCulture.CompareInfo.m_pSortingTable, this.m_sortingLCID, source, value, startIndex, count, (int)options);
				}
				return this.SyntheticIndexOf(source, value, startIndex, count, CompareInfo.GetNativeCompareFlags(options));
			}
		}

		// Token: 0x06002421 RID: 9249
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern int IndexOfChar(void* pSortingTable, int sortingLCID, string source, char value, int startIndex, int count, int options);

		// Token: 0x06002422 RID: 9250
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern int IndexOfString(void* pSortingTable, int sortingLCID, string source, string value, int startIndex, int count, int options);

		// Token: 0x06002423 RID: 9251 RVA: 0x0005CE05 File Offset: 0x0005BE05
		public virtual int LastIndexOf(string source, char value)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return this.LastIndexOf(source, value, source.Length - 1, source.Length, CompareOptions.None);
		}

		// Token: 0x06002424 RID: 9252 RVA: 0x0005CE2C File Offset: 0x0005BE2C
		public virtual int LastIndexOf(string source, string value)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return this.LastIndexOf(source, value, source.Length - 1, source.Length, CompareOptions.None);
		}

		// Token: 0x06002425 RID: 9253 RVA: 0x0005CE53 File Offset: 0x0005BE53
		public virtual int LastIndexOf(string source, char value, CompareOptions options)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return this.LastIndexOf(source, value, source.Length - 1, source.Length, options);
		}

		// Token: 0x06002426 RID: 9254 RVA: 0x0005CE7A File Offset: 0x0005BE7A
		public virtual int LastIndexOf(string source, string value, CompareOptions options)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return this.LastIndexOf(source, value, source.Length - 1, source.Length, options);
		}

		// Token: 0x06002427 RID: 9255 RVA: 0x0005CEA1 File Offset: 0x0005BEA1
		public virtual int LastIndexOf(string source, char value, int startIndex)
		{
			return this.LastIndexOf(source, value, startIndex, startIndex + 1, CompareOptions.None);
		}

		// Token: 0x06002428 RID: 9256 RVA: 0x0005CEB0 File Offset: 0x0005BEB0
		public virtual int LastIndexOf(string source, string value, int startIndex)
		{
			return this.LastIndexOf(source, value, startIndex, startIndex + 1, CompareOptions.None);
		}

		// Token: 0x06002429 RID: 9257 RVA: 0x0005CEBF File Offset: 0x0005BEBF
		public virtual int LastIndexOf(string source, char value, int startIndex, CompareOptions options)
		{
			return this.LastIndexOf(source, value, startIndex, startIndex + 1, options);
		}

		// Token: 0x0600242A RID: 9258 RVA: 0x0005CECF File Offset: 0x0005BECF
		public virtual int LastIndexOf(string source, string value, int startIndex, CompareOptions options)
		{
			return this.LastIndexOf(source, value, startIndex, startIndex + 1, options);
		}

		// Token: 0x0600242B RID: 9259 RVA: 0x0005CEDF File Offset: 0x0005BEDF
		public virtual int LastIndexOf(string source, char value, int startIndex, int count)
		{
			return this.LastIndexOf(source, value, startIndex, count, CompareOptions.None);
		}

		// Token: 0x0600242C RID: 9260 RVA: 0x0005CEED File Offset: 0x0005BEED
		public virtual int LastIndexOf(string source, string value, int startIndex, int count)
		{
			return this.LastIndexOf(source, value, startIndex, count, CompareOptions.None);
		}

		// Token: 0x0600242D RID: 9261 RVA: 0x0005CEFC File Offset: 0x0005BEFC
		public virtual int LastIndexOf(string source, char value, int startIndex, int count, CompareOptions options)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if ((options & ~(CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreSymbols | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth)) != CompareOptions.None && options != CompareOptions.Ordinal && options != CompareOptions.OrdinalIgnoreCase)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidFlag"), "options");
			}
			if (source.Length == 0 && (startIndex == -1 || startIndex == 0))
			{
				return -1;
			}
			if (startIndex < 0 || startIndex > source.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			if (startIndex == source.Length)
			{
				startIndex--;
				if (count > 0)
				{
					count--;
				}
			}
			if (count < 0 || startIndex - count + 1 < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_Count"));
			}
			if (options == CompareOptions.OrdinalIgnoreCase)
			{
				return TextInfo.nativeLastIndexOfCharOrdinalIgnoreCase(TextInfo.InvariantNativeTextInfo, source, value, startIndex, count);
			}
			if (!this.IsSynthetic)
			{
				return CompareInfo.LastIndexOfChar(this.m_pSortingTable, this.m_sortingLCID, source, value, startIndex, count, (int)options);
			}
			if (options == CompareOptions.Ordinal)
			{
				return CompareInfo.LastIndexOfChar(CultureInfo.InvariantCulture.CompareInfo.m_pSortingTable, this.m_sortingLCID, source, value, startIndex, count, (int)options);
			}
			return this.SyntheticLastIndexOf(source, new string(value, 1), startIndex, count, CompareInfo.GetNativeCompareFlags(options));
		}

		// Token: 0x0600242E RID: 9262 RVA: 0x0005D034 File Offset: 0x0005C034
		public virtual int LastIndexOf(string source, string value, int startIndex, int count, CompareOptions options)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if ((options & ~(CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreSymbols | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth)) != CompareOptions.None && options != CompareOptions.Ordinal && options != CompareOptions.OrdinalIgnoreCase)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidFlag"), "options");
			}
			if (source.Length == 0 && (startIndex == -1 || startIndex == 0))
			{
				if (value.Length != 0)
				{
					return -1;
				}
				return 0;
			}
			else
			{
				if (startIndex < 0 || startIndex > source.Length)
				{
					throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				if (startIndex == source.Length)
				{
					startIndex--;
					if (count > 0)
					{
						count--;
					}
					if (value.Length == 0 && count >= 0 && startIndex - count + 1 >= 0)
					{
						return startIndex;
					}
				}
				if (count < 0 || startIndex - count + 1 < 0)
				{
					throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_Count"));
				}
				if (options == CompareOptions.OrdinalIgnoreCase)
				{
					return TextInfo.LastIndexOfStringOrdinalIgnoreCase(source, value, startIndex, count);
				}
				if (!this.IsSynthetic)
				{
					return CompareInfo.LastIndexOfString(this.m_pSortingTable, this.m_sortingLCID, source, value, startIndex, count, (int)options);
				}
				if (options == CompareOptions.Ordinal)
				{
					return CompareInfo.LastIndexOfString(CultureInfo.InvariantCulture.CompareInfo.m_pSortingTable, this.m_sortingLCID, source, value, startIndex, count, (int)options);
				}
				return this.SyntheticLastIndexOf(source, value, startIndex, count, CompareInfo.GetNativeCompareFlags(options));
			}
		}

		// Token: 0x0600242F RID: 9263
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern int LastIndexOfChar(void* pSortingTable, int sortingLCID, string source, char value, int startIndex, int count, int options);

		// Token: 0x06002430 RID: 9264
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern int LastIndexOfString(void* pSortingTable, int sortingLCID, string source, string value, int startIndex, int count, int options);

		// Token: 0x06002431 RID: 9265 RVA: 0x0005D18E File Offset: 0x0005C18E
		public virtual SortKey GetSortKey(string source, CompareOptions options)
		{
			if (this.IsSynthetic)
			{
				return new SortKey(this.m_sortingLCID, source, options);
			}
			return new SortKey(this.m_pSortingTable, this.m_sortingLCID, source, options);
		}

		// Token: 0x06002432 RID: 9266 RVA: 0x0005D1B9 File Offset: 0x0005C1B9
		public virtual SortKey GetSortKey(string source)
		{
			if (this.IsSynthetic)
			{
				return new SortKey(this.m_sortingLCID, source, CompareOptions.None);
			}
			return new SortKey(this.m_pSortingTable, this.m_sortingLCID, source, CompareOptions.None);
		}

		// Token: 0x06002433 RID: 9267 RVA: 0x0005D1E4 File Offset: 0x0005C1E4
		public override bool Equals(object value)
		{
			CompareInfo compareInfo = value as CompareInfo;
			return compareInfo != null && this.m_sortingLCID == compareInfo.m_sortingLCID && this.Name.Equals(compareInfo.Name);
		}

		// Token: 0x06002434 RID: 9268 RVA: 0x0005D21E File Offset: 0x0005C21E
		public override int GetHashCode()
		{
			return this.Name.GetHashCode();
		}

		// Token: 0x06002435 RID: 9269 RVA: 0x0005D22C File Offset: 0x0005C22C
		internal int GetHashCodeOfString(string source, CompareOptions options)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if ((options & ~(CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreSymbols | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth)) != CompareOptions.None)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidFlag"), "options");
			}
			if (source.Length == 0)
			{
				return 0;
			}
			if (this.IsSynthetic)
			{
				return CultureInfo.InvariantCulture.CompareInfo.GetHashCodeOfString(source, options);
			}
			return CompareInfo.nativeGetGlobalizedHashCode(this.m_pSortingTable, source, (int)options, this.m_sortingLCID);
		}

		// Token: 0x06002436 RID: 9270 RVA: 0x0005D299 File Offset: 0x0005C299
		public override string ToString()
		{
			return "CompareInfo - " + this.culture;
		}

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x06002437 RID: 9271 RVA: 0x0005D2B0 File Offset: 0x0005C2B0
		public int LCID
		{
			get
			{
				return this.culture;
			}
		}

		// Token: 0x06002438 RID: 9272 RVA: 0x0005D2B8 File Offset: 0x0005C2B8
		private unsafe static void* InitializeCompareInfo(void* pNativeGlobalizationAssembly, int sortingLCID)
		{
			void* result = null;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				Monitor.ReliableEnter(typeof(CultureTableRecord), ref flag);
				result = CompareInfo.InitializeNativeCompareInfo(pNativeGlobalizationAssembly, sortingLCID);
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(typeof(CultureTableRecord));
				}
			}
			return result;
		}

		// Token: 0x06002439 RID: 9273 RVA: 0x0005D310 File Offset: 0x0005C310
		void IDeserializationCallback.OnDeserialization(object sender)
		{
			this.OnDeserialized();
		}

		// Token: 0x0600243A RID: 9274
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void* InitializeNativeCompareInfo(void* pNativeGlobalizationAssembly, int sortingLCID);

		// Token: 0x04000F5E RID: 3934
		private const CompareOptions ValidIndexMaskOffFlags = ~(CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreSymbols | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth);

		// Token: 0x04000F5F RID: 3935
		private const CompareOptions ValidCompareMaskOffFlags = ~(CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreSymbols | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth | CompareOptions.StringSort);

		// Token: 0x04000F60 RID: 3936
		private const CompareOptions ValidHashCodeOfStringMaskOffFlags = ~(CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreSymbols | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth);

		// Token: 0x04000F61 RID: 3937
		private const int TraditionalChineseCultureId = 31748;

		// Token: 0x04000F62 RID: 3938
		private const int HongKongCultureId = 3076;

		// Token: 0x04000F63 RID: 3939
		internal const int CHT_CHS_LCID_COMPAREINFO_KEY_FLAG = -2147483648;

		// Token: 0x04000F64 RID: 3940
		private const int NORM_IGNORECASE = 1;

		// Token: 0x04000F65 RID: 3941
		private const int NORM_IGNOREKANATYPE = 65536;

		// Token: 0x04000F66 RID: 3942
		private const int NORM_IGNORENONSPACE = 2;

		// Token: 0x04000F67 RID: 3943
		private const int NORM_IGNORESYMBOLS = 4;

		// Token: 0x04000F68 RID: 3944
		private const int NORM_IGNOREWIDTH = 131072;

		// Token: 0x04000F69 RID: 3945
		private const int SORT_STRINGSORT = 4096;

		// Token: 0x04000F6A RID: 3946
		private static object s_InternalSyncObject;

		// Token: 0x04000F6B RID: 3947
		private int win32LCID;

		// Token: 0x04000F6C RID: 3948
		private int culture;

		// Token: 0x04000F6D RID: 3949
		[NonSerialized]
		internal unsafe void* m_pSortingTable;

		// Token: 0x04000F6E RID: 3950
		[NonSerialized]
		private int m_sortingLCID;

		// Token: 0x04000F6F RID: 3951
		[NonSerialized]
		private CultureTableRecord m_cultureTableRecord;

		// Token: 0x04000F70 RID: 3952
		[OptionalField(VersionAdded = 2)]
		private string m_name;

		// Token: 0x04000F71 RID: 3953
		[NonSerialized]
		private static int fFindNLSStringSupported;
	}
}
