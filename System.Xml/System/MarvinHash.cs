using System;
using System.Reflection;
using System.Security.Cryptography;

namespace System
{
	// Token: 0x02000022 RID: 34
	internal static class MarvinHash
	{
		// Token: 0x060000A7 RID: 167 RVA: 0x00003FB0 File Offset: 0x00002FB0
		private static bool IsItanium()
		{
			PortableExecutableKinds portableExecutableKinds;
			ImageFileMachine imageFileMachine;
			typeof(object).Module.GetPEKind(out portableExecutableKinds, out imageFileMachine);
			return imageFileMachine == ImageFileMachine.IA64;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00003FE0 File Offset: 0x00002FE0
		public unsafe static int ComputeHash32(string key, ulong seed)
		{
			int result;
			fixed (char* data = key)
			{
				result = MarvinHash.ComputeHash32((byte*)data, 2 * key.Length, seed);
			}
			return result;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00004010 File Offset: 0x00003010
		public unsafe static int ComputeHash32(char[] key, int start, int len, ulong seed)
		{
			int result;
			fixed (char* ptr = &key[start])
			{
				result = MarvinHash.ComputeHash32((byte*)ptr, 2 * len, seed);
			}
			return result;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00004038 File Offset: 0x00003038
		private unsafe static int ComputeHash32(byte* data, int count, ulong seed)
		{
			long num = MarvinHash.ComputeHash(data, count, seed);
			return (int)(num >> 32) ^ (int)num;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x0000405C File Offset: 0x0000305C
		private unsafe static long ComputeHashNonAligned(byte* data, int count, ulong seed)
		{
			uint num = (uint)count;
			uint num2 = (uint)seed;
			uint num3 = (uint)(seed >> 32);
			int num4 = 0;
			while (num >= 8U)
			{
				num2 += *(uint*)(data + num4);
				MarvinHash.Block(ref num2, ref num3);
				num2 += *(uint*)(data + num4 + 4);
				MarvinHash.Block(ref num2, ref num3);
				num4 += 8;
				num -= 8U;
			}
			switch (num)
			{
			case 0U:
				break;
			case 1U:
				goto IL_9B;
			case 2U:
				goto IL_BE;
			case 3U:
				goto IL_E1;
			case 4U:
				num2 += *(uint*)(data + num4);
				MarvinHash.Block(ref num2, ref num3);
				break;
			case 5U:
				num2 += *(uint*)(data + num4);
				num4 += 4;
				MarvinHash.Block(ref num2, ref num3);
				goto IL_9B;
			case 6U:
				num2 += *(uint*)(data + num4);
				num4 += 4;
				MarvinHash.Block(ref num2, ref num3);
				goto IL_BE;
			case 7U:
				num2 += *(uint*)(data + num4);
				num4 += 4;
				MarvinHash.Block(ref num2, ref num3);
				goto IL_E1;
			default:
				goto IL_F9;
			}
			num2 += 128U;
			goto IL_F9;
			IL_9B:
			num2 += (32768U | (uint)data[num4]);
			goto IL_F9;
			IL_BE:
			num2 += (8388608U | (uint)(*(ushort*)(data + num4)));
			goto IL_F9;
			IL_E1:
			num2 += (uint)(int.MinValue | (int)(data + num4)[2] << 16 | (int)(*(ushort*)(data + num4)));
			IL_F9:
			MarvinHash.Block(ref num2, ref num3);
			MarvinHash.Block(ref num2, ref num3);
			return (long)((ulong)num3 << 32 | (ulong)num2);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x0000417C File Offset: 0x0000317C
		private unsafe static long ComputeHashIA64(byte* data, int count, ulong seed)
		{
			uint num = (uint)count;
			uint num2 = (uint)seed;
			uint num3 = (uint)(seed >> 32);
			byte* ptr = data;
			while (num >= 4U)
			{
				num2 += (uint)((int)(*ptr) | (int)ptr[1] << 8 | (int)ptr[2] << 16 | (int)ptr[3] << 24);
				MarvinHash.Block(ref num2, ref num3);
				ptr += 4;
				num -= 4U;
			}
			switch (num)
			{
			case 0U:
				num2 += 128U;
				break;
			case 1U:
				num2 += (32768U | (uint)(*ptr));
				break;
			case 2U:
				num2 += (uint)(8388608 | (int)(*ptr) | (int)ptr[1] << 8);
				break;
			case 3U:
				num2 += (uint)(int.MinValue | (int)(*ptr) | (int)ptr[1] << 8 | (int)ptr[2] << 16);
				break;
			}
			MarvinHash.Block(ref num2, ref num3);
			MarvinHash.Block(ref num2, ref num3);
			return (long)((ulong)num3 << 32 | (ulong)num2);
		}

		// Token: 0x060000AD RID: 173 RVA: 0x0000424C File Offset: 0x0000324C
		private static void Block(ref uint rp0, ref uint rp1)
		{
			uint num = rp0;
			uint num2 = rp1;
			num2 ^= num;
			num = MarvinHash._rotl(num, 20);
			num += num2;
			num2 = MarvinHash._rotl(num2, 9);
			num2 ^= num;
			num = MarvinHash._rotl(num, 27);
			num += num2;
			num2 = MarvinHash._rotl(num2, 19);
			rp0 = num;
			rp1 = num2;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00004299 File Offset: 0x00003299
		private static uint _rotl(uint value, int shift)
		{
			return value << shift | value >> 32 - shift;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x000042AC File Offset: 0x000032AC
		public unsafe static ulong GenerateSeed()
		{
			byte[] array = new byte[8];
			ulong result;
			using (RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create())
			{
				randomNumberGenerator.GetBytes(array);
				try
				{
					fixed (byte* ptr = array)
					{
						result = (ulong)(*(long*)ptr);
					}
				}
				finally
				{
					byte* ptr = null;
				}
			}
			return result;
		}

		// Token: 0x04000484 RID: 1156
		private static readonly MarvinHash.ComputeHashImpl ComputeHash = MarvinHash.IsItanium() ? new MarvinHash.ComputeHashImpl(MarvinHash.ComputeHashIA64) : new MarvinHash.ComputeHashImpl(MarvinHash.ComputeHashNonAligned);

		// Token: 0x04000485 RID: 1157
		public static readonly ulong DefaultSeed = MarvinHash.GenerateSeed();

		// Token: 0x02000023 RID: 35
		// (Invoke) Token: 0x060000B2 RID: 178
		private unsafe delegate long ComputeHashImpl(byte* data, int count, ulong seed);
	}
}
