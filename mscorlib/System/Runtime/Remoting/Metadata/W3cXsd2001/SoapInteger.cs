using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x02000785 RID: 1925
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapInteger : ISoapXsd
	{
		// Token: 0x17000C04 RID: 3076
		// (get) Token: 0x06004497 RID: 17559 RVA: 0x000EAC84 File Offset: 0x000E9C84
		public static string XsdType
		{
			get
			{
				return "integer";
			}
		}

		// Token: 0x06004498 RID: 17560 RVA: 0x000EAC8B File Offset: 0x000E9C8B
		public string GetXsdType()
		{
			return SoapInteger.XsdType;
		}

		// Token: 0x06004499 RID: 17561 RVA: 0x000EAC92 File Offset: 0x000E9C92
		public SoapInteger()
		{
		}

		// Token: 0x0600449A RID: 17562 RVA: 0x000EAC9A File Offset: 0x000E9C9A
		public SoapInteger(decimal value)
		{
			this._value = decimal.Truncate(value);
		}

		// Token: 0x17000C05 RID: 3077
		// (get) Token: 0x0600449B RID: 17563 RVA: 0x000EACAE File Offset: 0x000E9CAE
		// (set) Token: 0x0600449C RID: 17564 RVA: 0x000EACB6 File Offset: 0x000E9CB6
		public decimal Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = decimal.Truncate(value);
			}
		}

		// Token: 0x0600449D RID: 17565 RVA: 0x000EACC4 File Offset: 0x000E9CC4
		public override string ToString()
		{
			return this._value.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x0600449E RID: 17566 RVA: 0x000EACD6 File Offset: 0x000E9CD6
		public static SoapInteger Parse(string value)
		{
			return new SoapInteger(decimal.Parse(value, NumberStyles.Integer, CultureInfo.InvariantCulture));
		}

		// Token: 0x04002255 RID: 8789
		private decimal _value;
	}
}
