using System;
using System.Globalization;
using System.Net;

namespace System
{
	// Token: 0x02000360 RID: 864
	internal class DomainNameHelper
	{
		// Token: 0x06001B84 RID: 7044 RVA: 0x000674DF File Offset: 0x000664DF
		private DomainNameHelper()
		{
		}

		// Token: 0x06001B85 RID: 7045 RVA: 0x000674E8 File Offset: 0x000664E8
		internal static string ParseCanonicalName(string str, int start, int end, ref bool loopback)
		{
			string text = null;
			for (int i = end - 1; i >= start; i--)
			{
				if (str[i] >= 'A' && str[i] <= 'Z')
				{
					text = str.Substring(start, end - start).ToLower(CultureInfo.InvariantCulture);
					break;
				}
				if (str[i] == ':')
				{
					end = i;
				}
			}
			if (text == null)
			{
				text = str.Substring(start, end - start);
			}
			if (text == "localhost" || text == "loopback")
			{
				loopback = true;
				return "localhost";
			}
			return text;
		}

		// Token: 0x06001B86 RID: 7046 RVA: 0x00067574 File Offset: 0x00066574
		internal unsafe static bool IsValid(char* name, ushort pos, ref int returnedEnd, ref bool notCanonical, bool notImplicitFile)
		{
			char* ptr = name + pos;
			char* ptr2 = ptr;
			char* ptr3 = name + returnedEnd;
			while (ptr2 < ptr3)
			{
				char c = *ptr2;
				if (c > '\u007f')
				{
					return false;
				}
				if (c == '/' || c == '\\' || (notImplicitFile && (c == ':' || c == '?' || c == '#')))
				{
					ptr3 = ptr2;
					break;
				}
				ptr2++;
			}
			if (ptr3 == ptr)
			{
				return false;
			}
			for (;;)
			{
				ptr2 = ptr;
				while (ptr2 < ptr3 && *ptr2 != '.')
				{
					ptr2++;
				}
				if (ptr == ptr2 || (long)(ptr2 - ptr) > 63L || !DomainNameHelper.IsASCIILetterOrDigit(*(ptr++), ref notCanonical))
				{
					break;
				}
				while (ptr < ptr2)
				{
					if (!DomainNameHelper.IsValidDomainLabelCharacter(*(ptr++), ref notCanonical))
					{
						return false;
					}
				}
				ptr++;
				if (ptr >= ptr3)
				{
					goto Block_13;
				}
			}
			return false;
			Block_13:
			returnedEnd = (int)((ushort)((long)(ptr3 - name)));
			return true;
		}

		// Token: 0x06001B87 RID: 7047 RVA: 0x0006762C File Offset: 0x0006662C
		internal unsafe static bool IsValidByIri(char* name, ushort pos, ref int returnedEnd, ref bool notCanonical, bool notImplicitFile)
		{
			char* ptr = name + pos;
			char* ptr2 = ptr;
			char* ptr3 = name + returnedEnd;
			while (ptr2 < ptr3)
			{
				char c = *ptr2;
				if (c == '/' || c == '\\' || (notImplicitFile && (c == ':' || c == '?' || c == '#')))
				{
					ptr3 = ptr2;
					break;
				}
				ptr2++;
			}
			if (ptr3 == ptr)
			{
				return false;
			}
			for (;;)
			{
				ptr2 = ptr;
				int num = 0;
				bool flag = false;
				while (ptr2 < ptr3 && *ptr2 != '.' && *ptr2 != '。' && *ptr2 != '．' && *ptr2 != '｡')
				{
					num++;
					if (*ptr2 > 'ÿ')
					{
						num++;
					}
					if (*ptr2 >= '\u00a0')
					{
						flag = true;
					}
					ptr2++;
				}
				if (ptr == ptr2 || (flag ? (num + 4) : num) > 63 || (*(ptr++) < '\u00a0' && !DomainNameHelper.IsASCIILetterOrDigit(*(ptr - 1), ref notCanonical)))
				{
					break;
				}
				while (ptr < ptr2)
				{
					if (*(ptr++) < '\u00a0' && !DomainNameHelper.IsValidDomainLabelCharacter(*(ptr - 1), ref notCanonical))
					{
						return false;
					}
				}
				ptr++;
				if (ptr >= ptr3)
				{
					goto Block_20;
				}
			}
			return false;
			Block_20:
			returnedEnd = (int)((ushort)((long)(ptr3 - name)));
			return true;
		}

		// Token: 0x06001B88 RID: 7048 RVA: 0x0006773D File Offset: 0x0006673D
		private static bool IsASCIILetter(char character, ref bool notCanonical)
		{
			if (character >= 'a' && character <= 'z')
			{
				return true;
			}
			if (character >= 'A' && character <= 'Z')
			{
				if (!notCanonical)
				{
					notCanonical = true;
				}
				return true;
			}
			return false;
		}

		// Token: 0x06001B89 RID: 7049 RVA: 0x00067760 File Offset: 0x00066760
		internal unsafe static string IdnEquivalent(char* hostname, int start, int end, ref bool allAscii, ref bool atLeastOneValidIdn)
		{
			string text = null;
			string text2 = DomainNameHelper.IdnEquivalent(hostname, start, end, ref allAscii, ref text);
			if (text2 != null)
			{
				string text3 = allAscii ? text2 : text;
				fixed (char* ptr = text3)
				{
					int length = text3.Length;
					int i = 0;
					int num = 0;
					bool flag = false;
					do
					{
						bool flag2 = false;
						bool flag3 = false;
						flag = false;
						for (i = num; i < length; i++)
						{
							char c = ptr[i];
							if (!flag3)
							{
								flag3 = true;
								if (i + 3 < length && DomainNameHelper.IsIdnAce(ptr, i))
								{
									i += 4;
									flag2 = true;
									continue;
								}
							}
							if (c == '.' || c == '。' || c == '．' || c == '｡')
							{
								flag = true;
								break;
							}
						}
						if (flag2)
						{
							try
							{
								IdnMapping idnMapping = new IdnMapping();
								idnMapping.GetUnicode(new string(ptr, num, i - num));
								atLeastOneValidIdn = true;
								break;
							}
							catch (ArgumentException)
							{
							}
						}
						num = i + (flag ? 1 : 0);
					}
					while (num < length);
				}
			}
			else
			{
				atLeastOneValidIdn = false;
			}
			return text2;
		}

		// Token: 0x06001B8A RID: 7050 RVA: 0x0006787C File Offset: 0x0006687C
		internal unsafe static string IdnEquivalent(char* hostname, int start, int end, ref bool allAscii, ref string bidiStrippedHost)
		{
			string result = null;
			if (end <= start)
			{
				return result;
			}
			int i = start;
			allAscii = true;
			while (i < end)
			{
				if (hostname[i] > '\u007f')
				{
					allAscii = false;
					break;
				}
				i++;
			}
			if (!allAscii)
			{
				IdnMapping idnMapping = new IdnMapping();
				bidiStrippedHost = Uri.StripBidiControlCharacter(hostname, start, end - start);
				string ascii;
				try
				{
					ascii = idnMapping.GetAscii(bidiStrippedHost);
					if (!ServicePointManager.AllowDangerousUnicodeDecompositions && DomainNameHelper.ContainsCharactersUnsafeForNormalizedHost(ascii))
					{
						throw new UriFormatException("net_uri_BadUnicodeHostForIdn");
					}
				}
				catch (ArgumentException)
				{
					throw new UriFormatException(SR.GetString("net_uri_BadUnicodeHostForIdn"));
				}
				return ascii;
			}
			string text = new string(hostname, start, end - start);
			if (text == null)
			{
				return null;
			}
			return text.ToLowerInvariant();
		}

		// Token: 0x06001B8B RID: 7051 RVA: 0x0006792C File Offset: 0x0006692C
		private static bool IsIdnAce(string input, int index)
		{
			return input[index] == 'x' && input[index + 1] == 'n' && input[index + 2] == '-' && input[index + 3] == '-';
		}

		// Token: 0x06001B8C RID: 7052 RVA: 0x00067963 File Offset: 0x00066963
		private unsafe static bool IsIdnAce(char* input, int index)
		{
			return input[index] == 'x' && input[index + 1] == 'n' && input[index + 2] == '-' && input[index + 3] == '-';
		}

		// Token: 0x06001B8D RID: 7053 RVA: 0x0006799C File Offset: 0x0006699C
		internal unsafe static string UnicodeEquivalent(string idnHost, char* hostname, int start, int end)
		{
			IdnMapping idnMapping = new IdnMapping();
			try
			{
				return idnMapping.GetUnicode(idnHost);
			}
			catch (ArgumentException)
			{
			}
			bool flag = true;
			return DomainNameHelper.UnicodeEquivalent(hostname, start, end, ref flag, ref flag);
		}

		// Token: 0x06001B8E RID: 7054 RVA: 0x000679DC File Offset: 0x000669DC
		internal unsafe static string UnicodeEquivalent(char* hostname, int start, int end, ref bool allAscii, ref bool atLeastOneValidIdn)
		{
			IdnMapping idnMapping = new IdnMapping();
			allAscii = true;
			atLeastOneValidIdn = false;
			string result = null;
			if (end <= start)
			{
				return result;
			}
			string text = Uri.StripBidiControlCharacter(hostname, start, end - start);
			string text2 = null;
			int num = 0;
			int i = 0;
			int length = text.Length;
			bool flag = false;
			do
			{
				bool flag2 = true;
				bool flag3 = false;
				bool flag4 = false;
				flag = false;
				for (i = num; i < length; i++)
				{
					char c = text[i];
					if (!flag4)
					{
						flag4 = true;
						if (i + 3 < length && c == 'x' && DomainNameHelper.IsIdnAce(text, i))
						{
							flag3 = true;
						}
					}
					if (flag2 && c > '\u007f')
					{
						flag2 = false;
						allAscii = false;
					}
					if (c == '.' || c == '。' || c == '．' || c == '｡')
					{
						flag = true;
						break;
					}
				}
				if (!flag2)
				{
					string text3 = text.Substring(num, i - num);
					try
					{
						text3 = idnMapping.GetAscii(text3);
					}
					catch (ArgumentException)
					{
						throw new UriFormatException(SR.GetString("net_uri_BadUnicodeHostForIdn"));
					}
					text2 += idnMapping.GetUnicode(text3);
					if (flag)
					{
						text2 += ".";
					}
				}
				else
				{
					bool flag5 = false;
					if (flag3)
					{
						try
						{
							text2 += idnMapping.GetUnicode(text.Substring(num, i - num));
							if (flag)
							{
								text2 += ".";
							}
							flag5 = true;
							atLeastOneValidIdn = true;
						}
						catch (ArgumentException)
						{
						}
					}
					if (!flag5)
					{
						text2 += text.Substring(num, i - num).ToLowerInvariant();
						if (flag)
						{
							text2 += ".";
						}
					}
				}
				num = i + (flag ? 1 : 0);
			}
			while (num < length);
			return text2;
		}

		// Token: 0x06001B8F RID: 7055 RVA: 0x00067B94 File Offset: 0x00066B94
		private static bool IsASCIILetterOrDigit(char character, ref bool notCanonical)
		{
			if ((character >= 'a' && character <= 'z') || (character >= '0' && character <= '9'))
			{
				return true;
			}
			if (character >= 'A' && character <= 'Z')
			{
				notCanonical = true;
				return true;
			}
			return false;
		}

		// Token: 0x06001B90 RID: 7056 RVA: 0x00067BBC File Offset: 0x00066BBC
		private static bool IsValidDomainLabelCharacter(char character, ref bool notCanonical)
		{
			if ((character >= 'a' && character <= 'z') || (character >= '0' && character <= '9') || character == '-' || character == '_')
			{
				return true;
			}
			if (character >= 'A' && character <= 'Z')
			{
				notCanonical = true;
				return true;
			}
			return false;
		}

		// Token: 0x06001B91 RID: 7057 RVA: 0x00067BEE File Offset: 0x00066BEE
		internal static bool ContainsCharactersUnsafeForNormalizedHost(string host)
		{
			return host.IndexOfAny(DomainNameHelper.s_UnsafeForNormalizedHost) != -1;
		}

		// Token: 0x04001C28 RID: 7208
		private const char c_DummyChar = '￿';

		// Token: 0x04001C29 RID: 7209
		internal const string Localhost = "localhost";

		// Token: 0x04001C2A RID: 7210
		internal const string Loopback = "loopback";

		// Token: 0x04001C2B RID: 7211
		private static readonly char[] s_UnsafeForNormalizedHost = new char[]
		{
			'\\',
			'/',
			'?',
			'@',
			'#',
			':',
			'[',
			']'
		};
	}
}
