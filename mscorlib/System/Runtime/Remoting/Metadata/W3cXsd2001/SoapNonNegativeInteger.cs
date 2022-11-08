using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x02000788 RID: 1928
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapNonNegativeInteger : ISoapXsd
	{
		// Token: 0x17000C0A RID: 3082
		// (get) Token: 0x060044AF RID: 17583 RVA: 0x000EAF00 File Offset: 0x000E9F00
		public static string XsdType
		{
			get
			{
				return "nonNegativeInteger";
			}
		}

		// Token: 0x060044B0 RID: 17584 RVA: 0x000EAF07 File Offset: 0x000E9F07
		public string GetXsdType()
		{
			return SoapNonNegativeInteger.XsdType;
		}

		// Token: 0x060044B1 RID: 17585 RVA: 0x000EAF0E File Offset: 0x000E9F0E
		public SoapNonNegativeInteger()
		{
		}

		// Token: 0x060044B2 RID: 17586 RVA: 0x000EAF18 File Offset: 0x000E9F18
		public SoapNonNegativeInteger(decimal value)
		{
			this._value = decimal.Truncate(value);
			if (this._value < 0m)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_SOAPInteropxsdInvalid"), new object[]
				{
					"xsd:nonNegativeInteger",
					value
				}));
			}
		}

		// Token: 0x17000C0B RID: 3083
		// (get) Token: 0x060044B3 RID: 17587 RVA: 0x000EAF7D File Offset: 0x000E9F7D
		// (set) Token: 0x060044B4 RID: 17588 RVA: 0x000EAF88 File Offset: 0x000E9F88
		public decimal Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = decimal.Truncate(value);
				if (this._value < 0m)
				{
					throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_SOAPInteropxsdInvalid"), new object[]
					{
						"xsd:nonNegativeInteger",
						value
					}));
				}
			}
		}

		// Token: 0x060044B5 RID: 17589 RVA: 0x000EAFE7 File Offset: 0x000E9FE7
		public override string ToString()
		{
			return this._value.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x060044B6 RID: 17590 RVA: 0x000EAFF9 File Offset: 0x000E9FF9
		public static SoapNonNegativeInteger Parse(string value)
		{
			return new SoapNonNegativeInteger(decimal.Parse(value, NumberStyles.Integer, CultureInfo.InvariantCulture));
		}

		// Token: 0x04002258 RID: 8792
		private decimal _value;
	}
}
