using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x02000798 RID: 1944
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapEntity : ISoapXsd
	{
		// Token: 0x17000C2C RID: 3116
		// (get) Token: 0x06004537 RID: 17719 RVA: 0x000EB773 File Offset: 0x000EA773
		public static string XsdType
		{
			get
			{
				return "ENTITY";
			}
		}

		// Token: 0x06004538 RID: 17720 RVA: 0x000EB77A File Offset: 0x000EA77A
		public string GetXsdType()
		{
			return SoapEntity.XsdType;
		}

		// Token: 0x06004539 RID: 17721 RVA: 0x000EB781 File Offset: 0x000EA781
		public SoapEntity()
		{
		}

		// Token: 0x0600453A RID: 17722 RVA: 0x000EB789 File Offset: 0x000EA789
		public SoapEntity(string value)
		{
			this._value = value;
		}

		// Token: 0x17000C2D RID: 3117
		// (get) Token: 0x0600453B RID: 17723 RVA: 0x000EB798 File Offset: 0x000EA798
		// (set) Token: 0x0600453C RID: 17724 RVA: 0x000EB7A0 File Offset: 0x000EA7A0
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

		// Token: 0x0600453D RID: 17725 RVA: 0x000EB7A9 File Offset: 0x000EA7A9
		public override string ToString()
		{
			return SoapType.Escape(this._value);
		}

		// Token: 0x0600453E RID: 17726 RVA: 0x000EB7B6 File Offset: 0x000EA7B6
		public static SoapEntity Parse(string value)
		{
			return new SoapEntity(value);
		}

		// Token: 0x0400226A RID: 8810
		private string _value;
	}
}
