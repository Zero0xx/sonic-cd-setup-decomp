using System;
using System.Runtime.InteropServices;
using System.Text;

namespace System
{
	// Token: 0x0200012B RID: 299
	[ComVisible(true)]
	[Serializable]
	public struct TimeSpan : IComparable, IComparable<TimeSpan>, IEquatable<TimeSpan>
	{
		// Token: 0x0600107D RID: 4221 RVA: 0x0002E310 File Offset: 0x0002D310
		public TimeSpan(long ticks)
		{
			this._ticks = ticks;
		}

		// Token: 0x0600107E RID: 4222 RVA: 0x0002E319 File Offset: 0x0002D319
		public TimeSpan(int hours, int minutes, int seconds)
		{
			this._ticks = TimeSpan.TimeToTicks(hours, minutes, seconds);
		}

		// Token: 0x0600107F RID: 4223 RVA: 0x0002E329 File Offset: 0x0002D329
		public TimeSpan(int days, int hours, int minutes, int seconds)
		{
			this = new TimeSpan(days, hours, minutes, seconds, 0);
		}

		// Token: 0x06001080 RID: 4224 RVA: 0x0002E338 File Offset: 0x0002D338
		public TimeSpan(int days, int hours, int minutes, int seconds, int milliseconds)
		{
			long num = ((long)days * 3600L * 24L + (long)hours * 3600L + (long)minutes * 60L + (long)seconds) * 1000L + (long)milliseconds;
			if (num > 922337203685477L || num < -922337203685477L)
			{
				throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("Overflow_TimeSpanTooLong"));
			}
			this._ticks = num * 10000L;
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06001081 RID: 4225 RVA: 0x0002E3AA File Offset: 0x0002D3AA
		public long Ticks
		{
			get
			{
				return this._ticks;
			}
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06001082 RID: 4226 RVA: 0x0002E3B2 File Offset: 0x0002D3B2
		public int Days
		{
			get
			{
				return (int)(this._ticks / 864000000000L);
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06001083 RID: 4227 RVA: 0x0002E3C5 File Offset: 0x0002D3C5
		public int Hours
		{
			get
			{
				return (int)(this._ticks / 36000000000L % 24L);
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06001084 RID: 4228 RVA: 0x0002E3DC File Offset: 0x0002D3DC
		public int Milliseconds
		{
			get
			{
				return (int)(this._ticks / 10000L % 1000L);
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06001085 RID: 4229 RVA: 0x0002E3F3 File Offset: 0x0002D3F3
		public int Minutes
		{
			get
			{
				return (int)(this._ticks / 600000000L % 60L);
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06001086 RID: 4230 RVA: 0x0002E407 File Offset: 0x0002D407
		public int Seconds
		{
			get
			{
				return (int)(this._ticks / 10000000L % 60L);
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06001087 RID: 4231 RVA: 0x0002E41B File Offset: 0x0002D41B
		public double TotalDays
		{
			get
			{
				return (double)this._ticks * 1.1574074074074074E-12;
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06001088 RID: 4232 RVA: 0x0002E42E File Offset: 0x0002D42E
		public double TotalHours
		{
			get
			{
				return (double)this._ticks * 2.7777777777777777E-11;
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06001089 RID: 4233 RVA: 0x0002E444 File Offset: 0x0002D444
		public double TotalMilliseconds
		{
			get
			{
				double num = (double)this._ticks * 0.0001;
				if (num > 922337203685477.0)
				{
					return 922337203685477.0;
				}
				if (num < -922337203685477.0)
				{
					return -922337203685477.0;
				}
				return num;
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x0600108A RID: 4234 RVA: 0x0002E490 File Offset: 0x0002D490
		public double TotalMinutes
		{
			get
			{
				return (double)this._ticks * 1.6666666666666667E-09;
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x0600108B RID: 4235 RVA: 0x0002E4A3 File Offset: 0x0002D4A3
		public double TotalSeconds
		{
			get
			{
				return (double)this._ticks * 1E-07;
			}
		}

		// Token: 0x0600108C RID: 4236 RVA: 0x0002E4B8 File Offset: 0x0002D4B8
		public TimeSpan Add(TimeSpan ts)
		{
			long num = this._ticks + ts._ticks;
			if (this._ticks >> 63 == ts._ticks >> 63 && this._ticks >> 63 != num >> 63)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_TimeSpanTooLong"));
			}
			return new TimeSpan(num);
		}

		// Token: 0x0600108D RID: 4237 RVA: 0x0002E50E File Offset: 0x0002D50E
		public static int Compare(TimeSpan t1, TimeSpan t2)
		{
			if (t1._ticks > t2._ticks)
			{
				return 1;
			}
			if (t1._ticks < t2._ticks)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x0600108E RID: 4238 RVA: 0x0002E538 File Offset: 0x0002D538
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (!(value is TimeSpan))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeTimeSpan"));
			}
			long ticks = ((TimeSpan)value)._ticks;
			if (this._ticks > ticks)
			{
				return 1;
			}
			if (this._ticks < ticks)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x0600108F RID: 4239 RVA: 0x0002E588 File Offset: 0x0002D588
		public int CompareTo(TimeSpan value)
		{
			long ticks = value._ticks;
			if (this._ticks > ticks)
			{
				return 1;
			}
			if (this._ticks < ticks)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x06001090 RID: 4240 RVA: 0x0002E5B4 File Offset: 0x0002D5B4
		public static TimeSpan FromDays(double value)
		{
			return TimeSpan.Interval(value, 86400000);
		}

		// Token: 0x06001091 RID: 4241 RVA: 0x0002E5C4 File Offset: 0x0002D5C4
		public TimeSpan Duration()
		{
			if (this._ticks == TimeSpan.MinValue._ticks)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Duration"));
			}
			return new TimeSpan((this._ticks >= 0L) ? this._ticks : (-this._ticks));
		}

		// Token: 0x06001092 RID: 4242 RVA: 0x0002E611 File Offset: 0x0002D611
		public override bool Equals(object value)
		{
			return value is TimeSpan && this._ticks == ((TimeSpan)value)._ticks;
		}

		// Token: 0x06001093 RID: 4243 RVA: 0x0002E630 File Offset: 0x0002D630
		public bool Equals(TimeSpan obj)
		{
			return this._ticks == obj._ticks;
		}

		// Token: 0x06001094 RID: 4244 RVA: 0x0002E641 File Offset: 0x0002D641
		public static bool Equals(TimeSpan t1, TimeSpan t2)
		{
			return t1._ticks == t2._ticks;
		}

		// Token: 0x06001095 RID: 4245 RVA: 0x0002E653 File Offset: 0x0002D653
		public override int GetHashCode()
		{
			return (int)this._ticks ^ (int)(this._ticks >> 32);
		}

		// Token: 0x06001096 RID: 4246 RVA: 0x0002E667 File Offset: 0x0002D667
		public static TimeSpan FromHours(double value)
		{
			return TimeSpan.Interval(value, 3600000);
		}

		// Token: 0x06001097 RID: 4247 RVA: 0x0002E674 File Offset: 0x0002D674
		private static TimeSpan Interval(double value, int scale)
		{
			if (double.IsNaN(value))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_CannotBeNaN"));
			}
			double num = value * (double)scale;
			double num2 = num + ((value >= 0.0) ? 0.5 : -0.5);
			if (num2 > 922337203685477.0 || num2 < -922337203685477.0)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_TimeSpanTooLong"));
			}
			return new TimeSpan((long)num2 * 10000L);
		}

		// Token: 0x06001098 RID: 4248 RVA: 0x0002E6F7 File Offset: 0x0002D6F7
		public static TimeSpan FromMilliseconds(double value)
		{
			return TimeSpan.Interval(value, 1);
		}

		// Token: 0x06001099 RID: 4249 RVA: 0x0002E700 File Offset: 0x0002D700
		public static TimeSpan FromMinutes(double value)
		{
			return TimeSpan.Interval(value, 60000);
		}

		// Token: 0x0600109A RID: 4250 RVA: 0x0002E70D File Offset: 0x0002D70D
		public TimeSpan Negate()
		{
			if (this._ticks == TimeSpan.MinValue._ticks)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_NegateTwosCompNum"));
			}
			return new TimeSpan(-this._ticks);
		}

		// Token: 0x0600109B RID: 4251 RVA: 0x0002E740 File Offset: 0x0002D740
		public static TimeSpan Parse(string s)
		{
			return new TimeSpan(default(TimeSpan.StringParser).Parse(s));
		}

		// Token: 0x0600109C RID: 4252 RVA: 0x0002E764 File Offset: 0x0002D764
		public static bool TryParse(string s, out TimeSpan result)
		{
			long ticks;
			if (default(TimeSpan.StringParser).TryParse(s, out ticks))
			{
				result = new TimeSpan(ticks);
				return true;
			}
			result = TimeSpan.Zero;
			return false;
		}

		// Token: 0x0600109D RID: 4253 RVA: 0x0002E7A1 File Offset: 0x0002D7A1
		public static TimeSpan FromSeconds(double value)
		{
			return TimeSpan.Interval(value, 1000);
		}

		// Token: 0x0600109E RID: 4254 RVA: 0x0002E7B0 File Offset: 0x0002D7B0
		public TimeSpan Subtract(TimeSpan ts)
		{
			long num = this._ticks - ts._ticks;
			if (this._ticks >> 63 != ts._ticks >> 63 && this._ticks >> 63 != num >> 63)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_TimeSpanTooLong"));
			}
			return new TimeSpan(num);
		}

		// Token: 0x0600109F RID: 4255 RVA: 0x0002E806 File Offset: 0x0002D806
		public static TimeSpan FromTicks(long value)
		{
			return new TimeSpan(value);
		}

		// Token: 0x060010A0 RID: 4256 RVA: 0x0002E810 File Offset: 0x0002D810
		internal static long TimeToTicks(int hour, int minute, int second)
		{
			long num = (long)hour * 3600L + (long)minute * 60L + (long)second;
			if (num > 922337203685L || num < -922337203685L)
			{
				throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("Overflow_TimeSpanTooLong"));
			}
			return num * 10000000L;
		}

		// Token: 0x060010A1 RID: 4257 RVA: 0x0002E862 File Offset: 0x0002D862
		private string IntToString(int n, int digits)
		{
			return ParseNumbers.IntToString(n, 10, digits, '0', 0);
		}

		// Token: 0x060010A2 RID: 4258 RVA: 0x0002E870 File Offset: 0x0002D870
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = (int)(this._ticks / 864000000000L);
			long num2 = this._ticks % 864000000000L;
			if (this._ticks < 0L)
			{
				stringBuilder.Append("-");
				num = -num;
				num2 = -num2;
			}
			if (num != 0)
			{
				stringBuilder.Append(num);
				stringBuilder.Append(".");
			}
			stringBuilder.Append(this.IntToString((int)(num2 / 36000000000L % 24L), 2));
			stringBuilder.Append(":");
			stringBuilder.Append(this.IntToString((int)(num2 / 600000000L % 60L), 2));
			stringBuilder.Append(":");
			stringBuilder.Append(this.IntToString((int)(num2 / 10000000L % 60L), 2));
			int num3 = (int)(num2 % 10000000L);
			if (num3 != 0)
			{
				stringBuilder.Append(".");
				stringBuilder.Append(this.IntToString(num3, 7));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060010A3 RID: 4259 RVA: 0x0002E973 File Offset: 0x0002D973
		public static TimeSpan operator -(TimeSpan t)
		{
			if (t._ticks == TimeSpan.MinValue._ticks)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_NegateTwosCompNum"));
			}
			return new TimeSpan(-t._ticks);
		}

		// Token: 0x060010A4 RID: 4260 RVA: 0x0002E9A5 File Offset: 0x0002D9A5
		public static TimeSpan operator -(TimeSpan t1, TimeSpan t2)
		{
			return t1.Subtract(t2);
		}

		// Token: 0x060010A5 RID: 4261 RVA: 0x0002E9AF File Offset: 0x0002D9AF
		public static TimeSpan operator +(TimeSpan t)
		{
			return t;
		}

		// Token: 0x060010A6 RID: 4262 RVA: 0x0002E9B2 File Offset: 0x0002D9B2
		public static TimeSpan operator +(TimeSpan t1, TimeSpan t2)
		{
			return t1.Add(t2);
		}

		// Token: 0x060010A7 RID: 4263 RVA: 0x0002E9BC File Offset: 0x0002D9BC
		public static bool operator ==(TimeSpan t1, TimeSpan t2)
		{
			return t1._ticks == t2._ticks;
		}

		// Token: 0x060010A8 RID: 4264 RVA: 0x0002E9CE File Offset: 0x0002D9CE
		public static bool operator !=(TimeSpan t1, TimeSpan t2)
		{
			return t1._ticks != t2._ticks;
		}

		// Token: 0x060010A9 RID: 4265 RVA: 0x0002E9E3 File Offset: 0x0002D9E3
		public static bool operator <(TimeSpan t1, TimeSpan t2)
		{
			return t1._ticks < t2._ticks;
		}

		// Token: 0x060010AA RID: 4266 RVA: 0x0002E9F5 File Offset: 0x0002D9F5
		public static bool operator <=(TimeSpan t1, TimeSpan t2)
		{
			return t1._ticks <= t2._ticks;
		}

		// Token: 0x060010AB RID: 4267 RVA: 0x0002EA0A File Offset: 0x0002DA0A
		public static bool operator >(TimeSpan t1, TimeSpan t2)
		{
			return t1._ticks > t2._ticks;
		}

		// Token: 0x060010AC RID: 4268 RVA: 0x0002EA1C File Offset: 0x0002DA1C
		public static bool operator >=(TimeSpan t1, TimeSpan t2)
		{
			return t1._ticks >= t2._ticks;
		}

		// Token: 0x04000584 RID: 1412
		public const long TicksPerMillisecond = 10000L;

		// Token: 0x04000585 RID: 1413
		private const double MillisecondsPerTick = 0.0001;

		// Token: 0x04000586 RID: 1414
		public const long TicksPerSecond = 10000000L;

		// Token: 0x04000587 RID: 1415
		private const double SecondsPerTick = 1E-07;

		// Token: 0x04000588 RID: 1416
		public const long TicksPerMinute = 600000000L;

		// Token: 0x04000589 RID: 1417
		private const double MinutesPerTick = 1.6666666666666667E-09;

		// Token: 0x0400058A RID: 1418
		public const long TicksPerHour = 36000000000L;

		// Token: 0x0400058B RID: 1419
		private const double HoursPerTick = 2.7777777777777777E-11;

		// Token: 0x0400058C RID: 1420
		public const long TicksPerDay = 864000000000L;

		// Token: 0x0400058D RID: 1421
		private const double DaysPerTick = 1.1574074074074074E-12;

		// Token: 0x0400058E RID: 1422
		private const int MillisPerSecond = 1000;

		// Token: 0x0400058F RID: 1423
		private const int MillisPerMinute = 60000;

		// Token: 0x04000590 RID: 1424
		private const int MillisPerHour = 3600000;

		// Token: 0x04000591 RID: 1425
		private const int MillisPerDay = 86400000;

		// Token: 0x04000592 RID: 1426
		private const long MaxSeconds = 922337203685L;

		// Token: 0x04000593 RID: 1427
		private const long MinSeconds = -922337203685L;

		// Token: 0x04000594 RID: 1428
		private const long MaxMilliSeconds = 922337203685477L;

		// Token: 0x04000595 RID: 1429
		private const long MinMilliSeconds = -922337203685477L;

		// Token: 0x04000596 RID: 1430
		public static readonly TimeSpan Zero = new TimeSpan(0L);

		// Token: 0x04000597 RID: 1431
		public static readonly TimeSpan MaxValue = new TimeSpan(long.MaxValue);

		// Token: 0x04000598 RID: 1432
		public static readonly TimeSpan MinValue = new TimeSpan(long.MinValue);

		// Token: 0x04000599 RID: 1433
		internal long _ticks;

		// Token: 0x0200012C RID: 300
		private struct StringParser
		{
			// Token: 0x060010AE RID: 4270 RVA: 0x0002EA68 File Offset: 0x0002DA68
			internal void NextChar()
			{
				if (this.pos < this.len)
				{
					this.pos++;
				}
				this.ch = ((this.pos < this.len) ? this.str[this.pos] : '\0');
			}

			// Token: 0x060010AF RID: 4271 RVA: 0x0002EABC File Offset: 0x0002DABC
			internal char NextNonDigit()
			{
				for (int i = this.pos; i < this.len; i++)
				{
					char c = this.str[i];
					if (c < '0' || c > '9')
					{
						return c;
					}
				}
				return '\0';
			}

			// Token: 0x060010B0 RID: 4272 RVA: 0x0002EAFC File Offset: 0x0002DAFC
			internal long Parse(string s)
			{
				long result;
				if (this.TryParse(s, out result))
				{
					return result;
				}
				switch (this.error)
				{
				case TimeSpan.StringParser.ParseError.Format:
					throw new FormatException(Environment.GetResourceString("Format_InvalidString"));
				case TimeSpan.StringParser.ParseError.Overflow:
					throw new OverflowException(Environment.GetResourceString("Overflow_TimeSpanTooLong"));
				case TimeSpan.StringParser.ParseError.OverflowHoursMinutesSeconds:
					throw new OverflowException(Environment.GetResourceString("Overflow_TimeSpanElementTooLarge"));
				case TimeSpan.StringParser.ParseError.ArgumentNull:
					throw new ArgumentNullException("s");
				default:
					return 0L;
				}
			}

			// Token: 0x060010B1 RID: 4273 RVA: 0x0002EB74 File Offset: 0x0002DB74
			internal bool TryParse(string s, out long value)
			{
				value = 0L;
				if (s == null)
				{
					this.error = TimeSpan.StringParser.ParseError.ArgumentNull;
					return false;
				}
				this.str = s;
				this.len = s.Length;
				this.pos = -1;
				this.NextChar();
				this.SkipBlanks();
				bool flag = false;
				if (this.ch == '-')
				{
					flag = true;
					this.NextChar();
				}
				long num;
				if (this.NextNonDigit() == ':')
				{
					if (!this.ParseTime(out num))
					{
						return false;
					}
				}
				else
				{
					int num2;
					if (!this.ParseInt(10675199, out num2))
					{
						return false;
					}
					num = (long)num2 * 864000000000L;
					if (this.ch == '.')
					{
						this.NextChar();
						long num3;
						if (!this.ParseTime(out num3))
						{
							return false;
						}
						num += num3;
					}
				}
				if (flag)
				{
					num = -num;
					if (num > 0L)
					{
						this.error = TimeSpan.StringParser.ParseError.Overflow;
						return false;
					}
				}
				else if (num < 0L)
				{
					this.error = TimeSpan.StringParser.ParseError.Overflow;
					return false;
				}
				this.SkipBlanks();
				if (this.pos < this.len)
				{
					this.error = TimeSpan.StringParser.ParseError.Format;
					return false;
				}
				value = num;
				return true;
			}

			// Token: 0x060010B2 RID: 4274 RVA: 0x0002EC64 File Offset: 0x0002DC64
			internal bool ParseInt(int max, out int i)
			{
				i = 0;
				int num = this.pos;
				while (this.ch >= '0' && this.ch <= '9')
				{
					if (((long)i & (long)((ulong)-268435456)) != 0L)
					{
						this.error = TimeSpan.StringParser.ParseError.Overflow;
						return false;
					}
					i = i * 10 + (int)this.ch - 48;
					if (i < 0)
					{
						this.error = TimeSpan.StringParser.ParseError.Overflow;
						return false;
					}
					this.NextChar();
				}
				if (num == this.pos)
				{
					this.error = TimeSpan.StringParser.ParseError.Format;
					return false;
				}
				if (i > max)
				{
					this.error = TimeSpan.StringParser.ParseError.Overflow;
					return false;
				}
				return true;
			}

			// Token: 0x060010B3 RID: 4275 RVA: 0x0002ECF0 File Offset: 0x0002DCF0
			internal bool ParseTime(out long time)
			{
				time = 0L;
				int num;
				if (!this.ParseInt(23, out num))
				{
					if (this.error == TimeSpan.StringParser.ParseError.Overflow)
					{
						this.error = TimeSpan.StringParser.ParseError.OverflowHoursMinutesSeconds;
					}
					return false;
				}
				time = (long)num * 36000000000L;
				if (this.ch != ':')
				{
					this.error = TimeSpan.StringParser.ParseError.Format;
					return false;
				}
				this.NextChar();
				if (!this.ParseInt(59, out num))
				{
					if (this.error == TimeSpan.StringParser.ParseError.Overflow)
					{
						this.error = TimeSpan.StringParser.ParseError.OverflowHoursMinutesSeconds;
					}
					return false;
				}
				time += (long)num * 600000000L;
				if (this.ch == ':')
				{
					this.NextChar();
					if (this.ch != '.')
					{
						if (!this.ParseInt(59, out num))
						{
							if (this.error == TimeSpan.StringParser.ParseError.Overflow)
							{
								this.error = TimeSpan.StringParser.ParseError.OverflowHoursMinutesSeconds;
							}
							return false;
						}
						time += (long)num * 10000000L;
					}
					if (this.ch == '.')
					{
						this.NextChar();
						int num2 = 10000000;
						while (num2 > 1 && this.ch >= '0' && this.ch <= '9')
						{
							num2 /= 10;
							time += (long)((int)(this.ch - '0') * num2);
							this.NextChar();
						}
					}
				}
				return true;
			}

			// Token: 0x060010B4 RID: 4276 RVA: 0x0002EE08 File Offset: 0x0002DE08
			internal void SkipBlanks()
			{
				while (this.ch == ' ' || this.ch == '\t')
				{
					this.NextChar();
				}
			}

			// Token: 0x0400059A RID: 1434
			private string str;

			// Token: 0x0400059B RID: 1435
			private char ch;

			// Token: 0x0400059C RID: 1436
			private int pos;

			// Token: 0x0400059D RID: 1437
			private int len;

			// Token: 0x0400059E RID: 1438
			private TimeSpan.StringParser.ParseError error;

			// Token: 0x0200012D RID: 301
			private enum ParseError
			{
				// Token: 0x040005A0 RID: 1440
				Format = 1,
				// Token: 0x040005A1 RID: 1441
				Overflow,
				// Token: 0x040005A2 RID: 1442
				OverflowHoursMinutesSeconds,
				// Token: 0x040005A3 RID: 1443
				ArgumentNull
			}
		}
	}
}
