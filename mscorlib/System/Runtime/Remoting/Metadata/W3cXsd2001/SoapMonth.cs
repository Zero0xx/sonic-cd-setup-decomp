using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x02000782 RID: 1922
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapMonth : ISoapXsd
	{
		// Token: 0x17000BFE RID: 3070
		// (get) Token: 0x0600447C RID: 17532 RVA: 0x000EA936 File Offset: 0x000E9936
		public static string XsdType
		{
			get
			{
				return "gMonth";
			}
		}

		// Token: 0x0600447D RID: 17533 RVA: 0x000EA93D File Offset: 0x000E993D
		public string GetXsdType()
		{
			return SoapMonth.XsdType;
		}

		// Token: 0x0600447E RID: 17534 RVA: 0x000EA944 File Offset: 0x000E9944
		public SoapMonth()
		{
		}

		// Token: 0x0600447F RID: 17535 RVA: 0x000EA957 File Offset: 0x000E9957
		public SoapMonth(DateTime value)
		{
			this._value = value;
		}

		// Token: 0x17000BFF RID: 3071
		// (get) Token: 0x06004480 RID: 17536 RVA: 0x000EA971 File Offset: 0x000E9971
		// (set) Token: 0x06004481 RID: 17537 RVA: 0x000EA979 File Offset: 0x000E9979
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

		// Token: 0x06004482 RID: 17538 RVA: 0x000EA982 File Offset: 0x000E9982
		public override string ToString()
		{
			return this._value.ToString("--MM--", CultureInfo.InvariantCulture);
		}

		// Token: 0x06004483 RID: 17539 RVA: 0x000EA999 File Offset: 0x000E9999
		public static SoapMonth Parse(string value)
		{
			return new SoapMonth(DateTime.ParseExact(value, SoapMonth.formats, CultureInfo.InvariantCulture, DateTimeStyles.None));
		}

		// Token: 0x04002250 RID: 8784
		private DateTime _value = DateTime.MinValue;

		// Token: 0x04002251 RID: 8785
		private static string[] formats = new string[]
		{
			"--MM--",
			"--MM--zzz"
		};
	}
}
