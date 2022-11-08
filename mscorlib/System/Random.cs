using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x020000EE RID: 238
	[ComVisible(true)]
	[Serializable]
	public class Random
	{
		// Token: 0x06000C8B RID: 3211 RVA: 0x000258E1 File Offset: 0x000248E1
		public Random() : this(Environment.TickCount)
		{
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x000258F0 File Offset: 0x000248F0
		public Random(int Seed)
		{
			int num = 161803398 - Math.Abs(Seed);
			this.SeedArray[55] = num;
			int num2 = 1;
			for (int i = 1; i < 55; i++)
			{
				int num3 = 21 * i % 55;
				this.SeedArray[num3] = num2;
				num2 = num - num2;
				if (num2 < 0)
				{
					num2 += int.MaxValue;
				}
				num = this.SeedArray[num3];
			}
			for (int j = 1; j < 5; j++)
			{
				for (int k = 1; k < 56; k++)
				{
					this.SeedArray[k] -= this.SeedArray[1 + (k + 30) % 55];
					if (this.SeedArray[k] < 0)
					{
						this.SeedArray[k] += int.MaxValue;
					}
				}
			}
			this.inext = 0;
			this.inextp = 21;
			Seed = 1;
		}

		// Token: 0x06000C8D RID: 3213 RVA: 0x000259E7 File Offset: 0x000249E7
		protected virtual double Sample()
		{
			return (double)this.InternalSample() * 4.656612875245797E-10;
		}

		// Token: 0x06000C8E RID: 3214 RVA: 0x000259FC File Offset: 0x000249FC
		private int InternalSample()
		{
			int num = this.inext;
			int num2 = this.inextp;
			if (++num >= 56)
			{
				num = 1;
			}
			if (++num2 >= 56)
			{
				num2 = 1;
			}
			int num3 = this.SeedArray[num] - this.SeedArray[num2];
			if (num3 < 0)
			{
				num3 += int.MaxValue;
			}
			this.SeedArray[num] = num3;
			this.inext = num;
			this.inextp = num2;
			return num3;
		}

		// Token: 0x06000C8F RID: 3215 RVA: 0x00025A63 File Offset: 0x00024A63
		public virtual int Next()
		{
			return this.InternalSample();
		}

		// Token: 0x06000C90 RID: 3216 RVA: 0x00025A6C File Offset: 0x00024A6C
		private double GetSampleForLargeRange()
		{
			int num = this.InternalSample();
			bool flag = this.InternalSample() % 2 == 0;
			if (flag)
			{
				num = -num;
			}
			double num2 = (double)num;
			num2 += 2147483646.0;
			return num2 / 4294967293.0;
		}

		// Token: 0x06000C91 RID: 3217 RVA: 0x00025AB4 File Offset: 0x00024AB4
		public virtual int Next(int minValue, int maxValue)
		{
			if (minValue > maxValue)
			{
				throw new ArgumentOutOfRangeException("minValue", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_MinMaxValue"), new object[]
				{
					"minValue",
					"maxValue"
				}));
			}
			long num = (long)maxValue - (long)minValue;
			if (num <= 2147483647L)
			{
				return (int)(this.Sample() * (double)num) + minValue;
			}
			return (int)((long)(this.GetSampleForLargeRange() * (double)num) + (long)minValue);
		}

		// Token: 0x06000C92 RID: 3218 RVA: 0x00025B28 File Offset: 0x00024B28
		public virtual int Next(int maxValue)
		{
			if (maxValue < 0)
			{
				throw new ArgumentOutOfRangeException("maxValue", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_MustBePositive"), new object[]
				{
					"maxValue"
				}));
			}
			return (int)(this.Sample() * (double)maxValue);
		}

		// Token: 0x06000C93 RID: 3219 RVA: 0x00025B72 File Offset: 0x00024B72
		public virtual double NextDouble()
		{
			return this.Sample();
		}

		// Token: 0x06000C94 RID: 3220 RVA: 0x00025B7C File Offset: 0x00024B7C
		public virtual void NextBytes(byte[] buffer)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			for (int i = 0; i < buffer.Length; i++)
			{
				buffer[i] = (byte)(this.InternalSample() % 256);
			}
		}

		// Token: 0x04000483 RID: 1155
		private const int MBIG = 2147483647;

		// Token: 0x04000484 RID: 1156
		private const int MSEED = 161803398;

		// Token: 0x04000485 RID: 1157
		private const int MZ = 0;

		// Token: 0x04000486 RID: 1158
		private int inext;

		// Token: 0x04000487 RID: 1159
		private int inextp;

		// Token: 0x04000488 RID: 1160
		private int[] SeedArray = new int[56];
	}
}
