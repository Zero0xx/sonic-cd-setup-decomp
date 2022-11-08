using System;

namespace System.Configuration
{
	// Token: 0x02000704 RID: 1796
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class DefaultSettingValueAttribute : Attribute
	{
		// Token: 0x06003735 RID: 14133 RVA: 0x000EAD02 File Offset: 0x000E9D02
		public DefaultSettingValueAttribute(string value)
		{
			this._value = value;
		}

		// Token: 0x17000CC6 RID: 3270
		// (get) Token: 0x06003736 RID: 14134 RVA: 0x000EAD11 File Offset: 0x000E9D11
		public string Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x040031AC RID: 12716
		private readonly string _value;
	}
}
