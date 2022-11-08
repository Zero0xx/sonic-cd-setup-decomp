using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;

namespace System
{
	// Token: 0x020000D2 RID: 210
	public static class Math
	{
		// Token: 0x06000BA7 RID: 2983
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Acos(double d);

		// Token: 0x06000BA8 RID: 2984
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Asin(double d);

		// Token: 0x06000BA9 RID: 2985
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Atan(double d);

		// Token: 0x06000BAA RID: 2986
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Atan2(double y, double x);

		// Token: 0x06000BAB RID: 2987 RVA: 0x00023713 File Offset: 0x00022713
		public static decimal Ceiling(decimal d)
		{
			return decimal.Ceiling(d);
		}

		// Token: 0x06000BAC RID: 2988
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Ceiling(double a);

		// Token: 0x06000BAD RID: 2989
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Cos(double d);

		// Token: 0x06000BAE RID: 2990
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Cosh(double value);

		// Token: 0x06000BAF RID: 2991 RVA: 0x0002371B File Offset: 0x0002271B
		public static decimal Floor(decimal d)
		{
			return decimal.Floor(d);
		}

		// Token: 0x06000BB0 RID: 2992
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Floor(double d);

		// Token: 0x06000BB1 RID: 2993 RVA: 0x00023724 File Offset: 0x00022724
		private unsafe static double InternalRound(double value, int digits, MidpointRounding mode)
		{
			if (Math.Abs(value) < Math.doubleRoundLimit)
			{
				double num = Math.roundPower10Double[digits];
				value *= num;
				if (mode == MidpointRounding.AwayFromZero)
				{
					double value2 = Math.SplitFractionDouble(&value);
					if (Math.Abs(value2) >= 0.5)
					{
						value += (double)Math.Sign(value2);
					}
				}
				else
				{
					value = Math.Round(value);
				}
				value /= num;
			}
			return value;
		}

		// Token: 0x06000BB2 RID: 2994 RVA: 0x00023784 File Offset: 0x00022784
		private unsafe static double InternalTruncate(double d)
		{
			Math.SplitFractionDouble(&d);
			return d;
		}

		// Token: 0x06000BB3 RID: 2995
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Sin(double a);

		// Token: 0x06000BB4 RID: 2996
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Tan(double a);

		// Token: 0x06000BB5 RID: 2997
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Sinh(double value);

		// Token: 0x06000BB6 RID: 2998
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Tanh(double value);

		// Token: 0x06000BB7 RID: 2999
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Round(double a);

		// Token: 0x06000BB8 RID: 3000 RVA: 0x00023790 File Offset: 0x00022790
		public static double Round(double value, int digits)
		{
			if (digits < 0 || digits > 15)
			{
				throw new ArgumentOutOfRangeException("digits", Environment.GetResourceString("ArgumentOutOfRange_RoundingDigits"));
			}
			return Math.InternalRound(value, digits, MidpointRounding.ToEven);
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x000237B8 File Offset: 0x000227B8
		public static double Round(double value, MidpointRounding mode)
		{
			return Math.Round(value, 0, mode);
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x000237C4 File Offset: 0x000227C4
		public static double Round(double value, int digits, MidpointRounding mode)
		{
			if (digits < 0 || digits > 15)
			{
				throw new ArgumentOutOfRangeException("digits", Environment.GetResourceString("ArgumentOutOfRange_RoundingDigits"));
			}
			if (mode < MidpointRounding.ToEven || mode > MidpointRounding.AwayFromZero)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidEnumValue", new object[]
				{
					mode,
					"MidpointRounding"
				}), "mode");
			}
			return Math.InternalRound(value, digits, mode);
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x0002382D File Offset: 0x0002282D
		public static decimal Round(decimal d)
		{
			return decimal.Round(d, 0);
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x00023836 File Offset: 0x00022836
		public static decimal Round(decimal d, int decimals)
		{
			return decimal.Round(d, decimals);
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x0002383F File Offset: 0x0002283F
		public static decimal Round(decimal d, MidpointRounding mode)
		{
			return decimal.Round(d, 0, mode);
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x00023849 File Offset: 0x00022849
		public static decimal Round(decimal d, int decimals, MidpointRounding mode)
		{
			return decimal.Round(d, decimals, mode);
		}

		// Token: 0x06000BBF RID: 3007
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern double SplitFractionDouble(double* value);

		// Token: 0x06000BC0 RID: 3008 RVA: 0x00023853 File Offset: 0x00022853
		public static decimal Truncate(decimal d)
		{
			return decimal.Truncate(d);
		}

		// Token: 0x06000BC1 RID: 3009 RVA: 0x0002385B File Offset: 0x0002285B
		public static double Truncate(double d)
		{
			return Math.InternalTruncate(d);
		}

		// Token: 0x06000BC2 RID: 3010
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Sqrt(double d);

		// Token: 0x06000BC3 RID: 3011
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Log(double d);

		// Token: 0x06000BC4 RID: 3012
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Log10(double d);

		// Token: 0x06000BC5 RID: 3013
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Exp(double d);

		// Token: 0x06000BC6 RID: 3014
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Pow(double x, double y);

		// Token: 0x06000BC7 RID: 3015 RVA: 0x00023864 File Offset: 0x00022864
		public static double IEEERemainder(double x, double y)
		{
			double num = x % y;
			if (double.IsNaN(num))
			{
				return double.NaN;
			}
			if (num == 0.0 && double.IsNegative(x))
			{
				return double.NegativeZero;
			}
			double num2 = num - Math.Abs(y) * (double)Math.Sign(x);
			if (Math.Abs(num2) == Math.Abs(num))
			{
				double num3 = x / y;
				double value = Math.Round(num3);
				if (Math.Abs(value) > Math.Abs(num3))
				{
					return num2;
				}
				return num;
			}
			else
			{
				if (Math.Abs(num2) < Math.Abs(num))
				{
					return num2;
				}
				return num;
			}
		}

		// Token: 0x06000BC8 RID: 3016 RVA: 0x000238EE File Offset: 0x000228EE
		[CLSCompliant(false)]
		public static sbyte Abs(sbyte value)
		{
			if (value >= 0)
			{
				return value;
			}
			return Math.AbsHelper(value);
		}

		// Token: 0x06000BC9 RID: 3017 RVA: 0x000238FC File Offset: 0x000228FC
		private static sbyte AbsHelper(sbyte value)
		{
			if (value == -128)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_NegateTwosCompNum"));
			}
			return -value;
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x00023916 File Offset: 0x00022916
		public static short Abs(short value)
		{
			if (value >= 0)
			{
				return value;
			}
			return Math.AbsHelper(value);
		}

		// Token: 0x06000BCB RID: 3019 RVA: 0x00023924 File Offset: 0x00022924
		private static short AbsHelper(short value)
		{
			if (value == -32768)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_NegateTwosCompNum"));
			}
			return -value;
		}

		// Token: 0x06000BCC RID: 3020 RVA: 0x00023941 File Offset: 0x00022941
		public static int Abs(int value)
		{
			if (value >= 0)
			{
				return value;
			}
			return Math.AbsHelper(value);
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x0002394F File Offset: 0x0002294F
		private static int AbsHelper(int value)
		{
			if (value == -2147483648)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_NegateTwosCompNum"));
			}
			return -value;
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x0002396B File Offset: 0x0002296B
		public static long Abs(long value)
		{
			if (value >= 0L)
			{
				return value;
			}
			return Math.AbsHelper(value);
		}

		// Token: 0x06000BCF RID: 3023 RVA: 0x0002397A File Offset: 0x0002297A
		private static long AbsHelper(long value)
		{
			if (value == -9223372036854775808L)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_NegateTwosCompNum"));
			}
			return -value;
		}

		// Token: 0x06000BD0 RID: 3024
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float Abs(float value);

		// Token: 0x06000BD1 RID: 3025
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Abs(double value);

		// Token: 0x06000BD2 RID: 3026 RVA: 0x0002399A File Offset: 0x0002299A
		public static decimal Abs(decimal value)
		{
			return decimal.Abs(value);
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x000239A2 File Offset: 0x000229A2
		[CLSCompliant(false)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static sbyte Max(sbyte val1, sbyte val2)
		{
			if (val1 < val2)
			{
				return val2;
			}
			return val1;
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x000239AB File Offset: 0x000229AB
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static byte Max(byte val1, byte val2)
		{
			if (val1 < val2)
			{
				return val2;
			}
			return val1;
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x000239B4 File Offset: 0x000229B4
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static short Max(short val1, short val2)
		{
			if (val1 < val2)
			{
				return val2;
			}
			return val1;
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x000239BD File Offset: 0x000229BD
		[CLSCompliant(false)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static ushort Max(ushort val1, ushort val2)
		{
			if (val1 < val2)
			{
				return val2;
			}
			return val1;
		}

		// Token: 0x06000BD7 RID: 3031 RVA: 0x000239C6 File Offset: 0x000229C6
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static int Max(int val1, int val2)
		{
			if (val1 < val2)
			{
				return val2;
			}
			return val1;
		}

		// Token: 0x06000BD8 RID: 3032 RVA: 0x000239CF File Offset: 0x000229CF
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[CLSCompliant(false)]
		public static uint Max(uint val1, uint val2)
		{
			if (val1 < val2)
			{
				return val2;
			}
			return val1;
		}

		// Token: 0x06000BD9 RID: 3033 RVA: 0x000239D8 File Offset: 0x000229D8
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static long Max(long val1, long val2)
		{
			if (val1 < val2)
			{
				return val2;
			}
			return val1;
		}

		// Token: 0x06000BDA RID: 3034 RVA: 0x000239E1 File Offset: 0x000229E1
		[CLSCompliant(false)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static ulong Max(ulong val1, ulong val2)
		{
			if (val1 < val2)
			{
				return val2;
			}
			return val1;
		}

		// Token: 0x06000BDB RID: 3035 RVA: 0x000239EA File Offset: 0x000229EA
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static float Max(float val1, float val2)
		{
			if (val1 > val2)
			{
				return val1;
			}
			if (float.IsNaN(val1))
			{
				return val1;
			}
			return val2;
		}

		// Token: 0x06000BDC RID: 3036 RVA: 0x000239FD File Offset: 0x000229FD
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static double Max(double val1, double val2)
		{
			if (val1 > val2)
			{
				return val1;
			}
			if (double.IsNaN(val1))
			{
				return val1;
			}
			return val2;
		}

		// Token: 0x06000BDD RID: 3037 RVA: 0x00023A10 File Offset: 0x00022A10
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static decimal Max(decimal val1, decimal val2)
		{
			return decimal.Max(val1, val2);
		}

		// Token: 0x06000BDE RID: 3038 RVA: 0x00023A19 File Offset: 0x00022A19
		[CLSCompliant(false)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static sbyte Min(sbyte val1, sbyte val2)
		{
			if (val1 > val2)
			{
				return val2;
			}
			return val1;
		}

		// Token: 0x06000BDF RID: 3039 RVA: 0x00023A22 File Offset: 0x00022A22
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static byte Min(byte val1, byte val2)
		{
			if (val1 > val2)
			{
				return val2;
			}
			return val1;
		}

		// Token: 0x06000BE0 RID: 3040 RVA: 0x00023A2B File Offset: 0x00022A2B
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static short Min(short val1, short val2)
		{
			if (val1 > val2)
			{
				return val2;
			}
			return val1;
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x00023A34 File Offset: 0x00022A34
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[CLSCompliant(false)]
		public static ushort Min(ushort val1, ushort val2)
		{
			if (val1 > val2)
			{
				return val2;
			}
			return val1;
		}

		// Token: 0x06000BE2 RID: 3042 RVA: 0x00023A3D File Offset: 0x00022A3D
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static int Min(int val1, int val2)
		{
			if (val1 > val2)
			{
				return val2;
			}
			return val1;
		}

		// Token: 0x06000BE3 RID: 3043 RVA: 0x00023A46 File Offset: 0x00022A46
		[CLSCompliant(false)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static uint Min(uint val1, uint val2)
		{
			if (val1 > val2)
			{
				return val2;
			}
			return val1;
		}

		// Token: 0x06000BE4 RID: 3044 RVA: 0x00023A4F File Offset: 0x00022A4F
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static long Min(long val1, long val2)
		{
			if (val1 > val2)
			{
				return val2;
			}
			return val1;
		}

		// Token: 0x06000BE5 RID: 3045 RVA: 0x00023A58 File Offset: 0x00022A58
		[CLSCompliant(false)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static ulong Min(ulong val1, ulong val2)
		{
			if (val1 > val2)
			{
				return val2;
			}
			return val1;
		}

		// Token: 0x06000BE6 RID: 3046 RVA: 0x00023A61 File Offset: 0x00022A61
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static float Min(float val1, float val2)
		{
			if (val1 < val2)
			{
				return val1;
			}
			if (float.IsNaN(val1))
			{
				return val1;
			}
			return val2;
		}

		// Token: 0x06000BE7 RID: 3047 RVA: 0x00023A74 File Offset: 0x00022A74
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static double Min(double val1, double val2)
		{
			if (val1 < val2)
			{
				return val1;
			}
			if (double.IsNaN(val1))
			{
				return val1;
			}
			return val2;
		}

		// Token: 0x06000BE8 RID: 3048 RVA: 0x00023A87 File Offset: 0x00022A87
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static decimal Min(decimal val1, decimal val2)
		{
			return decimal.Min(val1, val2);
		}

		// Token: 0x06000BE9 RID: 3049 RVA: 0x00023A90 File Offset: 0x00022A90
		public static double Log(double a, double newBase)
		{
			if (newBase == 1.0)
			{
				return double.NaN;
			}
			if (a != 1.0 && (newBase == 0.0 || double.IsPositiveInfinity(newBase)))
			{
				return double.NaN;
			}
			return Math.Log(a) / Math.Log(newBase);
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x00023AEA File Offset: 0x00022AEA
		[CLSCompliant(false)]
		public static int Sign(sbyte value)
		{
			if (value < 0)
			{
				return -1;
			}
			if (value > 0)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x00023AF9 File Offset: 0x00022AF9
		public static int Sign(short value)
		{
			if (value < 0)
			{
				return -1;
			}
			if (value > 0)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06000BEC RID: 3052 RVA: 0x00023B08 File Offset: 0x00022B08
		public static int Sign(int value)
		{
			if (value < 0)
			{
				return -1;
			}
			if (value > 0)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06000BED RID: 3053 RVA: 0x00023B17 File Offset: 0x00022B17
		public static int Sign(long value)
		{
			if (value < 0L)
			{
				return -1;
			}
			if (value > 0L)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06000BEE RID: 3054 RVA: 0x00023B28 File Offset: 0x00022B28
		public static int Sign(float value)
		{
			if (value < 0f)
			{
				return -1;
			}
			if (value > 0f)
			{
				return 1;
			}
			if (value == 0f)
			{
				return 0;
			}
			throw new ArithmeticException(Environment.GetResourceString("Arithmetic_NaN"));
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x00023B57 File Offset: 0x00022B57
		public static int Sign(double value)
		{
			if (value < 0.0)
			{
				return -1;
			}
			if (value > 0.0)
			{
				return 1;
			}
			if (value == 0.0)
			{
				return 0;
			}
			throw new ArithmeticException(Environment.GetResourceString("Arithmetic_NaN"));
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x00023B92 File Offset: 0x00022B92
		public static int Sign(decimal value)
		{
			if (value < 0m)
			{
				return -1;
			}
			if (value > 0m)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x00023BB5 File Offset: 0x00022BB5
		public static long BigMul(int a, int b)
		{
			return (long)a * (long)b;
		}

		// Token: 0x06000BF2 RID: 3058 RVA: 0x00023BBC File Offset: 0x00022BBC
		public static int DivRem(int a, int b, out int result)
		{
			result = a % b;
			return a / b;
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x00023BC6 File Offset: 0x00022BC6
		public static long DivRem(long a, long b, out long result)
		{
			result = a % b;
			return a / b;
		}

		// Token: 0x0400042F RID: 1071
		private const int maxRoundingDigits = 15;

		// Token: 0x04000430 RID: 1072
		public const double PI = 3.141592653589793;

		// Token: 0x04000431 RID: 1073
		public const double E = 2.718281828459045;

		// Token: 0x04000432 RID: 1074
		private static double doubleRoundLimit = 10000000000000000.0;

		// Token: 0x04000433 RID: 1075
		private static double[] roundPower10Double = new double[]
		{
			1.0,
			10.0,
			100.0,
			1000.0,
			10000.0,
			100000.0,
			1000000.0,
			10000000.0,
			100000000.0,
			1000000000.0,
			10000000000.0,
			100000000000.0,
			1000000000000.0,
			10000000000000.0,
			100000000000000.0,
			1000000000000000.0
		};
	}
}
