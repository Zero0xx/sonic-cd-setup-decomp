using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x0200077F RID: 1919
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapYear : ISoapXsd
	{
		// Token: 0x17000BF7 RID: 3063
		// (get) Token: 0x0600445E RID: 17502 RVA: 0x000EA6B2 File Offset: 0x000E96B2
		public static string XsdType
		{
			get
			{
				return "gYear";
			}
		}

		// Token: 0x0600445F RID: 17503 RVA: 0x000EA6B9 File Offset: 0x000E96B9
		public string GetXsdType()
		{
			return SoapYear.XsdType;
		}

		// Token: 0x06004460 RID: 17504 RVA: 0x000EA6C0 File Offset: 0x000E96C0
		public SoapYear()
		{
		}

		// Token: 0x06004461 RID: 17505 RVA: 0x000EA6D3 File Offset: 0x000E96D3
		public SoapYear(DateTime value)
		{
			this._value = value;
		}

		// Token: 0x06004462 RID: 17506 RVA: 0x000EA6ED File Offset: 0x000E96ED
		public SoapYear(DateTime value, int sign)
		{
			this._value = value;
			this._sign = sign;
		}

		// Token: 0x17000BF8 RID: 3064
		// (get) Token: 0x06004463 RID: 17507 RVA: 0x000EA70E File Offset: 0x000E970E
		// (set) Token: 0x06004464 RID: 17508 RVA: 0x000EA716 File Offset: 0x000E9716
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

		// Token: 0x17000BF9 RID: 3065
		// (get) Token: 0x06004465 RID: 17509 RVA: 0x000EA71F File Offset: 0x000E971F
		// (set) Token: 0x06004466 RID: 17510 RVA: 0x000EA727 File Offset: 0x000E9727
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

		// Token: 0x06004467 RID: 17511 RVA: 0x000EA730 File Offset: 0x000E9730
		public override string ToString()
		{
			if (this._sign < 0)
			{
				return this._value.ToString("'-'yyyy", CultureInfo.InvariantCulture);
			}
			return this._value.ToString("yyyy", CultureInfo.InvariantCulture);
		}

		// Token: 0x06004468 RID: 17512 RVA: 0x000EA768 File Offset: 0x000E9768
		public static SoapYear Parse(string value)
		{
			int sign = 0;
			if (value[0] == '-')
			{
				sign = -1;
			}
			return new SoapYear(DateTime.ParseExact(value, SoapYear.formats, CultureInfo.InvariantCulture, DateTimeStyles.None), sign);
		}

		// Token: 0x04002249 RID: 8777
		private DateTime _value = DateTime.MinValue;

		// Token: 0x0400224A RID: 8778
		private int _sign;

		// Token: 0x0400224B RID: 8779
		private static string[] formats = new string[]
		{
			"yyyy",
			"'+'yyyy",
			"'-'yyyy",
			"yyyyzzz",
			"'+'yyyyzzz",
			"'-'yyyyzzz"
		};
	}
}
