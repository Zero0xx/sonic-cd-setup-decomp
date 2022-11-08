﻿using System;

namespace System.Globalization
{
	// Token: 0x020003BF RID: 959
	[Serializable]
	public class JapaneseLunisolarCalendar : EastAsianLunisolarCalendar
	{
		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x06002692 RID: 9874 RVA: 0x0006EBE8 File Offset: 0x0006DBE8
		public override DateTime MinSupportedDateTime
		{
			get
			{
				return JapaneseLunisolarCalendar.minDate;
			}
		}

		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x06002693 RID: 9875 RVA: 0x0006EBEF File Offset: 0x0006DBEF
		public override DateTime MaxSupportedDateTime
		{
			get
			{
				return JapaneseLunisolarCalendar.maxDate;
			}
		}

		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x06002694 RID: 9876 RVA: 0x0006EBF6 File Offset: 0x0006DBF6
		internal override int MinCalendarYear
		{
			get
			{
				return 1960;
			}
		}

		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x06002695 RID: 9877 RVA: 0x0006EBFD File Offset: 0x0006DBFD
		internal override int MaxCalendarYear
		{
			get
			{
				return 2049;
			}
		}

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x06002696 RID: 9878 RVA: 0x0006EC04 File Offset: 0x0006DC04
		internal override DateTime MinDate
		{
			get
			{
				return JapaneseLunisolarCalendar.minDate;
			}
		}

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x06002697 RID: 9879 RVA: 0x0006EC0B File Offset: 0x0006DC0B
		internal override DateTime MaxDate
		{
			get
			{
				return JapaneseLunisolarCalendar.maxDate;
			}
		}

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x06002698 RID: 9880 RVA: 0x0006EC12 File Offset: 0x0006DC12
		internal override EraInfo[] CalEraInfo
		{
			get
			{
				return JapaneseLunisolarCalendar.m_EraInfo;
			}
		}

		// Token: 0x06002699 RID: 9881 RVA: 0x0006EC1C File Offset: 0x0006DC1C
		internal override int GetYearInfo(int LunarYear, int Index)
		{
			if (LunarYear < 1960 || LunarYear > 2049)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					1960,
					2049
				}));
			}
			return JapaneseLunisolarCalendar.yinfo[LunarYear - 1960, Index];
		}

		// Token: 0x0600269A RID: 9882 RVA: 0x0006EC8C File Offset: 0x0006DC8C
		internal override int GetYear(int year, DateTime time)
		{
			return this.helper.GetYear(year, time);
		}

		// Token: 0x0600269B RID: 9883 RVA: 0x0006EC9B File Offset: 0x0006DC9B
		internal override int GetGregorianYear(int year, int era)
		{
			return this.helper.GetGregorianYear(year, era);
		}

		// Token: 0x0600269C RID: 9884 RVA: 0x0006ECAA File Offset: 0x0006DCAA
		public JapaneseLunisolarCalendar()
		{
			this.helper = new GregorianCalendarHelper(this, JapaneseLunisolarCalendar.m_EraInfo);
		}

		// Token: 0x0600269D RID: 9885 RVA: 0x0006ECC3 File Offset: 0x0006DCC3
		public override int GetEra(DateTime time)
		{
			return this.helper.GetEra(time);
		}

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x0600269E RID: 9886 RVA: 0x0006ECD1 File Offset: 0x0006DCD1
		internal override int BaseCalendarID
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x0600269F RID: 9887 RVA: 0x0006ECD4 File Offset: 0x0006DCD4
		internal override int ID
		{
			get
			{
				return 14;
			}
		}

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x060026A0 RID: 9888 RVA: 0x0006ECD8 File Offset: 0x0006DCD8
		public override int[] Eras
		{
			get
			{
				return this.helper.Eras;
			}
		}

		// Token: 0x0400119A RID: 4506
		public const int JapaneseEra = 1;

		// Token: 0x0400119B RID: 4507
		internal const int MIN_LUNISOLAR_YEAR = 1960;

		// Token: 0x0400119C RID: 4508
		internal const int MAX_LUNISOLAR_YEAR = 2049;

		// Token: 0x0400119D RID: 4509
		internal const int MIN_GREGORIAN_YEAR = 1960;

		// Token: 0x0400119E RID: 4510
		internal const int MIN_GREGORIAN_MONTH = 1;

		// Token: 0x0400119F RID: 4511
		internal const int MIN_GREGORIAN_DAY = 28;

		// Token: 0x040011A0 RID: 4512
		internal const int MAX_GREGORIAN_YEAR = 2050;

		// Token: 0x040011A1 RID: 4513
		internal const int MAX_GREGORIAN_MONTH = 1;

		// Token: 0x040011A2 RID: 4514
		internal const int MAX_GREGORIAN_DAY = 22;

		// Token: 0x040011A3 RID: 4515
		internal static EraInfo[] m_EraInfo = GregorianCalendarHelper.InitEraInfo(14);

		// Token: 0x040011A4 RID: 4516
		internal GregorianCalendarHelper helper;

		// Token: 0x040011A5 RID: 4517
		internal static DateTime minDate = new DateTime(1960, 1, 28);

		// Token: 0x040011A6 RID: 4518
		internal static DateTime maxDate = new DateTime(new DateTime(2050, 1, 22, 23, 59, 59, 999).Ticks + 9999L);

		// Token: 0x040011A7 RID: 4519
		private static readonly int[,] yinfo = new int[,]
		{
			{
				6,
				1,
				28,
				44368
			},
			{
				0,
				2,
				15,
				43856
			},
			{
				0,
				2,
				5,
				19808
			},
			{
				4,
				1,
				25,
				42352
			},
			{
				0,
				2,
				13,
				42352
			},
			{
				0,
				2,
				2,
				21104
			},
			{
				3,
				1,
				22,
				26928
			},
			{
				0,
				2,
				9,
				55632
			},
			{
				7,
				1,
				30,
				27304
			},
			{
				0,
				2,
				17,
				22176
			},
			{
				0,
				2,
				6,
				39632
			},
			{
				5,
				1,
				27,
				19176
			},
			{
				0,
				2,
				15,
				19168
			},
			{
				0,
				2,
				3,
				42208
			},
			{
				4,
				1,
				23,
				53864
			},
			{
				0,
				2,
				11,
				53840
			},
			{
				8,
				1,
				31,
				54600
			},
			{
				0,
				2,
				18,
				46400
			},
			{
				0,
				2,
				7,
				54944
			},
			{
				6,
				1,
				28,
				38608
			},
			{
				0,
				2,
				16,
				38320
			},
			{
				0,
				2,
				5,
				18864
			},
			{
				4,
				1,
				25,
				42200
			},
			{
				0,
				2,
				13,
				42160
			},
			{
				10,
				2,
				2,
				45656
			},
			{
				0,
				2,
				20,
				27216
			},
			{
				0,
				2,
				9,
				27968
			},
			{
				6,
				1,
				29,
				46504
			},
			{
				0,
				2,
				18,
				11104
			},
			{
				0,
				2,
				6,
				38320
			},
			{
				5,
				1,
				27,
				18872
			},
			{
				0,
				2,
				15,
				18800
			},
			{
				0,
				2,
				4,
				25776
			},
			{
				3,
				1,
				23,
				27216
			},
			{
				0,
				2,
				10,
				59984
			},
			{
				8,
				1,
				31,
				27976
			},
			{
				0,
				2,
				19,
				23248
			},
			{
				0,
				2,
				8,
				11104
			},
			{
				5,
				1,
				28,
				37744
			},
			{
				0,
				2,
				16,
				37600
			},
			{
				0,
				2,
				5,
				51552
			},
			{
				4,
				1,
				24,
				58536
			},
			{
				0,
				2,
				12,
				54432
			},
			{
				0,
				2,
				1,
				55888
			},
			{
				2,
				1,
				22,
				23208
			},
			{
				0,
				2,
				9,
				22208
			},
			{
				7,
				1,
				29,
				43736
			},
			{
				0,
				2,
				18,
				9680
			},
			{
				0,
				2,
				7,
				37584
			},
			{
				5,
				1,
				26,
				51544
			},
			{
				0,
				2,
				14,
				43344
			},
			{
				0,
				2,
				3,
				46240
			},
			{
				3,
				1,
				23,
				47696
			},
			{
				0,
				2,
				10,
				46416
			},
			{
				9,
				1,
				31,
				21928
			},
			{
				0,
				2,
				19,
				19360
			},
			{
				0,
				2,
				8,
				42416
			},
			{
				5,
				1,
				28,
				21176
			},
			{
				0,
				2,
				16,
				21168
			},
			{
				0,
				2,
				5,
				43344
			},
			{
				4,
				1,
				25,
				46248
			},
			{
				0,
				2,
				12,
				27296
			},
			{
				0,
				2,
				1,
				44368
			},
			{
				2,
				1,
				22,
				21928
			},
			{
				0,
				2,
				10,
				19296
			},
			{
				6,
				1,
				29,
				42352
			},
			{
				0,
				2,
				17,
				42352
			},
			{
				0,
				2,
				7,
				21104
			},
			{
				5,
				1,
				27,
				26928
			},
			{
				0,
				2,
				13,
				55600
			},
			{
				0,
				2,
				3,
				23200
			},
			{
				3,
				1,
				23,
				43856
			},
			{
				0,
				2,
				11,
				38608
			},
			{
				11,
				1,
				31,
				19176
			},
			{
				0,
				2,
				19,
				19168
			},
			{
				0,
				2,
				8,
				42192
			},
			{
				6,
				1,
				28,
				53864
			},
			{
				0,
				2,
				15,
				53840
			},
			{
				0,
				2,
				4,
				54560
			},
			{
				5,
				1,
				24,
				55968
			},
			{
				0,
				2,
				12,
				46752
			},
			{
				0,
				2,
				1,
				38608
			},
			{
				2,
				1,
				22,
				19160
			},
			{
				0,
				2,
				10,
				18864
			},
			{
				7,
				1,
				30,
				42168
			},
			{
				0,
				2,
				17,
				42160
			},
			{
				0,
				2,
				6,
				45648
			},
			{
				5,
				1,
				26,
				46376
			},
			{
				0,
				2,
				14,
				27968
			},
			{
				0,
				2,
				2,
				44448
			}
		};
	}
}
