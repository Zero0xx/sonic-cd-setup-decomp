using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x02000784 RID: 1924
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapBase64Binary : ISoapXsd
	{
		// Token: 0x17000C02 RID: 3074
		// (get) Token: 0x0600448F RID: 17551 RVA: 0x000EABB8 File Offset: 0x000E9BB8
		public static string XsdType
		{
			get
			{
				return "base64Binary";
			}
		}

		// Token: 0x06004490 RID: 17552 RVA: 0x000EABBF File Offset: 0x000E9BBF
		public string GetXsdType()
		{
			return SoapBase64Binary.XsdType;
		}

		// Token: 0x06004491 RID: 17553 RVA: 0x000EABC6 File Offset: 0x000E9BC6
		public SoapBase64Binary()
		{
		}

		// Token: 0x06004492 RID: 17554 RVA: 0x000EABCE File Offset: 0x000E9BCE
		public SoapBase64Binary(byte[] value)
		{
			this._value = value;
		}

		// Token: 0x17000C03 RID: 3075
		// (get) Token: 0x06004493 RID: 17555 RVA: 0x000EABDD File Offset: 0x000E9BDD
		// (set) Token: 0x06004494 RID: 17556 RVA: 0x000EABE5 File Offset: 0x000E9BE5
		public byte[] Value
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

		// Token: 0x06004495 RID: 17557 RVA: 0x000EABEE File Offset: 0x000E9BEE
		public override string ToString()
		{
			if (this._value == null)
			{
				return null;
			}
			return SoapType.LineFeedsBin64(Convert.ToBase64String(this._value));
		}

		// Token: 0x06004496 RID: 17558 RVA: 0x000EAC0C File Offset: 0x000E9C0C
		public static SoapBase64Binary Parse(string value)
		{
			if (value == null || value.Length == 0)
			{
				return new SoapBase64Binary(new byte[0]);
			}
			byte[] value2;
			try
			{
				value2 = Convert.FromBase64String(SoapType.FilterBin64(value));
			}
			catch (Exception)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_SOAPInteropxsdInvalid"), new object[]
				{
					"base64Binary",
					value
				}));
			}
			return new SoapBase64Binary(value2);
		}

		// Token: 0x04002254 RID: 8788
		private byte[] _value;
	}
}
