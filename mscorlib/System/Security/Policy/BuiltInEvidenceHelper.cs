using System;

namespace System.Security.Policy
{
	// Token: 0x020004A4 RID: 1188
	internal static class BuiltInEvidenceHelper
	{
		// Token: 0x06002F2D RID: 12077 RVA: 0x0009FE52 File Offset: 0x0009EE52
		internal static void CopyIntToCharArray(int value, char[] buffer, int position)
		{
			buffer[position] = (char)(value >> 16 & 65535);
			buffer[position + 1] = (char)(value & 65535);
		}

		// Token: 0x06002F2E RID: 12078 RVA: 0x0009FE70 File Offset: 0x0009EE70
		internal static int GetIntFromCharArray(char[] buffer, int position)
		{
			int num = (int)buffer[position];
			num <<= 16;
			return num + (int)buffer[position + 1];
		}

		// Token: 0x06002F2F RID: 12079 RVA: 0x0009FE90 File Offset: 0x0009EE90
		internal static void CopyLongToCharArray(long value, char[] buffer, int position)
		{
			buffer[position] = (char)(value >> 48 & 65535L);
			buffer[position + 1] = (char)(value >> 32 & 65535L);
			buffer[position + 2] = (char)(value >> 16 & 65535L);
			buffer[position + 3] = (char)(value & 65535L);
		}

		// Token: 0x06002F30 RID: 12080 RVA: 0x0009FEDC File Offset: 0x0009EEDC
		internal static long GetLongFromCharArray(char[] buffer, int position)
		{
			long num = (long)((ulong)buffer[position]);
			num <<= 16;
			num += (long)((ulong)buffer[position + 1]);
			num <<= 16;
			num += (long)((ulong)buffer[position + 2]);
			num <<= 16;
			return num + (long)((ulong)buffer[position + 3]);
		}

		// Token: 0x040017FC RID: 6140
		internal const char idApplicationDirectory = '\0';

		// Token: 0x040017FD RID: 6141
		internal const char idPublisher = '\u0001';

		// Token: 0x040017FE RID: 6142
		internal const char idStrongName = '\u0002';

		// Token: 0x040017FF RID: 6143
		internal const char idZone = '\u0003';

		// Token: 0x04001800 RID: 6144
		internal const char idUrl = '\u0004';

		// Token: 0x04001801 RID: 6145
		internal const char idWebPage = '\u0005';

		// Token: 0x04001802 RID: 6146
		internal const char idSite = '\u0006';

		// Token: 0x04001803 RID: 6147
		internal const char idPermissionRequestEvidence = '\a';

		// Token: 0x04001804 RID: 6148
		internal const char idHash = '\b';

		// Token: 0x04001805 RID: 6149
		internal const char idGac = '\t';
	}
}
