using System;

namespace System
{
	// Token: 0x020003A2 RID: 930
	[Flags]
	internal enum ParseFlags
	{
		// Token: 0x0400101B RID: 4123
		HaveYear = 1,
		// Token: 0x0400101C RID: 4124
		HaveMonth = 2,
		// Token: 0x0400101D RID: 4125
		HaveDay = 4,
		// Token: 0x0400101E RID: 4126
		HaveHour = 8,
		// Token: 0x0400101F RID: 4127
		HaveMinute = 16,
		// Token: 0x04001020 RID: 4128
		HaveSecond = 32,
		// Token: 0x04001021 RID: 4129
		HaveTime = 64,
		// Token: 0x04001022 RID: 4130
		HaveDate = 128,
		// Token: 0x04001023 RID: 4131
		TimeZoneUsed = 256,
		// Token: 0x04001024 RID: 4132
		TimeZoneUtc = 512,
		// Token: 0x04001025 RID: 4133
		ParsedMonthName = 1024,
		// Token: 0x04001026 RID: 4134
		CaptureOffset = 2048,
		// Token: 0x04001027 RID: 4135
		YearDefault = 4096,
		// Token: 0x04001028 RID: 4136
		Rfc1123Pattern = 8192,
		// Token: 0x04001029 RID: 4137
		UtcSortPattern = 16384
	}
}
