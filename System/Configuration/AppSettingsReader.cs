using System;
using System.Collections.Specialized;
using System.Globalization;

namespace System.Configuration
{
	// Token: 0x0200079B RID: 1947
	public class AppSettingsReader
	{
		// Token: 0x06003BFC RID: 15356 RVA: 0x00100956 File Offset: 0x000FF956
		public AppSettingsReader()
		{
			this.map = ConfigurationManager.AppSettings;
		}

		// Token: 0x06003BFD RID: 15357 RVA: 0x0010096C File Offset: 0x000FF96C
		public object GetValue(string key, Type type)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			string text = this.map[key];
			if (text == null)
			{
				throw new InvalidOperationException(SR.GetString("AppSettingsReaderNoKey", new object[]
				{
					key
				}));
			}
			if (type != AppSettingsReader.stringType)
			{
				object result;
				try
				{
					result = Convert.ChangeType(text, type, CultureInfo.InvariantCulture);
				}
				catch (Exception)
				{
					string text2 = (text.Length == 0) ? "AppSettingsReaderEmptyString" : text;
					throw new InvalidOperationException(SR.GetString("AppSettingsReaderCantParse", new object[]
					{
						text2,
						key,
						type.ToString()
					}));
				}
				return result;
			}
			int noneNesting = this.GetNoneNesting(text);
			if (noneNesting == 0)
			{
				return text;
			}
			if (noneNesting == 1)
			{
				return null;
			}
			return text.Substring(1, text.Length - 2);
		}

		// Token: 0x06003BFE RID: 15358 RVA: 0x00100A50 File Offset: 0x000FFA50
		private int GetNoneNesting(string val)
		{
			int num = 0;
			int length = val.Length;
			if (length > 1)
			{
				while (val[num] == '(' && val[length - num - 1] == ')')
				{
					num++;
				}
				if (num > 0 && string.Compare(AppSettingsReader.NullString, 0, val, num, length - 2 * num, StringComparison.Ordinal) != 0)
				{
					num = 0;
				}
			}
			return num;
		}

		// Token: 0x040034B4 RID: 13492
		private NameValueCollection map;

		// Token: 0x040034B5 RID: 13493
		private static Type stringType = typeof(string);

		// Token: 0x040034B6 RID: 13494
		private static Type[] paramsArray = new Type[]
		{
			AppSettingsReader.stringType
		};

		// Token: 0x040034B7 RID: 13495
		private static string NullString = "None";
	}
}
