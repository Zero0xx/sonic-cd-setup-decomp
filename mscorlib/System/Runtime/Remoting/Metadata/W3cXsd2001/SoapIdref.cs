using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x02000797 RID: 1943
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapIdref : ISoapXsd
	{
		// Token: 0x17000C2A RID: 3114
		// (get) Token: 0x0600452F RID: 17711 RVA: 0x000EB728 File Offset: 0x000EA728
		public static string XsdType
		{
			get
			{
				return "IDREF";
			}
		}

		// Token: 0x06004530 RID: 17712 RVA: 0x000EB72F File Offset: 0x000EA72F
		public string GetXsdType()
		{
			return SoapIdref.XsdType;
		}

		// Token: 0x06004531 RID: 17713 RVA: 0x000EB736 File Offset: 0x000EA736
		public SoapIdref()
		{
		}

		// Token: 0x06004532 RID: 17714 RVA: 0x000EB73E File Offset: 0x000EA73E
		public SoapIdref(string value)
		{
			this._value = value;
		}

		// Token: 0x17000C2B RID: 3115
		// (get) Token: 0x06004533 RID: 17715 RVA: 0x000EB74D File Offset: 0x000EA74D
		// (set) Token: 0x06004534 RID: 17716 RVA: 0x000EB755 File Offset: 0x000EA755
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

		// Token: 0x06004535 RID: 17717 RVA: 0x000EB75E File Offset: 0x000EA75E
		public override string ToString()
		{
			return SoapType.Escape(this._value);
		}

		// Token: 0x06004536 RID: 17718 RVA: 0x000EB76B File Offset: 0x000EA76B
		public static SoapIdref Parse(string value)
		{
			return new SoapIdref(value);
		}

		// Token: 0x04002269 RID: 8809
		private string _value;
	}
}
