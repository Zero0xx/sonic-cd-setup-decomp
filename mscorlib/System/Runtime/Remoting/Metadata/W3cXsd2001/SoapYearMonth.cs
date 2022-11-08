using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x0200077E RID: 1918
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapYearMonth : ISoapXsd
	{
		// Token: 0x17000BF4 RID: 3060
		// (get) Token: 0x06004452 RID: 17490 RVA: 0x000EA57E File Offset: 0x000E957E
		public static string XsdType
		{
			get
			{
				return "gYearMonth";
			}
		}

		// Token: 0x06004453 RID: 17491 RVA: 0x000EA585 File Offset: 0x000E9585
		public string GetXsdType()
		{
			return SoapYearMonth.XsdType;
		}

		// Token: 0x06004454 RID: 17492 RVA: 0x000EA58C File Offset: 0x000E958C
		public SoapYearMonth()
		{
		}

		// Token: 0x06004455 RID: 17493 RVA: 0x000EA59F File Offset: 0x000E959F
		public SoapYearMonth(DateTime value)
		{
			this._value = value;
		}

		// Token: 0x06004456 RID: 17494 RVA: 0x000EA5B9 File Offset: 0x000E95B9
		public SoapYearMonth(DateTime value, int sign)
		{
			this._value = value;
			this._sign = sign;
		}

		// Token: 0x17000BF5 RID: 3061
		// (get) Token: 0x06004457 RID: 17495 RVA: 0x000EA5DA File Offset: 0x000E95DA
		// (set) Token: 0x06004458 RID: 17496 RVA: 0x000EA5E2 File Offset: 0x000E95E2
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

		// Token: 0x17000BF6 RID: 3062
		// (get) Token: 0x06004459 RID: 17497 RVA: 0x000EA5EB File Offset: 0x000E95EB
		// (set) Token: 0x0600445A RID: 17498 RVA: 0x000EA5F3 File Offset: 0x000E95F3
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

		// Token: 0x0600445B RID: 17499 RVA: 0x000EA5FC File Offset: 0x000E95FC
		public override string ToString()
		{
			if (this._sign < 0)
			{
				return this._value.ToString("'-'yyyy-MM", CultureInfo.InvariantCulture);
			}
			return this._value.ToString("yyyy-MM", CultureInfo.InvariantCulture);
		}

		// Token: 0x0600445C RID: 17500 RVA: 0x000EA634 File Offset: 0x000E9634
		public static SoapYearMonth Parse(string value)
		{
			int sign = 0;
			if (value[0] == '-')
			{
				sign = -1;
			}
			return new SoapYearMonth(DateTime.ParseExact(value, SoapYearMonth.formats, CultureInfo.InvariantCulture, DateTimeStyles.None), sign);
		}

		// Token: 0x04002246 RID: 8774
		private DateTime _value = DateTime.MinValue;

		// Token: 0x04002247 RID: 8775
		private int _sign;

		// Token: 0x04002248 RID: 8776
		private static string[] formats = new string[]
		{
			"yyyy-MM",
			"'+'yyyy-MM",
			"'-'yyyy-MM",
			"yyyy-MMzzz",
			"'+'yyyy-MMzzz",
			"'-'yyyy-MMzzz"
		};
	}
}
