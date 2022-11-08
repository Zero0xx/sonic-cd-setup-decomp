using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x02000079 RID: 121
	[ComVisible(true)]
	public static class Buffer
	{
		// Token: 0x060006C4 RID: 1732
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void BlockCopy(Array src, int srcOffset, Array dst, int dstOffset, int count);

		// Token: 0x060006C5 RID: 1733
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void InternalBlockCopy(Array src, int srcOffset, Array dst, int dstOffset, int count);

		// Token: 0x060006C6 RID: 1734 RVA: 0x00016824 File Offset: 0x00015824
		internal unsafe static int IndexOfByte(byte* src, byte value, int index, int count)
		{
			byte* ptr = src + index;
			while ((ptr & 3) != 0)
			{
				if (count == 0)
				{
					return -1;
				}
				if (*ptr == value)
				{
					return (int)((long)(ptr - src));
				}
				count--;
				ptr++;
			}
			uint num = (uint)(((int)value << 8) + (int)value);
			num = (num << 16) + num;
			while (count > 3)
			{
				uint num2 = *(uint*)ptr;
				num2 ^= num;
				uint num3 = 2130640639U + num2;
				num2 ^= uint.MaxValue;
				num2 ^= num3;
				num2 &= 2164326656U;
				if (num2 != 0U)
				{
					int num4 = (int)((long)(ptr - src));
					if (*ptr == value)
					{
						return num4;
					}
					if (ptr[1] == value)
					{
						return num4 + 1;
					}
					if (ptr[2] == value)
					{
						return num4 + 2;
					}
					if (ptr[3] == value)
					{
						return num4 + 3;
					}
				}
				count -= 4;
				ptr += 4;
			}
			while (count > 0)
			{
				if (*ptr == value)
				{
					return (int)((long)(ptr - src));
				}
				count--;
				ptr++;
			}
			return -1;
		}

		// Token: 0x060006C7 RID: 1735
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern byte GetByte(Array array, int index);

		// Token: 0x060006C8 RID: 1736
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetByte(Array array, int index, byte value);

		// Token: 0x060006C9 RID: 1737
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int ByteLength(Array array);

		// Token: 0x060006CA RID: 1738 RVA: 0x000168EE File Offset: 0x000158EE
		internal unsafe static void ZeroMemory(byte* src, long len)
		{
			for (;;)
			{
				long num = len;
				len = num - 1L;
				if (num <= 0L)
				{
					break;
				}
				src[len] = 0;
			}
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x00016904 File Offset: 0x00015904
		internal unsafe static void memcpy(byte* src, int srcIndex, byte[] dest, int destIndex, int len)
		{
			if (len == 0)
			{
				return;
			}
			fixed (byte* ptr = dest)
			{
				Buffer.memcpyimpl(src + srcIndex, ptr + destIndex, len);
			}
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x00016940 File Offset: 0x00015940
		internal unsafe static void memcpy(byte[] src, int srcIndex, byte* pDest, int destIndex, int len)
		{
			if (len == 0)
			{
				return;
			}
			fixed (byte* ptr = src)
			{
				Buffer.memcpyimpl(ptr + srcIndex, pDest + destIndex, len);
			}
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x0001697A File Offset: 0x0001597A
		internal unsafe static void memcpy(char* pSrc, int srcIndex, char* pDest, int destIndex, int len)
		{
			if (len == 0)
			{
				return;
			}
			Buffer.memcpyimpl((byte*)(pSrc + srcIndex), (byte*)(pDest + destIndex), len * 2);
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x00016998 File Offset: 0x00015998
		internal unsafe static void memcpyimpl(byte* src, byte* dest, int len)
		{
			if (len >= 16)
			{
				do
				{
					*(long*)dest = *(long*)src;
					*(long*)(dest + 8) = *(long*)(src + 8);
					dest += 16;
					src += 16;
				}
				while ((len -= 16) >= 16);
			}
			if (len > 0)
			{
				if ((len & 8) != 0)
				{
					*(long*)dest = *(long*)src;
					dest += 8;
					src += 8;
				}
				if ((len & 4) != 0)
				{
					*(int*)dest = *(int*)src;
					dest += 4;
					src += 4;
				}
				if ((len & 2) != 0)
				{
					*(short*)dest = *(short*)src;
					dest += 2;
					src += 2;
				}
				if ((len & 1) != 0)
				{
					*(dest++) = *(src++);
				}
			}
		}
	}
}
