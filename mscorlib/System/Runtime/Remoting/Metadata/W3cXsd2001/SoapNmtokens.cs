using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x02000794 RID: 1940
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapNmtokens : ISoapXsd
	{
		// Token: 0x17000C24 RID: 3108
		// (get) Token: 0x06004517 RID: 17687 RVA: 0x000EB647 File Offset: 0x000EA647
		public static string XsdType
		{
			get
			{
				return "NMTOKENS";
			}
		}

		// Token: 0x06004518 RID: 17688 RVA: 0x000EB64E File Offset: 0x000EA64E
		public string GetXsdType()
		{
			return SoapNmtokens.XsdType;
		}

		// Token: 0x06004519 RID: 17689 RVA: 0x000EB655 File Offset: 0x000EA655
		public SoapNmtokens()
		{
		}

		// Token: 0x0600451A RID: 17690 RVA: 0x000EB65D File Offset: 0x000EA65D
		public SoapNmtokens(string value)
		{
			this._value = value;
		}

		// Token: 0x17000C25 RID: 3109
		// (get) Token: 0x0600451B RID: 17691 RVA: 0x000EB66C File Offset: 0x000EA66C
		// (set) Token: 0x0600451C RID: 17692 RVA: 0x000EB674 File Offset: 0x000EA674
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

		// Token: 0x0600451D RID: 17693 RVA: 0x000EB67D File Offset: 0x000EA67D
		public override string ToString()
		{
			return SoapType.Escape(this._value);
		}

		// Token: 0x0600451E RID: 17694 RVA: 0x000EB68A File Offset: 0x000EA68A
		public static SoapNmtokens Parse(string value)
		{
			return new SoapNmtokens(value);
		}

		// Token: 0x04002266 RID: 8806
		private string _value;
	}
}
