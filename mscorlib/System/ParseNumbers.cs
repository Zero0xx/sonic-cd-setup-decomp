using System;
using System.Runtime.CompilerServices;

namespace System
{
	// Token: 0x020000EB RID: 235
	internal static class ParseNumbers
	{
		// Token: 0x06000C7F RID: 3199 RVA: 0x0002583A File Offset: 0x0002483A
		public static long StringToLong(string s, int radix, int flags)
		{
			return ParseNumbers.StringToLong(s, radix, flags, null);
		}

		// Token: 0x06000C80 RID: 3200
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe static extern long StringToLong(string s, int radix, int flags, int* currPos);

		// Token: 0x06000C81 RID: 3201 RVA: 0x00025848 File Offset: 0x00024848
		public unsafe static long StringToLong(string s, int radix, int flags, ref int currPos)
		{
			fixed (int* ptr = &currPos)
			{
				return ParseNumbers.StringToLong(s, radix, flags, ptr);
			}
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x00025865 File Offset: 0x00024865
		public static int StringToInt(string s, int radix, int flags)
		{
			return ParseNumbers.StringToInt(s, radix, flags, null);
		}

		// Token: 0x06000C83 RID: 3203
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe static extern int StringToInt(string s, int radix, int flags, int* currPos);

		// Token: 0x06000C84 RID: 3204 RVA: 0x00025874 File Offset: 0x00024874
		public unsafe static int StringToInt(string s, int radix, int flags, ref int currPos)
		{
			fixed (int* ptr = &currPos)
			{
				return ParseNumbers.StringToInt(s, radix, flags, ptr);
			}
		}

		// Token: 0x06000C85 RID: 3205
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string IntToString(int l, int radix, int width, char paddingChar, int flags);

		// Token: 0x06000C86 RID: 3206
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string LongToString(long l, int radix, int width, char paddingChar, int flags);

		// Token: 0x04000473 RID: 1139
		internal const int PrintAsI1 = 64;

		// Token: 0x04000474 RID: 1140
		internal const int PrintAsI2 = 128;

		// Token: 0x04000475 RID: 1141
		internal const int PrintAsI4 = 256;

		// Token: 0x04000476 RID: 1142
		internal const int TreatAsUnsigned = 512;

		// Token: 0x04000477 RID: 1143
		internal const int TreatAsI1 = 1024;

		// Token: 0x04000478 RID: 1144
		internal const int TreatAsI2 = 2048;

		// Token: 0x04000479 RID: 1145
		internal const int IsTight = 4096;

		// Token: 0x0400047A RID: 1146
		internal const int NoSpace = 8192;
	}
}
