using System;
using System.ComponentModel;

namespace System.IO
{
	// Token: 0x0200072C RID: 1836
	[AttributeUsage(AttributeTargets.All)]
	public class IODescriptionAttribute : DescriptionAttribute
	{
		// Token: 0x06003817 RID: 14359 RVA: 0x000ECF0E File Offset: 0x000EBF0E
		public IODescriptionAttribute(string description) : base(description)
		{
		}

		// Token: 0x17000D02 RID: 3330
		// (get) Token: 0x06003818 RID: 14360 RVA: 0x000ECF17 File Offset: 0x000EBF17
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

		// Token: 0x04003212 RID: 12818
		private bool replaced;
	}
}
