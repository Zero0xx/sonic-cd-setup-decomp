using System;

namespace System
{
	// Token: 0x02000363 RID: 867
	internal class UncNameHelper
	{
		// Token: 0x06001B9E RID: 7070 RVA: 0x000683F4 File Offset: 0x000673F4
		private UncNameHelper()
		{
		}

		// Token: 0x06001B9F RID: 7071 RVA: 0x000683FC File Offset: 0x000673FC
		internal static string ParseCanonicalName(string str, int start, int end, ref bool loopback)
		{
			return DomainNameHelper.ParseCanonicalName(str, start, end, ref loopback);
		}

		// Token: 0x06001BA0 RID: 7072 RVA: 0x00068408 File Offset: 0x00067408
		internal unsafe static bool IsValid(char* name, ushort start, ref int returnedEnd, bool notImplicitFile)
		{
			ushort num = (ushort)returnedEnd;
			if (start == num)
			{
				return false;
			}
			bool flag = false;
			ushort num2;
			for (num2 = start; num2 < num; num2 += 1)
			{
				if (name[num2] == '/' || name[num2] == '\\' || (notImplicitFile && (name[num2] == ':' || name[num2] == '?' || name[num2] == '#')))
				{
					num = num2;
					break;
				}
				if (name[num2] == '.')
				{
					num2 += 1;
					break;
				}
				if (char.IsLetter(name[num2]) || name[num2] == '-' || name[num2] == '_')
				{
					flag = true;
				}
				else if (name[num2] < '0' || name[num2] > '9')
				{
					return false;
				}
			}
			if (!flag)
			{
				return false;
			}
			while (num2 < num)
			{
				if (name[num2] == '/' || name[num2] == '\\' || (notImplicitFile && (name[num2] == ':' || name[num2] == '?' || name[num2] == '#')))
				{
					num = num2;
					break;
				}
				if (name[num2] == '.')
				{
					if (!flag || (num2 - 1 >= start && name[num2 - 1] == '.'))
					{
						return false;
					}
					flag = false;
				}
				else if (name[num2] == '-' || name[num2] == '_')
				{
					if (!flag)
					{
						return false;
					}
				}
				else
				{
					if (!char.IsLetter(name[num2]) && (name[num2] < '0' || name[num2] > '9'))
					{
						return false;
					}
					if (!flag)
					{
						flag = true;
					}
				}
				num2 += 1;
			}
			if (num2 - 1 >= start && name[num2 - 1] == '.')
			{
				flag = true;
			}
			if (!flag)
			{
				return false;
			}
			returnedEnd = (int)num;
			return true;
		}

		// Token: 0x04001C2F RID: 7215
		internal const int MaximumInternetNameLength = 256;
	}
}
