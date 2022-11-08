using System;
using System.IO;

namespace System.Windows.Forms
{
	// Token: 0x020005CA RID: 1482
	internal static class AutomationMessages
	{
		// Token: 0x06004DFA RID: 19962 RVA: 0x0011FE80 File Offset: 0x0011EE80
		public static IntPtr WriteAutomationText(string text)
		{
			IntPtr zero = IntPtr.Zero;
			string text2 = AutomationMessages.GenerateLogFileName(ref zero);
			if (text2 != null)
			{
				try
				{
					FileStream fileStream = new FileStream(text2, FileMode.Create, FileAccess.Write);
					StreamWriter streamWriter = new StreamWriter(fileStream);
					streamWriter.WriteLine(text);
					streamWriter.Dispose();
					fileStream.Dispose();
				}
				catch
				{
					zero = IntPtr.Zero;
				}
			}
			return zero;
		}

		// Token: 0x06004DFB RID: 19963 RVA: 0x0011FEE0 File Offset: 0x0011EEE0
		public static string ReadAutomationText(IntPtr fileId)
		{
			string result = null;
			if (fileId != IntPtr.Zero)
			{
				string path = AutomationMessages.GenerateLogFileName(ref fileId);
				if (File.Exists(path))
				{
					try
					{
						FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
						StreamReader streamReader = new StreamReader(fileStream);
						result = streamReader.ReadToEnd();
						streamReader.Dispose();
						fileStream.Dispose();
					}
					catch
					{
						result = null;
					}
				}
			}
			return result;
		}

		// Token: 0x06004DFC RID: 19964 RVA: 0x0011FF48 File Offset: 0x0011EF48
		private static string GenerateLogFileName(ref IntPtr fileId)
		{
			string result = null;
			string environmentVariable = Environment.GetEnvironmentVariable("TEMP");
			if (environmentVariable != null)
			{
				if (fileId == IntPtr.Zero)
				{
					Random random = new Random(DateTime.Now.Millisecond);
					fileId = new IntPtr(random.Next());
				}
				result = string.Concat(new object[]
				{
					environmentVariable,
					"\\Maui",
					fileId,
					".log"
				});
			}
			return result;
		}

		// Token: 0x04003289 RID: 12937
		private const int WM_USER = 1024;

		// Token: 0x0400328A RID: 12938
		internal const int PGM_GETBUTTONCOUNT = 1104;

		// Token: 0x0400328B RID: 12939
		internal const int PGM_GETBUTTONSTATE = 1106;

		// Token: 0x0400328C RID: 12940
		internal const int PGM_SETBUTTONSTATE = 1105;

		// Token: 0x0400328D RID: 12941
		internal const int PGM_GETBUTTONTEXT = 1107;

		// Token: 0x0400328E RID: 12942
		internal const int PGM_GETBUTTONTOOLTIPTEXT = 1108;

		// Token: 0x0400328F RID: 12943
		internal const int PGM_GETROWCOORDS = 1109;

		// Token: 0x04003290 RID: 12944
		internal const int PGM_GETVISIBLEROWCOUNT = 1110;

		// Token: 0x04003291 RID: 12945
		internal const int PGM_GETSELECTEDROW = 1111;

		// Token: 0x04003292 RID: 12946
		internal const int PGM_SETSELECTEDTAB = 1112;

		// Token: 0x04003293 RID: 12947
		internal const int PGM_GETTESTINGINFO = 1113;
	}
}
