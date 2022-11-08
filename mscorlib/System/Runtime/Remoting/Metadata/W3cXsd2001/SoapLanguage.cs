using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x0200078F RID: 1935
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapLanguage : ISoapXsd
	{
		// Token: 0x17000C1A RID: 3098
		// (get) Token: 0x060044EF RID: 17647 RVA: 0x000EB4D0 File Offset: 0x000EA4D0
		public static string XsdType
		{
			get
			{
				return "language";
			}
		}

		// Token: 0x060044F0 RID: 17648 RVA: 0x000EB4D7 File Offset: 0x000EA4D7
		public string GetXsdType()
		{
			return SoapLanguage.XsdType;
		}

		// Token: 0x060044F1 RID: 17649 RVA: 0x000EB4DE File Offset: 0x000EA4DE
		public SoapLanguage()
		{
		}

		// Token: 0x060044F2 RID: 17650 RVA: 0x000EB4E6 File Offset: 0x000EA4E6
		public SoapLanguage(string value)
		{
			this._value = value;
		}

		// Token: 0x17000C1B RID: 3099
		// (get) Token: 0x060044F3 RID: 17651 RVA: 0x000EB4F5 File Offset: 0x000EA4F5
		// (set) Token: 0x060044F4 RID: 17652 RVA: 0x000EB4FD File Offset: 0x000EA4FD
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

		// Token: 0x060044F5 RID: 17653 RVA: 0x000EB506 File Offset: 0x000EA506
		public override string ToString()
		{
			return SoapType.Escape(this._value);
		}

		// Token: 0x060044F6 RID: 17654 RVA: 0x000EB513 File Offset: 0x000EA513
		public static SoapLanguage Parse(string value)
		{
			return new SoapLanguage(value);
		}

		// Token: 0x04002261 RID: 8801
		private string _value;
	}
}
