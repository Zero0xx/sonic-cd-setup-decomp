using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x0200078A RID: 1930
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapAnyUri : ISoapXsd
	{
		// Token: 0x17000C0E RID: 3086
		// (get) Token: 0x060044BF RID: 17599 RVA: 0x000EB110 File Offset: 0x000EA110
		public static string XsdType
		{
			get
			{
				return "anyURI";
			}
		}

		// Token: 0x060044C0 RID: 17600 RVA: 0x000EB117 File Offset: 0x000EA117
		public string GetXsdType()
		{
			return SoapAnyUri.XsdType;
		}

		// Token: 0x060044C1 RID: 17601 RVA: 0x000EB11E File Offset: 0x000EA11E
		public SoapAnyUri()
		{
		}

		// Token: 0x060044C2 RID: 17602 RVA: 0x000EB126 File Offset: 0x000EA126
		public SoapAnyUri(string value)
		{
			this._value = value;
		}

		// Token: 0x17000C0F RID: 3087
		// (get) Token: 0x060044C3 RID: 17603 RVA: 0x000EB135 File Offset: 0x000EA135
		// (set) Token: 0x060044C4 RID: 17604 RVA: 0x000EB13D File Offset: 0x000EA13D
		public string Value
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

		// Token: 0x060044C5 RID: 17605 RVA: 0x000EB146 File Offset: 0x000EA146
		public override string ToString()
		{
			return this._value;
		}

		// Token: 0x060044C6 RID: 17606 RVA: 0x000EB14E File Offset: 0x000EA14E
		public static SoapAnyUri Parse(string value)
		{
			return new SoapAnyUri(value);
		}

		// Token: 0x0400225A RID: 8794
		private string _value;
	}
}
