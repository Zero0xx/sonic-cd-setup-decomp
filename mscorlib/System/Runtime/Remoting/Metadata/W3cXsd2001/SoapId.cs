using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x02000796 RID: 1942
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapId : ISoapXsd
	{
		// Token: 0x17000C28 RID: 3112
		// (get) Token: 0x06004527 RID: 17703 RVA: 0x000EB6DD File Offset: 0x000EA6DD
		public static string XsdType
		{
			get
			{
				return "ID";
			}
		}

		// Token: 0x06004528 RID: 17704 RVA: 0x000EB6E4 File Offset: 0x000EA6E4
		public string GetXsdType()
		{
			return SoapId.XsdType;
		}

		// Token: 0x06004529 RID: 17705 RVA: 0x000EB6EB File Offset: 0x000EA6EB
		public SoapId()
		{
		}

		// Token: 0x0600452A RID: 17706 RVA: 0x000EB6F3 File Offset: 0x000EA6F3
		public SoapId(string value)
		{
			this._value = value;
		}

		// Token: 0x17000C29 RID: 3113
		// (get) Token: 0x0600452B RID: 17707 RVA: 0x000EB702 File Offset: 0x000EA702
		// (set) Token: 0x0600452C RID: 17708 RVA: 0x000EB70A File Offset: 0x000EA70A
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

		// Token: 0x0600452D RID: 17709 RVA: 0x000EB713 File Offset: 0x000EA713
		public override string ToString()
		{
			return SoapType.Escape(this._value);
		}

		// Token: 0x0600452E RID: 17710 RVA: 0x000EB720 File Offset: 0x000EA720
		public static SoapId Parse(string value)
		{
			return new SoapId(value);
		}

		// Token: 0x04002268 RID: 8808
		private string _value;
	}
}
