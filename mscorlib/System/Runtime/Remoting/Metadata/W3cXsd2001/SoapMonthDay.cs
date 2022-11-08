using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x02000780 RID: 1920
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapMonthDay : ISoapXsd
	{
		// Token: 0x17000BFA RID: 3066
		// (get) Token: 0x0600446A RID: 17514 RVA: 0x000EA7E6 File Offset: 0x000E97E6
		public static string XsdType
		{
			get
			{
				return "gMonthDay";
			}
		}

		// Token: 0x0600446B RID: 17515 RVA: 0x000EA7ED File Offset: 0x000E97ED
		public string GetXsdType()
		{
			return SoapMonthDay.XsdType;
		}

		// Token: 0x0600446C RID: 17516 RVA: 0x000EA7F4 File Offset: 0x000E97F4
		public SoapMonthDay()
		{
		}

		// Token: 0x0600446D RID: 17517 RVA: 0x000EA807 File Offset: 0x000E9807
		public SoapMonthDay(DateTime value)
		{
			this._value = value;
		}

		// Token: 0x17000BFB RID: 3067
		// (get) Token: 0x0600446E RID: 17518 RVA: 0x000EA821 File Offset: 0x000E9821
		// (set) Token: 0x0600446F RID: 17519 RVA: 0x000EA829 File Offset: 0x000E9829
		public DateTime Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		// Token: 0x06004470 RID: 17520 RVA: 0x000EA832 File Offset: 0x000E9832
		public override string ToString()
		{
			return this._value.ToString("'--'MM'-'dd", CultureInfo.InvariantCulture);
		}

		// Token: 0x06004471 RID: 17521 RVA: 0x000EA849 File Offset: 0x000E9849
		public static SoapMonthDay Parse(string value)
		{
			return new SoapMonthDay(DateTime.ParseExact(value, SoapMonthDay.formats, CultureInfo.InvariantCulture, DateTimeStyles.None));
		}

		// Token: 0x0400224C RID: 8780
		private DateTime _value = DateTime.MinValue;

		// Token: 0x0400224D RID: 8781
		private static string[] formats = new string[]
		{
			"--MM-dd",
			"--MM-ddzzz"
		};
	}
}
