using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x0200078C RID: 1932
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapNotation : ISoapXsd
	{
		// Token: 0x17000C14 RID: 3092
		// (get) Token: 0x060044D5 RID: 17621 RVA: 0x000EB25D File Offset: 0x000EA25D
		public static string XsdType
		{
			get
			{
				return "NOTATION";
			}
		}

		// Token: 0x060044D6 RID: 17622 RVA: 0x000EB264 File Offset: 0x000EA264
		public string GetXsdType()
		{
			return SoapNotation.XsdType;
		}

		// Token: 0x060044D7 RID: 17623 RVA: 0x000EB26B File Offset: 0x000EA26B
		public SoapNotation()
		{
		}

		// Token: 0x060044D8 RID: 17624 RVA: 0x000EB273 File Offset: 0x000EA273
		public SoapNotation(string value)
		{
			this._value = value;
		}

		// Token: 0x17000C15 RID: 3093
		// (get) Token: 0x060044D9 RID: 17625 RVA: 0x000EB282 File Offset: 0x000EA282
		// (set) Token: 0x060044DA RID: 17626 RVA: 0x000EB28A File Offset: 0x000EA28A
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

		// Token: 0x060044DB RID: 17627 RVA: 0x000EB293 File Offset: 0x000EA293
		public override string ToString()
		{
			return this._value;
		}

		// Token: 0x060044DC RID: 17628 RVA: 0x000EB29B File Offset: 0x000EA29B
		public static SoapNotation Parse(string value)
		{
			return new SoapNotation(value);
		}

		// Token: 0x0400225E RID: 8798
		private string _value;
	}
}
