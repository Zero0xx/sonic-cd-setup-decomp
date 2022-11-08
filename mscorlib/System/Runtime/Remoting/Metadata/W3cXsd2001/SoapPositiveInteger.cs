using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x02000786 RID: 1926
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapPositiveInteger : ISoapXsd
	{
		// Token: 0x17000C06 RID: 3078
		// (get) Token: 0x0600449F RID: 17567 RVA: 0x000EACE9 File Offset: 0x000E9CE9
		public static string XsdType
		{
			get
			{
				return "positiveInteger";
			}
		}

		// Token: 0x060044A0 RID: 17568 RVA: 0x000EACF0 File Offset: 0x000E9CF0
		public string GetXsdType()
		{
			return SoapPositiveInteger.XsdType;
		}

		// Token: 0x060044A1 RID: 17569 RVA: 0x000EACF7 File Offset: 0x000E9CF7
		public SoapPositiveInteger()
		{
		}

		// Token: 0x060044A2 RID: 17570 RVA: 0x000EAD00 File Offset: 0x000E9D00
		public SoapPositiveInteger(decimal value)
		{
			this._value = decimal.Truncate(value);
			if (this._value < 1m)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_SOAPInteropxsdInvalid"), new object[]
				{
					"xsd:positiveInteger",
					value
				}));
			}
		}

		// Token: 0x17000C07 RID: 3079
		// (get) Token: 0x060044A3 RID: 17571 RVA: 0x000EAD65 File Offset: 0x000E9D65
		// (set) Token: 0x060044A4 RID: 17572 RVA: 0x000EAD70 File Offset: 0x000E9D70
		public decimal Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = decimal.Truncate(value);
				if (this._value < 1m)
				{
					throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_SOAPInteropxsdInvalid"), new object[]
					{
						"xsd:positiveInteger",
						value
					}));
				}
			}
		}

		// Token: 0x060044A5 RID: 17573 RVA: 0x000EADCF File Offset: 0x000E9DCF
		public override string ToString()
		{
			return this._value.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x060044A6 RID: 17574 RVA: 0x000EADE1 File Offset: 0x000E9DE1
		public static SoapPositiveInteger Parse(string value)
		{
			return new SoapPositiveInteger(decimal.Parse(value, NumberStyles.Integer, CultureInfo.InvariantCulture));
		}

		// Token: 0x04002256 RID: 8790
		private decimal _value;
	}
}
