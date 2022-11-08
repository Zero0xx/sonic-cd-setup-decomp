using System;

namespace System
{
	// Token: 0x02000361 RID: 865
	internal class IPv4AddressHelper
	{
		// Token: 0x06001B93 RID: 7059 RVA: 0x00067C30 File Offset: 0x00066C30
		private IPv4AddressHelper()
		{
		}

		// Token: 0x06001B94 RID: 7060 RVA: 0x00067C38 File Offset: 0x00066C38
		internal unsafe static string ParseCanonicalName(string str, int start, int end, ref bool isLoopback)
		{
			byte* ptr = stackalloc byte[1 * 4];
			isLoopback = IPv4AddressHelper.Parse(str, ptr, start, end);
			return string.Concat(new object[]
			{
				*ptr,
				".",
				ptr[1],
				".",
				ptr[2],
				".",
				ptr[3]
			});
		}

		// Token: 0x06001B95 RID: 7061 RVA: 0x00067CAC File Offset: 0x00066CAC
		internal unsafe static int ParseHostNumber(string str, int start, int end)
		{
			byte* ptr = stackalloc byte[1 * 4];
			IPv4AddressHelper.Parse(str, ptr, start, end);
			return ((int)(*ptr) << 24) + ((int)ptr[1] << 16) + ((int)ptr[2] << 8) + (int)ptr[3];
		}

		// Token: 0x06001B96 RID: 7062 RVA: 0x00067CE8 File Offset: 0x00066CE8
		internal unsafe static bool IsValid(char* name, int start, ref int end, bool allowIPv6, bool notImplicitFile)
		{
			int num = 0;
			int num2 = 0;
			bool flag = false;
			while (start < end)
			{
				char c = name[start];
				if (allowIPv6)
				{
					if (c == ']' || c == '/')
					{
						break;
					}
					if (c == '%')
					{
						break;
					}
				}
				else if (c == '/' || c == '\\' || (notImplicitFile && (c == ':' || c == '?' || c == '#')))
				{
					break;
				}
				if (c <= '9' && c >= '0')
				{
					flag = true;
					num2 = num2 * 10 + (int)(name[start] - '0');
					if (num2 > 255)
					{
						return false;
					}
				}
				else
				{
					if (c != '.')
					{
						return false;
					}
					if (!flag)
					{
						return false;
					}
					num++;
					flag = false;
					num2 = 0;
				}
				start++;
			}
			bool flag2 = num == 3 && flag;
			if (flag2)
			{
				end = start;
			}
			return flag2;
		}

		// Token: 0x06001B97 RID: 7063 RVA: 0x00067D90 File Offset: 0x00066D90
		private unsafe static bool Parse(string name, byte* numbers, int start, int end)
		{
			for (int i = 0; i < 4; i++)
			{
				byte b = 0;
				char c;
				while (start < end && (c = name[start]) != '.' && c != ':')
				{
					b = b * 10 + (byte)(c - '0');
					start++;
				}
				numbers[i] = b;
				start++;
			}
			return *numbers == 127;
		}

		// Token: 0x04001C2C RID: 7212
		private const int NumberOfLabels = 4;
	}
}
