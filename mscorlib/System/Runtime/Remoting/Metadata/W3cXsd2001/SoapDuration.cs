using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x0200077B RID: 1915
	[ComVisible(true)]
	public sealed class SoapDuration
	{
		// Token: 0x17000BEE RID: 3054
		// (get) Token: 0x06004438 RID: 17464 RVA: 0x000E9E20 File Offset: 0x000E8E20
		public static string XsdType
		{
			get
			{
				return "duration";
			}
		}

		// Token: 0x06004439 RID: 17465 RVA: 0x000E9E28 File Offset: 0x000E8E28
		private static void CarryOver(int inDays, out int years, out int months, out int days)
		{
			years = inDays / 360;
			int num = years * 360;
			months = Math.Max(0, inDays - num) / 30;
			int num2 = months * 30;
			days = Math.Max(0, inDays - (num + num2));
			days = inDays % 30;
		}

		// Token: 0x0600443A RID: 17466 RVA: 0x000E9E70 File Offset: 0x000E8E70
		public static string ToString(TimeSpan timeSpan)
		{
			StringBuilder stringBuilder = new StringBuilder(10);
			stringBuilder.Length = 0;
			if (TimeSpan.Compare(timeSpan, TimeSpan.Zero) < 1)
			{
				stringBuilder.Append('-');
			}
			int value = 0;
			int value2 = 0;
			int value3 = 0;
			SoapDuration.CarryOver(Math.Abs(timeSpan.Days), out value, out value2, out value3);
			stringBuilder.Append('P');
			stringBuilder.Append(value);
			stringBuilder.Append('Y');
			stringBuilder.Append(value2);
			stringBuilder.Append('M');
			stringBuilder.Append(value3);
			stringBuilder.Append("DT");
			stringBuilder.Append(Math.Abs(timeSpan.Hours));
			stringBuilder.Append('H');
			stringBuilder.Append(Math.Abs(timeSpan.Minutes));
			stringBuilder.Append('M');
			stringBuilder.Append(Math.Abs(timeSpan.Seconds));
			long num = Math.Abs(timeSpan.Ticks % 864000000000L);
			int num2 = (int)(num % 10000000L);
			if (num2 != 0)
			{
				string value4 = ParseNumbers.IntToString(num2, 10, 7, '0', 0);
				stringBuilder.Append('.');
				stringBuilder.Append(value4);
			}
			stringBuilder.Append('S');
			return stringBuilder.ToString();
		}

		// Token: 0x0600443B RID: 17467 RVA: 0x000E9FA4 File Offset: 0x000E8FA4
		public static TimeSpan Parse(string value)
		{
			int num = 1;
			TimeSpan result;
			try
			{
				if (value == null)
				{
					result = TimeSpan.Zero;
				}
				else
				{
					if (value[0] == '-')
					{
						num = -1;
					}
					char[] array = value.ToCharArray();
					string s = "0";
					string s2 = "0";
					string s3 = "0";
					string s4 = "0";
					string s5 = "0";
					string str = "0";
					string str2 = "0";
					bool flag = false;
					bool flag2 = false;
					int num2 = 0;
					for (int i = 0; i < array.Length; i++)
					{
						char c = array[i];
						if (c <= 'H')
						{
							if (c != '.')
							{
								if (c != 'D')
								{
									if (c == 'H')
									{
										s4 = new string(array, num2, i - num2);
										num2 = i + 1;
									}
								}
								else
								{
									s3 = new string(array, num2, i - num2);
									num2 = i + 1;
								}
							}
							else
							{
								flag2 = true;
								str = new string(array, num2, i - num2);
								num2 = i + 1;
							}
						}
						else if (c != 'M')
						{
							switch (c)
							{
							case 'P':
								num2 = i + 1;
								break;
							case 'Q':
							case 'R':
								break;
							case 'S':
								if (!flag2)
								{
									str = new string(array, num2, i - num2);
								}
								else
								{
									str2 = new string(array, num2, i - num2);
								}
								break;
							case 'T':
								flag = true;
								num2 = i + 1;
								break;
							default:
								switch (c)
								{
								case 'Y':
									s = new string(array, num2, i - num2);
									num2 = i + 1;
									break;
								}
								break;
							}
						}
						else
						{
							if (flag)
							{
								s5 = new string(array, num2, i - num2);
							}
							else
							{
								s2 = new string(array, num2, i - num2);
							}
							num2 = i + 1;
						}
					}
					long ticks = (long)num * ((long.Parse(s, CultureInfo.InvariantCulture) * 360L + long.Parse(s2, CultureInfo.InvariantCulture) * 30L + long.Parse(s3, CultureInfo.InvariantCulture)) * 864000000000L + long.Parse(s4, CultureInfo.InvariantCulture) * 36000000000L + long.Parse(s5, CultureInfo.InvariantCulture) * 600000000L + Convert.ToInt64(double.Parse(str + "." + str2, CultureInfo.InvariantCulture) * 10000000.0));
					result = new TimeSpan(ticks);
				}
			}
			catch (Exception)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_SOAPInteropxsdInvalid"), new object[]
				{
					"xsd:duration",
					value
				}));
			}
			return result;
		}
	}
}
