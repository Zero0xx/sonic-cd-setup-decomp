using System;

namespace System.Net
{
	// Token: 0x020004B6 RID: 1206
	internal static class ChunkParse
	{
		// Token: 0x0600254C RID: 9548 RVA: 0x00094CFC File Offset: 0x00093CFC
		internal static int SkipPastCRLF(IReadChunkBytes Source)
		{
			int num = 0;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			int nextByte = Source.NextByte;
			num++;
			while (nextByte != -1)
			{
				if (flag3)
				{
					if (nextByte != 10)
					{
						return 0;
					}
					if (flag)
					{
						return 0;
					}
					if (!flag2)
					{
						return num;
					}
					flag4 = true;
					flag = true;
					flag3 = false;
				}
				else if (flag4)
				{
					if (nextByte != 32 && nextByte != 9)
					{
						return 0;
					}
					flag = true;
					flag4 = false;
				}
				if (!flag)
				{
					int num2 = nextByte;
					if (num2 <= 13)
					{
						if (num2 == 10)
						{
							return 0;
						}
						if (num2 == 13)
						{
							flag3 = true;
						}
					}
					else if (num2 != 34)
					{
						if (num2 == 92)
						{
							if (flag2)
							{
								flag = true;
							}
						}
					}
					else
					{
						flag2 = !flag2;
					}
				}
				else
				{
					flag = false;
				}
				nextByte = Source.NextByte;
				num++;
			}
			return -1;
		}

		// Token: 0x0600254D RID: 9549 RVA: 0x00094DB0 File Offset: 0x00093DB0
		internal static int GetChunkSize(IReadChunkBytes Source, out int chunkSize)
		{
			int num = 0;
			int num2 = Source.NextByte;
			int num3 = 0;
			if (num2 == 10 || num2 == 13)
			{
				num3++;
				num2 = Source.NextByte;
			}
			while (num2 != -1)
			{
				if (num2 >= 48 && num2 <= 57)
				{
					num2 -= 48;
				}
				else
				{
					if (num2 >= 97 && num2 <= 102)
					{
						num2 -= 97;
					}
					else
					{
						if (num2 < 65 || num2 > 70)
						{
							Source.NextByte = num2;
							chunkSize = num;
							return num3;
						}
						num2 -= 65;
					}
					num2 += 10;
				}
				num *= 16;
				num += num2;
				num3++;
				num2 = Source.NextByte;
			}
			chunkSize = num;
			return -1;
		}
	}
}
