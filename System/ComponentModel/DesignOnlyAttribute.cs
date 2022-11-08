using System;

namespace System.ComponentModel
{
	// Token: 0x020000D6 RID: 214
	[AttributeUsage(AttributeTargets.All)]
	public sealed class DesignOnlyAttribute : Attribute
	{
		// Token: 0x06000739 RID: 1849 RVA: 0x0001A97B File Offset: 0x0001997B
		public DesignOnlyAttribute(bool isDesignOnly)
		{
			this.isDesignOnly = isDesignOnly;
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x0600073A RID: 1850 RVA: 0x0001A98A File Offset: 0x0001998A
		public bool IsDesignOnly
		{
			get
			{
				return this.isDesignOnly;
			}
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x0001A992 File Offset: 0x00019992
		public override bool IsDefaultAttribute()
		{
			return this.IsDesignOnly == DesignOnlyAttribute.Default.IsDesignOnly;
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x0001A9A8 File Offset: 0x000199A8
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DesignOnlyAttribute designOnlyAttribute = obj as DesignOnlyAttribute;
			return designOnlyAttribute != null && designOnlyAttribute.isDesignOnly == this.isDesignOnly;
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x0001A9D5 File Offset: 0x000199D5
		public override int GetHashCode()
		{
			return this.isDesignOnly.GetHashCode();
		}

		// Token: 0x04000953 RID: 2387
		private bool isDesignOnly;

		// Token: 0x04000954 RID: 2388
		public static readonly DesignOnlyAttribute Yes = new DesignOnlyAttribute(true);

		// Token: 0x04000955 RID: 2389
		public static readonly DesignOnlyAttribute No = new DesignOnlyAttribute(false);

		// Token: 0x04000956 RID: 2390
		public static readonly DesignOnlyAttribute Default = DesignOnlyAttribute.No;
	}
}
