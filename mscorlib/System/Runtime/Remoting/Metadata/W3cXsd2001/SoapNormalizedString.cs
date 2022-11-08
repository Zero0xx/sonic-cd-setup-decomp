using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x0200078D RID: 1933
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapNormalizedString : ISoapXsd
	{
		// Token: 0x17000C16 RID: 3094
		// (get) Token: 0x060044DD RID: 17629 RVA: 0x000EB2A3 File Offset: 0x000EA2A3
		public static string XsdType
		{
			get
			{
				return "normalizedString";
			}
		}

		// Token: 0x060044DE RID: 17630 RVA: 0x000EB2AA File Offset: 0x000EA2AA
		public string GetXsdType()
		{
			return SoapNormalizedString.XsdType;
		}

		// Token: 0x060044DF RID: 17631 RVA: 0x000EB2B1 File Offset: 0x000EA2B1
		public SoapNormalizedString()
		{
		}

		// Token: 0x060044E0 RID: 17632 RVA: 0x000EB2B9 File Offset: 0x000EA2B9
		public SoapNormalizedString(string value)
		{
			this._value = this.Validate(value);
		}

		// Token: 0x17000C17 RID: 3095
		// (get) Token: 0x060044E1 RID: 17633 RVA: 0x000EB2CE File Offset: 0x000EA2CE
		// (set) Token: 0x060044E2 RID: 17634 RVA: 0x000EB2D6 File Offset: 0x000EA2D6
		public string Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = this.Validate(value);
			}
		}

		// Token: 0x060044E3 RID: 17635 RVA: 0x000EB2E5 File Offset: 0x000EA2E5
		public override string ToString()
		{
			return SoapType.Escape(this._value);
		}

		// Token: 0x060044E4 RID: 17636 RVA: 0x000EB2F2 File Offset: 0x000EA2F2
		public static SoapNormalizedString Parse(string value)
		{
			return new SoapNormalizedString(value);
		}

		// Token: 0x060044E5 RID: 17637 RVA: 0x000EB308 File Offset: 0x000EA308
		private string Validate(string value)
		{
			if (value == null || value.Length == 0)
			{
				return value;
			}
			char[] anyOf = new char[]
			{
				'\r',
				'\n',
				'\t'
			};
			int num = value.LastIndexOfAny(anyOf);
			if (num > -1)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_SOAPInteropxsdInvalid"), new object[]
				{
					"xsd:normalizedString",
					value
				}));
			}
			return value;
		}

		// Token: 0x0400225F RID: 8799
		private string _value;
	}
}
