using System;
using System.Collections.Specialized;
using System.Text;

namespace System.Diagnostics
{
	// Token: 0x02000778 RID: 1912
	internal static class EnvironmentBlock
	{
		// Token: 0x06003B1A RID: 15130 RVA: 0x000FB9F8 File Offset: 0x000FA9F8
		public static byte[] ToByteArray(StringDictionary sd, bool unicode)
		{
			string[] array = new string[sd.Count];
			sd.Keys.CopyTo(array, 0);
			string[] array2 = new string[sd.Count];
			sd.Values.CopyTo(array2, 0);
			Array.Sort(array, array2, OrdinalCaseInsensitiveComparer.Default);
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < sd.Count; i++)
			{
				stringBuilder.Append(array[i]);
				stringBuilder.Append('=');
				stringBuilder.Append(array2[i]);
				stringBuilder.Append('\0');
			}
			stringBuilder.Append('\0');
			byte[] bytes;
			if (unicode)
			{
				bytes = Encoding.Unicode.GetBytes(stringBuilder.ToString());
			}
			else
			{
				bytes = Encoding.Default.GetBytes(stringBuilder.ToString());
			}
			if (bytes.Length > 65535)
			{
				throw new InvalidOperationException(SR.GetString("EnvironmentBlockTooLong", new object[]
				{
					bytes.Length
				}));
			}
			return bytes;
		}
	}
}
