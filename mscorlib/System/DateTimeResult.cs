using System;
using System.Globalization;

namespace System
{
	// Token: 0x020003A3 RID: 931
	internal struct DateTimeResult
	{
		// Token: 0x06002518 RID: 9496 RVA: 0x0006550C File Offset: 0x0006450C
		internal void Init()
		{
			this.Year = -1;
			this.Month = -1;
			this.Day = -1;
			this.fraction = -1.0;
			this.era = -1;
		}

		// Token: 0x06002519 RID: 9497 RVA: 0x00065539 File Offset: 0x00064539
		internal void SetDate(int year, int month, int day)
		{
			this.Year = year;
			this.Month = month;
			this.Day = day;
		}

		// Token: 0x0600251A RID: 9498 RVA: 0x00065550 File Offset: 0x00064550
		internal void SetFailure(ParseFailureKind failure, string failureMessageID, object failureMessageFormatArgument)
		{
			this.failure = failure;
			this.failureMessageID = failureMessageID;
			this.failureMessageFormatArgument = failureMessageFormatArgument;
		}

		// Token: 0x0600251B RID: 9499 RVA: 0x00065567 File Offset: 0x00064567
		internal void SetFailure(ParseFailureKind failure, string failureMessageID, object failureMessageFormatArgument, string failureArgumentName)
		{
			this.failure = failure;
			this.failureMessageID = failureMessageID;
			this.failureMessageFormatArgument = failureMessageFormatArgument;
			this.failureArgumentName = failureArgumentName;
		}

		// Token: 0x0400102A RID: 4138
		internal int Year;

		// Token: 0x0400102B RID: 4139
		internal int Month;

		// Token: 0x0400102C RID: 4140
		internal int Day;

		// Token: 0x0400102D RID: 4141
		internal int Hour;

		// Token: 0x0400102E RID: 4142
		internal int Minute;

		// Token: 0x0400102F RID: 4143
		internal int Second;

		// Token: 0x04001030 RID: 4144
		internal double fraction;

		// Token: 0x04001031 RID: 4145
		internal int era;

		// Token: 0x04001032 RID: 4146
		internal ParseFlags flags;

		// Token: 0x04001033 RID: 4147
		internal TimeSpan timeZoneOffset;

		// Token: 0x04001034 RID: 4148
		internal Calendar calendar;

		// Token: 0x04001035 RID: 4149
		internal DateTime parsedDate;

		// Token: 0x04001036 RID: 4150
		internal ParseFailureKind failure;

		// Token: 0x04001037 RID: 4151
		internal string failureMessageID;

		// Token: 0x04001038 RID: 4152
		internal object failureMessageFormatArgument;

		// Token: 0x04001039 RID: 4153
		internal string failureArgumentName;
	}
}
