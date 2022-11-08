using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x02000789 RID: 1929
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapNegativeInteger : ISoapXsd
	{
		// Token: 0x17000C0C RID: 3084
		// (get) Token: 0x060044B7 RID: 17591 RVA: 0x000EB00C File Offset: 0x000EA00C
		public static string XsdType
		{
			get
			{
				return "negativeInteger";
			}
		}

		// Token: 0x060044B8 RID: 17592 RVA: 0x000EB013 File Offset: 0x000EA013
		public string GetXsdType()
		{
			return SoapNegativeInteger.XsdType;
		}

		// Token: 0x060044B9 RID: 17593 RVA: 0x000EB01A File Offset: 0x000EA01A
		public SoapNegativeInteger()
		{
		}

		// Token: 0x060044BA RID: 17594 RVA: 0x000EB024 File Offset: 0x000EA024
		public SoapNegativeInteger(decimal value)
		{
			this._value = decimal.Truncate(value);
			if (value > -1m)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_SOAPInteropxsdInvalid"), new object[]
				{
					"xsd:negativeInteger",
					value
				}));
			}
		}

		// Token: 0x17000C0D RID: 3085
		// (get) Token: 0x060044BB RID: 17595 RVA: 0x000EB084 File Offset: 0x000EA084
		// (set) Token: 0x060044BC RID: 17596 RVA: 0x000EB08C File Offset: 0x000EA08C
		public decimal Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = decimal.Truncate(value);
				if (this._value > -1m)
				{
					throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_SOAPInteropxsdInvalid"), new object[]
					{
						"xsd:negativeInteger",
						value
					}));
				}
			}
		}

		// Token: 0x060044BD RID: 17597 RVA: 0x000EB0EB File Offset: 0x000EA0EB
		public override string ToString()
		{
			return this._value.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x060044BE RID: 17598 RVA: 0x000EB0FD File Offset: 0x000EA0FD
		public static SoapNegativeInteger Parse(string value)
		{
			return new SoapNegativeInteger(decimal.Parse(value, NumberStyles.Integer, CultureInfo.InvariantCulture));
		}

		// Token: 0x04002259 RID: 8793
		private decimal _value;
	}
}
