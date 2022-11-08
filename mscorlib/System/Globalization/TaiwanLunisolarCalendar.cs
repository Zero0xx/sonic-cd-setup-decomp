using System;

namespace System.Globalization
{
	// Token: 0x020003C8 RID: 968
	[Serializable]
	public class TaiwanLunisolarCalendar : EastAsianLunisolarCalendar
	{
		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x0600275B RID: 10075 RVA: 0x0007598E File Offset: 0x0007498E
		public override DateTime MinSupportedDateTime
		{
			get
			{
				return TaiwanLunisolarCalendar.minDate;
			}
		}

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x0600275C RID: 10076 RVA: 0x00075995 File Offset: 0x00074995
		public override DateTime MaxSupportedDateTime
		{
			get
			{
				return TaiwanLunisolarCalendar.maxDate;
			}
		}

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x0600275D RID: 10077 RVA: 0x0007599C File Offset: 0x0007499C
		internal override int MinCalendarYear
		{
			get
			{
				return 1912;
			}
		}

		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x0600275E RID: 10078 RVA: 0x000759A3 File Offset: 0x000749A3
		internal override int MaxCalendarYear
		{
			get
			{
				return 2050;
			}
		}

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x0600275F RID: 10079 RVA: 0x000759AA File Offset: 0x000749AA
		internal override DateTime MinDate
		{
			get
			{
				return TaiwanLunisolarCalendar.minDate;
			}
		}

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x06002760 RID: 10080 RVA: 0x000759B1 File Offset: 0x000749B1
		internal override DateTime MaxDate
		{
			get
			{
				return TaiwanLunisolarCalendar.maxDate;
			}
		}

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x06002761 RID: 10081 RVA: 0x000759B8 File Offset: 0x000749B8
		internal override EraInfo[] CalEraInfo
		{
			get
			{
				return TaiwanLunisolarCalendar.m_EraInfo;
			}
		}

		// Token: 0x06002762 RID: 10082 RVA: 0x000759C0 File Offset: 0x000749C0
		internal override int GetYearInfo(int LunarYear, int Index)
		{
			if (LunarYear < 1912 || LunarYear > 2050)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					1912,
					2050
				}));
			}
			return TaiwanLunisolarCalendar.yinfo[LunarYear - 1912, Index];
		}

		// Token: 0x06002763 RID: 10083 RVA: 0x00075A30 File Offset: 0x00074A30
		internal override int GetYear(int year, DateTime time)
		{
			return this.helper.GetYear(year, time);
		}

		// Token: 0x06002764 RID: 10084 RVA: 0x00075A3F File Offset: 0x00074A3F
		internal override int GetGregorianYear(int year, int era)
		{
			return this.helper.GetGregorianYear(year, era);
		}

		// Token: 0x06002765 RID: 10085 RVA: 0x00075A4E File Offset: 0x00074A4E
		public TaiwanLunisolarCalendar()
		{
			this.helper = new GregorianCalendarHelper(this, TaiwanLunisolarCalendar.m_EraInfo);
		}

		// Token: 0x06002766 RID: 10086 RVA: 0x00075A67 File Offset: 0x00074A67
		public override int GetEra(DateTime time)
		{
			return this.helper.GetEra(time);
		}

		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x06002767 RID: 10087 RVA: 0x00075A75 File Offset: 0x00074A75
		internal override int BaseCalendarID
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x06002768 RID: 10088 RVA: 0x00075A78 File Offset: 0x00074A78
		internal override int ID
		{
			get
			{
				return 21;
			}
		}

		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x06002769 RID: 10089 RVA: 0x00075A7C File Offset: 0x00074A7C
		public override int[] Eras
		{
			get
			{
				return this.helper.Eras;
			}
		}

		// Token: 0x040011E4 RID: 4580
		internal const int MIN_LUNISOLAR_YEAR = 1912;

		// Token: 0x040011E5 RID: 4581
		internal const int MAX_LUNISOLAR_YEAR = 2050;

		// Token: 0x040011E6 RID: 4582
		internal const int MIN_GREGORIAN_YEAR = 1912;

		// Token: 0x040011E7 RID: 4583
		internal const int MIN_GREGORIAN_MONTH = 2;

		// Token: 0x040011E8 RID: 4584
		internal const int MIN_GREGORIAN_DAY = 18;

		// Token: 0x040011E9 RID: 4585
		internal const int MAX_GREGORIAN_YEAR = 2051;

		// Token: 0x040011EA RID: 4586
		internal const int MAX_GREGORIAN_MONTH = 2;

		// Token: 0x040011EB RID: 4587
		internal const int MAX_GREGORIAN_DAY = 10;

		// Token: 0x040011EC RID: 4588
		internal static EraInfo[] m_EraInfo = GregorianCalendarHelper.InitEraInfo(4);

		// Token: 0x040011ED RID: 4589
		internal GregorianCalendarHelper helper;

		// Token: 0x040011EE RID: 4590
		internal static DateTime minDate = new DateTime(1912, 2, 18);

		// Token: 0x040011EF RID: 4591
		internal static DateTime maxDate = new DateTime(new DateTime(2051, 2, 10, 23, 59, 59, 999).Ticks + 9999L);

		// Token: 0x040011F0 RID: 4592
		private static readonly int[,] yinfo = new int[,]
		{
			{
				0,
				2,
				18,
				42192
			},
			{
				0,
				2,
				6,
				53840
			},
			{
				5,
				1,
				26,
				54568
			},
			{
				0,
				2,
				14,
				46400
			},
			{
				0,
				2,
				3,
				54944
			},
			{
				2,
				1,
				23,
				38608
			},
			{
				0,
				2,
				11,
				38320
			},
			{
				7,
				2,
				1,
				18872
			},
			{
				0,
				2,
				20,
				18800
			},
			{
				0,
				2,
				8,
				42160
			},
			{
				5,
				1,
				28,
				45656
			},
			{
				0,
				2,
				16,
				27216
			},
			{
				0,
				2,
				5,
				27968
			},
			{
				4,
				1,
				24,
				44456
			},
			{
				0,
				2,
				13,
				11104
			},
			{
				0,
				2,
				2,
				38256
			},
			{
				2,
				1,
				23,
				18808
			},
			{
				0,
				2,
				10,
				18800
			},
			{
				6,
				1,
				30,
				25776
			},
			{
				0,
				2,
				17,
				54432
			},
			{
				0,
				2,
				6,
				59984
			},
			{
				5,
				1,
				26,
				27976
			},
			{
				0,
				2,
				14,
				23248
			},
			{
				0,
				2,
				4,
				11104
			},
			{
				3,
				1,
				24,
				37744
			},
			{
				0,
				2,
				11,
				37600
			},
			{
				7,
				1,
				31,
				51560
			},
			{
				0,
				2,
				19,
				51536
			},
			{
				0,
				2,
				8,
				54432
			},
			{
				6,
				1,
				27,
				55888
			},
			{
				0,
				2,
				15,
				46416
			},
			{
				0,
				2,
				5,
				22176
			},
			{
				4,
				1,
				25,
				43736
			},
			{
				0,
				2,
				13,
				9680
			},
			{
				0,
				2,
				2,
				37584
			},
			{
				2,
				1,
				22,
				51544
			},
			{
				0,
				2,
				10,
				43344
			},
			{
				7,
				1,
				29,
				46248
			},
			{
				0,
				2,
				17,
				27808
			},
			{
				0,
				2,
				6,
				46416
			},
			{
				5,
				1,
				27,
				21928
			},
			{
				0,
				2,
				14,
				19872
			},
			{
				0,
				2,
				3,
				42416
			},
			{
				3,
				1,
				24,
				21176
			},
			{
				0,
				2,
				12,
				21168
			},
			{
				8,
				1,
				31,
				43344
			},
			{
				0,
				2,
				18,
				59728
			},
			{
				0,
				2,
				8,
				27296
			},
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
				19296
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
				21088
			},
			{
				3,
				1,
				21,
				59696
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
				23208
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
				38608
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
				19152
			},
			{
				0,
				2,
				3,
				42192
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
				54568
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
				46752
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
				42168
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
				44448
			},
			{
				0,
				2,
				17,
				43872
			},
			{
				0,
				2,
				6,
				38256
			},
			{
				5,
				1,
				27,
				18808
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
				27432
			},
			{
				0,
				2,
				19,
				23232
			},
			{
				0,
				2,
				7,
				43872
			},
			{
				5,
				1,
				28,
				37736
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
				54440
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
				22176
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
				4,
				1,
				23,
				46416
			},
			{
				0,
				2,
				10,
				44368
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
				6,
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
				43312
			},
			{
				4,
				1,
				25,
				29864
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
				19880
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
				42208
			},
			{
				0,
				2,
				6,
				53856
			},
			{
				5,
				1,
				26,
				59696
			},
			{
				0,
				2,
				13,
				54576
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
				27472
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
				19152
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
				53848
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
				46496
			},
			{
				0,
				2,
				1,
				22224
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
				43600
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
				27936
			},
			{
				0,
				2,
				2,
				44448
			},
			{
				3,
				1,
				23,
				21936
			}
		};
	}
}
