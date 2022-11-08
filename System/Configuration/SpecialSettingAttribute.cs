using System;

namespace System.Configuration
{
	// Token: 0x0200070C RID: 1804
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
	public sealed class SpecialSettingAttribute : Attribute
	{
		// Token: 0x06003745 RID: 14149 RVA: 0x000EADC2 File Offset: 0x000E9DC2
		public SpecialSettingAttribute(SpecialSetting specialSetting)
		{
			this._specialSetting = specialSetting;
		}

		// Token: 0x17000CCD RID: 3277
		// (get) Token: 0x06003746 RID: 14150 RVA: 0x000EADD1 File Offset: 0x000E9DD1
		public SpecialSetting SpecialSetting
		{
			get
			{
				return this._specialSetting;
			}
		}

		// Token: 0x040031B3 RID: 12723
		private readonly SpecialSetting _specialSetting;
	}
}
