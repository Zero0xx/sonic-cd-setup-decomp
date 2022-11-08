using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x02000790 RID: 1936
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapName : ISoapXsd
	{
		// Token: 0x17000C1C RID: 3100
		// (get) Token: 0x060044F7 RID: 17655 RVA: 0x000EB51B File Offset: 0x000EA51B
		public static string XsdType
		{
			get
			{
				return "Name";
			}
		}

		// Token: 0x060044F8 RID: 17656 RVA: 0x000EB522 File Offset: 0x000EA522
		public string GetXsdType()
		{
			return SoapName.XsdType;
		}

		// Token: 0x060044F9 RID: 17657 RVA: 0x000EB529 File Offset: 0x000EA529
		public SoapName()
		{
		}

		// Token: 0x060044FA RID: 17658 RVA: 0x000EB531 File Offset: 0x000EA531
		public SoapName(string value)
		{
			this._value = value;
		}

		// Token: 0x17000C1D RID: 3101
		// (get) Token: 0x060044FB RID: 17659 RVA: 0x000EB540 File Offset: 0x000EA540
		// (set) Token: 0x060044FC RID: 17660 RVA: 0x000EB548 File Offset: 0x000EA548
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

		// Token: 0x060044FD RID: 17661 RVA: 0x000EB551 File Offset: 0x000EA551
		public override string ToString()
		{
			return SoapType.Escape(this._value);
		}

		// Token: 0x060044FE RID: 17662 RVA: 0x000EB55E File Offset: 0x000EA55E
		public static SoapName Parse(string value)
		{
			return new SoapName(value);
		}

		// Token: 0x04002262 RID: 8802
		private string _value;
	}
}
