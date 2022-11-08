using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x02000787 RID: 1927
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapNonPositiveInteger : ISoapXsd
	{
		// Token: 0x17000C08 RID: 3080
		// (get) Token: 0x060044A7 RID: 17575 RVA: 0x000EADF4 File Offset: 0x000E9DF4
		public static string XsdType
		{
			get
			{
				return "nonPositiveInteger";
			}
		}

		// Token: 0x060044A8 RID: 17576 RVA: 0x000EADFB File Offset: 0x000E9DFB
		public string GetXsdType()
		{
			return SoapNonPositiveInteger.XsdType;
		}

		// Token: 0x060044A9 RID: 17577 RVA: 0x000EAE02 File Offset: 0x000E9E02
		public SoapNonPositiveInteger()
		{
		}

		// Token: 0x060044AA RID: 17578 RVA: 0x000EAE0C File Offset: 0x000E9E0C
		public SoapNonPositiveInteger(decimal value)
		{
			this._value = decimal.Truncate(value);
			if (this._value > 0m)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_SOAPInteropxsdInvalid"), new object[]
				{
					"xsd:nonPositiveInteger",
					value
				}));
			}
		}

		// Token: 0x17000C09 RID: 3081
		// (get) Token: 0x060044AB RID: 17579 RVA: 0x000EAE71 File Offset: 0x000E9E71
		// (set) Token: 0x060044AC RID: 17580 RVA: 0x000EAE7C File Offset: 0x000E9E7C
		public decimal Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = decimal.Truncate(value);
				if (this._value > 0m)
				{
					throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_SOAPInteropxsdInvalid"), new object[]
					{
						"xsd:nonPositiveInteger",
						value
					}));
				}
			}
		}

		// Token: 0x060044AD RID: 17581 RVA: 0x000EAEDB File Offset: 0x000E9EDB
		public override string ToString()
		{
			return this._value.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x060044AE RID: 17582 RVA: 0x000EAEED File Offset: 0x000E9EED
		public static SoapNonPositiveInteger Parse(string value)
		{
			return new SoapNonPositiveInteger(decimal.Parse(value, NumberStyles.Integer, CultureInfo.InvariantCulture));
		}

		// Token: 0x04002257 RID: 8791
		private decimal _value;
	}
}
