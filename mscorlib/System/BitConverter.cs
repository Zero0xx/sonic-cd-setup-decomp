using System;

namespace System
{
	// Token: 0x02000077 RID: 119
	public static class BitConverter
	{
		// Token: 0x0600068F RID: 1679 RVA: 0x000160CC File Offset: 0x000150CC
		public static byte[] GetBytes(bool value)
		{
			return new byte[]
			{
				value ? 1 : 0
			};
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x000160EB File Offset: 0x000150EB
		public static byte[] GetBytes(char value)
		{
			return BitConverter.GetBytes((short)value);
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x000160F4 File Offset: 0x000150F4
		public unsafe static byte[] GetBytes(short value)
		{
			byte[] array = new byte[2];
			fixed (byte* ptr = array)
			{
				*(short*)ptr = value;
			}
			return array;
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x00016128 File Offset: 0x00015128
		public unsafe static byte[] GetBytes(int value)
		{
			byte[] array = new byte[4];
			fixed (byte* ptr = array)
			{
				*(int*)ptr = value;
			}
			return array;
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x0001615C File Offset: 0x0001515C
		public unsafe static byte[] GetBytes(long value)
		{
			byte[] array = new byte[8];
			fixed (byte* ptr = array)
			{
				*(long*)ptr = value;
			}
			return array;
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x0001618F File Offset: 0x0001518F
		[CLSCompliant(false)]
		public static byte[] GetBytes(ushort value)
		{
			return BitConverter.GetBytes((short)value);
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x00016198 File Offset: 0x00015198
		[CLSCompliant(false)]
		public static byte[] GetBytes(uint value)
		{
			return BitConverter.GetBytes((int)value);
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x000161A0 File Offset: 0x000151A0
		[CLSCompliant(false)]
		public static byte[] GetBytes(ulong value)
		{
			return BitConverter.GetBytes((long)value);
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x000161A8 File Offset: 0x000151A8
		public unsafe static byte[] GetBytes(float value)
		{
			return BitConverter.GetBytes(*(int*)(&value));
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x000161B3 File Offset: 0x000151B3
		public unsafe static byte[] GetBytes(double value)
		{
			return BitConverter.GetBytes(*(long*)(&value));
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x000161BE File Offset: 0x000151BE
		public static char ToChar(byte[] value, int startIndex)
		{
			return (char)BitConverter.ToInt16(value, startIndex);
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x000161C8 File Offset: 0x000151C8
		public unsafe static short ToInt16(byte[] value, int startIndex)
		{
			if (value == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.value);
			}
			if ((ulong)startIndex >= (ulong)((long)value.Length))
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.startIndex, ExceptionResource.ArgumentOutOfRange_Index);
			}
			if (startIndex > value.Length - 2)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
			}
			fixed (byte* ptr = &value[startIndex])
			{
				short result;
				if (startIndex % 2 == 0)
				{
					result = *(short*)ptr;
				}
				else if (BitConverter.IsLittleEndian)
				{
					result = (short)((int)(*ptr) | (int)ptr[1] << 8);
				}
				else
				{
					result = (short)((int)(*ptr) << 8 | (int)ptr[1]);
				}
				return result;
			}
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x0001623C File Offset: 0x0001523C
		public unsafe static int ToInt32(byte[] value, int startIndex)
		{
			if (value == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.value);
			}
			if ((ulong)startIndex >= (ulong)((long)value.Length))
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.startIndex, ExceptionResource.ArgumentOutOfRange_Index);
			}
			if (startIndex > value.Length - 4)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
			}
			fixed (byte* ptr = &value[startIndex])
			{
				int result;
				if (startIndex % 4 == 0)
				{
					result = *(int*)ptr;
				}
				else if (BitConverter.IsLittleEndian)
				{
					result = ((int)(*ptr) | (int)ptr[1] << 8 | (int)ptr[2] << 16 | (int)ptr[3] << 24);
				}
				else
				{
					result = ((int)(*ptr) << 24 | (int)ptr[1] << 16 | (int)ptr[2] << 8 | (int)ptr[3]);
				}
				return result;
			}
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x000162D4 File Offset: 0x000152D4
		public unsafe static long ToInt64(byte[] value, int startIndex)
		{
			if (value == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.value);
			}
			if ((ulong)startIndex >= (ulong)((long)value.Length))
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.startIndex, ExceptionResource.ArgumentOutOfRange_Index);
			}
			if (startIndex > value.Length - 8)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
			}
			fixed (byte* ptr = &value[startIndex])
			{
				long result;
				if (startIndex % 8 == 0)
				{
					result = *(long*)ptr;
				}
				else if (BitConverter.IsLittleEndian)
				{
					int num = (int)(*ptr) | (int)ptr[1] << 8 | (int)ptr[2] << 16 | (int)ptr[3] << 24;
					int num2 = (int)ptr[4] | (int)ptr[5] << 8 | (int)ptr[6] << 16 | (int)ptr[7] << 24;
					result = (long)((ulong)num | (ulong)((ulong)((long)num2) << 32));
				}
				else
				{
					int num3 = (int)(*ptr) << 24 | (int)ptr[1] << 16 | (int)ptr[2] << 8 | (int)ptr[3];
					int num4 = (int)ptr[4] << 24 | (int)ptr[5] << 16 | (int)ptr[6] << 8 | (int)ptr[7];
					result = (long)((ulong)num4 | (ulong)((ulong)((long)num3) << 32));
				}
				return result;
			}
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x000163CE File Offset: 0x000153CE
		[CLSCompliant(false)]
		public static ushort ToUInt16(byte[] value, int startIndex)
		{
			return (ushort)BitConverter.ToInt16(value, startIndex);
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x000163D8 File Offset: 0x000153D8
		[CLSCompliant(false)]
		public static uint ToUInt32(byte[] value, int startIndex)
		{
			return (uint)BitConverter.ToInt32(value, startIndex);
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x000163E1 File Offset: 0x000153E1
		[CLSCompliant(false)]
		public static ulong ToUInt64(byte[] value, int startIndex)
		{
			return (ulong)BitConverter.ToInt64(value, startIndex);
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x000163EC File Offset: 0x000153EC
		public unsafe static float ToSingle(byte[] value, int startIndex)
		{
			int num = BitConverter.ToInt32(value, startIndex);
			return *(float*)(&num);
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x00016408 File Offset: 0x00015408
		public unsafe static double ToDouble(byte[] value, int startIndex)
		{
			long num = BitConverter.ToInt64(value, startIndex);
			return *(double*)(&num);
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x00016421 File Offset: 0x00015421
		private static char GetHexValue(int i)
		{
			if (i < 10)
			{
				return (char)(i + 48);
			}
			return (char)(i - 10 + 65);
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x00016438 File Offset: 0x00015438
		public static string ToString(byte[] value, int startIndex, int length)
		{
			if (value == null)
			{
				throw new ArgumentNullException("byteArray");
			}
			int num = value.Length;
			if (startIndex < 0 || (startIndex >= num && startIndex > 0))
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_StartIndex"));
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", Environment.GetResourceString("ArgumentOutOfRange_GenericPositive"));
			}
			if (startIndex > num - length)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_ArrayPlusOffTooSmall"));
			}
			if (length == 0)
			{
				return string.Empty;
			}
			char[] array = new char[length * 3];
			int num2 = startIndex;
			for (int i = 0; i < length * 3; i += 3)
			{
				byte b = value[num2++];
				array[i] = BitConverter.GetHexValue((int)(b / 16));
				array[i + 1] = BitConverter.GetHexValue((int)(b % 16));
				array[i + 2] = '-';
			}
			return new string(array, 0, array.Length - 1);
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x00016508 File Offset: 0x00015508
		public static string ToString(byte[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return BitConverter.ToString(value, 0, value.Length);
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x00016522 File Offset: 0x00015522
		public static string ToString(byte[] value, int startIndex)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return BitConverter.ToString(value, startIndex, value.Length - startIndex);
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x00016540 File Offset: 0x00015540
		public static bool ToBoolean(byte[] value, int startIndex)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (startIndex > value.Length - 1)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			return value[startIndex] != 0;
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x00016599 File Offset: 0x00015599
		public unsafe static long DoubleToInt64Bits(double value)
		{
			return *(long*)(&value);
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x0001659F File Offset: 0x0001559F
		public unsafe static double Int64BitsToDouble(long value)
		{
			return *(double*)(&value);
		}

		// Token: 0x04000215 RID: 533
		public static readonly bool IsLittleEndian = true;
	}
}
