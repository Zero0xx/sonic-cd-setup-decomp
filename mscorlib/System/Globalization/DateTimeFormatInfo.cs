using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using Microsoft.Win32;

namespace System.Globalization
{
	// Token: 0x020003A9 RID: 937
	[ComVisible(true)]
	[Serializable]
	public sealed class DateTimeFormatInfo : ICloneable, IFormatProvider
	{
		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x0600251D RID: 9501 RVA: 0x00065596 File Offset: 0x00064596
		internal int CultureId
		{
			get
			{
				return this.m_cultureTableRecord.CultureID;
			}
		}

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x0600251E RID: 9502 RVA: 0x000655A4 File Offset: 0x000645A4
		private static object InternalSyncObject
		{
			get
			{
				if (DateTimeFormatInfo.s_InternalSyncObject == null)
				{
					object value = new object();
					Interlocked.CompareExchange(ref DateTimeFormatInfo.s_InternalSyncObject, value, null);
				}
				return DateTimeFormatInfo.s_InternalSyncObject;
			}
		}

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x0600251F RID: 9503 RVA: 0x000655D0 File Offset: 0x000645D0
		internal string CultureName
		{
			get
			{
				if (this.m_name == null)
				{
					this.m_name = this.m_cultureTableRecord.SNAME;
				}
				return this.m_name;
			}
		}

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x06002520 RID: 9504 RVA: 0x000655F1 File Offset: 0x000645F1
		internal string LanguageName
		{
			get
			{
				if (this.m_langName == null)
				{
					this.m_langName = this.m_cultureTableRecord.SISO639LANGNAME;
				}
				return this.m_langName;
			}
		}

		// Token: 0x06002521 RID: 9505 RVA: 0x00065614 File Offset: 0x00064614
		private string[] GetAbbreviatedDayOfWeekNames()
		{
			if (this.abbreviatedDayNames == null && this.abbreviatedDayNames == null)
			{
				string[] array = null;
				if (!this.m_isDefaultCalendar)
				{
					array = CalendarTable.Default.SABBREVDAYNAMES(this.Calendar.ID);
				}
				if (array == null || array.Length == 0 || array[0].Length == 0)
				{
					array = this.m_cultureTableRecord.SABBREVDAYNAMES;
				}
				Thread.MemoryBarrier();
				this.abbreviatedDayNames = array;
			}
			return this.abbreviatedDayNames;
		}

		// Token: 0x06002522 RID: 9506 RVA: 0x00065684 File Offset: 0x00064684
		private string[] internalGetSuperShortDayNames()
		{
			if (this.m_superShortDayNames == null && this.m_superShortDayNames == null)
			{
				string[] array = null;
				if (!this.m_isDefaultCalendar)
				{
					array = CalendarTable.Default.SSUPERSHORTDAYNAMES(this.Calendar.ID);
				}
				if (array == null || array.Length == 0 || array[0].Length == 0)
				{
					array = this.m_cultureTableRecord.SSUPERSHORTDAYNAMES;
				}
				Thread.MemoryBarrier();
				this.m_superShortDayNames = array;
			}
			return this.m_superShortDayNames;
		}

		// Token: 0x06002523 RID: 9507 RVA: 0x000656F4 File Offset: 0x000646F4
		private string[] GetDayOfWeekNames()
		{
			if (this.dayNames == null && this.dayNames == null)
			{
				string[] array = null;
				if (!this.m_isDefaultCalendar)
				{
					array = CalendarTable.Default.SDAYNAMES(this.Calendar.ID);
				}
				if (array == null || array.Length == 0 || array[0].Length == 0)
				{
					array = this.m_cultureTableRecord.SDAYNAMES;
				}
				Thread.MemoryBarrier();
				this.dayNames = array;
			}
			return this.dayNames;
		}

		// Token: 0x06002524 RID: 9508 RVA: 0x00065764 File Offset: 0x00064764
		private string[] GetAbbreviatedMonthNames()
		{
			if (this.abbreviatedMonthNames == null && this.abbreviatedMonthNames == null)
			{
				string[] array = null;
				if (!this.m_isDefaultCalendar)
				{
					array = CalendarTable.Default.SABBREVMONTHNAMES(this.Calendar.ID);
				}
				if (array == null || array.Length == 0 || array[0].Length == 0)
				{
					array = this.m_cultureTableRecord.SABBREVMONTHNAMES;
				}
				Thread.MemoryBarrier();
				this.abbreviatedMonthNames = array;
			}
			return this.abbreviatedMonthNames;
		}

		// Token: 0x06002525 RID: 9509 RVA: 0x000657D4 File Offset: 0x000647D4
		private string[] GetMonthNames()
		{
			if (this.monthNames == null)
			{
				string[] array = null;
				if (!this.m_isDefaultCalendar)
				{
					array = CalendarTable.Default.SMONTHNAMES(this.Calendar.ID);
				}
				if (array == null || array.Length == 0 || array[0].Length == 0)
				{
					array = this.m_cultureTableRecord.SMONTHNAMES;
				}
				Thread.MemoryBarrier();
				this.monthNames = array;
			}
			return this.monthNames;
		}

		// Token: 0x06002526 RID: 9510 RVA: 0x0006583C File Offset: 0x0006483C
		public DateTimeFormatInfo()
		{
			this.m_cultureTableRecord = CultureInfo.InvariantCulture.m_cultureTableRecord;
			this.m_isDefaultCalendar = true;
			this.calendar = GregorianCalendar.GetDefaultInstance();
			this.InitializeOverridableProperties();
		}

		// Token: 0x06002527 RID: 9511 RVA: 0x0006588C File Offset: 0x0006488C
		internal DateTimeFormatInfo(CultureTableRecord cultureTable, int cultureID, Calendar cal)
		{
			this.m_cultureTableRecord = cultureTable;
			this.Calendar = cal;
		}

		// Token: 0x06002528 RID: 9512 RVA: 0x000658B8 File Offset: 0x000648B8
		[OnDeserialized]
		private void OnDeserialized(StreamingContext ctx)
		{
			if (CultureTableRecord.IsCustomCultureId(this.CultureID))
			{
				this.m_cultureTableRecord = CultureTableRecord.GetCultureTableRecord(this.m_name, this.m_useUserOverride);
			}
			else
			{
				this.m_cultureTableRecord = CultureTableRecord.GetCultureTableRecord(this.CultureID, this.m_useUserOverride);
			}
			if (this.calendar == null)
			{
				this.calendar = (Calendar)GregorianCalendar.GetDefaultInstance().Clone();
				this.calendar.SetReadOnlyState(this.m_isReadOnly);
			}
			else
			{
				CultureInfo.CheckDomainSafetyObject(this.calendar, this);
			}
			this.InitializeOverridableProperties();
		}

		// Token: 0x06002529 RID: 9513 RVA: 0x00065944 File Offset: 0x00064944
		[OnSerializing]
		private void OnSerializing(StreamingContext ctx)
		{
			this.CultureID = this.m_cultureTableRecord.CultureID;
			this.m_useUserOverride = this.m_cultureTableRecord.UseUserOverride;
			this.nDataItem = this.m_cultureTableRecord.EverettDataItem();
			if (CultureTableRecord.IsCustomCultureId(this.CultureID))
			{
				this.m_name = this.CultureName;
			}
		}

		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x0600252A RID: 9514 RVA: 0x000659A0 File Offset: 0x000649A0
		public static DateTimeFormatInfo InvariantInfo
		{
			get
			{
				if (DateTimeFormatInfo.invariantInfo == null)
				{
					DateTimeFormatInfo dateTimeFormatInfo = new DateTimeFormatInfo();
					dateTimeFormatInfo.Calendar.SetReadOnlyState(true);
					dateTimeFormatInfo.m_isReadOnly = true;
					DateTimeFormatInfo.invariantInfo = dateTimeFormatInfo;
				}
				return DateTimeFormatInfo.invariantInfo;
			}
		}

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x0600252B RID: 9515 RVA: 0x000659D8 File Offset: 0x000649D8
		public static DateTimeFormatInfo CurrentInfo
		{
			get
			{
				CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
				if (!currentCulture.m_isInherited)
				{
					DateTimeFormatInfo dateTimeInfo = currentCulture.dateTimeInfo;
					if (dateTimeInfo != null)
					{
						return dateTimeInfo;
					}
				}
				return (DateTimeFormatInfo)currentCulture.GetFormat(typeof(DateTimeFormatInfo));
			}
		}

		// Token: 0x0600252C RID: 9516 RVA: 0x00065A1C File Offset: 0x00064A1C
		public static DateTimeFormatInfo GetInstance(IFormatProvider provider)
		{
			CultureInfo cultureInfo = provider as CultureInfo;
			if (cultureInfo != null && !cultureInfo.m_isInherited)
			{
				DateTimeFormatInfo dateTimeFormatInfo = cultureInfo.dateTimeInfo;
				if (dateTimeFormatInfo != null)
				{
					return dateTimeFormatInfo;
				}
				return cultureInfo.DateTimeFormat;
			}
			else
			{
				DateTimeFormatInfo dateTimeFormatInfo = provider as DateTimeFormatInfo;
				if (dateTimeFormatInfo != null)
				{
					return dateTimeFormatInfo;
				}
				if (provider != null)
				{
					dateTimeFormatInfo = (provider.GetFormat(typeof(DateTimeFormatInfo)) as DateTimeFormatInfo);
					if (dateTimeFormatInfo != null)
					{
						return dateTimeFormatInfo;
					}
				}
				return DateTimeFormatInfo.CurrentInfo;
			}
		}

		// Token: 0x0600252D RID: 9517 RVA: 0x00065A7D File Offset: 0x00064A7D
		public object GetFormat(Type formatType)
		{
			if (formatType != typeof(DateTimeFormatInfo))
			{
				return null;
			}
			return this;
		}

		// Token: 0x0600252E RID: 9518 RVA: 0x00065A90 File Offset: 0x00064A90
		public object Clone()
		{
			DateTimeFormatInfo dateTimeFormatInfo = (DateTimeFormatInfo)base.MemberwiseClone();
			dateTimeFormatInfo.calendar = (Calendar)this.Calendar.Clone();
			dateTimeFormatInfo.m_isReadOnly = false;
			return dateTimeFormatInfo;
		}

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x0600252F RID: 9519 RVA: 0x00065AC7 File Offset: 0x00064AC7
		// (set) Token: 0x06002530 RID: 9520 RVA: 0x00065ACF File Offset: 0x00064ACF
		public string AMDesignator
		{
			get
			{
				return this.amDesignator;
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("value", Environment.GetResourceString("ArgumentNull_String"));
				}
				this.ClearTokenHashTable(true);
				this.amDesignator = value;
			}
		}

		// Token: 0x06002531 RID: 9521 RVA: 0x00065B00 File Offset: 0x00064B00
		private void InitializeOverridableProperties()
		{
			if (this.amDesignator == null)
			{
				this.amDesignator = this.m_cultureTableRecord.S1159;
			}
			if (this.pmDesignator == null)
			{
				this.pmDesignator = this.m_cultureTableRecord.S2359;
			}
			if (this.longTimePattern == null)
			{
				this.longTimePattern = this.m_cultureTableRecord.STIMEFORMAT;
			}
			if (this.firstDayOfWeek == -1)
			{
				this.firstDayOfWeek = (int)this.m_cultureTableRecord.IFIRSTDAYOFWEEK;
			}
			if (this.calendarWeekRule == -1)
			{
				this.calendarWeekRule = (int)this.m_cultureTableRecord.IFIRSTWEEKOFYEAR;
			}
			if (this.yearMonthPattern == null)
			{
				this.yearMonthPattern = this.GetYearMonthPattern(this.calendar.ID);
			}
			if (this.shortDatePattern == null)
			{
				this.shortDatePattern = this.GetShortDatePattern(this.calendar.ID);
			}
			if (this.longDatePattern == null)
			{
				this.longDatePattern = this.GetLongDatePattern(this.calendar.ID);
			}
		}

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x06002532 RID: 9522 RVA: 0x00065BE9 File Offset: 0x00064BE9
		// (set) Token: 0x06002533 RID: 9523 RVA: 0x00065BF4 File Offset: 0x00064BF4
		public Calendar Calendar
		{
			get
			{
				return this.calendar;
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("value", Environment.GetResourceString("ArgumentNull_Obj"));
				}
				if (value == this.calendar)
				{
					return;
				}
				CultureInfo.CheckDomainSafetyObject(value, this);
				for (int i = 0; i < this.OptionalCalendars.Length; i++)
				{
					if (this.OptionalCalendars[i] == value.ID)
					{
						this.ClearTokenHashTable(false);
						this.m_isDefaultCalendar = (value.ID == 1);
						if (this.calendar != null)
						{
							this.m_eraNames = null;
							this.m_abbrevEraNames = null;
							this.m_abbrevEnglishEraNames = null;
							this.shortDatePattern = null;
							this.yearMonthPattern = null;
							this.monthDayPattern = null;
							this.longDatePattern = null;
							this.dayNames = null;
							this.abbreviatedDayNames = null;
							this.m_superShortDayNames = null;
							this.monthNames = null;
							this.abbreviatedMonthNames = null;
							this.genitiveMonthNames = null;
							this.m_genitiveAbbreviatedMonthNames = null;
							this.leapYearMonthNames = null;
							this.formatFlags = DateTimeFormatFlags.NotInitialized;
							this.fullDateTimePattern = null;
							this.generalShortTimePattern = null;
							this.generalLongTimePattern = null;
							this.allShortDatePatterns = null;
							this.allLongDatePatterns = null;
							this.allYearMonthPatterns = null;
							this.dateTimeOffsetPattern = null;
						}
						this.calendar = value;
						if (this.m_cultureTableRecord.UseCurrentCalendar(value.ID))
						{
							DTFIUserOverrideValues dtfiuserOverrideValues = default(DTFIUserOverrideValues);
							this.m_cultureTableRecord.GetDTFIOverrideValues(ref dtfiuserOverrideValues);
							if (this.m_cultureTableRecord.SLONGDATE != dtfiuserOverrideValues.longDatePattern || this.m_cultureTableRecord.SSHORTDATE != dtfiuserOverrideValues.shortDatePattern || this.m_cultureTableRecord.STIMEFORMAT != dtfiuserOverrideValues.longTimePattern || this.m_cultureTableRecord.SYEARMONTH != dtfiuserOverrideValues.yearMonthPattern)
							{
								this.m_scanDateWords = true;
							}
							this.amDesignator = dtfiuserOverrideValues.amDesignator;
							this.pmDesignator = dtfiuserOverrideValues.pmDesignator;
							this.longTimePattern = dtfiuserOverrideValues.longTimePattern;
							this.firstDayOfWeek = dtfiuserOverrideValues.firstDayOfWeek;
							this.calendarWeekRule = dtfiuserOverrideValues.calendarWeekRule;
							this.shortDatePattern = dtfiuserOverrideValues.shortDatePattern;
							this.longDatePattern = dtfiuserOverrideValues.longDatePattern;
							this.yearMonthPattern = dtfiuserOverrideValues.yearMonthPattern;
							if (this.yearMonthPattern == null || this.yearMonthPattern.Length == 0)
							{
								this.yearMonthPattern = this.GetYearMonthPattern(value.ID);
								return;
							}
						}
						else
						{
							this.InitializeOverridableProperties();
						}
						return;
					}
				}
				throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("Argument_InvalidCalendar"));
			}
		}

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x06002534 RID: 9524 RVA: 0x00065E66 File Offset: 0x00064E66
		internal int[] OptionalCalendars
		{
			get
			{
				if (this.optionalCalendars == null)
				{
					this.optionalCalendars = this.m_cultureTableRecord.IOPTIONALCALENDARS;
				}
				return this.optionalCalendars;
			}
		}

		// Token: 0x06002535 RID: 9525 RVA: 0x00065E88 File Offset: 0x00064E88
		public int GetEra(string eraName)
		{
			if (eraName == null)
			{
				throw new ArgumentNullException("eraName", Environment.GetResourceString("ArgumentNull_String"));
			}
			for (int i = 0; i < this.EraNames.Length; i++)
			{
				if (this.m_eraNames[i].Length > 0 && string.Compare(eraName, this.m_eraNames[i], true, CultureInfo.CurrentCulture) == 0)
				{
					return i + 1;
				}
			}
			for (int j = 0; j < this.AbbreviatedEraNames.Length; j++)
			{
				if (string.Compare(eraName, this.m_abbrevEraNames[j], true, CultureInfo.CurrentCulture) == 0)
				{
					return j + 1;
				}
			}
			for (int k = 0; k < this.AbbreviatedEnglishEraNames.Length; k++)
			{
				if (string.Compare(eraName, this.m_abbrevEnglishEraNames[k], true, CultureInfo.InvariantCulture) == 0)
				{
					return k + 1;
				}
			}
			return -1;
		}

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x06002536 RID: 9526 RVA: 0x00065F48 File Offset: 0x00064F48
		internal string[] EraNames
		{
			get
			{
				if (this.m_eraNames == null)
				{
					if (this.Calendar.ID == 1)
					{
						this.m_eraNames = new string[]
						{
							this.m_cultureTableRecord.SADERA
						};
					}
					else if (this.Calendar.ID != 4)
					{
						this.m_eraNames = CalendarTable.Default.SERANAMES(this.Calendar.ID);
					}
					else
					{
						this.m_eraNames = new string[]
						{
							CalendarTable.nativeGetEraName(1028, this.Calendar.ID)
						};
					}
				}
				return this.m_eraNames;
			}
		}

		// Token: 0x06002537 RID: 9527 RVA: 0x00065FE0 File Offset: 0x00064FE0
		public string GetEraName(int era)
		{
			if (era == 0)
			{
				era = this.Calendar.CurrentEraValue;
			}
			if (--era < this.EraNames.Length && era >= 0)
			{
				return this.m_eraNames[era];
			}
			throw new ArgumentOutOfRangeException("era", Environment.GetResourceString("ArgumentOutOfRange_InvalidEraValue"));
		}

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x06002538 RID: 9528 RVA: 0x00066030 File Offset: 0x00065030
		internal string[] AbbreviatedEraNames
		{
			get
			{
				if (this.m_abbrevEraNames == null)
				{
					if (this.Calendar.ID == 4)
					{
						string eraName = this.GetEraName(1);
						if (eraName.Length > 0)
						{
							if (eraName.Length == 4)
							{
								this.m_abbrevEraNames = new string[]
								{
									eraName.Substring(2, 2)
								};
							}
							else
							{
								this.m_abbrevEraNames = new string[]
								{
									eraName
								};
							}
						}
						else
						{
							this.m_abbrevEraNames = new string[0];
						}
					}
					else if (this.Calendar.ID == 1)
					{
						this.m_abbrevEraNames = new string[]
						{
							this.m_cultureTableRecord.SABBREVADERA
						};
					}
					else
					{
						this.m_abbrevEraNames = CalendarTable.Default.SABBREVERANAMES(this.Calendar.ID);
					}
				}
				return this.m_abbrevEraNames;
			}
		}

		// Token: 0x06002539 RID: 9529 RVA: 0x000660FC File Offset: 0x000650FC
		public string GetAbbreviatedEraName(int era)
		{
			if (this.AbbreviatedEraNames.Length == 0)
			{
				return this.GetEraName(era);
			}
			if (era == 0)
			{
				era = this.Calendar.CurrentEraValue;
			}
			if (--era < this.m_abbrevEraNames.Length && era >= 0)
			{
				return this.m_abbrevEraNames[era];
			}
			throw new ArgumentOutOfRangeException("era", Environment.GetResourceString("ArgumentOutOfRange_InvalidEraValue"));
		}

		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x0600253A RID: 9530 RVA: 0x0006615C File Offset: 0x0006515C
		internal string[] AbbreviatedEnglishEraNames
		{
			get
			{
				if (this.m_abbrevEnglishEraNames == null)
				{
					this.m_abbrevEnglishEraNames = CalendarTable.Default.SABBREVENGERANAMES(this.Calendar.ID);
				}
				return this.m_abbrevEnglishEraNames;
			}
		}

		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x0600253B RID: 9531 RVA: 0x00066188 File Offset: 0x00065188
		// (set) Token: 0x0600253C RID: 9532 RVA: 0x000661D6 File Offset: 0x000651D6
		public string DateSeparator
		{
			get
			{
				if (this.dateSeparator == null)
				{
					if (this.Calendar.ID == 3 && !GregorianCalendarHelper.EnforceLegacyJapaneseDateParsing)
					{
						this.dateSeparator = "/";
					}
					else
					{
						this.dateSeparator = this.m_cultureTableRecord.SDATE;
					}
				}
				return this.dateSeparator;
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("value", Environment.GetResourceString("ArgumentNull_String"));
				}
				this.ClearTokenHashTable(true);
				this.dateSeparator = value;
			}
		}

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x0600253D RID: 9533 RVA: 0x00066204 File Offset: 0x00065204
		// (set) Token: 0x0600253E RID: 9534 RVA: 0x0006620C File Offset: 0x0006520C
		public DayOfWeek FirstDayOfWeek
		{
			get
			{
				return (DayOfWeek)this.firstDayOfWeek;
			}
			set
			{
				this.VerifyWritable();
				if (value >= DayOfWeek.Sunday && value <= DayOfWeek.Saturday)
				{
					this.firstDayOfWeek = (int)value;
					return;
				}
				throw new ArgumentOutOfRangeException("value", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					DayOfWeek.Sunday,
					DayOfWeek.Saturday
				}));
			}
		}

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x0600253F RID: 9535 RVA: 0x00066267 File Offset: 0x00065267
		// (set) Token: 0x06002540 RID: 9536 RVA: 0x00066270 File Offset: 0x00065270
		public CalendarWeekRule CalendarWeekRule
		{
			get
			{
				return (CalendarWeekRule)this.calendarWeekRule;
			}
			set
			{
				this.VerifyWritable();
				if (value >= CalendarWeekRule.FirstDay && value <= CalendarWeekRule.FirstFourDayWeek)
				{
					this.calendarWeekRule = (int)value;
					return;
				}
				throw new ArgumentOutOfRangeException("value", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					CalendarWeekRule.FirstDay,
					CalendarWeekRule.FirstFourDayWeek
				}));
			}
		}

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x06002541 RID: 9537 RVA: 0x000662CB File Offset: 0x000652CB
		// (set) Token: 0x06002542 RID: 9538 RVA: 0x000662F7 File Offset: 0x000652F7
		public string FullDateTimePattern
		{
			get
			{
				if (this.fullDateTimePattern == null)
				{
					this.fullDateTimePattern = this.LongDatePattern + " " + this.LongTimePattern;
				}
				return this.fullDateTimePattern;
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("value", Environment.GetResourceString("ArgumentNull_String"));
				}
				this.fullDateTimePattern = value;
			}
		}

		// Token: 0x06002543 RID: 9539 RVA: 0x00066320 File Offset: 0x00065320
		private string GetLongDatePattern(int calID)
		{
			string result = string.Empty;
			if (!this.m_isDefaultCalendar)
			{
				result = CalendarTable.Default.SLONGDATE(calID)[0];
			}
			else
			{
				result = this.m_cultureTableRecord.SLONGDATE;
			}
			return result;
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x06002544 RID: 9540 RVA: 0x00066358 File Offset: 0x00065358
		// (set) Token: 0x06002545 RID: 9541 RVA: 0x00066360 File Offset: 0x00065360
		public string LongDatePattern
		{
			get
			{
				return this.longDatePattern;
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("value", Environment.GetResourceString("ArgumentNull_String"));
				}
				this.ClearTokenHashTable(true);
				this.SetDefaultPatternAsFirstItem(this.allLongDatePatterns, value);
				this.longDatePattern = value;
				this.fullDateTimePattern = null;
			}
		}

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x06002546 RID: 9542 RVA: 0x000663AD File Offset: 0x000653AD
		// (set) Token: 0x06002547 RID: 9543 RVA: 0x000663B5 File Offset: 0x000653B5
		public string LongTimePattern
		{
			get
			{
				return this.longTimePattern;
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("value", Environment.GetResourceString("ArgumentNull_String"));
				}
				this.longTimePattern = value;
				this.fullDateTimePattern = null;
				this.generalLongTimePattern = null;
				this.dateTimeOffsetPattern = null;
			}
		}

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x06002548 RID: 9544 RVA: 0x000663F4 File Offset: 0x000653F4
		// (set) Token: 0x06002549 RID: 9545 RVA: 0x00066456 File Offset: 0x00065456
		public string MonthDayPattern
		{
			get
			{
				if (this.monthDayPattern == null)
				{
					string text;
					if (this.m_isDefaultCalendar)
					{
						text = this.m_cultureTableRecord.SMONTHDAY;
					}
					else
					{
						text = CalendarTable.Default.SMONTHDAY(this.Calendar.ID);
						if (text.Length == 0)
						{
							text = this.m_cultureTableRecord.SMONTHDAY;
						}
					}
					this.monthDayPattern = text;
				}
				return this.monthDayPattern;
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("value", Environment.GetResourceString("ArgumentNull_String"));
				}
				this.monthDayPattern = value;
			}
		}

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x0600254A RID: 9546 RVA: 0x0006647D File Offset: 0x0006547D
		// (set) Token: 0x0600254B RID: 9547 RVA: 0x00066485 File Offset: 0x00065485
		public string PMDesignator
		{
			get
			{
				return this.pmDesignator;
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("value", Environment.GetResourceString("ArgumentNull_String"));
				}
				this.ClearTokenHashTable(true);
				this.pmDesignator = value;
			}
		}

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x0600254C RID: 9548 RVA: 0x000664B3 File Offset: 0x000654B3
		public string RFC1123Pattern
		{
			get
			{
				return "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
			}
		}

		// Token: 0x0600254D RID: 9549 RVA: 0x000664BC File Offset: 0x000654BC
		internal string GetShortDatePattern(int calID)
		{
			string result = string.Empty;
			if (!this.m_isDefaultCalendar)
			{
				result = CalendarTable.Default.SSHORTDATE(calID)[0];
			}
			else
			{
				result = this.m_cultureTableRecord.SSHORTDATE;
			}
			return result;
		}

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x0600254E RID: 9550 RVA: 0x000664F4 File Offset: 0x000654F4
		// (set) Token: 0x0600254F RID: 9551 RVA: 0x000664FC File Offset: 0x000654FC
		public string ShortDatePattern
		{
			get
			{
				return this.shortDatePattern;
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("value", Environment.GetResourceString("ArgumentNull_String"));
				}
				this.SetDefaultPatternAsFirstItem(this.allShortDatePatterns, value);
				this.shortDatePattern = value;
				this.generalLongTimePattern = null;
				this.generalShortTimePattern = null;
				this.dateTimeOffsetPattern = null;
			}
		}

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x06002550 RID: 9552 RVA: 0x00066550 File Offset: 0x00065550
		// (set) Token: 0x06002551 RID: 9553 RVA: 0x00066571 File Offset: 0x00065571
		public string ShortTimePattern
		{
			get
			{
				if (this.shortTimePattern == null)
				{
					this.shortTimePattern = this.m_cultureTableRecord.SSHORTTIME;
				}
				return this.shortTimePattern;
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("value", Environment.GetResourceString("ArgumentNull_String"));
				}
				this.shortTimePattern = value;
				this.generalShortTimePattern = null;
			}
		}

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x06002552 RID: 9554 RVA: 0x0006659F File Offset: 0x0006559F
		public string SortableDateTimePattern
		{
			get
			{
				return "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
			}
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x06002553 RID: 9555 RVA: 0x000665A6 File Offset: 0x000655A6
		internal string GeneralShortTimePattern
		{
			get
			{
				if (this.generalShortTimePattern == null)
				{
					this.generalShortTimePattern = this.ShortDatePattern + " " + this.ShortTimePattern;
				}
				return this.generalShortTimePattern;
			}
		}

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x06002554 RID: 9556 RVA: 0x000665D2 File Offset: 0x000655D2
		internal string GeneralLongTimePattern
		{
			get
			{
				if (this.generalLongTimePattern == null)
				{
					this.generalLongTimePattern = this.ShortDatePattern + " " + this.LongTimePattern;
				}
				return this.generalLongTimePattern;
			}
		}

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x06002555 RID: 9557 RVA: 0x000665FE File Offset: 0x000655FE
		internal string DateTimeOffsetPattern
		{
			get
			{
				if (this.dateTimeOffsetPattern == null)
				{
					this.dateTimeOffsetPattern = this.ShortDatePattern + " " + this.LongTimePattern + " zzz";
				}
				return this.dateTimeOffsetPattern;
			}
		}

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x06002556 RID: 9558 RVA: 0x0006662F File Offset: 0x0006562F
		// (set) Token: 0x06002557 RID: 9559 RVA: 0x00066650 File Offset: 0x00065650
		public string TimeSeparator
		{
			get
			{
				if (this.timeSeparator == null)
				{
					this.timeSeparator = this.m_cultureTableRecord.STIME;
				}
				return this.timeSeparator;
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("value", Environment.GetResourceString("ArgumentNull_String"));
				}
				this.ClearTokenHashTable(true);
				this.timeSeparator = value;
			}
		}

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x06002558 RID: 9560 RVA: 0x0006667E File Offset: 0x0006567E
		public string UniversalSortableDateTimePattern
		{
			get
			{
				return "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
			}
		}

		// Token: 0x06002559 RID: 9561 RVA: 0x00066688 File Offset: 0x00065688
		private string GetYearMonthPattern(int calID)
		{
			string result;
			if (!this.m_isDefaultCalendar)
			{
				result = CalendarTable.Default.SYEARMONTH(calID)[0];
			}
			else
			{
				result = this.m_cultureTableRecord.SYEARMONTHS[0];
			}
			return result;
		}

		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x0600255A RID: 9562 RVA: 0x000666BE File Offset: 0x000656BE
		// (set) Token: 0x0600255B RID: 9563 RVA: 0x000666C6 File Offset: 0x000656C6
		public string YearMonthPattern
		{
			get
			{
				return this.yearMonthPattern;
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("value", Environment.GetResourceString("ArgumentNull_String"));
				}
				this.yearMonthPattern = value;
				this.SetDefaultPatternAsFirstItem(this.allYearMonthPatterns, this.yearMonthPattern);
			}
		}

		// Token: 0x0600255C RID: 9564 RVA: 0x00066700 File Offset: 0x00065700
		private void CheckNullValue(string[] values, int length)
		{
			for (int i = 0; i < length; i++)
			{
				if (values[i] == null)
				{
					throw new ArgumentNullException("value", Environment.GetResourceString("ArgumentNull_ArrayValue"));
				}
			}
		}

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x0600255D RID: 9565 RVA: 0x00066733 File Offset: 0x00065733
		// (set) Token: 0x0600255E RID: 9566 RVA: 0x00066748 File Offset: 0x00065748
		public string[] AbbreviatedDayNames
		{
			get
			{
				return (string[])this.GetAbbreviatedDayOfWeekNames().Clone();
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("value", Environment.GetResourceString("ArgumentNull_Array"));
				}
				if (value.Length != 7)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidArrayLength"), new object[]
					{
						7
					}), "value");
				}
				this.CheckNullValue(value, value.Length);
				this.ClearTokenHashTable(true);
				this.abbreviatedDayNames = value;
			}
		}

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x0600255F RID: 9567 RVA: 0x000667C1 File Offset: 0x000657C1
		// (set) Token: 0x06002560 RID: 9568 RVA: 0x000667D4 File Offset: 0x000657D4
		[ComVisible(false)]
		public string[] ShortestDayNames
		{
			get
			{
				return (string[])this.internalGetSuperShortDayNames().Clone();
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("value", Environment.GetResourceString("ArgumentNull_Array"));
				}
				if (value.Length != 7)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidArrayLength"), new object[]
					{
						7
					}), "value");
				}
				this.CheckNullValue(value, value.Length);
				this.m_superShortDayNames = value;
			}
		}

		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x06002561 RID: 9569 RVA: 0x00066846 File Offset: 0x00065846
		// (set) Token: 0x06002562 RID: 9570 RVA: 0x00066858 File Offset: 0x00065858
		public string[] DayNames
		{
			get
			{
				return (string[])this.GetDayOfWeekNames().Clone();
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("value", Environment.GetResourceString("ArgumentNull_Array"));
				}
				if (value.Length != 7)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidArrayLength"), new object[]
					{
						7
					}), "value");
				}
				this.CheckNullValue(value, value.Length);
				this.ClearTokenHashTable(true);
				this.dayNames = value;
			}
		}

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x06002563 RID: 9571 RVA: 0x000668D1 File Offset: 0x000658D1
		// (set) Token: 0x06002564 RID: 9572 RVA: 0x000668E4 File Offset: 0x000658E4
		public string[] AbbreviatedMonthNames
		{
			get
			{
				return (string[])this.GetAbbreviatedMonthNames().Clone();
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("value", Environment.GetResourceString("ArgumentNull_Array"));
				}
				if (value.Length != 13)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidArrayLength"), new object[]
					{
						13
					}), "value");
				}
				this.CheckNullValue(value, value.Length - 1);
				this.ClearTokenHashTable(true);
				this.abbreviatedMonthNames = value;
			}
		}

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x06002565 RID: 9573 RVA: 0x00066961 File Offset: 0x00065961
		// (set) Token: 0x06002566 RID: 9574 RVA: 0x00066974 File Offset: 0x00065974
		public string[] MonthNames
		{
			get
			{
				return (string[])this.GetMonthNames().Clone();
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("value", Environment.GetResourceString("ArgumentNull_Array"));
				}
				if (value.Length != 13)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidArrayLength"), new object[]
					{
						13
					}), "value");
				}
				this.CheckNullValue(value, value.Length - 1);
				this.monthNames = value;
				this.ClearTokenHashTable(true);
			}
		}

		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x06002567 RID: 9575 RVA: 0x000669F1 File Offset: 0x000659F1
		internal bool HasSpacesInMonthNames
		{
			get
			{
				return (this.FormatFlags & DateTimeFormatFlags.UseSpacesInMonthNames) != DateTimeFormatFlags.None;
			}
		}

		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x06002568 RID: 9576 RVA: 0x00066A01 File Offset: 0x00065A01
		internal bool HasSpacesInDayNames
		{
			get
			{
				return (this.FormatFlags & DateTimeFormatFlags.UseSpacesInDayNames) != DateTimeFormatFlags.None;
			}
		}

		// Token: 0x06002569 RID: 9577 RVA: 0x00066A14 File Offset: 0x00065A14
		internal string internalGetMonthName(int month, MonthNameStyles style, bool abbreviated)
		{
			string[] array;
			switch (style)
			{
			case MonthNameStyles.Genitive:
				array = this.internalGetGenitiveMonthNames(abbreviated);
				break;
			case MonthNameStyles.LeapYear:
				array = this.internalGetLeapYearMonthNames();
				break;
			default:
				array = (abbreviated ? this.GetAbbreviatedMonthNames() : this.GetMonthNames());
				break;
			}
			if (month < 1 || month > array.Length)
			{
				throw new ArgumentOutOfRangeException("month", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					1,
					array.Length
				}));
			}
			return array[month - 1];
		}

		// Token: 0x0600256A RID: 9578 RVA: 0x00066AA8 File Offset: 0x00065AA8
		private string[] internalGetGenitiveMonthNames(bool abbreviated)
		{
			if (abbreviated)
			{
				if (this.m_genitiveAbbreviatedMonthNames == null)
				{
					if (this.m_isDefaultCalendar)
					{
						string[] sabbrevmonthgenitivenames = this.m_cultureTableRecord.SABBREVMONTHGENITIVENAMES;
						if (sabbrevmonthgenitivenames.Length > 0)
						{
							this.m_genitiveAbbreviatedMonthNames = sabbrevmonthgenitivenames;
						}
						else
						{
							this.m_genitiveAbbreviatedMonthNames = this.GetAbbreviatedMonthNames();
						}
					}
					else
					{
						this.m_genitiveAbbreviatedMonthNames = this.GetAbbreviatedMonthNames();
					}
				}
				return this.m_genitiveAbbreviatedMonthNames;
			}
			if (this.genitiveMonthNames == null)
			{
				if (this.m_isDefaultCalendar)
				{
					string[] smonthgenitivenames = this.m_cultureTableRecord.SMONTHGENITIVENAMES;
					if (smonthgenitivenames.Length > 0)
					{
						this.genitiveMonthNames = smonthgenitivenames;
					}
					else
					{
						this.genitiveMonthNames = this.GetMonthNames();
					}
				}
				else
				{
					this.genitiveMonthNames = this.GetMonthNames();
				}
			}
			return this.genitiveMonthNames;
		}

		// Token: 0x0600256B RID: 9579 RVA: 0x00066B50 File Offset: 0x00065B50
		internal string[] internalGetLeapYearMonthNames()
		{
			if (this.leapYearMonthNames == null)
			{
				if (this.m_isDefaultCalendar)
				{
					this.leapYearMonthNames = this.GetMonthNames();
				}
				else
				{
					string[] array = CalendarTable.Default.SLEAPYEARMONTHNAMES(this.Calendar.ID);
					if (array.Length > 0)
					{
						this.leapYearMonthNames = array;
					}
					else
					{
						this.leapYearMonthNames = this.GetMonthNames();
					}
				}
			}
			return this.leapYearMonthNames;
		}

		// Token: 0x0600256C RID: 9580 RVA: 0x00066BB4 File Offset: 0x00065BB4
		public string GetAbbreviatedDayName(DayOfWeek dayofweek)
		{
			if (dayofweek < DayOfWeek.Sunday || dayofweek > DayOfWeek.Saturday)
			{
				throw new ArgumentOutOfRangeException("dayofweek", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					DayOfWeek.Sunday,
					DayOfWeek.Saturday
				}));
			}
			return this.GetAbbreviatedDayOfWeekNames()[(int)dayofweek];
		}

		// Token: 0x0600256D RID: 9581 RVA: 0x00066C0C File Offset: 0x00065C0C
		[ComVisible(false)]
		public string GetShortestDayName(DayOfWeek dayOfWeek)
		{
			if (dayOfWeek < DayOfWeek.Sunday || dayOfWeek > DayOfWeek.Saturday)
			{
				throw new ArgumentOutOfRangeException("dayOfWeek", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					DayOfWeek.Sunday,
					DayOfWeek.Saturday
				}));
			}
			return this.internalGetSuperShortDayNames()[(int)dayOfWeek];
		}

		// Token: 0x0600256E RID: 9582 RVA: 0x00066C64 File Offset: 0x00065C64
		internal string[] GetCombinedPatterns(string[] patterns1, string[] patterns2, string connectString)
		{
			string[] array = new string[patterns1.Length * patterns2.Length];
			for (int i = 0; i < patterns1.Length; i++)
			{
				for (int j = 0; j < patterns2.Length; j++)
				{
					array[i * patterns2.Length + j] = patterns1[i] + connectString + patterns2[j];
				}
			}
			return array;
		}

		// Token: 0x0600256F RID: 9583 RVA: 0x00066CB0 File Offset: 0x00065CB0
		public string[] GetAllDateTimePatterns()
		{
			ArrayList arrayList = new ArrayList(132);
			for (int i = 0; i < DateTimeFormat.allStandardFormats.Length; i++)
			{
				string[] allDateTimePatterns = this.GetAllDateTimePatterns(DateTimeFormat.allStandardFormats[i]);
				for (int j = 0; j < allDateTimePatterns.Length; j++)
				{
					arrayList.Add(allDateTimePatterns[j]);
				}
			}
			string[] array = new string[arrayList.Count];
			arrayList.CopyTo(0, array, 0, arrayList.Count);
			return array;
		}

		// Token: 0x06002570 RID: 9584 RVA: 0x00066D20 File Offset: 0x00065D20
		public string[] GetAllDateTimePatterns(char format)
		{
			if (format > 'U')
			{
				if (format != 'Y')
				{
					switch (format)
					{
					case 'd':
						return this.ClonedAllShortDatePatterns;
					case 'e':
						goto IL_1E8;
					case 'f':
						return this.GetCombinedPatterns(this.ClonedAllLongDatePatterns, this.ClonedAllShortTimePatterns, " ");
					case 'g':
						return this.GetCombinedPatterns(this.ClonedAllShortDatePatterns, this.ClonedAllShortTimePatterns, " ");
					default:
						switch (format)
						{
						case 'm':
							goto IL_143;
						case 'n':
						case 'p':
						case 'q':
						case 'v':
						case 'w':
						case 'x':
							goto IL_1E8;
						case 'o':
							goto IL_15A;
						case 'r':
							goto IL_170;
						case 's':
							return new string[]
							{
								"yyyy'-'MM'-'dd'T'HH':'mm':'ss"
							};
						case 't':
							return this.ClonedAllShortTimePatterns;
						case 'u':
							return new string[]
							{
								this.UniversalSortableDateTimePattern
							};
						case 'y':
							break;
						default:
							goto IL_1E8;
						}
						break;
					}
				}
				return this.ClonedAllYearMonthPatterns;
			}
			switch (format)
			{
			case 'D':
				return this.ClonedAllLongDatePatterns;
			case 'E':
				goto IL_1E8;
			case 'F':
				return this.GetCombinedPatterns(this.ClonedAllLongDatePatterns, this.ClonedAllLongTimePatterns, " ");
			case 'G':
				return this.GetCombinedPatterns(this.ClonedAllShortDatePatterns, this.ClonedAllLongTimePatterns, " ");
			default:
				switch (format)
				{
				case 'M':
					break;
				case 'N':
				case 'P':
				case 'Q':
				case 'S':
					goto IL_1E8;
				case 'O':
					goto IL_15A;
				case 'R':
					goto IL_170;
				case 'T':
					return this.ClonedAllLongTimePatterns;
				case 'U':
					return this.GetCombinedPatterns(this.ClonedAllLongDatePatterns, this.ClonedAllLongTimePatterns, " ");
				default:
					goto IL_1E8;
				}
				break;
			}
			IL_143:
			return new string[]
			{
				this.MonthDayPattern
			};
			IL_15A:
			return new string[]
			{
				"yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK"
			};
			IL_170:
			return new string[]
			{
				"ddd, dd MMM yyyy HH':'mm':'ss 'GMT'"
			};
			IL_1E8:
			throw new ArgumentException(Environment.GetResourceString("Argument_BadFormatSpecifier"), "format");
		}

		// Token: 0x06002571 RID: 9585 RVA: 0x00066F2C File Offset: 0x00065F2C
		public string GetDayName(DayOfWeek dayofweek)
		{
			if (dayofweek < DayOfWeek.Sunday || dayofweek > DayOfWeek.Saturday)
			{
				throw new ArgumentOutOfRangeException("dayofweek", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					DayOfWeek.Sunday,
					DayOfWeek.Saturday
				}));
			}
			return this.GetDayOfWeekNames()[(int)dayofweek];
		}

		// Token: 0x06002572 RID: 9586 RVA: 0x00066F84 File Offset: 0x00065F84
		public string GetAbbreviatedMonthName(int month)
		{
			if (month < 1 || month > 13)
			{
				throw new ArgumentOutOfRangeException("month", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					1,
					13
				}));
			}
			return this.GetAbbreviatedMonthNames()[month - 1];
		}

		// Token: 0x06002573 RID: 9587 RVA: 0x00066FE0 File Offset: 0x00065FE0
		public string GetMonthName(int month)
		{
			if (month < 1 || month > 13)
			{
				throw new ArgumentOutOfRangeException("month", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					1,
					13
				}));
			}
			return this.GetMonthNames()[month - 1];
		}

		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x06002574 RID: 9588 RVA: 0x0006703C File Offset: 0x0006603C
		internal string[] ClonedAllYearMonthPatterns
		{
			get
			{
				if (this.allYearMonthPatterns == null)
				{
					string[] array;
					if (!this.m_isDefaultCalendar)
					{
						array = CalendarTable.Default.SYEARMONTH(this.Calendar.ID);
						if (array == null)
						{
							array = this.m_cultureTableRecord.SYEARMONTHS;
						}
					}
					else
					{
						array = this.m_cultureTableRecord.SYEARMONTHS;
					}
					Thread.MemoryBarrier();
					this.SetDefaultPatternAsFirstItem(array, this.YearMonthPattern);
					this.allYearMonthPatterns = array;
				}
				if (this.allYearMonthPatterns[0].Equals(this.YearMonthPattern))
				{
					return (string[])this.allYearMonthPatterns.Clone();
				}
				return this.AddDefaultFormat(this.allYearMonthPatterns, this.YearMonthPattern);
			}
		}

		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x06002575 RID: 9589 RVA: 0x000670E0 File Offset: 0x000660E0
		internal string[] ClonedAllShortDatePatterns
		{
			get
			{
				if (this.allShortDatePatterns == null)
				{
					string[] array;
					if (!this.m_isDefaultCalendar)
					{
						array = CalendarTable.Default.SSHORTDATE(this.Calendar.ID);
						if (array == null)
						{
							array = this.m_cultureTableRecord.SSHORTDATES;
						}
					}
					else
					{
						array = this.m_cultureTableRecord.SSHORTDATES;
					}
					Thread.MemoryBarrier();
					this.SetDefaultPatternAsFirstItem(array, this.ShortDatePattern);
					this.allShortDatePatterns = array;
				}
				if (this.allShortDatePatterns[0].Equals(this.ShortDatePattern))
				{
					return (string[])this.allShortDatePatterns.Clone();
				}
				return this.AddDefaultFormat(this.allShortDatePatterns, this.ShortDatePattern);
			}
		}

		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x06002576 RID: 9590 RVA: 0x00067184 File Offset: 0x00066184
		internal string[] ClonedAllLongDatePatterns
		{
			get
			{
				if (this.allLongDatePatterns == null)
				{
					string[] array;
					if (!this.m_isDefaultCalendar)
					{
						array = CalendarTable.Default.SLONGDATE(this.Calendar.ID);
						if (array == null)
						{
							array = this.m_cultureTableRecord.SLONGDATES;
						}
					}
					else
					{
						array = this.m_cultureTableRecord.SLONGDATES;
					}
					Thread.MemoryBarrier();
					this.SetDefaultPatternAsFirstItem(array, this.LongDatePattern);
					this.allLongDatePatterns = array;
				}
				if (this.allLongDatePatterns[0].Equals(this.LongDatePattern))
				{
					return (string[])this.allLongDatePatterns.Clone();
				}
				return this.AddDefaultFormat(this.allLongDatePatterns, this.LongDatePattern);
			}
		}

		// Token: 0x06002577 RID: 9591 RVA: 0x00067228 File Offset: 0x00066228
		internal void SetDefaultPatternAsFirstItem(string[] patterns, string defaultPattern)
		{
			if (patterns == null)
			{
				return;
			}
			for (int i = 0; i < patterns.Length; i++)
			{
				if (patterns[i].Equals(defaultPattern))
				{
					if (i != 0)
					{
						string text = patterns[i];
						for (int j = i; j > 0; j--)
						{
							patterns[j] = patterns[j - 1];
						}
						patterns[0] = text;
					}
					return;
				}
			}
		}

		// Token: 0x06002578 RID: 9592 RVA: 0x00067274 File Offset: 0x00066274
		internal string[] AddDefaultFormat(string[] datePatterns, string defaultDateFormat)
		{
			string[] array = new string[datePatterns.Length + 1];
			array[0] = defaultDateFormat;
			Array.Copy(datePatterns, 0, array, 1, datePatterns.Length);
			this.m_scanDateWords = true;
			return array;
		}

		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x06002579 RID: 9593 RVA: 0x000672A4 File Offset: 0x000662A4
		internal string[] ClonedAllShortTimePatterns
		{
			get
			{
				if (this.allShortTimePatterns == null)
				{
					this.allShortTimePatterns = this.m_cultureTableRecord.SSHORTTIMES;
					this.SetDefaultPatternAsFirstItem(this.allShortTimePatterns, this.ShortTimePattern);
				}
				if (this.allShortTimePatterns[0].Equals(this.ShortTimePattern))
				{
					return (string[])this.allShortTimePatterns.Clone();
				}
				return this.AddDefaultFormat(this.allShortTimePatterns, this.ShortTimePattern);
			}
		}

		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x0600257A RID: 9594 RVA: 0x00067314 File Offset: 0x00066314
		internal string[] ClonedAllLongTimePatterns
		{
			get
			{
				if (this.allLongTimePatterns == null)
				{
					this.allLongTimePatterns = this.m_cultureTableRecord.STIMEFORMATS;
					this.SetDefaultPatternAsFirstItem(this.allLongTimePatterns, this.LongTimePattern);
				}
				if (this.allLongTimePatterns[0].Equals(this.LongTimePattern))
				{
					return (string[])this.allLongTimePatterns.Clone();
				}
				return this.AddDefaultFormat(this.allLongTimePatterns, this.LongTimePattern);
			}
		}

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x0600257B RID: 9595 RVA: 0x00067384 File Offset: 0x00066384
		internal string[] DateWords
		{
			get
			{
				if (this.m_dateWords == null)
				{
					this.m_dateWords = this.m_cultureTableRecord.SDATEWORDS;
				}
				return this.m_dateWords;
			}
		}

		// Token: 0x0600257C RID: 9596 RVA: 0x000673A8 File Offset: 0x000663A8
		public static DateTimeFormatInfo ReadOnly(DateTimeFormatInfo dtfi)
		{
			if (dtfi == null)
			{
				throw new ArgumentNullException("dtfi", Environment.GetResourceString("ArgumentNull_Obj"));
			}
			if (dtfi.IsReadOnly)
			{
				return dtfi;
			}
			DateTimeFormatInfo dateTimeFormatInfo = (DateTimeFormatInfo)dtfi.MemberwiseClone();
			dateTimeFormatInfo.Calendar = Calendar.ReadOnly(dateTimeFormatInfo.Calendar);
			dateTimeFormatInfo.m_isReadOnly = true;
			return dateTimeFormatInfo;
		}

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x0600257D RID: 9597 RVA: 0x000673FC File Offset: 0x000663FC
		public bool IsReadOnly
		{
			get
			{
				return this.m_isReadOnly;
			}
		}

		// Token: 0x0600257E RID: 9598 RVA: 0x00067404 File Offset: 0x00066404
		private static int CalendarIdToCultureId(int calendarId)
		{
			switch (calendarId)
			{
			case 2:
				return 1065;
			case 3:
				return 1041;
			case 4:
				return 1028;
			case 5:
				return 1042;
			case 6:
			case 10:
			case 23:
				return 1025;
			case 7:
				return 1054;
			case 8:
				return 1037;
			case 9:
				return 5121;
			case 11:
			case 12:
				return 2049;
			}
			return 0;
		}

		// Token: 0x0600257F RID: 9599 RVA: 0x000674AC File Offset: 0x000664AC
		private string GetCalendarNativeNameFallback(int calendarId)
		{
			if (DateTimeFormatInfo.m_calendarNativeNames == null)
			{
				lock (DateTimeFormatInfo.InternalSyncObject)
				{
					if (DateTimeFormatInfo.m_calendarNativeNames == null)
					{
						DateTimeFormatInfo.m_calendarNativeNames = new Hashtable();
					}
				}
			}
			string text = (string)DateTimeFormatInfo.m_calendarNativeNames[calendarId];
			if (text != null)
			{
				return text;
			}
			string text2 = string.Empty;
			int num = DateTimeFormatInfo.CalendarIdToCultureId(calendarId);
			if (num != 0)
			{
				string[] snativecalnames = new CultureTableRecord(num, false).SNATIVECALNAMES;
				int num2 = this.calendar.ID - 1;
				if (num2 < snativecalnames.Length && snativecalnames[num2].Length > 0 && snativecalnames[num2][0] != '﻿')
				{
					text2 = snativecalnames[num2];
				}
			}
			lock (DateTimeFormatInfo.InternalSyncObject)
			{
				if (DateTimeFormatInfo.m_calendarNativeNames[calendarId] == null)
				{
					DateTimeFormatInfo.m_calendarNativeNames[calendarId] = text2;
				}
			}
			return text2;
		}

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x06002580 RID: 9600 RVA: 0x000675B0 File Offset: 0x000665B0
		[ComVisible(false)]
		public string NativeCalendarName
		{
			get
			{
				if (this.Calendar.ID == 4)
				{
					string text = DateTimeFormatInfo.GetCalendarInfo(1028, 4, 2);
					if (text == null)
					{
						text = CalendarTable.nativeGetEraName(1028, 4);
						if (text == null)
						{
							text = string.Empty;
						}
					}
					return text;
				}
				string[] snativecalnames = this.m_cultureTableRecord.SNATIVECALNAMES;
				int num = this.calendar.ID - 1;
				if (num < snativecalnames.Length)
				{
					if (snativecalnames[num].Length <= 0)
					{
						return this.GetCalendarNativeNameFallback(this.calendar.ID);
					}
					if (snativecalnames[num][0] != '﻿')
					{
						return snativecalnames[num];
					}
				}
				return string.Empty;
			}
		}

		// Token: 0x06002581 RID: 9601 RVA: 0x00067648 File Offset: 0x00066648
		[ComVisible(false)]
		public void SetAllDateTimePatterns(string[] patterns, char format)
		{
			this.VerifyWritable();
			if (patterns == null)
			{
				throw new ArgumentNullException("patterns", Environment.GetResourceString("ArgumentNull_Array"));
			}
			if (patterns.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_ArrayZeroError"), "patterns");
			}
			for (int i = 0; i < patterns.Length; i++)
			{
				if (patterns[i] == null)
				{
					throw new ArgumentNullException(Environment.GetResourceString("ArgumentNull_ArrayValue"));
				}
			}
			if (format <= 'Y')
			{
				if (format == 'D')
				{
					this.LongDatePattern = patterns[0];
					this.allLongDatePatterns = patterns;
					return;
				}
				if (format == 'T')
				{
					this.LongTimePattern = patterns[0];
					this.allLongTimePatterns = patterns;
					return;
				}
				if (format != 'Y')
				{
					goto IL_D9;
				}
			}
			else
			{
				if (format == 'd')
				{
					this.ShortDatePattern = patterns[0];
					this.allShortDatePatterns = patterns;
					return;
				}
				if (format == 't')
				{
					this.ShortTimePattern = patterns[0];
					this.allShortTimePatterns = patterns;
					return;
				}
				if (format != 'y')
				{
					goto IL_D9;
				}
			}
			this.yearMonthPattern = patterns[0];
			this.allYearMonthPatterns = patterns;
			return;
			IL_D9:
			throw new ArgumentException(Environment.GetResourceString("Argument_BadFormatSpecifier"), "format");
		}

		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x06002582 RID: 9602 RVA: 0x00067742 File Offset: 0x00066742
		// (set) Token: 0x06002583 RID: 9603 RVA: 0x00067758 File Offset: 0x00066758
		[ComVisible(false)]
		public string[] AbbreviatedMonthGenitiveNames
		{
			get
			{
				return (string[])this.internalGetGenitiveMonthNames(true).Clone();
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("value", Environment.GetResourceString("ArgumentNull_Array"));
				}
				if (value.Length != 13)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidArrayLength"), new object[]
					{
						13
					}), "value");
				}
				this.CheckNullValue(value, value.Length - 1);
				this.ClearTokenHashTable(true);
				this.m_genitiveAbbreviatedMonthNames = value;
			}
		}

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x06002584 RID: 9604 RVA: 0x000677D5 File Offset: 0x000667D5
		// (set) Token: 0x06002585 RID: 9605 RVA: 0x000677E8 File Offset: 0x000667E8
		[ComVisible(false)]
		public string[] MonthGenitiveNames
		{
			get
			{
				return (string[])this.internalGetGenitiveMonthNames(false).Clone();
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("value", Environment.GetResourceString("ArgumentNull_Array"));
				}
				if (value.Length != 13)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidArrayLength"), new object[]
					{
						13
					}), "value");
				}
				this.CheckNullValue(value, value.Length - 1);
				this.genitiveMonthNames = value;
				this.ClearTokenHashTable(true);
			}
		}

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x06002586 RID: 9606 RVA: 0x00067868 File Offset: 0x00066868
		internal CompareInfo CompareInfo
		{
			get
			{
				if (this.m_compareInfo == null)
				{
					if (CultureTableRecord.IsCustomCultureId(this.CultureId))
					{
						this.m_compareInfo = CompareInfo.GetCompareInfo((int)this.m_cultureTableRecord.ICOMPAREINFO);
					}
					else
					{
						this.m_compareInfo = CompareInfo.GetCompareInfo(this.CultureId);
					}
				}
				return this.m_compareInfo;
			}
		}

		// Token: 0x06002587 RID: 9607 RVA: 0x000678B9 File Offset: 0x000668B9
		private void VerifyWritable()
		{
			if (this.m_isReadOnly)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ReadOnly"));
			}
		}

		// Token: 0x06002588 RID: 9608 RVA: 0x000678D4 File Offset: 0x000668D4
		internal static void ValidateStyles(DateTimeStyles style, string parameterName)
		{
			if ((style & ~(DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite | DateTimeStyles.AllowInnerWhite | DateTimeStyles.NoCurrentDateDefault | DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeLocal | DateTimeStyles.AssumeUniversal | DateTimeStyles.RoundtripKind)) != DateTimeStyles.None)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidDateTimeStyles"), parameterName);
			}
			if ((style & DateTimeStyles.AssumeLocal) != DateTimeStyles.None && (style & DateTimeStyles.AssumeUniversal) != DateTimeStyles.None)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_ConflictingDateTimeStyles"), parameterName);
			}
			if ((style & DateTimeStyles.RoundtripKind) != DateTimeStyles.None && (style & (DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeLocal | DateTimeStyles.AssumeUniversal)) != DateTimeStyles.None)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_ConflictingDateTimeRoundtripStyles"), parameterName);
			}
		}

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x06002589 RID: 9609 RVA: 0x00067938 File Offset: 0x00066938
		internal DateTimeFormatFlags FormatFlags
		{
			get
			{
				if (this.formatFlags == DateTimeFormatFlags.NotInitialized)
				{
					if (this.m_scanDateWords || this.m_cultureTableRecord.IsSynthetic)
					{
						this.formatFlags = DateTimeFormatFlags.None;
						this.formatFlags |= (DateTimeFormatFlags)DateTimeFormatInfoScanner.GetFormatFlagGenitiveMonth(this.MonthNames, this.internalGetGenitiveMonthNames(false), this.AbbreviatedMonthNames, this.internalGetGenitiveMonthNames(true));
						this.formatFlags |= (DateTimeFormatFlags)DateTimeFormatInfoScanner.GetFormatFlagUseSpaceInMonthNames(this.MonthNames, this.internalGetGenitiveMonthNames(false), this.AbbreviatedMonthNames, this.internalGetGenitiveMonthNames(true));
						this.formatFlags |= (DateTimeFormatFlags)DateTimeFormatInfoScanner.GetFormatFlagUseSpaceInDayNames(this.DayNames, this.AbbreviatedDayNames);
						this.formatFlags |= (DateTimeFormatFlags)DateTimeFormatInfoScanner.GetFormatFlagUseHebrewCalendar(this.Calendar.ID);
					}
					else if (this.m_isDefaultCalendar)
					{
						this.formatFlags = this.m_cultureTableRecord.IFORMATFLAGS;
					}
					else
					{
						this.formatFlags = (DateTimeFormatFlags)CalendarTable.Default.IFORMATFLAGS(this.Calendar.ID);
					}
				}
				return this.formatFlags;
			}
		}

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x0600258A RID: 9610 RVA: 0x00067A44 File Offset: 0x00066A44
		internal bool HasForceTwoDigitYears
		{
			get
			{
				switch (this.calendar.ID)
				{
				case 3:
				case 4:
					return true;
				default:
					return false;
				}
			}
		}

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x0600258B RID: 9611 RVA: 0x00067A72 File Offset: 0x00066A72
		internal bool HasYearMonthAdjustment
		{
			get
			{
				return (this.FormatFlags & DateTimeFormatFlags.UseHebrewRule) != DateTimeFormatFlags.None;
			}
		}

		// Token: 0x0600258C RID: 9612 RVA: 0x00067A84 File Offset: 0x00066A84
		internal bool YearMonthAdjustment(ref int year, ref int month, bool parsedMonthName)
		{
			if ((this.FormatFlags & DateTimeFormatFlags.UseHebrewRule) != DateTimeFormatFlags.None)
			{
				if (year < 1000)
				{
					year += 5000;
				}
				if (year < this.Calendar.GetYear(this.Calendar.MinSupportedDateTime) || year > this.Calendar.GetYear(this.Calendar.MaxSupportedDateTime))
				{
					return false;
				}
				if (parsedMonthName && !this.Calendar.IsLeapYear(year))
				{
					if (month >= 8)
					{
						month--;
					}
					else if (month == 7)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600258D RID: 9613 RVA: 0x00067B0C File Offset: 0x00066B0C
		internal static DateTimeFormatInfo GetJapaneseCalendarDTFI()
		{
			DateTimeFormatInfo dateTimeFormatInfo = DateTimeFormatInfo.m_jajpDTFI;
			if (dateTimeFormatInfo == null)
			{
				dateTimeFormatInfo = new CultureInfo("ja-JP", false).DateTimeFormat;
				dateTimeFormatInfo.Calendar = JapaneseCalendar.GetDefaultInstance();
				DateTimeFormatInfo.m_jajpDTFI = dateTimeFormatInfo;
			}
			return dateTimeFormatInfo;
		}

		// Token: 0x0600258E RID: 9614 RVA: 0x00067B48 File Offset: 0x00066B48
		internal static DateTimeFormatInfo GetTaiwanCalendarDTFI()
		{
			DateTimeFormatInfo dateTimeFormatInfo = DateTimeFormatInfo.m_zhtwDTFI;
			if (dateTimeFormatInfo == null)
			{
				dateTimeFormatInfo = new CultureInfo("zh-TW", false).DateTimeFormat;
				dateTimeFormatInfo.Calendar = TaiwanCalendar.GetDefaultInstance();
				DateTimeFormatInfo.m_zhtwDTFI = dateTimeFormatInfo;
			}
			return dateTimeFormatInfo;
		}

		// Token: 0x0600258F RID: 9615 RVA: 0x00067B81 File Offset: 0x00066B81
		private void ClearTokenHashTable(bool scanDateWords)
		{
			this.m_dtfiTokenHash = null;
			this.m_dateWords = null;
			this.m_scanDateWords = scanDateWords;
			this.formatFlags = DateTimeFormatFlags.NotInitialized;
		}

		// Token: 0x06002590 RID: 9616 RVA: 0x00067BA0 File Offset: 0x00066BA0
		internal TokenHashValue[] CreateTokenHashTable()
		{
			TokenHashValue[] array = this.m_dtfiTokenHash;
			if (array == null)
			{
				array = new TokenHashValue[199];
				this.InsertHash(array, ",", TokenType.IgnorableSymbol, 0);
				this.InsertHash(array, ".", TokenType.IgnorableSymbol, 0);
				this.InsertHash(array, this.TimeSeparator, TokenType.SEP_Time, 0);
				this.InsertHash(array, this.AMDesignator, (TokenType)1027, 0);
				this.InsertHash(array, this.PMDesignator, (TokenType)1284, 1);
				if (this.CultureName.Equals("sq-AL"))
				{
					this.InsertHash(array, "." + this.AMDesignator, (TokenType)1027, 0);
					this.InsertHash(array, "." + this.PMDesignator, (TokenType)1284, 1);
				}
				this.InsertHash(array, "年", TokenType.SEP_YearSuff, 0);
				this.InsertHash(array, "년", TokenType.SEP_YearSuff, 0);
				this.InsertHash(array, "月", TokenType.SEP_MonthSuff, 0);
				this.InsertHash(array, "월", TokenType.SEP_MonthSuff, 0);
				this.InsertHash(array, "日", TokenType.SEP_DaySuff, 0);
				this.InsertHash(array, "일", TokenType.SEP_DaySuff, 0);
				this.InsertHash(array, "時", TokenType.SEP_HourSuff, 0);
				this.InsertHash(array, "时", TokenType.SEP_HourSuff, 0);
				this.InsertHash(array, "分", TokenType.SEP_MinuteSuff, 0);
				this.InsertHash(array, "秒", TokenType.SEP_SecondSuff, 0);
				if (!GregorianCalendarHelper.EnforceLegacyJapaneseDateParsing && this.Calendar.ID == 3)
				{
					this.InsertHash(array, "元", TokenType.YearNumberToken, 1);
					this.InsertHash(array, "(", TokenType.IgnorableSymbol, 0);
					this.InsertHash(array, ")", TokenType.IgnorableSymbol, 0);
				}
				if (this.LanguageName.Equals("ko"))
				{
					this.InsertHash(array, "시", TokenType.SEP_HourSuff, 0);
					this.InsertHash(array, "분", TokenType.SEP_MinuteSuff, 0);
					this.InsertHash(array, "초", TokenType.SEP_SecondSuff, 0);
				}
				if (this.CultureName.Equals("ky-KG"))
				{
					this.InsertHash(array, "-", TokenType.IgnorableSymbol, 0);
				}
				else
				{
					this.InsertHash(array, "-", TokenType.SEP_DateOrOffset, 0);
				}
				string[] array2;
				if (!this.m_scanDateWords)
				{
					array2 = this.ClonedAllLongDatePatterns;
				}
				if (this.m_scanDateWords || this.m_cultureTableRecord.IsSynthetic)
				{
					DateTimeFormatInfoScanner dateTimeFormatInfoScanner = new DateTimeFormatInfoScanner();
					array2 = (this.m_dateWords = dateTimeFormatInfoScanner.GetDateWordsOfDTFI(this));
					DateTimeFormatFlags dateTimeFormatFlags = this.FormatFlags;
					this.m_scanDateWords = false;
				}
				else
				{
					array2 = this.DateWords;
				}
				bool flag = false;
				if (array2 != null)
				{
					for (int i = 0; i < array2.Length; i++)
					{
						switch (array2[i][0])
						{
						case '':
						{
							string monthPostfix = array2[i].Substring(1);
							this.AddMonthNames(array, monthPostfix);
							break;
						}
						case '':
						{
							string text = array2[i].Substring(1);
							this.InsertHash(array, text, TokenType.IgnorableSymbol, 0);
							if (this.DateSeparator.Trim(null).Equals(text))
							{
								flag = true;
							}
							break;
						}
						default:
							this.InsertHash(array, array2[i], TokenType.DateWordToken, 0);
							if (this.CultureName.Equals("eu-ES"))
							{
								this.InsertHash(array, "." + array2[i], TokenType.DateWordToken, 0);
							}
							break;
						}
					}
				}
				if (!flag)
				{
					this.InsertHash(array, this.DateSeparator, TokenType.SEP_Date, 0);
				}
				this.AddMonthNames(array, null);
				for (int j = 1; j <= 13; j++)
				{
					this.InsertHash(array, this.GetAbbreviatedMonthName(j), TokenType.MonthToken, j);
				}
				if (this.CultureName.Equals("gl-ES"))
				{
					for (int k = 1; k <= 13; k++)
					{
						string monthName = this.GetMonthName(k);
						if (monthName.Length > 0)
						{
							this.InsertHash(array, monthName + "de", TokenType.MonthToken, k);
						}
					}
				}
				if ((this.FormatFlags & DateTimeFormatFlags.UseGenitiveMonth) != DateTimeFormatFlags.None)
				{
					for (int l = 1; l <= 13; l++)
					{
						string str = this.internalGetMonthName(l, MonthNameStyles.Genitive, false);
						this.InsertHash(array, str, TokenType.MonthToken, l);
					}
				}
				if ((this.FormatFlags & DateTimeFormatFlags.UseLeapYearMonth) != DateTimeFormatFlags.None)
				{
					for (int m = 1; m <= 13; m++)
					{
						string str2 = this.internalGetMonthName(m, MonthNameStyles.LeapYear, false);
						this.InsertHash(array, str2, TokenType.MonthToken, m);
					}
				}
				for (int n = 0; n < 7; n++)
				{
					string str3 = this.GetDayName((DayOfWeek)n);
					this.InsertHash(array, str3, TokenType.DayOfWeekToken, n);
					str3 = this.GetAbbreviatedDayName((DayOfWeek)n);
					this.InsertHash(array, str3, TokenType.DayOfWeekToken, n);
				}
				int[] eras = this.calendar.Eras;
				for (int num = 1; num <= eras.Length; num++)
				{
					this.InsertHash(array, this.GetEraName(num), TokenType.EraToken, num);
					this.InsertHash(array, this.GetAbbreviatedEraName(num), TokenType.EraToken, num);
				}
				if (this.LanguageName.Equals("ja"))
				{
					for (int num2 = 0; num2 < 7; num2++)
					{
						string str4 = "(" + this.GetAbbreviatedDayName((DayOfWeek)num2) + ")";
						this.InsertHash(array, str4, TokenType.DayOfWeekToken, num2);
					}
					if (this.Calendar.GetType() != typeof(JapaneseCalendar))
					{
						DateTimeFormatInfo japaneseCalendarDTFI = DateTimeFormatInfo.GetJapaneseCalendarDTFI();
						for (int num3 = 1; num3 <= japaneseCalendarDTFI.Calendar.Eras.Length; num3++)
						{
							this.InsertHash(array, japaneseCalendarDTFI.GetEraName(num3), TokenType.JapaneseEraToken, num3);
							this.InsertHash(array, japaneseCalendarDTFI.GetAbbreviatedEraName(num3), TokenType.JapaneseEraToken, num3);
							this.InsertHash(array, japaneseCalendarDTFI.AbbreviatedEnglishEraNames[num3 - 1], TokenType.JapaneseEraToken, num3);
						}
					}
				}
				else if (this.CultureName.Equals("zh-TW"))
				{
					DateTimeFormatInfo taiwanCalendarDTFI = DateTimeFormatInfo.GetTaiwanCalendarDTFI();
					for (int num4 = 1; num4 <= taiwanCalendarDTFI.Calendar.Eras.Length; num4++)
					{
						if (taiwanCalendarDTFI.GetEraName(num4).Length > 0)
						{
							this.InsertHash(array, taiwanCalendarDTFI.GetEraName(num4), TokenType.TEraToken, num4);
						}
					}
				}
				this.InsertHash(array, DateTimeFormatInfo.InvariantInfo.AMDesignator, (TokenType)1027, 0);
				this.InsertHash(array, DateTimeFormatInfo.InvariantInfo.PMDesignator, (TokenType)1284, 1);
				for (int num5 = 1; num5 <= 12; num5++)
				{
					string str5 = DateTimeFormatInfo.InvariantInfo.GetMonthName(num5);
					this.InsertHash(array, str5, TokenType.MonthToken, num5);
					str5 = DateTimeFormatInfo.InvariantInfo.GetAbbreviatedMonthName(num5);
					this.InsertHash(array, str5, TokenType.MonthToken, num5);
				}
				for (int num6 = 0; num6 < 7; num6++)
				{
					string str6 = DateTimeFormatInfo.InvariantInfo.GetDayName((DayOfWeek)num6);
					this.InsertHash(array, str6, TokenType.DayOfWeekToken, num6);
					str6 = DateTimeFormatInfo.InvariantInfo.GetAbbreviatedDayName((DayOfWeek)num6);
					this.InsertHash(array, str6, TokenType.DayOfWeekToken, num6);
				}
				for (int num7 = 0; num7 < this.AbbreviatedEnglishEraNames.Length; num7++)
				{
					this.InsertHash(array, this.AbbreviatedEnglishEraNames[num7], TokenType.EraToken, num7 + 1);
				}
				this.InsertHash(array, "T", TokenType.SEP_LocalTimeMark, 0);
				this.InsertHash(array, "GMT", TokenType.TimeZoneToken, 0);
				this.InsertHash(array, "Z", TokenType.TimeZoneToken, 0);
				this.InsertHash(array, "/", TokenType.SEP_Date, 0);
				this.InsertHash(array, ":", TokenType.SEP_Time, 0);
				this.m_dtfiTokenHash = array;
			}
			return array;
		}

		// Token: 0x06002591 RID: 9617 RVA: 0x000682CC File Offset: 0x000672CC
		private void AddMonthNames(TokenHashValue[] temp, string monthPostfix)
		{
			for (int i = 1; i <= 13; i++)
			{
				string text = this.GetMonthName(i);
				if (text.Length > 0)
				{
					if (monthPostfix != null)
					{
						this.InsertHash(temp, text + monthPostfix, TokenType.MonthToken, i);
					}
					else
					{
						this.InsertHash(temp, text, TokenType.MonthToken, i);
					}
				}
				text = this.GetAbbreviatedMonthName(i);
				this.InsertHash(temp, text, TokenType.MonthToken, i);
			}
		}

		// Token: 0x06002592 RID: 9618 RVA: 0x00068328 File Offset: 0x00067328
		private static bool TryParseHebrewNumber(ref __DTString str, out bool badFormat, out int number)
		{
			number = -1;
			badFormat = false;
			int index = str.Index;
			if (!HebrewNumber.IsDigit(str.Value[index]))
			{
				return false;
			}
			HebrewNumberParsingContext hebrewNumberParsingContext = new HebrewNumberParsingContext(0);
			HebrewNumberParsingState hebrewNumberParsingState;
			for (;;)
			{
				hebrewNumberParsingState = HebrewNumber.ParseByChar(str.Value[index++], ref hebrewNumberParsingContext);
				switch (hebrewNumberParsingState)
				{
				case HebrewNumberParsingState.InvalidHebrewNumber:
				case HebrewNumberParsingState.NotHebrewDigit:
					return false;
				default:
					if (index >= str.Value.Length || hebrewNumberParsingState == HebrewNumberParsingState.FoundEndOfHebrewNumber)
					{
						goto IL_6D;
					}
					break;
				}
			}
			return false;
			IL_6D:
			if (hebrewNumberParsingState != HebrewNumberParsingState.FoundEndOfHebrewNumber)
			{
				return false;
			}
			str.Advance(index - str.Index);
			number = hebrewNumberParsingContext.result;
			return true;
		}

		// Token: 0x06002593 RID: 9619 RVA: 0x000683C1 File Offset: 0x000673C1
		private static bool IsHebrewChar(char ch)
		{
			return ch >= '֐' && ch <= '׿';
		}

		// Token: 0x06002594 RID: 9620 RVA: 0x000683D8 File Offset: 0x000673D8
		private bool IsAllowedJapaneseTokenFollowedByNonSpaceLetter(string tokenString, char nextCh)
		{
			return !GregorianCalendarHelper.EnforceLegacyJapaneseDateParsing && this.Calendar.ID == 3 && (nextCh == "元"[0] || (tokenString == "元" && nextCh == "年"[0]));
		}

		// Token: 0x06002595 RID: 9621 RVA: 0x00068428 File Offset: 0x00067428
		internal bool Tokenize(TokenType TokenMask, out TokenType tokenType, out int tokenValue, ref __DTString str)
		{
			tokenType = TokenType.UnknownToken;
			tokenValue = 0;
			char c = str.m_current;
			bool flag = char.IsLetter(c);
			if (flag)
			{
				c = char.ToLower(c, CultureInfo.CurrentCulture);
				bool flag2;
				if (DateTimeFormatInfo.IsHebrewChar(c) && TokenMask == TokenType.RegularTokenMask && DateTimeFormatInfo.TryParseHebrewNumber(ref str, out flag2, out tokenValue))
				{
					if (flag2)
					{
						tokenType = TokenType.UnknownToken;
						return false;
					}
					tokenType = TokenType.HebrewNumber;
					return true;
				}
			}
			int num = (int)(c % 'Ç');
			int num2 = (int)('\u0001' + c % 'Å');
			int num3 = str.len - str.Index;
			int num4 = 0;
			TokenHashValue[] array = this.m_dtfiTokenHash;
			if (array == null)
			{
				array = this.CreateTokenHashTable();
			}
			TokenHashValue tokenHashValue;
			int count;
			int count2;
			for (;;)
			{
				tokenHashValue = array[num];
				if (tokenHashValue == null)
				{
					return false;
				}
				if ((tokenHashValue.tokenType & TokenMask) > (TokenType)0 && tokenHashValue.tokenString.Length <= num3)
				{
					if (string.Compare(str.Value, str.Index, tokenHashValue.tokenString, 0, tokenHashValue.tokenString.Length, true, CultureInfo.CurrentCulture) == 0)
					{
						break;
					}
					if (tokenHashValue.tokenType == TokenType.MonthToken && this.HasSpacesInMonthNames)
					{
						count = 0;
						if (str.MatchSpecifiedWords(tokenHashValue.tokenString, true, ref count))
						{
							goto Block_17;
						}
					}
					else if (tokenHashValue.tokenType == TokenType.DayOfWeekToken && this.HasSpacesInDayNames)
					{
						count2 = 0;
						if (str.MatchSpecifiedWords(tokenHashValue.tokenString, true, ref count2))
						{
							goto Block_20;
						}
					}
				}
				num4++;
				num += num2;
				if (num >= 199)
				{
					num -= 199;
				}
				if (num4 >= 199)
				{
					return false;
				}
			}
			int index;
			if (flag && (index = str.Index + tokenHashValue.tokenString.Length) < str.len)
			{
				char c2 = str.Value[index];
				if (char.IsLetter(c2) && !this.IsAllowedJapaneseTokenFollowedByNonSpaceLetter(tokenHashValue.tokenString, c2))
				{
					return false;
				}
			}
			tokenType = (tokenHashValue.tokenType & TokenMask);
			tokenValue = tokenHashValue.tokenValue;
			str.Advance(tokenHashValue.tokenString.Length);
			return true;
			Block_17:
			tokenType = (tokenHashValue.tokenType & TokenMask);
			tokenValue = tokenHashValue.tokenValue;
			str.Advance(count);
			return true;
			Block_20:
			tokenType = (tokenHashValue.tokenType & TokenMask);
			tokenValue = tokenHashValue.tokenValue;
			str.Advance(count2);
			return true;
		}

		// Token: 0x06002596 RID: 9622 RVA: 0x00068644 File Offset: 0x00067644
		private void InsertAtCurrentHashNode(TokenHashValue[] hashTable, string str, char ch, TokenType tokenType, int tokenValue, int pos, int hashcode, int hashProbe)
		{
			TokenHashValue tokenHashValue = hashTable[hashcode];
			hashTable[hashcode] = new TokenHashValue(str, tokenType, tokenValue);
			while (++pos < 199)
			{
				hashcode += hashProbe;
				if (hashcode >= 199)
				{
					hashcode -= 199;
				}
				TokenHashValue tokenHashValue2 = hashTable[hashcode];
				if (tokenHashValue2 == null || char.ToLower(tokenHashValue2.tokenString[0], CultureInfo.CurrentCulture) == ch)
				{
					hashTable[hashcode] = tokenHashValue;
					if (tokenHashValue2 == null)
					{
						return;
					}
					tokenHashValue = tokenHashValue2;
				}
			}
		}

		// Token: 0x06002597 RID: 9623 RVA: 0x000686BC File Offset: 0x000676BC
		private void InsertHash(TokenHashValue[] hashTable, string str, TokenType tokenType, int tokenValue)
		{
			if (str == null || str.Length == 0)
			{
				return;
			}
			int num = 0;
			if (char.IsWhiteSpace(str[0]) || char.IsWhiteSpace(str[str.Length - 1]))
			{
				str = str.Trim(null);
				if (str.Length == 0)
				{
					return;
				}
			}
			char c = char.ToLower(str[0], CultureInfo.CurrentCulture);
			int num2 = (int)(c % 'Ç');
			int num3 = (int)('\u0001' + c % 'Å');
			for (;;)
			{
				TokenHashValue tokenHashValue = hashTable[num2];
				if (tokenHashValue == null)
				{
					break;
				}
				if (str.Length >= tokenHashValue.tokenString.Length && string.Compare(str, 0, tokenHashValue.tokenString, 0, tokenHashValue.tokenString.Length, true, CultureInfo.CurrentCulture) == 0)
				{
					if (str.Length > tokenHashValue.tokenString.Length)
					{
						goto Block_7;
					}
					if ((tokenType & TokenType.SeparatorTokenMask) != (tokenHashValue.tokenType & TokenType.SeparatorTokenMask))
					{
						tokenHashValue.tokenType |= tokenType;
						if (tokenValue != 0)
						{
							tokenHashValue.tokenValue = tokenValue;
						}
					}
				}
				num++;
				num2 += num3;
				if (num2 >= 199)
				{
					num2 -= 199;
				}
				if (num >= 199)
				{
					return;
				}
			}
			hashTable[num2] = new TokenHashValue(str, tokenType, tokenValue);
			return;
			Block_7:
			this.InsertAtCurrentHashNode(hashTable, str, c, tokenType, tokenValue, num, num2, num3);
		}

		// Token: 0x06002598 RID: 9624 RVA: 0x000687F0 File Offset: 0x000677F0
		internal static string GetCalendarInfo(int culture, int calendar, int calType)
		{
			int calendarInfo = Win32Native.GetCalendarInfo(culture, calendar, calType, null, 0, IntPtr.Zero);
			if (calendarInfo > 0)
			{
				StringBuilder stringBuilder = new StringBuilder(calendarInfo);
				calendarInfo = Win32Native.GetCalendarInfo(culture, calendar, calType, stringBuilder, calendarInfo, IntPtr.Zero);
				if (calendarInfo > 0)
				{
					return stringBuilder.ToString(0, calendarInfo - 1);
				}
			}
			return null;
		}

		// Token: 0x0400107C RID: 4220
		internal const string rfc1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";

		// Token: 0x0400107D RID: 4221
		internal const string sortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";

		// Token: 0x0400107E RID: 4222
		internal const string universalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";

		// Token: 0x0400107F RID: 4223
		private const int DEFAULT_ALL_DATETIMES_SIZE = 132;

		// Token: 0x04001080 RID: 4224
		internal const DateTimeStyles InvalidDateTimeStyles = ~(DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite | DateTimeStyles.AllowInnerWhite | DateTimeStyles.NoCurrentDateDefault | DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeLocal | DateTimeStyles.AssumeUniversal | DateTimeStyles.RoundtripKind);

		// Token: 0x04001081 RID: 4225
		private const int TOKEN_HASH_SIZE = 199;

		// Token: 0x04001082 RID: 4226
		private const int SECOND_PRIME = 197;

		// Token: 0x04001083 RID: 4227
		private const string dateSeparatorOrTimeZoneOffset = "-";

		// Token: 0x04001084 RID: 4228
		private const string invariantDateSeparator = "/";

		// Token: 0x04001085 RID: 4229
		private const string invariantTimeSeparator = ":";

		// Token: 0x04001086 RID: 4230
		internal const string CJKYearSuff = "年";

		// Token: 0x04001087 RID: 4231
		internal const string CJKMonthSuff = "月";

		// Token: 0x04001088 RID: 4232
		internal const string CJKDaySuff = "日";

		// Token: 0x04001089 RID: 4233
		internal const string KoreanYearSuff = "년";

		// Token: 0x0400108A RID: 4234
		internal const string KoreanMonthSuff = "월";

		// Token: 0x0400108B RID: 4235
		internal const string KoreanDaySuff = "일";

		// Token: 0x0400108C RID: 4236
		internal const string KoreanHourSuff = "시";

		// Token: 0x0400108D RID: 4237
		internal const string KoreanMinuteSuff = "분";

		// Token: 0x0400108E RID: 4238
		internal const string KoreanSecondSuff = "초";

		// Token: 0x0400108F RID: 4239
		internal const string CJKHourSuff = "時";

		// Token: 0x04001090 RID: 4240
		internal const string ChineseHourSuff = "时";

		// Token: 0x04001091 RID: 4241
		internal const string CJKMinuteSuff = "分";

		// Token: 0x04001092 RID: 4242
		internal const string CJKSecondSuff = "秒";

		// Token: 0x04001093 RID: 4243
		internal const string JapaneseEraStart = "元";

		// Token: 0x04001094 RID: 4244
		internal const string LocalTimeMark = "T";

		// Token: 0x04001095 RID: 4245
		internal const string KoreanLangName = "ko";

		// Token: 0x04001096 RID: 4246
		internal const string JapaneseLangName = "ja";

		// Token: 0x04001097 RID: 4247
		internal const string EnglishLangName = "en";

		// Token: 0x04001098 RID: 4248
		internal const int CAL_SCALNAME = 2;

		// Token: 0x04001099 RID: 4249
		private static DateTimeFormatInfo invariantInfo;

		// Token: 0x0400109A RID: 4250
		[NonSerialized]
		internal CultureTableRecord m_cultureTableRecord;

		// Token: 0x0400109B RID: 4251
		[OptionalField(VersionAdded = 2)]
		internal string m_name;

		// Token: 0x0400109C RID: 4252
		[NonSerialized]
		internal string m_langName;

		// Token: 0x0400109D RID: 4253
		[NonSerialized]
		internal CompareInfo m_compareInfo;

		// Token: 0x0400109E RID: 4254
		internal bool m_isDefaultCalendar;

		// Token: 0x0400109F RID: 4255
		internal bool bUseCalendarInfo;

		// Token: 0x040010A0 RID: 4256
		internal string amDesignator;

		// Token: 0x040010A1 RID: 4257
		internal string pmDesignator;

		// Token: 0x040010A2 RID: 4258
		internal string dateSeparator;

		// Token: 0x040010A3 RID: 4259
		internal string longTimePattern;

		// Token: 0x040010A4 RID: 4260
		internal string shortTimePattern;

		// Token: 0x040010A5 RID: 4261
		internal string generalShortTimePattern;

		// Token: 0x040010A6 RID: 4262
		internal string generalLongTimePattern;

		// Token: 0x040010A7 RID: 4263
		internal string timeSeparator;

		// Token: 0x040010A8 RID: 4264
		internal string monthDayPattern;

		// Token: 0x040010A9 RID: 4265
		[OptionalField(VersionAdded = 3)]
		internal string dateTimeOffsetPattern;

		// Token: 0x040010AA RID: 4266
		internal string[] allShortTimePatterns;

		// Token: 0x040010AB RID: 4267
		internal string[] allLongTimePatterns;

		// Token: 0x040010AC RID: 4268
		internal Calendar calendar;

		// Token: 0x040010AD RID: 4269
		internal int firstDayOfWeek = -1;

		// Token: 0x040010AE RID: 4270
		internal int calendarWeekRule = -1;

		// Token: 0x040010AF RID: 4271
		internal string fullDateTimePattern;

		// Token: 0x040010B0 RID: 4272
		internal string longDatePattern;

		// Token: 0x040010B1 RID: 4273
		internal string shortDatePattern;

		// Token: 0x040010B2 RID: 4274
		internal string yearMonthPattern;

		// Token: 0x040010B3 RID: 4275
		internal string[] abbreviatedDayNames;

		// Token: 0x040010B4 RID: 4276
		[OptionalField(VersionAdded = 2)]
		internal string[] m_superShortDayNames;

		// Token: 0x040010B5 RID: 4277
		internal string[] dayNames;

		// Token: 0x040010B6 RID: 4278
		internal string[] abbreviatedMonthNames;

		// Token: 0x040010B7 RID: 4279
		internal string[] monthNames;

		// Token: 0x040010B8 RID: 4280
		[OptionalField(VersionAdded = 2)]
		internal string[] genitiveMonthNames;

		// Token: 0x040010B9 RID: 4281
		[OptionalField(VersionAdded = 2)]
		internal string[] m_genitiveAbbreviatedMonthNames;

		// Token: 0x040010BA RID: 4282
		[OptionalField(VersionAdded = 2)]
		internal string[] leapYearMonthNames;

		// Token: 0x040010BB RID: 4283
		[NonSerialized]
		internal string[] allYearMonthPatterns;

		// Token: 0x040010BC RID: 4284
		internal string[] allShortDatePatterns;

		// Token: 0x040010BD RID: 4285
		internal string[] allLongDatePatterns;

		// Token: 0x040010BE RID: 4286
		internal string[] m_eraNames;

		// Token: 0x040010BF RID: 4287
		internal string[] m_abbrevEraNames;

		// Token: 0x040010C0 RID: 4288
		internal string[] m_abbrevEnglishEraNames;

		// Token: 0x040010C1 RID: 4289
		internal string[] m_dateWords;

		// Token: 0x040010C2 RID: 4290
		internal int[] optionalCalendars;

		// Token: 0x040010C3 RID: 4291
		internal bool m_isReadOnly;

		// Token: 0x040010C4 RID: 4292
		[OptionalField(VersionAdded = 2)]
		internal DateTimeFormatFlags formatFlags = DateTimeFormatFlags.NotInitialized;

		// Token: 0x040010C5 RID: 4293
		private static Hashtable m_calendarNativeNames;

		// Token: 0x040010C6 RID: 4294
		private static object s_InternalSyncObject;

		// Token: 0x040010C7 RID: 4295
		private int CultureID;

		// Token: 0x040010C8 RID: 4296
		private bool m_useUserOverride;

		// Token: 0x040010C9 RID: 4297
		private int nDataItem;

		// Token: 0x040010CA RID: 4298
		private static char[] MonthSpaces = new char[]
		{
			' ',
			'\u00a0'
		};

		// Token: 0x040010CB RID: 4299
		[NonSerialized]
		private TokenHashValue[] m_dtfiTokenHash;

		// Token: 0x040010CC RID: 4300
		[NonSerialized]
		private bool m_scanDateWords;

		// Token: 0x040010CD RID: 4301
		private static DateTimeFormatInfo m_jajpDTFI = null;

		// Token: 0x040010CE RID: 4302
		private static DateTimeFormatInfo m_zhtwDTFI = null;
	}
}
