using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x02000795 RID: 1941
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapNcName : ISoapXsd
	{
		// Token: 0x17000C26 RID: 3110
		// (get) Token: 0x0600451F RID: 17695 RVA: 0x000EB692 File Offset: 0x000EA692
		public static string XsdType
		{
			get
			{
				return "NCName";
			}
		}

		// Token: 0x06004520 RID: 17696 RVA: 0x000EB699 File Offset: 0x000EA699
		public string GetXsdType()
		{
			return SoapNcName.XsdType;
		}

		// Token: 0x06004521 RID: 17697 RVA: 0x000EB6A0 File Offset: 0x000EA6A0
		public SoapNcName()
		{
		}

		// Token: 0x06004522 RID: 17698 RVA: 0x000EB6A8 File Offset: 0x000EA6A8
		public SoapNcName(string value)
		{
			this._value = value;
		}

		// Token: 0x17000C27 RID: 3111
		// (get) Token: 0x06004523 RID: 17699 RVA: 0x000EB6B7 File Offset: 0x000EA6B7
		// (set) Token: 0x06004524 RID: 17700 RVA: 0x000EB6BF File Offset: 0x000EA6BF
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

		// Token: 0x06004525 RID: 17701 RVA: 0x000EB6C8 File Offset: 0x000EA6C8
		public override string ToString()
		{
			return SoapType.Escape(this._value);
		}

		// Token: 0x06004526 RID: 17702 RVA: 0x000EB6D5 File Offset: 0x000EA6D5
		public static SoapNcName Parse(string value)
		{
			return new SoapNcName(value);
		}

		// Token: 0x04002267 RID: 8807
		private string _value;
	}
}
