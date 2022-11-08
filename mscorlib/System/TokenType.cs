using System;

namespace System
{
	// Token: 0x020003A5 RID: 933
	internal enum TokenType
	{
		// Token: 0x04001044 RID: 4164
		NumberToken = 1,
		// Token: 0x04001045 RID: 4165
		YearNumberToken,
		// Token: 0x04001046 RID: 4166
		Am,
		// Token: 0x04001047 RID: 4167
		Pm,
		// Token: 0x04001048 RID: 4168
		MonthToken,
		// Token: 0x04001049 RID: 4169
		EndOfString,
		// Token: 0x0400104A RID: 4170
		DayOfWeekToken,
		// Token: 0x0400104B RID: 4171
		TimeZoneToken,
		// Token: 0x0400104C RID: 4172
		EraToken,
		// Token: 0x0400104D RID: 4173
		DateWordToken,
		// Token: 0x0400104E RID: 4174
		UnknownToken,
		// Token: 0x0400104F RID: 4175
		HebrewNumber,
		// Token: 0x04001050 RID: 4176
		JapaneseEraToken,
		// Token: 0x04001051 RID: 4177
		TEraToken,
		// Token: 0x04001052 RID: 4178
		IgnorableSymbol,
		// Token: 0x04001053 RID: 4179
		SEP_Unk = 256,
		// Token: 0x04001054 RID: 4180
		SEP_End = 512,
		// Token: 0x04001055 RID: 4181
		SEP_Space = 768,
		// Token: 0x04001056 RID: 4182
		SEP_Am = 1024,
		// Token: 0x04001057 RID: 4183
		SEP_Pm = 1280,
		// Token: 0x04001058 RID: 4184
		SEP_Date = 1536,
		// Token: 0x04001059 RID: 4185
		SEP_Time = 1792,
		// Token: 0x0400105A RID: 4186
		SEP_YearSuff = 2048,
		// Token: 0x0400105B RID: 4187
		SEP_MonthSuff = 2304,
		// Token: 0x0400105C RID: 4188
		SEP_DaySuff = 2560,
		// Token: 0x0400105D RID: 4189
		SEP_HourSuff = 2816,
		// Token: 0x0400105E RID: 4190
		SEP_MinuteSuff = 3072,
		// Token: 0x0400105F RID: 4191
		SEP_SecondSuff = 3328,
		// Token: 0x04001060 RID: 4192
		SEP_LocalTimeMark = 3584,
		// Token: 0x04001061 RID: 4193
		SEP_DateOrOffset = 3840,
		// Token: 0x04001062 RID: 4194
		RegularTokenMask = 255,
		// Token: 0x04001063 RID: 4195
		SeparatorTokenMask = 65280
	}
}
