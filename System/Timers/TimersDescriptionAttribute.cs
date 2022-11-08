using System;
using System.ComponentModel;

namespace System.Timers
{
	// Token: 0x02000736 RID: 1846
	[AttributeUsage(AttributeTargets.All)]
	public class TimersDescriptionAttribute : DescriptionAttribute
	{
		// Token: 0x06003849 RID: 14409 RVA: 0x000ED7B0 File Offset: 0x000EC7B0
		public TimersDescriptionAttribute(string description) : base(description)
		{
		}

		// Token: 0x17000D0F RID: 3343
		// (get) Token: 0x0600384A RID: 14410 RVA: 0x000ED7B9 File Offset: 0x000EC7B9
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

		// Token: 0x04003232 RID: 12850
		private bool replaced;
	}
}
