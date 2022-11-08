using System;
using System.Runtime.CompilerServices;

namespace System.Globalization
{
	// Token: 0x0200038A RID: 906
	internal class CalendarTable : BaseInfoTable
	{
		// Token: 0x0600237E RID: 9086 RVA: 0x00059C0B File Offset: 0x00058C0B
		internal CalendarTable(string fileName, bool fromAssembly) : base(fileName, fromAssembly)
		{
		}

		// Token: 0x0600237F RID: 9087 RVA: 0x00059C18 File Offset: 0x00058C18
		internal unsafe override void SetDataItemPointers()
		{
			this.m_itemSize = (uint)this.m_pCultureHeader->sizeCalendarItem;
			this.m_numItem = (uint)this.m_pCultureHeader->numCalendarItems;
			this.m_pDataPool = (ushort*)(this.m_pDataFileStart + this.m_pCultureHeader->offsetToDataPool);
			this.m_pItemData = this.m_pDataFileStart + this.m_pCultureHeader->offsetToCalendarItemData - this.m_itemSize;
			this.m_calendars = (CalendarTableData*)(this.m_pDataFileStart + this.m_pCultureHeader->offsetToCalendarItemData - sizeof(CalendarTableData));
		}

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x06002380 RID: 9088 RVA: 0x00059C9D File Offset: 0x00058C9D
		internal static CalendarTable Default
		{
			get
			{
				return CalendarTable.m_defaultInstance;
			}
		}

		// Token: 0x06002381 RID: 9089 RVA: 0x00059CA4 File Offset: 0x00058CA4
		internal unsafe int ICURRENTERA(int id)
		{
			if (JapaneseCalendarTable.IsJapaneseCalendar(id))
			{
				return JapaneseCalendarTable.CurrentEra(id);
			}
			return (int)this.m_calendars[id].iCurrentEra;
		}

		// Token: 0x06002382 RID: 9090 RVA: 0x00059CCA File Offset: 0x00058CCA
		internal unsafe int IFORMATFLAGS(int id)
		{
			return (int)this.m_calendars[id].iFormatFlags;
		}

		// Token: 0x06002383 RID: 9091 RVA: 0x00059CE1 File Offset: 0x00058CE1
		internal unsafe string[] SDAYNAMES(int id)
		{
			return base.GetStringArray(this.m_calendars[id].saDayNames);
		}

		// Token: 0x06002384 RID: 9092 RVA: 0x00059CFE File Offset: 0x00058CFE
		internal unsafe string[] SABBREVDAYNAMES(int id)
		{
			return base.GetStringArray(this.m_calendars[id].saAbbrevDayNames);
		}

		// Token: 0x06002385 RID: 9093 RVA: 0x00059D1B File Offset: 0x00058D1B
		internal unsafe string[] SSUPERSHORTDAYNAMES(int id)
		{
			return base.GetStringArray(this.m_calendars[id].saSuperShortDayNames);
		}

		// Token: 0x06002386 RID: 9094 RVA: 0x00059D38 File Offset: 0x00058D38
		internal unsafe string[] SMONTHNAMES(int id)
		{
			return base.GetStringArray(this.m_calendars[id].saMonthNames);
		}

		// Token: 0x06002387 RID: 9095 RVA: 0x00059D55 File Offset: 0x00058D55
		internal unsafe string[] SABBREVMONTHNAMES(int id)
		{
			return base.GetStringArray(this.m_calendars[id].saAbbrevMonthNames);
		}

		// Token: 0x06002388 RID: 9096 RVA: 0x00059D72 File Offset: 0x00058D72
		internal unsafe string[] SLEAPYEARMONTHNAMES(int id)
		{
			return base.GetStringArray(this.m_calendars[id].saLeapYearMonthNames);
		}

		// Token: 0x06002389 RID: 9097 RVA: 0x00059D8F File Offset: 0x00058D8F
		internal unsafe string[] SSHORTDATE(int id)
		{
			return base.GetStringArray(this.m_calendars[id].saShortDate);
		}

		// Token: 0x0600238A RID: 9098 RVA: 0x00059DAC File Offset: 0x00058DAC
		internal unsafe string[] SLONGDATE(int id)
		{
			return base.GetStringArray(this.m_calendars[id].saLongDate);
		}

		// Token: 0x0600238B RID: 9099 RVA: 0x00059DC9 File Offset: 0x00058DC9
		internal unsafe string[] SYEARMONTH(int id)
		{
			return base.GetStringArray(this.m_calendars[id].saYearMonth);
		}

		// Token: 0x0600238C RID: 9100 RVA: 0x00059DE6 File Offset: 0x00058DE6
		internal unsafe string SMONTHDAY(int id)
		{
			return base.GetStringPoolString(this.m_calendars[id].sMonthDay);
		}

		// Token: 0x0600238D RID: 9101 RVA: 0x00059E03 File Offset: 0x00058E03
		internal unsafe int[][] SERARANGES(int id)
		{
			if (JapaneseCalendarTable.IsJapaneseCalendar(id))
			{
				return JapaneseCalendarTable.EraRanges(id);
			}
			return base.GetWordArrayArray(this.m_calendars[id].waaEraRanges);
		}

		// Token: 0x0600238E RID: 9102 RVA: 0x00059E2F File Offset: 0x00058E2F
		internal unsafe string[] SERANAMES(int id)
		{
			if (JapaneseCalendarTable.IsJapaneseCalendar(id))
			{
				return JapaneseCalendarTable.EraNames(id);
			}
			return base.GetStringArray(this.m_calendars[id].saEraNames);
		}

		// Token: 0x0600238F RID: 9103 RVA: 0x00059E5B File Offset: 0x00058E5B
		internal unsafe string[] SABBREVERANAMES(int id)
		{
			if (JapaneseCalendarTable.IsJapaneseCalendar(id))
			{
				return JapaneseCalendarTable.AbbrevEraNames(id);
			}
			return base.GetStringArray(this.m_calendars[id].saAbbrevEraNames);
		}

		// Token: 0x06002390 RID: 9104 RVA: 0x00059E87 File Offset: 0x00058E87
		internal unsafe string[] SABBREVENGERANAMES(int id)
		{
			if (JapaneseCalendarTable.IsJapaneseCalendar(id))
			{
				return JapaneseCalendarTable.EnglishEraNames(id);
			}
			return base.GetStringArray(this.m_calendars[id].saAbbrevEnglishEraNames);
		}

		// Token: 0x06002391 RID: 9105
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string nativeGetEraName(int culture, int calID);

		// Token: 0x04000F19 RID: 3865
		private static CalendarTable m_defaultInstance = new CalendarTable("culture.nlp", true);

		// Token: 0x04000F1A RID: 3866
		private unsafe CalendarTableData* m_calendars;
	}
}
