using System;

namespace System.Windows.Forms
{
	// Token: 0x020005A6 RID: 1446
	public class NumericUpDownAcceleration
	{
		// Token: 0x06004B02 RID: 19202 RVA: 0x0010FF64 File Offset: 0x0010EF64
		public NumericUpDownAcceleration(int seconds, decimal increment)
		{
			if (seconds < 0)
			{
				throw new ArgumentOutOfRangeException("seconds", seconds, SR.GetString("NumericUpDownLessThanZeroError"));
			}
			if (increment < 0m)
			{
				throw new ArgumentOutOfRangeException("increment", increment, SR.GetString("NumericUpDownLessThanZeroError"));
			}
			this.seconds = seconds;
			this.increment = increment;
		}

		// Token: 0x17000ECA RID: 3786
		// (get) Token: 0x06004B03 RID: 19203 RVA: 0x0010FFCD File Offset: 0x0010EFCD
		// (set) Token: 0x06004B04 RID: 19204 RVA: 0x0010FFD5 File Offset: 0x0010EFD5
		public int Seconds
		{
			get
			{
				return this.seconds;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("seconds", value, SR.GetString("NumericUpDownLessThanZeroError"));
				}
				this.seconds = value;
			}
		}

		// Token: 0x17000ECB RID: 3787
		// (get) Token: 0x06004B05 RID: 19205 RVA: 0x0010FFFD File Offset: 0x0010EFFD
		// (set) Token: 0x06004B06 RID: 19206 RVA: 0x00110005 File Offset: 0x0010F005
		public decimal Increment
		{
			get
			{
				return this.increment;
			}
			set
			{
				if (value < 0m)
				{
					throw new ArgumentOutOfRangeException("increment", value, SR.GetString("NumericUpDownLessThanZeroError"));
				}
				this.increment = value;
			}
		}

		// Token: 0x040030E9 RID: 12521
		private int seconds;

		// Token: 0x040030EA RID: 12522
		private decimal increment;
	}
}
