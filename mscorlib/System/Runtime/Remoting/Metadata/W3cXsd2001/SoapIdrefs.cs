using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x02000791 RID: 1937
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapIdrefs : ISoapXsd
	{
		// Token: 0x17000C1E RID: 3102
		// (get) Token: 0x060044FF RID: 17663 RVA: 0x000EB566 File Offset: 0x000EA566
		public static string XsdType
		{
			get
			{
				return "IDREFS";
			}
		}

		// Token: 0x06004500 RID: 17664 RVA: 0x000EB56D File Offset: 0x000EA56D
		public string GetXsdType()
		{
			return SoapIdrefs.XsdType;
		}

		// Token: 0x06004501 RID: 17665 RVA: 0x000EB574 File Offset: 0x000EA574
		public SoapIdrefs()
		{
		}

		// Token: 0x06004502 RID: 17666 RVA: 0x000EB57C File Offset: 0x000EA57C
		public SoapIdrefs(string value)
		{
			this._value = value;
		}

		// Token: 0x17000C1F RID: 3103
		// (get) Token: 0x06004503 RID: 17667 RVA: 0x000EB58B File Offset: 0x000EA58B
		// (set) Token: 0x06004504 RID: 17668 RVA: 0x000EB593 File Offset: 0x000EA593
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

		// Token: 0x06004505 RID: 17669 RVA: 0x000EB59C File Offset: 0x000EA59C
		public override string ToString()
		{
			return SoapType.Escape(this._value);
		}

		// Token: 0x06004506 RID: 17670 RVA: 0x000EB5A9 File Offset: 0x000EA5A9
		public static SoapIdrefs Parse(string value)
		{
			return new SoapIdrefs(value);
		}

		// Token: 0x04002263 RID: 8803
		private string _value;
	}
}
