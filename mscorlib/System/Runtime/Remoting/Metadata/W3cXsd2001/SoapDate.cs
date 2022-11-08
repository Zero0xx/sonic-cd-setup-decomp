using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x0200077D RID: 1917
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapDate : ISoapXsd
	{
		// Token: 0x17000BF1 RID: 3057
		// (get) Token: 0x06004446 RID: 17478 RVA: 0x000EA404 File Offset: 0x000E9404
		public static string XsdType
		{
			get
			{
				return "date";
			}
		}

		// Token: 0x06004447 RID: 17479 RVA: 0x000EA40B File Offset: 0x000E940B
		public string GetXsdType()
		{
			return SoapDate.XsdType;
		}

		// Token: 0x06004448 RID: 17480 RVA: 0x000EA414 File Offset: 0x000E9414
		public SoapDate()
		{
		}

		// Token: 0x06004449 RID: 17481 RVA: 0x000EA43C File Offset: 0x000E943C
		public SoapDate(DateTime value)
		{
			this._value = value;
		}

		// Token: 0x0600444A RID: 17482 RVA: 0x000EA46C File Offset: 0x000E946C
		public SoapDate(DateTime value, int sign)
		{
			this._value = value;
			this._sign = sign;
		}

		// Token: 0x17000BF2 RID: 3058
		// (get) Token: 0x0600444B RID: 17483 RVA: 0x000EA4A0 File Offset: 0x000E94A0
		// (set) Token: 0x0600444C RID: 17484 RVA: 0x000EA4A8 File Offset: 0x000E94A8
		public DateTime Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value.Date;
			}
		}

		// Token: 0x17000BF3 RID: 3059
		// (get) Token: 0x0600444D RID: 17485 RVA: 0x000EA4B7 File Offset: 0x000E94B7
		// (set) Token: 0x0600444E RID: 17486 RVA: 0x000EA4BF File Offset: 0x000E94BF
		public int Sign
		{
			get
			{
				return this._sign;
			}
			set
			{
				this._sign = value;
			}
		}

		// Token: 0x0600444F RID: 17487 RVA: 0x000EA4C8 File Offset: 0x000E94C8
		public override string ToString()
		{
			if (this._sign < 0)
			{
				return this._value.ToString("'-'yyyy-MM-dd", CultureInfo.InvariantCulture);
			}
			return this._value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
		}

		// Token: 0x06004450 RID: 17488 RVA: 0x000EA500 File Offset: 0x000E9500
		public static SoapDate Parse(string value)
		{
			int sign = 0;
			if (value[0] == '-')
			{
				sign = -1;
			}
			return new SoapDate(DateTime.ParseExact(value, SoapDate.formats, CultureInfo.InvariantCulture, DateTimeStyles.None), sign);
		}

		// Token: 0x04002243 RID: 8771
		private DateTime _value = DateTime.MinValue.Date;

		// Token: 0x04002244 RID: 8772
		private int _sign;

		// Token: 0x04002245 RID: 8773
		private static string[] formats = new string[]
		{
			"yyyy-MM-dd",
			"'+'yyyy-MM-dd",
			"'-'yyyy-MM-dd",
			"yyyy-MM-ddzzz",
			"'+'yyyy-MM-ddzzz",
			"'-'yyyy-MM-ddzzz"
		};
	}
}
