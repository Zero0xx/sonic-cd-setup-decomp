using System;
using System.Text;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x02000778 RID: 1912
	internal static class SoapType
	{
		// Token: 0x0600442E RID: 17454 RVA: 0x000E989C File Offset: 0x000E889C
		internal static string FilterBin64(string value)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < value.Length; i++)
			{
				if (value[i] != ' ' && value[i] != '\n' && value[i] != '\r')
				{
					stringBuilder.Append(value[i]);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600442F RID: 17455 RVA: 0x000E98F8 File Offset: 0x000E88F8
		internal static string LineFeedsBin64(string value)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < value.Length; i++)
			{
				if (i % 76 == 0)
				{
					stringBuilder.Append('\n');
				}
				stringBuilder.Append(value[i]);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06004430 RID: 17456 RVA: 0x000E9940 File Offset: 0x000E8940
		internal static string Escape(string value)
		{
			if (value == null || value.Length == 0)
			{
				return value;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = value.IndexOf('&');
			if (num > -1)
			{
				stringBuilder.Append(value);
				stringBuilder.Replace("&", "&#38;", num, stringBuilder.Length - num);
			}
			num = value.IndexOf('"');
			if (num > -1)
			{
				if (stringBuilder.Length == 0)
				{
					stringBuilder.Append(value);
				}
				stringBuilder.Replace("\"", "&#34;", num, stringBuilder.Length - num);
			}
			num = value.IndexOf('\'');
			if (num > -1)
			{
				if (stringBuilder.Length == 0)
				{
					stringBuilder.Append(value);
				}
				stringBuilder.Replace("'", "&#39;", num, stringBuilder.Length - num);
			}
			num = value.IndexOf('<');
			if (num > -1)
			{
				if (stringBuilder.Length == 0)
				{
					stringBuilder.Append(value);
				}
				stringBuilder.Replace("<", "&#60;", num, stringBuilder.Length - num);
			}
			num = value.IndexOf('>');
			if (num > -1)
			{
				if (stringBuilder.Length == 0)
				{
					stringBuilder.Append(value);
				}
				stringBuilder.Replace(">", "&#62;", num, stringBuilder.Length - num);
			}
			num = value.IndexOf('\0');
			if (num > -1)
			{
				if (stringBuilder.Length == 0)
				{
					stringBuilder.Append(value);
				}
				stringBuilder.Replace('\0'.ToString(), "&#0;", num, stringBuilder.Length - num);
			}
			string result;
			if (stringBuilder.Length > 0)
			{
				result = stringBuilder.ToString();
			}
			else
			{
				result = value;
			}
			return result;
		}

		// Token: 0x04002222 RID: 8738
		internal static Type typeofSoapTime = typeof(SoapTime);

		// Token: 0x04002223 RID: 8739
		internal static Type typeofSoapDate = typeof(SoapDate);

		// Token: 0x04002224 RID: 8740
		internal static Type typeofSoapYearMonth = typeof(SoapYearMonth);

		// Token: 0x04002225 RID: 8741
		internal static Type typeofSoapYear = typeof(SoapYear);

		// Token: 0x04002226 RID: 8742
		internal static Type typeofSoapMonthDay = typeof(SoapMonthDay);

		// Token: 0x04002227 RID: 8743
		internal static Type typeofSoapDay = typeof(SoapDay);

		// Token: 0x04002228 RID: 8744
		internal static Type typeofSoapMonth = typeof(SoapMonth);

		// Token: 0x04002229 RID: 8745
		internal static Type typeofSoapHexBinary = typeof(SoapHexBinary);

		// Token: 0x0400222A RID: 8746
		internal static Type typeofSoapBase64Binary = typeof(SoapBase64Binary);

		// Token: 0x0400222B RID: 8747
		internal static Type typeofSoapInteger = typeof(SoapInteger);

		// Token: 0x0400222C RID: 8748
		internal static Type typeofSoapPositiveInteger = typeof(SoapPositiveInteger);

		// Token: 0x0400222D RID: 8749
		internal static Type typeofSoapNonPositiveInteger = typeof(SoapNonPositiveInteger);

		// Token: 0x0400222E RID: 8750
		internal static Type typeofSoapNonNegativeInteger = typeof(SoapNonNegativeInteger);

		// Token: 0x0400222F RID: 8751
		internal static Type typeofSoapNegativeInteger = typeof(SoapNegativeInteger);

		// Token: 0x04002230 RID: 8752
		internal static Type typeofSoapAnyUri = typeof(SoapAnyUri);

		// Token: 0x04002231 RID: 8753
		internal static Type typeofSoapQName = typeof(SoapQName);

		// Token: 0x04002232 RID: 8754
		internal static Type typeofSoapNotation = typeof(SoapNotation);

		// Token: 0x04002233 RID: 8755
		internal static Type typeofSoapNormalizedString = typeof(SoapNormalizedString);

		// Token: 0x04002234 RID: 8756
		internal static Type typeofSoapToken = typeof(SoapToken);

		// Token: 0x04002235 RID: 8757
		internal static Type typeofSoapLanguage = typeof(SoapLanguage);

		// Token: 0x04002236 RID: 8758
		internal static Type typeofSoapName = typeof(SoapName);

		// Token: 0x04002237 RID: 8759
		internal static Type typeofSoapIdrefs = typeof(SoapIdrefs);

		// Token: 0x04002238 RID: 8760
		internal static Type typeofSoapEntities = typeof(SoapEntities);

		// Token: 0x04002239 RID: 8761
		internal static Type typeofSoapNmtoken = typeof(SoapNmtoken);

		// Token: 0x0400223A RID: 8762
		internal static Type typeofSoapNmtokens = typeof(SoapNmtokens);

		// Token: 0x0400223B RID: 8763
		internal static Type typeofSoapNcName = typeof(SoapNcName);

		// Token: 0x0400223C RID: 8764
		internal static Type typeofSoapId = typeof(SoapId);

		// Token: 0x0400223D RID: 8765
		internal static Type typeofSoapIdref = typeof(SoapIdref);

		// Token: 0x0400223E RID: 8766
		internal static Type typeofSoapEntity = typeof(SoapEntity);

		// Token: 0x0400223F RID: 8767
		internal static Type typeofISoapXsd = typeof(ISoapXsd);
	}
}
