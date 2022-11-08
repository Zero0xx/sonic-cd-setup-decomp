using System;

namespace System.Xml
{
	// Token: 0x02000064 RID: 100
	internal class TernaryTreeReadOnly
	{
		// Token: 0x0600037B RID: 891 RVA: 0x00011B69 File Offset: 0x00010B69
		public TernaryTreeReadOnly(byte[] nodeBuffer)
		{
			this.nodeBuffer = nodeBuffer;
		}

		// Token: 0x0600037C RID: 892 RVA: 0x00011B78 File Offset: 0x00010B78
		public byte FindCaseInsensitiveString(string stringToFind)
		{
			int num = 0;
			int num2 = 0;
			byte[] array = this.nodeBuffer;
			int num3 = (int)stringToFind[num];
			if (num3 > 122)
			{
				return 0;
			}
			if (num3 >= 97)
			{
				num3 -= 32;
			}
			int num4;
			for (;;)
			{
				num4 = num2 * 4;
				int num5 = (int)array[num4];
				if (num3 < num5)
				{
					if (array[num4 + 1] == 0)
					{
						return 0;
					}
					num2 += (int)array[num4 + 1];
				}
				else if (num3 > num5)
				{
					if (array[num4 + 2] == 0)
					{
						return 0;
					}
					num2 += (int)array[num4 + 2];
				}
				else
				{
					if (num3 == 0)
					{
						break;
					}
					num2++;
					num++;
					if (num == stringToFind.Length)
					{
						num3 = 0;
					}
					else
					{
						num3 = (int)stringToFind[num];
						if (num3 > 122)
						{
							return 0;
						}
						if (num3 >= 97)
						{
							num3 -= 32;
						}
					}
				}
			}
			return array[num4 + 3];
		}

		// Token: 0x040005BC RID: 1468
		private byte[] nodeBuffer;
	}
}
