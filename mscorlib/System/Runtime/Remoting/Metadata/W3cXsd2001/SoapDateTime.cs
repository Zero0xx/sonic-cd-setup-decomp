using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x0200077A RID: 1914
	[ComVisible(true)]
	public sealed class SoapDateTime
	{
		// Token: 0x17000BED RID: 3053
		// (get) Token: 0x06004433 RID: 17459 RVA: 0x000E9C8B File Offset: 0x000E8C8B
		public static string XsdType
		{
			get
			{
				return "dateTime";
			}
		}

		// Token: 0x06004434 RID: 17460 RVA: 0x000E9C92 File Offset: 0x000E8C92
		public static string ToString(DateTime value)
		{
			return value.ToString("yyyy-MM-dd'T'HH:mm:ss.fffffffzzz", CultureInfo.InvariantCulture);
		}

		// Token: 0x06004435 RID: 17461 RVA: 0x000E9CA8 File Offset: 0x000E8CA8
		public static DateTime Parse(string value)
		{
			DateTime result;
			try
			{
				if (value == null)
				{
					result = DateTime.MinValue;
				}
				else
				{
					string s = value;
					if (value.EndsWith("Z", StringComparison.Ordinal))
					{
						s = value.Substring(0, value.Length - 1) + "-00:00";
					}
					result = DateTime.ParseExact(s, SoapDateTime.formats, CultureInfo.InvariantCulture, DateTimeStyles.None);
				}
			}
			catch (Exception)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_SOAPInteropxsdInvalid"), new object[]
				{
					"xsd:dateTime",
					value
				}));
			}
			return result;
		}

		// Token: 0x04002240 RID: 8768
		private static string[] formats = new string[]
		{
			"yyyy-MM-dd'T'HH:mm:ss.fffffffzzz",
			"yyyy-MM-dd'T'HH:mm:ss.ffff",
			"yyyy-MM-dd'T'HH:mm:ss.ffffzzz",
			"yyyy-MM-dd'T'HH:mm:ss.fff",
			"yyyy-MM-dd'T'HH:mm:ss.fffzzz",
			"yyyy-MM-dd'T'HH:mm:ss.ff",
			"yyyy-MM-dd'T'HH:mm:ss.ffzzz",
			"yyyy-MM-dd'T'HH:mm:ss.f",
			"yyyy-MM-dd'T'HH:mm:ss.fzzz",
			"yyyy-MM-dd'T'HH:mm:ss",
			"yyyy-MM-dd'T'HH:mm:sszzz",
			"yyyy-MM-dd'T'HH:mm:ss.fffff",
			"yyyy-MM-dd'T'HH:mm:ss.fffffzzz",
			"yyyy-MM-dd'T'HH:mm:ss.ffffff",
			"yyyy-MM-dd'T'HH:mm:ss.ffffffzzz",
			"yyyy-MM-dd'T'HH:mm:ss.fffffff",
			"yyyy-MM-dd'T'HH:mm:ss.ffffffff",
			"yyyy-MM-dd'T'HH:mm:ss.ffffffffzzz",
			"yyyy-MM-dd'T'HH:mm:ss.fffffffff",
			"yyyy-MM-dd'T'HH:mm:ss.fffffffffzzz",
			"yyyy-MM-dd'T'HH:mm:ss.ffffffffff",
			"yyyy-MM-dd'T'HH:mm:ss.ffffffffffzzz"
		};
	}
}
