using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x02000783 RID: 1923
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapHexBinary : ISoapXsd
	{
		// Token: 0x17000C00 RID: 3072
		// (get) Token: 0x06004485 RID: 17541 RVA: 0x000EA9DE File Offset: 0x000E99DE
		public static string XsdType
		{
			get
			{
				return "hexBinary";
			}
		}

		// Token: 0x06004486 RID: 17542 RVA: 0x000EA9E5 File Offset: 0x000E99E5
		public string GetXsdType()
		{
			return SoapHexBinary.XsdType;
		}

		// Token: 0x06004487 RID: 17543 RVA: 0x000EA9EC File Offset: 0x000E99EC
		public SoapHexBinary()
		{
		}

		// Token: 0x06004488 RID: 17544 RVA: 0x000EAA01 File Offset: 0x000E9A01
		public SoapHexBinary(byte[] value)
		{
			this._value = value;
		}

		// Token: 0x17000C01 RID: 3073
		// (get) Token: 0x06004489 RID: 17545 RVA: 0x000EAA1D File Offset: 0x000E9A1D
		// (set) Token: 0x0600448A RID: 17546 RVA: 0x000EAA25 File Offset: 0x000E9A25
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

		// Token: 0x0600448B RID: 17547 RVA: 0x000EAA30 File Offset: 0x000E9A30
		public override string ToString()
		{
			this.sb.Length = 0;
			for (int i = 0; i < this._value.Length; i++)
			{
				string text = this._value[i].ToString("X", CultureInfo.InvariantCulture);
				if (text.Length == 1)
				{
					this.sb.Append('0');
				}
				this.sb.Append(text);
			}
			return this.sb.ToString();
		}

		// Token: 0x0600448C RID: 17548 RVA: 0x000EAAA7 File Offset: 0x000E9AA7
		public static SoapHexBinary Parse(string value)
		{
			return new SoapHexBinary(SoapHexBinary.ToByteArray(SoapType.FilterBin64(value)));
		}

		// Token: 0x0600448D RID: 17549 RVA: 0x000EAABC File Offset: 0x000E9ABC
		private static byte[] ToByteArray(string value)
		{
			char[] array = value.ToCharArray();
			if (array.Length % 2 != 0)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_SOAPInteropxsdInvalid"), new object[]
				{
					"xsd:hexBinary",
					value
				}));
			}
			byte[] array2 = new byte[array.Length / 2];
			for (int i = 0; i < array.Length / 2; i++)
			{
				array2[i] = SoapHexBinary.ToByte(array[i * 2], value) * 16 + SoapHexBinary.ToByte(array[i * 2 + 1], value);
			}
			return array2;
		}

		// Token: 0x0600448E RID: 17550 RVA: 0x000EAB44 File Offset: 0x000E9B44
		private static byte ToByte(char c, string value)
		{
			byte result = 0;
			string s = c.ToString();
			try
			{
				s = c.ToString();
				result = byte.Parse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
			}
			catch (Exception)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_SOAPInteropxsdInvalid"), new object[]
				{
					"xsd:hexBinary",
					value
				}));
			}
			return result;
		}

		// Token: 0x04002252 RID: 8786
		private byte[] _value;

		// Token: 0x04002253 RID: 8787
		private StringBuilder sb = new StringBuilder(100);
	}
}
