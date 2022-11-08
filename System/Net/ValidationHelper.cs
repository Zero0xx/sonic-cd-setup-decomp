using System;
using System.Globalization;

namespace System.Net
{
	// Token: 0x020003EF RID: 1007
	internal static class ValidationHelper
	{
		// Token: 0x06002086 RID: 8326 RVA: 0x000807D0 File Offset: 0x0007F7D0
		public static string[] MakeEmptyArrayNull(string[] stringArray)
		{
			if (stringArray == null || stringArray.Length == 0)
			{
				return null;
			}
			return stringArray;
		}

		// Token: 0x06002087 RID: 8327 RVA: 0x000807DD File Offset: 0x0007F7DD
		public static string MakeStringNull(string stringValue)
		{
			if (stringValue == null || stringValue.Length == 0)
			{
				return null;
			}
			return stringValue;
		}

		// Token: 0x06002088 RID: 8328 RVA: 0x000807ED File Offset: 0x0007F7ED
		public static string ExceptionMessage(Exception exception)
		{
			if (exception == null)
			{
				return string.Empty;
			}
			if (exception.InnerException == null)
			{
				return exception.Message;
			}
			return exception.Message + " (" + ValidationHelper.ExceptionMessage(exception.InnerException) + ")";
		}

		// Token: 0x06002089 RID: 8329 RVA: 0x00080828 File Offset: 0x0007F828
		public static string ToString(object objectValue)
		{
			if (objectValue == null)
			{
				return "(null)";
			}
			if (objectValue is string && ((string)objectValue).Length == 0)
			{
				return "(string.empty)";
			}
			if (objectValue is Exception)
			{
				return ValidationHelper.ExceptionMessage(objectValue as Exception);
			}
			if (objectValue is IntPtr)
			{
				return "0x" + ((IntPtr)objectValue).ToString("x");
			}
			return objectValue.ToString();
		}

		// Token: 0x0600208A RID: 8330 RVA: 0x0008089C File Offset: 0x0007F89C
		public static string HashString(object objectValue)
		{
			if (objectValue == null)
			{
				return "(null)";
			}
			if (objectValue is string && ((string)objectValue).Length == 0)
			{
				return "(string.empty)";
			}
			return objectValue.GetHashCode().ToString(NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x0600208B RID: 8331 RVA: 0x000808E0 File Offset: 0x0007F8E0
		public static bool IsInvalidHttpString(string stringValue)
		{
			return stringValue.IndexOfAny(ValidationHelper.InvalidParamChars) != -1;
		}

		// Token: 0x0600208C RID: 8332 RVA: 0x000808F3 File Offset: 0x0007F8F3
		public static bool IsBlankString(string stringValue)
		{
			return stringValue == null || stringValue.Length == 0;
		}

		// Token: 0x0600208D RID: 8333 RVA: 0x00080903 File Offset: 0x0007F903
		public static bool ValidateTcpPort(int port)
		{
			return port >= 0 && port <= 65535;
		}

		// Token: 0x0600208E RID: 8334 RVA: 0x00080916 File Offset: 0x0007F916
		public static bool ValidateRange(int actual, int fromAllowed, int toAllowed)
		{
			return actual >= fromAllowed && actual <= toAllowed;
		}

		// Token: 0x0600208F RID: 8335 RVA: 0x00080925 File Offset: 0x0007F925
		internal static void ValidateSegment(ArraySegment<byte> segment)
		{
			if (segment.Offset < 0 || segment.Count < 0 || segment.Count > segment.Array.Length - segment.Offset)
			{
				throw new ArgumentOutOfRangeException("segment");
			}
		}

		// Token: 0x04001FD0 RID: 8144
		public static string[] EmptyArray = new string[0];

		// Token: 0x04001FD1 RID: 8145
		internal static readonly char[] InvalidMethodChars = new char[]
		{
			' ',
			'\r',
			'\n',
			'\t'
		};

		// Token: 0x04001FD2 RID: 8146
		internal static readonly char[] InvalidParamChars = new char[]
		{
			'(',
			')',
			'<',
			'>',
			'@',
			',',
			';',
			':',
			'\\',
			'"',
			'\'',
			'/',
			'[',
			']',
			'?',
			'=',
			'{',
			'}',
			' ',
			'\t',
			'\r',
			'\n'
		};
	}
}
