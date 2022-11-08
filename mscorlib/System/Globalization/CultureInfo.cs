using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Threading;
using Microsoft.Win32;

namespace System.Globalization
{
	// Token: 0x02000394 RID: 916
	[ComVisible(true)]
	[Serializable]
	public class CultureInfo : ICloneable, IFormatProvider
	{
		// Token: 0x0600243B RID: 9275
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool IsValidLCID(int LCID, int flag);

		// Token: 0x0600243C RID: 9276
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool IsWin9xInstalledCulture(string cultureKey, int LCID);

		// Token: 0x0600243D RID: 9277
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern string nativeGetUserDefaultLCID(int* LCID, int lcidType);

		// Token: 0x0600243E RID: 9278
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern string nativeGetUserDefaultUILanguage(int* LCID);

		// Token: 0x0600243F RID: 9279
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern string nativeGetSystemDefaultUILanguage(int* LCID);

		// Token: 0x06002440 RID: 9280
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool nativeSetThreadLocale(int LCID);

		// Token: 0x06002441 RID: 9281
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string nativeGetLocaleInfo(int LCID, int field);

		// Token: 0x06002442 RID: 9282
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int nativeGetCurrentCalendar();

		// Token: 0x06002443 RID: 9283
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool nativeGetDTFIUserValues(int lcid, ref DTFIUserOverrideValues values);

		// Token: 0x06002444 RID: 9284
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool nativeGetNFIUserValues(int lcid, NumberFormatInfo nfi);

		// Token: 0x06002445 RID: 9285
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool nativeGetCultureData(int lcid, ref CultureData cultureData);

		// Token: 0x06002446 RID: 9286
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool nativeEnumSystemLocales(out int[] localesArray);

		// Token: 0x06002447 RID: 9287
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string nativeGetCultureName(int lcid, bool useSNameLCType, bool getMonthName);

		// Token: 0x06002448 RID: 9288
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string nativeGetWindowsDirectory();

		// Token: 0x06002449 RID: 9289
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool nativeFileExists(string fileName);

		// Token: 0x0600244A RID: 9290
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern int* nativeGetStaticInt32DataTable(int type, out int tableSize);

		// Token: 0x0600244B RID: 9291 RVA: 0x0005D318 File Offset: 0x0005C318
		internal unsafe static int GetNativeSortKey(int lcid, int flags, string source, int cchSrc, out byte[] sortKey)
		{
			sortKey = null;
			if (string.IsNullOrEmpty(source) || cchSrc == 0)
			{
				sortKey = new byte[0];
				source = "\0";
				cchSrc = 1;
			}
			int num;
			fixed (char* src = source)
			{
				num = Win32Native.LCMapStringW(lcid, flags | 1024, src, cchSrc, null, 0);
				if (num == 0)
				{
					return -1;
				}
				if (sortKey == null)
				{
					sortKey = new byte[num];
					fixed (byte* ptr = sortKey)
					{
						num = Win32Native.LCMapStringW(lcid, flags | 1024, src, cchSrc, (char*)ptr, num);
					}
				}
			}
			return num;
		}

		// Token: 0x0600244C RID: 9292 RVA: 0x0005D3B8 File Offset: 0x0005C3B8
		static CultureInfo()
		{
			if (CultureInfo.m_InvariantCultureInfo == null)
			{
				CultureInfo.m_InvariantCultureInfo = new CultureInfo(127, false)
				{
					m_isReadOnly = true
				};
			}
			CultureInfo.m_userDefaultCulture = (CultureInfo.m_userDefaultUICulture = CultureInfo.m_InvariantCultureInfo);
			CultureInfo.m_userDefaultCulture = CultureInfo.InitUserDefaultCulture();
			CultureInfo.m_userDefaultUICulture = CultureInfo.InitUserDefaultUICulture();
		}

		// Token: 0x0600244D RID: 9293 RVA: 0x0005D408 File Offset: 0x0005C408
		private unsafe static CultureInfo InitUserDefaultCulture()
		{
			int preferLCID;
			string fallbackToString = CultureInfo.nativeGetUserDefaultLCID(&preferLCID, 1024);
			CultureInfo cultureByLCIDOrName = CultureInfo.GetCultureByLCIDOrName(preferLCID, fallbackToString);
			if (cultureByLCIDOrName == null)
			{
				fallbackToString = CultureInfo.nativeGetUserDefaultLCID(&preferLCID, 2048);
				cultureByLCIDOrName = CultureInfo.GetCultureByLCIDOrName(preferLCID, fallbackToString);
				if (cultureByLCIDOrName == null)
				{
					return CultureInfo.InvariantCulture;
				}
			}
			cultureByLCIDOrName.m_isReadOnly = true;
			return cultureByLCIDOrName;
		}

		// Token: 0x0600244E RID: 9294 RVA: 0x0005D458 File Offset: 0x0005C458
		private unsafe static CultureInfo InitUserDefaultUICulture()
		{
			int num;
			string text = CultureInfo.nativeGetUserDefaultUILanguage(&num);
			if (num == CultureInfo.UserDefaultCulture.LCID || text == CultureInfo.UserDefaultCulture.Name)
			{
				return CultureInfo.UserDefaultCulture;
			}
			CultureInfo cultureByLCIDOrName = CultureInfo.GetCultureByLCIDOrName(num, text);
			if (cultureByLCIDOrName == null)
			{
				text = CultureInfo.nativeGetSystemDefaultUILanguage(&num);
				cultureByLCIDOrName = CultureInfo.GetCultureByLCIDOrName(num, text);
			}
			if (cultureByLCIDOrName == null)
			{
				return CultureInfo.InvariantCulture;
			}
			cultureByLCIDOrName.m_isReadOnly = true;
			return cultureByLCIDOrName;
		}

		// Token: 0x0600244F RID: 9295 RVA: 0x0005D4C0 File Offset: 0x0005C4C0
		public CultureInfo(string name) : this(name, true)
		{
		}

		// Token: 0x06002450 RID: 9296 RVA: 0x0005D4CC File Offset: 0x0005C4CC
		public CultureInfo(string name, bool useUserOverride)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name", Environment.GetResourceString("ArgumentNull_String"));
			}
			this.m_cultureTableRecord = CultureTableRecord.GetCultureTableRecord(name, useUserOverride);
			this.cultureID = this.m_cultureTableRecord.ActualCultureID;
			this.m_name = this.m_cultureTableRecord.ActualName;
			this.m_isInherited = (base.GetType() != typeof(CultureInfo));
		}

		// Token: 0x06002451 RID: 9297 RVA: 0x0005D541 File Offset: 0x0005C541
		public CultureInfo(int culture) : this(culture, true)
		{
		}

		// Token: 0x06002452 RID: 9298 RVA: 0x0005D54C File Offset: 0x0005C54C
		public CultureInfo(int culture, bool useUserOverride)
		{
			if (culture < 0)
			{
				throw new ArgumentOutOfRangeException("culture", Environment.GetResourceString("ArgumentOutOfRange_NeedPosNum"));
			}
			if (culture <= 1024)
			{
				if (culture != 0 && culture != 1024)
				{
					goto IL_75;
				}
			}
			else if (culture != 2048 && culture != 3072 && culture != 4096)
			{
				goto IL_75;
			}
			throw new ArgumentException(Environment.GetResourceString("Argument_CultureNotSupported", new object[]
			{
				culture
			}), "culture");
			IL_75:
			this.cultureID = culture;
			this.m_cultureTableRecord = CultureTableRecord.GetCultureTableRecord(this.cultureID, useUserOverride);
			this.m_name = this.m_cultureTableRecord.ActualName;
			this.m_isInherited = (base.GetType() != typeof(CultureInfo));
		}

		// Token: 0x06002453 RID: 9299 RVA: 0x0005D614 File Offset: 0x0005C614
		internal static void CheckDomainSafetyObject(object obj, object container)
		{
			if (obj.GetType().Assembly != typeof(CultureInfo).Assembly)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidOperation_SubclassedObject"), new object[]
				{
					obj.GetType(),
					container.GetType()
				}));
			}
		}

		// Token: 0x06002454 RID: 9300 RVA: 0x0005D674 File Offset: 0x0005C674
		[OnDeserialized]
		private void OnDeserialized(StreamingContext ctx)
		{
			if (this.m_name != null && this.cultureID != 1034)
			{
				this.m_cultureTableRecord = CultureTableRecord.GetCultureTableRecord(this.m_name, this.m_useUserOverride);
			}
			else
			{
				this.m_cultureTableRecord = CultureTableRecord.GetCultureTableRecord(this.cultureID, this.m_useUserOverride);
			}
			this.m_isInherited = (base.GetType() != typeof(CultureInfo));
			if (this.m_name == null)
			{
				this.m_name = this.m_cultureTableRecord.ActualName;
			}
			if (base.GetType().Assembly == typeof(CultureInfo).Assembly)
			{
				if (this.textInfo != null)
				{
					CultureInfo.CheckDomainSafetyObject(this.textInfo, this);
				}
				if (this.compareInfo != null)
				{
					CultureInfo.CheckDomainSafetyObject(this.compareInfo, this);
				}
			}
		}

		// Token: 0x06002455 RID: 9301 RVA: 0x0005D73E File Offset: 0x0005C73E
		[OnSerializing]
		private void OnSerializing(StreamingContext ctx)
		{
			this.m_name = this.m_cultureTableRecord.CultureName;
			this.m_useUserOverride = this.m_cultureTableRecord.UseUserOverride;
			this.m_dataItem = this.m_cultureTableRecord.EverettDataItem();
		}

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x06002456 RID: 9302 RVA: 0x0005D773 File Offset: 0x0005C773
		internal bool IsSafeCrossDomain
		{
			get
			{
				return this.m_isSafeCrossDomain;
			}
		}

		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x06002457 RID: 9303 RVA: 0x0005D77B File Offset: 0x0005C77B
		internal int CreatedDomainID
		{
			get
			{
				return this.m_createdDomainID;
			}
		}

		// Token: 0x06002458 RID: 9304 RVA: 0x0005D783 File Offset: 0x0005C783
		internal void StartCrossDomainTracking()
		{
			if (this.m_createdDomainID != 0)
			{
				return;
			}
			if (base.GetType() == typeof(CultureInfo))
			{
				this.m_isSafeCrossDomain = true;
			}
			Thread.MemoryBarrier();
			this.m_createdDomainID = Thread.GetDomainID();
		}

		// Token: 0x06002459 RID: 9305 RVA: 0x0005D7B8 File Offset: 0x0005C7B8
		internal CultureInfo(string cultureName, string textAndCompareCultureName)
		{
			if (cultureName == null)
			{
				throw new ArgumentNullException("cultureName", Environment.GetResourceString("ArgumentNull_String"));
			}
			this.m_cultureTableRecord = CultureTableRecord.GetCultureTableRecord(cultureName, false);
			this.cultureID = this.m_cultureTableRecord.ActualCultureID;
			this.m_name = this.m_cultureTableRecord.ActualName;
			CultureInfo cultureInfo = CultureInfo.GetCultureInfo(textAndCompareCultureName);
			this.compareInfo = cultureInfo.CompareInfo;
			this.textInfo = cultureInfo.TextInfo;
		}

		// Token: 0x0600245A RID: 9306 RVA: 0x0005D834 File Offset: 0x0005C834
		private static CultureInfo GetCultureByLCIDOrName(int preferLCID, string fallbackToString)
		{
			CultureInfo cultureInfo = null;
			if ((preferLCID & 1023) != 0)
			{
				try
				{
					cultureInfo = new CultureInfo(preferLCID);
				}
				catch (ArgumentException)
				{
				}
			}
			if (cultureInfo == null && fallbackToString != null && fallbackToString.Length > 0)
			{
				try
				{
					cultureInfo = new CultureInfo(fallbackToString);
				}
				catch (ArgumentException)
				{
				}
			}
			return cultureInfo;
		}

		// Token: 0x0600245B RID: 9307 RVA: 0x0005D890 File Offset: 0x0005C890
		public static CultureInfo CreateSpecificCulture(string name)
		{
			CultureInfo cultureInfo;
			try
			{
				cultureInfo = new CultureInfo(name);
			}
			catch (ArgumentException ex)
			{
				cultureInfo = null;
				for (int i = 0; i < name.Length; i++)
				{
					if ('-' == name[i])
					{
						try
						{
							cultureInfo = new CultureInfo(name.Substring(0, i));
							break;
						}
						catch (ArgumentException)
						{
							throw ex;
						}
					}
				}
				if (cultureInfo == null)
				{
					throw ex;
				}
			}
			if (!cultureInfo.IsNeutralCulture)
			{
				return cultureInfo;
			}
			int lcid = cultureInfo.LCID;
			if ((lcid & 1023) == 4)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NoSpecificCulture"));
			}
			return new CultureInfo(cultureInfo.m_cultureTableRecord.SSPECIFICCULTURE);
		}

		// Token: 0x0600245C RID: 9308 RVA: 0x0005D938 File Offset: 0x0005C938
		internal static bool VerifyCultureName(CultureInfo culture, bool throwException)
		{
			if (!culture.m_isInherited)
			{
				return true;
			}
			string name = culture.Name;
			int i = 0;
			while (i < name.Length)
			{
				char c = name[i];
				if (!char.IsLetterOrDigit(c) && c != '-' && c != '_')
				{
					if (throwException)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_InvalidResourceCultureName", new object[]
						{
							name
						}));
					}
					return false;
				}
				else
				{
					i++;
				}
			}
			return true;
		}

		// Token: 0x0600245D RID: 9309 RVA: 0x0005D9A3 File Offset: 0x0005C9A3
		internal static int GetSubLangID(int culture)
		{
			return culture >> 10 & 63;
		}

		// Token: 0x0600245E RID: 9310 RVA: 0x0005D9AC File Offset: 0x0005C9AC
		internal static int GetLangID(int culture)
		{
			return culture & 65535;
		}

		// Token: 0x0600245F RID: 9311 RVA: 0x0005D9B5 File Offset: 0x0005C9B5
		internal static int GetSortID(int lcid)
		{
			return lcid >> 16 & 15;
		}

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x06002460 RID: 9312 RVA: 0x0005D9BE File Offset: 0x0005C9BE
		public static CultureInfo CurrentCulture
		{
			get
			{
				return Thread.CurrentThread.CurrentCulture;
			}
		}

		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x06002461 RID: 9313 RVA: 0x0005D9CC File Offset: 0x0005C9CC
		internal static CultureInfo UserDefaultCulture
		{
			get
			{
				CultureInfo cultureInfo = CultureInfo.m_userDefaultCulture;
				if (cultureInfo == null)
				{
					CultureInfo.m_userDefaultCulture = CultureInfo.InvariantCulture;
					cultureInfo = CultureInfo.InitUserDefaultCulture();
					CultureInfo.m_userDefaultCulture = cultureInfo;
				}
				return cultureInfo;
			}
		}

		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x06002462 RID: 9314 RVA: 0x0005D9FC File Offset: 0x0005C9FC
		internal static CultureInfo UserDefaultUICulture
		{
			get
			{
				CultureInfo cultureInfo = CultureInfo.m_userDefaultUICulture;
				if (cultureInfo == null)
				{
					CultureInfo.m_userDefaultUICulture = CultureInfo.InvariantCulture;
					cultureInfo = CultureInfo.InitUserDefaultUICulture();
					CultureInfo.m_userDefaultUICulture = cultureInfo;
				}
				return cultureInfo;
			}
		}

		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x06002463 RID: 9315 RVA: 0x0005DA29 File Offset: 0x0005CA29
		public static CultureInfo CurrentUICulture
		{
			get
			{
				return Thread.CurrentThread.CurrentUICulture;
			}
		}

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x06002464 RID: 9316 RVA: 0x0005DA38 File Offset: 0x0005CA38
		public unsafe static CultureInfo InstalledUICulture
		{
			get
			{
				CultureInfo cultureInfo = CultureInfo.m_InstalledUICultureInfo;
				if (cultureInfo == null)
				{
					int preferLCID;
					string fallbackToString = CultureInfo.nativeGetSystemDefaultUILanguage(&preferLCID);
					cultureInfo = CultureInfo.GetCultureByLCIDOrName(preferLCID, fallbackToString);
					if (cultureInfo == null)
					{
						cultureInfo = new CultureInfo(127, true);
					}
					cultureInfo.m_isReadOnly = true;
					CultureInfo.m_InstalledUICultureInfo = cultureInfo;
				}
				return cultureInfo;
			}
		}

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x06002465 RID: 9317 RVA: 0x0005DA79 File Offset: 0x0005CA79
		public static CultureInfo InvariantCulture
		{
			get
			{
				return CultureInfo.m_InvariantCultureInfo;
			}
		}

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x06002466 RID: 9318 RVA: 0x0005DA80 File Offset: 0x0005CA80
		public virtual CultureInfo Parent
		{
			get
			{
				if (this.m_parent == null)
				{
					try
					{
						int iparent = (int)this.m_cultureTableRecord.IPARENT;
						if (iparent == 127)
						{
							this.m_parent = CultureInfo.InvariantCulture;
						}
						else if (CultureTableRecord.IsCustomCultureId(iparent) || CultureTable.IsOldNeutralChineseCulture(this))
						{
							this.m_parent = new CultureInfo(this.m_cultureTableRecord.SPARENT);
						}
						else
						{
							this.m_parent = new CultureInfo(iparent, this.m_cultureTableRecord.UseUserOverride);
						}
					}
					catch (ArgumentException)
					{
						this.m_parent = CultureInfo.InvariantCulture;
					}
				}
				return this.m_parent;
			}
		}

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x06002467 RID: 9319 RVA: 0x0005DB18 File Offset: 0x0005CB18
		public virtual int LCID
		{
			get
			{
				return this.cultureID;
			}
		}

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x06002468 RID: 9320 RVA: 0x0005DB20 File Offset: 0x0005CB20
		[ComVisible(false)]
		public virtual int KeyboardLayoutId
		{
			get
			{
				return (int)this.m_cultureTableRecord.IINPUTLANGUAGEHANDLE;
			}
		}

		// Token: 0x06002469 RID: 9321 RVA: 0x0005DB3A File Offset: 0x0005CB3A
		public static CultureInfo[] GetCultures(CultureTypes types)
		{
			return CultureTable.Default.GetCultures(types);
		}

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x0600246A RID: 9322 RVA: 0x0005DB47 File Offset: 0x0005CB47
		public virtual string Name
		{
			get
			{
				if (this.m_nonSortName == null)
				{
					this.m_nonSortName = this.m_cultureTableRecord.CultureName;
				}
				return this.m_nonSortName;
			}
		}

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x0600246B RID: 9323 RVA: 0x0005DB68 File Offset: 0x0005CB68
		internal string SortName
		{
			get
			{
				if (this.m_sortName == null)
				{
					if (CultureTableRecord.IsCustomCultureId(this.cultureID))
					{
						CultureInfo cultureInfo = CultureInfo.GetCultureInfo(this.CompareInfoId);
						if (CultureTableRecord.IsCustomCultureId(cultureInfo.cultureID))
						{
							this.m_sortName = this.m_cultureTableRecord.SNAME;
						}
						else
						{
							this.m_sortName = cultureInfo.SortName;
						}
					}
					else
					{
						this.m_sortName = this.m_name;
					}
				}
				return this.m_sortName;
			}
		}

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x0600246C RID: 9324 RVA: 0x0005DBD6 File Offset: 0x0005CBD6
		[ComVisible(false)]
		public string IetfLanguageTag
		{
			get
			{
				if (CultureTable.IsOldNeutralChineseCulture(this))
				{
					if (this.LCID == 31748)
					{
						return "zh-Hant";
					}
					if (this.LCID == 4)
					{
						return "zh-Hans";
					}
				}
				return this.Name;
			}
		}

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x0600246D RID: 9325 RVA: 0x0005DC08 File Offset: 0x0005CC08
		public virtual string DisplayName
		{
			get
			{
				if (this.m_cultureTableRecord.IsCustomCulture)
				{
					if (!this.m_cultureTableRecord.IsReplacementCulture)
					{
						return this.m_cultureTableRecord.SNATIVEDISPLAYNAME;
					}
					if (this.m_cultureTableRecord.IsSynthetic)
					{
						return this.m_cultureTableRecord.CultureNativeDisplayName;
					}
					return Environment.GetResourceString("Globalization.ci_" + this.m_name);
				}
				else
				{
					if (this.m_cultureTableRecord.IsSynthetic)
					{
						return this.m_cultureTableRecord.CultureNativeDisplayName;
					}
					return Environment.GetResourceString("Globalization.ci_" + this.m_name);
				}
			}
		}

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x0600246E RID: 9326 RVA: 0x0005DC98 File Offset: 0x0005CC98
		public virtual string NativeName
		{
			get
			{
				return this.m_cultureTableRecord.SNATIVEDISPLAYNAME;
			}
		}

		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x0600246F RID: 9327 RVA: 0x0005DCA5 File Offset: 0x0005CCA5
		public virtual string EnglishName
		{
			get
			{
				return this.m_cultureTableRecord.SENGDISPLAYNAME;
			}
		}

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x06002470 RID: 9328 RVA: 0x0005DCB2 File Offset: 0x0005CCB2
		public virtual string TwoLetterISOLanguageName
		{
			get
			{
				return this.m_cultureTableRecord.SISO639LANGNAME;
			}
		}

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x06002471 RID: 9329 RVA: 0x0005DCBF File Offset: 0x0005CCBF
		public virtual string ThreeLetterISOLanguageName
		{
			get
			{
				return this.m_cultureTableRecord.SISO639LANGNAME2;
			}
		}

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x06002472 RID: 9330 RVA: 0x0005DCCC File Offset: 0x0005CCCC
		public virtual string ThreeLetterWindowsLanguageName
		{
			get
			{
				return this.m_cultureTableRecord.SABBREVLANGNAME;
			}
		}

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x06002473 RID: 9331 RVA: 0x0005DCDC File Offset: 0x0005CCDC
		public virtual CompareInfo CompareInfo
		{
			get
			{
				if (this.compareInfo == null)
				{
					int num;
					if (this.IsNeutralCulture && !CultureTableRecord.IsCustomCultureId(this.cultureID))
					{
						num = this.cultureID;
					}
					else
					{
						num = this.CompareInfoId;
					}
					if (this.Name == "zh-CHS" || this.Name == "zh-CHT")
					{
						num |= int.MinValue;
					}
					CompareInfo compareInfoWithPrefixedLcid = CompareInfo.GetCompareInfoWithPrefixedLcid(num, int.MinValue);
					compareInfoWithPrefixedLcid.SetName(this.SortName);
					this.compareInfo = compareInfoWithPrefixedLcid;
				}
				return this.compareInfo;
			}
		}

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x06002474 RID: 9332 RVA: 0x0005DD68 File Offset: 0x0005CD68
		internal int CompareInfoId
		{
			get
			{
				int result;
				if (this.cultureID == 1034)
				{
					result = 1034;
				}
				else if (CultureInfo.GetSortID(this.cultureID) != 0)
				{
					result = this.cultureID;
				}
				else
				{
					result = (int)this.m_cultureTableRecord.ICOMPAREINFO;
				}
				return result;
			}
		}

		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x06002475 RID: 9333 RVA: 0x0005DDB0 File Offset: 0x0005CDB0
		public virtual TextInfo TextInfo
		{
			get
			{
				if (this.textInfo == null)
				{
					TextInfo textInfo = new TextInfo(this.m_cultureTableRecord);
					textInfo.SetReadOnlyState(this.m_isReadOnly);
					this.textInfo = textInfo;
				}
				return this.textInfo;
			}
		}

		// Token: 0x06002476 RID: 9334 RVA: 0x0005DDEC File Offset: 0x0005CDEC
		public override bool Equals(object value)
		{
			if (object.ReferenceEquals(this, value))
			{
				return true;
			}
			CultureInfo cultureInfo = value as CultureInfo;
			return cultureInfo != null && this.Name.Equals(cultureInfo.Name) && this.CompareInfo.Equals(cultureInfo.CompareInfo);
		}

		// Token: 0x06002477 RID: 9335 RVA: 0x0005DE36 File Offset: 0x0005CE36
		public override int GetHashCode()
		{
			return this.Name.GetHashCode() + this.CompareInfo.GetHashCode();
		}

		// Token: 0x06002478 RID: 9336 RVA: 0x0005DE4F File Offset: 0x0005CE4F
		public override string ToString()
		{
			return this.m_name;
		}

		// Token: 0x06002479 RID: 9337 RVA: 0x0005DE57 File Offset: 0x0005CE57
		public virtual object GetFormat(Type formatType)
		{
			if (formatType == typeof(NumberFormatInfo))
			{
				return this.NumberFormat;
			}
			if (formatType == typeof(DateTimeFormatInfo))
			{
				return this.DateTimeFormat;
			}
			return null;
		}

		// Token: 0x0600247A RID: 9338 RVA: 0x0005DE84 File Offset: 0x0005CE84
		internal static void CheckNeutral(CultureInfo culture)
		{
			if (culture.IsNeutralCulture)
			{
				throw new NotSupportedException(Environment.GetResourceString("Argument_CultureInvalidFormat", new object[]
				{
					culture.m_name
				}));
			}
		}

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x0600247B RID: 9339 RVA: 0x0005DEBA File Offset: 0x0005CEBA
		public virtual bool IsNeutralCulture
		{
			get
			{
				return this.m_cultureTableRecord.IsNeutralCulture;
			}
		}

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x0600247C RID: 9340 RVA: 0x0005DEC8 File Offset: 0x0005CEC8
		[ComVisible(false)]
		public CultureTypes CultureTypes
		{
			get
			{
				CultureTypes cultureTypes = (CultureTypes)0;
				if (this.m_cultureTableRecord.IsNeutralCulture)
				{
					cultureTypes |= CultureTypes.NeutralCultures;
				}
				else
				{
					cultureTypes |= CultureTypes.SpecificCultures;
				}
				if (this.m_cultureTableRecord.IsSynthetic)
				{
					cultureTypes |= (CultureTypes.InstalledWin32Cultures | CultureTypes.WindowsOnlyCultures);
				}
				else
				{
					if (CultureTable.IsInstalledLCID(this.cultureID))
					{
						cultureTypes |= CultureTypes.InstalledWin32Cultures;
					}
					if (!this.m_cultureTableRecord.IsCustomCulture || this.m_cultureTableRecord.IsReplacementCulture)
					{
						cultureTypes |= CultureTypes.FrameworkCultures;
					}
				}
				if (this.m_cultureTableRecord.IsCustomCulture)
				{
					cultureTypes |= CultureTypes.UserCustomCulture;
					if (this.m_cultureTableRecord.IsReplacementCulture)
					{
						cultureTypes |= CultureTypes.ReplacementCultures;
					}
				}
				return cultureTypes;
			}
		}

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x0600247D RID: 9341 RVA: 0x0005DF58 File Offset: 0x0005CF58
		// (set) Token: 0x0600247E RID: 9342 RVA: 0x0005DF98 File Offset: 0x0005CF98
		public virtual NumberFormatInfo NumberFormat
		{
			get
			{
				CultureInfo.CheckNeutral(this);
				if (this.numInfo == null)
				{
					this.numInfo = new NumberFormatInfo(this.m_cultureTableRecord)
					{
						isReadOnly = this.m_isReadOnly
					};
				}
				return this.numInfo;
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("value", Environment.GetResourceString("ArgumentNull_Obj"));
				}
				this.numInfo = value;
			}
		}

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x0600247F RID: 9343 RVA: 0x0005DFC0 File Offset: 0x0005CFC0
		// (set) Token: 0x06002480 RID: 9344 RVA: 0x0005E016 File Offset: 0x0005D016
		public virtual DateTimeFormatInfo DateTimeFormat
		{
			get
			{
				if (this.dateTimeInfo == null)
				{
					CultureInfo.CheckNeutral(this);
					DateTimeFormatInfo dateTimeFormatInfo = new DateTimeFormatInfo(this.m_cultureTableRecord, CultureInfo.GetLangID(this.cultureID), this.Calendar);
					dateTimeFormatInfo.m_isReadOnly = this.m_isReadOnly;
					Thread.MemoryBarrier();
					this.dateTimeInfo = dateTimeFormatInfo;
				}
				return this.dateTimeInfo;
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("value", Environment.GetResourceString("ArgumentNull_Obj"));
				}
				this.dateTimeInfo = value;
			}
		}

		// Token: 0x06002481 RID: 9345 RVA: 0x0005E03D File Offset: 0x0005D03D
		public void ClearCachedData()
		{
			CultureInfo.m_userDefaultUICulture = null;
			CultureInfo.m_userDefaultCulture = null;
			RegionInfo.m_currentRegionInfo = null;
			TimeZone.ResetTimeZone();
			CultureInfo.m_LcidCachedCultures = null;
			CultureInfo.m_NameCachedCultures = null;
			CultureTableRecord.ResetCustomCulturesCache();
			CompareInfo.ClearDefaultAssemblyCache();
		}

		// Token: 0x06002482 RID: 9346 RVA: 0x0005E06C File Offset: 0x0005D06C
		internal static Calendar GetCalendarInstance(int calType)
		{
			if (calType == 1)
			{
				return new GregorianCalendar();
			}
			return CultureInfo.GetCalendarInstanceRare(calType);
		}

		// Token: 0x06002483 RID: 9347 RVA: 0x0005E080 File Offset: 0x0005D080
		internal static Calendar GetCalendarInstanceRare(int calType)
		{
			switch (calType)
			{
			case 2:
			case 9:
			case 10:
			case 11:
			case 12:
				return new GregorianCalendar((GregorianCalendarTypes)calType);
			case 3:
				return new JapaneseCalendar();
			case 4:
				return new TaiwanCalendar();
			case 5:
				return new KoreanCalendar();
			case 6:
				return new HijriCalendar();
			case 7:
				return new ThaiBuddhistCalendar();
			case 8:
				return new HebrewCalendar();
			case 14:
				return new JapaneseLunisolarCalendar();
			case 15:
				return new ChineseLunisolarCalendar();
			case 20:
				return new KoreanLunisolarCalendar();
			case 21:
				return new TaiwanLunisolarCalendar();
			case 22:
				return new PersianCalendar();
			case 23:
				return new UmAlQuraCalendar();
			}
			return new GregorianCalendar();
		}

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x06002484 RID: 9348 RVA: 0x0005E148 File Offset: 0x0005D148
		public virtual Calendar Calendar
		{
			get
			{
				if (this.calendar == null)
				{
					int icalendartype = (int)this.m_cultureTableRecord.ICALENDARTYPE;
					Calendar calendarInstance = CultureInfo.GetCalendarInstance(icalendartype);
					Thread.MemoryBarrier();
					calendarInstance.SetReadOnlyState(this.m_isReadOnly);
					this.calendar = calendarInstance;
				}
				return this.calendar;
			}
		}

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x06002485 RID: 9349 RVA: 0x0005E190 File Offset: 0x0005D190
		public virtual Calendar[] OptionalCalendars
		{
			get
			{
				int[] ioptionalcalendars = this.m_cultureTableRecord.IOPTIONALCALENDARS;
				Calendar[] array = new Calendar[ioptionalcalendars.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = CultureInfo.GetCalendarInstance(ioptionalcalendars[i]);
				}
				return array;
			}
		}

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x06002486 RID: 9350 RVA: 0x0005E1CC File Offset: 0x0005D1CC
		public bool UseUserOverride
		{
			get
			{
				return this.m_cultureTableRecord.UseUserOverride;
			}
		}

		// Token: 0x06002487 RID: 9351 RVA: 0x0005E1DC File Offset: 0x0005D1DC
		[ComVisible(false)]
		public CultureInfo GetConsoleFallbackUICulture()
		{
			CultureInfo cultureInfo = this.m_consoleFallbackCulture;
			if (cultureInfo == null)
			{
				cultureInfo = CultureInfo.GetCultureInfo(this.m_cultureTableRecord.SCONSOLEFALLBACKNAME);
				cultureInfo.m_isReadOnly = true;
				this.m_consoleFallbackCulture = cultureInfo;
			}
			return cultureInfo;
		}

		// Token: 0x06002488 RID: 9352 RVA: 0x0005E214 File Offset: 0x0005D214
		public virtual object Clone()
		{
			CultureInfo cultureInfo = (CultureInfo)base.MemberwiseClone();
			cultureInfo.m_isReadOnly = false;
			if (!cultureInfo.IsNeutralCulture)
			{
				if (!this.m_isInherited)
				{
					if (this.dateTimeInfo != null)
					{
						cultureInfo.dateTimeInfo = (DateTimeFormatInfo)this.dateTimeInfo.Clone();
					}
					if (this.numInfo != null)
					{
						cultureInfo.numInfo = (NumberFormatInfo)this.numInfo.Clone();
					}
				}
				else
				{
					cultureInfo.DateTimeFormat = (DateTimeFormatInfo)this.DateTimeFormat.Clone();
					cultureInfo.NumberFormat = (NumberFormatInfo)this.NumberFormat.Clone();
				}
			}
			if (this.textInfo != null)
			{
				cultureInfo.textInfo = (TextInfo)this.textInfo.Clone();
			}
			if (this.calendar != null)
			{
				cultureInfo.calendar = (Calendar)this.calendar.Clone();
			}
			return cultureInfo;
		}

		// Token: 0x06002489 RID: 9353 RVA: 0x0005E2EC File Offset: 0x0005D2EC
		public static CultureInfo ReadOnly(CultureInfo ci)
		{
			if (ci == null)
			{
				throw new ArgumentNullException("ci");
			}
			if (ci.IsReadOnly)
			{
				return ci;
			}
			CultureInfo cultureInfo = (CultureInfo)ci.MemberwiseClone();
			if (!ci.IsNeutralCulture)
			{
				if (!ci.m_isInherited)
				{
					if (ci.dateTimeInfo != null)
					{
						cultureInfo.dateTimeInfo = DateTimeFormatInfo.ReadOnly(ci.dateTimeInfo);
					}
					if (ci.numInfo != null)
					{
						cultureInfo.numInfo = NumberFormatInfo.ReadOnly(ci.numInfo);
					}
				}
				else
				{
					cultureInfo.DateTimeFormat = DateTimeFormatInfo.ReadOnly(ci.DateTimeFormat);
					cultureInfo.NumberFormat = NumberFormatInfo.ReadOnly(ci.NumberFormat);
				}
			}
			if (ci.textInfo != null)
			{
				cultureInfo.textInfo = TextInfo.ReadOnly(ci.textInfo);
			}
			if (ci.calendar != null)
			{
				cultureInfo.calendar = Calendar.ReadOnly(ci.calendar);
			}
			cultureInfo.m_isReadOnly = true;
			return cultureInfo;
		}

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x0600248A RID: 9354 RVA: 0x0005E3BD File Offset: 0x0005D3BD
		public bool IsReadOnly
		{
			get
			{
				return this.m_isReadOnly;
			}
		}

		// Token: 0x0600248B RID: 9355 RVA: 0x0005E3C5 File Offset: 0x0005D3C5
		private void VerifyWritable()
		{
			if (this.m_isReadOnly)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ReadOnly"));
			}
		}

		// Token: 0x0600248C RID: 9356 RVA: 0x0005E3E0 File Offset: 0x0005D3E0
		internal static CultureInfo GetCultureInfoHelper(int lcid, string name, string altName)
		{
			Hashtable hashtable = CultureInfo.m_NameCachedCultures;
			if (name != null)
			{
				name = CultureTableRecord.AnsiToLower(name);
			}
			if (altName != null)
			{
				altName = CultureTableRecord.AnsiToLower(altName);
			}
			CultureInfo cultureInfo;
			if (hashtable == null)
			{
				hashtable = Hashtable.Synchronized(new Hashtable());
			}
			else if (lcid == -1)
			{
				cultureInfo = (CultureInfo)hashtable[name + '�' + altName];
				if (cultureInfo != null)
				{
					return cultureInfo;
				}
			}
			else if (lcid == 0)
			{
				cultureInfo = (CultureInfo)hashtable[name];
				if (cultureInfo != null)
				{
					return cultureInfo;
				}
			}
			Hashtable hashtable2 = CultureInfo.m_LcidCachedCultures;
			if (hashtable2 == null)
			{
				hashtable2 = Hashtable.Synchronized(new Hashtable());
			}
			else if (lcid > 0)
			{
				cultureInfo = (CultureInfo)hashtable2[lcid];
				if (cultureInfo != null)
				{
					return cultureInfo;
				}
			}
			try
			{
				switch (lcid)
				{
				case -1:
					cultureInfo = new CultureInfo(name, altName);
					break;
				case 0:
					cultureInfo = new CultureInfo(name, false);
					break;
				default:
					if (CultureInfo.m_userDefaultCulture != null && CultureInfo.m_userDefaultCulture.LCID == lcid)
					{
						cultureInfo = (CultureInfo)CultureInfo.m_userDefaultCulture.Clone();
						cultureInfo.m_cultureTableRecord = cultureInfo.m_cultureTableRecord.CloneWithUserOverride(false);
					}
					else
					{
						cultureInfo = new CultureInfo(lcid, false);
					}
					break;
				}
			}
			catch (ArgumentException)
			{
				return null;
			}
			cultureInfo.m_isReadOnly = true;
			if (lcid == -1)
			{
				hashtable[name + '�' + altName] = cultureInfo;
				cultureInfo.TextInfo.SetReadOnlyState(true);
			}
			else
			{
				if (!CultureTable.IsNewNeutralChineseCulture(cultureInfo))
				{
					hashtable2[cultureInfo.LCID] = cultureInfo;
				}
				string key = CultureTableRecord.AnsiToLower(cultureInfo.m_name);
				hashtable[key] = cultureInfo;
			}
			if (-1 != lcid)
			{
				CultureInfo.m_LcidCachedCultures = hashtable2;
			}
			CultureInfo.m_NameCachedCultures = hashtable;
			return cultureInfo;
		}

		// Token: 0x0600248D RID: 9357 RVA: 0x0005E57C File Offset: 0x0005D57C
		public static CultureInfo GetCultureInfo(int culture)
		{
			if (culture <= 0)
			{
				throw new ArgumentOutOfRangeException("culture", Environment.GetResourceString("ArgumentOutOfRange_NeedPosNum"));
			}
			CultureInfo cultureInfoHelper = CultureInfo.GetCultureInfoHelper(culture, null, null);
			if (cultureInfoHelper == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_CultureNotSupported", new object[]
				{
					culture
				}), "culture");
			}
			return cultureInfoHelper;
		}

		// Token: 0x0600248E RID: 9358 RVA: 0x0005E5D8 File Offset: 0x0005D5D8
		public static CultureInfo GetCultureInfo(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			CultureInfo cultureInfoHelper = CultureInfo.GetCultureInfoHelper(0, name, null);
			if (cultureInfoHelper == null)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidCultureName"), new object[]
				{
					name
				}), "name");
			}
			return cultureInfoHelper;
		}

		// Token: 0x0600248F RID: 9359 RVA: 0x0005E62C File Offset: 0x0005D62C
		public static CultureInfo GetCultureInfo(string name, string altName)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (altName == null)
			{
				throw new ArgumentNullException("altName");
			}
			CultureInfo cultureInfoHelper = CultureInfo.GetCultureInfoHelper(-1, name, altName);
			if (cultureInfoHelper == null)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_OneOfCulturesNotSupported"), new object[]
				{
					name,
					altName
				}), "name");
			}
			return cultureInfoHelper;
		}

		// Token: 0x06002490 RID: 9360 RVA: 0x0005E694 File Offset: 0x0005D694
		public static CultureInfo GetCultureInfoByIetfLanguageTag(string name)
		{
			if ("zh-CHT".Equals(name, StringComparison.OrdinalIgnoreCase) || "zh-CHS".Equals(name, StringComparison.OrdinalIgnoreCase))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_CultureIetfNotSupported"), new object[]
				{
					name
				}), "name");
			}
			CultureInfo cultureInfo = CultureInfo.GetCultureInfo(name);
			if (CultureInfo.GetSortID(cultureInfo.cultureID) != 0 || cultureInfo.cultureID == 1034)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_CultureIetfNotSupported"), new object[]
				{
					name
				}), "name");
			}
			return cultureInfo;
		}

		// Token: 0x04000F72 RID: 3954
		internal const int zh_CHT_CultureID = 31748;

		// Token: 0x04000F73 RID: 3955
		internal const int zh_CHS_CultureID = 4;

		// Token: 0x04000F74 RID: 3956
		internal const int sr_CultureID = 31770;

		// Token: 0x04000F75 RID: 3957
		internal const int sr_SP_Latn_CultureID = 2074;

		// Token: 0x04000F76 RID: 3958
		internal const int LOCALE_INVARIANT = 127;

		// Token: 0x04000F77 RID: 3959
		private const int LOCALE_NEUTRAL = 0;

		// Token: 0x04000F78 RID: 3960
		internal const int LOCALE_USER_DEFAULT = 1024;

		// Token: 0x04000F79 RID: 3961
		internal const int LOCALE_SYSTEM_DEFAULT = 2048;

		// Token: 0x04000F7A RID: 3962
		internal const int LOCALE_CUSTOM_DEFAULT = 3072;

		// Token: 0x04000F7B RID: 3963
		internal const int LOCALE_CUSTOM_UNSPECIFIED = 4096;

		// Token: 0x04000F7C RID: 3964
		internal const int LOCALE_TRADITIONAL_SPANISH = 1034;

		// Token: 0x04000F7D RID: 3965
		internal const int LCID_INSTALLED = 1;

		// Token: 0x04000F7E RID: 3966
		internal const int LCID_SUPPORTED = 2;

		// Token: 0x04000F7F RID: 3967
		internal int cultureID;

		// Token: 0x04000F80 RID: 3968
		internal bool m_isReadOnly;

		// Token: 0x04000F81 RID: 3969
		internal CompareInfo compareInfo;

		// Token: 0x04000F82 RID: 3970
		internal TextInfo textInfo;

		// Token: 0x04000F83 RID: 3971
		internal NumberFormatInfo numInfo;

		// Token: 0x04000F84 RID: 3972
		internal DateTimeFormatInfo dateTimeInfo;

		// Token: 0x04000F85 RID: 3973
		internal Calendar calendar;

		// Token: 0x04000F86 RID: 3974
		[NonSerialized]
		internal CultureTableRecord m_cultureTableRecord;

		// Token: 0x04000F87 RID: 3975
		[NonSerialized]
		internal bool m_isInherited;

		// Token: 0x04000F88 RID: 3976
		[NonSerialized]
		private bool m_isSafeCrossDomain;

		// Token: 0x04000F89 RID: 3977
		[NonSerialized]
		private int m_createdDomainID;

		// Token: 0x04000F8A RID: 3978
		[NonSerialized]
		private CultureInfo m_consoleFallbackCulture;

		// Token: 0x04000F8B RID: 3979
		internal string m_name;

		// Token: 0x04000F8C RID: 3980
		[NonSerialized]
		private string m_nonSortName;

		// Token: 0x04000F8D RID: 3981
		[NonSerialized]
		private string m_sortName;

		// Token: 0x04000F8E RID: 3982
		private static CultureInfo m_userDefaultCulture;

		// Token: 0x04000F8F RID: 3983
		private static CultureInfo m_InvariantCultureInfo;

		// Token: 0x04000F90 RID: 3984
		private static CultureInfo m_userDefaultUICulture;

		// Token: 0x04000F91 RID: 3985
		private static CultureInfo m_InstalledUICultureInfo;

		// Token: 0x04000F92 RID: 3986
		private static Hashtable m_LcidCachedCultures;

		// Token: 0x04000F93 RID: 3987
		private static Hashtable m_NameCachedCultures;

		// Token: 0x04000F94 RID: 3988
		[NonSerialized]
		private CultureInfo m_parent;

		// Token: 0x04000F95 RID: 3989
		private int m_dataItem;

		// Token: 0x04000F96 RID: 3990
		private bool m_useUserOverride;
	}
}
