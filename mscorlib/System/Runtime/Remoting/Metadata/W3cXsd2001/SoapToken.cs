using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x0200078E RID: 1934
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapToken : ISoapXsd
	{
		// Token: 0x17000C18 RID: 3096
		// (get) Token: 0x060044E6 RID: 17638 RVA: 0x000EB36F File Offset: 0x000EA36F
		public static string XsdType
		{
			get
			{
				return "token";
			}
		}

		// Token: 0x060044E7 RID: 17639 RVA: 0x000EB376 File Offset: 0x000EA376
		public string GetXsdType()
		{
			return SoapToken.XsdType;
		}

		// Token: 0x060044E8 RID: 17640 RVA: 0x000EB37D File Offset: 0x000EA37D
		public SoapToken()
		{
		}

		// Token: 0x060044E9 RID: 17641 RVA: 0x000EB385 File Offset: 0x000EA385
		public SoapToken(string value)
		{
			this._value = this.Validate(value);
		}

		// Token: 0x17000C19 RID: 3097
		// (get) Token: 0x060044EA RID: 17642 RVA: 0x000EB39A File Offset: 0x000EA39A
		// (set) Token: 0x060044EB RID: 17643 RVA: 0x000EB3A2 File Offset: 0x000EA3A2
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

		// Token: 0x060044EC RID: 17644 RVA: 0x000EB3B1 File Offset: 0x000EA3B1
		public override string ToString()
		{
			return SoapType.Escape(this._value);
		}

		// Token: 0x060044ED RID: 17645 RVA: 0x000EB3BE File Offset: 0x000EA3BE
		public static SoapToken Parse(string value)
		{
			return new SoapToken(value);
		}

		// Token: 0x060044EE RID: 17646 RVA: 0x000EB3C8 File Offset: 0x000EA3C8
		private string Validate(string value)
		{
			if (value == null || value.Length == 0)
			{
				return value;
			}
			char[] anyOf = new char[]
			{
				'\r',
				'\t'
			};
			int num = value.LastIndexOfAny(anyOf);
			if (num > -1)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_SOAPInteropxsdInvalid"), new object[]
				{
					"xsd:token",
					value
				}));
			}
			if (value.Length > 0 && (char.IsWhiteSpace(value[0]) || char.IsWhiteSpace(value[value.Length - 1])))
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_SOAPInteropxsdInvalid"), new object[]
				{
					"xsd:token",
					value
				}));
			}
			num = value.IndexOf("  ");
			if (num > -1)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_SOAPInteropxsdInvalid"), new object[]
				{
					"xsd:token",
					value
				}));
			}
			return value;
		}

		// Token: 0x04002260 RID: 8800
		private string _value;
	}
}
