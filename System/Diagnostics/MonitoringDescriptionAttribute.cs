using System;
using System.ComponentModel;

namespace System.Diagnostics
{
	// Token: 0x0200075F RID: 1887
	[AttributeUsage(AttributeTargets.All)]
	public class MonitoringDescriptionAttribute : DescriptionAttribute
	{
		// Token: 0x060039F2 RID: 14834 RVA: 0x000F52F7 File Offset: 0x000F42F7
		public MonitoringDescriptionAttribute(string description) : base(description)
		{
		}

		// Token: 0x17000D88 RID: 3464
		// (get) Token: 0x060039F3 RID: 14835 RVA: 0x000F5300 File Offset: 0x000F4300
		public override string Description
		{
			get
			{
				if (!this.replaced)
				{
					this.replaced = true;
					base.DescriptionValue = SR.GetString(base.Description);
				}
				return base.Description;
			}
		}

		// Token: 0x040032E1 RID: 13025
		private bool replaced;
	}
}
